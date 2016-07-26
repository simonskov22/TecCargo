<?php


class admin_calculator extends pageSettings{
    
    public function __construct() {
        global $_URL,$_SCRIPTFILES,$_STYLEFILES;

        $_SCRIPTFILES[] = $_URL."script/admin_calculator.js";
        $_STYLEFILES[] = $_URL."style/admin_calculator.css";

        $this->AdminOnly = true;
        $this->onlyOneFunc = TRUE;
        $this->defaultFunc = "calculator";
        $this->title = "Admin Begregn Zone";
    }
    
    private function PostCalculator(){
        global $database;
        
        $this->loadTemplate = false;

        $postValue = array($_POST['data'][0],$_POST['data'][1]);
        
        $kilo = $database->SQLReadyString($_POST['data'][2]);
        $sqlPostnumb = array();
        $status = true;

        

        //postnummer kategori
        $postArray =    array(1000 , 3700       , 3800         ,4000       ,4800      ,
            5000       , 6000      , 7000      , 8000      , 9000      , 10000);
        $postSqlArray = array(false, '1000-3699', '3700-3799', false ,'4000-4799','4800-4999',
            '5000-5999', '6000-6999', '7000-7999', '8000-8999', '9000-9999', false);
         
        //kilo kategori
        $kiloArray = array(0, 30,40,50,75,100,125,150,175,200,250,300,350,400,450,500,600,700,800,900,1000,
            1100,1200,1300,1400,1500,1600,1700,1800,1900,2000,2100,2200,2300,2400,2500,2600,2700,2800,2900,3000);

        
        //finder ud af hvad kategori postnummeret ligger i
        foreach ($postValue as $postnummer) {
            
            if (ctype_digit($postnummer)) {
                
                for ($i = 0; $i < count($postArray); $i++) {

                    if($postnummer < $postArray[$i]){
                        $lastQuery .= "i: $i :: $postnummer ::: $postArray[$i] :::: $postSqlArray[$i] <br>";
                        $sqlPostnumb[] = $postSqlArray[$i];
                        break;
                    }
                }
            }
            else{
                
                $sqlPostnumb[] = false;
            }
        }
        
        //hvis postnummerene er forkerte
        if(!$sqlPostnumb[0] || !$sqlPostnumb[1]){
            
            $status = false;
        }
        
        
        if($status){
            
            //henter takst zone fra database
            $zoneRow = $database->GetRow("SELECT `$sqlPostnumb[0]` FROM `{$database->prefix}CalculateZone` WHERE `post` = '$sqlPostnumb[1]';", array_I);

            $lastQuery .= $database->lastQuery ."<br>";
            $id = 0;

            for($i = 0; $i < count($kiloArray) -1; $i++){

                if ($kilo > $kiloArray[$i] && $kilo <= $kiloArray[$i +1]) {

                    $id = ($i +1);
                    break;
                }
            }

            //hent start pris og takst procent zone
            $startpriceRow = $database->GetRow("SELECT `startPris`,`takstProcent` FROM `{$database->prefix}PriceGodsPart` WHERE `id` = $id;");
            $startprice = doubleval($startpriceRow->startPris);

            //hvis takst zone ikke er 1 
            if($zoneRow[0] != 1){

                $zoneProcentRow = $database->GetRow("SELECT `Takst$zoneRow[0]` FROM `{$database->prefix}ProcentGodsPart` WHERE `id` = $startpriceRow->takstProcent;", array_I);
                $lastQuery .= $database->lastQuery ." -- -- <br>";

                //gang start prisen med procent
                $procent = doubleval($zoneProcentRow[0]);
                $startprice = (($startprice * ($procent / 100)) + $startprice);
            }

            $result = number_format($startprice, 2 ,',','.') . "kr.";
        }

        echo json_encode(array("status" => $status,"result" => $result));
        
        die();

    }

    public function calculator(){
        global $_URL;
        
        if(isset($_POST['action']) && $_POST['action'] == "calculator"){
            $this->PostCalculator();
        }
        
        ?>
    <div class="calculaterBox">
        <div class="calculaterHeader"><h2>Beregn Pris</h2></div>
        <h4 class="helpText">
            <span id="calculaterStatus_1">Start postnummer</span> + 
            <span id="calculaterStatus_2">Slut postnummer</span> + 
            <span id="calculaterStatus_3">Kilo</span></h4>
        <p id="calculator_input" class="input" contentEditable="true"></p>
        <table class="buttons">
            <tr>
                <td><button onclick="calculator_addvalue('7');">7</button></td>
                <td><button onclick="calculator_addvalue('8');">8</button></td>
                <td><button onclick="calculator_addvalue('9');">9</button></td>
                <td>
                    <button onclick="calculator_back();" class="clear">&#65513;</button>
                    <button onclick="calculator_clear();" class="clear">C</button>
                </td>
            </tr>
            <tr>
                <td><button onclick="calculator_addvalue('4');">4</button></td>
                <td><button onclick="calculator_addvalue('5');">5</button></td>
                <td><button onclick="calculator_addvalue('6');">6</button></td>
                <td><button onclick="calculator_addvalue('+');" class="plus">+</button></td>
            </tr>
            <tr>
                <td><button onclick="calculator_addvalue('1');">1</button></td>
                <td><button onclick="calculator_addvalue('2');">2</button></td>
                <td><button onclick="calculator_addvalue('3');">3</button></td>
                <td rowspan="2"><button onclick="calculator_submit();" class="enter" style="height: 82px;">=</button></td>
            </tr>
            
            <tr>
                <td colspan="2"><button onclick="calculator_addvalue('0');" style="width:100%;">0</button></td>
                <td><button onclick="calculator_addvalue(',');">,</button></td>
            </tr>
        </table>
        <div class="beregnKilo" style="display: none;">
            <input id="post_kilo" class="kiloInput" type="text">
            <button id="post_kiloSubmit" class="begregnkiloSubmit" onclick="tecCalculator_Submit('<?php echo $_URL."admin_calculator/jquery/zone/"; ?>');" style="margin: 4px 10px 4px;">Enter</button>
        </div>
    </div>
<?php
    }
    
}
