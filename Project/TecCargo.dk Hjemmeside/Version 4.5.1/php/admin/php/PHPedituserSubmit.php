<?php
    include_once '../../config.php';
    getUserRole(TRUE, TRUE);
    
    $table = "members";
    
    
    //og om de nødvendige bruger settings er sat
    if(!isset($_SESSION['myusername']) || !isset($_POST['username']) || !isset($_POST['id']) ||
        empty($_SESSION['myusername']) || empty($_POST['username']) || empty($_POST['id'])){
        
        echo "Error: 1 ";
        
        //skiver til log
        writeToLog("variabler ikke udflydt.");
        
        header("location:" .Teccargo_url);
        exit();
    }
    
    $logCreatorUser = $_SESSION['myusername'];
    $id = $_POST['id'];
    $username = $_POST['username'];
    $password = $_POST['password'];
    $passwordC = $_POST['passwordC'];
    $firstname = $_POST['firstname'];
    $lastname = $_POST['lastname'];
    $email = $_POST['email'];
    $rank = $_POST['rank'];

    //tjekker om denne bruger er admin
    //og hvis brugeren er den sidste admin
    //kan man ikke skifte rank på ham
    $result = databaseSelect("rank", $table, "WHERE id=$id");
    $row = mysqli_fetch_array($result);
    
    if($row['rank'] == "Admin" && $rank != "Admin"){
        $result = databaseSelect("rank", $table, "WHERE rank='Admin'");
        $count = mysqli_num_rows($result);
        
        if($count == 1){
            echo 'Error: 2';
            exit();
        }
    }
    
    //hvis bruger skifter navn
    //så må det nye brugernavn ikke findes i databasen
    $result = databaseSelect("username", $table, "WHERE id=$id");
    $row = mysqli_fetch_array($result);
    
    if($row['username'] != $username){
        $result = databaseSelect("*", $table, "WHERE username='$username'");
        $count = mysqli_num_rows($result);
        
        if($count != 0){
        echo "Error: 3";
            exit();
        }
    }
    
    //hvis kodeordne er sat, tjek om de er ens
    //og om de indeholder 6 eller flere tegn
    $allowPassChange = "";
    
    if(!empty($password) && !empty($passwordC)){
        if(strlen($password) < 6){
            echo "Error: 4";
            exit();
        }
        else if($password != $passwordC){
            echo "Error: 5";
            exit();
        }
        //laver encrypt på kodeordet
        else{
            $hash = hash("sha256", $password);
            $salt = createSalt();
            
            $encryptPassword = hash("sha256", $salt .$hash);
            $allowPassChange = "password='$encryptPassword',passwordC='$salt',";
        }
    }
    
    
    
    
    $sqlConnect = mysqli_connect(DB_host, DB_username, DB_password, DB_database);
    $sqlcmd = "UPDATE $table SET username='$username',$allowPassChange firstname='$firstname',lastname='$lastname',email='$email',rank='$rank' WHERE id=$id";
    //echo $sqlcmd;
    $result = mysqli_query($sqlConnect, $sqlcmd);
    
    //tjekker om det er gået igennem
    if($result){
        //skiver til log
        writeToLog("Brugeren $username er nu opdaterer." .PHP_EOL ."Admin: $logCreatorUser");
    }
    else{
        echo "Error: 6";
        
        //skriver til log
        writeToLog("Kunne ikke opdatere brugeren." .PHP_EOL ."Admin: $logCreatorUser" .PHP_EOL ."SQL: $sqlcmd");
    }
       
    function createSalt()
    {
        $saltString = md5(uniqid(rand(),TRUE));
        return substr($saltString, 2, 30);
    }
    