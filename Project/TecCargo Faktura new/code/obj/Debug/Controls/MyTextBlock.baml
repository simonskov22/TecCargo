   M S B A M L   `   `   `  ����R  NTecCargo Faktura System, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:TecCargo_Faktura.ControlsTecCargo_Faktura.Controls  5      A%clr-namespace:TecCargo_Faktura.ModelsTecCargo_Faktura.Models  5      +    %TecCargo_Faktura.Controls.MyTextBlock   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      9local'clr-namespace:TecCargo_Faktura.Controls     5      7model%clr-namespace:TecCargo_Faktura.Models     5      -   5          >�Name$  
_Textblock��5       K� ZIndex$
 1000��5      $��0q�6#   ��5      �� *Height+ �� FallbackValue$	 200�� � 	Resources 5      7   1TecCargo_Faktura.Models.BoolToVisibilityConverter   PlaceholderConverter�   �TecCargo_Faktura.Models.BoolToValueConverter`1[[System.Windows.Visibility, PresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35]]   	TrueValue   
FalseValue2   ,TecCargo_Faktura.Models.BoolToBrushConverter  ConverterBorderBrush�   �TecCargo_Faktura.Models.BoolToValueConverter`1[[System.Windows.Media.Brush, PresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35]]   	TrueValue   
FalseValue��%s   &	        &	 6       5   
   $ Visible=�6G   $ 	Collapsed=�6[     5   
   
 �  ��
 �����6R   5      .��5      � -   5        
�������$
��Left=�5      {�69   �� *
IsUnLock+ �� ElementName$ 
_Textblock��.+�5   
   �� -   5        	 ��Name$	 
_hovertext��5      $y�Hidden=�6$   {�. $��0q�6I   ���	#FF5489BD���	#FFE2E0E0$
��auto��5      $	��Top=�5      $
��Left=�5      $
��10,5q�5      $
��Bold"�5      $��1,1,1,0q�5      ���	#FF5489BD$	 100��5      ��5      �� *Placeholder+$ 
_Textblock����5      �� *Width+$ 
_Textblock����5      �� *PlaceTextSize+$ 
_Textblock��5       �� 5#   
   -   5        
 ��Name$
 placeholder��5#      $��0q�6*   ���	#FF5489BD �� Padding$
 10,5q�5(      ���	#FFE2E0E0��5$      �� *Placeholder+$ 
_Textblock����5&      �� *Width+$ 
_Textblock����5'      �� *Height+$ 
_Textblock����5(      �� *PlaceTextSize+$ 
_Textblock��y�5)      �� $ Input�� �� Path$ Text.IsEmpty� �� 	Converter# [  5,      �� 5.   
   -   5         ��Name$ textboxScroll��5.      $��Disabled=�6`   $
��Auto=�6�   $��0q�6�   ��6.   �� *Height+$ 
_Textblock��.��5/      �� -   5        $��Input��55       �� TextWrapping$
 Wrap=�5/      $��1q�52      $��0q�53      
������  �� AcceptsReturn . $
��auto��57      $
��10,5q�58      $	��Top=�5:      ��51      �� *RequireEnableRedBorder+$ 
_Textblock��# [ ��54      �� *Text+$ 
_Textblock����56      �� *Width+$ 
_Textblock����57      �� *Height+$ 
_Textblock����59      �� *
TextSize+$ 
_Textblock��5=      5?      5@      5A      