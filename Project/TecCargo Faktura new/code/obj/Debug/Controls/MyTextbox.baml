   M S B A M L   `   `   `  ����O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  5      R NTecCargo Faktura System, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:TecCargo_Faktura.ModelsTecCargo_Faktura.Models 5      )   #TecCargo_Faktura.Controls.MyTextbox   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      9System&clr-namespace:System;assembly=mscorlib     5      8models%clr-namespace:TecCargo_Faktura.Models     5      -   5          >�Name$  _Textbox��5       K� ZIndex$
 1000��5      ��5	      �� *Width+ �� FallbackValue$	 300�� � 	Resources 5      7  1TecCargo_Faktura.Models.BoolToVisibilityConverter   ConverterPlaceholder�  �TecCargo_Faktura.Models.BoolToValueConverter`1[[System.Windows.Visibility, PresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35]]   	TrueValue   
FalseValue2  ,TecCargo_Faktura.Models.BoolToBrushConverter  ConverterBorderBrush�  �TecCargo_Faktura.Models.BoolToValueConverter`1[[System.Windows.Media.Brush, PresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35]]   	TrueValue   
FalseValue��%s   &	        &	 6       5   
   $ Visible=�6H   $ 	Collapsed=�6\     5   
   
 �  ��
 �����6S   5      .��5      � -   5        
�������$��0q�5      $
��Left=�6)   {�5      �� *
IsUnLock+ �� ElementName$ _Textbox��.+�5   
   �� -   5        	 ��Name$	 
_hovertext��5      {�. $y�	Collapsed=�5      ���	#FF5489BD���	#FFE2E0E0$	��Top=�5      $
��Left=�5      $
��10,5q�5      $
��Bold"�5!      $��1,1,1,0q�5"      ���	#FF5489BD��5      �� *Placeholder+$ _Textbox����5      �� *Width+$ _Textbox����5      �� *Height+$ _Textbox����5       �� *
TextSize+$ _Textbox��5#      �� 5%   
   -   5        
 ��Name$
 placeholder��5%      $
��Left=�5&      ���	#FF5489BD���	#FFE2E0E0 �� Padding$
 10,5q�5+      ��5'      �� *Placeholder+$ _Textbox����5)      �� *Width+$ _Textbox����5*      �� *Height+$ _Textbox����5,      �� *
TextSize+$ _Textbox��y�5-      �� $ Input�� �� Path$ Text.IsEmpty� �� 	Converter# [  5/      �� 52   
   -   5         ��Name$ Input��58      $��1q�54      $	��Top=�55      $
��Left=�56      
������ $
��10,5q�5;      $��Center=�5=      ��53      �� *RequireEnableRedBorder+$ _Textbox��# [ ��57      �� *Text+$ _Textbox����59      �� *Width+$ _Textbox����5:      �� *Height+$ _Textbox����5<      �� *
TextSize+$ _Textbox��5?      5A      5B      