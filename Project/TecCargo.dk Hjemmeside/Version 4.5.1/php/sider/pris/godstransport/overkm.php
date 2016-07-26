<?php
    include_once '../../../config.php';
    getUserRole(true, false);
?>
<html>
    <body>
        <div class="buttonTable" style="width:200px; marg">
                <button type="button" class="km1B bgreen" style="float:left; margin-right: 20px;">Side 1</button>
                <button type="button" class="km6B bblue">Side 2</button>
            </div>
            <div class="takst-list kmlist1">
                <table style='text-align: center;'>
                    <tr>
                        <th></th>
                        <th>Hvidovre</th>
                        <th>Rønne</th>
                        <th>Hvidovre</th>
                        <th>Hvidovre</th>
                        <th>Odense</th>
                    </tr>
                    <tr class='thred'>
                        <td>km</td>
                        <td>Betjener<br>Nordøstlige<br>Sjælland</td>
                        <td>Betjener kun<br>Bornholm</td>
                        <td>Betjener<br>sydvestlige<br>Sjælland</td>
                        <td>Betjener<br>Lolland<br>Falster og<br>Møn</td>
                        <td>Betjener<br>Fyn og<br>omkringligge<br>nde øer</td>
                    </tr>
                    <tr class='thred'>
                        <td></td>
                        <td>1000-3699</td>
                        <td>3700-3799</td>
                        <td>4000-4799</td>
                        <td>4800-4999</td>
                        <td>5000-5999</td>
                    </tr>
                    <tr class='trblue'>
                        <td><b>Hvidovre</b><br>1000-3699</td>
                        <td>X</td>
                        <td></td>
                        <td>90</td>
                        <td>125</td>
                        <td>160</td>
                    </tr>
                    <tr class='trred'>
                        <td><b>Rønne</b><br>3700-3799</td>
                        <td></td>
                        <td>X</td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr class='trblue'>
                        <td><b>Hvidovre</b><br>4000-4799</td>
                        <td>90</td>
                        <td></td>
                        <td>X</td>
                        <td>135</td>
                        <td>75</td>
                    </tr>
                    <tr class='trred'>
                        <td><b>Hvidovre</b><br>4800-4999</td>
                        <td>125</td>
                        <td></td>
                        <td>105</td>
                        <td>X</td>
                        <td>170</td>
                    </tr>
                    <tr class='trblue'>
                        <td><b>Odense</b><br>5000-5999</td>
                        <td>160</td>
                        <td></td>
                        <td>75</td>
                        <td>170</td>
                        <td>X</td>
                    </tr>
                    <tr class='trred'>
                        <td><b>Kolding</b><br>6000-6999</td>
                        <td>225</td>
                        <td></td>
                        <td>140</td>
                        <td>235</td>
                        <td>70</td>
                    </tr>
                    <tr class='trblue'>
                        <td><b>Herning</b><br>7000-7999</td>
                        <td>300</td>
                        <td></td>
                        <td>220</td>
                        <td>315</td>
                        <td>150</td>
                    </tr>
                    <tr class='trred'>
                        <td><b>Århus</b><br>8000-8999</td>
                        <td>300</td>
                        <td></td>
                        <td>220</td>
                        <td>310</td>
                        <td>145</td>
                    </tr>
                    <tr class='trblue'>
                        <td><b>Aalborg</b><br>9000-9999</td>
                        <td>410</td>
                        <td></td>
                        <td>325</td>
                        <td>420</td>
                        <td>2555</td>
                    </tr>
                </table>
            </div>
            <div class="takst-list kmlist2 hiddeli">
                <table style='text-align: center;'>
                    <tr>
                        <th></th>
                        <th>Kolding</th>
                        <th>Herning</th>
                        <th>Århus</th>
                        <th>Aalborg</th>
                    </tr>
                    <tr class='thred'>
                        <td>km</td>
                        <td>Betjener<br>Sydjylland</td>
                        <td>Betjener<br>det vestlige<br>Midjylland</td>
                        <td>Betjener<br>det østlige<br>Midjylland</td>
                        <td>Betjener<br>Nordjylland</td>
                    </tr>
                    <tr class='thred'>
                        <td></td>
                        <td>6000-6999</td>
                        <td>7000-7999</td>
                        <td>8000-8999</td>
                        <td>9000-9999</td>
                    </tr>
                    <tr class='trblue'>
                        <td><b>Hvidovre</b><br>1000-3699</td>
                        <td>225</td>
                        <td>300</td>
                        <td>300</td>
                        <td>410</td>
                    </tr>
                    <tr class='trred'>
                        <td><b>Rønne</b><br>3700-3799</td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr class='trblue'>
                        <td><b>Hvidovre</b><br>4000-4799</td>
                        <td>140</td>
                        <td>220</td>
                        <td>220</td>
                        <td>325</td>
                    </tr>
                    <tr class='trred'>
                        <td><b>Hvidovre</b><br>4800-4999</td>
                        <td>235</td>
                        <td>315</td>
                        <td>310</td>
                        <td>420</td>
                    </tr>
                    <tr class='trblue'>
                        <td><b>Odense</b><br>5000-5999</td>
                        <td>70</td>
                        <td>150</td>
                        <td>145</td>
                        <td>255</td>
                    </tr>
                    <tr class='trred'>
                        <td><b>Kolding</b><br>6000-6999</td>
                        <td>X</td>
                        <td>105</td>
                        <td>100</td>
                        <td>210</td>
                    </tr>
                    <tr class='trblue'>
                        <td><b>Herning</b><br>7000-7999</td>
                        <td>105</td>
                        <td>X</td>
                        <td>85</td>
                        <td>130</td>
                    </tr>
                    <tr class='trred'>
                        <td><b>Århus</b><br>8000-8999</td>
                        <td>100</td>
                        <td>85</td>
                        <td>X</td>
                        <td>120</td>
                    </tr>
                    <tr class='trblue'>
                        <td><b>Aalborg</b><br>9000-9999</td>
                        <td>210</td>
                        <td>130</td>
                        <td>120</td>
                        <td>X</td>
                    </tr>
                </table>
            </div>
    </body>
</html>