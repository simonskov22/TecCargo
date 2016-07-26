<?php


class pakke_price extends pageSettings{
    
    public function __construct() {
        
        $this->onlyOneFunc = true;
        $this->defaultFunc = "Pakke";
    }
    
    public function Pakke($param = null){
        
        if(!is_array($param)){
            $param[] = 'goplus';
        }
        
        $setting = $this->GetType($param[0]);
        
        $this->pakkePrice($setting);
    }
    
    private function pakkePrice($setting) {
        global $_URL, $database, $_STYLEFILES;
        $_STYLEFILES[] = $_URL."style/prices.css";
        
        
        $textVal = array(
            array(30,330,"2,5"),
            array(40,330,"5,0"),
            array(50,330,"9,0"),
            array(60,330,"13,0"),
            array(70,330,"17,0"),
            array(80,330,"21,0"),
            array(245,330,"25,0")
        );
        
        
        $pakkeRows = $database->GetRow("SELECT * FROM `{$database->prefix}PricePakke` WHERE type= '{$setting['sql']}';",array_I);
        
        
        
        
        echo "<table class='tecKurerTable'>"
                . "<tr>"
                    . "<th rowspan='2' style='width: 360px;'><img class='logo' style='float: inherit;' src='{$_URL}images/logo.png'/></th>"
                    . "<td colspan='4' style='text-align: center;'>"
                        . "<span style='font-size: 1.50em; font-weight: bold;'>Pakketransport</span> "
                        . "<span style='font-weight: bold;'>Prisliste DK Domestic</span>"
                        . "<br>"
                        . "<i>Ekskl. moms.</i>"
                    . "</td>"
                . "</tr>"
                . "<tr>"
                    . "<td  colspan='4'>"
                            . "Prisen afhænger af pakkens størrelse. Der er i alt syv priskategorier."
                            . " For at finde prisen, skal<br>du måle den lægste og den korsteste side af pakken og lægge de to tal sammen."
                            . " Summen afgør, om pakken er XS, S, M, L, Xl, 2XL eller 3XL."
                            . "<br>"
                            . "<br>"
                            . "<b>"
                            . "TECCARgo accepterer ikke enheder i deres pakketransport,"
                            . "<br>"
                            . "der overskrider følgende mål og vægt:"
                            . "<br>"
                            . "</b>"
                            . "<i>"
                                . "(Længde maksimal. 180cm)"
                                . "<br>"
                                . "(Længden +  den største omkreds målt i en anden retning end længden maksimal. 330 cm)"
                                . "<br>"
                                . "(Fysisk brutto vægt maksimal 25 kg)"
                            . "</i>"
                            . "</td>"
                . "</tr>"
                . "<tr class='centerText'>"
                    . "<td>Pakkens<br>Størrelse</td>"
                    . "<td>Lægste +<br>Korteste side</td>"
                    . "<td>Længde +<br>Omkreds</td>"
                    . "<td>Fysisk Vægt<br>(Brutti)</td>"
                    . "<td style='background: #B1B2B4;'><b>{$setting['title']}</b><br>Priser</td>"
                . "</tr>";
                    
            $sqlId = 2;
            
            $imgesList = array("xs-pakke.png","s-pakke.png","m-pakke.png","l-pakke.png","xl-pakke.png","2xl-pakke.png","3xl-pakke.png","metric.jpg");
            for($i = 0; $i < count($textVal); $i++) {

                echo "<tr class='centerText'>"
                        . "<td><img style='width: 350px;' src='{$_URL}images/{$imgesList[$i]}'/></td>"
                        . "<td>maks. {$textVal[$i][0]} cm</td>"
                        . "<td>maks. {$textVal[$i][1]} cm</td>"
                        . "<td>maks. {$textVal[$i][2]} kg</td>"
                        . "<td>{$pakkeRows[$sqlId]} kr</td>"
                    . "</tr>";

                $sqlId++;
            }
            
            echo "<tr class='centerText'>"
                    . "<td colspan='3'><img style='width: 620px;height: 150px;' src='{$_URL}images/$imgesList[7]'/></td>"
                    . "<td>Servicegebyr<br>pr. pakke</td>"
                    . "<td>{$pakkeRows[$sqlId]}</td>"
                . "</tr>";
                    
        echo "</table>";
    }
    
    private function GetType($type){
        if($type == "gogreen"){
            
            $sqlType = "green";
            $title = "GoGreen";
            
        }
        else{
            
            $sqlType = "plus";
            $title = "GoPlus";
        }
        
        return array("sql" => $sqlType, "title" => $title);
    }
}