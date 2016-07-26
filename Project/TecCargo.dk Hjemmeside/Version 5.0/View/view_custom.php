<?php

class view_custom extends pageSettings{
    
    
    public function __construct() {
        $this->title = "Forside";
        $this->onlyOneFunc = true;
        $this->defaultFunc = "CreatePage";
        
    }
    
    /**
     * 
     * @global databaseSetup $database
     * @param array $param
     * @return type
     */
    public function CreatePage($param = null) {
        global $database,$_URL,$_STYLEFILES;
        $_STYLEFILES[] = $_URL ."style/view_custom.css";
        if(!is_array($param) && count($param) < 0){
            $this->title = "Not Found";
            echo "Siden kunne ikke findes.";
            return ;
        }
        $lastParamId = count($param)-1;
        
        $title = $database->SQLReadyString($param[$lastParamId]);
        
        $pageRow = $database->GetRow("SELECT `content`,`useIndent` FROM `{$database->prefix}Pages` WHERE `title` = '$title';");
        
        if(!$pageRow){
            
            $this->title = "Not Found";
            echo "Siden kunne ikke findes.";
            return;
        }
        ?>
<script>
    $(document).ready(function(){
    $("body input[type='button']").each(function(){
       
        //showPage();|Se Pris| 
        var text = $(this).val();
        var length = text.length;
        var value = ["","",""];
        var selector = 0;
        
        for(var i = 0; i < length; i++){
            
            if(text.substring(i,i+1) === "|"){
                selector++;
            }
            else{
                value[selector] += text.substring(i,i+1);
            }
        }
        var button = "<button onclick=\""+value[0]+"\" class=\"defaultButton"+value[2]+"\">"+value[1]+"</button>";
        $(this).replaceWith(button);
    });
});
</script>
<div class="defaultPageSettings">
        <?php 
        
        $this->title = $title;
        if ($pageRow->useIndent) {
            echo "<div class='customCenter'>";
            echo $pageRow->content;
            echo "</div>";
        }
        else{
            
            echo $pageRow->content;
        }
        ?>
</div>
    <?php
    }
}