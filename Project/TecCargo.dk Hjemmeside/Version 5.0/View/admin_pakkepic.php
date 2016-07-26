<?php

require_once 'inc/form.php';

class admin_pakkepic extends pageSettings{
    
    public function __construct() {
        parent::__construct();
        global $_URL,$_SCRIPTFILES,$_STYLEFILES;
        
        $this->defaultFunc = "editPicture";
        $this->onlyOneFunc = true;
        $this->AdminOnly = true;
        $this->title = "Admin Pakke billeder";
        
        $_STYLEFILES[] = $_URL. "style/admin_user.css";
        $_STYLEFILES[] = $_URL. "style/form.css";
        
        $_SCRIPTFILES[] = $_URL. "script/form.js";
        
        $this->AdminOnly = true;
    }
    
    public function editPicture($param = null) {
        global $database;
        
        if(isset($_POST['action']) && $_POST['action'] == "update"){
            $this->Post_Update();
        }
        
        $pictureRow = $database->GetResults("SELECT `picture` FROM `{$database->prefix}PakkePic`;");
        
        $form = new form();
        $form->AddPicturePickerForm();
        ?>
<style>
    .tecForm .editPakkePic input[type='button'].tecFormPicturePickButtonOpen{
        width: 150px;
    }
    
</style>
<form class="tecForm adminUser" style="width: 800px;" method="post">
    <input type="hidden" name="action" value="update"/>
    <div class="formheader">
        <h2>Menu billeder</h2>
    </div>
    <table class="formcontent editPakkePic" style="text-align: center; margin: 10px 0px">
        <tr>
            <th>XS</th>
            <th>S</th>
            <th>M</th>
            <th>L</th>
        </tr>
        <tr>
            <td><?php echo $form->AddPictureButton("xs", "Vælg Billed", $pictureRow[0]->picture); ?></td>
            <td><?php echo $form->AddPictureButton("s", "Vælg Billed", $pictureRow[1]->picture); ?></td>
            <td><?php echo $form->AddPictureButton("m", "Vælg Billed", $pictureRow[2]->picture); ?></td>
            <td><?php echo $form->AddPictureButton("l", "Vælg Billed", $pictureRow[3]->picture); ?></td>
        </tr>
        <tr>
            <th>XL</th>
            <th>2XL</th>
            <th>3XL</th>
            <th>Gebyr</th>
        </tr>
        <tr>
            <td><?php echo $form->AddPictureButton("xl", "Vælg Billed", $pictureRow[4]->picture); ?></td>
            <td><?php echo $form->AddPictureButton("xxl", "Vælg Billed", $pictureRow[5]->picture); ?></td>
            <td><?php echo $form->AddPictureButton("xxxl", "Vælg Billed", $pictureRow[6]->picture); ?></td>
            <td><?php echo $form->AddPictureButton("gebyr", "Vælg Billed", $pictureRow[7]->picture); ?></td>
        </tr>
    </table>
    <div class="formbottom" style="text-align: right;">
        <input class="defaultButton" type="submit" value="Gem" style="margin-right: 15px;"/>
    </div>
</form>
        <?php 
    }
    
    private function Post_Update() {
        global $database;
        
        $database->Update($database->prefix."PakkePic",array("picture" => $_POST['xs']),array("id"=>1));
        $database->Update($database->prefix."PakkePic",array("picture" => $_POST['s']),array("id"=>2));
        $database->Update($database->prefix."PakkePic",array("picture" => $_POST['m']),array("id"=>3));
        $database->Update($database->prefix."PakkePic",array("picture" => $_POST['l']),array("id"=>4));
        $database->Update($database->prefix."PakkePic",array("picture" => $_POST['xl']),array("id"=>5));
        $database->Update($database->prefix."PakkePic",array("picture" => $_POST['xxl']),array("id"=>6));
        $database->Update($database->prefix."PakkePic",array("picture" => $_POST['xxxl']),array("id"=>7));
        $database->Update($database->prefix."PakkePic",array("picture" => $_POST['gebyr']),array("id"=>8));
    }
    
}