   M S B A M L   `   `   `  ���� F  BPieControls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null>.clr-namespace:PieControls;assembly=PieControlsPieControls  O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem | xSystem.Windows.Controls.DataVisualization.Toolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35�rclr-namespace:System.Windows.Controls.DataVisualization;assembly=System.Windows.Controls.DataVisualization.Toolkit)System.Windows.Controls.DataVisualization �{clr-namespace:System.Windows.Controls.DataVisualization.Charting;assembly=System.Windows.Controls.DataVisualization.Toolkit2System.Windows.Controls.DataVisualization.Charting  TecCargo Faktura System-   'TecCargo_Faktura.WindowsView.AdminPanel   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     >pie.clr-namespace:PieControls;assembly=PieControls     6sys&clr-namespace:System;assembly=mscorlib     �datavisrclr-namespace:System.Windows.Controls.DataVisualization;assembly=System.Windows.Controls.DataVisualization.Toolkit     �chart{clr-namespace:System.Windows.Controls.DataVisualization.Charting;assembly=System.Windows.Controls.DataVisualization.Toolkit     -     � Title$  Admin Kontrolpanel��$
��2000��$
��1200�� � WindowState$ 	Maximized=� � WindowStartupLocation$ CenterScreen=� � Icon$> 8/TecCargo Faktura System;component/Images/Icons/logo.ico��.���  � RowDefinitions �� $��70 ��� $��* ��� $��50 �.+�� $��0��$��Stretch=�$
��Auto�����	#FF2C72B8.+��� $��Right=�$��60��$��0,5,10,5q�$��Center=�$��55��$<��6/TecCargo Faktura System;component/Images/Pdf/logo.png���� -    ��Name$ ButtonAdmin_Startmenu��$��
Start Menu��$��67��$
��Left=�$��	10,10,0,0q�$	��Top=��� $��1��$��0,10,0,0q�.��� $
��auto��$��Center=� �� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto �.+�9� $��0��$��Indstillinger��$
��Left=�$��0,0,0,0q�$	��Top=�$
��1000��.��� $
��auto��$
��auto�� �� $
��auto ��� $
��auto �.+��� -   $ ButtonAdmin_UseBoldPDF��$��0��$��Brug fed skrift i PDF��$
��Left=�$��	10,0,10,0q�$��10,10,10,10q�$	��Top=��� -   $ ButtonAdmin_ResetToNew��$��0��$��Indstil som ny��$��	10,0,10,0q�$��150,10,10,10q�$	��Top=�$
��Left=�� $��1��$��0��$
��auto��$
��auto��$��10,0,0,0q� �� $
��auto ��� $
��auto �.+��� $��0�� �� Padding$ 5q����	#FFB8B8B8$��1q� �� CornerRadius$ 10��$
��Left=�$
��auto��$��0,0,0,10q�$	��Top=�$
��auto��._��  �� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto � � ColumnDefinitions �� $
��auto ��� $
��auto �.+�� $��0��$��0��$��0,0,0,0q�$
��auto��$
��auto��$	��Top=�$
��Left=� �� $
��auto ��� $
��auto � �� $
��auto ��� $
��auto ��� $
��auto �.+��� $��0��$��0��$��3��$��Søg efter fragtbreve��$��0,0,0,0q�$	��Top=�$��Center=�	� -   	 	�Name$	 radioButtonSearch_0��$��1��$��0��$��Kontakt person��
 d� 	IsChecked$

 TrueY�$��0,0,15,0q�$��0,0,0,0q�$	��Top=�$
��Left=�	� -   $	 radioButtonSearch_1��$��1��$��1��$
��Dato��$
 FalseY�$��0,0,15,0q�$��0,0,0,0q�$	��Top=�$
��Left=�	� -   $	 radioButtonSearch_2��$��1��$��2��$��Post nummer��$
 FalseY�$��0,0,15,0q�$��0,0,0,0q�$	��Top=�$
��Left=�� -    �Name$ gridSearch_KontP��$��1��$��0��.+��� $��Kontakt person��$��0,0,0,0q�$	��Top=�$
��Left=��� -	    ��Name$ textBoxFragt_KontP��$	��100��$��24��$��	100,2,0,0q�$	��Top=�$
��Left=�� -
   $ gridSearch_Date��$y�	Collapsed=�$��2��$��0��.+��� $��Start��$��0,0,0,0q�$	��Top=�$
��Left=��� $
��Slut��$��	160,0,0,0q�$	��Top=�$
��Left=�(  "System.Windows.Controls.DatePicker  -     Name$ datepickerSearchS��$��40,1,0,0q�$	��Top=�$
��Left=�  -   $ datepickerSearchE��$��	190,1,0,0q�$	��Top=�$
��Left=�� -   $ gridSearch_Post��$y�	Collapsed=�$��3��$��0��.+��� $��Post nr.��$��0,0,0,0q�$	��Top=�$
��Left=��� -   $ textBoxFragt_post��$��50��$��24��$��60,1,0,0q�$	��Top=�$
��Left=��� -   $��4��$��0��$	��100��$
��Søg��$��	10,5,10,5q�$	��Top=�$
��Bold"�$��Center=��� -    ��Name$ listViewSearch_Output��$��1��$y�	Collapsed=�$��0��$��2��$
��auto��$
��auto��$	��500��$��0,0,0,10q�$	��Top=�$
��Left=� �� View �� .P��� $��Filnavn�� �� Width$	 130�� �� DisplayMemberBinding �� *value1+�� $
��Dato��$	 130�� �� *value2+�� $��Opretter��$
 auto�� �� *value3+�� $	 100�� �� CellTemplate �� .R��� -   $��Åben som PDF��$��0q�$��Center=�9� $��2��$��Kunde Database��$
��Left=�$��0,0,0,0q�$	��Top=�$
��1000��.��� $
��auto��$
��auto��.+�&   System.Windows.Controls.DataGrid  -     Name$  DataGridAdmin_CustomerData��   CanUserAddRows .     AutoGenerateColumns .  $
��Left=�$��
10,10,10,0q�$	��Top=�$
��auto��$	��120������ *AdminKundeDataView+   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ Kontakt Nr.��   Width5  /System.Windows.Controls.DataGridLengthConverter$	 100    
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *KontaktNumber+  $ 
Firma/Navn��$	 140  .  �� *	FirmaName+  $ Adresse��$	 140  .  �� *	Adresse+  $ Post Nr.��$ 70  .  �� *
PostNumber+4  .System.Windows.Controls.DataGridCheckBoxColumn  $
 Slet��$ 40  �� *Delete+�� -   $! ButtonAdminCustom_deleteAll��$��	Slet alle��$��67��$
��Left=�$��352,140,0,10q�$	��Top=��� -   $ ButtonAdminCustom_delete��$��Slet valgte��$��77��$
��Left=�$��424,140,0,10q�$	��Top=�9� $��3��$��
Fragtbreve��$
��Left=�$��0,0,0,0q�$	��Top=�$
��1000��.��� $
��auto��$
��auto�� �� $
��auto ��� $
��auto �.+�� $��0��$
��auto��$��0q�$	��Top=�$
��Left=� �� $
��auto ��� $
��auto �.+�  -   $  DataGridAdmin_FragtPdfData��$��0�� .   .  $
��Left=�$��
10,10,10,0q�$	��Top=�$
��auto��$
��auto��$	��600������ *AdminFragtbrevPdfDataView+   $ Filnavn�� . $	 150  �� *
FileName+  $ Ændringsdato�� . $	 100  �� *
LastModify+  $
 Slet��$ 40  �� *Delete+�� -   $' !ButtonAdminFragtbrevPdf_deleteAll��$��1��$��	Slet alle��$��67��$��Right=�$��	0,5,90,10q�$	��Top=��� -   $$ ButtonAdminFragtbrevPdf_delete��$��1��$��Slet valgte��$��77��$��Right=�$��	0,5,10,10q�$	��Top=�� $��1��$
��auto��$��0q�$	��Top=�$
��Left=� �� $
��auto ��� $
��auto �.+�  -   $  DataGridAdmin_FragtXmlData��$��0�� .   .  $
��Left=�$��
10,10,10,0q�$	��Top=�$
��auto��$
��auto��$	��600������ *AdminSaveFragtbrevDataView+   $ Filnavn�� . $	 150  �� *
FileName+  $ Ændringsdato�� . $	 100  �� *
LastModify+  $
 Slet��$ 40  �� *Delete+�� -   $$ ButtonAdminFragtbrevXml_delete��$��1��$��Slet valgte��$��77��$��Right=�$��	0,5,10,10q�$	��Top=��� -   $' !ButtonAdminFragtbrevXml_deleteAll��$��1��$��	Slet alle��$��67��$��Right=�$��	0,5,90,10q�$	��Top=�9� $��4��$��Faktura��$
��Left=�$��0,0,0,0q�$	��Top=�$
��1000��.��� $
��auto��$
��auto�� �� $
��auto ��� $
��auto �.+�� $��0��$
��auto��$��0q�$	��Top=�$
��Left=� �� $
��auto ��� $
��auto �.+�  -   $" DataGridAdmin_FakturaPdfData��$��0�� .   .  $
��Left=�$��
10,10,10,0q�$	��Top=�$
��auto��$
��auto��$	��600������ *AdminFragtbrevPdfDataView+   $ Filnavn�� . $	 150  �� *
FileName+  $ Ændringsdato�� . $	 100  �� *
LastModify+  $
 Slet��$ 40  �� *Delete+�� -   $) #ButtonAdminFragtbrevPdf_delete_Copy��$��1��$��Slet valgte��$��77��$��Right=�$��	0,5,10,10q�$	��Top=��� -   $, &ButtonAdminFragtbrevPdf_deleteAll_Copy��$��1��$��	Slet alle��$��67��$��Right=�$��	0,5,90,10q�$	��Top=�� $��1��$
��auto��$��0q�$	��Top=�$
��Left=� �� $
��auto ��� $
��auto �.+�  -   $" DataGridAdmin_FakturaXmlData��$��0�� .   .  $
��Left=�$��
10,10,10,0q�$	��Top=�$
��auto��$
��auto��$	��600������ *AdminSaveFragtbrevDataView+   $ Filnavn�� . $	 150  �� *
FileName+  $ Ændringsdato�� . $	 100  �� *
LastModify+  $
 Slet��$ 40  �� *Delete+�� -   $) #ButtonAdminFragtbrevXml_delete_Copy��$��1��$��Slet valgte��$��77��$��Right=�$��	0,5,10,10q�$	��Top=��� -    $, &ButtonAdminFragtbrevXml_deleteAll_Copy��$��1��$��	Slet alle��$��67��$��Right=�$��	0,5,90,10q�$	��Top=�9� $��1�� 9� 
IsExpanded . $��Diagram Status��$
��Left=�$��0,0,0,0q�$	��Top=�$
��1000��.��� $
��auto��$
��auto��$	��Top=�$
��Left=�$��0,0,0,0q�.+��  �� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto � �� $
��auto ��� $
��auto �.+�� $��0��$��0��$
��auto��$
��auto��$��0,0,0,0q�$	��Top=�$
��Left=� �� $
��auto ��� $
��auto �.+�>  8System.Windows.Controls.DataVisualization.Charting.Chart  -!   $��pieFragt_Status��$��0��   Title$ Fragtbreve Status��$	��400��$	��200��
������ 
������ 
����  �   Series B	  <System.Windows.Controls.DataVisualization.Charting.PieSeries	  H
  BSystem.Windows.Controls.DataVisualization.Charting.DataPointSeries 
  IndependentValueBinding ��  �� Path$	 Key�  
  DependentValueBinding  �� $ Value�9� $��1��$��Status info��$
��auto��$��10,-20,0,10q�$	��Top=�$
��Left=�.��� $
��auto��$
��auto��$	��Top=�$
��Left=�$��0,0,0,0q�.+��� -"   $ listViewFragt_NotDone��$
��auto��$	��470��$
��auto��$	��500��$��	25,10,0,0q�$	��Top=�$
��Left=� �� .P��� $��Filnavn��$	 130�� �� *value1+�� $
��Dato��$ 90�� �� *value2+�� $��Opretter��$
 auto�� �� *value3+�� $��Status��$
 auto�� �� *value4+� $��0��$��1��$
��auto��$
��auto��$��0,0,0,0q�$	��Top=�$
��Left=� �� $
��auto ��� $
��auto �.+�  -#   $��pieFaktura_Status��$��0��$ Faktura Status��$	��400��$	��200��
������ 
������ 
����  � 	   �� $	 Key�  �� $ Value�9� $��1��$��Status info��$
��auto��$��10,-20,0,10q�$	��Top=�$
��Left=�.��� $
��auto��$
��auto��$	��Top=�$
��Left=�$��0,0,0,0q�.+��� -$   $ listViewFakt_NotDone��$
��auto��$	��470��$
��auto��$	��500��$��	25,10,0,0q�$	��Top=�$
��Left=� �� .P��� $��Filnavn��$	 130�� �� *value1+�� $
��Dato��$ 90�� �� *value2+�� $��Opretter��$
 auto�� �� *value3+�� $��Status��$
 auto�� �� *value4+� $��1��$��0��$
��auto��$
��auto��$��0,0,0,0q�$	��Top=�$
��Left=� �� $
��auto ��� $
��auto �.+�  -%   $��pieFragt_PdfDone��$��0��$! Afsluttet Fragtbreve Status��$	��400��$	��200��
������ 
������ 
����  � 	   �� $	 Key�  �� $ Value��� $��1��$��0��$	��290��$	��100��$	��Top=�$��0,0,0,10q�$��Center=�.��� .+��� $��Vis fra��$��0,0,0,0q�$	��Top=�$��Center=��� $��Start��$��	40,10,0,0q�$	��Top=�$
��Left=��� $
��Slut��$��
190,10,0,0q�$	��Top=�$
��Left=�  -&   $ datePickDateFragtS��! �� MinValue"   SelectedDate#" Z! $	��120��$	��Top=�$
��Left=�$��0,30,0,0q�  -'   $ datePickDateFragtE��
# �� Now#" Z# $	��120��$	��Top=�$
��Left=�$��
150,30,0,0q�	� -(   $	 radioSelectFragtUge��$	��Uge��$��	20,70,0,0q�$	��Top=�$
��Left=�	� -)   $	 radioSelectFragtMouth��$

 TrueY�$��Måned��$��
110,70,0,0q�$	��Top=�$
��Left=�	� -*   $	 radioSelectFragtYear��$	��År��$��
220,70,0,0q�$	��Top=�$
��Left=�� $��1��$��1��$
��auto��$
��auto��$��0,0,0,0q�$	��Top=�$
��Left=� �� $
��auto ��� $
��auto �.+�  -+   $��pieFaktura_PdfDone��$��0��$ Afsluttet Faktura Status��$	��400��$	��200��
������ 
������ 
����  � 	   �� $	 Key�  �� $ Value��� $��1��$��1��$	��290��$	��100��$	��Top=�$��Center=�.��� .+��� $��Vis fra��$��0,0,0,0q�$	��Top=�$��Center=��� $��Start��$��	40,10,0,0q�$	��Top=�$
��Left=��� $
��Slut��$��
190,10,0,0q�$	��Top=�$
��Left=�  -,   $ datePickDateFakturaS��#" Z! $	��120��$	��Top=�$
��Left=�$��0,30,0,0q�  --   $ datePickDateFakturaE��#" Z# $	��120��$	��Top=�$
��Left=�$��
150,30,0,0q�	� -.   $	 radioSelectFakturaUge��$	��Uge��$��	20,70,0,0q�$	��Top=�$
��Left=�	� -/   $	 radioSelectFakturaMouth��$

 TrueY�$��Måned��$��
110,70,0,0q�$	��Top=�$
��Left=�	� -0   $	 radioSelectFakturaYear��$	��År��$��
220,70,0,0q�$	��Top=�$
��Left=�� $��2��$��0��$��2�� �� $
��auto ��� $
��auto �.+�  -1   $  Name$$ chartFaktura_Price��$��0��$ 
Indtjening��$	��300��
������ 
����  ��� $��1��$��0��$��2��$
��auto��$
��auto��$	��Top=�$��0,0,0,10q�$��Center=�.���  �� $
��auto ��� $
��auto ��� $
��auto � �� $
��auto ��� $
��auto �.+�� $��0��$��0��$
��auto��$
��auto��$��7,10,0,0q�$	��Top=�$
��Left=�.+�	� -2   $	 radioSelectFaktp_0��$
 FalseY�$	��Uge��$��0,0,0,0q�$��Bottom=�$
��Left=�	� -3   $	 radioSelectFaktp_1��$
 FalseY�$��Måned��$��	110,0,0,0q�$	��Top=�$
��Left=�	� -4   $	 radioSelectFaktp_2��$

 TrueY�$	��År��$��	220,0,0,0q�$	��Top=�$
��Left=�� -5   $ gridFaktMonth��$y�	Collapsed=�$��1��$��0��$	��246��$
��auto��$	��Top=�$��23,0,0,0q�$
��Left=�.+��� -6   % ��Name$% checkboxFaktPriceM_0��$

 TrueY�$��0,0,0,0q�$	��Top=�$
��Left=��� -7   $% checkboxFaktPriceM_1��$

 TrueY�$��19,0,0,0q�$	��Top=�$
��Left=��� -8   $% checkboxFaktPriceM_2��$

 TrueY�$��38,0,0,0q�$	��Top=�$
��Left=��� -9   $% checkboxFaktPriceM_3��$

 TrueY�$��57,0,0,0q�$	��Top=�$
��Left=��� -:   $% checkboxFaktPriceM_4��$

 TrueY�$��76,0,0,0q�$	��Top=�$
��Left=��� -;   $% checkboxFaktPriceM_5��$

 TrueY�$��95,0,0,0q�$	��Top=�$
��Left=��� -<   $% checkboxFaktPriceM_6��$

 TrueY�$��	114,0,0,0q�$	��Top=�$
��Left=��� -=   $% checkboxFaktPriceM_7��$

 TrueY�$��	133,0,0,0q�$	��Top=�$
��Left=��� ->   $% checkboxFaktPriceM_8��$

 TrueY�$��	152,0,0,0q�$	��Top=�$
��Left=��� -?   $% checkboxFaktPriceM_9��$

 TrueY�$��	171,0,0,0q�$	��Top=�$
��Left=��� -@   $% checkboxFaktPriceM_10��$

 TrueY�$��	190,0,0,0q�$	��Top=�$
��Left=��� -A   $% checkboxFaktPriceM_11��$

 TrueY�$��	209,0,0,0q�$	��Top=�$
��Left=��� $��Januar��$
��Left=�$	��100��$��
-41,50,0,0q�$	��Top=�& J� RenderTransformOrigin$& 0.5,0.5&�z�V� .~��� �� �� ' �� Angle$' 90Q�T� �� $��Februar��$
��Left=�$	��100��$��
-23,50,0,0q�$	��Top=�$& 0.5,0.5&�z�V� .~��� �� �� $' 90Q�T� �� $��Marts��$
��Left=�$	��100��$��	-3,50,0,0q�$	��Top=�$& 0.5,0.5&�z�V� .~��� �� �� $' 90Q�T� �� $��April��$
��Left=�$	��100��$��	16,50,0,0q�$	��Top=�$& 0.5,0.5&�z�V� .~��� �� �� $' 90Q�T� �� $	��Maj��$
��Left=�$	��100��$��	35,50,0,0q�$	��Top=�$& 0.5,0.5&�z�V� .~��� �� �� $' 90Q�T� �� $
��Juni��$
��Left=�$	��100��$��	54,50,0,0q�$	��Top=�$& 0.5,0.5&�z�V� .~��� �� �� $' 90Q�T� �� $
��Juli��$
��Left=�$	��100��$��	73,50,0,0q�$	��Top=�$& 0.5,0.5&�z�V� .~��� �� �� $' 90Q�T� �� $��August��$
��Left=�$	��100��$��	92,50,0,0q�$	��Top=�$& 0.5,0.5&�z�V� .~��� �� �� $' 90Q�T� �� $��	September��$
��Left=�$	��100��$��
111,50,0,0q�$	��Top=�$& 0.5,0.5&�z�V� .~��� �� �� $' 90Q�T� �� $��Oktober��$
��Left=�$	��100��$��
130,50,0,0q�$	��Top=�$& 0.5,0.5&�z�V� .~��� �� �� $' 90Q�T� �� $��November��$
��Left=�$	��100��$��
149,50,0,0q�$	��Top=�$& 0.5,0.5&�z�V� .~��� �� �� $' 90Q�T� �� $��December��$
��Left=�$	��100��$��
168,50,0,0q�$	��Top=�$& 0.5,0.5&�z�V� .~��� �� �� $' 90Q�T� � -B   $ gridFaktYear��$y�	Collapsed=�$��0��$��1��$��2��$	��137��$
��auto��$��0,10,0,0q�$	��Top=�$
��Left=�.+��� -C   $ listviewFaktPrice_years��$
��Left=�$	��125��$��0,0,0,0q�$	��Top=�$	��137�� �� .P��� $��Vis For��$ 70���� $ 50�� �� .R��� -D   $��Fjern��$��0q�$��Center=��� -E   $ buttonFaktPrice_years��$��Tilføj��$
��Left=�$��
97,130,0,0q�$	��Top=�$��40���� -F   ( ��Name$( comboboxFaktPrice_years��$
��Left=�$��	0,130,0,0q�$	��Top=�$��81���� -G   $��2��$��0��$��2��$��Hent Diagram��$��Right=�$��	10,0,10,0q�$��0,10,0,0q�$	��Top=�$
��auto��� $��2��$��Stretch=�$
��Auto�����	#FF2C72B8.+��� -H   ) ��Name$) LabelVersion_version��$��	Version: ��$
��Left=�$	��Top=��� $��Kodet af: Simon Skov��$
��Left=�$��0,24,0,0q�$	��Top=�