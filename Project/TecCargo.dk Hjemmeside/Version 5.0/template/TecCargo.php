<?php

require_once 'inc/navigation.php';

class template {
    
    public $title;
    public $content;
    public $isAdminPage;
    


    public function __construct() {
        global $_SCRIPTFILES, $_STYLEFILES, $_URL, $_URLPATH,$navigationClass;
        
        
       //$_SCRIPTFILES[] = $_URL."script/default.js";
       
       $_SCRIPTFILES[] = $_URL."script/showPage.js";
       $_STYLEFILES[] = $_URL."style/showPage.css";
       
       $_STYLEFILES[] = $_URL."style/TecCargo.css"; 
       
       
       $_STYLEFILES[] = $_URL."style/navigation.css"; 
       $_SCRIPTFILES[] = $_URL."script/navigation.js"; 
       
       
       
//        $_STYLEFILES[] = $_URL."style/style.css";
//        $_STYLEFILES[] = $_URL."style/newadminStyle.css";
//        $_STYLEFILES[] = $_URL."style/css/dcmegamenu.css";
//        $_STYLEFILES[] = $_URL."style/css/skins/white.css";
        
//        $_STYLEFILES[] = $_URL."plugins/bootstrap/css/bootstrap.min.css";
//        $_STYLEFILES[] = $_URL."plugins/bootstrap/css/bootstrap-theme.min.css";
//        $_SCRIPTFILES[] = $_URL."plugins/bootstrap/3.3.5/js/bootstrap.min.js";
        
    }
    
    public function ShowTemplate() {
        global $_VERSION,$message, $_URL,$database;
        
        $navigation = new navigation();
        
        if($this->isAdminPage && IsAdmin()){
            $navigation->MakeAdminNavigation();
        }
        else if(IsLogin()){
            $navigation->MakeMemberNavigation();
        }
        else{
            $navigation->MakeGuestNavigation();
        }
        session_start();
        if(isset($_SESSION['user'])){
            if($_SESSION['user'] == "login"){
                if(IsLogin()){
                    $message->AddMessage(msg_succes,"Nu logget ind","Velkommen tilbage.");
                }
                else{
                    
                    $message->AddMessage(msg_error,"Ikke logget ind","Kunne ikke logge dig ind. prøv igen.");
                }
            }
            else if($_SESSION['user'] == "logout"){
                if(!IsLogin()){
                    $message->AddMessage(msg_succes,"Nu logget ud","Du er nu logget ud.");
                }
                else{
                    
                    $message->AddMessage(msg_error,"Ikke logget ud","Kunne ikke logge dig ud. prøv igen.");
                }
            }
        }
        session_unset();
        session_destroy();
        
        
        ?>
        <!DOCTYPE html>
        <html>
            <head>
                <title>TECcargo.dk - <?php echo $this->title; ?></title>
                <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
                <link rel="shortcut icon" href="<?php echo $_URL;?>images/icon.ico">

                <?php $this->executeScript(); ?>
                
                <?php $this->executeStyle(); ?>
            </head>
            <body class="tecCargo">
                <div class="top outerBox">
                    <div class="innerBox">
                        <div class="contentBottom">
                            <img class="logo" src="<?php echo $_URL;?>images/logo2.png">
                            <img class='text' src='<?php echo $_URL;?>images/logo3.png'>
                        </div>
                        
                        <div class="memberBox">
                            <div class="formHeader">
                                <?php 
                                 if(IsLogin()){
                                     $userId = GetCurrentUserId();
                                     $userRow = $database->GetRow("SELECT `name`, `lastname` FROM `{$database->prefix}Users` WHERE `userId` = $userId;");
                                     
                                    echo "<h4>Logget ind som <span class='uppercase'>$userRow->name</span> <span class='uppercase'>$userRow->lastname</span></h4>";
                                 }
                                 else{
                                    echo "<h4>Log ind</h4>";
                                 }
                                ?>
                            </div>
                            <div class="formContent">
                                
                                <?php 
                                if(IsLogin()){
                                ?>
                                <a href="<?php echo $_URL."user/Myprofile/"; ?>" class="blue">Min profil</a>
                                <a href="<?php echo $_URL."user/logout/"; ?>" class="red">Log ud</a>
                                <?php
                                }
                                else{
                                ?>
                                    <form method="post" action="<?php echo $_URL."user/login/"; ?>">
                                        <input type="hidden" name="action" value="login"/>
                                        <input name="user" type="text"/>
                                        <input name="pass" type="password"/>
                                        <input name="remeberMe" id='remeber' type="checkbox"/>
                                        <label for="remeber" style="font-size: 14px;">Husk mig</label>
                                        <input type="submit" class="defaultButton" value="Log Ind"/>
                                    </form>
                                <?php
                                }
                                ?>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Menu -->
                
                <div class="navigation outerBox">
                    <div class="innerBox">
                        
                        <?php $navigation->excuteNavigation(); ?>
                        
                    </div>
                
                </div>

                <!-- Sidens indhold-->
                <div class="outerBox">
                    
                    <div class="content innerBox">
                        <div class="contentInner">
                        <?php $this->executeMessage(msg_warning, ""); ?>
                        <?php echo $this->content; ?>
                        </div>
                    </div>
                </div>

                <div class="bottom outerBox">
                    <table class="tableFooter innerBox">
                        <tr>
                            <td class="footer1">Version: <?php echo $_VERSION; ?></td>
                            <td class="footer2">Copyright &COPY; 2014 - <?php echo date("Y"); ?></td>
                            <td class="footer3">Kodet af: Simon Skov</td>
                        </tr>
                    </table>
                </div> 
            </body>
        </html>
        <?php
    }
    
    private function executeScript() {
        global $_SCRIPTFILES;
        
        foreach ($_SCRIPTFILES as $value) {
            
            echo "<script type='text/javascript' src='$value'></script>";
        }
        
    }
    
    private function executeStyle() {
        global $_STYLEFILES;
        
        foreach ($_STYLEFILES as $value) {

            echo "<link href='$value' rel='stylesheet' type='text/css' />";
        }
        
    }
    
    /**
     * 
     * @global message $message
     * @param type $type
     * @param type $class
     */
    private function executeMessage() {
        global $message,$_URL;
        
        $messageError = $message->GetMessage(msg_error);
        $messageSucces = $message->GetMessage(msg_succes);
        $messageWarning = $message->GetMessage(msg_warning);
        
        
        $messageArray = array(
            array($messageError, ""),
            array($messageSucces, ""),
            array($messageWarning, "")
        );
        
        
        ?>
        <style>
        .defaultMessageError{

            background-color: #F14545;
        }
        .defaultMessageSuccess{

            background-color: #45F146;
        }
        .defaultMessageWarning{

            background-color: #F1DF45;
        }
        .defaultMessage{
            position: relative;
            border: 2px groove #E8E5E5;
            border-radius: 5px;
            margin: 5px 0px 15px 0px;
            min-height: 50px;
            padding: 15px 10px;
        }
        .defaultMessageTitle{
            font-weight: bold;
            display: block;
            font-size: 18px;
        }
        .defaultMessageContent{
            display: block;
            text-indent: 15px;
        }
        .defaultMessageImg{ 
            position: absolute;
            height: 30px;
            width: 30px;
            right: -15px;
            top: -15px;
        }
    
    
        </style>
        
        <?php
        
        for($i = 0; $i < count($messageArray); $i++) {
            
            
            if($i == msg_error){
                $msgColorClass = "defaultMessageError";
            }
            else if($i == msg_succes){
                $msgColorClass = "defaultMessageSuccess";
            }
            else{
                $msgColorClass = "defaultMessageWarning";
            }
                
            
            for($a = 0; $a < count($messageArray[$i][0]); $a ++) {
                
                
                echo "<div class='defaultMessage $msgColorClass' id='defaultMessage{$i}_{$a}'>"
                . "<img class='defaultMessageImg' onclick='tecCloseBox(\"#defaultMessage{$i}_{$a}\");' src='{$_URL}images/icons/close.png' />"
                    . "<span class='defaultMessageTitle'>{$messageArray[$i][0][$a]['title']}</span>"
                        . "<span class='defaultMessageContent'>{$messageArray[$i][0][$a]['message']}</span>"
                . "</div>";
            }
        }
    }
}
