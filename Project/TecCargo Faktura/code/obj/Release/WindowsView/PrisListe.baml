   M S B A M L   `   `   `  ����   TecCargo Faktura System,    &TecCargo_Faktura.WindowsView.prisListe   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     $
��auto��$	��599��  � WindowStartupLocation$  CenterScreen=� � Icon$> 8/TecCargo Faktura System;component/Images/Icons/logo.ico��.���  � RowDefinitions �� $��50 ��� $��* ��� $��30 �.+�� $��0��$��Stretch=�$
��Auto�����	#FF2C72B8.+��� $��Beløb for faktura��$��Center=�$��115,10,104,0q�$��Center=�$
��Bold"�$��Segoe UI Black)�$��18(����	#FF254078$
��auto��$
��auto���� $��Right=�$��45��$��0,5,10,0q�$��Center=�$��45��$<��6/TecCargo Faktura System;component/Images/Pdf/logo.png��� $��1��$��Stretch=�$
��auto��$
��Auto��.+�&   System.Windows.Controls.DataGrid  -     Name$ datagridPricelist��$��Center=�$
��0,40q�   CanUserDeleteRows .     
IsReadOnly .    CanUserAddRows .     CanUserReorderColumns .     AutoGenerateColumns .  $	��Top=�$
��auto��$	��358������ *FakturaPriceDataView+	   Columns	 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn
   Header$

 Navn��   MinWidth$	 150Q�1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *name+  $

 Pris��$	 100Q� �� *price+   	CellStyle �� .���  �� Property � �� �� Value$ Right=�� $��2��$��Stretch=�$
��Auto�����	#FF2C72B8