<?php

        $startPost = $_POST['startPost'];
        $slutPost = $_POST['slutPost'];
        $kilo = $_POST['kilo'];
        
        //formatere til engelske komma regler
        $kilo = str_replace(",", ".", $kilo);
        
        if($startPost == 0 || $slutPost == 0 || !is_numeric($kilo))
        {
            $errormsg = "<label class='errormsg'><b>Fejl:</b></label><br>";
            if($startPost == 0)
            {
                $errormsg .="<label class='errormsg'>Vælg et start post nr.</label><br>";
            }
            if($slutPost == 0)
            {
                $errormsg .="<label class='errormsg'>Vælg et slut post nr.</label><br>";
            }
            if(!is_numeric($kilo))
            {
                $errormsg .="<label class='errormsg'>Skriv et tal i antal kilo</label>";
            }
            echo $errormsg;
        }
        else
        {
            findpris();
        }
            
        
    function findpris()
    {
        global $startPost,$slutPost,$kilo;
        
        include '../../../config.php';
        
        $sqlConnect = mysqli_connect(DB_host, DB_username, DB_password, DB_database);
        
        //finder antal kilometer fra databasen og gemmer det i en variable
        $sqlcmd_find_kilometer = "SELECT `" .$slutPost ."` FROM  `beregnPris_kilometer` WHERE id=" .$startPost;
        $resultKilomter = mysqli_query($sqlConnect, $sqlcmd_find_kilometer);
        $kilometer = mysqli_fetch_array($resultKilomter);
         
        //finder antal takster fra databasen og gemmer det i en variable
        $sqlcmd_find_takst = "SELECT `" .$slutPost ."` FROM  `beregnPris_takster` WHERE id=" .$startPost;
        $resulttakst = mysqli_query($sqlConnect, $sqlcmd_find_takst);
        $takster = mysqli_fetch_array($resulttakst);
        
        
        //alle kiloerne
        $kiloListArray = array(30, 32, 40, 75, 100,125, 150, 175, 200, 250,300, 350, 400, 450,
            500,600, 700, 800, 900, 1000,1100, 1200, 1300, 1400, 1500,1600, 1700, 1800,
            1900, 2000,2100, 2200, 2300, 2400, 2500,2600, 2700, 2800, 2900, 3000);
        
        //variabler til at finde procenten i databasen
        $id = 1;
        $a = 5;
        for ($i = 0;$i < 40; $i++)
        {
            if ($kilo <= $kiloListArray[$i])
            {
                //finder prisen for takst 1 som skal bruges til at beregne procenten
                $sqlcmd_find_pris_kg = "SELECT `kg` FROM  `prisliste_kg` WHERE id=" .($i +1);
                //procenten
                $sqlcmd_find_pris = "SELECT `Takst" .$takster[0] ."` FROM  `prisliste_takst` WHERE id=" .$id;
                break;
            }
            //hver gang den har kørt fem gange igennem vil den komme ind her
            if($i > $a)
            {
                $id++;
                $a += 5;
            }
        }
        //find prisen
        if($takster[0] == 1)
        {
            $resultPrice = mysqli_query($sqlConnect, $sqlcmd_find_pris_kg);
            $rowPrice = mysqli_fetch_array($resultPrice);

            $prisen = number_format($rowPrice[0],2,",",".");
        }
        else 
        {
            $resultPrice = mysqli_query($sqlConnect, $sqlcmd_find_pris_kg);
            $rowPrice = mysqli_fetch_array($resultPrice);

            $resultProcent = mysqli_query($sqlConnect, $sqlcmd_find_pris);
            $rowProcent = mysqli_fetch_array($resultProcent);

            $prisen = number_format((($rowPrice[0] / 100) * $rowProcent[0]) + $rowPrice[0],2,",",".");
        }
        
        $table = "<table class='beregntable'><tr>";
        $table .= "<th>Pris</th>";
        $table .= "<td>" .$prisen ." kr</td>";
        $table .= "</tr><tr>";
        $table .= "<th>Antal Takster</th>";
        $table .= "<td>" .$takster[0] ."</td>";
        $table .= "</tr><tr>";
        $table .= "<th>Antal kilometer</th>";
        $table .= "<td>" .$kilometer[0] ." km</td>";
        $table .= "</tr></table>";
        echo $table;
    }