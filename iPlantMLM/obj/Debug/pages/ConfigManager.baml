   M S B A M L   `   `   `  ����O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  5       	iPlantMLM   iPlantMLM.ConfigManager   5      � }InfragisticsWPF4.DataPresenter.ExcelExporter.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbm iInfragisticsWPF4.Editors.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbu qInfragisticsWPF4.Documents.Excel.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbp lInfragisticsWPF4.OutlookBar.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbe aInfragisticsWPF4.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbs oInfragisticsWPF4.DataPresenter.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb>igWPF(http://schemas.infragistics.com/xaml/wpf       � �InfragisticsWPF4.Controls.Editors.XamDateTimeInput.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb�	 |InfragisticsWPF4.Controls.Editors.XamSlider.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbn
 jInfragisticsWPF4.DragDrop.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbw sInfragisticsWPF4.DataVisualization.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb� |InfragisticsWPF4.Controls.Menus.XamDataTree.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb� �InfragisticsWPF4.Controls.Editors.XamMaskedInput.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb� ~InfragisticsWPF4.Controls.Charts.XamDataChart.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb� �InfragisticsWPF4.Controls.Editors.XamPropertyGrid.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbq mInfragisticsWPF4.DataManager.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb?ig$http://schemas.infragistics.com/xaml
  	 
        5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      7core&clr-namespace:System;assembly=mscorlib     5      -   5          N� Title$  ConfigManager��5      .��5      � 
������� � RowDefinitions 5   
   �� 5      $	��22* �6   �� 5      $
��341* �6    � ColumnDefinitions 5   
   �� 5      $	��275 �6   �� 5      $
��588* �6   �� 5      $
��245* �6   .+�5   
   �� 
���   �$��1q�6%   $��1��69   $��0��6F   $��0,0,0,10q�6V   ._�5      �� -   5         ��Name$ SV_DateList��5      $y�Visible=�61   $��Disabled=�6F   $
��Auto=�6o    �� PanningMode$
 Both=�6�   .��5      �� -   5         ��Name$ TV_Main��5   4    �� Orientation$ Vertical=�6   5      5      5      �� 57   
   -   5        $��Stp_Top��57      $��0��6$   $��1��61   $ 
Horizontal=�6A   .+�58      �� -   5        $��Btn_Add��58   L   $��80��6   $��35��6    $��15,0,0,0q�6,   $��15(�6>   ���	#FF00B9FF.��59      �� $ 
Horizontal=�6   .+�5:      �� $��/images\新增.png��6   $��25��64   �� 5;      .��6   新增6!   5<      5=      �� 5>      -   5        $��Btn_Up��5>   L   $��80��6   $��35��6    $��15,0,0,0q�6,   $��15(�6>   ���	#FF00B9FF.��5?      �� $ 
Horizontal=�6   .+�5@      �� $��/Images\上 移.png��6   $��25��65   �� 5A      .��6   上移6    5B      5C      �� 5D      -   5        $��Btn_Down��5D   L   $��80��6   $��35��6    $��15,0,0,0q�6,   $��15(�6>   ���	#FF00B9FF.��5E      �� $ 
Horizontal=�6   .+�5F      �� $��/Images\下 移.png��6   $��25��65   �� 5G      .��6   下移6    5H      5I      �� 5P      -   5        $��
Btn_Active��5P   L   $��80��6   $��35��6    $��15,0,0,0q�6,   $��15(�6>   ���	#FF00B9FF.��5Q      �� $ 
Horizontal=�6   .+�5R      �� $��/Images\激活.png��6   $��25��64   �� 5S      .��6   激活6    5T      5U      �� 5V      -	   5        $��	Btn_Close��5V   L   $��80��6   $��35��6    $��15,0,0,0q�6,   $��15(�6>   ���	#FF00B9FF.��5W      �� $ 
Horizontal=�6   .+�5X      �� $��/Images\取消.png��6   $��25��64   �� 5Y      .��6   关闭6    5Z      5[      �� 5\      -
   5        $��
Btn_Export��5\   M   $	��120��6   $��35��6!   $��15,0,0,0q�6-   $��15(�6?   ���	#FF00B9FF.��5]      �� $ 
Horizontal=�6   .+�5^      �� $��/Images\保存_黑.png��6   $��25��66   �� 5_      .��6   Excel导出6%   5`      5a      5b      � 5d   
   $��1��6   $��1��6   $��2��6,   .+�5e      �� -   5         ��Name$ TB_Title��5e   f   $��*产品-工位未选择��6   $��20(�6)   $
��Bold"�67   $��Center=�6I   $	��Top=�6x   4  .Infragistics.Windows.DataPresenter.XamDataGrid  5f      -   5          Name$ Xdg_MainGrid��5g   N   $��10,35,10,10q�5f       $��1��66   $��1��6C   $��2��6S   :  4Infragistics.Windows.DataPresenter.DataPresenterBase	   ThemeJ  DInfragistics.Windows.Themes.Internal.DataPresenterThemeTypeConverter$	 
Office2013 5g       
   GroupByAreaLocation$

 None=�63   $��18(�6�      ViewSettings 5h      9  3Infragistics.Windows.DataPresenter.GridViewSettings  5i         FieldSettings 5k      6  0Infragistics.Windows.DataPresenter.FieldSettings  5l         AllowRecordFiltering�  �Infragistics.Windows.Helpers.NullableConverter`1[[System.Boolean, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]$
 True 6*      	AllowEdit$ False 6F      Width$
 autoX�6X      AllowResize$
 True 6e      LabelTextAlignment�  �Infragistics.Windows.Helpers.NullableConverter`1[[System.Windows.TextAlignment, PresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35]]$ Center 6x      FieldLayoutSettings 5n      <  6Infragistics.Windows.DataPresenter.FieldLayoutSettings  5o         AutoGenerateFields$
 trueY�60      RecordSelectorLocation$ LeftOfCellArea=�6K   "   HeaderPrefixAreaDisplayMode$
 None=�6s      HeaderPlacementInGroupBy$ 	OnTopOnly=�6�      HeaderPlacement$ 	OnTopOnly=�6�      RecordSelectorNumberType$ DataItemIndex=�5p      !   RecordSelectorNumberFormat$ IncludeParentRecordNumber=�6@       RecordSelectorNumberStart$ 1X�6x      FieldLayouts 5r      4	  .Infragistics.Windows.DataPresenter.FieldLayout	  5s       	  
FieldItems. 5t      .
  (Infragistics.Windows.DataPresenter.Field
  2  ,Infragistics.Windows.DataPresenter.FieldItem   Name$ ID��6&      Label$ ID��60      
Visibility$ 	Collapsed=�6;   
  5u      $ 	ProductID��6&   $ 	ProductID��67   $ 	Collapsed=�6I   
  5v      $ PartPointID��6&   $ PartPointID��69   $ 	Collapsed=�6M   
  5w      $ ItemID��6&   $ ItemID��64   $ 	Collapsed=�6C   
  5x      $ PartID��6&   $ PartID��64   $ 	Collapsed=�6C   
  5y      $
 Type��6&   $
 Type��62   $ 	Collapsed=�6?   
  5z      $ EditorID��6&   $ EditorID��66   $ 	Collapsed=�6G   
  5{      $ EditTime��6&   $ EditTime��66   $ 	Collapsed=�6G   
  5|      $ Active��6&   $ Active��64   $ 	Collapsed=�6C   
  5}      $ OrderID��6&   $ OrderID��65   $ 	Collapsed=�6E   
  5      $ ProductName��6&   $ 产品规格��69   !  
  HorizontalContentAlignment�  �Infragistics.Windows.Helpers.NullableConverter`1[[System.Windows.HorizontalAlignment, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35]]$  Center 6F   !   Width$
! autoX�6j   
  5�      $ PartName��6&   $ 工位名称��66   $  Center 6C   $
! autoX�6g   
  5�      $ ItemCode��6&   $ 检验项点编号��66   $  Center 6E   $
! autoX�6i   
  5�      $ ItemName��6&   $ 检验项点名称��66   $  Center 6E   $
! autoX�6i   
  5�      $ TypeName��6&   $ 数值类型��66   $  Center 6C   $
! autoX�6g   
  5�      $ 
LowerLimit��6&   $ 数值下限��68   $  Center 6E   $
! autoX�6i   $ 	Collapsed=�6v   
  5�      $ LowerLimitText��6&   $ 数值下限��6<   $  Center 6I   $
! autoX�6m   
  5�      $ 
UpperLimit��6&   $ 数值上限��68   $  Center 6E   $
! autoX�6i   $ 	Collapsed=�6v   
  5�      $ UpperLimitText��6&   $ 数值上限��6<   $  Center 6I   $
! autoX�6m   
  5�      $ UnitText��6&   $ 数值单位��66   $  Center 6C   $
! autoX�6g   
  5�      $ DefaultValue��6&   $ 	默认值��6:   $  Center 6F   $
! autoX�6j   
  5�      $ TextDescription��6&   $ 文本说明��6=   $  Center 6J   $
! autoX�6n   
  5�      $ Editor��6&   $ 	编辑人��64   $  Center 6@   $
! autoX�6d   
  5�      $ EditTimeText��6&   $ 编辑时间��6:   $  Center 6G   $
! autoX�6k   
  5�      $ 
ActiveText��6&   $ 激活状态��68   $  Center 6E   $
! autoX�6i   5�      5�      5�      5�      5�      