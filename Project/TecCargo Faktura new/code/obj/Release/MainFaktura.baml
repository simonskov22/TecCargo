   M S B A M L   `   `   `  ���� X  TXceed.Wpf.Toolkit, Version=2.3.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f4P:clr-namespace:Xceed.Wpf.Toolkit;assembly=Xceed.Wpf.ToolkitXceed.Wpf.Toolkit   TecCargo Faktura System"   TecCargo_Faktura.MainFaktura   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     RwpfToolsKit:clr-namespace:Xceed.Wpf.Toolkit;assembly=Xceed.Wpf.Toolkit     -     � Title$  
Faktura - ��$��889.95��$
��2185�� � Icon$ Images/Icons/logo.ico�� � WindowStartupLocation$ CenterScreen=� � WindowState$ 	Maximized=�.���  � RowDefinitions �� $
��auto �$��70���� $��* ��� $
��auto �$��50��.+��� $��1��$��0,0,0,10q�$
��Auto=�.��� -   $��0,0,0,0q�$
��auto��$��Center=� � ColumnDefinitions �� $��* ��� $��* �.+�� $��0��$��0,0,0,0q� �� $
��auto ��� $
��auto ��� $
��auto �.+��� $��0��$��Faktura Adresse��{�.  $
��Left=�$	��100��$��	10,10,0,0q�$	��Top=�$	��499��$
��Bold"�.��� $��	0,0,-2,-1q�.+��� $��Kontakt nr.��$
��Left=�$	��Top=�$��	19,10,0,0q�$��Normal"��� $��
Firma/Navn��$
��Left=�$	��Top=�$��	19,35,0,0q�$��Normal"��� $��Adresse��$
��Left=�$	��Top=�$��253,10,-155,0q�$��Normal"��� $��Post nr.��$
��Left=�$	��Top=�$��253,35,-153,0q�$��Normal"��� -    ��Name$ TextBoxFaktura_customId��$
��Left=�$��20��$��92,14,-82,0q� �� TextWrapping$
 Wrap=�$	��Top=�$	��140�� �� 	UndoLimit$	 110��$��Normal"�$��1000100���� -   $ TextBoxFaktura_firma��$
��Left=�$��20��$��92,39,-82,0q�$
 Wrap=�$	��Top=�$	��140��$��Normal"�$��1000101���� -   $ TextBoxFaktura_adresse��$
��Left=�$��20��$��
310,14,0,0q�$
 Wrap=�$	��Top=�$	��169��$��Normal"�$��1000102���� -   $ TextBoxFaktura_post��$
��Left=�$��20��$��
310,39,0,0q�$
 Wrap=�$	��Top=�$��50��$��Normal"�$��1000103���� -   	 ��Name$	 LabelFaktura_bynavn��$�� ��$
��Left=�$��
362,36,0,0q�$	��Top=�$��Normal"�$	��114��$��5q��� -   
 ��Name$
 ListBoxFaktura_firmalist��$y�Hidden=�$
��Left=�$
��auto��$��24��$��84��$��92,58,0,-42q�$	��Top=�$	��140���� $��0��$��Transport Type��{�.  $
��Left=�$	��100��$��
522,10,0,0q�$	��Top=�$	��489��$
��Bold"�.��� $��0,0,0,0q�.+��� -	    ��Name$ ComboBoxTransport_type1�� �� SelectedIndex$ 0��$
��Left=�$��	19,20,0,0q�$	��Top=�$	��120��$��Normal"��� -
   $ ComboBoxTransport_type2��$y�Hidden=�$
��Left=�$��
166,20,0,0q�$	��Top=�$	��120��$��Normal"��� -   $ ComboBoxTransport_type3��$y�Hidden=�$
��Left=�$��
312,20,0,0q�$	��Top=�$	��120��$��Normal"��� -   $	 LabelTransport_kilometer��$y�Hidden=�$��Antal kilometer:��$
��Left=�$��	20,46,0,0q�$	��Top=�$��Normal"��� -   $  TextBoxTransport_kilometer��$y�Hidden=�$
��Left=�$��0,1,0,0q�$��23��$��
113,49,0,0q�$
 Wrap=�$��0��$	��Top=�$��41��$��Normal"��� $��0��$��Generelt��{�.  $
��Left=�$	��Top=�$��91��$	��499��$
��Bold"�$��
10,123,0,0q�.��� $
��Left=�$	��489��$��	0,0,-2,-2q�.+��� $��Leveringsdato��$
��Left=�$��	19,10,0,0q�$	��Top=�$��Normal"�(  "System.Windows.Controls.DatePicker  -     Name$ DatePickerGenerelt_Dato��$
��Left=�$��
108,11,0,0q�$	��Top=�$	��170��$��Normal"�   SelectedDateFormat$
 Long=�"   Xceed.Wpf.Toolkit.TimePicker  -     Name$ DatePickerGenerelt_Tid��$
��Left=�   Format$ Custom=�   FormatString$
 H:mm��$��283,10,0,35q�$��24��$��83��$��Normal"��� $��Rabat��$
��Left=�$��
382,11,0,0q�$	��Top=�$��Normal"��� -   $ TextBoxGenerelt_Rabat��$
��Left=�$��23��$��
428,11,0,0q�$
 Wrap=�$��0��$��0,3,0,0q�$	��Top=�$��39��$��Normal"��� $��0��$��Tidsforbrug��{�.  $
��Left=�$��91��$��522,123,0,0q�$	��Top=�$	��489��$
��Bold"�.��� $��0,0,0,0q�.+��� -   $ TextBoxTidsforbrug_Load��$
��Left=�$��0,1,0,0q�$��23��$��	79,22,0,0q�$
 Wrap=�$��0��$	��Top=�$��26��$��Normal"��� -   $ TextBoxTidsforbrug_Unload��$
��Left=�$��0,1,0,0q�$��23��$��
199,22,0,0q�$
 Wrap=�$��0��$	��Top=�$��26��$��Normal"��� -   $ TextBoxTidsforbrug_Vente��$
��Left=�$��0,1,0,0q�$��23��$��
288,22,0,0q�$
 Wrap=�$��0��$	��Top=�$��26��$��Normal"��� -   $ TextBoxTidsforbrug_Helper��$
��Left=�$��0,1,0,0q�$��23��$��
416,22,0,0q�$
 Wrap=�$��0��$	��Top=�$��26��$��Normal"��� $��
Læsse tid��$
��Left=�$��	14,19,0,0q�$	��Top=�$��Normal"��� $��Aflæssning tid��$
��Left=�$��
109,19,0,0q�$	��Top=�$��Normal"��� $��	Vente tid��$
��Left=�$��
230,18,0,0q�$	��Top=�$��Normal"��� $��Medhjælper tid��$
��Left=�$��
319,19,0,0q�$	��Top=�$��Normal"�� $��1��$
��auto��$��	0,10,0,10q�$	��Top=�.+�&   System.Windows.Controls.DataGrid  -     Name$ DataGridTransport_Pakker��$y�Visible=�   CanUserAddRows .  $��Center=�   CanUserDeleteRows .     CanUserReorderColumns .   %� IsVirtualizing .     
IsReadOnly .     AutoGenerateColumns .  $
��auto��$	��250��$	��Top=�$
��auto������ *FakturaTransportDataView+�� -    ��Name$! CheckBoxEkstraText_Button_1��$��2��$��Tilføj Bemærkning��$
��Left=�$��10,5,0,0q�$	��Top=�$	��130��$��5,2,5,2q�
�������$
��Bold"�$
��Left=�|�.  
��� � �$��22���� -    ��Name$ GroupBoxEkstraText��$��2��$��Evt. Bemærkninger��$y�	Collapsed=�$
��Left=�$��	10,32,0,0q�$	��997��$
��Bold"�$
��auto��$	��Top=�.��� $��0,0,0,0q�.+��� -    ��Name$ RichTextBoxEktra_Comment��$
��Left=�$	��122��$	��Top=�$	��782��$��
26,10,0,10q�$��Normal"� � 	Resources  �� 
TargetType �� Property �� Value��%3   'J�       �� " J�.���  �@��Margin$ 0q�. �/� .T�J� .*�.*��� $� ��� $��0,0,0,0q�$��1��$
��auto��$
��auto��$��3�� �� $
��auto ��� $
��auto �.+��� -   $ GroupBoxEkstraGebyr��$��0��$y�Visible=�$��Ekstra Tillæg��$
��Left=�$	��359��$��0,10,0,0q�$	��Top=�$	��390��$
��Bold"�.��� $
��auto��$��Center=�.+��� -   $ CheckBoxGebyr_Button_0��$��Chaufførmedhjælper��$
��Left=�$��0,9,0,0q�$	��Top=�$	��130��$��5,2,5,2q�
���  ��
�������$
��Bold"�$
��Left=�|�.  �� -   $ CheckBoxGebyr_Button_1��$��Flytte tillæg��$
��Left=�$��0,36,0,0q�$	��Top=�$	��130��$��5,2,5,2q�
���  ��
�������$
��Bold"�$
��Left=�|�.  �� -   $ CheckBoxGebyr_Button_2��$��ADR-tillæg��$
��Left=�$��0,63,0,0q�$	��Top=�$	��130��$��5,2,5,2q�
���  ��
�������$
��Bold"�$
��Left=�|�.  �� -   $ CheckBoxGebyr_Button_3��$��Aften- og nattillæg��$
��Left=�$��0,90,0,0q�$	��top=�$	��130��$��5,2,5,2q�
���  ��
�������$
��Bold"�$
��Left=�|�.  �� -   $ CheckBoxGebyr_Button_4��$��Weekendtillæg��$
��Left=�$��	0,117,0,0q�$	��Top=�$	��130��$��5,2,5,2q�
���  ��
�������$
��Bold"�$
��Left=�|�.  �� -   $ CheckBoxGebyr_Button_5��$��Yderzonetillæg��$
��Left=�$��	0,144,0,0q�$	��Top=�$	��130��$��5,2,5,2q�
���  ��
�������$
��Bold"�$
��Left=�|�.  �� -    $ CheckBoxGebyr_Button_6��$��Byttepalletillæg��$
��Left=�$��	0,171,0,0q�$	��Top=�$	��130��$��5,2,5,2q�
���  ��
�������$
��Bold"�$
��Left=�|�.  �� -!   $ CheckBoxGebyr_Button_7��$��SMS servicetillæg��$
��Left=�$��	0,198,0,0q�$	��Top=�$	��130��$��5,2,5,2q�
���  ��
�������$
��Bold"�$
��Left=�|�.  �� -"   $ CheckBoxGebyr_Button_8��$��Adresse korrektion��$
��Left=�$��	0,225,0,0q�$	��Top=�$	��130��$��5,2,5,2q�
���  ��
�������$
��Bold"�$
��Left=�|�.  �� -#   $ CheckBoxGebyr_Button_9��$��
Bro afgift��$
��Left=�$��	0,252,0,0q�$	��Top=�$	��130��$��5,2,5,2q�
���  ��
�������$
��Bold"�$
��Left=�|�.  �� -$   $ CheckBoxGebyr_Button_10��$��
Vej afgift��$
��Left=�$��	0,279,0,0q�$	��Top=�$	��130��$��5,2,5,2q�
���  ��
�������$
��Bold"�$
��Left=�|�.  �� -%   $ CheckBoxGebyr_Button_11��$��Færge afgift��$
��Left=�$��	0,306,0,0q�$	��Top=�$	��130��$��5,2,5,2q�
���  ��
�������$
��Bold"�$
��Left=�|�.  �� -&   $ CheckBoxGebyr_TextBox_0��$��Right=�$
��Left=�$��20��$��	135,9,0,0q�$
 Wrap=�$��0��$	��Top=�$��60��$��Normal"��� -'   $ CheckBoxGebyr_TextBox_1��$��Right=�$
��Left=�$��20��$��
135,36,0,0q�$
 Wrap=�$��0��$	��Top=�$��60��$��Normal"��� -(   $ CheckBoxGebyr_TextBox_6��$��Right=�$
��Left=�$��20��$��135,171,0,0q�$
 Wrap=�$��0��$	��Top=�$��60��$��Normal"��� -)   $ CheckBoxGebyr_TextBox_7��$��Right=�$
��Left=�$��20��$��135,198,0,0q�$
 Wrap=�$��0��$	��Top=�$��60��$��Normal"��� -*   $ CheckBoxGebyr_TextBox_9��$��Right=�$
��Left=�$��20��$��135,254,0,0q�$
 Wrap=�$��0��$	��Top=�$��60��$��Normal"��� -+   $ CheckBoxGebyr_TextBox_10��$��Right=�$
��Left=�$��20��$��135,281,0,0q�$
 Wrap=�$��0��$	��Top=�$��60��$��Normal"��� -,   $ CheckBoxGebyr_TextBox_11��$��Right=�$
��Left=�$��20��$��135,308,0,0q�$
 Wrap=�$��0��$	��Top=�$��60��$��Normal"��� $��Antal personer��$
��Left=�$��	201,7,0,0q�$	��Top=�$��Normal"��� $��Antal personer/enheder��$
��Left=�$��
201,34,0,0q�$	��Top=�$��Normal"��� $��Antal paller��$
��Left=�$��201,168,0,0q�$	��Top=�$��Normal"��� $��Antal advisering��$
��Left=�$��201,194,0,0q�$	��Top=�$��Normal"��� $	��.Kr��$
��Left=�$��201,250,0,0q�$	��Top=�$��Normal"��� $	��.Kr��$
��Left=�$��201,277,0,0q�$	��Top=�$��Normal"��� $	��.kr��$
��Left=�$��201,304,0,0q�$	��Top=�$��Normal"��� --   $ GroupBoxPrislist��$��1��$y�Visible=�$��Prislist��$
��Left=�$
��auto��$	��360��$��	0,10,0,10q�$	��Top=�$	��390��$
��Bold"�.��� .+��� -.   $  CheckBoxPris_Button_Update��$��Opdater��$
��Left=�$��290,-18,0,0q�$	��Top=�$��80��$��5,2,5,2q�
�����_�
�������$
��Bold"�$��Center=�  -/   $ datagridPricelist��$��1��$��0,15,0,5q�$��Normal"�$��Center=� .   .  .   .   .  $	��Top=�$
��auto��$	��380������ *FakturaPriceDataView+    Columns  0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn!   Header$
! Navn��"   MinWidth$	" 150Q�1  +System.Windows.Controls.DataGridBoundColumn#   Binding# �� *name+  $
! Pris��$	" 100Q�# �� *price+$   	CellStyle$ �� .���  � ��$ Right=�� $��0��$��Stretch=�$
��Auto�����	#FF2C72B8.+��� -0   $y�Visible=�$
��Left=�$��30��$��	10,29,0,0q�$	��Top=�$
��auto��$��2000000��.���� $
��auto��$	��Top=�$
��auto��$
��Left=�$A��;Images/Icons/1416249682_698913-icon-81-document-add-128.png��% � ToolTip$% Nyt fragtbrev���� -1   $ toolbarButton_Save��$
��Left=�$��30��$
��auto��$��	45,29,0,0q�$	��Top=�$��2000000��$	% Gem��.���� -2   & ��Name$& toolbarButton_SaveImage��$
��auto��$	��Top=�$
��auto��$
��Left=�$&�� Images/Icons/1417631293_save.png���� -3   $
��Left=�$��30��$
��auto��$��	80,29,0,0q�$	��Top=�$��2000000��.���� $
��auto��$	��Top=�$
��auto��$
��Left=�$F��@Images/Icons/1417103306_699434-icon-70-document-file-pdf-128.png��$% Exporter til pdf���� -4   $ Button_EkstraGebyr_Show��$
��Left=�$��30��$
��auto��$��
157,29,0,0q�$	��Top=�$��2000000��.���� $
��auto��$	��Top=�$
��auto��$
��Left=�$9��3Images/Icons/1427293881_698889-icon-146-tag-128.png��$% Vis ekstra gebyr���� -5   $
��Left=�$��30��$
��auto��$��
192,29,0,0q�$	��Top=�$��2000000��.���� $
��auto��$	��Top=�$
��auto��$
��Left=�$C��=Images/Icons/1427293853_699335-icon-62-document-table-128.png��$% Vis prisliste���� $��Right=�$��60��$��0,5,20,5q�$��Center=�$��55��$��Images/Pdf/logo.png���� -6   $��Ekstra Tillæg��$
��Left=�$��	127,4,0,0q�$	��Top=�$��75���� -7   $ ToolsbarButton_Filer��$��Filer��$
��Left=�$
��Left=�$	��8,0q�$��20��$��10,4,0,0q�$	��Top=�' � IsSharedSizeScope' . �� -8   $ ButtonAdmin_Startmenu��$��
Start Menu��$��67��$
��Left=�$��55,4,0,0q�$	��Top=��� -9   $��Pris Listen��$
��Left=�$��	207,4,0,0q�$	��Top=�$��65���� -:   $
 ToolsbarMenu_Filer��$y�Hidden=�$
��Left=�$
��auto��$
��auto��$	��100��$��10,23,0,-85q�$	��Top=����	#FFDDDDDD$��1q����	#FF707070$��Disabled=�$��Disabled=�.@��� -;   $��Nyt Faktura���� -<   $��Nyt Fragtbrev���� -=   $��Åben Faktura���� ->   $��Åben Fragtbrev���� -?   ( ��Name$( toolbarButton_SaveList��$��Gem Faktura���� -@   $��Exporter Til PDF��� $��2��$��Stretch=�$
��Auto�����	#FF2C72B8.+��� -A   $	 LabelVersion_version��$��	Version: ��$
��Left=�$	��Top=��� $��Kodet af: Simon Skov��$
��Left=�$��0,24,0,0q�$	��Top=�