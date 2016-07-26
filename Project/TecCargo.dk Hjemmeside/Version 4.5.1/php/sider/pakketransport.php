<?php
    include_once '../config.php';
    getUserRole(true, false);
?>
<html>
    <head>
        <style>
            .pricesButton
            {
                margin-top: 5px;
                margin-left: 18px;
                background: #254078;
                color: #fff;
                font-weight: bolder;
            }
        </style>
    </head>
    <body>
        
        <!-- Kurertransport Index -->
        
        <div class="transport-head">
            <h2>Pakketransport</h2>
            <p>
                Når du benytter TECcargo´s pakketransport kan du være sikker på, at pakken når hurtigt og sikkert frem.<br>
                Vi håndterer og distribuerer dagligt pakkeforsendelser på vegne af vore kunder til danske<br>
                erhvervsvirksomheder, detailbutikker og private kunder.<br>
                <br>
                Vi dækker hele Danmark inden for 24 timer. Det gør os til én af de sikreste og hurtigste pakkedistributører<br>
                i Danmark.<br> 
                <br>
                Med TECcargo´s prioriteret behandling under hele processen sikrer vi øget tryghed.<br>
                Vi tager fuldt ansvar for dine ting, imens de er i vores varetægt.
            </p>
        </div>
        
        <!-- Kurertransport Goplus -->

        <div class='transport-head transport-color border'>
            <div class="showprices" style="float: right;">
                <img src="../images/logo/goPlus.png" style="height: 100px; margin-right: 50px;">
                <button id="goplus" class="pricesButton">
                    <span class="prisLink" style="display: none;"><?php echo Teccargo_url;?>php/sider/prisPopup/pakketransport/pakketransportList.php</span>
                    Se Pris
                </button>
            </div>
            <h3>Priority<span class="hgo">GoPlus</span><span class="htm">&#8482</span></h3>
            <p>
                <b>GoPlus</b> er det ideelle valg til forsendelser der skal være fremme næste arbejdsdag, inden kl. 10.00.<br>
                <br>
                Vi holder selvfølgelig løbende status over dine forsendelser og vil til enhver tid kunne oplyse dig præcist,<br>
                hvor dine forsendelser befinder sig. 
            </p>
        </div>
        
        <!-- Kurertransport Gogreen -->
        
        <div class='transport-head transport-color border'>
            <div class="showprices" style="float: right;">
                <img src="../images/logo/goGreen.png" style="height: 100px; margin-right: 50px;">
                <button id="gogreen" class="pricesButton" style="background: #5AC440;">
                    <span class="prisLink" style="display: none;"><?php echo Teccargo_url;?>php/sider/prisPopup/pakketransport/pakketransportList.php</span>
                    Se Pris
                </button>
            </div>
            <h3>Economy<span style="color:#5AC440; font-size: 18px;">GoGreen</span><span class="htm">&#8482</span></h3>
            
            <p>
                <b>GoGreen</b> en økonomisk og miljøbevidst service til mindre presserende leveringer.<br>
                Det ideelle valg når økonomi og miljø skal balancere.<br>
                <br>
                Vi tilbyder vores kunder at medvirke til at reducere Co2-udledningen.<br>
                <br>

                GoGreen levering skal primært bidrage til at nedsætte spildte kilometer, og dermed hjælpe på<br>
                mindre Co2 udledning. Sidst men ikke mindst, er dette også den billigste leverancetype.
            </p>
        </div>
    </body>
</html>