<?php

include "config.php";
//connect til database

$sqlConnect = mysqli_connect(DB_host,DB_username,DB_password,DB_database)or die("cannot connect");

// Define $myusername and $mypassword 
 $myusername=$_POST['login_username']; 
 $mypassword=$_POST['login_password'];
  
 // To protect MySQL injection (more detail about MySQL injection)
 $myusername = stripslashes($myusername);
 $mypassword = stripslashes($mypassword);
 //$myusername = mysqli_real_escape_string($myusername);
 //$mypassword = mysqli_real_escape_string($mypassword);

//teccargo.dk
$sql="SELECT * FROM user WHERE Username='".$myusername."' and Password='".$mypassword."';";

$result=mysqli_query($sqlConnect,$sql);

$count=mysqli_num_rows($result);

//echo "navn = ".$myusername . " pass= " .$mypassword. " count= ".$count;


if(($count == 1))
{
	$row = mysqli_fetch_array($result);
	$myname = $row[3];
	$mylastname = $row[4];
	$myrole = $row[5];
	//echo "login ok";
	session_start();
	$_SESSION['myusername']=$myusername;
	$_SESSION['myname']=$myname;
	$_SESSION['mylastname']=$mylastname;
	$_SESSION['myrole']=$myrole;

	header("location:mainindex.php");
} 
else
{
	//echo "login ikke ok";
	header("location:error.php");
}