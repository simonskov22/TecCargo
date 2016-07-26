<?php

class form{

    public function __construct() {
        
    }
    
    public function AddPicturePickerForm() {
        global $_URL, $_URLPATH;
        
        ?>
        <script>
            $(document).ready(function (){
                <?php 

                $filePathAndName = glob($_URLPATH."/images/*");
                $pictureList = "[";

                foreach ($filePathAndName as $file) {

                    if(!is_dir($file)){

                        $fileBaseName = basename($file);

                        $picture = "['{$_URL}images/$fileBaseName','$fileBaseName']";
                        $pictureList .= $picture .',';
                    }
                }
                $pictureList = substr($pictureList, 0, strlen($pictureList) -1);
                $pictureList .= "]";
                ?>
                addPictureList(<?php echo $pictureList; ?>);

            });
        </script>
        <div class="tecFormFloatBox" style="display: none;">
            <div class="tecFormPicturePickBox adminUser" style="width: 1000px;">
                <img class="tecFormPicturePickBoxClose" onclick="tecFormPicturePick_Close('.tecFormFloatBox');" src="<?php echo $_URL."images/icons/close.png"; ?>" />
                <div class="tecFormPicturePickHeader formheader">
                    <h2>Vælg Billed</h2>
                    <input type="text" value="">
                </div>
                <div class="tecFormPicturePickContent">
                    <table style="width: 100%; border-collapse: collapse;">
                      
                    </table>
                </div>
                <div class="tecFormPicturePickBottom formbottom" style="text-align: center;">
                    <!--<button class="tecFormPicturePickButtonPage" style="float: left;">Forrige Side</button>-->
                    <button class="tecFormPicturePickButtonPick" onclick="tecFormPicturePick_Save(this);">Brug Valgte</button>
                    <!--<button class="tecFormPicturePickButtonPage" style="float: right;">Næste Side</button>-->
                </div>
            </div>
        </div>
        <?php
    }
    
    public function AddPictureButton($name,$buttonText,$lastSelect) {
        return "<input type='tecFormPicturePick' class='tecFormPicturePickButtonOpen' bind-buttonText='$buttonText' name='$name' value='$lastSelect'/>";
    }
    
    /**
     * array containt
     *  focus
     *  defaultVal
     *  value
     *  type
     *  required
     *  min
     *  max
     * 
     * default return value
     *   <input type='text' name='noname'/>
     * 
     * @param array $inputVal
     * @return string input
     */
    public function AddInputBox($inputVal) {
        
        $name = "name='noname'";
        $type = "type='text'";
        $startValue = "";
        $focus = "";
        $defaultVal = " bind-resetval=''";
        $required = "";
        $min = "";
        $max = "";
        $compare = "";
        $class = "";
        $disabled = "";
        
        foreach ($inputVal as $key => $value) {
            
            switch ($key) {
                
                case "focus":
                    $focus = " bind-focusid='$value'";
                    break;
                case "defaultVal":
                    $defaultVal = " bind-resetval='$value'";
                    break;
                case "value":
                    $startValue = " value='$value'";
                    break;
                case "name":
                    $name = "name='$value'";
                    break;
                case "type":
                    $type = "type='$value'";
                    break;
                case "required":
                    $required = " bind-required='$value'";
                    break;
                case "min":
                    $min = " bind-minlengt='$value'";
                    break;
                case "max":
                    $max = " bind-maxlengt='$value'";
                    break;
                case "compare":
                    $compare = " bind-compare='$value'";
                    break;
                case "bothValue":
                    $defaultVal = " bind-resetval='$value'";
                    $startValue = " value='$value'";
                    break;
                
                case "class":
                    $class = " class='$value'";
                    break;
                case "disabled":
                    if($value){
                        
                        $disabled = " disabled";
                    }
                    break;
            }
        }
        
        
        return "<input{$class} $type $name".$startValue.$focus.$defaultVal.$required.$min.$max.$compare.$disabled." />";
    }
    
    
    public function AddSelect($name, $option ,$selected = 0, $class = ""){
        
        $startSelect = false;
        $value = "";
        $selectText = "";
        
        if($selected >= 0 && $selected < count($option)){
            $startSelect = true;
            $value = $option[$selected][0];
            $selectText = $option[$selected][1];
        }
        
        $selectForm = "<div class='tecSelectBox $class'>"
                        . "<input type='hidden' name='$name' value='$value'/>"
                        . "<ul class='tecSelect'>"
                .           "<li class='selected'><span class='text'>$selectText</span> <span class='arrow'></span></li>"
                            . "<li class='option'>"
                                . "<ul>";
    
        for ($i = 0; $i < count($option); $i++) {
            
            $class = "";
            if($startSelect && $i == $selected){
                
                $class = "class='userCrowSelect' ";
            }
            
            $selectForm .= "<li {$class}data-value='{$option[$i][0]}'>{$option[$i][1]}</li>";
        }
        
        
        $selectForm .=          "</ul>"
                            . "</li>"
                        . "</ul>"
                    . "</div>";
    
        return $selectForm;

    }
}
//