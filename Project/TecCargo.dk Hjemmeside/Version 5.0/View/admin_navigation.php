<?php

require_once 'inc/form.php';

class admin_navigation extends pageSettings{
    
    public function __construct() {
        global $_URL, $_STYLEFILES, $_SCRIPTFILES;
        
        $_STYLEFILES[] = $_URL. "style/admin_user.css";
        $_STYLEFILES[] = $_URL. "style/form.css";
        
        $_SCRIPTFILES[] = $_URL. "script/form.js";
        
        $this->AdminOnly = true;
    }
    
    public function Delete($param = null) {
        
    }
    
    public function Add($param = null) {
        
        if(isset($_POST['action']) && $_POST['action'] === "add"){
            $this->Post_Add();
        }
        
        $values = array(
            "action" => "add",
            "name" => "",
            "url" => "",
            "picture" => "",
            "title" => "",
            "linkName" => ""
        );
        
        $this->LinkTemplate($values);
    }
    
    public function Edit($param = null) {
        global $database;
        
        $this->title = "Admin Rediger Link";
        
        if(isset($_POST['action']) && $_POST['action'] === "edit"){
            $this->Post_Add();
        }
        
        if(isset($param[0]) && ctype_digit($param[0])){
            
            $navId = intval($param[0]);
            $linkRow = $database->GetRow("SELECT `name`,`url`,`picture`,`title`,`linkName` FROM `{$database->prefix}Navigation` WHERE `navId` = $navId;");
        
            if($linkRow){
                
                 $values = array(
                    "pageTitle" => "Rediger link",
                    "action" => "edit",
                    "name" => $linkRow->name,
                    "url" => $linkRow->url,
                    "picture" => $linkRow->picture,
                    "title" => $linkRow->title,
                    "linkName" => $linkRow->linkName
        );
                $this->LinkTemplate($values);
            }
        }
        else{
            $values = array(
            "pageTitle" => "Rediger link",
            "pageButton" => "Rediger",
            "pageButtonLink" => "edit/"
                );
            $this->ListTemplate($values);
        }
        
        
    }
    
    private function Post_Add() {
        global $database, $message;
        
        $allowInsert = true;
        $type = $_POST['type'];
        $valueArray = array();
        $sqlValue = array();
        
        if(!ctype_digit($type)){
            $allowInsert = false;
        }
        
        switch ($type) {
            
            case navbar_link:
                
                $valueArray[] = array("sqlName" => "name","value" => $_POST['name']);
                $valueArray[] = array("sqlName" => "url","value" => $_POST['url']);
                break;
            
            case navbar_dropdown:
                
                $valueArray[] = array("sqlName" => "name","value" => $_POST['name']);
                $valueArray[] = array("sqlName" => "url","value" => $_POST['url']);
                $valueArray[] = array("sqlName" => "linkName","value" => $_POST['linkName']);
                break;
            
            case navbar_dropdownheader:
                
                $valueArray[] = array("sqlName" => "name","value" => $_POST['name']);
                $valueArray[] = array("sqlName" => "url","value" => $_POST['url']);
                $valueArray[] = array("sqlName" => "linkName","value" => $_POST['linkName']);
                $valueArray[] = array("sqlName" => "title","value" => $_POST['title']);
                break;
            
            case navbar_dropdownpic:
                
                $valueArray[] = array("sqlName" => "name","value" => $_POST['name']);
                $valueArray[] = array("sqlName" => "url","value" => $_POST['url']);
                $valueArray[] = array("sqlName" => "linkName","value" => $_POST['linkName']);
                $valueArray[] = array("sqlName" => "title","value" => $_POST['title']);
                $valueArray[] = array("sqlName" => "picture","value" => $_POST['picture']);
                break;
        }
        
        foreach ($valueArray as $value) {
            
            if(strlen($value['value']) == 0){
                
                $allowInsert = false;
                break;
            }
            else{
                $sqlValue[$value['sqlName']] = $value['value'];
            }
        }
        
        if($allowInsert){
            
            $database->Insert($database->prefix."Navigation", $sqlValue);
            
            $message->AddMessage(msg_succes,"Link tilføjet","");
        }
        else{
            
            $message->AddMessage(msg_error,"Link ikke tilføjet","");
        }
    }
    
    private function LinkTemplate($values) {
        global $_URL;
        
       
        
        $this->title = "Admin Tilføj Link";
        
        $form = new form();
        $form->AddPicturePickerForm();
        
        $textArray = array(
            array("focus" => "#row0", "name" => "name","value" => $values['name']),
            array("focus" => "#row1", "name" => "url","value" => $values['url']),
            array("focus" => "#row3", "name" => "title","value" => $values['title']),
            array("focus" => "#row4", "name" => "linkName","value" => $values['linkName'])
        );
        
        $selectOptionVal = array(
            array(navbar_link,"Link"),
            array(navbar_dropdown,"Dropdown"),
            array(navbar_dropdownheader,"Dropdown med title"),
            array(navbar_dropdownpic,"Dropdown med billed")
        );
        
        
        ?>
<style>
    .navselectType .tecSelect, td.navButton input.tecFormPicturePickButtonOpen{
        width: 280px !important;
    }
    .fromContent{
        width: 100%; 
        border-collapse: collapse;
    }
    .fromContent td{
        padding: 10px 5px;
    }
</style>
<script>
    $(document).ready(function(){
        
        navUpdateField(".navTypeChange input");
        
        $(".navTypeChange input").change(function (){
            
            navUpdateField(this);
        });
    });
    
    function navUpdateField(selector){
        
       var value = parseInt($(selector).val());
            
        switch(value){
            case 0:
                $("#row2").hide();
                $("#row3").hide();
                $("#row4").hide();
                break;
            case 1:
                $("#row2").hide();
                $("#row3").hide();
                $("#row4").show();
                break;
            case 2:
                $("#row2").hide();
                $("#row3").show();
                $("#row4").show();
                break;
            case 3:
                $("#row2").show();
                $("#row3").show();
                $("#row4").show();
                break;
        }   
    }
</script>
<div>
    <form class="adminUser tecForm" method="post" action="<?php echo $_URL."admin_navigation/add/"; ?>">
        <input type="hidden" name="action" value="<?php echo $values['action']; ?>"/>
        <div class="formheader">
            <h2>Tilføj link</h2>
        </div>
        <table class="fromContent">
            <tr>
                <td>Type</td>
                <td style="text-align: right;" class="navTypeChange">
                    <?php echo $form->AddSelect("type",$selectOptionVal, $values['type'], "navselectType"); ?>
                </td>
            </tr>
            <tr id="row0">
                <td>Menu navn</td>
                <td style="text-align: right;"><?php echo $form->AddInputBox($textArray[0]); ?></td>
            </tr>
            <tr id="row1">
                <td>URL Adresse</td>
                <td style="text-align: right;"><?php echo $form->AddInputBox($textArray[1]); ?></td>
            </tr>
            <tr id="row2">
                <td>Billed</td>
                <td style="text-align: right;" class="navButton"><?php echo $form->AddPictureButton("picture", "Vælg billed", $values['picture']); ?></td>
            </tr>
            <tr id="row3">
                <td>Title</td>
                <td style="text-align: right;"><?php echo $form->AddInputBox($textArray[2]); ?></td>
            </tr>
            <tr id="row4">
                <td>Link navn</td>
                <td style="text-align: right;"><?php echo $form->AddInputBox($textArray[3]); ?></td>
            </tr>
        </table>
        <div class="formbottom" style="text-align: right; padding-right: 10px;">
            <input class="defaultButton" type="submit" value="Tilføj"/>
        </div>
    </form>
</div>

        <?php
    }
    private function ListTemplate($values) {
        global $database,$_URL;
        
        $linkRow = $database->GetResults("SELECT `navId`,`name`,`url` FROM `{$database->prefix}Navigation` WHERE type = ".navbar_link.";");
        $dropdownRow = $database->GetResults("SELECT `navId`,`name`,`url`,`linkName` FROM `{$database->prefix}Navigation` WHERE type = ".navbar_dropdown.";");
        $dropdownTitleRow = $database->GetResults("SELECT `navId`,`name`,`url`,`linkName`,`title` FROM `{$database->prefix}Navigation` WHERE type = ".navbar_dropdownheader.";");
        $dropdownPicRow = $database->GetResults("SELECT `navId`,`name`,`url`,`linkName`,`title`,`picture` FROM `{$database->prefix}Navigation` WHERE type = ".navbar_dropdownpic.";");
        
        $linkArray = array();
        $linkArray[] = array("name"=>"Link","value"=>$linkRow);
        $linkArray[] = array("name"=>"Dropdown","value"=>$dropdownRow);
        $linkArray[] = array("name"=>"Dropdown med title","value"=>$dropdownTitleRow);
        $linkArray[] = array("name"=>"Dropdown med billed","value"=>$dropdownPicRow);
        
        ?>

<div class="adminUser">
    <div class="formheader">
        <h2><?php echo $values["pageTitle"]; ?></h2>
    </div>
    <table class="formcontent">
        <?php 
            foreach ($linkArray as $value) {
                
                if(is_array($value['value'])){
                    
                    echo "<tr>"
                            . "<th colspan='2'>{$value['name']}</th>"
                        . "</tr>";
                            
                    foreach ($value['value'] as $row) {
                        
                        echo "<tr>"
                            . "<td>{$row->name}</td>"
                            . "<td><a href='{$_URL}admin_navigation/{$values['pageButtonLink']}{$row->navId}/'>{$values['pageButton']}</a></td>"
                        . "</tr>";
                    }
                }
            }
        ?>
    </table>
    <div class="formbottom"></div>
</div>
        <?php

    }
}