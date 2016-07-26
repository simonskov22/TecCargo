<?php
    include '../../../config.php';
    
    //henter priserne
    $price = array();
    for($i = 0;$i < 26;$i++)
    {
        $price[] = $_POST["kurerArray_$i"];
    }
    $id = $_POST['id'];
    
    $sqlConnect = mysqli_connect(DB_host, DB_username, DB_password, DB_database);
    
    $sqlcmd = "UPDATE kurertransportPris SET billedBil='$price[0]',gruppe='$price[1]',billedVaegt='$price[2]',"
            . "vaegt='$price[3]',ladmaal='$price[4]',laesseaabning='$price[5]',lift='$price[6]',guvareal='$price[7]',"
            . "rumindhold='$price[8]',prisPrTur='$price[9]',startgebyr='$price[10]',kilometertaksst='$price[11]',"
            . "tidsForbrug='$price[12]',tidsForbrug2='$price[13]',chauffoermedhjaelper='$price[14]',flytteTilaeg='$price[15]',"
            . "`ADR-tilaeg`='$price[16]',natTillaeg='$price[17]',weekendTillaeg='$price[18]',yderzoneTillaeg='$price[19]',"
            . "byttepalleTillaeg='$price[20]',`SMS-servicetillaeg`='$price[21]',adresseKorrektion='$price[22]',braendstofgebyr='$price[23]',"
            . "miljoegebyr='$price[24]',adminnistrationsgebyr='$price[25]' WHERE id='$id'";
    
    $result = mysqli_query($sqlConnect, $sqlcmd);
    
    if($result)
    {
        $message = "<b>Godkendt:</b><br><br>Prisen er nu gemt.";
    }
    else
    {
        //echo mysqli_error($sqlConnect);
 
        $message = "<b>Fejl:</b><br><br>Prisen kunne ikke gemmes.";
    }

    session_start();
    $_SESSION['Kurer'] = $message;
    
    if($id <= 4)
    {
        $backLink = "location:../../sider/edit-gorush.php";
    }
    else
    {
        $backLink = "location:../../sider/edit-goflex.php";
    }

    header($backLink);
