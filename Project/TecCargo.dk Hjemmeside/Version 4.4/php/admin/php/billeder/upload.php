<?php

$backLink = "location:../../sider/billede-upload.php";
session_start();

if($_FILES['upload_file']['error'] > 0)
{
    $_SESSION['Error'] = 'Error: ' .$_FILES['upload_file']['error'];
}
else
{
    $_SESSION['Upload'] = 'Upload: ' .$_FILES['upload_file']['name'] .'<br>';
    $_SESSION['Type'] = 'Type: ' .$_FILES['upload_file']['type'] .'<br>';
    $_SESSION['Size'] = 'Size: ' .$_FILES['upload_file']['size'] / 1024 .' kb<br>';
    
    
    if(file_exists("../../../../images/" .$_FILES['upload_file']['name']))
    {
        $_SESSION['Info'] = 'File exists';
    }
    else
    {
        move_uploaded_file($_FILES['upload_file']['tmp_name'], "../../../../images/" .$_FILES['upload_file']['name']);
        $_SESSION['Info'] = 'file uploaded';
    }
    
    
}

header($backLink);