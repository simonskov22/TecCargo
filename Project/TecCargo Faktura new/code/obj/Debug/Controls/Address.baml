   M S B A M L   `   `   `  ����R  NTecCargo Faktura System, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:TecCargo_Faktura.ControlsTecCargo_Faktura.Controls  5      '    !TecCargo_Faktura.Controls.Address   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      <TecCargo'clr-namespace:TecCargo_Faktura.Controls     5      -   5        $��_Address��5      .��5
      � -   5          �Name$  Grid_Contain��5
       � RowDefinitions 5   
   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   .+�5   
   )   #TecCargo_Faktura.Controls.MyTextbox  -   5          Name$ Textbox_ContactTlf��5   *   )   #TecCargo_Faktura.Controls.MyTextbox   TextChanged$! Textbox_GetList_TextChanged��6m      PreviewKeyDown$ Textbox_Contact_KeyDown��6�   $��1��6      Placeholder$ Kontakt Nr.��6F      IsInt . $	��0,3q�6�      IsUnLock 6�   �� *
IsUnLock+ �� ElementName$ _Address��!	   ShowAlwaysPlaceholderOnTop	 6  �� *ShowAlwaysPlaceholderOnTop+$ _Address��  5   
   -   5        $ Textbox_FirmName��5   *   $! Textbox_GetList_TextChanged��6]   $ Textbox_Contact_KeyDown��6�   $��2��6   $ 
Firma/Navn��6D   $	��0,3q�6�    6�   �� *
IsUnLock+$ _Address��	 6�   �� *ShowAlwaysPlaceholderOnTop+$ _Address��  5   
   -   5        $ Textbox_ContactPer��5   *   $��3��6   $ Kontakt Person��6F   $	��0,3q�6c    6p   �� *
IsUnLock+$ _Address��	 6�   �� *ShowAlwaysPlaceholderOnTop+$ _Address��  5   
   -   5        $ Textbox_Address��5   *   $��4��6   $ Adresse��6C   $	��0,3q�6Y    6f   �� *
IsUnLock+$ _Address��	 6�   �� *ShowAlwaysPlaceholderOnTop+$ _Address��  5   
   -   5        $ Textbox_ZipCode��5   *   
   KeyUp$
 Textbox_ZipCode_KeyUp��6�   $! Textbox_ZipCode_TextChanged��6�   $��5��6   $ Post Nr.��6C   $	��0,3q�6\      	MaxLength$ 4��6i    . $	��120��6�   $
��Left=�6�    6�   �� *
IsUnLock+$ _Address��	 6'  �� *ShowAlwaysPlaceholderOnTop+$ _Address���� 5   
   -   5         ��Name$ TextBlock_City��5      $��5��6,   $
��Left=�69   $�� ��6T   $��20(�6\   $��Center=�6j   $��	130,0,0,0q�6�   .   (TecCargo_Faktura.Controls.CheckBoxButton  5   
   -	   5          Name$ CheckBox_Pay��5   /   $��6��6"   $	��300��6v      Text 6E   �� *	TextCheck+$ _Address��y�6�   �� *CheckboxVisibility+$ _Address��{�6�   �� *
IsUnLock+$ _Address���� 5    
   -
   5         ��Name$ ListBox_Customer��5       $	��150��6-   $��0,45,0,0q�6=   $��1��6O   $��6��6\    K� ZIndex$
 1100��6m   $	��Top=�5!      $	��Top=�6*   $
��2,15q�5"      $��18(�6!   
�������$y�	Collapsed=�5$   &   ��5%      �� 5&       �� 
TargetType" ��6    �� Triggers 5'      P� 5(       P� Property �@��
IsSelected P� Value . . �5)      ��  �� Property � �� �� Value$
 Bold"�6;   �� 5*       � �� �	#FF5DA8F36;   �� 5+       � ��
 �   �6;   5,       �� 	Resources 5.       �� Color��%{  (��       5/   *   *$"SystemColors.HighlightTextBrushKey+)(�� #     50   *   * SystemColors.HighlightBrushKey+)(�� F     51   *   *SystemColors.ControlBrushKey+)(�� i     52   *   *1/SystemColors.InactiveSelectionHighlightBrushKey+)�� 5/      $ 	#FF5DA8F3��6`   �� 50      $ 	#FF5DA8F3��6\   �� 51      $ 	#FF5DA8F3��6Z   �� 52      $ 	#FF5DA8F3��6m   53      54      56      57      58      