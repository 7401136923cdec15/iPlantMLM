   M S B A M L   `   `   `  ����D  @iPlantMLM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullM+clr-namespace:Shris.NewEnergy.iPlant.DeviceShris.NewEnergy.iPlant.Device  5   	   /    )Shris.NewEnergy.iPlant.Device.ExcSelectUI   5   	   � �InfragisticsWPF4.Controls.Editors.XamDateTimeInput.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb� |InfragisticsWPF4.Controls.Editors.XamSlider.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbn jInfragisticsWPF4.DragDrop.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbw sInfragisticsWPF4.DataVisualization.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb� |InfragisticsWPF4.Controls.Menus.XamDataTree.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb� �InfragisticsWPF4.Controls.Editors.XamMaskedInput.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb� ~InfragisticsWPF4.Controls.Charts.XamDataChart.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbe aInfragisticsWPF4.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb�	 �InfragisticsWPF4.Controls.Editors.XamPropertyGrid.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbq
 mInfragisticsWPF4.DataManager.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb?ig$http://schemas.infragistics.com/xaml
         	 
 \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   :My+clr-namespace:Shris.NewEnergy.iPlant.Device     5   	     � Title$  车型选择��5   	   $	��800��6   $	��850��6#    � 
ResizeMode$ NoResize=�6/    � WindowStartupLocation$ CenterScreen=�6E    � AllowsTransparency .  � WindowStyle$
 None=�6�   
������  � Icon$ /Images/crrc_icon.jpg��6�   $��18(�6�    � 	Resources 5         AddObjectStyle �� 
TargetType �� Triggers	 P� Property
 P� Value �� Property �� Value  RemoveObjectStyle  BorderStyleB  BorderStyleC  IGBorderStyle  BorderStyle��%D  &	        &	     &	     &	 F    &	 ~    &	 �    �� 5	   
   " ��6'    5
      P� 5      	 �@J�IsMouseOver
 . . �5      ��  � ��$ /Images/add.png��65   6O   5      P� 5      	 �@J�IsMouseOver
 .  . �5      ��  � ��$ /Images/add (2).png��65   6S   5      5      �� 5   
   " ��6*    5      P� 5      	 �@J�IsMouseOver
 . . �5      ��  � ��$ /Images/remove.png��65   6R   5      P� 5      	 �@J�IsMouseOver
 .  . �5      ��  � ��$ /Images/remove (2).png��65   6V   5      5      �� 5   
   " ��6%    5      P� 5      	 �@J�IsMouseOver
 . . �5       ��  � ��
 �����63   �� 5!       � ��
 �����64   5"      P� 5#      	 �@J�IsMouseOver
 .  . �5$      ��  � ��
 �����63   �� 5%       � ��
 �����64   5&      5(      �� 5)   
   " ��6%    5*      P� 5+      	 �@J�IsMouseOver
 . . �5,      ��  � ��
 ��ح�63   �� 5-       � �� �#3E3E3E64   5.      P� 5/      	 �@J�IsMouseOver
 .  . �50      ��  � ��
 �����63   �� 51       � �� �#3E3E3E64   52      54      �� 55   
   " ��6&    56      P� 57      	 �@J�IsMouseOver
 . . �58      ��  � �� �#309ECB63   �� 59       � �� �#2788B164   5:      P� 5;      	 �@J�IsMouseOver
 .  . �5<      ��  � �� �#52525263   �� 5=       � �� �#3E3E3E64   5>      5@      �� 5A   
   " ��6$   .�5B      ��  � ��
 �   �6,   5C      5D      .��5F      �� 
�������$��1q�6    $��0,0,0,0q�65   $��Center=�6F   
�������._�5G   
   ��  �� Orientation$ Vertical=�6   $��Center=�6,   .+�5H      �� $��2��6   
�������$��0,0,0,1q�65   ���#F0F0F0$��45��6d   ._�5I      � .+�5J      �� -   5        
�������$
��Left=�5J   0   $��1q�6K    �� CornerRadius$ 5��6_   ���#eeeeee � Cursor$
 Hand��6�   $��80��6�   $��5q�6�    J� Effect 5K      3  -System.Windows.Media.Effects.DropShadowEffect  5L         
BlurRadius$ 4Q�6/      ShadowDepth$ 0Q�6>   ._�5N      � .+�5O      �� $��1��6$   $��/Images/back.png��64   $
��Left=�6N   $��5,0,0,0q�6i   �� 5P      $��返回��6$   $��Right=�61   $��Center=�6M   
���   �$��0,0,8,0q�6{   5Q      5R      �� 5S      -   5         ��Name$ TB_Title��5S       $��选择��62   $
��Bold"�6<   $��Center=�6N   $��Center=�6k   $��25(�6�   �� 5U      -   5         ��Name$ Border_AllSelect��5U   �   
�������$��Right=�60   $��1q�6L   $ 5��6`   ���#eeeeee$
 Hand��6�   $��80��6�   $��5q�6�   $y�	Collapsed=�6�    5V        5W      $ 4Q�6/   $ 0Q�6>   ._�5Y      � .+�5Z      �� $��全选��6$   $��Center=�61   $��Center=�6N   
���   �$��0,0,0,0q�6|   5[      5\      5]      5^      � 5`      $��5q�6   .+�5a      �� -   5        $ 	TB_Search��5a   �   $��请输入搜索内容��6   $��Center=�6+   $��20,0,0,0q�6F   $
��Left=�6X   
�������6�   �� 5b      -   5         ��Name$ TE_CallContent��5b   ;   
������ $�� ��63   $��45��6S   $��Center=�6_   $��20,0,0,0q�6�   
����  �6�   5c      �� 5e      $	��600��6   $
��Auto=�6)   $
��Auto=�6N    �� PanningMode$
 Both=�6q   .��5f      � .+�5g      0   *Shris.NewEnergy.iPlant.Device.SelectItemUC  -   5          Name$ 	SIU_Items��5g   1   $��0q�6&   61   � 5i      -   5         �Name$ Grid_SearchTip��5i      $��0,5,0,5q�63   $y�	Collapsed=�6D   $	��Top=�6[   .+�5j      �� $��搜索结果为空!��6$   $��5,0,5,0q�63   $��20(�6D   $��Center=�6R   
���  ��6o   5k      5l      5m      �� 5o      $��2��6   
�������$��0,1,0,0q�65   
�������$��50��6b   $��0q�6n   ._�5p      �� -   5        $ BT_Save��5p   �   $��2��6   $ 8��6&   $��Center=�67   $	��150��6T   $��35��6`   $��1q�6l   $��5q�6�   #��[ 6�   $
 Hand��6�   ._�5q      �� $ 
Horizontal=�6!   $��Center=�6:   $��Center=�6W   .+�5r      �� $��25��6    $��/Images/hj_qr.png��6+   �� 5s      $��确认��6    $��18(�6-   $
��Bold"�6;   
���   �$��Center=�6`   $��Center=�6�   $��0,0,0,0q�6�   5t      5u      5v      5w      5x      5y      