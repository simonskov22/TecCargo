<?php

class admin_gods extends pageSettings{
    
    public function __construct() {
        global $_URL, $_SCRIPTFILES, $_STYLEFILES,$_URLPATH;
        
        $_SCRIPTFILES[] = $_URL."script/admin_user.js";
        $_SCRIPTFILES[] = $_URL."script/form.js";
        
        $_STYLEFILES[] = $_URL."style/form.css";
        $_STYLEFILES[] = $_URL."style/admin_user.css";
        
        require_once $_URLPATH.'/inc/form.php';
        
        $this->AdminOnly = true;
        $this->onlyOneFunc = true;
        $this->defaultFunc = "godstransport";
        $this->title = "Admin Rediger Godstransport";
    }
    
    /**
     * 
     * @global databaseSetup $database
     * @global message $message
     */
    public function Post_Update() {
        global $database, $message;
        
        $startPriceSQl = "";
        $takstPriceSQL = "";
            
        $startPriceCount = 1;
        $updateSucces = true;
        

        for ($i = 0; $i < 8; $i++) {


            for ($a = 0; $a < 6; $a++) {

                if($a < 5){

                    $result = $database->Update($database->prefix."PriceGodsPart", 
                        array("startPris" => $_POST["input_$i"][$a]),
                        array("id" => $startPriceCount));
                    
//                    $startPriceSQl .= "PRIS: {$_POST["input_$i"][$a]} "
//                    . "<br>--SQL: {$database->lastQuery}"
//                    . "<br>----Name: input_$i ::: $a"."<br>";
//                    
                    $startPriceCount++;
                }
                else{

                    $result = $database->Update($database->prefix."ProcentGodsPart", array(
                        "Takst2"  => $_POST["input_$i"][5],
                        "Takst3"  => $_POST["input_$i"][6],
                        "Takst4"  => $_POST["input_$i"][7],
                        "Takst5"  => $_POST["input_$i"][8],
                        "Takst6"  => $_POST["input_$i"][9],
                        "Takst7"  => $_POST["input_$i"][10],
                        "Takst8"  => $_POST["input_$i"][11],
                        "Takst9"  => $_POST["input_$i"][12],
                        "Takst10" => $_POST["input_$i"][13]
                    ), array("id" => $i + 1));
                }
                
                if(!$result){
                    
                    $updateSucces = false;
                }

            }
        }
        
        if($updateSucces){
            
            $message->AddMessage(msg_succes, "Pris Gemt", "");
        }
        else{
            
            $message->AddMessage(msg_error, "Pris Ikke Gemt", $database->lastError);
        }
    }
    
    public function godstransport($param = null) {
        global $database,$_URL;
        
        if(isset($_POST['action']) &&  $_POST['action'] == "gods"){
            
            $this->Post_Update();
        }
        
        
        ?>
<style>
    .removeUlStyle ul{
        padding: 0;
        margin: 0;
        text-indent: 0;
        list-style: none;
    }
    input[type='text'].inputTextSmall{
        width: 100px;
        text-align: right;
        padding-right: 5px;
    }
</style>
<?php
        
        $form = new form();
        
        $fieldname = array("30 - 100 kg","125 - 250 kg","300 - 500 kg","600 - 1.000 kg",
                "1.100 - 1.500 kg","1.600 - 2.000 kg","2.100 - 2.500 kg", "2.600 - 3.000 kg");

        $kgform = array("30 kg","40 kg","50 kg","75 kg","100 kg","125 kg","150 kg","175 kg",
            "200 kg","250 kg","300 kg","350 kg","400 kg","450 kg","500 kg","600 kg","700 kg",
            "800 kg","900 kg","1.000 kg","1.100 kg","1.200 kg","1.300 kg","1.400 kg","1.500 kg",
            "1.600 kg","1.700 kg","1.800 kg","1.900 kg","2.000 kg","2.100 kg","2.200 kg","2.300 kg",
            "2.400 kg","2.500 kg","2.600 kg","2.700 kg","2.800 kg","2.900 kg","3.000 kg");

        $startPrice = $database->GetResults("SELECT `id`,`startPris` FROM `{$database->prefix}PriceGodsPart` ORDER BY `id` ASC;");
        $procentPrice = $database->GetResults("SELECT `id`, `Takst2`, `Takst3`, `Takst4`, `Takst5`,"
                . " `Takst6`, `Takst7`, `Takst8`, `Takst9`, `Takst10` FROM `{$database->prefix}ProcentGodsPart` ORDER BY `id` ASC;", array_I);
        
                
        $kiloTextCount = 0;
        $tableTrContentArray = array();
        
        for($i = 0; $i < count($fieldname); $i++){
            $takstCountText = 2;
            $ulContentArray = array();
            
            for($a = 0; $a < 3; $a++){
            
                $contentUl = "<ul>";
                
                for ($b = 0; $b < 5; $b++) {
                    
                    if($takstCountText > 10){
                        break;
                    }
                    
                    if($a == 0){
                        
                        $text1 = $kgform[$kiloTextCount];
                        $text2 = ".kr";
                        $value = $startPrice[$kiloTextCount]->startPris;
                        $kiloTextCount++;
                        
                    }
                    else{
                        $text1 = "Takst $takstCountText";
                        $text2 = "%";
                        $value = $procentPrice[$i][$takstCountText -1];
                        $takstCountText++;
                    }
                    $contentUl .= "<li id='row_{$i}_{$a}_{$b}'>"
                            . "<span class='tecformLiText1'>$text1</span>"
                            . $form->AddInputBox(array("class" => "inputTextSmall","name" => "input_{$i}[]","bothValue" => $value, "focus"=> "#row_{$i}_{$a}_{$b}"))
                            . "<span class='tecformLiText2'>$text2</span></li>";
                }

                $contentUl .= "</ul>";
                
                $ulContentArray[] = $contentUl;
            }
            $tableTrContentArray[] = $ulContentArray;
        }
        
        echo "<form class='tecForm adminUser removeUlStyle' method='post' style='width: 800px;'>"
            . "<input type='hidden' name='action' value='gods' />"
            . "<div class='formheader'><h2>Rediger pris for GoPart</h2></div>";
        
        for($i = 0; $i < count($fieldname); $i++) {
            
            echo "<fieldset>"
                    . "<legend>$fieldname[$i]</legend>"
                    . "<table>"
                        . "<tr class='tecformHederSpace'>"
                            . "<th>Startpris</th>"
                            . "<th colspan='2'>Forøg med Procent</th>"
                        . "</tr>"
                        . "<tr>"
                            . "<td>{$tableTrContentArray[$i][0]}</td>"
                            . "<td>{$tableTrContentArray[$i][1]}</td>"
                            . "<td>{$tableTrContentArray[$i][2]}</td>"
                        . "</tr>"
                    . "</table>"
                . "</fieldset>";
        }
        echo "<div class='formbottom'>"
            . "<input type='reset' onclick='return false;' class='defaultButton' style='margin-left: 10px;'>"
            . "<input type='submit' onclick='return false;' class='defaultButton' value='Gem' style='float: right; margin-right: 8px;'>"
        . "</div>"
    ."</form>";
    }
    
}