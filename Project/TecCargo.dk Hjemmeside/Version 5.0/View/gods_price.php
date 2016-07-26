<?php


class gods_price extends pageSettings{
    
    public function __construct() {
        global $_URL,$_SCRIPTFILES,$_STYLEFILES;
        
        $_STYLEFILES[] = $_URL ."style/view_custom.css";
        $_SCRIPTFILES[] = $_URL."script/prices.js";
        $_STYLEFILES[] = $_URL."style/prices.css";
        
        $this->onlyOneFunc = true;
        $this->defaultFunc = "Godstransport";
    }
    
    public function Godstransport($param = null){
        global $database;
        
        if(is_array($param) && ctype_digit($param[0]) &&
                $param[0] >= 0 && $param[0] <=2){
            
            $type = $param[0];
        }
        else{
            
            $type = 0;
        }
        
        echo "<div class='customCenter'>";
        
        $this->Menu($type);
        
        switch ($type) {
            case 0:
                
                $this->pricetable();
                break;

            case 1:
                $this->zoneTable();
                break;
            
            case 2:
                
                $this->title = "Godstransport Beregning";
                
                $contentRow = $database->GetRow("SELECT `content` FROM `{$database->prefix}Pages` WHERE `pageId` = 16;");
                echo "<div class='defaultPageSettings'>". $contentRow->content."</div>";
                break;
        }
        
        echo "</div>";
    }
    
    private function Menu($type) {
        global $_URL;
        
        $titles = array("Takstzoner prisliste<br>Danmark excl moms","Takst Zoner","Godstransport");
        $selected = array(
            array("","class='seleced'",""),
            array("class='seleced'","",""),
            array("","","class='seleced'")
        );
        ?>

<table class="pricesDivGods">
    <tr>
        <td style="text-align: left;"><img src="<?php echo $_URL."images/logo.png"; ?>"></td>
        <td style="text-align: center;" class="header"><?php echo $titles[$type]; ?></td>
        <td style="text-align: right;">
            <div class="buttons">
                <a href="<?php echo $_URL."gods_price/1/"; ?>" <?php echo $selected[$type][0]; ?>>Takst Zoner</a>
                <a href="<?php echo $_URL."gods_price/0/"; ?>" <?php echo $selected[$type][1]; ?>>Prisliste</a>
                <a href="<?php echo $_URL."gods_price/2/"; ?>" <?php echo $selected[$type][2]; ?>>Beregning af ladmeter</a>
            </div>
        </td>
    </tr>
</table>
<?php
    }


    private function pricetable() {
        global $database;
        
        $this->title = "Godstransport Priser";
        
        $kgform = array("FIRST","30 kg","40 kg","50 kg","75 kg","100 kg","125 kg","150 kg","175 kg",
            "200 kg","250 kg","300 kg","350 kg","400 kg","450 kg","500 kg","600 kg","700 kg",
            "800 kg","900 kg","1.000 kg","NEW","1.100 kg","1.200 kg","1.300 kg","1.400 kg","1.500 kg",
            "1.600 kg","1.700 kg","1.800 kg","1.900 kg","2.000 kg","2.100 kg","2.200 kg","2.300 kg",
            "2.400 kg","2.500 kg","2.600 kg","2.700 kg","2.800 kg","2.900 kg","3.000 kg");
        
        $headerText = array("","Takst 1","Takst 2","Takst 3","Takst 4",
            "Takst 5","Takst 6","Takst 7","Takst 8","Takst 9","Takst 10");
        
        
        $startPris = $database->GetResults("SELECT `startPris`, `takstProcent` FROM `{$database->prefix}PriceGodsPart` ORDER BY `id` ASC;");
        $takstProcent = $database->GetResults("SELECT * FROM `{$database->prefix}ProcentGodsPart`;",array_I);
        
        $sqlCount = 0;
        $changeClass = true;
        echo "<table class='tecGodsPrice'>";
        foreach($kgform as $kilo){
            
            if($kilo == "NEW"){
                echo "</table><table class='tecGodsPrice'>";
                $changeClass = true;
            }
            
            $priceClass = $changeClass ? "light" : "dark";
            $changeClass = !$changeClass;
            
            echo "<tr>";
            
            for($a = 0; $a <= 10; $a++) {
                
                if($kilo == "NEW" || $kilo == "FIRST"){
                    echo "<th class='header'>$headerText[$a]</th>";
                }
                else{
                    if($a == 0){
                        $text = $kilo;
                    }
                    else{
                        
                        $procent = 0;
                        
                        if($a != 1){
                            $procent = $takstProcent[$startPris[$sqlCount]->takstProcent -1][$a-1];
                            $procent = $procent / 100;
                        }
                        
                        $price = ($startPris[$sqlCount]->startPris * $procent) + $startPris[$sqlCount]->startPris;
                        $text = number_format($price, 2, ',', '.') ." kr";
                        
                       
                    }
                    
                    echo "<td class='$priceClass mark'>$text</td>";
                }
            }
            
            
            
            echo "</tr>";
            
            
            if($kilo != "FIRST" && $kilo != "NEW"){
                $sqlCount++;
                
            }
        }
        
        echo '</table>';
        
        echo "<p style='padding: 15px 0;color: #f73700; text-align: center;'>For øer uden selvstændigt postnr. og uden fast broforbindelse gælder taksterne kun til afskibningssted.</p>";
        
    }
    
    private function zoneTable() {
        
        $this->title = "Takst Zoner";
        
        $takstValues = array(
            array(0 ,"","<b>Hvidovre</b>","<b>Rønne</b>","<b>Hvidovre</b>","<b>Hvidovre</b>",
                "<b>Odense</b>","<b>Kolding</b>","<b>Herning</b>","<b>Århus</b>","<b>Aalborg</b>"),
            array(0 ,"Takster","Betjener<br>Nordøstlige<br>Sjælland","Betjener kun<br>Bornholm",
                "Betjener<br>sydvestlige<br>Sjælland","Betjener<br>Lolland<br>Falster og<br>Møn",
                "Betjener<br>Fyn og<br>omkringligge<br>nde øer","Betjener<br>Sydjylland",
                "Betjener<br>det vestlige<br>Midjylland","Betjener<br>det østlige<br>Midjylland",
                "Betjener<br>Nordjylland"),
            array(0 ,"","1000-3699","3700-3799","4000-4799","4800-4999","5000-5999",
                "6000-6999","7000-7999","8000-8999","9000-9999"),
            array(1 ,"<b>Hvidovre</b><br>1000-3699","1","6","2","3","5","6","8","8","9"),
            array(2 ,"<b>Rønne</b><br>3700-3799","6","1","6","6","8","8","9","9","10"),
            array(1 ,"<b>Hvidovre</b><br>4000-4799","2","6","1","3","3","4","6","6","8"),
            array(2 ,"<b>Hvidovre</b><br>4800-4999","3","6","3","1","5","6","8","8","9"),
            array(1 ,"<b>Odense</b><br>5000-5999","5","8","3","5","1","2","4","4","7"),
            array(2 ,"<b>Kolding</b><br>6000-6999","6","8","4","6","2","1","2","2","6"),
            array(1 ,"<b>Herning</b><br>7000-7999","8","9","6","8","4","2","1","2","3"),
            array(2 ,"<b>Århus</b><br>8000-8999","8","9","6","8","4","2","2","1","3"),
            array(1 ,"<b>Aalborg</b><br>9000-9999","9","10","8","9","7","6","3","3","1")
        );
        
        $takstColor = array("takstColorBlue","takstColorDark","takstColorLight");
        ?>
<style>
    .takstTable{
        width: 100%;
        border-collapse: collapse;
    }
    .takstTable td{
        text-align: center;
    }
    .takstTable .takstColorBlue{
        background: #254078;
        color: #fff;
    }
    .takstTable .takstColorDark{
        background: #B1B2B4;
        color: #000;
    }
    .takstTable .takstColorLight{
        background: #fff;
        color: #000;
    }
</style>
        <?php
        
        echo "<table class='takstTable'>";
        
        foreach ($takstValues as $takst) {
            
            echo "<tr class='{$takstColor[$takst[0]]}'>";
            
            for ($i = 1; $i < count($takst); $i++) {
                echo "<td>$takst[$i]</td>";
            }
            
            echo "</tr>";
        }
        
        echo "</table>";
    }
}