<?php


require_once 'inc/form.php';

class admin_navigationpicture extends pageSettings{
    
    public function __construct() {
        parent::__construct();
        global $_URL,$_SCRIPTFILES,$_STYLEFILES;
        
        $this->defaultFunc = "editPicture";
        $this->onlyOneFunc = true;
        $this->AdminOnly = true;
        $this->title = "Admin Menu billeder";
        
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
        
        $pictureRow = $database->GetResults("SELECT `picture` FROM `{$database->prefix}MenuPic`;");
        
        $form = new form();
        $form->AddPicturePickerForm();
        
        ?>
<form class="tecForm adminUser" style="width: 800px;" method="post">
    <input type="hidden" name="action" value="update"/>
    <div class="formheader">
        <h2>Menu billeder</h2>
    </div>
    <table class="formcontent" style="text-align: center; margin: 10px 0px">
        <tr>
            <th>Kurertransport</th>
            <th>Pakketransport</th>
            <th>Godstransport</th>
        </tr>
        <tr>
            <td><?php echo $form->AddPictureButton("kurer", "Vælg Billed", $pictureRow[0]->picture); ?></td>
            <td><?php echo $form->AddPictureButton("pakke", "Vælg Billed", $pictureRow[1]->picture); ?></td>
            <td><?php echo $form->AddPictureButton("gods", "Vælg Billed", $pictureRow[2]->picture); ?></td>
        </tr>
        <tr>
            <th>Logistics</th>
            <th>Montage</th>
            <th></th>
        </tr>
        <tr>
            <td><?php echo $form->AddPictureButton("logistics", "Vælg Billed", $pictureRow[3]->picture); ?></td>
            <td><?php echo $form->AddPictureButton("montage", "Vælg Billed", $pictureRow[4]->picture); ?></td>
            <td></td>
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
        
        $database->Update($database->prefix."MenuPic",array("picture" => $_POST['kurer']),array("id"=>1));
        $database->Update($database->prefix."MenuPic",array("picture" => $_POST['pakke']),array("id"=>2));
        $database->Update($database->prefix."MenuPic",array("picture" => $_POST['gods']),array("id"=>3));
        $database->Update($database->prefix."MenuPic",array("picture" => $_POST['logistics']),array("id"=>4));
        $database->Update($database->prefix."MenuPic",array("picture" => $_POST['montage']),array("id"=>5));
    }
    
}