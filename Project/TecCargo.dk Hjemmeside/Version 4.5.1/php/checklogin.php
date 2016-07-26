<?php

include_once "config.php";

// Define $myusername and $mypassword 
 $myusername=$_POST['login_username']; 
 $mypassword=$_POST['login_password'];
  
 // To protect MySQL injection (more detail about MySQL injection)
 $myusername = stripslashes($myusername);
 $mypassword = stripslashes($mypassword);
 //$myusername = mysqli_real_escape_string($myusername);
 //$mypassword = mysqli_real_escape_string($mypassword);

//teccargo.dk
$sql="SELECT password,passwordC,rank FROM members WHERE username='".$myusername."';";

$result = databaseQuery($sql);


$count=mysqli_num_rows($result);

//echo "navn = ".$myusername . " pass= " .$mypassword. " count= ".$count;


if(($count == 1))
{
    
    
    $row = mysqli_fetch_array($result);
    $dbPassword = $row[0];
    $dbsalt = $row[1];
    $myrole = $row[2];
    
    
    //encrypt kodeord
    $hash = hash('sha256', $mypassword);
    $decryptPassword = hash('sha256', $dbsalt . $hash);
    
    //echo "$dbPassword <br> $decryptPassword ";
    
    if ($dbPassword == $decryptPassword) {    
        //echo "login ok";
        session_start();
        $_SESSION['myusername']=$myusername;
        $_SESSION['myrole']=$myrole;

        header("location:mainindex.php");
    }
    else
    {
        //echo "login ikke ok";
        header("location:error.php");
    }
} 
else
{
    //echo "login ikke ok 2";
    header("location:error.php");
}