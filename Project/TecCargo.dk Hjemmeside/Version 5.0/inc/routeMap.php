<?php


$urlParam = explode("/", $_SERVER['REDIRECT_URL']);
$urlParamReady = array();

foreach ($urlParam as $param) {
    
    $param = trim($param);
    $param = strtolower($param);
    
    if(strlen($param) > 0){
    
        $urlParamReady[] = $param;
    }
}

//hvis man kun skriver adressen uden at angive en side
//eller ikke er logget ind skal man ikke kunne se de andre sider
if($urlParamReady[0] == ""){
    
    $urlParamReady[0] = "forside";
}

ob_start();

if(in_array($urlParamReady[0], GetPages()) && $urlParamReady[0] != "view_custom"){
    
    
    include_once "View/{$urlParamReady[0]}.php";
    
    $classObj = new $urlParamReady[0];
}
else{
    
    include_once "View/view_custom.php";
    
    $classObj = new view_custom();
    $urlParamReady[] = $urlParamReady[0];
    
}
if($classObj->onlyOneFunc){

    $funcParm = array();

    for($i = 1; $i < count($urlParamReady); $i++){


        $funcParm[] = $urlParamReady[$i];
    }

    call_user_method($classObj->defaultFunc, $classObj,$funcParm);
}
else{
    if(method_exists($classObj, $urlParamReady[1]) && is_callable(array($classObj, $urlParamReady[1]))){


        if(count($urlParamReady) == 2){


            call_user_method($urlParamReady[1], $classObj);
        }
        else if(count($urlParamReady) > 2){

            $funcParm = array();

            for($i = 2; $i < count($urlParamReady); $i++){
                $funcParm[] = $urlParamReady[$i];
            }


            call_user_method($urlParamReady[1], $classObj,$funcParm);
        }
    }
}
$content = ob_get_contents();
ob_end_clean();


if($classObj->AdminOnly && !IsAdmin() ||
    $classObj->memberOnly && !IsLogin()){
    
    $classObj->title = "Ingen Adgang";
    $content = "Du har ikke tilladelse til at se siden.";
}


if(in_array("notemplate", $urlParamReady)){
    global $_STYLEFILES;
    
    $javaScript = "var stylevar = [";
    
    foreach ($_STYLEFILES as $stylelink) {
        $javaScript .= "'$stylelink', ";
    }
    $javaScript = substr($javaScript, 0, strlen($javaScript) -2) . "];";
    
    
    echo "<script>$(document).ready(function(){ $javaScript pageStyle(stylevar); });</script>";
    $classObj->loadTemplate = false; 
}

if($classObj->loadTemplate){
    
    include_once "template/$classObj->template.php";

    $templateObj = new template();
    $templateObj->title = $classObj->title;
    $templateObj->content = $content;
    $templateObj->isAdminPage = $classObj->AdminOnly;
    $templateObj->ShowTemplate();
}
else{
    echo $content;
}

//foreach ($viewPages as $value) {
//    //echo "--.$value<br>";
//}


function GetPages(){
    
    $viewPages = glob("View/*.php");
    $returnValue = array();
    
    foreach ($viewPages as $page) {
        
        $page = basename($page);
        $page = str_replace(".php", "", $page);
        $page = strtolower($page);
        
        $returnValue[] = $page;
    }

    return $returnValue;
}