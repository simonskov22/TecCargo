   M S B A M L   `   `   `  ���� X  TXceed.Wpf.Toolkit, Version=2.3.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f4P:clr-namespace:Xceed.Wpf.Toolkit;assembly=Xceed.Wpf.ToolkitXceed.Wpf.Toolkit  O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem R NTecCargo Faktura System, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:TecCargo_Faktura.ModelsTecCargo_Faktura.Models $   TecCargo_Faktura.MainFragtBrev   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     RwpfToolsKit:clr-namespace:Xceed.Wpf.Toolkit;assembly=Xceed.Wpf.Toolkit     9System&clr-namespace:System;assembly=mscorlib     8Models%clr-namespace:TecCargo_Faktura.Models     -     �Name$  WindowFragt_index�� � Title$ Opret Fragtbrev��$��1879.334��$
��1085�� � WindowStartupLocation$ CenterScreen=� � Icon$ Images/Icons/logo.ico�� � WindowState$ 	Maximized=�.��� $��0,0,0,0q� � RowDefinitions �� $��70 ��� $��* ��� $��50 �.+��� -    ��Name$ ScrollviewBox��$��1��$
��Auto=�.��� -   $��Center=�$
��1077��$��0,0,0,20q�.+��� -    ��Name$ GroupBoxGenerelt��$��Generelt��$
��Left=�$	��171��$��
24,157,0,0q�$	��Top=�$
��1017��$
��Bold"�.��� $��
0,0,-2,-12q�.+��� -    ��Name$ TextBoxGenerelt_reference��$
��Left=�$��20��$��
139,14,0,0q�	 �� TextWrapping$
	 Wrap=�$	��Top=�$	��140��$��Normal"�$��1000301���� $��	Reference��$
��Left=�$	��Top=�$��	37,10,0,0q�$��Normal"��� $��
Dato dag 1��$
��Left=�$	��Top=�$��	332,8,0,0q�$��Normal"��� $��
Dato dag 2��$
��Left=�$	��Top=�$��
332,59,0,0q�$��Normal"�
 J� RenderTransformOrigin$
 0.583,0.308&��� $��	Fragtmand��$
��Left=�$	��Top=�$��	37,36,0,0q�$��Normal"��� -   $ TextBoxGenerelt_fragtmand��$
��Left=�$��20��$��
139,40,0,0q�$
	 Wrap=�$	��Top=�$	��140�� �� 	UndoLimit$	 110��$��Normal"�$��1000302���� $��Rute  dag 1��$
��Left=�$	��Top=�$��
332,33,0,0q�$��Normal"��� $��
Rute dag 2��$
��Left=�$	��Top=�$��
332,85,0,0q�$��Normal"��� $��Efterkrav inkl. gebyr��$
��Left=�$	��Top=�$��
609,33,0,0q�$��Normal"�$��26��$	��122���� $��Forsikringssum kr.��$
��Left=�$	��Top=�$��
609,61,0,0q�$��Normal"�$��26���� $��Præmie kr.��$
��Left=�$	��Top=�$��
609,86,0,0q�$��Normal"�$��26���� $��I alt. kr. inkl. moms

��$
��Left=�$	��Top=�$��609,110,0,0q�$��Normal"�$��26���� $��Forsikringstype��$
��Left=�$	��Top=�$��	37,61,0,0q�$��Normal"�$��26���� -   $ TextBoxGenerelt_efterkrav��$
��Left=�$��20��$��
739,39,0,0q�$
	 Wrap=�$	��Top=�$	��140��$��Normal"�$��1000308��{�.  �� -   $ TextBoxGenerelt_rute1��$
��Left=�$��20��$��
418,39,0,0q�$
	 Wrap=�$	��Top=�$	��140��$	 110��$��Normal"�$��1000305���� -	   $ TextBoxGenerelt_rute2��$
��Left=�$��20��$��
418,91,0,0q�$
	 Wrap=�$	��Top=�$	��140��$��Normal"�$��1000307���� -
   $  TextBoxGenerelt_forsikring��$
��Left=�$��20��$��
739,65,0,0q�$
	 Wrap=�$	��Top=�$	��140��$��Normal"�$��1000309��{�.  �� -   $ TextBoxGenerelt_praemie��$
��Left=�$��20��$��
739,90,0,0q�$
	 Wrap=�$	��Top=�$	��140��$��Normal"�$��1000310��{�.  �� -   $ TextBoxGenerelt_ialt��$
��Left=�$��20��$��739,116,0,0q�$
	 Wrap=�$	��Top=�$	��140��$	 110��$��Normal"�$��1000311��{�.  �� -    ��Name$ ComboBoxGenerelt_type��$
��Left=� �� SelectedIndex$ 0��$��
139,65,0,0q�$	��Top=�$��46��$
��auto��$��1000303��$��Normal"���-  'System.Windows.Data.CompositeCollection  �� $��Ingen���� $��A���� $��B���� $��C���� $��D��(  "System.Windows.Controls.DatePicker  -     Name$ DatePickerGenerelt_dag1��$
��Left=�$��
418,10,0,0q�$	��Top=�$	��140��   FirstDayOfWeek$ Monday=�$��1000304��$��Normal"�  -   $ DatePickerGenerelt_dag2��$
��Left=�$��
418,64,0,0q�$	��Top=�$	��140��$ Monday=�$��1000306��$��Normal"��� -    ��Name$  CheckBoxGenerelt_efterkrav��$��LeftToRight=�$�� ��$
��Left=�$��
673,13,0,0q�$	��Top=�$��Normal"�$��1000500���� $��
Efterkrav:��$
��Left=�$��	609,7,0,0q�$	��Top=�$��Normal"��� -   $ GroupBoxGodslinjer��$��
Godslinjer��$
��Left=�$��
24,410,0,0q�$
��1017��$
��Bold"�$	��254��$	��Top=�.��� $��	0,0,-2,-2q�.+�&   System.Windows.Controls.DataGrid  -     Name$ DataGridGodslinjer��$y�Hidden=� %� IsVirtualizing .     AutoGenerateColumns .  $
��Left=�$��	27,56,0,0q�$	��Top=�$	��168��$	��950��$��1000400��   CanUserReorderColumns .     CanUserSortColumns .  $��Normal"����� *godsIndhold+�� -    ��Name$" ButtonGodslinjer_deleteCells��$y�Hidden=�$��Slet ubrugte felter��$��24��$	��115��$��862,32,30,178q�$
��Bold"�
���  ��
��������� -   $&  ComboBoxGodslinjer_TransportType��$
��Left=�$��
153,14,0,0q�$	��Top=�$	��120��$��Normal"���  �� $��Kurertransport���� $��Pakketransport���� $��Godstransport���� -   $" ComboBoxGodslinjer_Pakketype��$y�Hidden=�$
��Left=�$��
304,14,0,0q�$	��Top=�$	��120��$
 0.825,1.182&�$��Normal"��� -   $  ComboBoxGodslinjer_Biltype��$
��Left=�$��
454,14,0,0q�$y�Hidden=�$	��Top=�$	��120��$��Normal"���  �� $��Ingen���� $��Grp. 1���� $��Grp. 2���� $��Grp. 3���� $��Grp. 4���� $��Transport type��$
��Left=�$��	61,10,0,0q�$	��Top=�$��Normal"��� -   $ GroupBoxByttepaller��$��Byttepaller��$
��Left=�$��72��$��
24,333,0,0q�$	��Top=�$
��1017��$
��Bold"�.��� .+��� $��1/2 Paller:��$
��Left=�$��26��$��
305,11,0,0q�$	��Top=�$��69��$��Normal"��� $��1/1 Paller:��$
��Left=�$��26��$��
162,10,0,0q�$	��Top=�$��72��$��Normal"��� $��1/4 Paller:��$
��Left=�$��26��$��
454,11,0,0q�$	��Top=�$��69��$��Normal"��� -   $ CheckBoxByttepalle_active��$��LeftToRight=�$�� ��$
��Left=�$��
100,16,0,0q�$	��Top=�$��Normal"�$��1000500���� -   $  TextBoxByttepalle_palle1_1��$
��Left=�$��
234,14,0,0q�$
	 Wrap=�$��49��$��Normal"�$��23��$	��Top=�$��1000501���� -   $  TextBoxByttepalle_palle1_4��$
��Left=�$��23��$��
526,14,0,0q�$
	 Wrap=�$	��Top=�$��49��$��Normal"�$��1000503���� -   $  TextBoxByttepalle_palle1_2��$
��Left=�$��23��$��
379,14,0,0q�$
	 Wrap=�$	��Top=�$��49��$��Normal"�$��1000502���� $��Byttepaller:��$
��Left=�$��	26,10,0,0q�$	��Top=�$��Normal"�$
 1.899,1.115&��� -   $ GroupBoxEvtComment��$��Evt. Bemærkninger��$
��Left=�$��
24,669,0,0q�$
��1017��$
��Bold"�$	��213��$	��Top=�.���� -    ��Name$ RichTextBoxEktra_Comment��$
��Left=�$	��140��$	��Top=�$	��800��$��	27,26,0,0q�$��Normal"� � 	Resources  �� 
TargetType �� Property �� Value��%3   'J�       �� " J�.���  �@��Margin$ 0q�. �/� .T�J� .*�.*��� $� ���� -   $ CloseFragtbrevBox��$y�Hidden=�{�.  $��Afslut Fragtbrev��$
��Left=�$��
24,887,0,0q�$
��1017��$
��Bold"�$	��145��$	��Top=�
���   ����	#FFF00000���	#FFF0F0F0.��� .+��� $��Generelt��$
��Left=�$	��Top=�$��91��$	��443��$
��Bold"�$��	25,21,0,0q�.��� $
��Left=�$	��433��$��	0,0,-2,-2q�.+��� $��Leveringsdato��$
��Left=�$��	19,10,0,0q�$	��Top=�$��Normal"�  -   $ DatePickerGenerelt_Dato��$
��Left=�$��
108,11,0,0q�$	��Top=�$	��170��$��Normal"�   SelectedDateFormat$
 Long=�"   Xceed.Wpf.Toolkit.TimePicker  -      Name$ DatePickerGenerelt_Tid��$
��Left=�   Format$ Custom=�   FormatString$
 H:mm��$��283,10,0,35q�$��24��$��83��$��Normal"��� $��Rabat��$
��Left=�$��	18,41,0,0q�$	��Top=�$��Normal"��� -!   $ TextBoxGenerelt_Rabat��$
��Left=�$��23��$��	64,41,0,0q�$
	 Wrap=�$��0��$��0,3,0,0q�$	��Top=�$��39��$��Normal"��� -"     ��Name$  LabelTransport_kilometer��$y�Visible=�$��Antal kilometer:��$
��Left=�$��
185,41,0,0q�$	��Top=�$��Normal"��� -#   $  TextBoxTransport_kilometer��$y�Visible=�$
��Left=�$��0,1,0,0q�$��23��$��
283,43,0,0q�$
	 Wrap=�$��0��$	��Top=�$��41��$��Normal"��� -$   $ GroupBoxTidforbrug��$��Tidsforbrug��$
��Left=�$��91��$��
496,21,0,0q�$	��Top=�$	��485��$
��Bold"�.��� $��0,0,0,0q�.+��� -%   $ TextBoxTidsforbrug_Load��$
��Left=�$��0,1,0,0q�$��23��$��	79,22,0,0q�$
	 Wrap=�$��0��$	��Top=�$��26��$��Normal"��� -&   $ TextBoxTidsforbrug_Unload��$
��Left=�$��0,1,0,0q�$��23��$��
199,22,0,0q�$
	 Wrap=�$��0��$	��Top=�$��26��$��Normal"��� -'   $ TextBoxTidsforbrug_Vente��$
��Left=�$��0,1,0,0q�$��23��$��
288,22,0,0q�$
	 Wrap=�$��0��$	��Top=�$��26��$��Normal"��� -(   $ TextBoxTidsforbrug_Helper��$
��Left=�$��0,1,0,0q�$��23��$��
416,22,0,0q�$
	 Wrap=�$��0��$	��Top=�$��26��$��Normal"��� $��
Læsse tid��$
��Left=�$��	14,19,0,0q�$	��Top=�$��Normal"��� $��Aflæssning tid��$
��Left=�$��
109,19,0,0q�$	��Top=�$��Normal"��� $��	Vente tid��$
��Left=�$��
230,18,0,0q�$	��Top=�$��Normal"��� $��Medhjælper tid��$
��Left=�$��
319,19,0,0q�$	��Top=�$��Normal"��� -)   $ GroupBoxAfsender��$��Afsender��$
��Left=�$	��119��$��	24,22,0,0q�$	��Top=�$	��503��$
��Bold"�.��� $��	0,0,-2,-2q�.+��� $��Kontakt nr.��$
��Left=�$	��Top=�$��	19,10,0,0q�$��Normal"��� $��
Firma/Navn��$
��Left=�$	��Top=�$��	19,35,0,0q�$��Normal"��� $��Adresse��$
��Left=�$	��Top=�$��264,10,-155,0q�$��Normal"��� $��Post nr.��$
��Left=�$	��Top=�$��264,35,-153,0q�$��Normal"��� -*   $ TextBoxAfsend_customId��$
��Left=�$��20��$��112,14,-82,0q�$
	 Wrap=�$	��Top=�$	��140��$	 110��$��Normal"�$��1000100���� -+   $ TextBoxAfsend_firma��$
��Left=�$��20��$��112,39,-82,0q�$
	 Wrap=�$	��Top=�$	��140��$��Normal"�$��1000101���� -,   $ TextBoxAfsend_adresse��$
��Left=�$��20��$��
321,14,0,0q�$
	 Wrap=�$	��Top=�$	��169��$��Normal"�$��1000102���� --   $ TextBoxAfsend_post��$
��Left=�$��20��$��
321,39,0,0q�$
	 Wrap=�$	��Top=�$��50��$��Normal"�$��1000103���� -.   $ CheckBoxAfsend_pay��$��LeftToRight=�$�� ��$
��Left=�$��
309,66,0,0q�$	��Top=�$��Normal"�$��15��$
 -0.4,-0.533&�$��1000104���� $��Betaler��$
��Left=�$��
264,60,0,0q�$	��Top=�$��Normal"��� -/   $  LabelAfsend_bynavn��$�� ��$
��Left=�$��
376,35,0,0q�$	��Top=�$��Normal"�$	��114��$��5q��� $��Kontakt person��$
��Left=�$	��Top=�$��	19,60,0,0q�$��Normal"��� -0   $ TextBoxAfsend_pKont��$
��Left=�$��20��$��
112,64,0,0q�$
	 Wrap=�$	��Top=�$	��140��$��Normal"�$��1000101���� -1   ! ��Name$! ListBoxAfsend_firmalist��$y�Hidden=�$
��Left=�$
��auto��$��24��$��84��$��112,58,0,-42q�$	��Top=�$	��140��$��Normal"��� -2   $ GroupBoxModtager��$��Modtager��$
��Left=�$	��119��$��
534,22,0,0q�$	��Top=�$	��503��$
��Bold"�.��� $��	0,0,-2,-3q�.+��� $��Kontakt nr.��$
��Left=�$	��Top=�$��	19,10,0,0q�$��Normal"��� $��
Firma/Navn��$
��Left=�$	��Top=�$��	19,35,0,0q�$��Normal"��� $��Adresse��$
��Left=�$	��Top=�$��
264,10,0,0q�$��Normal"��� $��Post nr.��$
��Left=�$	��Top=�$��
264,35,0,0q�$��Normal"��� -3   $ TextBoxModtag_customId��$
��Left=�$��20��$��112,14,-82,0q�$
	 Wrap=�$	��Top=�$	��140��$	 110��$��Normal"�$��1000200���� -4   $ TextBoxModtag_firma��$
��Left=�$��20��$��112,39,-82,0q�$
	 Wrap=�$	��Top=�$	��140��$��Normal"�$��1000201���� -5   $ TextBoxModtag_adresse��$
��Left=�$��20��$��
321,14,0,0q�$
	 Wrap=�$	��Top=�$	��169��$��Normal"�$��1000202���� -6   $ TextBoxModtag_post��$
��Left=�$��20��$��
321,39,0,0q�$
	 Wrap=�$	��Top=�$��50��$��Normal"�$��1000204���� -7   $ CheckBoxModtag_pay��$��LeftToRight=�$�� ��$
��Left=�$��
309,66,0,0q�$	��Top=�$��Normal"�$��15��$
 
-1.933,0.6&�$��1000205���� $��Betaler��$
��Left=�$��
264,60,0,0q�$	��Top=�$��Normal"��� -8   $  LabelModtag_bynavn��$�� ��$
��Left=�$��
376,36,0,0q�$	��Top=�$��Normal"�$	��114���� $��Kontakt person��$
��Left=�$	��Top=�$��	19,60,0,0q�$��Normal"��� -9   $ TextBoxModtag_pKont��$
��Left=�$��20��$��
112,64,0,0q�$
	 Wrap=�$	��Top=�$	��140��$��Normal"�$��1000101���� -:   $! ListBoxModtag_firmalist��$
��Left=�$
��auto��$��24��$y�Hidden=�$��84��$��112,58,0,-42q�$	��Top=�$	��140��$��Normal"�� $��2��$��Stretch=�$��50��$
��Auto�����	#FF2C72B8.+��� -;   $  LabelVersion_version��$��	Version: ��$
��Left=�$	��Top=��� $��Kodet af: Simon Skov��$
��Left=�$��0,24,0,0q�$	��Top=�� $��0��$��Stretch=�$
��Auto�����	#FF2C72B8.+��� -<   $ ToolsbarButton_Filer��$��Filer��$
��Left=�$
��Left=�$	��8,0q�$��20��$��10,4,0,0q�$	��Top=�" � IsSharedSizeScope" . �� -=   $ toolbarButton_save��$
��Left=�$��30��$
��auto��$��	45,29,0,0q�$	��Top=�$��2000000��# � ToolTip$	# Gem�����	#FFDDDDDD.���� ->   $ ��Name$$ toolbarButton_saveImage��$
��auto��$	��Top=�$
��auto��$
��Left=�$&�� Images/Icons/1417631293_save.png��% J� OpacityMask
% ������� -?   $
��Left=�$��30��$
��auto��$��	80,29,0,0q�$	��Top=�$��2000000��.���� $
��auto��$	��Top=�$
��auto��$
��Left=�$F��@Images/Icons/1417103306_699434-icon-70-document-file-pdf-128.png��$# Exporter til pdf��
% �   ��� -@   $
��Left=�$��30��$��	10,29,0,0q�$	��Top=�$
��auto��$��2000000��.���� $
��auto��$	��Top=�$
��auto��$
��Left=�$A��;Images/Icons/1416249682_698913-icon-81-document-add-128.png��$# Nyt fragtbrev���� $��Right=�$��60��$��0,5,21,5q�$��Center=�$��55��$��Images/Pdf/logo.png���� -A   $! ToolsbarMenu_Filer��$y�Hidden=�$
��Left=�$
��auto��$
��auto��$	��100��$��10,23,0,-85q�$	��Top=����	#FFDDDDDD$��1q����	#FF707070$��Disabled=�$��Disabled=�.@��� -B   $��Nyt Faktura���� -C   $��Nyt Fragtbrev���� -D   $��Åben Faktura���� -E   $��Åben Fragtbrev���� -F   & ��Name$& toolbarButtonListSave��$��Gem Fragtbrev���� -G   $��Exporter Til PDF���� -H   $ ButtonAdmin_Startmenu��$��
Start Menu��$��67��$
��Left=�$��55,4,0,0q�$	��Top=��� -I   $ ButtonTool_CloseFragtbrev��$��Afslut Fragtbrev��$y�Hidden=�{�.  $	��100��$
��Left=�$��	127,4,0,0q�$	��Top=�|�.  �� -J   $&  ButtonTool_CloseFragtbrevComment��$
��Left=�$��20��$��	227,4,0,0q�$y�Hidden=�{�.  $	��Top=�$��2000000��.���� $
��auto��$	��Top=�$
��auto��$
��Left=�$C��=Images/Icons/1416249686_698873-icon-136-document-edit-128.png��$# Exporter til pdf��