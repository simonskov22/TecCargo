   M S B A M L   `   `   `  ����  TecCargo Dagbog#    TecCargo_Dagbog.View.OpenList   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  
Fil finder��5   	   $
��auto��6   $
��auto��6*    � Icon$0 */TecCargo Dagbog;component/Images/logo.ico��67    � 
ResizeMode$ NoResize=�5   	    � WindowStartupLocation$ CenterScreen=�6    � WindowStyle$
 none=�5   	   ���	#FF2C72B8$��2,2,2,2q�64    � ShowInTaskbar .   � SizeToContent$ WidthAndHeight=�5      .��5      � $
��auto��6   $
��auto��6    � RowDefinitions 5	   
   �� 5
      $��50 �6   �� 5      $��* �6   �� 5      $��35 �6   .+�5   
   � $��0��6   $��Stretch=�6   $
��Auto��6:   ���	#FF2C72B8.+�5      �� $��
Fil Finder��6   $
��Bold"�6)   $��18(�6;   $��10,8,0,0q�6I   $
��auto��6[   $
��auto��6i   $	��Top=�6v   $
��Left=�6�   ���	#FF0034686�   �� 5      $��Right=�6   $��45��60   $��0,5,10,0q�6<   $��Center=�6N   $��45��6i   $0��*/TecCargo Dagbog;component/Images/logo.png��6t   5      � 5   
   $��1��6   $��Center=�6   $	��Top=�69   $
��auto��6Q   $
��Auto��6_   .+�5      � $
��auto��6   $��10q�6!    5      �� 5      $
��auto �6$   �� 5      $
��auto �6$   �� 5      $
��auto �6$   .+�5      � $��0��6   $��0,0,0,10q�6$   .+�5      �� $��Navn:��6   $
��Left=�6,   $��0,0,0,0q�6G   $��Center=�6X   �� 5      -   5        $�� ��5      $��	100,0,0,0q�6&   $	��100��69   $��24��6E   $��Center=�6Q   $
��Left=�6s   $��Center=�6�   6�   5      �� 5       -   5         ��Name$ ListViewOpenFileNames��5    (   $��1��6   $��Stretch=�6G   $	��168��6e   $	��550��6r   $��0,0,0,10q�6�   $	��Top=�6�   	 �� View	 5!      �� 5"      .P�5#      �� 
 �� Width$	
 100��6-   $
��Navn��69    �� DisplayMemberBinding 6G   �� *name+5$      5&      � 5'      $��2��6   $��0,0,0,0q�6$   .+�5(      �� $��Filnavn:��6   $
��Left=�6/   $��0,0,0,0q�6J   $	��Top=�6[   �� 5)      -   5         ��Name$ TextBoxFileName_name��5)      $�� ��6S   $��Right=�6^   $��26��6z   $��0,0,0,0q�6�   $	��Top=�6�   $	��312��6�   
����������	#FF858585$��1q�6�   $��5,3,5,5q�6�   5*      5+      5,      � 5-   
   $��2��6   $��Stretch=�6   $
��Auto��6:   ���	#FF2C72B8.+�5/      �� -   5         ��Name$ ButtonFileName_save��5/      $��12(�62   {�.   �� 	IsDefault . $��Åben��6c   $��Right=�6r   $��5q�6�   $
��10,3q�6�   $	��Top=�6�   $
��auto��6�   $
��auto��6�   6�   �� 50      -   5        $ ButtonFileName_canel��50      $��12(�63    �� IsCancel . $��Annuller��6Q   $
��Left=�6d   $��5q�6   $	��5,3q�6�   $	��Top=�6�   $
��auto��6�   $
��auto��6�   6�   52      54      55      