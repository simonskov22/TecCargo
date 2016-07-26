<?php


class admin_forside extends pageSettings{
    
    public function __construct() {
        global $_URL, $_STYLEFILES;

        $_STYLEFILES[] = $_URL."style/adminForside.css";

        $this->AdminOnly = true;
        $this->onlyOneFunc = TRUE;
        $this->defaultFunc = "admin_forside";
        $this->title = "Admin Forside";
    }
    
    
    public function admin_forside($param = null) {
        global $_URL;
        
        ?>
        <table class="adminforside">
            <tr class="header">
                <th colspan="3">Brugerindstillinger</th>
            </tr>
            <tr>
                <td>
                    <img src="<?php echo $_URL."images/admin/add_user.png"; ?>"><br>
                    <a href="<?php echo $_URL."admin_user/create/"; ?>" class="defaultButton adminbuton">Opret bruger</a>
                </td>
                <td>
                    <img src="<?php echo $_URL."images/admin/edit_user.png"; ?>"><br>
                    <a href="<?php echo $_URL."admin_user/edit/"; ?>" class="defaultButton adminbuton">Rediger bruger</a>
                </td>
                <td>
                    <img src="<?php echo $_URL."images/admin/remove_user.png"; ?>"><br>
                    <a href="<?php echo $_URL."admin_user/delete/"; ?>" class="defaultButton adminbuton">Slet bruger</a>
                </td>
            </tr>
            <tr class="header">
                <th colspan="3">Sider</th>
            </tr>
            <tr>
                <td>
                    <img src="<?php echo $_URL."images/admin/document.png"; ?>"><br>
                    <a href="<?php echo $_URL."admin_custompage/create/"; ?>" class="defaultButton adminbuton">Opret side</a>
                </td>
                <td>
                    <img src="<?php echo $_URL."images/admin/document.png"; ?>"><br>
                    <a href="<?php echo $_URL."admin_custompage/edit/"; ?>" class="defaultButton adminbuton">Rediger side</a>
                </td>
                <td>
                    <img src="<?php echo $_URL."images/admin/document.png"; ?>"><br>
                    <a href="<?php echo $_URL."admin_custompage/delete/"; ?>" class="defaultButton adminbuton">Slet side</a>
                </td>
            </tr>
            
            <tr class="header">
                <th colspan="3">Kurertransport Priser</th>
            </tr>
            <tr>
                <td style="position: relative;">
                    <img src="<?php echo $_URL."images/admin/price.png"; ?>"><br>
                    <a href="<?php echo $_URL."admin_kurer/gorush/"; ?>" class="defaultButton adminbuton">GoRush</a>
                    <span class="overfloat"><span class="colorBlue">Go</span><span class="colorRed">Rush</span></span>
                </td>
                <td style="position: relative;">
                    <img src="<?php echo $_URL."images/admin/price.png"; ?>"><br>
                    <a href="<?php echo $_URL."admin_kurer/goflex/"; ?>" class="defaultButton adminbuton">GoFlex</a>
                    <span class="overfloat"><span class="colorBlue">Go</span><span class="colorRed">Flex</span></span>
                </td>
                
                <td style="position: relative;">
                    <img src="<?php echo $_URL."images/admin/price.png"; ?>"><br>
                    <a href="<?php echo "#"; ?>" class="defaultButton adminbutonIn">GoVIP</a>
                    <span class="overfloat"><span class="colorBlue">Go</span><span class="colorRed">VIP</span></span>
                </td>
            </tr>
            <tr class="header">
                <th colspan="3">Pakketransport Priser</th>
            </tr>
            <tr>
                <td style="position: relative;">
                    <img src="<?php echo $_URL."images/admin/price.png"; ?>"><br>
                    <a href="<?php echo $_URL."admin_pakke/goplus/"; ?>" class="defaultButton adminbuton">GoPlus</a>
                    <span class="overfloat"><span class="colorBlue">Go</span><span class="colorRed">Plus</span></span>
                </td>
                <td style="position: relative;">
                    <img src="<?php echo $_URL."images/admin/price.png"; ?>"><br>
                    <a href="<?php echo $_URL."admin_pakke/gogreen/"; ?>" class="defaultButton adminbuton">GoGreen</a>
                    <span class="overfloat"><span class="colorBlue">Go</span><span class="colorGreen">Green</span></span>
                </td>
                <td></td>
            </tr>
            <tr class="header">
                <th colspan="3">Godstransport Priser</th>
            </tr>
            <tr>
                <td style="position: relative;">
                    <img src="<?php echo $_URL."images/admin/price.png"; ?>"><br>
                    <a href="<?php echo $_URL."admin_gods/gopart/"; ?>" class="defaultButton adminbuton">GoPart</a>
                    <span class="overfloat"><span class="colorBlue">Go</span><span class="colorRed">Part</span></span>
                </td>
                <td style="position: relative;">
                    <img src="<?php echo $_URL."images/admin/price.png"; ?>"><br>
                    <a href="#" class="defaultButton adminbutonIn">GoFull</a>
                    <span class="overfloat"><span class="colorBlue">Go</span><span class="colorRed">Full</span></span>
                </td>
                <td></td>
            </tr>

            <tr class="header">
                <th colspan="3">Andet</th>
            </tr>
            <tr>
                <td>
                    <img src="<?php echo $_URL."images/admin/calculator.png"; ?>"><br>
                    <a href="<?php echo $_URL."admin_calculator/"; ?>" class="defaultButton adminbuton">Beregner</a>
                </td>
                <td>
                    <img src="<?php echo $_URL."images/admin/photos.png"; ?>"><br>
                    <a href="<?php echo $_URL."admin_picture/"; ?>" class="defaultButton adminbuton">Billeder</a>
                </td>
                <td>
                    <img src="<?php echo $_URL."images/admin/barcode.png"; ?>"><br>
                    <a href="#" class="defaultButton adminbutonIn">Stregkode</a>
                </td>
            </tr>
            <tr>
                <td>
                    <img src="<?php echo $_URL."images/admin/photos.png"; ?>"><br>
                    <a href="<?php echo $_URL."admin_navigationpicture/"; ?>" class="defaultButton adminbuton">Skift billed menu</a>
                </td>
                <td>
                    <img src="<?php echo $_URL."images/admin/photos.png"; ?>"><br>
                    <a href="<?php echo $_URL."admin_pakkepic/"; ?>" class="defaultButton adminbuton">Skift billed pakke</a>
                </td>
                <td>
                </td>
            </tr>
        </table>
        <?php
    }
}