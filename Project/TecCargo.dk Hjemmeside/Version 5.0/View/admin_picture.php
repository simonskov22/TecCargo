<?php
class admin_picture extends pageSettings{
    
    public function __construct() {
        global $_URL, $_STYLEFILES, $_SCRIPTFILES;
        
        $_STYLEFILES[] = $_URL."style/form.css";
        $_STYLEFILES[] = $_URL."style/admin_user.css";
        $_STYLEFILES[] = $_URL."style/admin_picture.css";
        
        $_SCRIPTFILES[] = $_URL."script/admin_picture.js";
        $_SCRIPTFILES[] = $_URL."script/admin_user.js";
        
        $this->onlyOneFunc = true;
        $this->defaultFunc = "picture";
        $this->AdminOnly = true;
        $this->title = "Admin Billeder";
        
    }
    /**
     * 
     * @global message $message
     */
    private function Post_Upload() {
        global $_URLPATH, $message;

        
        
        $uploadDir = $_URLPATH.'/images/';
        $errorMsg = "<ul>";
        $uploadSucces = true;
    
        $files = $_FILES['files'];
        
        for ($i = 0; $i < count($files['name']); $i++) {
            
            
            if ($files['type'][$i] == "image/jpeg" || $files['type'][$i] == "image/gif" || $files['type'][$i] == "image/png") {

                //tjekker om filen findes
                
                
                if(!file_exists($uploadDir .basename($files['name'][$i]))) {

                    //hvis den ikke kan flytte filen skal den sige der er en fejl
                    if(!move_uploaded_file($files['tmp_name'][$i], $uploadDir .basename($files['name'][$i])))
                    {
                        $errorMsg .= "<li>".$files['error'][$i]."</li>";
                        $uploadSucces = false;
                    }
                }
                else{
                    $errorMsg .= "<li>".$files['error'][$i]."</li>";
                    $uploadSucces = false;
                }
            }
            else{
                $errorMsg .= "<li>".$files['error'][$i]."</li>";
                $uploadSucces = false;
            }
        }

        if ($uploadSucces) {

            $message->AddMessage(msg_succes, "Billeder Uploaded", "");  

        }
        else{
            $errorMsg .= "</ul>";
            $message->AddMessage(msg_error, "Billeder Ikke Uploaded", $errorMsg);  
        }

    }
    
    /**
     * 
     * @global message $message
     */
    private function Post_Delete() {
        global $_URLPATH, $message;
        
        $fileIsDeleted = false;
        $deletedFiles = "<ul>";
        $fileIsNotDeleted = false;
        $deletedFilesNot = "<ul>";

        $imagesDir = $_URLPATH."/images/";

        //sletter valgte billeder
        foreach ($_POST['deleteFiles'] as $picture) {

            if(file_exists($imagesDir.$picture)){
                
                $fileIsDeleted = true;
                
                $deletedFiles .= "<li>$picture</li>";
                unlink($imagesDir.$picture);
            }
            else{
                
                $deletedFilesNot .= "<li>$picture</li>";
                $fileIsNotDeleted = true;
            }
        }
        
        if($fileIsDeleted){
            
            $deletedFiles .= "</ul>";
            $message->AddMessage(msg_succes, "Følgende filer er blevet slettet", $deletedFiles);
        }
        if($fileIsNotDeleted){
            
            $deletedFilesNot .= "</ul>";
            $message->AddMessage(msg_error, "Følgende filer er ikke blevet slettet", $deletedFilesNot);
        }
        
        if(!$fileIsDeleted && !$fileIsNotDeleted){
            
            $message->AddMessage(msg_warning, "Noget gik galt", "");
        }
        


    }


    public function picture($param = null) {
        global $_URL, $_URLPATH;
        
        
        if(isset($_POST['action'])){
            
            switch ($_POST['action']) {
                case "upload":

                    $this->Post_Upload();
                    break;
                case "delete":
                    
                    $this->Post_Delete();
                    break;
            }
        }
        
        $picturePage = 20;
        
        $filePathAndName = glob($_URLPATH."/images/*");
        $pictureArray = array();
        
        foreach ($filePathAndName as $file) {

            if(!is_dir($file)){
                $fileBaseName = basename($file);
                $pictureArray[] = $fileBaseName;
            }
        }
        
        $lastPage = count($pictureArray) / $picturePage;
        $lastPage = ceil($lastPage);
        
        if(is_array($param) && ctype_digit($param[0]) && 
            $param[0] > 0 && $param[0] <= $lastPage){
            
            $page = intval($param[0]);
        }
        else{
            
            $page = 1;
        }
        
        $stopPicture = $page * $picturePage;
        $startPicture = $stopPicture - $picturePage;
        
     ?>

<div class="tecFormFloatBox tecFormUpload" style="display: none;">
    <form method="post" action="<?php echo $_URL."admin_picture/"; ?>" enctype="multipart/form-data">
        <input type="hidden" name="action" value="upload"/>
        
        <div class="tecFormPicturePickBox adminUser" style="width: 800px;">
            <img class="tecFormPicturePickBoxClose" onclick="tecCloseBox('.tecFormFloatBox');" src="<?php echo $_URL."images/icons/close.png"; ?>" />
            <div class="tecFormPicturePickHeader formheader">
                <h2>Tilføj billeder</h2>
                <button class="tecFormPicturePickButtonPick defaultButton" onclick="$('#pickPictureUpload').click(); return false;" style="float: right;margin-right: 30px;">Tilføj billeder</button>

                <input id="pickPictureUpload" type="file" id="files" name="files[]" class="upload" accept="image/*" multiple style="display: none;">
            
            </div>
            <div class="tecFormPicturePickContent">
                <table style="width: 100%; border-collapse: collapse;">

                </table>
            </div>
            <div class="tecFormPicturePickBottom formbottom" style="text-align: center;">
                <input type="submit" class="tecFormPicturePickButtonPick defaultButton" value="Gem" />
            </div>
        </div>
    </form>
</div>



<form class="adminUser tecPictureForm" method="post" action="<?php echo $_URL."admin_picture/"; ?>" style="width: 800px;">
    <input type="hidden" name="action" value="delete"/>

    <div>
        <div class="formheader">
            <h2>Billeder</h2>
            <button class="tecFormPicturePickButtonPick defaultButton" style="float: right;margin-top: -25px;margin-right: 15px;" onclick="tecPictureBox_Show(); return false;">Tilføj billeder</button>
        </div>
    </div>
    <table class="adminPictureTable formcontent">
        <?php 
            for($i = $startPicture; $i < count($pictureArray) &&
                    $i < $stopPicture; $i++) {
                echo "<tr bind-value='$pictureArray[$i]'>"
                        . "<td><img src='{$_URL}images/$pictureArray[$i]'/></td>"
                        . "<td>$pictureArray[$i]</td>"
                    . "</tr>";
            }
        ?>
    </table>
    
    <div class="formbottom" style="text-align: center;">
        <input type="reset" class="defaultButton" style="float: left; margin-left: 10px;" onclick="return false;">
        <input type="button" class="defaultButton pageButton" onclick="window.location.href='<?php echo $_URL."admin_picture/".($page - 1)."/"; ?>'"  value="Forrige side">
        <span style="color: #fff;"><?php echo "$page/$lastPage";?></span>
        <input type="button" class="defaultButton pageButton" onclick="window.location.href='<?php echo $_URL."admin_picture/".($page + 1)."/"; ?>'" value="Næste side">
        <input type="submit" class="defaultButton" value="Slet" style="float: right;margin-right: 10px;"  onclick="return false;">
    </div>
</form>
<?php
    }
}