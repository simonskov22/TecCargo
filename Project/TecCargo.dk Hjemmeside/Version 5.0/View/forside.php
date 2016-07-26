<?php



class forside extends pageSettings{
    


    public function __construct() {
        global $_URL, $_SCRIPTFILES, $_STYLEFILES;
        
        $_SCRIPTFILES[] = $_URL."script/forside.js";
        $_STYLEFILES[] = $_URL."style/forside.css";
        
        
        $this->title = "Forside";
        $this->onlyOneFunc = true;
        $this->defaultFunc = "DefaultCall";
        $this->memberOnly = false;
    }
    
    public function DefaultCall($param = null) {
        
        if (IsLogin()) {
            $this->ShowMemberPage();
        }
        else{
            $this->ShowGuestPage();
        }
    }


    private function ShowMemberPage() {
        global $_URL;
        ?>
<div class="circleCenter">
    <div class="circleBox">

        <ul>
            <li><a href="<?php echo $_URL."Kurertransport/"; ?>" bind-line='#tecLine1' class="redBorder hoverLineShow hideBorder" style="margin-left: 200px;">Express<span class="redText">GoRush</span><span class="blackText">&#8482;</span></a></li>
            <li><a href="<?php echo $_URL."Kurertransport/"; ?>" bind-line='#tecLine2' class="redBorder hoverLineShow hideBorder" style="margin-left: 115px;">Express<span class="redText">GoFlex</span><span class="blackText">&#8482;</span></a></li>
            <li><a href="<?php echo $_URL."Kurertransport/"; ?>" bind-line='#tecLine3' class="redBorder hoverLineShow hideBorder" style="margin-left: 87px;">Express<span class="redText">GoVIP</span><span class="blackText">&#8482;</span></a></li>
            <li><a href="<?php echo $_URL."Pakketransport/"; ?>" bind-line='#tecLine4' class="redBorder hoverLineShow hideBorder" style="margin-left: 115px;">Priority<span class="redText">GoPlus</span><span class="blackText">&#8482;</span></a></li>
            <li><a href="<?php echo $_URL."Pakketransport/"; ?>" bind-line='#tecLine5' class="greenBorder hoverLineShow hideBorder" style="margin-left: 394px;">Economy<span class="greenText">GoGreen</span><span class="blackText">&#8482;</span></a></li>
        </ul>
        <div class="imgBox hoverLineShow" bind-line=".logoLink" bind-links="a.hoverLineShow">
            <img src="<?php echo $_URL."images/logo.png"; ?>"/>
        </div>
        <ul>
            <li><a href="<?php echo $_URL."Godstransport/"; ?>" bind-line='#tecLine6' class="redBorder hoverLineShow hideBorder" style="margin-left: -90px;">Cargo<span class="redText">GoFull</span><span class="blackText">&#8482;</span></a></li>
            <li><a href="<?php echo $_URL."Godstransport/"; ?>" bind-line='#tecLine7' class="redBorder hoverLineShow hideBorder" style="margin-left: 21px;">Cargo<span class="redText">GoPart</span><span class="blackText">&#8482;</span></a></li>
            <li><a href="<?php echo $_URL."Lagerhotel/"; ?>" bind-line='#tecLine8' class="redBorder hoverLineShow hideBorder" style="margin-left: 44px;">WHS<span class="redText">Logistics</span><span class="blackText">&#8482;</span></a></li>
            <li><a href="<?php echo $_URL."Montage/"; ?>" bind-line='#tecLine9' class="redBorder hoverLineShow hideBorder" style="margin-left: 21px;">Tech<span class="redText">Montage</span><span class="blackText">&#8482;</span></a></li>
        </ul>

        <svg class="hoverLines">
            <line class="logoLink lineRed" id="tecLine1" x1="353" y1="91" x2="392" y2="128"></line>
            <line class="logoLink lineRed" id="tecLine2" x1="260" y1="196" x2="325" y2="207"></line>
            <line class="logoLink lineRed" id="tecLine3" x1="226" y1="316" x2="306" y2="316"></line>
            <line class="logoLink lineRed" id="tecLine4" x1="256" y1="438" x2="339" y2="420"></line>
            <line class="logoLink lineGreen" id="tecLine5" x1="480" y1="541" x2="480" y2="503"></line>
            <line class="logoLink lineRed" id="tecLine6" x1="571" y1="91" x2="551" y2="118"></line>
            <line class="logoLink lineRed" id="tecLine7" x1="682" y1="196" x2="636" y2="210"></line>
            <line class="logoLink lineRed" id="tecLine8" x1="704" y1="316" x2="654" y2="316"></line>
            <line class="logoLink lineRed" id="tecLine9" x1="682" y1="438" x2="620" y2="420"></line>
        </svg>
    </div>
</div>

        <?php
    }
    
    private function ShowGuestPage() {
        global $database;
        $page = $database->GetRow("SELECT `content` FROM `{$database->prefix}Pages` WHERE `title` = 'Praktikcenter';");
        ?>
<div style="text-align: center;">
            <div class='tecTrackBox'>
                <div class="header">
                    <h3>Track & Trace</h3>
                </div>
                <div>
                    <form method='POST'>
                        <div class="text">
                            <p>Skriv dit tracking id for at se hvor din pakke er</p>
                            <p>(Virker ikke endnu)</p>
                        </div>
                        <div class="searchInput">
                            <input type='text'>
                            <input type='submit' value='Søg' onclick="return false;">
                        </div>
                    </form>
                </div>
            </div>

<div class="parktikcenterBox">
                <?php echo $page->content; ?>
            </div>
</div>
        <?php
    }
}