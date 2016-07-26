<?php

class admin_custompage extends pageSettings{
    
    private $pageValue;


    public function __construct() {
        global $_URL, $_SCRIPTFILES, $_STYLEFILES;
        
//        $_STYLEFILES[] = $_URL."plugins/CLEditor1_4_5/jquery.cleditor.css";
//        $_SCRIPTFILES[] = $_URL."plugins/CLEditor1_4_5/jquery.min.js";
//        $_SCRIPTFILES[] = $_URL."plugins/CLEditor1_4_5/jquery.cleditor.min.js";
        
        $_SCRIPTFILES[] = $_URL."plugins/ckeditor/ckeditor.js";
        
        $_SCRIPTFILES[] = $_URL."script/admin_customPage.js";
        $_STYLEFILES[] = $_URL."style/admin_customPage.css";
        
        $_SCRIPTFILES[] = $_URL."script/form.js";
        $_STYLEFILES[] = $_URL."style/form.css";
        
        $_SCRIPTFILES[] = $_URL."plugins/ckeditor/plugins/codesnippet/lib/highlight/highlight.pack.js";
        $_STYLEFILES[] = $_URL."plugins/ckeditor/plugins/codesnippet/lib/highlight/styles/default.css";
        
        $_STYLEFILES[] = $_URL."style/admin_user.css";
        
        
        $this->AdminOnly = TRUE;
        
        $this->pageValue = array(
            "link" => "create",
            "type" => "create",
            "title" => "Opret Side",
            "submit" => "Opret",
            "inputTitle" => "",
            "inputContent" => "",
            "pageId" => 0,
            'useIndent' => 0
        );
    }
    
    public function Post_Create() {
        global $database, $message;
        
        $title = $_POST['title'];
        $titleNotInUse = $database->ValueIsUnique($database->prefix."Pages", $title, 'title');
        $content = $_POST['content'];
        $allowInsert = true;
        $useIndent = $_POST['useIndent'] ? 1 : 0;
        
        if(!$titleNotInUse){
            
            $message->AddMessage(msg_error, "Side ikke oprettet", "Kan ikke oprette en side med samme title.");
            $allowInsert = false;
            
        }
        if(strlen($title) == 0){
            
            $message->AddMessage(msg_error, "Side ikke oprettet", "Kan ikke oprette en side uden title.");
            $allowInsert = false;
        }
        if(strlen($content) == 0){
            
            $message->AddMessage(msg_error, "Side ikke oprettet", "Kan ikke oprette en side uden indhold.");
            $allowInsert = false;
        }
        
        if($allowInsert){
            $result = $database->Insert($database->prefix."Pages", array(
                "title" => $title,
                "content" => $content,
                "useIndent" => $useIndent
            ));

            $this->pageValue['inputTitle'] = $title;
            $this->pageValue['inputContent'] = $content;
            $this->pageValue['useIndent'] = $useIndent;
            
            if($result){
                
                $this->pageValue['link'] = "edit";
                $this->pageValue['type'] = "edit";
                $this->pageValue['title'] = "Rediger Side";
                $this->pageValue['submit'] = "Gem";
                $message->AddMessage(msg_succes, "Side oprettet", "Siden er nu oprettet.");
            }
            else{
                $message->AddMessage(msg_error, "Side ikke oprettet", "Noget gik galt.");
            }
        }
    }
    
    public function Post_Edit() {
        global $database, $message;
        
        
        $pageId = intval($_POST['pageId']);
        $title = $_POST['title'];
        $titleNotInUse = $database->ValueIsUnique($database->prefix."Pages", $title, 'title');
        $titleNotSame = $database->ValueIsSame($database->prefix."Pages", $title, 'title', array("pageId" => $pageId));
        $content = $_POST['content'];
        $useIndent = $_POST['useIndent'] ? 1 : 0;
        $allowInsert = true;
        
        if(!$titleNotInUse && !$titleNotSame){
            
            $message->AddMessage(msg_error, "Side ikke oprettet", "Kan ikke oprette en side med samme title.");
            $allowInsert = false;
            
        }
        if(strlen($title) == 0){
            
            $message->AddMessage(msg_error, "Side ikke oprettet", "Kan ikke oprette en side uden title.");
            $allowInsert = false;
        }
        if(strlen($content) == 0){
            
            $message->AddMessage(msg_error, "Side ikke oprettet", "Kan ikke oprette en side uden indhold.");
            $allowInsert = false;
        }
        
        if($allowInsert){
            $result = $database->Update($database->prefix."Pages", array(
                "title" => $title,
                "content" => $content,
                "useIndent" => $useIndent
            ),array("pageId" => $pageId));

            
            $this->pageValue['link'] = "edit";
            $this->pageValue['type'] = "edit";
            $this->pageValue['title'] = "Rediger Side";
            $this->pageValue['submit'] = "Gem";
            $this->pageValue['inputTitle'] = $title;
            $this->pageValue['inputContent'] = $content;
            $this->pageValue['useIndent'] = $useIndent;

            if($result){
                
                $message->AddMessage(msg_succes, "Side Gemt", "Siden er nu gemt.");
            }
            else{
                $message->AddMessage(msg_error, "Side ikke gemt", "Noget gik galt.");
            }
        }
    }
    
    private function Post_Delete() {
        global $database, $message;
        
        $pageDeleted = true;
        
        
        foreach ($_POST['deleteIds'] as $pageId) {
            
            if (ctype_digit($pageId)) {
                
                $pageId = intval($pageId);
                
                $result = $database->Delete($database->prefix."Pages", array("pageId" => $pageId));
                
                if(!$result){
                    $pageDeleted = false;
                }
            }
        }
        
        if($pageDeleted){
            
            $message->AddMessage(msg_succes, "Sider Slettet", "");
        }
        else{
            $message->AddMessage(msg_error, "Sider Ikke Slettet", "");
        }
    }
    
    public function Edit($param = null) {
        global $database;
        
        $pageFound = false;
        $this->title = "Admin Rediger Side";
        
        if(is_array($param)){
            
            $pageId = intval($param[0]);
             
             $pageRow = $database->GetRow("SELECT `title`,`content`,`useIndent` FROM `{$database->prefix}Pages` WHERE `pageId` = $pageId;");
             
             if(count($pageRow) == 1){
                 
                 $pageFound = true;
                 $this->pageValue['link'] = "edit";
                 $this->pageValue['type'] = "edit";
                 $this->pageValue['title'] = "Rediger Side";
                 $this->pageValue['submit'] = "Gem";
                 $this->pageValue['inputTitle'] = $pageRow->title;
                 $this->pageValue['inputContent'] = $pageRow->content;
                 $this->pageValue['pageId'] = $pageId;
                 $this->pageValue['useIndent'] = $pageRow->useIndent;
             }
        }
        
        if(isset($_POST['action']) && $_POST['action'] == "edit"){
            $this->Post_Edit();
        }
        
        if($pageFound){
        
            $this->customPageTemplate();
        }
        else{
            
            $this->selectPage();
        }
    }
    
    public function Create($param = null) {
        
        $this->title = "Admin Opret Side";
        
        if(isset($_POST['action']) && $_POST['action'] == "create"){
            $this->Post_Create();
        }
        
        $this->customPageTemplate();
    }
    
    public function Delete($param = null) {
        global $_URL,$database;    
        
        $this->title = "Admin Slet Side";
        
        if(isset($_POST['action']) &&  $_POST['action'] == "delete"){
            
            $this->Post_Delete();
        }
        
        $pageRows = $database->GetResults("SELECT `pageId`,`title` FROM `{$database->prefix}Pages`;");
        
        
        ?>

        <div>
            <form class="adminUser tecFormPicker" bind-multiSelect="true" method="post" action="<?php echo $_URL."admin_custompage/delete/"; ?>" style="width: 800px;">
                
                <input type="hidden" name="action" value="delete" />
                <div class="formheader"><h2 style="padding: 20px;">Slet Sider</h2></div>
                <table class="tecFormPicker pageDeleteTable" style="margin-bottom: 0px;margin-top: 20px; width: 100%;">
                    <tr>
                        <th>Title</th>
                    </tr>
                    <?php 
                    if(is_array($pageRows)){
                        foreach ($pageRows as $page) {


                            echo "<tr bind-inputname='deleteIds[]' bind-inputval='$page->pageId' class='tecFormClickOn'>"
                            . "<td>$page->title</td>"
                            . "</tr>";
                        }
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
    
    /**
     * 
     * @global databaseSetup $database
     * @global message $message
     * @param type $pageValue
     */
    private function customPageTemplate(){
        global $_URL, $database, $message;
        
        $customPage = $database->GetResults("SELECT `pageId`,`title` FROM `{$database->prefix}Pages`;");
        
        
        ?>
<!--<script> 
    $(document).ready(function() {
            $("#pagecontent").cleditor({
                /* width: 500,*/ // width not including margins, borders or padding
                height: 500, // height not including margins, borders or padding
                controls: // controls to add to the toolbar
                    "bold italic underline strikethrough subscript superscript | font size " +
                    "style | color highlight removeformat | bullets numbering | outdent " +
                    "indent | alignleft center alignright justify | undo redo | " +
                    "rule image link unlink | cut copy paste pastetext | print source",
                colors: // colors in the color popup
                    "FFF FCC FC9 FF9 FFC 9F9 9FF CFF CCF FCF " +
                    "CCC F66 F96 FF6 FF3 6F9 3FF 6FF 99F F9F " +
                    "BBB F00 F90 FC6 FF0 3F3 6CC 3CF 66C C6C " +
                    "999 C00 F60 FC3 FC0 3C0 0CC 36F 63F C3C " +
                    "666 900 C60 C93 990 090 399 33F 60C 939 " +
                    "333 600 930 963 660 060 366 009 339 636 " +
                    "000 300 630 633 330 030 033 006 309 303" + 
                    "000 254078 F73700 000 000 000 000 000 000 000",
                fonts: // font names in the font popup
                    "Arial,Arial Black,Comic Sans MS,Courier New,Narrow,Garamond," +
                    "Georgia,Impact,Sans Serif,Serif,Tahoma,Trebuchet MS,Verdana",
                sizes: // sizes in the font size popup
                    "1,2,3,4,5,6,7",
                styles: // styles in the style popup
                    [["Paragraph", "<p>"], ["Header 1", "<h1>"], ["Header 2", "<h2>"],
                    ["Header 3", "<h3>"],  ["Header 4","<h4>"],  ["Header 5","<h5>"],
                    ["Header 6","<h6>"]],
                useCSS: false, // use CSS to style HTML when possible (not supported in ie)
                docType: // Document type contained within the editor
                    '<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">',
                docCSSFile: // CSS file used to style the document contained within the editor
                    "",
                bodyStyle: // style to assign to document body contained within the editor
                    "margin:4px; font:10pt Arial,Verdana; cursor:text"
            });
        });
</script>-->
<form class="adminUser tecCustomPage" method="post" action="<?php echo $_URL."admin_custompage/".$this->pageValue['link']."/".$this->pageValue['pageId'].'/'; ?>" style="width: 100%">
    <input type="hidden" name="action" value="<?php echo $this->pageValue['type']; ?>"/>
    <input type="hidden" name="pageId" value="<?php echo $this->pageValue['pageId']; ?>"/>
    
    <div class="formheader" style="padding: 25px 0; position: relative; text-align: inherit;">
        <div class="tecDropdownBox">
            <ul class="tecDropdown removeUlStyle">
                <li class="selected"><span class="text" bind-value="-1">Rediger side</span>  <span class="arrow">\/</span></li>
                <li>
                    <ul class="option removeUlStyle">
                        <?php 
                        if(is_array($customPage)){
                            foreach ($customPage as $page) {
                                echo "<li bind-value='$page->pageId'>$page->title</li>";
                            }
                        }
                        ?>
                    </ul>
                </li>
            </ul>
            <button class="defaultButton" onclick="tecCustomPage_Edit('<?php echo $_URL."admin_custompage/edit/"; ?>',this); return false;">Rediger</button>
        </div>
        <a href="<?php echo ""; ?>" class="defaultButton buttonCreateNew">Opret ny</a>
        <h2 style="display: inline-block; margin-left: 80px;"><?php echo $this->pageValue['title']; ?></h2>
        <a id="tecCustomPageLink" class="tecCustomPageLink" bind-url="<?php echo $_URL; ?>" target="_blank" href="<?php echo $_URL.$this->pageValue['inputTitle'].'/'; ?>"><?php echo $_URL.$this->pageValue['inputTitle'].'/'; ?></a>
    </div>
    
    <input class="inputTitle" name="title" type="text" value="<?php echo $this->pageValue['inputTitle']; ?>"/>
    
    <div style="margin-bottom: 10px; margin-left: 10px;">
        <input id="useIndent" type="checkbox" name="useIndent" onchange="formCheckbutton('#labelIndent');" style="display:none;" <?php echo $this->pageValue['useIndent'] ? "checked" : ""; ?>/>
        <label id="labelIndent" unselectable="on" onselectstart="return false;" onmousedown="return false;" for="useIndent" class="defaultButton formCheckLabel" onclick="" bind-on="Brug indryk" bind-off="Brug ikke indryk"></label>
    </div>
    <textarea id="pagecontent" name="content"><?php echo $this->pageValue['inputContent']; ?></textarea>
    
    <div class="formbottom" style="text-align: right;">
        <input class="defaultButton" type="submit" value="<?php echo $this->pageValue['submit']; ?>" style="margin-right: 10px;"/>
    </div>
</form>

<script type="text/javascript" charset="utf-8">
    CKEDITOR.replace( 'pagecontent' );
</script>

<?php
    
    }
    
    
    private function selectPage() {
       global $_URL,$database;    
        
        $this->title = "Admin Rediger Side";
        
        
        $pageRows = $database->GetResults("SELECT `pageId`,`title` FROM `{$database->prefix}Pages`;");
        
        
        ?>

<div class="adminUser" style="width:800px;">
    <div class="formheader"><h2 style="padding: 20px;">Reiger Sider</h2></div>
    <table class="tecFormPicker pageDeleteTable" style="margin-bottom: 0px;margin-top: 20px; width: 100%;">
        <tr>
            <th colspan="2">Title</th>
        </tr>
        <?php 
        if(is_array($pageRows)){
            foreach ($pageRows as $page) {


                echo "<tr>"
                . "<td>$page->title</td>"
                        . "<td style='text-align: right; padding-right: 15px;'><a class='defaultButton buttonsColorLightBlue' href='$page->pageId/' style='color: #fff;'>Rediger</a></td>"
                . "</tr>";
            }
        }
        ?>
    </table>

    <div class="formbottom">

    </div>
</div>
        <?php
        
    }
}