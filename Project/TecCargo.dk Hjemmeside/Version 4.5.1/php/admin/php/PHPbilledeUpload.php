<?php
    
    include_once '../../config.php';
    getUserRole(TRUE, TRUE);
    
    if (!isset($_SESSION['myusername'])){
        
        //skiver til log
        writeToLog("variabler ikke udflydt.");
        echo json_encode(array('Text' => 'Der skete en fejl. tjek logs.', 'Error' => true));
        
        exit();
    }

    $admin = $_SESSION['myusername'];
    $error = false;
    $files = "";

    $uploaddir = '../../../images/';
    foreach($_FILES as $file)
    {
        if ($file['type'] == "image/jpeg" || $file['type'] == "image/gif" || $file['type'] == "image/png") {
            
            //tjekker om filen findes
            if(!file_exists($uploaddir .basename($file['name']))) {
                
                //hvis den ikke kan flytte filen skal den sige der er en fejl
                if(!move_uploaded_file($file['tmp_name'], $uploaddir .basename($file['name'])))
                {
                    $errorText = $file['error'];
                    $error = true;
                }
            }
            else{
                    $errorText = "Filen findes.";
                    $error = true;
            }
        }
        else{
            $errorText = $file['error'];
            $error = true;
        }
            
        //oplysninger til log
        $files .= "Navn: " .$file['name'] .", Type: " .$file['type'] .", Bytes: " .$file['size'] .", Fejl Besked: " .$errorText.PHP_EOL;
    }
    
    //hvis der er fejl
    if ($error) {
        echo json_encode(array('Text' => 'Der skete en fejl. tjek logs.', 'Error' => $error));
        
        //skiver til log
        writeToLog("Der var nogle billeder der ikke kunne gemmes." .PHP_EOL ."Admin: $admin" .PHP_EOL ."Filer: --------------------------------------".PHP_EOL
                .$files .PHP_EOL ."--------------------------------------");
    }
    else{
        echo json_encode(array('Text' => 'Billederne er nu uploaded.', 'Error' => $error));
        //skiver til log
        writeToLog("Nye billeder." .PHP_EOL ."Admin: $admin" .PHP_EOL ."Filer: --------------------------------------".PHP_EOL
                .$files .PHP_EOL ."--------------------------------------");
        
    }
