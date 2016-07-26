<?php
session_start();

include '../../config.php';
include '../../is-admin.php';
$linkMainIndex = "../../../themes/index.html";
$linkAdminBar = "../../../themes/navbarAdmin.html";
$linkBillede = "../themes/billede-upload.html";


$status = '';
$statusDisplay = "style='display: none;'";

if(isset($_SESSION['Error']))
{
    $status = $_SESSION['Error'];
    $statusDisplay = "style='display: block;'";
}
if(isset($_SESSION['Upload']) && isset($_SESSION['Type']) && isset($_SESSION['Size']) && isset($_SESSION['Info']))
{
    $status = $_SESSION['Upload'];
    $status .= $_SESSION['Type'];
    $status .= $_SESSION['Size'];
    $status .= $_SESSION['Info'];
    $statusDisplay = "block";
}
if(isset($_SESSION['Delete']))
{
    $status = $_SESSION['Delete'];
    $statusDisplay = "block";
}

$side = $_GET['site'];

if(is_null($side))
{
    $side = 1;
}

//path to directory to scan
$dir = "../../../images/";


// Open a directory, and read its contents
if (is_dir($dir))
{
    if (($dh = opendir($dir)))
    {
        $folderA = Array();
        $pictureA = Array();
        $fileNameA = Array();
        
        while (($file = readdir($dh)) !== false)
        {
            if($file != '.' && $file != '..' && !is_dir($dir .$file))
            {
                $pictureA[] = "<td><img src='" .$dir .$file ."'></td>";
                $fileNameA[] = $file;
            }
        }
            $picTab = "";
            
            //sorting array
            sort($pictureA);
            sort($fileNameA);
            
            //hvormange filer der er
            $pictureCount = count($pictureA);
            for($i = (20 * ($side -1));$i < $pictureCount && $i < (20 * $side);$i++)
            {
                    $picTab .= "<tr>";
                    $picTab .= $pictureA[$i];
                    $picTab .= "<td>$fileNameA[$i]</td>";
                    $picTab .= "<td style='text-align: center;'><input type='checkbox' name='files[]' value='$fileNameA[$i]'></td>";
                    $picTab .= "</tr>";
            }
        
        $sider = ceil(($pictureCount / 20));
        
        closedir($dh);
    }
}

$mainIndex = file_get_contents($linkMainIndex);
$adminBarthemes = file_get_contents($linkAdminBar);
$billedthemes = file_get_contents($linkBillede);

$billedthemes = str_replace("%TABLEP%", $picTab, $billedthemes);
$billedthemes = str_replace("%SIDE%", $side, $billedthemes);
$billedthemes = str_replace("%SIDER%", $sider, $billedthemes);

$billedthemes = str_replace("%STATUSDISPLAY%", $statusDisplay, $billedthemes);
$billedthemes = str_replace("%STATUS%", $status, $billedthemes);

$mainIndex = str_replace("%NavBar%", $adminBarthemes, $mainIndex);
$mainIndex = str_replace("%Index%", $billedthemes, $mainIndex);
$mainIndex = str_replace("%TECCARGO%", Teccargo_url, $mainIndex);

echo $mainIndex;

unset($_SESSION['Error']);
unset($_SESSION['Upload']);
unset($_SESSION['Type']);
unset($_SESSION['Size']);
unset($_SESSION['Info']);
unset($_SESSION['Delete']);

