<?php

    include_once '../../config.php';
    getUserRole(TRUE, TRUE);
    
    if(!isset($_SESSION['myusername']) || !isset($_POST['page'])){
        
        //skiver til log
        writeToLog("variabler ikke udflydt.");
        
        header("location:" .Teccargo_url);
        exit();
    }
    $page = $_POST['page'] -1;
    
    //beregner billederne
    $dir = "../../../images/";
    
    //tjekker om den kan finde en mappe der hedder iamges
    //hvis den kan vil den åbne den
    if(is_dir($dir)){
        if(($dh = opendir($dir))){

            $billedeName = array();

            //læser navne på bilderne i mappen
            //hvis det er en mappe vil den ikke tage den med
            while (($file = readdir($dh)) !== false)
            {
                if($file != '.' && $file != '..' && !is_dir($dir .$file))
                {
                    $billedeName[] = $file;
                }
            }
            
            closedir($dh);


            //sorting array
            sort($billedePart);
            sort($billedeName);

            //hvormange billeder er der
            $pictureCount = count($billedeName);
            
            $htmlTablePic = "";
            //udskriver bildederne
            for($i = (15 * ($page));$i < $pictureCount && $i < (15 * ($page +1));$i++)
            {
                $htmlTablePic .= "<tr id=pic_$billedeName[$i]>"
                        . "<td><button id='picture_$billedeName[$i]' class='pictureSettings' onclick='return false;'>Skift</button></td>"
                        . "<td><img class='minimaxsize' src='$dir/$billedeName[$i]'></td>"
                    . "</tr>";
            }
            
            $sider = ceil(($pictureCount / 15));
            
            //nextpage og backpage
            if($page == $sider -1){
                $nextpage = "none";
            }
            else{
                $nextpage = $page + 2;
            }
            if($page == 0){
                $backpage = "none";
            }
            else{
                $backpage = $page;
            }
            
            
            echo json_encode(array("table" => $htmlTablePic,"nextpage" => $nextpage,"backpage" => $backpage,"nowpage" => $page+ 1 ."/$sider"));
        }
    }