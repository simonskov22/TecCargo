<?php
    include_once '../php/config.php';
    
    //database billede link
    
    $billed_link = array();
    
    $sqlConnect = mysqli_connect(DB_host, DB_username, DB_password, DB_database)or die(mysqli_error());
    
    $sqlResult = mysqli_query($sqlConnect, "SELECT billede_link FROM `billede_menu`;");
    
    while ($row = mysqli_fetch_array($sqlResult)) {
        $billed_link[] = $row[0];
    }
    
?>
<div class="white" style="width: 1240px; margin: 0 auto;">
    <ul id="mega-menu-9" class="mega-menu">
        <li><a href="#" id="page1">Forside</a></li>
        <li><a href="#">Produkter</a>
            <ul class="hide">
                <li>
                    <div class="menuNheight">
                        <img src="../images/<?php echo $billed_link[0];?>">
                    </div>
                    <a href="#">Kurertransport</a>
                    <ul>
                        <li>
                            <a href="#" id="page2">
                                Express<span class="red">GoRush</span><span class="black">&#8482;</span><br><br>
                                Express<span class="red">GoFlex</span><span class="black">&#8482;</span><br><br>
                                Express<span class="red">GoVIP</span><span class="black">&#8482;</span>
                            </a>
                        </li>
                    </ul>
                </li>


                <li>
                    <div class="menuNheight">
                        <img src="../images/<?php echo $billed_link[1];?>">
                    </div>
                    <a href="#">Pakketransport</a>
                    <ul>
                        <li>
                            <a href="#" id="page3">
                                Priority<span class="red">GoPlus</span><span class="black">&#8482;</span><br><br>
                                Economy<span style="color:#5AC440;">GoGreen</span><span class="black">&#8482;</span>
                            </a>
                        </li>
                    </ul>
                </li>

                <li>
                    <div class="menuNheight">
                        <img src="../images/<?php echo $billed_link[2];?>">
                    </div>
                    <a href="#">Godstransport</a>
                    <ul>
                        <li>
                            <a href="#" id="page4">
                                Cargo<span class="red">GoFull</span><span class="black">&#8482;</span><br><br>
                                Cargo<span class="red">GoPart</span><span class="black">&#8482;</span>
                            </a>
                        </li>
                    </ul>
                </li>

                <li>
                    <div class="menuNheight">
                        <img src="../images/<?php echo $billed_link[3];?>">
                    </div>
                    <a href="#">Logistics</a>
                    <ul>
                        <li><a href="#" id="page5">WHS<span class="red">Logistics</span><span class="black">&#8482;</span></a></li>
                    </ul>
                </li>


                <li>
                    <div class="menuNheight">
                        <img src="../images/<?php echo $billed_link[4];?>">
                    </div>
                    <a href="#">Montage</a>
                    <ul>
                        <li><a href="#" id="page6">Tech<span class="red">Montage</span><span class="black">&#8482;</span></a></li>
                    </ul>
                </li>
            </ul>
        </li>
        <li><a href="#">Priser</a>
            <ul class="hide">
                <li><a href="#">Kurertransport</a>
                    <ul>
                        <li><a href="#" id="page7">Express<span class="red">GoRush</span><span class="black">&#8482;</span></a></li>
                        <li><a href="#" id="page8">Express<span class="red">GoFlex</span><span class="black">&#8482;</span></a></li>
                        <li><a href="#" id="page9">Express<span class="red">GoVIP</span><span class="black">&#8482;</span></a></li>
                    </ul>
                </li>
                <li><a href="#">Pakketransport</a>
                    <ul>
                        <li><a href="#" id="page10">Priority<span class="red">GoPlus</span><span class="black">&#8482;</span></a></li>
                        <li><a href="#" id="page11">Economy<span style="color:#5AC440;">GoGreen</span><span class="black">&#8482;</span></a></li>
                    </ul>
                </li>
                <li><a href="#">Godtransport</a>
                    <ul>
                        <li><a href="#" id="page12">Cargo<span class="red">GoFull</span><span class="black">&#8482;</span></a></li>
                        <li><a href="#" id="page13">Cargo<span class="red">GoPart</span><span class="black">&#8482;</span></a></li>
                    </ul>
                </li>
                <li><a href="#">Logistics</a>
                    <ul>
                        <li><a href="#" id="page20">WHS<span class="red">Logistics</span><span class="black">&#8482;</span></a></li>
                    </ul>
                </li>
                <li><a href="#">Montage</a>
                    <ul>
                        <li><a href="#">Tech<span class="red">Montage</span><span class="black">&#8482;</span></a></li>
                    </ul>
                </li>
            </ul>
        </li>
        <li><a href="#" id="page15">Tracking</a></li>
        <li><a href="#">Om os</a>
            <ul class="hide">
                <li><a href="#" id="page16">Kvalitetspolitik</a></li>
                <li><a href="#" id="page17">Miljøpolitik</a></li>
                <li><a href="#" id="page18">Fragt service</a></li>
                <li><a href="#" id="page19">Font</a></li>
                <li><a href="#" id="page21">Farlig gods</a></li>
                <li><a href="#" id="page22">Indledning</a></li>
                <li><a href="#" id="page23">Styr på køre- og hviletiden</a></li>
            </ul>
        </li>
        <div style="float: right; margin-top: 5px; margin-right: 10px;">
            <?php if(getUserRole(false, false) == "Admin") {?>
            <button onclick="location.href='admin'" class="backbutton">Admin</button>
            <?php }?>
            <button onclick="location.href='logout.php'" class="logoutbutton">Log af</button>
        </div>
    </ul>
</div>