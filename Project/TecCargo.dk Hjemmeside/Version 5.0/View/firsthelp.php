<?php


require_once(getcwd().'/plugins/FPDF/fpdf.php');
require_once(getcwd().'/plugins/FPDI/fpdi.php');

class firsthelp{
    
    public function __construct() {
        $this->Firsthelp();
    }
    
    public function Firsthelp($param = null) {
        
        ?>
<style>
    .editBox { 
        background-image:url(<?php echo "images/firsthelp.png"; ?>); 
        background-repeat:no-repeat;
        background-size:cover;
        height: 1600px;
        width: 1200px;
        margin: 0 auto;
    }
    .editBox .inputText{
        
        position: absolute;
        margin-left: 595px;
        width: 534px;
        background-color: #C1C1C1;
        border: 1px solid #000;
        margin-top: 164px;
        height: 51px;
        font-size: 20px;
        padding-left: 20px;
    }
</style>
<div class="editBox">
    <form method="post">
        <input type="hidden" name="action" value="pdf"/>

        <input class="inputText" style="margin-top: 164px;" name="" type="text"/>
        <input class="inputText" style="margin-top: 164px;" name="" type="text"/>
        <input class="inputText" style="margin-top: 164px;" name="" type="text"/>
        <input class="inputText" style="margin-top: 164px;" name="" type="text"/>
        <input class="inputText" style="margin-top: 164px;" name="" type="text"/>
</form>
</div>
        <?php
    }
    
    public function CreatPDF() {

        // initiate FPDI
        $pdf = new FPDI();
        
        // add a page
        $pdf->AddPage();
        // set the source file
        $pdf->setSourceFile(getcwd()."/files/firsthelp.pdf");
        // import page 1
        $tplIdx = $pdf->importPage(1);
        // use the imported page and place it at point 10,10 with a width of 100 mm
        $pdf->useTemplate($tplIdx);

        // now write some text above the imported page
        $pdf->SetFont('Helvetica',"",10);
        $pdf->SetTextColor(0, 0, 0);
        $pdf->SetXY(105, 33);
        $pdf->Write(0, 'This is just a simple text');
        
        $pdf->SetXY(105, 42);
        $pdf->Write(0, 'This is just a simple text');
        
        $pdf->SetXY(105, 51);
        $pdf->Write(0, 'This is just a simple text');
        
        $pdf->SetXY(105, 60);
        $pdf->Write(0, 'This is just a simple text');
        
        $pdf->SetXY(105, 69);
        $pdf->Write(0, 'This is just a simple text');

        $pdf->Output();
        
        
    }
}