<?php

    include_once '../../config.php';
    getUserRole(TRUE, TRUE);
    
    //henter menu
    
    ob_start();
    include '../../../themes/navbarAdminT.php';
    $NavBar = ob_get_contents();
    ob_end_clean();
    
    ob_start();
?>
<div>
    <img src="../../../images/admin/left_circular.png" style="position: absolute; cursor: pointer; height: 50px;" onclick="previousPage();">
    
    <div class="adminNBeregn adminForm">
        <div class="adminNbox"><h2>Beregn zoner</h2></div>
        <div class="begregnInfobox">
            <h4 id="beregn_Info">Vælg start post nummer</h4>
        </div>
        <div class="beregnResultBox">
            <button id="beregnKiloReset" class="beregnReset">Reset</button>
            <div>
                <table class="beregnTableA">
                    <tr>
                        <th>Start</th>
                        <th>Slut</th>
                        <th>Kilo</th>
                    </tr>
                    <tr>
                        <td id="post_1"></td>
                        <td id="post_2"></td>
                        <td id="post_3"></td>
                    </tr>
                </table>
            </div>
        </div>
        <table class="beregnButton">
            <tr>
                <td><button>1000-3699</button></td>
                <td><button>3700-3799</button></td>
                <td><button>4000-4799</button></td>
            </tr>
            <tr>
                <td><button>4800-4999</button></td>
                <td><button>5000-5999</button></td>
                <td><button>6000-6999</button></td>
            </tr>
            <tr>
                <td><button>7000-7999</button></td>
                <td><button>8000-8999</button></td>
                <td><button>9000-9999</button></td>
            </tr>
        </table>
        <div class="beregnKilo" style="display: none;">
            <input id="post_kilo" class="kiloInput" type="text">
            <button id="post_kiloSubmit" class="begregnkiloSubmit" style="margin: 4px 10px 4px;">Enter</button>
        </div>
    </div>
</div>
<?php
$Index = ob_get_contents();
ob_end_clean();

include_once '../../../themes/indexT.php';