   M S B A M L   `   `   `  ����  	iPlantMLM#    iPlantMLM.CapacityGradingPage   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      � }InfragisticsWPF4.DataPresenter.ExcelExporter.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbm iInfragisticsWPF4.Editors.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbu qInfragisticsWPF4.Documents.Excel.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbp lInfragisticsWPF4.OutlookBar.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbe	 aInfragisticsWPF4.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbs
 oInfragisticsWPF4.DataPresenter.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb>igWPF(http://schemas.infragistics.com/xaml/wpf     	 
 5      -   5          N� Title$  UserManagerPage��5	      .��5      � 
������� � ColumnDefinitions 5   
   �� 5      $��7* �6   �� 5      $	��210 �6   .+�5   
   �� $��容量分档列表��6   $
��Bold"�6#   $��12(�65   $��5,0,5,5q�6C   .��5      4 
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
 ,Infragistics.Windows.DataPresenter.FieldItem   Name$ ID��6&      Label$  ��60      
Visibility$ 	Collapsed=�69   
  5!      $ 	ProductID��6&   $  ��67   $ 	Collapsed=�6@   
  5"      $ ProductName��6&   $ 产品规格��69   ! 
  HorizontalContentAlignment� 	 �Infragistics.Windows.Helpers.NullableConverter`1[[System.Windows.HorizontalAlignment, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35]]$ Center 6F      Width$
 autoX�6j   
  5#      $
 Gear��6&   $ 档位��62   $ Center 6=   $
 autoX�6a   
  5$      $ 
LowerLimit��6&   $  ��68   $ 	Collapsed=�6A   
  5%      $ LowerLimitText��6&   $ 	下限(F)��6<   $ Center 6J   $
 autoX�6n   
  5&      $ UpLimit��6&   $  ��65   $ 	Collapsed=�6>   
  5'      $ UpLimitText��6&   $ 	上限(F)��69   $ Center 6G   $
 autoX�6k   
  5(      $ Explain��6&   $ 说明��65   $ Center 6@   $
 autoX�6d   5)      5+      5,      �� 5.   
   $��容量分档操作��6   $
��Bold"�6#   $��12(�65   $��5,0,5,5q�6C   $��1��6T   .��5/      �� $
��Auto=�6   $
��Auto=�6@   .��50      �� -   5        $��	Stp_Right��50      .+�51      �� $��产品规格��6   
�������$��1q�6C   $��
10,10,10,0q�6W   $	��160��6k   $��Center=�6w   $��Center=�6�   �� 52      -   5        $��Cbb_Role��52       � Cursor$
 Hand��6/   
�������$��1q�6U   $��Stretch=�6i   $��
10,-1,10,0q�6�   $	��160��6�   $��30��6�   $��Center=�6�   $��Center=�6�   $��Center=�6�   
������� �� SelectedValuePath$ 	ProductID��6(   �� DisplayMemberPath$ ProductName��6F  �� 54      $��档位��6   
�������$��1q�6A   $��
10,10,10,0q�6U   $	��160��6i   $��Center=�6u   $��Center=�6�   �� 55      -   5        $��Tbx_UserName��55      $
 Hand��62   
�������$��1q�6X   $��Stretch=�6l   $��
10,-1,10,0q�6�   $	��160��6�   $��30��6�   $��Center=�6�   $��Center=�6�   $��Center=�6�   �� 57      $��容量下限(F)��6   
�������$��1q�6F   $��
10,10,10,0q�6Z   $	��160��6n   $��Center=�6z   $��Center=�6�   �� 58      -   5        $��
Tbx_WorkNo��58      $
 Hand��60   
�������$��1q�6V   $��Stretch=�6j   $��
10,-1,10,0q�6�   $	��160��6�   $��30��6�   $��Center=�6�   $��Center=�6�   $��Center=�6�   �� 5:      $��容量上限(F)��6   
�������$��1q�6F   $��
10,10,10,0q�6Z   $	��160��6n   $��Center=�6z   $��Center=�6�   �� 5;      -   5        $��	Tbx_Phone��5;      $
 Hand��6/   
�������$��1q�6U   $��Stretch=�6i   $��
10,-1,10,0q�6�   $	��160��6�   $��30��6�   $��Center=�6�   $��Center=�6�   $��Center=�6�   �� 5=      $��说明��6   
�������$��1q�6A   $��
10,10,10,0q�6U   $	��160��6i   $��Center=�6u   $��Center=�6�   $
��Bold"�6�   �� 5>      -   5        $��Tbx_Explain��5>      $
 Hand��61   
�������$��1q�6W   $��Stretch=�6k   $��
10,-1,10,0q�6�   $	��160��6�   $��60��6�   $��Center=�6�   $��Center=�6�   $��Center=�6�    �� TextWrapping$
 Wrap=�6  �� 5@      -	   5          ��Name$  Cmd_Save��5@      $
 Hand��6/   
�������$��1q�6U   $��
10,10,10,0q�6i   $	��160��6}   $��Center=�6�   $��Center=�6�   $��30��6�   ���	#FF00B9FF.��5A      �� ! �� Orientation$! 
Horizontal=�6%   .+�5B      �� $��/images\保存.png��6$   $��25��6<   �� 5C      .��6$   新增6(   5D      5E      �� 5G      -
   5        $  
Cmd_Update��5G      $
 Hand��61   
�������$��1q�6W   $��
10,10,10,0q�6k   $	��160��6   $��Center=�6�   $��Center=�6�   $��30��6�   ���	#FF00B9FF.��5H      �� $! 
Horizontal=�6%   .+�5I      �� $��/images\修改.png��6$   $��25��6<   �� 5J      .��6$   修改6(   5K      5L      �� 5N      -   5        $  
Cmd_Delete��5N      $
 Hand��61   
�������$��1q�6W   $��
10,10,10,0q�6k   $	��160��6   $��Center=�6�   $��Center=�6�   $��30��6�   ���	#FF00B9FF.��5O      �� $! 
Horizontal=�6%   .+�5P      �� $��/images\删 除.png��6$   $��25��6=   �� 5Q      .��6$   删除6(   5R      5S      �� 5U      -   5        $  
Cmd_Import��5U      $
 Hand��61   
�������$��1q�6W   $��
10,10,10,0q�6k   $	��160��6   $��Center=�6�   $��Center=�6�   $��30��6�   ���	#FF00B9FF.��5V      �� $! 
Horizontal=�6%   .+�5W      �� $��/images\创建用户.png��6$   $��25��6>   �� 5X      .��6$   Excel导入6-   5Y      5Z      �� 5\      -   5        $  Cmd_ResetPsw��5\      $
 Hand��63   
�������$��1q�6Y   $��
10,10,10,0q�6m   $	��160��6�   $��Center=�6�   $��Center=�6�   $��30��6�   ���	#FF00B9FF.��5]      �� $! 
Horizontal=�6%   .+�5^      �� $��/images\重置密码.png��6$   $��25��6>   �� 5_      .��6$   Excel导出6-   5`      5a      5b      5c      5d      5e      5f      