<?php

$user = $_POST['user'];

//fjerner "userid_"
$userDone = substr($user, 7);

include '../../../config.php';

//database connect
$sqlConnect = mysqli_connect(DB_host, DB_username, DB_password, DB_database)or die('cannot connect');

//database command
$sqlcmd = "SELECT * FROM user WHERE Username='" .$userDone ."';";
$result = mysqli_query($sqlConnect, $sqlcmd);

$row = mysqli_fetch_array($result);

$userData = '<fieldset><legend>Person oplysninger</legend>';
$userData .= '<input type="text" placeholder="Fornavn" name="edit-user-name" value="' .$row[3] .'"><br>';
$userData .= '<input type="text" placeholder="Efternavn" name="edit-user-lastname" value="' .$row[4] .'"><br>';

$userData .= '</fieldset><fieldset><legend>Login oplysninger</legend>';
$userData .= '<input type="text" placeholder="Brugernavn" name="edit-user-username" value="' .$row[1] .'"><br>';
$userData .= '<input type="password" placeholder="Kodeord" name="edit-user-password"><br>';

if($row[5] == "Bruger")
{
    $userData .= '</fieldset><fieldset><legend>Bruger type</legend>';
    $userData .= '<input type="radio" value="Bruger" name="edit-user-type" id="create-radio-start" checked="true">Bruger';
    $userData .= '<input type="radio" value="Admin" name="edit-user-type">Admin<br></fieldset>';
}
else
{
    $userData .= '</fieldset><fieldset><legend>Bruger type</legend>';
    $userData .= '<input type="radio" value="Bruger" name="edit-user-type" id="create-radio-start">Bruger';
    $userData .= '<input type="radio" value="Admin" name="edit-user-type" checked="true">Admin<br></fieldset>';
}

echo $userData;