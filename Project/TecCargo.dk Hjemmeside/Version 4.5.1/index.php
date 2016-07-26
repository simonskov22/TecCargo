<?php 

include 'php/getBrowser.php';
include_once 'php/config.php';
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

//laver login menu
ob_start();
?>
<!-- login form -->
<div class='login'>
    <form name='login' method='POST' action='php/checklogin.php'>
        <input type='text' name='login_username' class="login-text" placeholder="Brugernavn" value="<?php echo $brugernavnLogin; ?>">
        <input type='password' name='login_password' class="login-text" placeholder="Password" value="<?php echo $kodeLogin; ?>">
        <input type='submit' value='Log ind' name='login_submit' class="login-button">
    </form>
</div>
<?php
$NavBar = ob_get_contents();
ob_end_clean();

ob_start();
?>
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

<?php
$Index = ob_get_contents();
ob_end_clean();

include_once 'themes/indexT.php';