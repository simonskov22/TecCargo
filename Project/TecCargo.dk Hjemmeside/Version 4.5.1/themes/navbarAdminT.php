<?php
    include_once '../php/config.php';
?>

<div class="white">
    <div style="width: 1240px; margin-left: auto; margin-right: auto;">
        <ul id="mega-menu-9" class="mega-menu">
            
            <li><a href="<?php echo Teccargo_url ."php/admin/index.php";?>">Admin</a></li>
            <li><a href="#">Brugerindstillinger</a>
                <ul class="hide">
                    <li><a href="<?php echo Teccargo_url ."php/admin/sider/createuser.php"; ?>">Opret Bruger</a></li>
                    <li><a href="<?php echo Teccargo_url ."php/admin/sider/edituser.php"; ?>">Rediger Bruger</a></li>
                    <li><a href="<?php echo Teccargo_url ."php/admin/sider/deleteuser.php"; ?>">Slet Bruger</a></li>
                </ul>
            </li>
            <li><a href="#">Priser</a>
                    <ul class="hide">
                        <li><a href="#">Kurertransport</a>
                            <ul>
                                <li><a href="<?php echo Teccargo_url ."php/admin/sider/kurerpris.php?type=rush"; ?>">Express<span class="red">GoRush</span><span class="black">&#8482;</span></a></li>
                                <li><a href="<?php echo Teccargo_url ."php/admin/sider/kurerpris.php?type=flex"; ?>">Express<span class="red">GoFlex</span><span class="black">&#8482;</span></a></li>
                            </ul>
                        </li>
                        <li><a href="#">Pakketransport</a>
                            <ul>
                                <li><a href="<?php echo Teccargo_url ."php/admin/sider/pakkepris.php?type=plus"; ?>">Priority<span class="red">GoPlus</span><span class="black">&#8482;</span></a></li>
                                <li><a href="<?php echo Teccargo_url ."php/admin/sider/pakkepris.php?type=green"; ?>">Economy<span style="color:#5AC440;">GoGreen</span><span class="black">&#8482;</span></a></li>
                            </ul>
                        </li>
                        <li><a href="#">Godtransport</a>
                            <ul>
                                <li><a href="<?php echo Teccargo_url ."php/admin/sider/godspris.php?type=part"; ?>">Cargo<span class="red">GoPart</span><span class="black">&#8482;</span></a></li>
                            </ul>
                        </li>
                    </ul>
            </li>
            <li><a href="#">Andet</a>
                <ul class="hide">
                    <li><a href="<?php echo Teccargo_url ."php/admin/sider/beregnzone.php"; ?>">Beregner</a></li>
                    <li><a href="<?php echo Teccargo_url ."php/admin/sider/billeder.php"; ?>">Billeder</a></li>
                    
                </ul>
            </li>
            
            <div style="float: right; margin-top: 5px; margin-right: 10px;">
                <button onclick="location.href='<?php echo Teccargo_url; ?>php/mainindex.php'" class="backbutton">Til Forside</button>
                <button onclick="location.href='<?php echo Teccargo_url; ?>php/logout.php'" class="logoutbutton">Log af</button>
            </div>
        </ul>


    </div>
</div>
