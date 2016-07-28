<?php

class template {
    
    public $title;
    public $content;
    public $isAdminPage;


    public function __construct() {
        global $_SCRIPTFILES, $_STYLEFILES, $_URL;
        

        $_SCRIPTFILES[] = $_URL."script/default.js";

        $_SCRIPTFILES[] = $_URL."script/showPage.js";
        $_STYLEFILES[] = $_URL."style/showPage.css";
        
        $_STYLEFILES[] = $_URL."style/style.css";
        $_STYLEFILES[] = $_URL."style/newadminStyle.css";
        $_STYLEFILES[] = $_URL."style/css/dcmegamenu.css";
        $_STYLEFILES[] = $_URL."style/css/skins/white.css";
        
//        $_STYLEFILES[] = $_URL."plugins/bootstrap/css/bootstrap.min.css";
//        $_STYLEFILES[] = $_URL."plugins/bootstrap/css/bootstrap-theme.min.css";
//        $_SCRIPTFILES[] = $_URL."plugins/bootstrap/3.3.5/js/bootstrap.min.js";
        
    }
    
    public function ShowTemplate() {
        global $_VERSION,$navBarUser,$navBarAdmin, $_URL;
        
        ?>
        <!DOCTYPE html>
        <html>
            <head>
                <title>TECcargo.dk - <?php echo $this->title; ?></title>
                <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
                <link rel="shortcut icon" href="<?php echo $_URL;?>images/icon.ico">

                <?php $this->executeScript(); ?>

                <script type="text/javascript">
                    $(document).ready(function($){
                        $('#mega-menu-9').dcMegaMenu({
                            rowItems: '5',
                            speed: 'fast',
                            effect: 'fade'
                        });
                        $("#followNav").sticky({topSpacing:0});
                    });
                </script>
                
                <?php $this->executeStyle(); ?>

                <style>
                    .sitev2
                    {
                        padding: 20px;
                    }
                    .kurerButton
                    {
                        background: #254078;
                        border: 2px outset #4268B8;
                    }
                    .kurerButton:hover
                    {
                        background: #31456F;
                        border: 2px outset #4C5A79;
                    }
                    .kurerTableView
                    {
                        margin-left: auto;
                        margin-right: auto;
                        border-collapse: collapse;
                    }
                    .kurerTableView td
                    {
                        border: 2px solid black;
                    }
                    .kurerTableView input
                    {
                        width: 200px;
                    }
                    .tableFooter
                    {
                        margin-top: 10px;
                        margin-left: auto;
                        margin-right: auto;
                        width: 1200px;
                    }
                    .footer1
                    {
                        float: left;
                    }
                    .footer2
                    {
                        text-align: center;
                    }
                    .footer3
                    {
                        float: right;
                    }
                </style>

            </head>
            <body>
                <div class="headv2">
                    <div class="head-cont">
                        <img class="logo" src="<?php echo $_URL;?>images/logo2.png">
                        <img class='logo3' src='<?php echo $_URL;?>images/logo3.png'>

                    </div>
                </div>

                <!-- Menu -->
                <div id="followNav" class="navv2"><?php echo $this->isAdminPage ? $navBarAdmin : $navBarUser;?></div>

                <!-- Sidens indhold-->
                <div class="sitev2">
                    <?php $this->executeMessage(msg_warning, ""); ?>
                    <?php echo $this->content; ?>
                </div>

                <div>
                    <table class="tableFooter">
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
