<?php
    $_VERSION = "5.0";
    //$_URL = "/";
    $_URL = "/";
    $_URLPATH = getcwd();
    $_SCRIPTFILES = array();
    $_STYLEFILES = array();
    
    $_SCRIPTFILES[] = "http://ajax.googleapis.com/ajax/libs/jquery/2.0.0/jquery.min.js";
    $_SCRIPTFILES[] = $_URL."script/js/jquery.hoverIntent.minified.js";
//    $_SCRIPTFILES[] = $_URL."script/js/jquery.dcmegamenu.1.3.3.js";
//    $_SCRIPTFILES[] = $_URL."script/js/jquery.session.js";
//    $_SCRIPTFILES[] = $_URL."script/js/jquery.sticky.js";
    
    
    require_once 'inc/config.php';
    require_once 'inc/database.php';
    require_once 'inc/message.php';
    require_once 'inc/function.php';
    require_once 'inc/navigation.php';
    require_once 'inc/pageSettingsClass.php';
    require_once 'inc/routeMap.php';