   M S B A M L   `   `   `  ����R  NTecCargo Faktura System, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:TecCargo_Faktura.ControlsTecCargo_Faktura.Controls  5      (    "TecCargo_Faktura.Controls.PrisList   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      9local'clr-namespace:TecCargo_Faktura.Controls     5      -   5        $	��300��5   #     � 	Resources  5         Datagrid_TextRight �� 
TargetType �� Property �� Value  PrimaryBrush �� Color  PrimaryFont 	 Row_1 	 Row_2  Row_End  Row_End_Color��%m  &	        &	 Y     &	 |     &	 �     &	 �     &	 �     &	     �� 5   
   " ��6+   .�5      ��  �@��TextAlignment$ Right=�6.   5      �� 5   
   $ 	#FF4169AA��6/   �� 5   
   $ White��6.   �� 5   
   $ 	#FFFFFFFF��6(   �� 5   
   $ 	#FFDEDEDE��6(   �� 5   
   $ 	#FFB6B6B6��6*   �� 5   
   $ 	#FF000000��60   5      .��5      � .+�5   
   &   System.Windows.Controls.DataGrid  -   5        $��_itemssource��5         AutoGenerateColumns .    RowHeaderWidth$ 0Q�6B      
IsReadOnly .  �� AlternationCount$ 2��5   %   	   GridLinesVisibility$
	 None=�6:   
   CanUserReorderColumns
 .     CanUserResizeColumns .     CanUserSortColumns .     CanUserResizeRows .  #��[ 5   -     5      ��%X   (��       5   "   * SystemColors.HighlightBrushKey+)�� 6   $ Transparent��6T   5         ColumnHeaderStyle 5       �� 5!      =  7System.Windows.Controls.Primitives.DataGridColumnHeader"  6   .�5"      ��  � ��# [ 63   �� 5#       � ��# [ 63   �� 5$       � ��$ Center=�6C   �� 5%       � ��$ 10q�60   �� 5&       � ��$ 15(�61   �� 5'       � ��$
 Bold"�63   �� 5(       � ��$
 Left=�6C   5)         	CellStyle 5+      �� 5,      *  $System.Windows.Controls.DataGridCell"  6   .�5.      ��  � ��$ 0q�68   �� 5/       � ��
 ���� 64   �� 50       � ��$ 5q�60   �� 51       � �� 52      �� 53       �� 
TargetType"  6.   .R�54   "   ��  �� Padding# z��6)   #��z��55   )   #��z��56   )   #��z��57   )    J� SnapsToDevicePixels . ._�59   &   �� # z 67   5:   #   5;      5=       �� Triggers 5@      P� 5A       P� Property �@ 
IsSelected P� Value .  . �5B      ��  � ��
 ���� 6;   �� 5C       �@J�IsHitTestVisible .  6A   5E      5I         RowStyle 5K      �� 5L      )  #System.Windows.Controls.DataGridRow"  6   .�5M      ��  � ��$ 0q�68   �� 5N       � ��
 ���� 64   �� 5O       � ��$
 5,15q�60    5Q      P� 5R       �@ AlternationIndex$ 0��6>   . �5S      ��  � ��# [ 6;   5T      P� 5U       �@ AlternationIndex$ 1��6>   . �5V      ��  � ��# [ 6;   5W      P� 5X       �@ 
IsSelected . . �5Y      ��  � ��
 ���� 6;   �� 5Z       �@J�IsHitTestVisible .  6A   5\      �� 5_       �� Binding 6&   �� *
isResult+ �� Value true6C   .a�5`      ��  � ��# [ 6;   �� 5a       � ��# [ 6;   �� 5b       � ��$
 Bold"�6;   5c      �� 5f       6&   �� *
isHeader+ true6C   .a�5g      ��  � ��$
 Bold"�6;   �� 5h       � ��$
 -3,0q�67   5i      �� 5k       6&   �� *	isSpace+ true6B   .a�5l      ��  � ��$	 0,0q�67   5m      5o         Columns 5r      0  *System.Windows.Controls.DataGridTextColumn  5s      ,  &System.Windows.Controls.DataGridColumn   Header$ Faktura��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$ * 6T   1  +System.Windows.Controls.DataGridBoundColumn   Binding 66   ��  �� Path$
 name�6T     5t      $
 Pris��6%      ElementStyle# [  6R      MinWidth$ 80Q�6�    63   �� $ price�6�   5w      5x      5y      