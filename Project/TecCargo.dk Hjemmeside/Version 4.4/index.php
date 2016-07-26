<?php 
include 'php/getBrowser.php';

$browser = getBrowser();

if($browser['name'] == 'Internet Explorer' && $browser['version'] == '9.0' )
{
   $brugernavnLogin = "Brugernavn";
   $kodeLogin = "Password";
}
else 
{    
   $brugernavnLogin = "";
   $kodeLogin = "";
}
?>
<!DOCTYPE html>
<html>
    <head>
        <title>TECcargo.dk</title>
        <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
        <link href="style/style.css" rel="stylesheet">
        <LINK REL="shortcut icon" HREF="images/icon.ico">
        
    </head>
    <body>
        <div class="headv2">
            <div class="head-cont">
                <a href="index.php"><img class="logo" src="images/logo2.png"></a>
                <a href='index.php'><img class='logo3' src='images/logo3.png'></a>
                
                
            </div>
        </div>
        
        <!-- Menu -->
        <div class="navv2">
            
            <!-- login form -->
                <div class='login'>
                    <form name='login' method='POST' action='php/checklogin.php'>
                        <input type='text' name='login_username' class="login-text" placeholder="Brugernavn" value="<?php echo $brugernavnLogin; ?>">
                        <input type='password' name='login_password' class="login-text" placeholder="Password" value="<?php echo $kodeLogin; ?>">
                        <input type='submit' value='Log ind' name='login_submit' class="login-button">
                    </form>
                </div>
        </div>
        
        <!-- Sidens indhold-->
        <div class="sitev2">
            <div  style="padding-top: 20px; ">

                <!-- track -->
                <div class='f_track' style="float: left; margin-right: 20px;">
                    <h3 style='text-align:center; padding: 30px;'>Track</h3>
                    <br>
                    <div class='form_track'>
                        <form method='POST'>
                            <input type='text' name='f_track_id' style='width:153px;'>
                            <br>
                            <input type='submit' value='Reset'>
                            <input type='submit' value='Find' name='f_track_find' style='float:right;'>
                            <br>
                        </form>
                    </div>
                    <div style='margin-top:20px; margin-bottom:20px; margin-left:30px; margin-right:30px; height:250px; overflow:auto;'>
                        <center><p>Skriv dit tracking id for at se hvor din pakke er</p>
                        <p>(Virker ikke endnu)</p></center>
                        <?php
                                //echo $track_pakke_string;
                        ?>
                    </div>
                </div>
                <?php include 'php/sider/praktikcenter.php';?>

            </div>
        </div>
            
       <footer align="center">
                <?php include 'php/footer.php';?>
            </footer> 
    </body>
</html>
