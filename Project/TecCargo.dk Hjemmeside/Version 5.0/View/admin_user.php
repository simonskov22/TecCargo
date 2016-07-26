<?php

require_once 'inc/form.php';

class admin_user extends pageSettings{
    
    public function __construct() {
        $this->AdminOnly = true;
        
        global $_URL, $_SCRIPTFILES, $_STYLEFILES;
        
        
        $_SCRIPTFILES[] = $_URL."script/admin_user.js";
        $_SCRIPTFILES[] = $_URL."script/form.js";
        
        $_STYLEFILES[] = $_URL."style/form.css";
        $_STYLEFILES[] = $_URL."style/admin_user.css";
        
    }
    
    public function jquery($param = null) {
        
        $this->loadTemplate = false;
        
        if(is_array($param)){
            
            if($param[0] == "edit"){
                $this->getUserData($param[1]);
            }
        }
    }
    
    /**
     * 
     * @global databaseSetup $database
     * @global message $message
     */
    private function Post_Create() {
        global $database, $message;
        
        $allowInsert = true;
        $errorContent = "";
        $username = $_POST['user'];
        $password = $_POST['pass'];
        $pessword = $_POST['pess'];
        $name = $_POST['name'];
        $lastname = $_POST['lastname'];
        $email = $_POST['email'];
        $isAdmin = intval($_POST['rank']);
        $salt = createSalt();
        $encryptPass = EncryptPass($password, $salt);
        
        $requirement = array(
            strlen($username) < 3,
            !$database->ValueIsUnique($database->prefix."Users", $username, 'username'),
            strlen($password) < 6,
            $password != $pessword,
            strlen($name) < 1,
            strlen($lastname) < 1,
            strlen($email) != 0 && !$database->ValueIsUnique($database->prefix."Users", $email, 'email') 
        );
        
        $errorMessages = array(
            "Brugernavn skal indeholde minimum 3 tegn.",
            "Brugernavnet bliver brugt af en anden.",
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
            
            $database->Insert($database->prefix."Users", array(
                "username"  => $username,
                "password"  => $encryptPass,
                "pessword"  => $salt,
                "name"      => $name,
                "lastname"  => $lastname,
                "email"     => $email,
                "admin"     => $isAdmin
            ));
            
            
            $message->AddMessage(msg_succes, "Bruger oprettet", "");
        }
        else{
            $messageContent = "<ul>$errorContent</ul>";
            $message->AddMessage(msg_error, "Bruger ikke oprettet", $messageContent);
        }
    }
    
    /**
     * 
     * @global databaseSetup $database
     * @global message $message
     */
    private function Post_Edit() {
        global $database, $message;
        
        $allowInsert = true;
        $errorContent = "";
        $userId = intval($_POST['userId']);
        $username = $_POST['user'];
        $password = $_POST['pass'];
        $pessword = $_POST['pess'];
        $name = $_POST['name'];
        $lastname = $_POST['lastname'];
        $email = $_POST['email'];
        $isAdmin = intval($_POST['rank']);
        $salt = createSalt();
        $encryptPass = EncryptPass($password, $salt);
        
        
        
        $requirement = array(
            strlen($username) < 3,
            !$database->ValueIsUnique($database->prefix."Users", $username, 'username')
            && !$database->ValueIsSame($database->prefix."Users", $username, 'username', array("userId" => $userId)),
            strlen($password) != 0 && strlen($password) < 6,
            strlen($password) != 0 && $password != $pessword,
            strlen($name) < 1,
            strlen($lastname) < 1,
            !$database->ValueIsSame($database->prefix."Users", $email, 'email', array("userId" => $userId)) 
            && !$database->ValueIsUnique($database->prefix."Users", $email, 'email') && strlen($email) != 0
        );
        
        $errorMessages = array(
            "Brugernavn skal indeholde minimum 3 tegn.",
            "Brugernavnet bliver brugt af en anden.",
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
                "username"  => $username,
                "name"      => $name,
                "lastname"  => $lastname,
                "email"     => $email,
                "admin"     => $isAdmin
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
    
    
    /**
     * 
     * @global databaseSetup $database
     * @global message $message
     */
    private function Post_Delete() {
        global $database, $message;
        
        $adminCountDb = $database->GetRow("SELECT COUNT(*) FROM `{$database->prefix}Users` WHERE admin = 1;", array_I);
        $adminDeleteCount = 0;
        $errorMsg = "<ul>";
        $allowDelete = true;
        
        
        foreach ($_POST['deleteIds'] as $userId) {
            
            $userRow = $database->GetRow("SELECT `username`, `admin` FROM `{$database->prefix}Users` WHERE `userId` = ".  intval($userId). ";");
            
            if($userRow->admin == 1){
                $adminDeleteCount++;
            }
            
            $userDelete[] = array("userId" => intval($userId), "username" => $userRow->username);
            
        }
        
        if($adminCountDb[0] == $adminDeleteCount){
            
            $allowDelete = false;
            $errorMsg .= "<li>Der skal minimum være en admin tilbage.</li>";
        }
        
        if($allowDelete){
            
            $successMsg = "<ul>";
            
            foreach ($userDelete as $user) {
                
                $database->Delete($database->prefix."Users", array("userId" => $user['userId']));
                
                $successMsg .= "<li>{$user['username']}: er blivet slettet.</li>";
            }
            
            $successMsg .= "</ul>";
            
            $message->AddMessage(msg_succes, "Bruger Slettet", $successMsg);
        }
        else{
            
            $errorMsg .= "</ul>";
            $message->AddMessage(msg_error, "Bruger Ikke Slettet", $errorMsg);
        }
    }
    
    public function create($param = null) {
        global $_URL;
        
        $form = new form();
        
        $this->title = "Admin Opret Bruger";
        
        if(isset($_POST['action']) &&  $_POST['action'] == "create"){
            
            $this->Post_Create();
        }
        
        ?>
        <form class="tecForm adminUser" method="post" action="<?php echo $_URL."admin_user/create/"; ?>">
            <input name="action" type="hidden" value='create'/>
            <div class="formheader"><h2>Opret Bruger</h2></div>
            <table class="formcontent">
                <?php
                $formValues = array(
                    array("focus" => "#row1", "name" => "user", "labeltext" => "Brugernavn:", "min"=>3, "required"=> 'true'),
                    array("focus" => "#row2", "name" => "pass", "type" => "password", "labeltext" => "Kodeord:", "min"=>6, "required"=> 'true'),
                    array("focus" => "#row3", "name" => "pess", "type" => "password", "labeltext" => "Kodeord igen:", "min"=>6, "required"=> 'true', "compare" => "pass"),
                    array(),
                    array("focus" => "#row4", "name" => "name", "labeltext" => "Navn:", "required"=> 'true'),
                    array("focus" => "#row5", "name" => "lastname", "labeltext" => "Efternavn:", "required"=> 'true'),
                    array("focus" => "#row6", "name" => "email", "labeltext" => "Email:")
                );

                    foreach ($formValues as $value) {
                        if(count($value) == 0){
                            echo "<tr class='tablespace'><td colspan='2'></td></tr>";
                        }
                        else{
                            
                            echo "<tr id='".substr($value['focus'], 1)."' class='userCrow1 userCrowRemove'>"
                                    . "<td>{$value['labeltext']}</td>"
                                    . "<td style='text-align: right;'>{$form->AddInputBox($value)}</td>"
                                . "</tr>";
                        }
                    }

                ?>
                <tr class="tablespace">
                    <td rowspan="2">Brugerniveau</td>
                    <td><input id="radioUser" name="rank" type="radio" value="User" checked="true">Bruger</td>
                </tr>
                <tr>
                    <td><input id="radioAdmin" name="rank" type="radio" value="Admin">Admin</td>
                </tr>
            </table>

            <div class="formbottom">
                <input type="reset" class="defaultButton" onclick="return false;" style="margin-left: 10px;">
                <input type="submit" class="defaultButton" value="Opret" onclick="return false;" style="float: right; margin-right: 8px;">
            </div>
        </form>
        <?php
    }
    
    /**
     * 
     * @global type $_URL
     * @global databaseSetup $database
     * @param type $param
     */
    public function edit($param = null) {
        global $_URL, $database;
        
        $this->title = "Admin Rediger Bruger";
        
        if(isset($_POST['action']) &&  $_POST['action'] == "edit"){
            
            $this->Post_Edit();
        }
        
        $userIds = $database->GetResults("SELECT `userId`,`name`,`lastname` FROM `{$database->prefix}Users`");
        
        ?>

        <div> 
            <img src="<?php echo $_URL."images/admin/left_circular.png";?>" style="position: absolute; cursor: pointer; height: 50px;" onclick="previousPage();">
            
            <div class="adminDropDown">
                <ul class="dropdownContent">
                    <li class="dropdownHeader">Vælg bruger <span class="arrow"></span></li>
                    <li class="dropdownOption">
                        <ul>
                            <?php 
                                foreach ($userIds as $user) {
                                    $link = "{$_URL}admin_user/jquery/edit/$user->userId/";

                                    echo "<li onclick='GetData(\"$link\")'>$user->name $user->lastname</li>";
                                }
                            ?>
                        </ul>
                    </li>
                </ul>
            </div>
            
            
            <div style="padding-top: 80px;">
                <form id="editForm" class="tecForm adminUser" method="post" action="<?php echo $_URL."admin_user/edit/"; ?>">
                    
                    <input name="action" type="hidden" value='edit'/>
                    <input name="userId" type="hidden">
                    <div class="formheader"><h2>Rediger Bruger</h2></div>
                    <table  class="formcontent">
                        <?php
                        $form = new form();
                        
                        $formValues = array(
                            array("focus" => "#row1", "name" => "user", "labeltext" => "Brugernavn:", "min"=>3, "required"=> 'true'),
                            array("focus" => "#row2", "name" => "pass", "type" => "password", "labeltext" => "Kodeord:", "min"=>6),
                            array("focus" => "#row3", "name" => "pess", "type" => "password", "labeltext" => "Kodeord igen:", "min"=>6),
                            array(),
                            array("focus" => "#row4", "name" => "name", "labeltext" => "Navn:", "required"=> 'true'),
                            array("focus" => "#row5", "name" => "lastname", "labeltext" => "Efternavn:", "required"=> 'true'),
                            array("focus" => "#row6", "name" => "email", "labeltext" => "Email:")
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
                        <tr class="tablespace">
                            <td rowspan="2">Brugerniveau</td>
                            <td><input id="radioUser" name="rank" type="radio" value="User" checked="true">Bruger</td>
                        </tr>
                        <tr>
                            <td><input id="radioAdmin" name="rank" type="radio" value="Admin">Admin</td>
                        </tr>
                    </table>

                    <div class="formbottom">
                        <input type="reset" class="defaultButton" onclick="return false;" style="margin-left: 10px;">
                        <input type="submit" class="defaultButton" value="Gem" onclick="return false;" style="float: right; margin-right: 8px;">
                    </div>
                </form>
            </div>
        </div>    
            
        <?php
    }
    private function getUserData($userId){
        global $database;
        
        if(!ctype_digit($userId)){
            return false;
        }
        
        $userId = intval($userId);
        
        $userInfo = $database->GetRow("SELECT `username`,`name`,`lastname`,`email`,`admin` FROM `{$database->prefix}Users` WHERE `userId` = $userId;");
        
        echo json_encode(array("userId" => $userId, "user" => $userInfo->username, "name" => $userInfo->name,
            "lastname" => $userInfo->lastname, "email" => $userInfo->email, "admin" => $userInfo->admin));
    }
    
    public function delete($param = null) {
        
        global $_URL,$database;
        
        $this->title = "Admin Slet Bruger";
        
        if(isset($_POST['action']) &&  $_POST['action'] == "delete"){
            
            $this->Post_Delete();
        }
        
        $usersInfo = $database->GetResults("SELECT `userId`, `username`, `name`, `lastname`, `admin` FROM `{$database->prefix}Users`;");
        ?>

        <div>
            <img src="<?php echo $_URL."images/admin/left_circular.png"; ?>" style="position: absolute; cursor: pointer; height: 50px;" onclick="previousPage();">

            <form class="adminUser tecFormPicker" bind-multiSelect="true" method="post" action="<?php echo $_URL."admin_user/delete/"; ?>" style="width: 800px;">
                
                <input type="hidden" name="action" value="delete" />
                <div class="formheader"><h2>Slet Brugere</h2></div>
                <table class="formcontent">
                    <tr>
                        <th>Brugernavn</th>
                        <th>Navn</th>
                        <th>Brugerniveau</th>
                    </tr>
                    <?php 
                    
                        foreach ($usersInfo as $user) {

                            $isAdmin = $user->admin == 1 ? "Admin" : "Bruger";

                            echo "<tr bind-inputname='deleteIds[]' bind-inputval='$user->userId' class='tecFormClickOn'>"
                            . "<td>$user->username</td>"
                            . "<td>$user->name $user->lastname</td>"
                            . "<td>$isAdmin</td>"
                            . "</tr>";
                        }
                    
                    ?>
                </table>

                <div class="formbottom">
                    <input type="reset" class="defaultButton" style="margin-left: 10px;" onclick="return false;" />
                    <input type="submit" class="defaultButton" value="Slet" style="float: right; margin-right: 8px;" />
                </div>
            </form>
        </div>
        <?php
    }
}
