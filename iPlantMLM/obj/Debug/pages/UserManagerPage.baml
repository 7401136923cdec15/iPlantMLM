   M S B A M L   `   `   `  ����  	iPlantMLM    iPlantMLM.UserManagerPage   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      � }InfragisticsWPF4.DataPresenter.ExcelExporter.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbm iInfragisticsWPF4.Editors.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbu qInfragisticsWPF4.Documents.Excel.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbp lInfragisticsWPF4.OutlookBar.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbe	 aInfragisticsWPF4.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbs
 oInfragisticsWPF4.DataPresenter.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb>igWPF(http://schemas.infragistics.com/xaml/wpf     	 
 5      -   5          N� Title$  UserManagerPage��5	      .��5      � 
������� � ColumnDefinitions 5   
   �� 5      $��7* �6   �� 5      $	��210 �6   .+�5   
   �� $��用户信息��6   $
��Bold"�6!   $��12(�63   $��5,0,5,5q�6A   .��5      4 
 .Infragistics.Windows.DataPresenter.XamDataGrid  -   5          Name$ Xdg_MainGrid��5   N   $��10q�5       $��1��6-   $��5��6:   : 
 4Infragistics.Windows.DataPresenter.DataPresenterBase   ThemeJ 
 DInfragistics.Windows.Themes.Internal.DataPresenterThemeTypeConverter$ 
Office2013 5          GroupByAreaLocation$
 None=�63   $��20(�6�      ViewSettings 5      9 
 3Infragistics.Windows.DataPresenter.GridViewSettings  5         FieldSettings 5      6 
 0Infragistics.Windows.DataPresenter.FieldSettings  5         AllowRecordFiltering� 	 �Infragistics.Windows.Helpers.NullableConverter`1[[System.Boolean, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]$
 True 6*      	AllowEdit$ False 6F   	   Width$
	 autoX�6X   
   AllowResize$

 True 6e      LabelTextAlignment� 	 �Infragistics.Windows.Helpers.NullableConverter`1[[System.Windows.TextAlignment, PresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35]]$ Center 6x      FieldLayoutSettings 5      < 
 6Infragistics.Windows.DataPresenter.FieldLayoutSettings  5         AutoGenerateFields$
 trueY�60      RecordSelectorLocation$ LeftOfCellArea=�6K   "   HeaderPrefixAreaDisplayMode$
 None=�6s      HeaderPlacementInGroupBy$ 	OnTopOnly=�6�      HeaderPlacement$ 	OnTopOnly=�6�      RecordSelectorNumberType$ DataItemIndex=�5      !   RecordSelectorNumberFormat$ IncludeParentRecordNumber=�6@       RecordSelectorNumberStart$ 1X�6x      FieldLayouts 5      4	 
 .Infragistics.Windows.DataPresenter.FieldLayout	  5       	  
FieldItems. 5       .
 
 (Infragistics.Windows.DataPresenter.Field
  2 
 ,Infragistics.Windows.DataPresenter.FieldItem   Name$ ID��6&      Label$ ID��60      
Visibility$ 	Collapsed=�6;   
  5!      $
 Name��6&   $ 	用户名��62   ! 
  HorizontalContentAlignment� 	 �Infragistics.Windows.Helpers.NullableConverter`1[[System.Windows.HorizontalAlignment, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35]]$ Center 6>      Width$
 autoX�6b   
  5"      $ LoginID��6&   $ 工号��65   $ Center 6@   $
 autoX�6d   
  5#      $ Phone��6&   $ 电话��63   $ Center 6>   $
 autoX�6b   
  5$      $ GradName��6&   $ 角色��66   $ Center 6A   $
 autoX�6e   
  5%      $ PartPowerName��6&   $ 工位权限��6;   $ Center 6H   $
 autoX�6l   
  5&      $ Password��6&   $ 密码��66   $ 	Collapsed=�6A   
  5'      $ 	LoginName��6&   $  ��67   $ 	Collapsed=�6@   
  5(      $ 
CreateDate��6&   $  ��68   $ 	Collapsed=�6A   
  5)      $ Operator��6&   $  ��66   $ 	Collapsed=�6?   
  5*      $ DepartmentID��6&   $  ��6:   $ 	Collapsed=�6C   
  5+      $
 Grad��6&   $  ��62   $ 	Collapsed=�6;   
  5,      $ Manager��6&   $  ��65   $ 	Collapsed=�6>   
  5-      $ Email��6&   $  ��63   $ 	Collapsed=�6<   
  5.      $ WeiXin��6&   $  ��64   $ 	Collapsed=�6=   
  5/      $ Position��6&   $  ��66   $ 	Collapsed=�6?   
  50      $ PhoneMAC��6&   $  ��66   $ 	Collapsed=�6?   
  51      $ Online��6&   $  ��64   $ 	Collapsed=�6=   
  52      $ 
OnLineTime��6&   $  ��68   $ 	Collapsed=�6A   
  53      $ DepartureDate��6&   $  ��6;   $ 	Collapsed=�6D   
  54      $ LastOnLineTime��6&   $  ��6<   $ 	Collapsed=�6E   
  55      $ DutyID��6&   $  ��64   $ 	Collapsed=�6=   
  56      $
 Type��6&   $  ��62   $ 	Collapsed=�6;   
  57      $ 
SuperiorID��6&   $  ��68   $ 	Collapsed=�6A   
  59      $ Company��6&   $  ��65   $ 	Collapsed=�6>   
  5:      $ 
Department��6&   $  ��68   $ 	Collapsed=�6A   
  5;      $ OnShift��6&   $  ��65   $ 	Collapsed=�6>   
  5<      $ 	PartPower��6&   $  ��67   $ 	Collapsed=�6@   
  5?      $ Active��6&   $ 激活状态��64   $ Center 6A   $
 autoX�6e   $ 	Collapsed=�6r   
  5B      $ 
ActiveText��6&   $ 激活状态��68   $ Center 6E   $
 autoX�6i   5C      5E      5F      �� 5H   
   $��用户操作��6   $
��Bold"�6!   $��12(�63   $��5,0,5,5q�6A   $��1��6R   .��5I      �� $
��Auto=�6   $
��Auto=�6@   .��5J      �� -   5        $��	Stp_Right��5J      .+�5K      �� $��	用户名��6   
�������$��1q�6B   $��
10,10,10,0q�6V   $	��160��6j   $��Center=�6v   $��Center=�6�   �� 5L      -   5        $��Tbx_UserName��5L       � Cursor$
 Hand��62   
�������$��1q�6X   $��Stretch=�6l   $��
10,-1,10,0q�6�   $	��160��6�   $��30��6�   $��Center=�6�   $��Center=�6�   $��Center=�6�   �� 5M      $��工号��6   
�������$��1q�6A   $��
10,10,10,0q�6U   $	��160��6i   $��Center=�6u   $��Center=�6�   �� 5N      -   5        $��
Tbx_WorkNo��5N      $
 Hand��60   
�������$��1q�6V   $��Stretch=�6j   $��
10,-1,10,0q�6�   $	��160��6�   $��30��6�   $��Center=�6�   $��Center=�6�   $��Center=�6�   �� 5P      $��电话��6   
�������$��1q�6A   $��
10,10,10,0q�6U   $	��160��6i   $��Center=�6u   $��Center=�6�   �� 5Q      -   5        $��	Tbx_Phone��5Q      $
 Hand��6/   
�������$��1q�6U   $��Stretch=�6i   $��
10,-1,10,0q�6�   $	��160��6�   $��30��6�   $��Center=�6�   $��Center=�6�   $��Center=�6�   �� 5S      $��角色��6   
�������$��1q�6A   $��
10,10,10,0q�6U   $	��160��6i   $��Center=�6u   $��Center=�6�   �� 5T      -   5        $��Cbb_Role��5T      $
 Hand��6/   
�������$��1q�6U   $��Stretch=�6i   $��
10,-1,10,0q�6�   $	��160��6�   $��30��6�   $��Center=�6�   $��Center=�6�   $��Center=�6�   
������� �� SelectedValuePath$ ID��6(   �� DisplayMemberPath$
 Name��6?  �� 5V      $��激活状态��6   
�������$��1q�6C   $��
10,10,10,0q�6W   $	��160��6k   $��Center=�6w   $��Center=�6�   �� 5W      -   5        $��
Cbb_Active��5W      $
 Hand��61   
�������$��1q�6W   $��Stretch=�6k   $��
10,-1,10,0q�6�   $	��160��6�   $��30��6�   $��Center=�6�   $��Center=�6�   $��Center=�6�   
�������$	 Key��6*  $ Value��6B  �� 5Y      $��0��6   $��0��6,   $��	10,0,10,5q�6<   ��5Z      �� 5[      .+�5\   "   �� $��工位权限��6(   $
��Bold"�67   5]      .��5_      �� -	   5         ��Name$ SP_StationPower��5_   M     �� Orientation$  Vertical=�6%   $��5,0,0,0q�6<   .+�5`      �� $  
Horizontal=�6)   .+�5a   "   �� $��电容包检测1(OP010)��6+   
! � Tag$! 1��6C   5b      �� 5c      $  
Horizontal=�6)   .+�5d   "   �� $��电容包检测2(OP011)��6+   $! 2��6C   5e      �� 5f      $  
Horizontal=�6)   .+�5g   "   �� $��电容包静置(OP020)��6+   $! 3��6B   5h      �� 5i      $  
Horizontal=�6)   .+�5j   "   �� $��单体电压检测(OP021)��6+   $! 4��6C   5k      �� 5l      $  
Horizontal=�6)   .+�5m   "   �� $��电容包放电(OP022)��6+   $! 5��6B   5n      �� 5o      $  
Horizontal=�6)   .+�5p   "   �� $��容量内阻测试(OP030)��6+   $! 6��6C   5q      �� 5r      $  
Horizontal=�6)   .+�5s   "   �� $��电容包装配(OP040)��6+   $! 7��6B   5t      �� 5u      $  
Horizontal=�6)   .+�5v   "   �� $��模组装配(OP050)��6+   $! 8��6A   5w      �� 5x      $  
Horizontal=�6)   .+�5y   "   �� $��模组检测(OP060)��6+   $! 9��6A   5z      �� 5{      $  
Horizontal=�6)   .+�5|   "   �� $��三串测试(OP070)��6+   $! 10��6A   5}      �� 5~      $  
Horizontal=�6)   .+�5   "   �� $��包装(装箱)(OP080)��6+   $! 11��6C   5�      �� 5�      $  
Horizontal=�6)   .+�5�   "   �� $��包装(装托)(OP080)��6+   $! 12��6C   5�      5�      5�      �� 5�      -
   5        " ��Name$" Cmd_Add��5�      $
 Hand��6.   
�������$��1q�6T   $��
10,10,10,0q�6h   $	��160��6|   $��Center=�6�   $��Center=�6�   $��30��6�   ���	#FF00B9FF.��5�      �� $  
Horizontal=�6%   .+�5�      �� $��/images\创建用户.png��6$   $��25��6>   �� 5�      .��6$   创建用户6*   5�      5�      �� 5�      -   5        $" 
Cmd_Import��5�      $
 Hand��61   
�������$��1q�6W   $��
10,10,10,0q�6k   $	��160��6   $��Center=�6�   $��Center=�6�   $��30��6�   ���	#FF00B9FF.��5�      �� $  
Horizontal=�6%   .+�5�      �� $��/images\创建用户.png��6$   $��25��6>   �� 5�      .��6$   导入用户6*   5�      5�      �� 5�      -   5        $" Cmd_Disable��5�      $
 Hand��62   
�������$��1q�6X   $��
10,10,10,0q�6l   $	��160��6�   $��Center=�6�   $��Center=�6�   $��30��6�   ���	#FF00B9FF.��5�      �� $  
Horizontal=�6%   .+�5�      �� $��/images\取消.png��6$   $��25��6<   �� 5�      .��6$   关闭用户6*   5�      5�      �� 5�      -   5        $" 
Cmd_Active��5�      $
 Hand��61   
�������$��1q�6W   $��
10,10,10,0q�6k   $	��160��6   $��Center=�6�   $��Center=�6�   $��30��6�   ���	#FF00B9FF.��5�      �� $  
Horizontal=�6%   .+�5�      �� $��/Images\激活.png��6$   $��25��6<   �� 5�      .��6$   激活用户6*   5�      5�      �� 5�      -   5        $" Cmd_Save��5�      $
 Hand��6/   
�������$��1q�6U   $��
10,10,10,0q�6i   $	��160��6}   $��Center=�6�   $��Center=�6�   $��30��6�   ���	#FF00B9FF.��5�      �� $  
Horizontal=�6%   .+�5�      �� $��/images\保存.png��6$   $��25��6<   �� 5�      .��6$   修改用户6*   5�      5�      �� 5�      -   5        $" Cmd_ResetPsw��5�      $
 Hand��63   
�������$��1q�6Y   $��
10,10,10,0q�6m   $	��160��6�   $��Center=�6�   $��Center=�6�   $��30��6�   ���	#FF00B9FF.��5�      �� $  
Horizontal=�6%   .+�5�      �� $��/images\重置密码.png��6$   $��25��6>   �� 5�      .��6$   重置密码6*   5�      5�      �� 5�      -   5        $" 
Cmd_Export��5�      $
 Hand��61   
�������$��1q�6W   $��
10,10,10,0q�6k   $	��160��6   $��Center=�6�   $��Center=�6�   $��30��6�   ���	#FF00B9FF.��5�      �� $  
Horizontal=�6%   .+�5�      �� $��/images\保存_黑.png��6$   $��25��6>   �� 5�      .��6$   导出用户6*   5�      5�      5�      5�      5�      5�      5�      