   M S B A M L   `   `   `  ����  TecCargo Faktura System    TecCargo_Faktura.FileName   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5        $
��auto��5   	   $
��auto��6     � 
ResizeMode$  NoResize=�6$    � WindowStartupLocation$ CenterScreen=�6:    � SizeToContent$ WidthAndHeight=�6_    � Icon$ Images/Icons/logo.ico��6~   .��5      � $
��auto��6   $
��auto��6    � RowDefinitions 5   
   �� 5      $��50 �6   �� 5      $��* �6   �� 5	      $��30 �6   .+�5   
   � $��0��6   $��Stretch=�6   $
��Auto��6:   ���	#FF2C72B8.+�5      �� -   5         ��Name$ windowTitle��5      $�� ��6)   $��Center=�64   $��0,0,0,0q�6Q   $��Center=�6b   $
��Bold"�6}   $��Segoe UI Black)�6�   $��18(�6�   ���	#FF254078$
��auto��6�   $
��auto��6�   �� 5      $��Right=�6   $��45��60   $��0,5,10,0q�6<   $��Center=�6N   $��45��6i   $<��6/TecCargo Faktura System;component/Images/Pdf/logo.png��6t   5      � 5   
   $��1��6   $��Stretch=�6   $
��auto��6:   $
��Auto��6H   .+�5      � $
��auto��6   $��10,10,10,10q�6!    5      �� 5      $
��auto �6$   �� 5      $
��auto �6$   �� 5      $
��auto �6$   �� 5      $
��auto �6$   .+�5      � $��0��6   $��0,0,0,10q�6$   .+�5      �� $��Fragtbrev nr:��6   $
��Left=�64   $��0,0,0,0q�6O   $��Center=�6`   �� 5      -   5        $�� ��5      $��	100,0,0,0q�6&   $	��100��69   $��24��6E   $��Center=�6Q   $
��Left=�6s   $��Center=�6�   6�   5      �� 5      -   5         ��Name$ ListViewOpenFileNames��5   (   $��1��6   $��Stretch=�6G   $	��168��6e   $	��550��6r   $��0,0,0,10q�6�   $	��Top=�6�    �� View 5      �� 5       .P�5!      ��  �� Width$	 100��6-   $��Filnavn��69   	 �� DisplayMemberBinding	 6J   �� *	Filnavn+�� 5"      $	 100��6-   $��Ændringsdato��69   	 6O   �� *
EditDate+�� 5#      $
 auto��6-   $��Opretter��6:   	 6L   �� *	creatorId+�� 5$      $
 auto��6-   $��Fragtbrev: Status��6:   	 6U   �� *statusFragt+�� 5%      $
 auto��6-   $��Faktura: Status��6:   	 6S   �� *
statusFakt+5&      5(      � 5)      $��2��6   $��0,0,0,10q�6$   .+�5*      �� $��Filnavn:��6   $
��Left=�6/   $��0,0,0,0q�6J   $	��Top=�6[   �� 5+      -   5        $ TextBoxFileName_name��5+      $�� ��6S   $��Right=�6^   $��26��6z   $��0,0,0,0q�6�   $	��Top=�6�   $	��312��6�   
����������	#FF858585$��1q�6�   $��5,3,5,5q�6�   5,      � 5-      $��3��6   .+�5.      �� -   5        
 ��Name$
 ButtonFileName_save��5.      {�.   �� 	IsDefault . $��Åben��6]   $��Right=�6l   $��0,0,0,0q�6�   $	��Top=�6�   $��75��6�   $��27��6�   �� 5/      -   5        $
 ButtonFileName_canel��5/       �� IsCancel . $��Annuller��6K   $
��Left=�6^   $��0,0,0,0q�6y   $	��Top=�6�   $��75��6�   $��27��6�   50      5?      5@      � 5A   
   $��2��6   $��Stretch=�6   $
��Auto��6:   ���	#FF2C72B85B      5D      5E      