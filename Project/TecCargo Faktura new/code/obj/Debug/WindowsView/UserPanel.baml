   M S B A M L   `   `   `  ����X  TXceed.Wpf.Toolkit, Version=2.3.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f4P:clr-namespace:Xceed.Wpf.Toolkit;assembly=Xceed.Wpf.ToolkitXceed.Wpf.Toolkit  5   	   O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5   	   R NTecCargo Faktura System, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:TecCargo_Faktura.ModelsTecCargo_Faktura.Models 5   	   ?$clr-namespace:TecCargo_Faktura.ClassTecCargo_Faktura.Class 5   	   E'clr-namespace:TecCargo_Faktura.ControlsTecCargo_Faktura.Controls 5   	   ,   &TecCargo_Faktura.WindowsView.UserPanel   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   RwpfToolsKit:clr-namespace:Xceed.Wpf.Toolkit;assembly=Xceed.Wpf.Toolkit     5   	   9System&clr-namespace:System;assembly=mscorlib     5   	   8Models%clr-namespace:TecCargo_Faktura.Models     5   	   <Placeholder$clr-namespace:TecCargo_Faktura.Class     5   	   <TecCargo'clr-namespace:TecCargo_Faktura.Controls     5   	   -   5          �Name$  
_UserPanel��5   	    � Title$ Opret Fragtbrev��5   	   $	��500��6!   $
��1500��6.    � WindowStartupLocation$ CenterScreen=�6;    � WindowState$ 	Maximized=�5   	    � 	Resources 5      7  1TecCargo_Faktura.Models.BoolToVisibilityConverter   ShowPricelistConverter�  �TecCargo_Faktura.Models.BoolToValueConverter`1[[System.Windows.Visibility, PresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35]]   	TrueValue   
FalseValue  BoolToVisibilityConverterR��%�   &	        &	 6       5   
   $ Visible=�6J   $ 	Collapsed=�6^     5   
   $ Visible=�6N   $ 	Collapsed=�6b   5      .��5      �  � RowDefinitions 5   
   �� 5      $	��100 �6   �� 5      $��* �6   �� 5      $��50 �6   .+�5   
   �� -   5         ��Name$ ScrollViewBox��5      $��1��6.   $
��Auto=�5      $
��Auto=�5      .��5       � $��Center=�6   $
��1200��60   $��1��6@   	 � ColumnDefinitions	 5!      �� 5"      $
��auto �6'   �� 5#      $
��auto �6'   .+�5%      � -   5        $��Grid_Viewbox��5%      $��0��6,   $
��1000��6<    5&      �� 5'      $
��auto �6(   �� 5(      $
��auto �6(   �� 5)      $
��auto �6(   �� 5+      $
��auto �6(   �� 5,      $
��auto �6(   �� 5-      $
��auto �6(   �� 5/      $
��auto �6(   �� 50      $
��auto �6(   �� 51      $
��auto �6(   �� 53      $��* �6(   .+�56      � $��0��6   $y�	Collapsed=�6(   $
��Left=�6?   $��0,20,0,0q�6Z   	 57      �� 58      $
��auto �6/   �� 59      $
��auto �6/   �� 5:      $
��auto �6/   .+�5=      '  !TecCargo_Faktura.Controls.Address  -   5        
  Name$
 Address_Afsender��5=   +   '  !TecCargo_Faktura.Controls.Address   PayClick$ Address_Contact_PayClick��6{   $��0��6E      Header$ 	Afsender:��6U      	TextCheck$ Franko��6h   $
��10,0q�6�   !   ShowAlwaysPlaceholderOnTop 6�   �� *ChangeHeaderInfoState+ �� ElementName$ 
_UserPanel��  5>      -   5        $
 Address_Modtager��5>   +   $ Address_Contact_PayClick��6|   $��1��6E   $ 	Modtager:��6U   $ Ufranko��6h   $
��10,0q�6�    6�   �� *ChangeHeaderInfoState+$ 
_UserPanel��  5?      -   5        $
 Address_Modtager2��5?   +   $ Address_Contact_PayClick��6�   $��2��6F   $ Anden Betaler:��6V   $ Ufranko��6n   $
��10,0q�6�      HeaderIsCheckButton .  6�   �� *ChangeHeaderInfoState+$ 
_UserPanel��5A      � 5B      $��1��6   $y�	Collapsed=�6(   $��0,20,0,0q�6?   $
��Left=�6Q    5C      �� 5D      $
��auto �6,   �� 5E      $
��auto �6,   �� 5F      $
��auto �6,   �� 5G      $
��auto �6,   �� 5H      $
��auto �6,   .+�5K      �� $��0��6    $��	Generelt:��6-   $	��Top=�6A   $
��Left=�6Y   $��20(�6t   $
��Bold"�6�   � 5M      $��1��6   $��0,20,0,0q�6,   	 5N      �� 5O   "   $
��auto �63   �� 5P   "   $
��auto �63   �� 5Q   "   $
��auto �63   .+�5T      � $��0��6#    5U   "   �� 5V   &   $
��auto �64   �� 5W   &   $
��auto �64   .+�5Z   "   )  #TecCargo_Faktura.Controls.MyTextbox  -   5          Name$" MyTextbox_Generelt_Reference��5Z   N      RequireMinimumLength$ 1��65      Text$  ��6t      Placeholder$ 	Reference��6|   $��0��6�   $��	30,5,10,0q�6�   !   ShowAlwaysPlaceholderOnTop 6�   �� *ChangeHeaderInfoState+$ 
_UserPanel��6�     5[   "   -   5        $" MyTextbox_Generelt_Fragtmand��5[   5   $ 	Fragtmand��6[   $��1��6�   $��	30,5,10,0q�6�    6s   �� *ChangeHeaderInfoState+$ 
_UserPanel��6�   5]      � 5^      $��1��6#    5_   "   �� 5`   &   $
��auto �64   �� 5a   &   $
��auto �64   	 5c   "   �� 5d   &   $
��auto �67   �� 5e   &   $
��auto �67   .+�5h   "   .  (TecCargo_Faktura.Controls.DateTimePicker  -	   5          Name$$ DateTimePicker_Generelt_Rute_1��5h   :   $��0��6b      Placeholder$ Dato Rute 1��6o   $��0��6�   $
��Left=�6�   $	��210��6�   $��0,5,5,0q�6�   !   ShowAlwaysPlaceholderOnTop 6�   �� *ChangeHeaderInfoState+$ 
_UserPanel��  5i   "   -
   5        $ MyTextbox_Generelt_Rute_1��5i   5   $��0��6X   $��1��6e      IsInt . $ Rute 1��6�   $  ��6�      	MaxLength$ 4��6�   $��90��6  $��Right=�6  $��0,5,5,0q�6)   6�   �� *ChangeHeaderInfoState+$ 
_UserPanel��6)    5k   "   -   5        $$ DateTimePicker_Generelt_Rute_2��5k   :   $��1��6b   $ Dato Rute 2��6o   $��0��6�   $
��Left=�6�   $	��210��6�   $��0,5,5,0q�6�    6�   �� *ChangeHeaderInfoState+$ 
_UserPanel��  5l   "   -   5        $ MyTextbox_Generelt_Rute_2��5l   5   $��1��6X   $��1��6e    . $ Rute 2��6�   $  ��6�   $ 4��6�   $��90��6  $��Right=�6  $��0,5,5,0q�6*   6�   �� *ChangeHeaderInfoState+$ 
_UserPanel��6*  5n      (  "TecCargo_Faktura.Controls.Selector  5p      -   5          Name$' !Selector_Generelt_Forsikringstype��5p   0   $��2��6[      MaxRow$ 2��6k      Header$ Forsikringstype��6v      Items 5r   &   ��A��67   ��5s   &   C��67   ��5t   &   B��67   ��5u   &   D��67   ��5v   &   	Ingen��6;   5x      5y      � 5{      $��2��6   $��0,20,0,0q�6,   	 5|      �� 5}   "   $
��auto �63   �� 5~   "   $
��auto �63   �� 5   "   $
��auto �63   �� 5�   "   $
��auto �63   .+�5�      .  (TecCargo_Faktura.Controls.CheckBoxButton  -   5          Name$  CheckBoxButton_Byttepaller��5�   6   $��0��6Z       Text$  Brug byttepaller��6j   $��0,5,35,0q�6�     5�      -   5        $ MyTextbox_Byttepaller_1��5�   1   $��1��6R   $ 
1/1 Paller��6c    . $	��130��6�   $��5q�6�    6�   �� *ChangeHeaderInfoState+$ 
_UserPanel��  5�      -   5        $ MyTextbox_Byttepaller_2��5�   1   $��2��6R   $ 
1/2 Paller��6c    . $	��130��6�   $��5q�6�    6�   �� *ChangeHeaderInfoState+$ 
_UserPanel��  5�      -   5        $ MyTextbox_Byttepaller_3��5�   1   $��3��6R   $ 
1/4 Paller��6c    . $	��130��6�   $��5q�6�    6�   �� *ChangeHeaderInfoState+$ 
_UserPanel��5�      � 5�      $��3��6   $��0,20,0,0q�6,    5�      �� 5�   "   $
��auto �60   �� 5�   "   $
��auto �60   �� 5�   "   $
��auto �60   	 5�      �� 5�   "   $
��auto �63   �� 5�   "   $
��auto �63   .+�5�        -   5        $ CheckBoxButton_Efterkrav��5�   6   $  	Efterkrav��6X   !   Checked! .  $��0��6y   $��0��6�   $��2��6�   $
��Left=�6�     5�      -   5        $ MyTextbox_Efterkrav_1��5�   1   $ Efterkrav inkl. gebyr��6�   "   IsDouble" . $��1��6  $��0��6  $
��Left=�6  $��40,5,5,0q�6:  y�6P   �� *	Checked+$ CheckBoxButton_Efterkrav��# �� 	Converter## [  6L  �� *ChangeHeaderInfoState+$ 
_UserPanel��  5�      -   5        $ MyTextbox_Efterkrav_2��5�   1   $ Forsikringssum kr.��6�   " . $��2��6�   $��0��6  $
��Left=�6  $��40,5,5,0q�67  y�6P   �� *	Checked+$ CheckBoxButton_Efterkrav��## [  6I  �� *ChangeHeaderInfoState+$ 
_UserPanel��  5�      -   5        $ MyTextbox_Efterkrav_3��5�   1   $ Præmie kr.��6�   " . $��1��6�   $��1��6  $
��Left=�6  $��0,5,5,0q�6/  y�6P   �� *	Checked+$ CheckBoxButton_Efterkrav��## [  6@  �� *ChangeHeaderInfoState+$ 
_UserPanel��  5�      -   5        $ MyTextbox_Efterkrav_4��5�   1   $ I alt. kr. inkl. moms��6�   " . $��2��6  $��1��6  $
��Left=�6  $��0,5,5,0q�69  y�6P   �� *	Checked+$ CheckBoxButton_Efterkrav��## [  6J  �� *ChangeHeaderInfoState+$ 
_UserPanel��5�      5�      � 5�      $��2��6   $y�	Collapsed=�6(   $��0,20,0,0q�6?   $
��Left=�6Q    5�      �� 5�      $
��auto �6,   �� 5�      $
��auto �6,   �� 5�      $
��auto �6,   	 5�      �� 5�      $	��320 �6/   �� 5�      $	��320 �6/   �� 5�      $	��320 �6/   .+�5�      �� $��0��6    $��0��6-   $��3��6=   $��
Transport:��6Q   $
��auto��6f   $
��auto��6s   $
��Bold"�6�   $��20(�6�   $	��Top=�6�   $
��Left=�6�   $��0,0,0,20q�6�     5�      -   5        $ Selector_Transport1��5�   ,   (	  "TecCargo_Faktura.Controls.Selector$ 	  SelectionChanged$)$ #Selector_Transport_SelectionChanged��6�   $ Trin: 1��6I   $��1��6Z   $��0��6g   $ 3��6w   $
��Left=�6�    5�   "   ��Kurertransport��6@   ��5�   "   Pakketransport��6@   ��5�   "   Godstransport��6?   5�        5�      -   5        $ Selector_Transport2��5�   ,   $)$ #Selector_Transport_SelectionChanged��6�   $ Trin: 2��6I   $��1��6Z   $��1��6g   $ 3��6w   $y�	Collapsed=�6�   $
��Left=�6�     5�      -   5        $ Selector_Transport3��5�   ,   $)$ #Selector_Transport_SelectionChanged��6�   $ Trin: 3��6I   $��1��6Z   $��2��6g   $ 2��6w   $y�	Collapsed=�6�   $
��Left=�6�   ,
  &TecCargo_Faktura.Controls.PakkeControl
  5�      -   5        % 
 Name$% PakkeControl_Transport��5�   0   $��2��6P   $��0��6]   $��3��6m   $y�	Collapsed=�6�   $��0,20,0,0q�6�   !& 
  ShowAlwaysPlaceholderOnTop& 6�   �� *ChangeHeaderInfoState+$ 
_UserPanel��5�      � 5�      $��3��6   $��0,20,0,0q�6(   $
��Left=�6:   $y�	Collapsed=�6U   .+�5�      �� ' �� Orientation$' Vertical=�6%   .+�5�      �� $��Ekstra:��6$   $
��auto��66   $
��auto��6C   $
��Bold"�6Q   $��20(�6c   $	��Top=�6q   $
��Left=�6�   $��0,0,0,0q�6�   +  %TecCargo_Faktura.Controls.MyTextBlock  5�      -   5        (  Name$( MyTextBlock_Notifications��5�   3   $��40,0,0,0q�6�   )   Placeholder$) Evt. Bemærkninger��6�   $	��600��6�   $	��200��6�   $
��Left=�6�   !*   ShowAlwaysPlaceholderOnTop* 6W   �� *ChangeHeaderInfoState+$ 
_UserPanel��6�   5�      5�      � 5�      -   5        $��Grid_CheckStatusContainer��5�   (   $��4��6   $y�Visible=�6I   $
��Left=�6^   $��0,20,0,0q�6y   	 5�      �� 5�      $
��auto �6/   �� 5�      $
��auto �6/   5�      � 5�      -   5        $��5��5�      $y�	Collapsed=�6(   .+�5�      0  *TecCargo_Faktura.Controls.MyDocumentViewer  -   5        +  Name$ + MyDocumentViewer_PDFViewer��5�   4   5�      � 5�      $��6��6   $y�	Collapsed=�6(   $
��Left=�6?   $��0,20,0,0q�6Z    5�      �� 5�      $
��auto �6,   �� 5�      $
��auto �6,   �� 5�      $
��auto �6,   �� 5�      $
��auto �6,   	 5�      �� 5�      $
��auto �6/   �� 5�      $	��150 �6/   �� 5�      $
��auto �6/   .+�5�      �� $��0��6    $��0��6-   $��3��6=   $��Afslut Fragtbrev:��6Q   $
��auto��6m   $
��auto��6z   $
��Bold"�6�   $��20(�6�   $	��Top=�6�   $
��Left=�6�   � 5�      $��1��6   $��0��6,    5�      �� 5�   "   $
��auto �60   �� 5�   "   $
��auto �60   �� 5�   "   $
��auto �60   	 5�      �� 5�   "   $
��auto �63   �� 5�   "   $
��auto �63   .+�5�      �� $��General��6$   $��0��66   $��0��6C   $��2��6S   $
��Bold"�6g   $��15(�6y     5�      -   5        $) #DateTimePicker_Afslut_Leveringsdato��5�   6   $��1��6d   $��0��6q   $��2��6�   $ Leveringsdato��6�   $	��250��6�   $
��Left=�6�   ,   Format$, dd/MM/yyyy H:mm��6�   -   TimeVisibility$- Visible=�6�    6
  �� *ChangeHeaderInfoState+$ 
_UserPanel��  5�      -    5        $ MyTextbox_Afslut_Rabat��5�   1   $ 10��6Q   $��2��6[   $��0��6h   $ Rabat��6x   $	��125��6�   $
��Left=�6�   $��0,5,5,0q�6�    6�   �� *ChangeHeaderInfoState+$ 
_UserPanel��  5�      -!   5        $  MyTextbox_Afslut_Kilometer��5�   1   $ 10��6U   $��2��6_   $��1��6m   $ 	Kilometer��6}   $	��125��6�   $��Right=�6�   $��0,5,5,0q�6�    6�   �� *ChangeHeaderInfoState+$ 
_UserPanel��5�      � 5�      $��1��6   $��2��6,    5�      �� 5�   "   $
��auto �60   �� 5�   "   $
��auto �60   �� 5�   "   $
��auto �60   	 5�      �� 5�   "   $
��auto �63   �� 5�   "   $
��auto �63   .+�5�      �� $��Tidsforbrug��6$   $��0��6:   $��0��6G   $��2��6W   $
��Bold"�6k   $��15(�6}     5�      -"   5        $$ MyTextbox_Afslut_Tidsforbrug_1��5�   1   $ 10��6Y   $ 
Læsse tid��6c   $��1��6{   $��0��6�   $	��200��6�    . $��0,5,5,0q�6�    6�   �� *ChangeHeaderInfoState+$ 
_UserPanel��  5�      -#   5        $$ MyTextbox_Afslut_Tidsforbrug_2��5�   1   $ 10��6Y   $ Aflæssning tid��6c   $��1��6�   $��1��6�   $	��200��6�    . $��0,5,5,0q�6�    6�   �� *ChangeHeaderInfoState+$ 
_UserPanel��  5�      -$   5        $$ MyTextbox_Afslut_Tidsforbrug_3��5�   1   $ 10��6Y   $ 	Vente tid��6c   $��2��6{   $��0��6�   $	��200��6�    . $��0,5,5,0q�6�    6�   �� *ChangeHeaderInfoState+$ 
_UserPanel��  5�      -%   5        $$ MyTextbox_Afslut_Tidsforbrug_4��5�   1   $ 10��6Y   $ Medhjælper tid��6c   $��3��6�   $��1��6�   $	��200��6�    . $��0,5,5,0q�6�    6�   �� *ChangeHeaderInfoState+$ 
_UserPanel��5�        5�      -&   5        $"( MyTextBlock_Afslut_Kommentar��5�   /   $) Kommentar til fragtbrev��6U   $��0,20,0,0q�6{   $��2��6�   $��0��6�   $��3��6�   $	��500��6�   $	��200��6�   $
��Left=�6�   * 6�   �� *ChangeHeaderInfoState+$ 
_UserPanel��5�      � 5     $��7��6   $y�	Collapsed=�6(   $
��Left=�6?   $��0,20,0,0q�6Z    5     �� 5     $
��auto �6,   �� 5     $
��auto �6,   �� 5     $
��auto �6,   �� 5     $
��auto �6,   	 5     �� 5	     $
��auto �6/   �� 5
     $	��150 �6/   �� 5     $
��auto �6/   .+�5       -'   5        $
 Address_Faktura��5  +   $��0��6D   $��0��6Q   $��2��6a   .   IsUnLock. .  $
��Left=�6�   $ Faktura Adresse��6�   /   CheckboxVisibility$/ 	Collapsed=�6�    6�   �� *ChangeHeaderInfoState+$ 
_UserPanel��� 5     $��0��6   $��2��6,    5     �� 5  "   $
��auto �60   �� 5  "   $
��auto �60   �� 5  "   $
��auto �60   	 5     �� 5  "   $
��auto �63   �� 5  "   $
��auto �63   .+�5     �� $��General��6$   $��0��66   $��0��6C   $��2��6S   $
��Bold"�6g   $��15(�6y     5     $��1��67   $��0��6D   $��2��6T   $ Leveringsdato��6h   $	��250��6�   $
��Left=�6�   $, dd/MM/yyyy H:mm��6�   $- Visible=�6�    6�   �� *ChangeHeaderInfoState+$ 
_UserPanel��  5     $��2��61   $��0��6>   $ Rabat��6N   $	��125��6b   $
��Left=�6n   $��0,5,5,0q�6�    6�   �� *ChangeHeaderInfoState+$ 
_UserPanel��  5     $��2��61   $��1��6?   $ 	Kilometer��6O   $	��125��6g   $��Right=�6s   $��0,5,5,0q�6�    6�   �� *ChangeHeaderInfoState+$ 
_UserPanel��5     � 5      $��1��6   $��2��6,    5!     �� 5"  "   $
��auto �60   �� 5#  "   $
��auto �60   �� 5$  "   $
��auto �60   	 5&     �� 5'  "   $
��auto �63   �� 5(  "   $
��auto �63   .+�5+     �� $��Tidsforbrug��6$   $��0��6:   $��0��6G   $��2��6W   $
��Bold"�6k   $��15(�6}     5-     $ 10��61   $ 
Læsse tid��6;   $��1��6S   $��0��6`   $	��200��6p    . $��0,5,5,0q�6�    6�   �� *ChangeHeaderInfoState+$ 
_UserPanel��  5.     $ 10��61   $ Aflæssning tid��6;   $��1��6X   $��1��6e   $	��200��6u    . $��0,5,5,0q�6�    6�   �� *ChangeHeaderInfoState+$ 
_UserPanel��  50     $ 10��61   $ 	Vente tid��6;   $��2��6S   $��0��6`   $	��200��6p    . $��0,5,5,0q�6�    6�   �� *ChangeHeaderInfoState+$ 
_UserPanel��  51     $ 10��61   $ Medhjælper tid��6;   $��3��6X   $��1��6e   $	��200��6u    . $��0,5,5,0q�6�    6�   �� *ChangeHeaderInfoState+$ 
_UserPanel��52     53     � 54     $��8��6   $y�	Collapsed=�6(   .+�55     %  TecCargo_Faktura.Controls.Gebyr  -(   5        0  Name$0 Gebyr_Ekstra��55  )   $��0,0,0,0q�6?   !1   ShowAlwaysPlaceholderOnTop1 6P   �� *ChangeHeaderInfoState+$ 
_UserPanel��56     � 58     $��9��6   $��Center=�6(   $��Bottom=�6E   	 59     �� 5:     $
��auto �6/   �� 5;     $
��auto �6/   .+�5>     �� -)   5        $��Button_GridViewBack��5>  !   $��0��6<   $	��200��6L   $��60��6X   $��
0,20,20,10q�6d   $��Center=�6x   .��5?     �� $' 
Horizontal=�6)   .+�5@  "   �� $��50��6(   $��50��63   $?��9/TecCargo Faktura System;component/Images/Icons/arrow.png��6?   2 J� RenderTransformOrigin$2 0.5,0.5&�6�   z�5A  &   V� 5B  *   .~�5C  .   �� �� 5D  .   �� 5E  .   3 �� Angle$	3 180Q�6>   T� 5F  .   5G  +   5I  #   �� 5J  "   $��Gå Tilbage��6(   $
��10,0q�6=   $
��Bold"�6L   $��20(�6^   $��Center=�6l   5K     5L     �� 5M     -*   5        $��Button_GridViewNext��5M  !   $��1��6<   $	��200��6L   $��60��6X   $��
20,20,0,10q�6d   $��Center=�6x   .��5N     �� $' 
Horizontal=�6)   .+�5O  "   �� $��
Gå Videre��6(   $
��10,0q�6<   $
��Bold"�6K   $��20(�6]   $��Center=�6k   �� 5P  "   $��50��6(   $��50��63   $?��9/TecCargo Faktura System;component/Images/Icons/arrow.png��6?   $��Right=�6�   5Q     5R     5S     5T     � 5V     $��1��6   $	��Top=�6#   $
��Left=�6;   ��.  .+�5W     (  "TecCargo_Faktura.Controls.PrisList  -+   5        4  Name$4 Prislist_faktura��5W  (   $��Right=�5X     $	��Top=�5Y     y�5Z     �� *ShowPricelist+$ 
_UserPanel��## [  5\     5]     5_     � 5a  
   $��2��6   $��0��6   $��3��6,   $��Stretch=�6@   $��50��6^   $
��Auto��6j   ���	#FF2C72B8	 5b     �� 5c     $��* �6#   �� 5d     $
��auto �6#   .+�5g     � $��0��6    5h     �� 5i     $
��auto �6$   �� 5j     $
��auto �6$   .+�5l     �� $��0��6   $' 
Horizontal=�6*   .+�5m     �� $��Version:��6   $
��Left=�6/   $	��Top=�6J   �� 5n     +  %TecCargo_Faktura.Models.ImportantData5   PVersion#��Z5 6   $
��Left=�6O   $	��Top=�6j   5o     �� 5p     $��1��6   $' 
Horizontal=�6*   .+�5q     �� $��	Kodet af:��6   $
��Left=�60   $	��Top=�6K   �� 5r     6   PAuthor#��Z6 6   $
��Left=�6N   $	��Top=�6i   5s     5t     � 5v     $��1��6   	 5w     �� 5x     $
��auto �6'   �� 5y     $
��auto �6'   .+�5|     �� $' 
Horizontal=�6   $��0��66   .+�5}     �� $��
Fragtbrev:��6    $��Center=�62   $
��Bold"�6M   $��15(�6_   �� 5~     -,   5        $��Image_ButtomStatusFragt��5~     $��30��6;   $��30��6G   5     �� 5�     $' 
Horizontal=�6   $��1��66   $��	35,0,20,0q�6F   .+�5�     �� $��Faktura:��6    $��Center=�60   $
��Bold"�6K   $��15(�6]   �� 5�     --   5        $��Image_ButtomStatusFaktura��5�     $��30��6=   $��30��6I   5�     5�     5�     � 5�  
   $��0��6   $��Stretch=�6   $��0��6:   $
��Auto��6J   ���	#FF2C72B8	 5�     �� 5�     $
��auto �6#   �� 5�     $
��auto �6#   �� 5�     $��* �6#   �� 5�     $
��auto �6#   .+�5�     � $��0,2,0,0q�6   $��0��6$   $
��Left=�64   	 5�     �� 5�     $
��auto �6'   �� 5�     $��30 �6'   �� 5�     $
��auto �6'    5�     �� 5�     $
��auto �6$   �� 5�     $
��auto �6$   �� 5�     $
��auto �6$   .+�5�     � $��0��6   $��0��6$   	 5�     �� 5�     $
��auto �6+   �� 5�     $
��auto �6+   .+�5�     �� -.   5        $��0��5�     $��Filer��6-   $	��5,2q�6=   $��2,0,0,2q�6n   �� 5�     -/   5        $��1��5�     $��
Start Menu��6-   $	��5,2q�6B   $��2,0,0,2q�6t   5�     � 5�     $��1��6   $��0��6$   	 5�     �� 5�     $
��auto �6+   �� 5�     $
��auto �6+   �� 5�     $
��auto �6+   .+�5�     �� -0   5        $��Button_Save��5�     $��0��60   $��35��6@   7 � ToolTip$	7 Gem��6L   $��2,0,0,2q�6{   .��5�     �� $F��@/TecCargo Faktura System;component/Images/Icons/save_checkv2.png��6    5�     �� 5�     -1   5        $��1��5�     $��35��6-   $7 Åben mappe��69   $��2,0,0,2q�6n   .��5�     �� $@��:/TecCargo Faktura System;component/Images/Icons/folder.png��6    5�     �� 5�     -2   5        $��2��5�     $��35��6-   $	7 Gem��69   $��2,0,0,2q�6g   .��5�     �� $>��8/TecCargo Faktura System;component/Images/Icons/lock.png��6    5�     5�     � 5�     $��2��6   $��0��6$   	 5�     �� 5�     $
��auto �6+   �� 5�     $
��auto �6+   �� 5�     $
��auto �6+   .+�5�     d� -3   5        $��0��5�  #   $��35��63   $7 Brug fuld side��6?   $��2,0,0,2q�6s   .��5�     �� $J��D/TecCargo Faktura System;component/Images/Icons/toolbar_FuldSide.png��6    5�     d� 5�     -4   5        $��1��5�  #   $��35��63   $7 Vis placeholder i toppen��6?   $��2,0,0,2q�6}   .��5�     �� $M��G/TecCargo Faktura System;component/Images/Icons/toolbar_Placeholder.png��6    5�     d� 5�     -5   5        $��2��5�  #   $��35��63   $7 Vis prisliste��6?   $��2,0,0,2q�6r   .��5�     �� $K��E/TecCargo Faktura System;component/Images/Icons/toolbar_PriceList.png��6    5�     5�     � 5�     $��0��6   $��2��6$   $��0,5,0,0q�64   	 5�     �� 5�     $
��auto �6+   �� 5�     $
��auto �6+   �� 5�     $
��auto �6+   .+�5�     �� -6   5        8 ��Name$8 Slider_Zoom��5�     $��0��62   9 � Value$	9 100Q�6B   : � Minimum$: 30Q�6N   ; � Maximum$	; 200Q�6[   $	��100��6i   $
��Left=�6u   $��2,0,0,0q�6�   < �� IsSnapToTickEnabled< .   Horizontal_Slider#���  6�   �� 5�     $��1��6    $��5,0,0,0q�6`   ��60   �� *Value+$ Slider_Zoom��6`   �� 5�     $��2��6    $��%��60   5�     � 5�     $��1��6   $��2��6$   	 5�     �� 5�     $
��auto �6+   �� 5�     $
��auto �6+   �� 5�     $
��auto �6+   .+�5�     d� -7   5        $��0��5�  #   $��35��63   $7 Brug fuld side��6?   $��2,0,0,2q�6�   .��5�     �� $J��D/TecCargo Faktura System;component/Images/Icons/toolbar_FuldSide.png��6    5�     d� 5�     -8   5        $��1��5�  #   $��35��63   $7 Vis placeholder i toppen��6?   $��2,0,0,2q�6�   .��5�     �� $M��G/TecCargo Faktura System;component/Images/Icons/toolbar_Placeholder.png��6    5�     d� 5�     -9   5        $��2��5�  #   $��35��63   $7 Vis prisliste��6?   $��2,0,0,2q�6�   .��5�     �� $K��E/TecCargo Faktura System;component/Images/Icons/toolbar_PriceList.png��6    5�     5�     5�     � 5�     $��2��6   $��Center=�6#   $��Center=�6@   $��0,10,0,0q�6[    5�     �� 5�     $
��auto �6$   �� 5�     $
��auto �6$   .+�5�     � $��0��6   $��Center=�6$   $��Center=�6A   $��0,0,0,-2q�6\   	 5�     �� 5�     $
��auto �6+   �� 5�     $
��auto �6+   �� 5�     $
��auto �6+   �� 5�     $��69 �6+   �� 5�     $
��auto �6+   �� 5�     $
��auto �6+   �� 5�     $
��auto �6+   .+�5     �� -:   5        $#��Line_ViewSelector_FragtFirstH��5     $��0��6@   = �� Stretch$
= Fill=�6P   	> �� X1$> 1��6�   $��10��6�   $	��Top=�6�   $
��Left=�6�   $��	0,-15,0,0q�6�   $��2��6�   ��6_   �� *
Background+$ Border_ViewSelectorBox��6�   �� 5     $��0��6   $��	0,-15,0,0q�6-   $
��Left=�6@   $	��Top=�6[   .+�5     �� $��2��6%   $��15��6/   ��6<   �� *
Background+$ Border_ViewSelectorBox��5     �� 5     $��1��6   $��	0,-24,0,0q�6-   $��20��6�   ? �� CornerRadius$? 5��6�   $	��Top=�6�   $
��Left=�6�   ��6@   �� *
Background+$ Border_ViewSelectorBox��._�5     �� -;   5        $"��TextBlock_ViewSelector_Fragt��5  $   $��	Fragtbrev��6H   
�������@ �� Padding$	@ 5,0q�6m   $	��Top=�6{   $
��Left=�6�   5     �� 5	     -<   5        $"��Line_ViewSelector_FragtLastH��5	     $��2��6?   $
= Fill=�6O   $> 1��6�   $��15��6�   $	��Top=�6�   $
��Left=�6�   $��	0,-15,0,0q�6�   $��2��6�   ��6^   �� *
Background+$ Border_ViewSelectorBox��6�   �� 5
     $��2��6   $��	0,-15,3,0q�6-   $��Right=�6@   $	��Top=�6\   .+�5     �� $��2��6%   $��15��6/   ��6<   �� *
Background+$ Border_ViewSelectorBox��5     �� 5     -=   5        $%��Line_ViewSelector_FakturaFirstH��5     $��4��6B   $
= Fill=�6R   $> 1��6�   $��10��6�   $	��Top=�6�   $
��Left=�6�   $��	0,-15,0,0q�6�   $��2��6�   ��6a   �� *
Background+$ Border_ViewSelectorBox��6�   �� 5     $��4��6   $��	0,-15,0,0q�6-   $
��Left=�6@   $	��Top=�6[   .+�5     �� $��2��6%   $��15��6/   ��6<   �� *
Background+$ Border_ViewSelectorBox��5     �� 5     $��5��6   $��	0,-24,0,0q�6-   $��20��6�   $? 5��6�   $	��Top=�6�   $
��Left=�6�   ��6@   �� *
Background+$ Border_ViewSelectorBox��._�5     �� ->   5        $$��TextBlock_ViewSelector_Faktura��5  $   $��Faktura��6J   
�������$	@ 5,0q�6m   $	��Top=�6{   $
��Left=�6�   5     �� 5     -?   5        $$��Line_ViewSelector_FakturaLastH��5     $��6��6A   $
= Fill=�6Q   $> 1��6�   $��15��6�   $	��Top=�6�   $
��Left=�6�   $��	0,-15,0,0q�6�   $��2��6�   ��6`   �� *
Background+$ Border_ViewSelectorBox��6�   �� 5     $��6��6   $��	0,-15,3,0q�6-   $��Right=�6@   $	��Top=�6\   .+�5     �� $��2��6%   $��15��6/   ��6<   �� *
Background+$ Border_ViewSelectorBox��5     5     �� 5     -@   5        A ��Name$A Border_ViewSelectorBox��5     $��1��69   $��50��6F   $��2q�6R   ���	#FF0959A8���	#FF0959A8$? 2��6�   ._�5     � 	 5      �� 5!     $
��auto �6/   �� 5"     $
��auto �6/   �� 5#     $
��auto �6/   .+�5'     �� -A   5        $��Button_ViewSelectorBack��5'  !   $��0��5(     $��0q�5)  !   $��46��5*  !   $��46��5+  !   #��[" �5,  !   .��5.     �� $?��9/TecCargo Faktura System;component/Images/Icons/arrow.png��6$   $2 0.5,0.5&�6g   z�5/  "   V� 50  &   .~�51  *   �� �� 52  *   �� 53  *   $	3 180Q�6:   T� 54  *   55  '   57     58     �� 5:     -B   5        $A Border_ViewSelector��5:  !   $��1��6?   $��46��6O   $��2q�6[   ���	#FF0959A8���	#FF959595$? 2��6�   ._�5;     � -C   5        $ ��Grid_ViewSelectorContainer��5;  #   5=     5>     �� 5@     -D   5        $��Button_ViewSelectorNext��5@  !   $��2��5A  %   $��0q�5B  %   $��46��5C  %   $��46��5D  %   #��[" �5E  %   .��5H     �� $?��9/TecCargo Faktura System;component/Images/Icons/arrow.png��6$   5I     5L     5M     5N     �� 5O     $��3��6   $��5q�6$   $<��6/TecCargo Faktura System;component/Images/Pdf/logo.png��6/   �� 5U     -E   5        B ��Name$B ToolsbarMenu_Filer��5U     $y�Hidden=�62   $
��Left=�6F   $
��auto��6a   $
��auto��6o   $	��100��6|   $��10,23,0,-85q�6�   $	��Top=�6�   ���	#FFDDDDDD$��1q�6�   ���	#FF707070$��Disabled=�6�   $��Disabled=�62  .@�5W     �� $��Nyt Faktura��6   �� 5X     $��Nyt Fragtbrev��6   �� 5Z     $��Åben Faktura��6   �� 5[     $��Åben Fragtbrev��6   �� 5]     -F   5        C ��Name$C toolbarButtonListSave��5]     $��Gem Fragtbrev��6=   �� 5^     -G   5        $C toolbarList_PDF��5^     $��Exporter Til PDF��67   5b     �� 5c     -H   5        D ��Name$D ButtonTool_CloseFragtbrev��5c  0   $��Send til faktura��6   $y�Hidden=�6S   {�.  $	��100��6y   $
��Left=�6�   $��	127,4,0,0q�6�   $	��Top=�6�   |�.  6�   �� 5d     -I   5        $&D  ButtonTool_CloseFragtbrevComment��5d  O   $
��Left=�6   $��20��60   $��	227,4,0,0q�6<   $y�Hidden=�6y   {�.  $	��Top=�6�   $��2000000��6�   .��5e     �� $
��auto��6   $	��Top=�6&   $
��auto��6>   $
��Left=�6K   $7 Exporter til pdf��6g   5f     5g     5h     5i     