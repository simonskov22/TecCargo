   M S B A M L   `   `   `  ����O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  5      R NTecCargo Faktura System, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:TecCargo_Faktura.ControlsTecCargo_Faktura.Controls 5      E'clr-namespace:TecCargo_Faktura.ControlsTecCargo_Faktura.Controls 5      A%clr-namespace:TecCargo_Faktura.ModelsTecCargo_Faktura.Models 5	      ,   &TecCargo_Faktura.Controls.PakkeControl   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      9System&clr-namespace:System;assembly=mscorlib     5      9local'clr-namespace:TecCargo_Faktura.Controls     5      <TecCargo'clr-namespace:TecCargo_Faktura.Controls     5      8Models%clr-namespace:TecCargo_Faktura.Models     5	      -   5        $��_PakkeControl��5   7     � 	Resources  5      7  1TecCargo_Faktura.Models.BoolToVisibilityConverter   BoolToVisibility_HideText�  �TecCargo_Faktura.Models.BoolToValueConverter`1[[System.Windows.Visibility, PresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35]]   	TrueValue   
FalseValue  PrimaryBrush �� Color  PrimaryFont 	 Row_1 	 Row_2  Row_1_Disable  Row_2_Disable  Button_PakkeInGrid �� 
TargetType �� Property �� Value  Grid_Header_Text 	 Grid_Header_Border��%�  &	        &	 3     &	 V     &	 u     &	 �     &	 �     &	 �     &	     &	 K    7  &		 �    7    5   
   $ Hidden=�6M   $ Visible=�6`   �� 5   
   $ 	#FF4169AA��6/   �� 5   
   $ White��6.   �� 5   
   $ 	#FFFFFFFF��6(   �� 5   
   $ 	#FFDEDEDE��6(   �� 5   
   $ 	#FFE6E6E6��60   �� 5   
   $ 	#FFD1D1D1��60   �� 5   
   " ��6+   .�5      ��  � ��
 �����6+   5      �� 5   
   " ��6)   .�5      ��  � ��8   6+   �� 5       � ��$ 15(�6)   �� 5       � ��$
 Bold"�6+   �� 5       � ��$
 Left=�6;   5       �� 5!   
   " ��6+   .�5"      ��  � ��8   6+   �� 5#       �@��Padding$ 10q�6(   5$      5%      .��5&      �  � ColumnDefinitions 5(   
   �� 5)      $
��auto �6   �� 5*      $
��auto �6   �� 5+      $
��auto �6   .+�5.   
   � -   5         �Name$ Grid_editItem1��5.      $��0��6   	 � RowDefinitions	 5/      �� 50      $
��auto �6    �� 51      $
��auto �6    �� 52      $
��auto �6    �� 53      $
��auto �6    �� 54      $
��auto �6    �� 55      $
��auto �6    �� 56      $
��auto �6    .+�59      �� -   5        $��	_hiddenId��59      $��-1��6)   $y�	Collapsed=�63   )  #TecCargo_Faktura.Controls.MyTextbox  5;      -   5        
  Name$
 _mrkNumb��5;   .   )  #TecCargo_Faktura.Controls.MyTextbox   	LostFocus$ Textbox_LostFocus��6W   $��0��6!      Placeholder$ Mrk./NR.��6@   $��	10,5,10,0q�6u   !   ShowAlwaysPlaceholderOnTop 6�   �� *ShowAlwaysPlaceholderOnTop+ �� ElementName$ _PakkeControl��  5<      -   5        $
 _indhold��5<   .   $ Textbox_LostFocus��6V   $��1��6!   $ Indhold��6@   $��	10,5,10,0q�6t    6�   �� *ShowAlwaysPlaceholderOnTop+$ _PakkeControl��  5=      -   5        $
 _antal��5=   .   $ Textbox_LostFocus��6y   $��2��6!   $ Antal��6>   $	��130��6R   $
��Left=�6^   $��	10,5,10,0q�6�    6�   �� *ShowAlwaysPlaceholderOnTop+$ _PakkeControl��  5>      -   5        $
 _weight��5>   .   $ Textbox_LostFocus��6z   $��2��6!   $ Vægt��6?   $	��130��6R   $��Right=�6^   $��	10,5,10,0q�6�    6�   �� *ShowAlwaysPlaceholderOnTop+$ _PakkeControl��  5?      -   5        $
 _volume��5?   .   $ Textbox_LostFocus��6^      TextChanged$ _volume_TextChanged��6�   $��3��6!   $ Rumfang (LxBxH)��6@   $��	10,5,10,0q�6|    6�   �� *ShowAlwaysPlaceholderOnTop+$ _PakkeControl��6�   (  "TecCargo_Faktura.Controls.Selector  5@      -	   5          Name$ _artType��5@   -   (  "TecCargo_Faktura.Controls.Selector   	LostFocus$ Textbox_LostFocus��6W   $��4��6       Header$	 Art��6?      MaxRow$ 2��6L   $	��100��6u      Items 5B      ��	Ingen��6+   ��5C      TDR��6)   ��5D      PLL��6)   ��5E      CLL��6)   ��5F      PKK��6)   ��5G      IBC��6)   5I        5K      -
   5        $ _transportType��5K   -   $ Textbox_LostFocus��6h   $��5��6    $ Transport Type��6E   $ 3��6]   $y�	Collapsed=�6�   $	��150��6�   �� 5N      -   5        $��
_AddButton��5P   3   $��6��5N      
������ 
������ $
��auto��5O      $	��Top=�6   $
��Left=�67   $
��Left=�5P      $��	10,10,0,0q�6E   .��5R      ��  �� Orientation$ 
Horizontal=�6   .+�5S      �� $��35��6   $>��8/TecCargo Faktura System;component/Images/Icons/plus.png��5T      �� 5U      -   5        $��_AddButtonText��5U      $��Tilføj��62   $��20(�6C   5V      5W      �� 5Y      -   5        $��_CancelButton��5[   3   $��6��5Y      
������ 
������ $
��auto��5Z      $	��Top=�6   $��Right=�67   $
��Left=�5[      $��	0,10,10,0q�6H   $y�Hidden=�5\   1   .��5]      �� $ 
Horizontal=�6   .+�5^      �� $��35��6   $@��:/TecCargo Faktura System;component/Images/Icons/cancel.png��5_      �� 5`      $��Annuller��6   $��20(�6/   5a      5b      5d      �� 5f   
   $��0,5,0,0q�6   $��1��6"   $	��320��62   $	��350��6A   $	��Top=�6Q   
�������$��1q�6|   ���	#FFA09F9F._�5g      � 	 5h      �� 5i      $
��auto �6$   �� 5j      $��* �6$   �� 5k      $
��auto �6$   .+�5m      �� $��0��6   $��0,0,0,1q�6&   ���	#FFA09F9F#��[	 6X   ._�5n      � .+�5o      �� $��Pakker��6    $��20(�61   $
��Bold"�6?   $
��Left=�6Q   #��[ 6l   �� 5p      -   5        $��
_packCount��5p       $�� ��62   $��20(�6=   $
��Bold"�6K   $��Right=�6]   #��[ 6y   5q      5r      �� 5t      -   5        $��_ScrollViewer_Packet��5t   �   $��1��6   $	��300��6,   $��Disabled=�6<   $
��Auto=�6e   .��5u      � -   5        $��_ItemsContains��5u       J� SnapsToDevicePixels .  5v      �� 5w      $
��auto �6/   �� 5x      $
��auto �6/   �� 5y      $
��auto �6/   5|      5}      � 5      $��2��6   ���	#FFC7C7C7 5�      �� 5�      $��* �6+   �� 5�      $��* �6+   .+�5�      �� -   5        $��_Button_Arrow_Back��5�   E   $��0��5�      
������ 
������ $
��auto��5�      $	��Top=�6#   $
��Left=�6;   $��0q�6W   $��5q�5�      #��[" �5�       J� RenderTransformOrigin$ 0.5,0.5&�6P   z�5�      V� 5�      .~�5�   "   �� �� 5�   "   �� 5�   "    �� Angle$	 180Q�62   T� 5�   "   5�      .��5�      �� $��35��6    $?��9/TecCargo Faktura System;component/Images/Icons/arrow.png��6,   5�      �� 5�      -   5        $��_Button_Arrow_Next��5�   E   $��1��5�      
������ 
������ $
��auto��5�      $	��Top=�6#   $��Right=�6;   $��0q�6W   $��5q�5�      #��[" �5�      .��5�      �� $��35��6    $?��9/TecCargo Faktura System;component/Images/Icons/arrow.png��6,   5�      5�      5�      5�      � 5�   
   -   5        $ grid_faktura��5�      $��2��6    5�      �� 5�      $	��130 �6#   �� 5�      $
��auto �6$   	 5�      �� 5�      $
��auto �6    �� 5�      $
��auto �6    �� 5�      $
��auto �6    �� 5�      $
��auto �6    .+�5�        -   5        $ Selector_Faktura_Takst��5�         SelectionChanged$2 ,Selector_CheckFakturaStatus_SelectionChanged��6t   $��0��6=   $��0��6M   $ Takst��6Z   $ 5��6i    5�      ��1��6'   ��5�      2��6'   ��5�      3��6'   ��5�      4��6'   ��5�      5��6'   ��5�      6��6'   ��5�      7��6'   ��5�      8��6'   ��5�      9��6'   ��5�      10��6(   5�        5�      -   5        $ Selector_Faktura_Beregn��5�      $2 ,Selector_CheckFakturaStatus_SelectionChanged��6s   $��1��6>   $��0��6N   $ Beregningstype��6[    5�      ��Ladmeter��6.   ��5�      
Palleplads��60   ��5�      
Volume��6,   5�      �� 5�      -   5        $��StackPanel_PacketPrice��5�      $ 
Horizontal=�67   $��1��6P   $��10,5,0,0q�6]   y�5�      ��  �� Path$ Text.IsEmpty�$ TextBlock_PacketPrice�� �� 	Converter# [  .+�5�      �� $��Pris: ��6   $
��Bold"�6*   $��15(�6<   �� 5�      -   5        $��TextBlock_PacketPrice��5�      $�� ��69   $��15(�6A   5�      �� 5�      -   5        $��_NextButton��5�   3   $��2��5�      $��0��6"   $��2��62   
������ 
������ $
��auto��5�      $	��Top=�6   $
��Left=�67   $
��Left=�5�      $��	10,10,0,0q�6F   $y�	Collapsed=�5�   /   .��5�      �� $ 
Horizontal=�6   .+�5�      �� $��35��6   $>��8/TecCargo Faktura System;component/Images/Icons/plus.png��5�      �� 5�      -   5        $��_NextButtonText��5�      $��20(�63   5�      5�      �� 5�      -   5        $��3��5�      $��0��6#   $��2��63   
������ 
������ $
��auto��5�      $	��Top=�6   $
��Left=�67   $
��Left=�5�      $��
10,10,10,0q�63   $y�Visible=�5�   1   .��5�      �� $ 
Horizontal=�6   .+�5�      �� $��35��6   $@��:/TecCargo Faktura System;component/Images/Icons/cancel.png��5�      �� 5�      $��Annuller��6   $��20(�6/   5�      5�      5�      5�      5�      