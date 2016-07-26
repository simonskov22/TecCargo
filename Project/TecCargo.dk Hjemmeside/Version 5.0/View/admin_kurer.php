<?php

class admin_kurer extends pageSettings{
    
    
    public function __construct() {
        global $_URL, $_SCRIPTFILES, $_STYLEFILES,$_URLPATH;
        
        $_SCRIPTFILES[] = $_URL."script/admin_user.js";
        $_SCRIPTFILES[] = $_URL."script/form.js";
        
        $_STYLEFILES[] = $_URL."style/form.css";
        $_STYLEFILES[] = $_URL."style/admin_user.css";
        
        require_once $_URLPATH.'/inc/form.php';
        
        $this->AdminOnly = true;
        $this->onlyOneFunc = true;
        $this->defaultFunc = "kurretransport";
        $this->title = "Admin Rediger kurretransport";
    }
    
    public function kurretransport($param = null) {
        
        if(!is_array($param)){
            echo "failed";
        }
        
        if(count($param) == 1){
            $this->SelectGroup($param[0]);
        }
        else{
            $this->EditPrices($param[0], $param[1]);
        }
        ?>

        <?php
    }
    
    private function GetType($type){
        
        if($type == "gorush"){
            
            $sqlType = "rush";
            $title = "GoRush";
        }
        else if($type == "goflex"){
            
            $sqlType = "flex";
            $title = "GoFlex";
            
        }
        
        return array("sql" => $sqlType, "title" => $title);
    }
    
    private function SelectGroup($type){
        global $_URL, $database;
        
        $typeInfo = $this->GetType($type);
        
        $groupInfo = $database->GetResults("SELECT `id`,`gruppe`,`billedBil` FROM `{$database->prefix}PriceKurer` WHERE `type` = '{$typeInfo['sql']}';");
        
        ?>
<style>
    .selectGroup{
        width: 1000px;
        margin: 0 auto;
        border-collapse: collapse;
    }
    .selectGroup img{
        
        max-height: 150px;
        max-width: 150px;
    }
    .selectGroup td, .selectGroup th{
        border: 2px solid #000;
        text-align: center;
    }
</style>
        <table class="selectGroup">
            <tr>
                <th colspan="4"><?php echo "Rediger priser for {$typeInfo['title']}"; ?></th>
            </tr>
            <tr>
                <?php
                    foreach ($groupInfo as $group) {
                        
                        echo "<td style='border-bottom: 0px;'><img src='$group->billedBil'></td>";
                    }
                ?>
            </tr>
            <tr>
                <?php
                    foreach ($groupInfo as $group) {
                        echo "<td style='border-top: 0px;'>"
                        . "<a class='defaultButton buttonsColorBlue' "
                        . "href='{$_URL}admin_kurer/$type/$group->id/'>$group->gruppe</a>";
                        
                    }
                ?>
            </tr>
        </table>
        <?php
    }
    
    /**
     * 
     * @global databaseSetup $database
     * @global message $message
     */
    private function Post_Update(){
        global $database, $message;
        
        $result = $database->Update($database->prefix."PriceKurer", array(
            "billedBil"             => $_POST['input_1'],
            "gruppe"                => $_POST['input_2'],
            "billedVaegt"           => $_POST['input_3'],
            "vaegt"                 => $_POST['input_4'],
            "ladmaal"               => $_POST['input_5'],
            "laesseaabning"         => $_POST['input_6'],
            "lift"                  => $_POST['input_7'],
            "guvareal"              => $_POST['input_8'],
            "rumindhold"            => $_POST['input_9'],
            "prisPrTur"             => $_POST['input_10'],
            "startgebyr"            => $_POST['input_11'],
            "kilometertaksst"       => $_POST['input_12'],
            "tidsForbrug"           => $_POST['input_13'],
            "tidsForbrug2"          => $_POST['input_14'],
            "chauffoermedhjaelper"  => $_POST['input_15'],
            "flytteTilaeg"          => $_POST['input_16'],
            "ADR-tilaeg"            => $_POST['input_17'],
            "natTillaeg"            => $_POST['input_18'],
            "weekendTillaeg"        => $_POST['input_19'],
            "yderzoneTillaeg"       => $_POST['input_20'],
            "byttepalleTillaeg"     => $_POST['input_21'],
            "SMS-servicetillaeg"    => $_POST['input_22'],
            "adresseKorrektion"     => $_POST['input_23'],
            "braendstofgebyr"       => $_POST['input_24'],
            "miljoegebyr"           => $_POST['input_25'],
            "adminnistrationsgebyr" => $_POST['input_26']
        ), array("id" => intval($_POST['input_0'])));
        
        
        if($result){
            
            $message->AddMessage(msg_succes, "Priser Gemt", "");
        }
        else{
            
            $message->AddMessage(msg_error, "Priser Ikke Gemt", "");
        }
    }
    
    private function EditPrices($type,$id) {
        global $_URL, $database;
        
        if(isset($_POST['action']) &&  $_POST['action'] == "kurer"){
            
            $this->Post_Update();
        }
        
       $typeInfo = $this->GetType($type);
       
       if(!ctype_digit($id)){
           
           echo "failed id";
           return false;
       }
        
        $formPicture = new form();
        $formPicture->AddPicturePickerForm();
        
        $kurrePrices = $database->GetRow("SELECT `id`, `billedBil`, `gruppe`,"
                . " `billedVaegt`, `vaegt`, `ladmaal`, `laesseaabning`, `lift`,"
                . " `guvareal`, `rumindhold`, `prisPrTur`, `startgebyr`,"
                . " `kilometertaksst`, `tidsForbrug`, `tidsForbrug2`,"
                . " `chauffoermedhjaelper`, `flytteTilaeg`,"
                . " `ADR-tilaeg`, `natTillaeg`, `weekendTillaeg`,"
                . " `yderzoneTillaeg`, `byttepalleTillaeg`,"
                . " `SMS-servicetillaeg`, `adresseKorrektion`,"
                . " `braendstofgebyr`, `miljoegebyr`,"
                . " `adminnistrationsgebyr`"
                . " FROM `{$database->prefix}PriceKurer` WHERE `id` = $id;");
        
                
        $formValues = array(
            array("text" => "ID",                                                           "bothValue" => $kurrePrices->id,                   "type" => "hidden"),
            array("text" => "Billede af bilen",                                             "bothValue" => $kurrePrices->billedBil,            "type" => "picture"),
            array("text" => "Overskrift",                                                   "bothValue" => $kurrePrices->gruppe,               "type" => "text"),
            array("text" => "Billede af kg",                                                "bothValue" => $kurrePrices->billedVaegt,          "type" => "picture"),
            array("text" => "Vægt",                                                         "bothValue" => $kurrePrices->vaegt,                "type" => "text"),
            array("text" => "Ladmål (l x b x h)",                                           "bothValue" => $kurrePrices->ladmaal,              "type" => "text"),
            array("text" => "Læsseåbning (b x h)",                                          "bothValue" => $kurrePrices->laesseaabning,        "type" => "text"),
            array("text" => "Lift (max vægt)",                                              "bothValue" => $kurrePrices->lift,                 "type" => "text"),
            array("text" => "Guvareal (m²)",                                                "bothValue" => $kurrePrices->guvareal,             "type" => "text"),
            array("text" => "Rumindhold (m³)",                                              "bothValue" => $kurrePrices->rumindhold,           "type" => "text"),
            array("text" => "Minimun pris pr. tur",                                         "bothValue" => $kurrePrices->prisPrTur,            "type" => "text"),
            array("text" => "Startgebyr",                                                   "bothValue" => $kurrePrices->startgebyr,           "type" => "text"),
            array("text" => "Kilometertakst. pr.kørte km",                                  "bothValue" => $kurrePrices->kilometertaksst,      "type" => "text"),
            array("text" => "Tidsforbrug (ekstra læssetid/ ventetid) taxametre pr. min.",   "bothValue" => $kurrePrices->tidsForbrug,          "type" => "text"),
            array("text" => "Tidsforbrug textmetre pr. påbegyndt 30min.",                   "bothValue" => $kurrePrices->tidsForbrug2,         "type" => "text"),
            array("text" => "Chaufførmedhjælper pr. påbegynft 30min.",                      "bothValue" => $kurrePrices->chauffoermedhjaelper, "type" => "text"),
            array("text" => "Flytte tilæg, (enheder over 90 kg) pr. mand/pr. enhed",        "bothValue" => $kurrePrices->flytteTilaeg,         "type" => "text"),
            array("text" => "ADR-tilæg (Fagligt gods) pr. forsendelese",                    "bothValue" => $kurrePrices->ADR-tilaeg,           "type" => "text"),
            array("text" => "Aften- og nattillæg (18:00-06:00) pr. forsendelse",            "bothValue" => $kurrePrices->natTillaeg,           "type" => "text"),
            array("text" => "Weekendtillæg (lørdag-søndag) pr. forsendelse",                "bothValue" => $kurrePrices->weekendTillaeg,       "type" => "text"),
            array("text" => "Yderzonetillæg beregnes af nettofragt",                        "bothValue" => $kurrePrices->yderzoneTillaeg,      "type" => "text"),
            array("text" => "Byttepalletillæg (franko/ufranko) pr. Palle",                  "bothValue" => $kurrePrices->byttepalleTillaegg,   "type" => "text"),
            array("text" => "SMS servicetillæg pr. advisering",                             "bothValue" => $kurrePrices->SMS-servicetillaeg,   "type" => "text"),
            array("text" => "Adresse korrektion pr. forsendelse",                           "bothValue" => $kurrePrices->adresseKorrektion,    "type" => "text"),
            array("text" => "Brændstofgebyr Beregnes af nettofragt",                        "bothValue" => $kurrePrices->braendstofgebyr,      "type" => "text"),
            array("text" => "Miljagebyr beregnes af nettofragt",                            "bothValue" => $kurrePrices->miljoegebyr,          "type" => "text"),
            array("text" => "Adminnistrationsgebyr pr. faktura",                            "bothValue" => $kurrePrices->adminnistrationsgebyr,"type" => "text"),
        );
        
        echo "<form class='tecForm adminUser' method='post' action='{$_URL}admin_kurer/$type/$id/' style='width: 700px;'>"
            . "<input type='hidden' name='action' value='kurer' />"
            ."<div class='formheader'>"
                . "<h2>Rediger pris for {$typeInfo['title']}</h2>"
            . "</div>"
            ."<table>";
        
        for($i = 0; $i < count($formValues); $i++){
            
            if($formValues[$i]['type'] != "hidden"){
                
            echo "<tr id='row$i'>"
                . "<td>{$formValues[$i]['text']}</td>";
                
                if($formValues[$i]['type'] == "text"){
                    $formValues[$i]['focus'] = "#row$i";
                    $formValues[$i]['name'] = "input_$i";
                    $element = $formPicture->AddInputBox($formValues[$i]);
                }
                else{
                    $element = $formPicture->AddPictureButton("input_$i", "Skift billede", $formValues[$i]['bothValue']);
                }
                
                echo  "<td>$element</td></tr>";
            }
            else{
                echo "<input name='input_$i' type='hidden' value='{$formValues[$i]['bothValue']}' />";
            }
        
        }
        echo "</table>"
            . "<div class='formbottom'>"
                . "<input type='reset' class='defaultButton' onclick='return false;' style='margin-left: 10px;'>"
                . "<input type='submit' class='defaultButton' value='Gem' onclick='return false;' style='float: right; margin-right: 8px;'>"
            ."</div>"
        . "</form>";
        
    }
}
