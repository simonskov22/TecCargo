<?php

require_once 'inc/form.php';

class user extends pageSettings{
    
    
    /**
     * 
     * @global databaseSetup $database
     */
    public function __construct() {
        global $_URL,$_SCRIPTFILES,$_STYLEFILES;
        
        $_STYLEFILES[] = $_URL."style/admin_user.css";
        $_STYLEFILES[] = $_URL."style/form.css";
        $_SCRIPTFILES[] = $_URL."script/form.js";
        
        $this->memberOnly = false;
        
    }
    
    public function Login($param = null) {
        global $database, $_URL;
        
        $this->title = "Log Ind";
        
        if(IsLogin()){
            header("location: {$_URL}");
            die();
        }
        else{
            
            $this->RemoveLoginCokie();
        }
        
        if($_POST['action'] == "login"){
            
            $username = $database->SQLReadyString($_POST['user']);
            $password = $database->SQLReadyString($_POST['pass']);
            $rememberMe = $_POST['remeberMe'];
            
            $result = $database->GetRow("SELECT `password`,`pessword` FROM `{$database->prefix}Users` WHERE `username` = '$username';");
            
            if($result){
                
                $encryptPass = EncryptPass($password, $result->pessword);
                
                if($encryptPass == $result->password){
                    
                    $rememberMeTime = $rememberMe ? (time()+60*60*24*30) : false ;
            
                    setcookie('AUID', $username, $rememberMeTime, "/", ".".$_SERVER['SERVER_NAME']);
                    setcookie('AUPA', $encryptPass, $rememberMeTime, "/", ".".$_SERVER['SERVER_NAME']);
                }
            }
            
        }
        session_start();
        $_SESSION['user'] = "login";
        header("location: {$_URL}");
        die();
    }
    
    public function Logout($param = null) {
        global $_URL;
        
        $this->RemoveLoginCokie();
        
        session_start();
        $_SESSION['user'] = "logout";
        header("location: {$_URL}");
        die();
    }
    
    private function RemoveLoginCokie() {
        
        setcookie('AUID', '', time()-300, "/", ".".$_SERVER['SERVER_NAME']);
        setcookie('AUPA', '', time()-300, "/", ".".$_SERVER['SERVER_NAME']);
    }


    public function MyProfile($param = null) {
        global $database;
        
        $this->memberOnly = true;
        $this->title = "Min Profil";
        
        if(isset($_POST['action']) && $_POST['action'] == "update"){
            $this->Post_Myprofile();
        }
        
        $userId = GetCurrentUserId();
        $userRow = $database->GetRow("SELECT `username`,`name`,`lastname`,`email` FROM `{$database->prefix}Users` WHERE `userId` = $userId;")
        
        ?>
<div style="padding-top: 80px;">
    <form id="editForm" class="tecForm adminUser" method="post">

        <input name="action" type="hidden" value='update'/>
        <div class="formheader"><h2>Min Profil</h2></div>
        <table  class="formcontent">
            <?php
            $form = new form();

            $formValues = array(
                array("focus" => "#row1", "bothValue" => $userRow->username, "name" => "user", "labeltext" => "Brugernavn:", "min"=>3, "required"=> 'true', "disabled" => true),
                array("focus" => "#row2", "name" => "pass", "type" => "password", "labeltext" => "Kodeord:", "min"=>6),
                array("focus" => "#row3", "name" => "pess", "type" => "password", "labeltext" => "Kodeord igen:", "min"=>6),
                array(),
                array("focus" => "#row4", "bothValue" => $userRow->name, "name" => "name", "labeltext" => "Navn:", "required"=> 'true'),
                array("focus" => "#row5", "bothValue" => $userRow->lastname, "name" => "lastname", "labeltext" => "Efternavn:", "required"=> 'true'),
                array("focus" => "#row6", "bothValue" => $userRow->email, "name" => "email", "labeltext" => "Email:")
            );

                foreach ($formValues as $value) {
                    if(count($value) == 0){
                        echo "<tr class='tablespace'><td colspan='2'></td></tr>";
                    }
                    else{


                        echo "<tr id='" .substr($value['focus'], 1)."' class='userCrow1 userCrowRemove'>"
                                . "<td>{$value['labeltext']}</td>"
                                . "<td style='text-align: right;'>{$form->AddInputBox($value)}</td>"
                            . "</tr>";
                    }
                }

            ?>
        </table>

        <div class="formbottom">
            <input type="reset" class="defaultButton" onclick="return false;" style="margin-left: 10px;">
            <input type="submit" class="defaultButton" value="Gem" onclick="return false;" style="float: right; margin-right: 8px;">
        </div>
    </form>
</div>

<?php
    }
    
    
    private function Post_Myprofile() {
        global $database, $message;
        
        $allowInsert = true;
        $errorContent = "";
        $userId = GetCurrentUserId();
        $password = $_POST['pass'];
        $pessword = $_POST['pess'];
        $name = $_POST['name'];
        $lastname = $_POST['lastname'];
        $email = $_POST['email'];
        $salt = createSalt();
        $encryptPass = EncryptPass($password, $salt);
        
        
        
        $requirement = array(
            strlen($password) != 0 && strlen($password) < 6,
            strlen($password) != 0 && $password != $pessword,
            strlen($name) < 1,
            strlen($lastname) < 1,
            !$database->ValueIsSame($database->prefix."Users", $email, 'email', array("userId" => $userId)) 
            && !$database->ValueIsUnique($database->prefix."Users", $email, 'email') && strlen($email) != 0
        );
        
        $errorMessages = array(
            "Kodeordet skal indeholde minimum 6 tegn.",
            "Kodeordene skal være ens.",
            "Navn kan ikke være tom.",
            "Efternavn kan ikke være tom.",
            "Email bilver brugt af en anden."
        );
        
        
        for($i = 0; $i < count($requirement); $i++){
            
            if($requirement[$i]){
                $errorContent .= "<li>$errorMessages[$i]</li>";
                $allowInsert = false;
            }
        }
        if(!$database->ValueIsUnique($database->prefix."Users", $email, 'email')){}
        
        if($allowInsert){
            
            
            $updateArray = array(
                "name"      => $name,
                "lastname"  => $lastname,
                "email"     => $email
            );
            
            if(strlen($password) != 0){
                
                $updateArray['password'] = $encryptPass;
                $updateArray['pessword'] = $salt;
            }
            
            
            $database->Update($database->prefix."Users", 
                $updateArray,
                array("userId" => $userId));
            
            
            $message->AddMessage(msg_succes, "Bruger opdateret", "");
        }
        else{
            $messageContent = "<ul>$errorContent</ul>";
            $message->AddMessage(msg_error, "Bruger ikke oprettet", $messageContent);
        }
    }
    
}