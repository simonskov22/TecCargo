<?php

class kurer_price extends pageSettings{
    
    public function __construct() {
        
        $this->onlyOneFunc = true;
        $this->defaultFunc = "Kurer";
        $this->title = "Kurertransport Prisliste";
    }
    
    public function Kurer($param = null) {
        
        if(!is_array($param)){
            $param[] = 'gorush';
        }
        
        $setting = $this->GetType($param[0]);
        
        $this->kurerPrice($setting);
    }
    
    private function kurerPrice($setting) {
        global $database,$_URL,$_STYLEFILES;
        $_STYLEFILES[] = $_URL."style/prices.css";

    $kurerTransportText = array(
        "HEADER","Ladmål (l x b x h)", "læsseåbning (b x h)", "Lift (max vægt)", "Guvareal (m&sup2)",
        "Rumindhold (m&sup3)",
        "HEADER","Minimun pris pr. tur", "Startgebyr", "Kilometertakst. pr.kørte km",
        "HEADER","Tidsforbrug (ekstra læssetid/ ventetid) taxametre pr. min.", "Tidsforbrug taxametre pr. påbegyndt 30min.",
        "Chaufførmedhjælper pr. påbegynft 30min.",
        "HEADER", "Flytte tilæg, (enheder over 90 kg) pr. mand/pr. enhed",
        "ADR-tilæg (Fagligt gods) pr. forsendelese", "Aften- og nattillæg (18:00-06:00) pr. forsendelse",
        "Weekendtillæg (lørdag-søndag) pr. forsendelse", "Yderzonetillæg beregnes af nettofragt",
        "Byttepalletillæg (franko/ufranko) pr. Palle", "SMS servicetillæg pr. advisering",
        "Adresse korrektion pr. forsendelse",
        "HEADER","Brændstofgebyr Beregnes af nettofragt",
        "Miljagebyr beregnes af nettofragt", "Adminnistrationsgebyr pr. faktura"
    );
    $kurerTransportHead = array("Vogn Beskrivelse", "Nettofragt", "Tid/ minut", "Tillæg for særlige ydelser", "Gebyr");
    
    $kurerExt = array("cm", "cm", "",
        "m&sup2", "m&sup3", "kr", 
        "kr <br><span style='font-size: 15px;'>(inkl. 20min. læssetid)</span>",
        "kr", "kr", "kr", "kr", "kr", "kr", "kr", "kr", "%", "kr",
        "kr", "kr", "%", "%", "kr");

    
    $kurerRows = $database->GetResults("SELECT * FROM `{$database->prefix}PriceKurer` WHERE `type` = '{$setting['sql']}';",array_I);
    
    
    echo "<table class='tecKurerTable'>"
    . "<tr>"
            . "<th rowspan='2'><img class='logo' src='{$_URL}images/logo.png'/ style='float: inherit;'></th>"
            . "<td colspan='4' style='text-align: center;'>"
                . "<span style='font-size: 1.50em; font-weight: bold;'>Kurertransport</span> "
                . "<span style='font-weight: bold;'>Prisliste DK Domestic</span>"
                . "<br>"
                . "<i>Ekskl. moms ekskl. broafgift og færgebillet.</i>"
            . "</td>"
        . "</tr>"
        . "<tr>"
            . "<th><img class='car' src='{$kurerRows[0][1]}'/></th>"
            . "<th><img class='car' src='{$kurerRows[1][1]}'/></th>"
            . "<th><img class='car' src='{$kurerRows[2][1]}'/></th>"
            . "<th><img class='car' src='{$kurerRows[3][1]}'/></th>"
        . "</tr>"
        . "<tr>"
            . "<th colspan='5'>Last / Vægt</th>"
        . "</tr>"
        . "<tr>"
            . "<td>"
                . "Max vægt pr.enhed pr. mand"
                . "<br>"
                . "Last indtil(vægt)"
            . "</td>"
            . "<th>"
                    . "{$kurerRows[0][2]}"
                    . "<br>"
                    . "<img class='weight' src='{$kurerRows[0][3]}' />"
                    . "<br>"
                    . "{$kurerRows[0][4]}"
            . "</th>"
                            
            . "<th>"
                    . "{$kurerRows[1][2]}"
                    . "<br>"
                    . "<img class='weight' src='{$kurerRows[1][3]}' />"
                    . "<br>"
                    . "{$kurerRows[1][4]}"
            . "</th>"
            . "<th>"
                    . "{$kurerRows[2][2]}"
                    . "<br>"
                    . "<img class='weight' src='{$kurerRows[2][3]}' />"
                    . "<br>"
                    . "{$kurerRows[2][4]}"
            . "</th>"
            . "<th>"
                    . "{$kurerRows[3][2]}"
                    . "<br>"
                    . "<img class='weight' src='{$kurerRows[3][3]}' />"
                    . "<br>"
                    . "{$kurerRows[3][4]}"
            . "</th>"
        . "</tr>";
    
    
    $headerId = 0;
    $sqlId = 5;
    $kurerExtId = 0;
    for ($i = 0; $i < count($kurerTransportText); $i++) {
        
        if($kurerTransportText[$i] === "HEADER"){
            
            echo "<tr>"
                    . "<th colspan='5'>$kurerTransportHead[$headerId]</th>"
                . "</tr>";
            
            $headerId++;
        }
        else{
            
            $priceExt = $kurerExt[$kurerExtId];
            
            echo "<tr>"
                    . "<td>$kurerTransportText[$i]</td>";
            
                    for ($a  = 0; $a < 4; $a++) {
                        
                        $priceVal = $kurerRows[$a][$sqlId];
                        
                        if($priceVal == "" || $priceVal == "-"){
                            $priceVal = "-";
                            $priceExt = "";
                                    
                        }
                        else{
                            
                            $priceExt = $kurerExt[$kurerExtId];;
                        }
                        
                        echo "<td>$priceVal $priceExt</td>";
                    }
            echo "</tr>";
                    
            $sqlId++;
            $kurerExtId++;
        }
    }
    
    echo "</table>";
    }
    
    
    private function GetType($type){
        if($type == "goflex"){
            
            $sqlType = "flex";
            $title = "GoFlex";
            
        }
        else{
            
            $sqlType = "rush";
            $title = "GoRush";
        }
        
        return array("sql" => $sqlType, "title" => $title);
    }
}