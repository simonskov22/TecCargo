<?php

$backLink = "location:../../sider/billede-upload.php";
session_start();

if(isset($_POST['files']))
{
    $filename=$_POST['files'];
    $_SESSION['Delete'] = "";
    
    $filecount = count($filename);
    for($i = 0;$i < $filecount; $i++)
    {
        if(file_exists("../../../../images/" .$filename[$i]))
        {
            $_SESSION['Delete'] .= 'Slettet: ' .$filename[$i] .'<br>';
            unlink("../../../../images/" .$filename[$i]);
        }
        else
        {
            $_SESSION['Delete'] .=  'kunne ikke Slette: ' .$filename[$i] .'<br>';
        }
    }
}

header($backLink);
