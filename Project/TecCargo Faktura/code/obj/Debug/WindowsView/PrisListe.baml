   M S B A M L   `   `   `  ����  TecCargo Faktura System,    &TecCargo_Faktura.WindowsView.prisListe   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   $
��auto��6@   $	��599��6N     � WindowStartupLocation$  CenterScreen=�6Z    � Icon$> 8/TecCargo Faktura System;component/Images/Icons/logo.ico��6   .��5      �  � RowDefinitions 5   
   �� 5      $��50 �6   �� 5      $��* �6   �� 5      $��30 �6   .+�5   
   � $��0��6   $��Stretch=�6   $
��Auto��6:   ���	#FF2C72B8.+�5      �� $��Beløb for faktura��6   $��Center=�60   $��115,10,104,0q�6M   $��Center=�6c   $
��Bold"�6~   $��Segoe UI Black)�6�   $��18(�6�   ���	#FF254078$
��auto��6�   $
��auto��6�   �� 5      $��Right=�6   $��45��60   $��0,5,10,0q�6<   $��Center=�6N   $��45��6i   $<��6/TecCargo Faktura System;component/Images/Pdf/logo.png��6t   5      � 5   
   $��1��6   $��Stretch=�6   $
��auto��6:   $
��Auto��6H   .+�5      &   System.Windows.Controls.DataGrid  -   5          Name$ datagridPricelist��5      $��Center=�62   $
��0,40q�6O      CanUserDeleteRows .     
IsReadOnly .    CanUserAddRows .     CanUserReorderColumns .     AutoGenerateColumns .  $	��Top=�6�   $
��auto��6�   $	��358��6  ��6  �� *FakturaPriceDataView+	   Columns	 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn
   Header$

 Navn��6)      MinWidth$	 150Q�6P   1  +System.Windows.Controls.DataGridBoundColumn   Binding 67   �� *name+6P     5      $

 Pris��6)   $	 100Q�6Q    67   �� *price+   	CellStyle 5      �� 5      .�5   "   ��  �� Property � �� �� Value$ Right=�6Y   5      5      5      5      � 5   
   $��2��6   $��Stretch=�6   $
��Auto��6:   ���	#FF2C72B85      5       5!      