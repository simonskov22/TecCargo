   M S B A M L   `   `   `  ����  TecCargo Læreplads DB*    $TecCargo_Læreplads_DB.View.OpenList   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  
Fil finder��5   	   $
��auto��6   $
��auto��6*    � Icon$7 1/TecCargo Læreplads DB;component/Images/logo.png��67    � 
ResizeMode$ NoResize=�5   	    � WindowStartupLocation$ CenterScreen=�6    � WindowStyle$
 none=�5   	   ���	#FF2C72B8$��2,2,2,2q�64    � ShowInTaskbar .   � SizeToContent$ WidthAndHeight=�6d   .��5   
   � $
��auto��6   $
��auto��6    � RowDefinitions 5      �� 5	      $��50 �6    �� 5
      $��* �6    �� 5      $��35 �6    .+�5   
   � $��0��6   $��Stretch=�6   $
��Auto��6:   ���	#FF2C72B8.+�5      �� $��
Fil Finder��6   $
��Bold"�6)   $��18(�6;   $��10,8,0,0q�6I   $
��auto��6[   $
��auto��6i   $	��Top=�6v   $
��Left=�6�   ���	#FF0034686�   �� 5      $��Right=�6   $��45��60   $��0,5,10,0q�6<   $��Center=�6N   $��45��6i   $7��1/TecCargo Læreplads DB;component/Images/logo.ico��6t   5      � 5   
   $��1��6   $��10q�6   $��Center=�6(   $	��Top=�6E   $
��auto��6]   $
��Auto��6k    5      �� 5      $
��auto �6    �� 5      $
��auto �6    �� 5      $
��auto �6    �� 5      $
��auto �6!   .+�5      � $��0��6   $��0,0,0,10q�6    .+�5      �� $��
Virsomhed:��6   $
��Left=�61   $��0,0,0,0q�6L   $��Center=�6]   �� 5      -   5        $��TextBox_Search��5      $�� ��64   $��70,0,0,0q�6<   $	��240��6N   $��24��6Z   $��Center=�6f   $
��Left=�6�   $��Center=�6�   6�   5      �� 5       -   5        $��ListViewOpenFileNames��5    $   $��1��6   $��Stretch=�6A   $	��168��6_   $
��auto��6l   $��0,0,0,10q�6y   $	��Top=�6�    �� View 5!      �� 5"      .P�5#      �� 	 �� Width$		 240��6)   $��	Virsomhed��65   
 �� DisplayMemberBinding
 6H   �� *name+�� 5$      $	 40��6)    �� CellTemplate 5%      �� 5&   "   .R�5'   &   �� -   5        $
��Slet��5'   -   6<   5(   #   5*      5+      5-      � 5.      $��2��6   $��0,0,0,0q�6    .+�5/      �� $��Filnavn:��6   $
��Left=�6+   $��0,0,0,0q�6F   $	��Top=�6W   �� 50      -   5         ��Name$ TextBoxFileName_name��50      $�� ��6O   $��Right=�6Z   $��26��6v   $��0,0,0,0q�6�   $	��Top=�6�   $	��312��6�   
����������	#FF858585$��1q�6�   $��5,3,5,5q�6�   51      52      � 53   
   $��2��6   $��Stretch=�6   $
��Auto��6:   ���	#FF2C72B8.+�55      �� -   5         ��Name$ ButtonFileName_save��55      $��12(�62   {�.   �� 	IsDefault . $��Åben��6c   $��Right=�6r   $��5q�6�   $
��10,3q�6�   $	��Top=�6�   $
��auto��6�   $
��auto��6�   6�   �� 56      -   5        $ ButtonFileName_canel��56      $��12(�63    �� IsCancel . $��Annuller��6Q   $
��Left=�6d   $��5q�6   $	��5,3q�6�   $	��Top=�6�   $
��auto��6�   $
��auto��6�   6�   57      59      5:      