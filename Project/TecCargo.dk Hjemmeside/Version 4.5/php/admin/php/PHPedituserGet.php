<?php

    include_once '../../config.php';
    getUserRole(TRUE, TRUE);
    //echo getUserRole(FALSE,FALSE);
    
    //tjekker om der er et bruger navn til logs
    //og om de nødvendige bruger settings er sat
    if(isset($_SESSION['myusername']) && isset($_GET['id'])) {
        $id = $_GET['id'];
    
        //henter bruger oplysninger fra databasen
        $sqlConnect = mysqli_connect(DB_host, DB_username, DB_password, DB_database);
        $sqlcmd = "SELECT username,firstname,lastname,email,rank FROM members WHERE id=$id";
        $result = mysqli_query($sqlConnect, $sqlcmd);
         


        //hvis det ikke er gået ud igennem til database skriv i log
        if($result){
            $row = mysqli_fetch_array($result);

            //bruger
            $username = $row[0];
            $firstname = $row[1];
            $lastname = $row[2];
            $email = $row[3];
            $rank = $row[4];
            $status = "OK";
        }
        else {
            $admin = $_SESSION['myusername'];
            writeToLog("Kunne ikke hente bruger oplysninger fra databasen.".PHP_EOL."Admin: $admin".PHP_EOL."SQL: $sqlcmd");
            
            $status = "failed 2";            
        }

    }
    else{
        
        //skiver til log
        writeToLog("variabler ikke udflydt.");
        
        $status = "failed 1"; 
    }
    
    
    echo json_encode(array("id" => $id, "username" => $username ,"firstname" => $firstname ,"lastname" => $lastname ,
        "email" => $email ,"rank" => $rank ,"status" => $status));