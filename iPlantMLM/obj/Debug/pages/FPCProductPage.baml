   M S B A M L   `   `   `  ����  	iPlantMLM    iPlantMLM.FPCProductPage   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      � }InfragisticsWPF4.DataPresenter.ExcelExporter.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbm iInfragisticsWPF4.Editors.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbu qInfragisticsWPF4.Documents.Excel.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbp lInfragisticsWPF4.OutlookBar.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbe	 aInfragisticsWPF4.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbs
 oInfragisticsWPF4.DataPresenter.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb>igWPF(http://schemas.infragistics.com/xaml/wpf     	 
 5      -   5          N� Title$  UserManagerPage��5	      .��5      � 
������� � ColumnDefinitions 5   
   �� 5      $��7* �6   �� 5      $	��210 �6   .+�5   
   �� $��产品规格列表��6   $
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
 ,Infragistics.Windows.DataPresenter.FieldItem   Name$ 	ProductID��6&      Label$  ��67      
Visibility$ 	Collapsed=�6@   
  5!      $ ProductName��6&   $ 产品规格��69   ! 
  HorizontalContentAlignment� 	 �Infragistics.Windows.Helpers.NullableConverter`1[[System.Windows.HorizontalAlignment, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35]]$ Center 6F      Width$
 autoX�6j   
  5"      $
 Name��6&   $ 产品名称��62   $ Center 6?   $
 autoX�6c   
  5#      $ Model��6&   $ 产品型号��63   $ Center 6@   $
 autoX�6d   
  5$      $ DescribeInfo��6&   $ 产品描述��6:   $ Center 6G   $
 autoX�6k   
  5%      $ ProductCode��6&   $ 生产阶段代码��69   $ Center 6H   $
 autoX�6l   
  5&      $ BarCodePrefix��6&   $ 二维码前缀��6;   $ Center 6I   $
 autoX�6m   
  5'      $ PackagePrefix��6&   $ 电容包前缀��6;   $ Center 6I   $
 autoX�6m   5(      5*      5+      �� 5-   
   $��产品规格操作��6   $
��Bold"�6#   $��12(�65   $��5,0,5,5q�6C   $��1��6T   .��5.      �� $
��Auto=�6   $
��Auto=�6@   .��5/      �� -   5        $��	Stp_Right��5/      .+�50      �� $��产品规格��6   
�������$��1q�6C   $��
10,10,10,0q�6W   $	��160��6k   $��Center=�6w   $��Center=�6�   �� 51      -   5        $��Tbx_UserName��51       � Cursor$
 Hand��62   
�������$��1q�6X   $��Stretch=�6l   $��
10,-1,10,0q�6�   $	��160��6�   $��30��6�   $��Center=�6�   $��Center=�6�   $��Center=�6�   �� 53      $��产品名称��6   
�������$��1q�6C   $��
10,10,10,0q�6W   $	��160��6k   $��Center=�6w   $��Center=�6�   �� 54      -   5        $��Tbx_ProName��54      $
 Hand��61   
�������$��1q�6W   $��Stretch=�6k   $��
10,-1,10,0q�6�   $	��160��6�   $��30��6�   $��Center=�6�   $��Center=�6�   $��Center=�6�   �� 56      $��产品型号��6   
�������$��1q�6C   $��
10,10,10,0q�6W   $	��160��6k   $��Center=�6w   $��Center=�6�   �� 57      -   5        $��	Tbx_Model��57      $
 Hand��6/   
�������$��1q�6U   $��Stretch=�6i   $��
10,-1,10,0q�6�   $	��160��6�   $��30��6�   $��Center=�6�   $��Center=�6�   $��Center=�6�   �� 59      $��产品描述��6   
�������$��1q�6C   $��
10,10,10,0q�6W   $	��160��6k   $��Center=�6w   $��Center=�6�   �� 5:      -   5        $��Tbx_Des��5:      $
 Hand��6-   
�������$��1q�6S   $��Stretch=�6g   $��
10,-1,10,0q�6�   $	��160��6�   $��60��6�   $��Center=�6�   $��Center=�6�   $��Center=�6�   �� 5<      $��生产阶段代码��6   
�������$��1q�6E   $��
10,10,10,0q�6Y   $	��160��6m   $��Center=�6y   $��Center=�6�   �� 5=      -   5        $��Tbx_ProductCode��5=      $
 Hand��65   
�������$��1q�6[   $��Stretch=�6o   $��
10,-1,10,0q�6�   $	��160��6�   $��30��6�   $��Center=�6�   $��Center=�6�   $��Center=�6�   �� 5?      $��二维码前缀��6   
�������$��1q�6D   $��
10,10,10,0q�6X   $	��160��6l   $��Center=�6x   $��Center=�6�   �� 5@      -	   5        $��Tbx_CodePrefix��5@      $
 Hand��64   
�������$��1q�6Z   $��Stretch=�6n   $��
10,-1,10,0q�6�   $	��160��6�   $��30��6�   $��Center=�6�   $��Center=�6�   $��Center=�6�   �� 5B      $��电容包前缀��6   
�������$��1q�6D   $��
10,10,10,0q�6X   $	��160��6l   $��Center=�6x   $��Center=�6�   �� 5C      -
   5        $��Tbx_PackagePrefix��5C      $
 Hand��67   
�������$��1q�6]   $��Stretch=�6q   $��
10,-1,10,0q�6�   $	��160��6�   $��30��6�   $��Center=�6�   $��Center=�6�   $��Center=�6�   �� 5E      -   5         ��Name$ Cmd_Add��5E      $
 Hand��6.   
�������$��1q�6T   $��
10,10,10,0q�6h   $	��160��6|   $��Center=�6�   $��Center=�6�   $��30��6�   ���	#FF00B9FF.��5F      ��  �� Orientation$ 
Horizontal=�6%   .+�5G      �� $��/images\保存.png��6$   $��25��6<   �� 5H      .��6$   新增6(   5I      5J      �� 5L      -   5        $ Cmd_Save��5L      $
 Hand��6/   
�������$��1q�6U   $��
10,10,10,0q�6i   $	��160��6}   $��Center=�6�   $��Center=�6�   $��30��6�   ���	#FF00B9FF.��5M      �� $ 
Horizontal=�6%   .+�5N      �� $��/images\修改.png��6$   $��25��6<   �� 5O      .��6$   修改6(   5P      5Q      �� 5S      -   5        $ 
Cmd_Delete��5S      $
 Hand��61   
�������$��1q�6W   $��
10,10,10,0q�6k   $	��160��6   $��Center=�6�   $��Center=�6�   $��30��6�   ���	#FF00B9FF.��5T      �� $ 
Horizontal=�6%   .+�5U      �� $��/images\删 除.png��6$   $��25��6=   �� 5V      .��6$   删除6(   5W      5X      �� 5Z      -   5        $ Cmd_ResetPsw��5Z      $
 Hand��63   
�������$��1q�6Y   $��
10,10,10,0q�6m   $	��160��6�   $��Center=�6�   $��Center=�6�   $��30��6�   ���	#FF00B9FF.��5[      �� $ 
Horizontal=�6%   .+�5\      �� $��/images\重置密码.png��6$   $��25��6>   �� 5]      .��6$   Excel导出6-   5^      5_      �� 5a      -   5        $ Cmd_CheckRule��5a      $
 Hand��64   
�������$��1q�6Z   $��
10,10,10,0q�6n   $	��160��6�   $��Center=�6�   $��Center=�6�   $��30��6�   ���	#FF00B9FF.��5b      �� $ 
Horizontal=�6%   .+�5c      �� $��/images\制度规程.png��6$   $��25��6>   �� 5d      .��6$   检测规程6*   5e      5f      �� 5h      -   5        $ Cmd_CapacityRule��5h      $
 Hand��67   
�������$��1q�6]   $��
10,10,10,0q�6q   $	��160��6�   $��Center=�6�   $��Center=�6�   $��30��6�   ���	#FF00B9FF.��5i      �� $ 
Horizontal=�6%   .+�5j      �� $��/images\24规则.png��6$   $��25��6>   �� 5k      .��6$   分档规则6*   5l      5m      �� 5o      -   5        $ Cmd_SOP��5o      $
 Hand��6.   
�������$��1q�6T   $��
10,10,10,0q�6h   $	��160��6|   $��Center=�6�   $��Center=�6�   $��30��6�   ���	#FF00B9FF.��5p      �� $ 
Horizontal=�6%   .+�5q      �� $��/images\sop.png��6$   $��25��6=   �� 5r      .��6$   作业指导书6+   5s      5t      5u      5v      5w      5x      5y      