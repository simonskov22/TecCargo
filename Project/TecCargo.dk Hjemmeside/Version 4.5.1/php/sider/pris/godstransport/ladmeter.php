<?php
    include_once '../../../config.php';
    getUserRole(true, false);
?>
<html>
    <body>
        <div class='transport-head ladmeter'>
            <h2>Beregning af ladmeter og palleplads</h2>
            <table class="transportNor">
                <tr>
                    <td rowspan="2">Transport Norden gælder:</td>
                    <td>1 LDM</td>
                    <td>=</td>
                    <td class="text-right">2000 kg.</td>
                </tr>
                <tr>
                    <td>1 PLL</td>
                    <td>=</td>
                    <td class="text-right">800 kg.</td>
                </tr>
                <tr class="mellemrum" colspan="4"></tr>
                <tr>
                    <td rowspan="2">Transport Europa gælder:</td>
                    <td>1 LDM</td>
                    <td>=</td>
                    <td class="text-right">1850 kg.</td>
                </tr>
                <tr>
                    <td>1 PLL</td>
                    <td>=</td>
                    <td class="text-right">740 kg.</td>
                </tr>
                <tr class="mellemrum" colspan="4"></tr>
                <tr>
                    <td rowspan="2">Transport Indland Danmark gælder:</td>
                    <td>1 LDM</td>
                    <td>=</td>
                    <td class="text-right">1500 kg.</td>
                </tr>
                <tr>
                    <td>1 PLL</td>
                    <td>=</td>
                    <td class="text-right">600 kg.</td>
                </tr>
                <tr class="mellemrum" colspan="4"></tr>
                <tr>
                    <td rowspan="2">Transport Indland Sverige gælder:</td>
                    <td>1 LDM</td>
                    <td>=</td>
                    <td class="text-right">1950 kg.</td>
                </tr>
                <tr>
                    <td>1 PLL</td>
                    <td>=</td>
                    <td class="text-right">780 kg.</td>
                </tr>
            </table>
            <p>
                LDM = Ladmeter<br>
                PLL = Palleplads
            </p>
            <h2>Eksempel på beregning indland Danmark:</h2>
            <h4>Ladmeter</h4>
            <p>
                En ladmeter betegnes når gods optager plads svarende til 1 meter af bilens længde,<br>
                hele bilens bredde (2,4 m) og højden eller beskaffenhed gør, at det ikke er muligt at bruge pladsen ovenpå<br>
                godset.<br> 
                Ladmeter benyttes når gods ikke kan stables og på gods med mål udover standart palle mål (120 X 80)<br>
                som f.eks maskiner m.m.<br>
                1 LDM omberegnes til 1500 kg, fragtpligtigvægt, hvis ikke den reelle vægt overstiger dette. <br>
            </p>
            <h4>Palleplads</h4>
            <p>
                (PPL) = 600 kg<br>
                En palleplads er pladsen svarende til mål på en EUR-palle (120 X 80 cm)og hvor beskaffenhed gør at andet<br>
                gods ikke kan eller må stables ovenpå.<br>
                En palleplads beregnes efter en EUR-palles mål (120 X 80 cm).<br>
                En PPL omberegnes til 600 kg, fragtpligtigvægt, hvis ikke den  reelle vægt overstiger dette.<br>
            </p>
            <table>
                <tr>
                    <td style="text-align: center;">
                        <span style="border-bottom: 2px solid black;">
                            EUR - palle længde 1,2 m X bredde 0,8 m
                        </span><br>
                        hele bilens lad bredde (2,4 m)
                    </td>
                    <td>= 0,4 LDM</td>
                </tr>
            </table>
            <h4>Volume</h4>
            <p>
                1 m³ kubikmeter = 250 kg<br>
                <br>
                Er godset 140 cm eller lavere, og egner sig til at blive stablet eller kan tåle at der stables på det,<br>
                beregnes den fragtpligtigevægt ud fra godsets m3. kubikmeter
            </p>
            <table>
                <tr>
                    <td style="text-align: center;">
                        <span style="border-bottom: 2px solid black;">
                            længde i cm X bredde i cm X højde i cm X 250kg
                        </span><br>
                        1.000.000
                    </td>
                    <td>= Volume kg</td>
                </tr>
            </table>
            <p>
                Gods hvor det ikke er muligt at stable ovenpå, beregnes altid udfra ladmeter.<br>
                Denne beslutning træffes af TECcargo og er altid gældende.<br>
                Såfremt ét af flg. Kritierier er oplyldt, afregnes efter ladmeter/palleplads:<br>
            </p>
            <ul>
                <li class="ladLi"><span style="color: black;">Pallen er højere end 140 cm</span></li>
                <li class="ladLi"><span style="color: black;">Pallen er angivet til IKKE at måtte stables</span></li>
                <li class="ladLi"><span style="color: black;">Pallen pga. opbygningen ikke muliggør, at der stables en anden palle ovenpå (skæv, ujævn,<br> kegleformet osv.)</span></li>
            </ul>
        </div>
    </body>
</html>

