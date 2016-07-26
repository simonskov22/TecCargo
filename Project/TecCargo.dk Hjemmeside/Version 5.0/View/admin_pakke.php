<?php


class admin_pakke extends pageSettings{
    
    public function __construct() {
        global $_URL, $_SCRIPTFILES, $_STYLEFILES,$_URLPATH;
        
        $_SCRIPTFILES[] = $_URL."script/admin_user.js";
        $_SCRIPTFILES[] = $_URL."script/form.js";
        
        $_STYLEFILES[] = $_URL."style/form.css";
        $_STYLEFILES[] = $_URL."style/admin_user.css";
        
        require_once $_URLPATH.'/inc/form.php';
        
        $this-> AdminOnly = true;
        $this->defaultFunc = "pakketransport";
        $this->onlyOneFunc = true;
        $this->title = "Admin Rediger Pakketransport";
    }
     /**
     * 
     * @global databaseSetup $database
     * @global message $message
     */
    private function Post_Update() {
        global $database, $message;
        
        
        $result = $database->Update($database->prefix."PricePakke",array(
            "xs" => $_POST['input_0'],
            "s" => $_POST['input_1'],
            "m" => $_POST['input_2'],
            "l" => $_POST['input_3'],
            "xl" => $_POST['input_4'],
            "2xl" => $_POST['input_5'],
            "3xl" => $_POST['input_6'],
            "gebyr" => $_POST['input_7'],
        ),array("id" => intval($_POST['id'])));
        
        
        if($result){
            
            $message->AddMessage(msg_succes, "Priser Gemt", "");
        }
        else{
            
            $message->AddMessage(msg_error, "Priser Ikke Gemt", "");
        }
    }


    public function pakketransport($param = null) {
        global $_URL,$database;
        
        if(isset($_POST['action']) &&  $_POST['action'] == "pakke"){
            
            $this->Post_Update();
        }
        
        $pakkeType = $this->GetType($param[0]);
        
        $formClass = new form();
        $priceSqlRows = $database->GetRow("SELECT `id`, `xs`, `s`, `m`, `l`, `xl`, `2xl`, `3xl`, `gebyr`"
                . " FROM `{$database->prefix}PricePakke` WHERE `type` = '{$pakkeType['sql']}';", array_I);
                
        
        $priceList = array(
            array("text" => "XS", "type" => "text", "bothValue" => $priceSqlRows[1]),
            array("text" => "S", "type" => "text", "bothValue" => $priceSqlRows[2]),
            array("text" => "M", "type" => "text", "bothValue" => $priceSqlRows[3]),
            array("text" => "L", "type" => "text", "bothValue" => $priceSqlRows[4]),
            array("text" => "XL", "type" => "text", "bothValue" => $priceSqlRows[5]),
            array("text" => "2XL", "type" => "text", "bothValue" => $priceSqlRows[6]),
            array("text" => "3XL", "type" => "text", "bothValue" => $priceSqlRows[7]),
            array("text" => "Gebyr", "type" => "text", "bothValue" => $priceSqlRows[8])
        );
        
        ?>
<form class="tecForm adminUser" method="post" action="<?php echo $_URL."admin_pakke/$param[0]/"; ?>">
    <input type="hidden" name="action" value="pakke" />
    <input type="hidden" name="id" value="<?php echo $priceSqlRows[0]; ?>" />
            <div class="formheader"><h2>Rediger pris for <?php echo $pakkeType['title']; ?></h2></div>
            <table style="width: 100%;border-collapse: collapse;">
                <?php
                    for($i = 0; $i < count($priceList); $i++) {
                            if($priceList[$i]['type'] == "text"){
                                
                                $priceList[$i]['name'] = "input_$i";
                                $priceList[$i]['focus'] = "#row$i";
                                
                                echo "<tr id='row$i'>"
                                        . "<th>{$priceList[$i]['text']}</th>"
                                        . "<td style='text-align: right;'>{$formClass->AddInputBox($priceList[$i])}</td>"
                                    . "</tr>";
                            }
                    }
                ?>
            </table>

            <input type="hidden" name="pakk_00" value="<?php echo $pristype;?>">
            <div class="formbottom">
                <input type="reset" class="defaultButton" onclick="reloadPage();" style="margin-left: 10px;">
                <input type="submit" class="defaultButton" onclick="return false" value="Gem" style="float: right; margin-right: 8px;">
            </div>
        </form>
        <?php
    }
    
    private function GetType($type){
        
        if($type == "goplus"){
            
            $sqlType = "plus";
            $title = "GoPlus";
        }
        else if($type == "gogreen"){
            
            $sqlType = "green";
            $title = "GoGreen";
            
        }
        
        return array("sql" => $sqlType, "title" => $title);
    }
}
