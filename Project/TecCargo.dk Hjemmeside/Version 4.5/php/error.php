<?php 
// Put this code in first line of web page.
 session_start();
 session_destroy();
?>

<!DOCTYPE html>
<html>
    <head>
        <title>TECcargo.dk</title>
        <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
        <link href="../style/style.css" rel="stylesheet">
        <LINK REL="shortcut icon" HREF="../images/icon.ico">
    </head>
    <body>
        <div class="headv2">
            <div class="head-cont">
                <a href="../index.php"><img class="logo" src="../images/logo2.png"></a>
                <a href='../index.php'><img class='logo3' src='../images/logo3.png'></a>
                
            </div>
        </div>
        
        <!-- Menu -->
        <div class="navv2">
            
        </div>
        
        <!-- Sidens indhold-->
        <div class="sitev2" style="padding-top: 20px;">
            <center>Forkert Brugernavn eller Kode</center>
            <center><button type="button" onclick="location.href='../index.php'" class="backbutton" style="margin-top: 10px; margin-bottom: 10px;">Til Startsiden</button></center>
        </div>
            
        <footer align="center">
                <?php include 'footer.php';?>
        </footer> 
    </body>
</html>
