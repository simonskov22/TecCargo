<?php
include '../../../config.php';

$backLink = "location:../../../admin.php";

//echo "navn: " .$_POST['navn'] ."<br> efternavn: " .$_POST['efternavn']  ."<br> bruger: " .$_POST['brugernavn'] ."<br> kode: " .$_POST['kodeord'] ."<br> type: " .$_POST['type'];

$save_navn = $_POST['name'];
$save_efternavn = $_POST['lastname'];
$save_brugernavn = $_POST['username'];
$save_password = $_POST['password'];
$save_role = $_POST['type'];

//tjekker om felterne er udfyldte
if ($save_navn == "" || $save_efternavn == "" || $save_brugernavn == "" || $save_password == "")
{
   $message = "<b>Fejl:</b><br><br>Kan ikke oprette bruger.<br>Alle felterne skal være udfyldt";
}
else
{
    //mysql connect
    $sqlConnect = mysqli_connect(DB_host, DB_username, DB_password, DB_database);
    //sql command`
    $sqlcmd ="INSERT INTO user (`Username`,`Password`,`name`,`last name`,`role`) VALUES('" .$save_brugernavn ."','" .$save_password ."','" .$save_navn ."','" .$save_efternavn ."','" .$save_role."')";
    //echo $sqlcmd;

    if (!mysqli_query($sqlConnect, $sqlcmd)) 
    {
        $message="<b>Fejl:</b><br><br> Brugeren blev ikke oprettet i databasen.";
    }
    else
    {
        $message = "<b>Godkendt:</b><br><br>Brugeren er blivet oprettet i databasen.";
    }
}

mysqli_close($sqlConnect);

session_start();
$_SESSION['Create'] = $message;

header($backLink);