<?php

define("navbar_link", 0);
define("navbar_dropdown", 1);
define("navbar_dropdownheader", 2);
define("navbar_dropdownpic", 3);

class navigation{
    
    private $navigationCont = array();
    private $showAdminButton = false;
    private $showMemberButton = false;


    public function __construct() {
    }
    
    public function MakeCategory($type, $id, $name) {
        
        if(!key_exists($id, $this->navigationCont)){
        
            $this->navigationCont[$id] = array("type" => $type, "name" => $name, "content" => array());
        }
    }
    
    public function AddLink($id, $link, $name = null,$header = null, $picture = null) {
        
        $index = -1;
        $headersCount = count($this->navigationCont[$id]['content']);
        $type = $this->navigationCont[$id]['type'];
        
        if(key_exists($id, $this->navigationCont)){
            
            $navLink = array($link, $name);
            
            if($type === navbar_dropdownpic || $type === navbar_dropdownheader){
                
                for ($i = 0; $i < $headersCount; $i++) {
                    
                    if($this->navigationCont[$id]['content'][$i]['header'] == $header){
                        
                        $index = $i;
                        break;
                    }
                }   
            }
            
            
            
            if($type === navbar_dropdownpic){
                
                if($this->navigationCont[$id]['picture'] === null){}
                else if($this->navigationCont[$id]['picture'] != $picture){}
            }
            
            if($index != -1){
                $this->navigationCont[$id]['content'][$index]['links'][] = $navLink;
            }
            else if($type == navbar_dropdown && $headersCount == 1){
                $this->navigationCont[$id]['content'][0]['links'][] = $navLink;
            }
            else{
                $this->navigationCont[$id]['content'][] = array("links" => array($navLink), "picture" => $picture, "header" => $header);
            }
        }
    }
    
    public function excuteNavigation(){
        
        echo "<div class='TecNavigationBox'>"
                . "<ul class='TecNavigation'>";
        
        foreach ($this->navigationCont as $catygory) {
            
            $headersCount = count($catygory['content']);
            $type = $catygory['type'];
            $menuName = $catygory['name'];
            $onclick = "";
            $dropDownArray = "";
            $link = "#";
            
            if($type !== navbar_link){
                
                $onclick = " onclick='return false;'";
                $dropDownArray = " <span class='dropDownArrow'></span>";
            }
            else{
                
                $link = $catygory['content'][0]['links'][0][0];
            }
            
            
            echo "<li class='navbar'>"
            . "<a href='{$link}' class='navbarLink'{$onclick}>"
                . "$menuName{$dropDownArray}"
            . "</a>";

            if($type !== navbar_link){
                $dropDownClass = $type == navbar_dropdown ? "dropdownSmall" : "dropdownLong";
                $addArrow = $type == navbar_dropdown ? "<span class='arrow'>&#8674;</span>" : "";
                
                echo "<div class='showOnHover $dropDownClass'>";
                
                for ($i = 0; $i < $headersCount; $i++) {
                    
                    
                    echo "<ul>";
                    
                    if($type == navbar_dropdownpic){
                        
                        echo "<li class='img'><img src='{$catygory['content'][$i]['picture']}'/></li>";
                    }
                    if($type != navbar_dropdown){
                        
                        echo "<li class='header'>{$catygory['content'][$i]['header']}</li>";
                    }
                    
                    foreach ($catygory['content'][$i]['links'] as $links) {
                        
                        echo "<li class='option'><a href='{$links[0]}'>{$addArrow}{$links[1]}</a></li>";
                    }
                    
                    echo "</ul>";
                }
                
                echo "</div>";
            }
        }
        
        echo "</li></ul>"; 
        
        if($this->showAdminButton){
            $this->AdminButton();
        }
        else if($this->showMemberButton){
            $this->MemberButton();
        }
        
    echo "</div>";
    }
    
    private function AdminButton() {
        global $_URL;
        
        echo "<div class='tecNavButtons'>"
        . "<a href='{$_URL}forside/' class='blue'>Forside</a>"
            . "</div>";
    }
    private function MemberButton() {
        global $_URL;
        if(IsAdmin()){
            echo "<div class='tecNavButtons'>"
            . "<a href='{$_URL}admin_forside/' class='blue'>Admin</a>"
                . "</div>";
        }
    }


    public function MakeAdminNavigation() {
        global $_URL;
        
        $this->showAdminButton = true;

        $this->MakeCategory(navbar_link, 0, "Admin");
        $this->MakeCategory(navbar_dropdown, 1, "Bruger");
        $this->MakeCategory(navbar_dropdown, 4, "Side");
        $this->MakeCategory(navbar_dropdownheader, 2, "Priser");
        $this->MakeCategory(navbar_dropdown, 3, "Andet");


        $this->AddLink(0, $_URL."Admin_Forside/");

        $this->AddLink(1, $_URL."Admin_User/Create/", "Opret");
        $this->AddLink(1, $_URL."Admin_User/Edit/", "Rediger");
        $this->AddLink(1, $_URL."Admin_User/Delete/", "Slet");
       
        $this->AddLink(4, $_URL."Admin_Custompage/Create/", "Opret");
        $this->AddLink(4, $_URL."Admin_Custompage/Edit/", "Rediger");
        $this->AddLink(4, $_URL."Admin_Custompage/Delete/", "Slet");
        
        
        $this->AddLink(2, $_URL."Admin_Kurer/GoRush/", "Express<span class='red'>GoRush</span><span class='black'>&#8482;</span>", "Kurertransport");
        $this->AddLink(2, $_URL."Admin_Kurer/GoFlex/", "Express<span class='red'>GoFlex</span><span class='black'>&#8482;</span>", "Kurertransport");
        
        $this->AddLink(2, $_URL."Admin_Pakke/GoPlus", "Priority<span class='red'>GoPlus</span><span class='black'>&#8482;</span>", "Pakketransport");
        $this->AddLink(2, $_URL."Admin_Pakke/GoGreen", "Economy<span class='green'>GoGreen</span><span class='black'>&#8482;</span>", "Pakketransport");

        $this->AddLink(2, "#", "Cargo<span class='red'>GoFull</span><span class='black'>&#8482;</span>", "Godstransport");
        $this->AddLink(2, $_URL."Admin_Gods/", "Cargo<span class='red'>GoPart</span><span class='black'>&#8482;</span>", "Godstransport");

        $this->AddLink(3, $_URL."Admin_Calculator/", "Zone Begregner");
        $this->AddLink(3, $_URL."Admin_picture/", "Billed upload");

    }
    public function MakeMemberNavigation() {
        global $_URL,$database;
    
        $pictureRow = $database->GetResults("SELECT `picture` FROM `{$database->prefix}MenuPic`;");
        
        $this->showMemberButton = true;

        $this->MakeCategory(navbar_link, 0, "Forside");
        $this->MakeCategory(navbar_dropdownpic, 1, "Produkter");
        $this->MakeCategory(navbar_dropdownheader, 2, "Priser");
        $this->MakeCategory(navbar_link, 3, "Tracking");
        $this->MakeCategory(navbar_dropdown, 4, "Om os");


        $this->AddLink(0, $_URL."Forside/");

        $kure = "Express<span class='red'>GoRush</span><span class='black'>&#8482;</span><br>"
            . "Express<span class='red'>GoFlex</span><span class='black'>&#8482;</span><br>"
            . "Express<span class='red'>GoVIP</span><span class='black'>&#8482;</span>";

        $pakke = "Priority<span class='red'>GoPlus</span><span class='black'>&#8482;</span><br>"
            . "Economy<span class='green'>GoGreen</span><span class='black'>&#8482;</span>";

        $gods = "Cargo<span class='red'>GoFull</span><span class='black'>&#8482;</span><br>"
            ."Cargo<span class='red'>GoPart</span><span class='black'>&#8482;</span>";

        $this->AddLink(1, $_URL."Kurertransport/", $kure, "Kurertransport", $pictureRow[0]->picture);
        $this->AddLink(1, $_URL."Pakketransport/", $pakke, "Pakketransport", $pictureRow[1]->picture);
        $this->AddLink(1, $_URL."Godstransport/", $gods, "Godstransport", $pictureRow[2]->picture);
        $this->AddLink(1, $_URL."Lagerhotel/", "WHS<span class='red'>Logistics</span><span class='black'>&#8482;</span>", "Logistics", $pictureRow[3]->picture);
        $this->AddLink(1, $_URL."Montage/", "Tech<span class='red'>Montage</span><span class='black'>&#8482;</span>", "Montage", $pictureRow[4]->picture);

        $this->AddLink(2, $_URL."Kurer_Price/GoRush/", "Express<span class='red'>GoRush</span><span class='black'>&#8482;</span>", "Kurertransport");
        $this->AddLink(2, $_URL."Kurer_Price/GoFlex/", "Express<span class='red'>GoFlex</span><span class='black'>&#8482;</span>", "Kurertransport");
        $this->AddLink(2, $_URL."Special_Tilbud/", "Express<span class='red'>GoVIP</span><span class='black'>&#8482;</span>", "Kurertransport");

        $this->AddLink(2, $_URL."Pakke_Price/GoPlus", "Priority<span class='red'>GoPlus</span><span class='black'>&#8482;</span>", "Pakketransport");
        $this->AddLink(2, $_URL."Pakke_Price/GoGreen", "Economy<span class='green'>GoGreen</span><span class='black'>&#8482;</span>", "Pakketransport");

        $this->AddLink(2, $_URL."Special_Tilbud/", "Cargo<span class='red'>GoFull</span><span class='black'>&#8482;</span>", "Godstransport");
        $this->AddLink(2, $_URL."Gods_price/", "Cargo<span class='red'>GoPart</span><span class='black'>&#8482;</span>", "Godstransport");

        $this->AddLink(2, $_URL."Lagerhotel_Price/", "WHS<span class='red'>Logistics</span><span class='black'>&#8482;</span>", "Logistics");
        $this->AddLink(2, $_URL."Special_Tilbud/", "Tech<span class='red'>Montage</span><span class='black'>&#8482;</span>", "Montage");

        $this->AddLink(3, $_URL."#");

        $this->AddLink(4, $_URL."Kvalitetspolitik/", "Kvalitetspolitik");
        $this->AddLink(4, $_URL."Miljøpolitik/", "Miljøpolitik");
        $this->AddLink(4, $_URL."Fragt service/", "Fragt service");
        $this->AddLink(4, $_URL."Font/", "Font");
        $this->AddLink(4, $_URL."Farlig gods/", "Farlig gods");
        $this->AddLink(4, $_URL."Indledning/", "Indledning");
        $this->AddLink(4, $_URL."Koreoghviletiden/", "Styr på køre- og hviletiden");
        $this->AddLink(4, $_URL."Transportforsikring/", "Transportforsikring");
    }
    public function MakeGuestNavigation() {
        
    }
}