<?php

    include_once '../../config.php';
    getUserRole(TRUE, TRUE);
    //echo getUserRole(FALSE,FALSE);
    
    //tjekker om der er et bruger navn til logs
    //og om de nødvendige bruger settings er sat
    if(!isset($_SESSION['myusername']) || !isset($_POST['username']) || 
    !isset($_POST['password']) || !isset($_POST['passwordC'])) {
        
        echo "Error: 1";
        
        //skiver til log
        writeToLog("variabler ikke udflydt.");
        
        header("location:" .Teccargo_url);
        exit();
    }

    //henter info
    $logCreatorUser = $_SESSION['myusername'];
    $username = $_POST['username'];
    $password = $_POST['password'];
    $passwordC = $_POST['passwordC'];
    $rank = $_POST['rank'];
    $name = $_POST['name'];
    $lastname = $_POST['lastname'];
    $email = $_POST['email'];
    
    
    //tjekker om kodeordne er ens
    if($password != $passwordC) {
        //echo "Kodeordene skal være ens.";
        echo "Error: 2";
        exit();
    }
    
    //tjekker om kodeordet er længere end 6 tegn
    if(strlen($password) < 6) {
        
        echo "Error: 3";
        exit();
    }
    
    //tjekker username findes i databasen der står noget i de andre variabler
    $sqlConnect = mysqli_connect(DB_host, DB_username, DB_password, DB_database);
    $sqlcmd = "SELECT username FROM members WHERE username='$username'";
    $result = mysqli_query($sqlConnect, $sqlcmd);
    $usernameCount = mysqli_num_rows($result);
    
    if($usernameCount != 0) {
        
        echo "Error: 4";
        exit();
    }
    
    //tjekker om email findes
    if(!empty($email)) {
        $sqlcmd = "SELECT email FROM members WHERE email='$email'";
        $result = mysqli_query($sqlConnect, $sqlcmd);
        $emailCount = mysqli_num_rows($result);
        
        if($emailCount != 0) {
        echo "Error: 5";
        exit();
        }
    }
    
    
    //encrypt kodeord
    $hash = hash('sha256', $password);
    $salt = createSalt();
    $encryptPassword = hash('sha256', $salt . $hash);
    
    //upload til databasen
    $sqlcmd = "INSERT INTO members(username,password,passwordC,firstname,lastname,email,rank) "
            . "VALUES('$username','$encryptPassword','$salt','$name','$lastname','$email','$rank');";
    $result = mysqli_query($sqlConnect, $sqlcmd);
    
    mysqli_close($sqlConnect);
    
    //om det er gået igennem uden fejl
    if($result) {
        //skiver til log
        writeToLog("Brugern $username er nu oprettet." .PHP_EOL ."Admin: $logCreatorUser");
    }
    else {
        
        echo "Error: 6";
        //skiver til log
        writeToLog("Kunne ikke oprette bruger." .PHP_EOL ."Admin: $logCreatorUser" .PHP_EOL ."SQL: $sqlcmd");
    }
    
function createSalt()
{
    $saltString = md5(uniqid(rand(),TRUE));
    return substr($saltString, 2, 30);
}