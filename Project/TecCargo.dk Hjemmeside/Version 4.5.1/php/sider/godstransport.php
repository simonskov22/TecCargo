<?php
    include_once '../config.php';
    getUserRole(true, false);
?>
<html>
    <body>
        
        <!-- Godstransport index -->
        <div class="transport-head">
            <h2>Godstransport</h2>
            <p>
                Med vores mange års erfaring inden for transportbranchen kan vi tilbyde vores kunder en enestående<br>
                løsning i forbindelse med godstransport og andre transportopgaver.<br>
                <br>
                Vi håndterer stykgods og pallegods både enkeltvis, i små portioner eller i større partier til en økonomisk<br>
                første-klasses dør-til-dør service med attraktive transittider og til en overkommelig konkurrencedygtig<br>
                pris.<br>
                <br>
                Vi håndterer altid vores godstransport med største omhu, som var det vores eget.<br>
                Vi leverer altid kvalitetsarbejde til den aftalte tid, og sørger for at godset bliver transporteret på den<br>
                mest effektive og sikre måde.<br>
                <br>
                Hos TECcargo er vores medarbejdere dygtige, engagerede og videreførende af  vores værdier med<br>
                godstransport i ethvert stykke arbejde.
            </p>
        </div>
        
        <!-- Godstransport GoFull -->
        
        <div class='transport-head transport-color border'>
            <div class="showprices" style="float: right;">
                <img src="../images/logo/goFull.png" style="height: 100px; margin-right: 50px;">
            </div>
            <h3>Cargo<span class="hgo">GoFull</span><span class="htm">&#8482</span></h3>
            <h4>Cargo service</h4>
            <ul>
                <li style="list-style-type: disc; color: #f73700;"><span style="color: #000;">SameDay Delivery</span></li>
            </ul>
            <p>
                
                Med en <b>GoFull</b> transport fylder vi hele fragtbilen med varer fra et opsamlingssted.<br>
                Transport af GoFull er altid direkte fra afsender til modtager, uden stop undervejs.
            </p>
        </div>
        
        <!-- Godstransport Gopart -->
        
        <div class='transport-head transport-color border pricePadding'>
            
            <div class="showprices" style="float: right;">
                <img src="../images/logo/goPart.png" style="height: 100px; margin-right: 50px;">
                <button id="gopart-1" class="pricesButton">
                    <span class="prisLink" style="display: none;"><?php echo Teccargo_url;?>php/sider/prisPopup/godstransport/prisliste.php</span>
                    Prisliste
                </button><br>
                <button id="gopart-2" class="pricesButton">
                    <span class="prisLink" style="display: none;"><?php echo Teccargo_url;?>php/sider/prisPopup/godstransport/overtak.php</span>
                    Takst Zoner
                </button><br>
                <button id="gopart-3" class="pricesButton">
                    <span class="prisLink" style="display: none;"><?php echo Teccargo_url;?>php/sider/prisPopup/godstransport/ladmeter.php</span>
                    Beregning af ladmeter
                </button><br>
            </div>
            
            <h3>Cargo<span class="hgo">GoPart</span><span class="htm">&#8482</span></h3>
            <h4>Cargo service</h4>
            <ul>
                <li style="list-style-type: disc; color: #f73700;"><span style="color: #000;">NextDay Delivery</span></li>
            </ul>
            <p>
                Med en <b>GoPart</b> transport betaler du kun for den plads du fylder på fragtbilen.<br>
                Transporten opsamler og leverer varer  fra flere steder på samme rute, og der kan derfor forekomme<br>
                omlæsning og terminal stop.
            </p>
        </div>
    </body>
</html>