   M S B A M L   `   `   `  ����R  NTecCargo Faktura System, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:TecCargo_Faktura.ControlsTecCargo_Faktura.Controls  5      %    TecCargo_Faktura.Controls.Gebyr   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      9local'clr-namespace:TecCargo_Faktura.Controls     5      -   5          >�Name$  _Gebyr��5	      .��5
      �  � ColumnDefinitions 5   
   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6    � RowDefinitions 5   
   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   .+�5   
   .   (TecCargo_Faktura.Controls.CheckBoxButton  -   5          Name$ CheckBoxButton_Gebyr_0��5      .   (TecCargo_Faktura.Controls.CheckBoxButton   Click$  CheckBoxButton_Gebyr_Click��6?   $��0��6b   $��0��6o      Text$ Chaufførmedhjælper��6      	ImageSize$ 25��6�      TextSize$ 18��6�     5   
   -   5        $ CheckBoxButton_Gebyr_1��5      $  CheckBoxButton_Gebyr_Click��6?   $��1��6b   $��0��6o   $ Flytte tillæg��6   $ 25��6�   $ 18��6�     5   
   -   5        $ CheckBoxButton_Gebyr_2��5      $  CheckBoxButton_Gebyr_Click��6?   $��2��6b   $��0��6o   $ ADR-tillæg��6   $ 25��6�   $ 18��6�     5   
   -   5        $ CheckBoxButton_Gebyr_3��5      $  CheckBoxButton_Gebyr_Click��6?   $��3��6b   $��0��6o   $ Aften- og nattillæg��6   $ 25��6�   $ 18��6�     5   
   -   5        $ CheckBoxButton_Gebyr_4��5      $  CheckBoxButton_Gebyr_Click��6?   $��4��6b   $��0��6o   $ Weekendtillæg��6   $ 25��6�   $ 18��6�     5   
   -   5        $ CheckBoxButton_Gebyr_5��5      $  CheckBoxButton_Gebyr_Click��6?   $��5��6b   $��0��6o   $ Yderzonetillæg��6   $ 25��6�   $ 18��6�     5    
   -   5        $ CheckBoxButton_Gebyr_6��5       $  CheckBoxButton_Gebyr_Click��6?   $��0��6b   $��2��6o   $ Byttepalletillæg��6   $ 25��6�   $ 18��6�     5!   
   -	   5        $ CheckBoxButton_Gebyr_7��5!      $  CheckBoxButton_Gebyr_Click��6?   $��1��6b   $��2��6o   $ SMS servicetillæg��6   $ 25��6�   $ 18��6�     5"   
   -
   5        $ CheckBoxButton_Gebyr_8��5"      $  CheckBoxButton_Gebyr_Click��6?   $��2��6b   $��2��6o   $ Adresse korrektion��6   $ 25��6�   $ 18��6�     5#   
   -   5        $ CheckBoxButton_Gebyr_9��5#      $  CheckBoxButton_Gebyr_Click��6?   $��3��6b   $��2��6o   $ 
Bro afgift��6   $ 25��6�   $ 18��6�     5$   
   -   5        $ CheckBoxButton_Gebyr_10��5$      $  CheckBoxButton_Gebyr_Click��6@   $��4��6c   $��2��6p   $ 
Vej afgift��6�   $ 25��6�   $ 18��6�     5%   
   -   5        $ CheckBoxButton_Gebyr_11��5%      $  CheckBoxButton_Gebyr_Click��6@   $��5��6c   $��2��6p   $ Færge afgift��6�   $ 25��6�   $ 18��6�   )   #TecCargo_Faktura.Controls.MyTextbox  5'   
   -   5          Name$ MyTextbox_Gebyr_0��5'      )   #TecCargo_Faktura.Controls.MyTextbox	   TextChanged$!	 MyTextbox_Gebyr_TextChanged��65   $��0��6_   $��1��6l   
   Placeholder$
 Personer��6|      IsInt . $	��150��6�      TextSize$ 15��6�      IsUnLock 6�   �� *	Checked+ �� ElementName$ CheckBoxButton_Gebyr_0��!   ShowAlwaysPlaceholderOnTop 6�   �� *ShowAlwaysPlaceholderOnTop+$ _Gebyr��  5(   
   -   5        $ MyTextbox_Gebyr_1��5(      $!	 MyTextbox_Gebyr_TextChanged��65   $��1��6_   $��1��6l   $
 Enheder��6|    . $	��150��6�   $ 15��6�    6�   �� *	Checked+$ CheckBoxButton_Gebyr_1�� 6�   �� *ShowAlwaysPlaceholderOnTop+$ _Gebyr��  5)   
   -   5        $ MyTextbox_Gebyr_6��5)      $!	 MyTextbox_Gebyr_TextChanged��65   $��0��6_   $��3��6l   $
 Paller��6|    . $	��150��6�   $ 15��6�    6�   �� *	Checked+$ CheckBoxButton_Gebyr_6�� 6�   �� *ShowAlwaysPlaceholderOnTop+$ _Gebyr��  5*   
   -   5        $ MyTextbox_Gebyr_7��5*      $!	 MyTextbox_Gebyr_TextChanged��65   $��1��6_   $��3��6l   $
 Adviseringer��6|    . $	��150��6�   $ 15��6�    6�   �� *	Checked+$ CheckBoxButton_Gebyr_7�� 6�   �� *ShowAlwaysPlaceholderOnTop+$ _Gebyr��  5+   
   -   5        $ MyTextbox_Gebyr_9��5+      $!	 MyTextbox_Gebyr_TextChanged��65   $��3��6_   $��3��6l   $

 Pris��6|      IsDouble . $	��150��6�   $ 15��6�    6�   �� *	Checked+$ CheckBoxButton_Gebyr_9�� 6�   �� *ShowAlwaysPlaceholderOnTop+$ _Gebyr��  5,   
   -   5        $ MyTextbox_Gebyr_10��5,      $!	 MyTextbox_Gebyr_TextChanged��66   $��4��6`   $��3��6m   $

 Pris��6}    . $	��150��6�   $ 15��6�    6�   �� *	Checked+$ CheckBoxButton_Gebyr_10�� 6�   �� *ShowAlwaysPlaceholderOnTop+$ _Gebyr��  5-   
   -   5        $ MyTextbox_Gebyr_11��5-      $!	 MyTextbox_Gebyr_TextChanged��66   $��5��6`   $��3��6m   $

 Pris��6}    . $	��150��6�   $ 15��6�    6�   �� *	Checked+$ CheckBoxButton_Gebyr_11�� 6�   �� *ShowAlwaysPlaceholderOnTop+$ _Gebyr��5.      5/      