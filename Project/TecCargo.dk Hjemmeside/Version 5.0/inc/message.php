<?php

define("msg_error", 0);
define("msg_succes", 1);
define("msg_warning", 2);

$message = new message();

class message{
    
    private $message = array();
    
    public function __construct() {}
    
    public function AddMessage($type,$title,$message){
        $massageContent = array("title" => $title, "message" => $message);
        
        $this->message[$type][] = $massageContent;
    }
    
    public function GetMessage($type){
        return $this->message[$type];
    }
}