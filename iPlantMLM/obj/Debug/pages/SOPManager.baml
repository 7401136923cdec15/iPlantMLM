   M S B A M L   `   `   `  ����O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  5       	iPlantMLM   iPlantMLM.SOPManager   5      � }InfragisticsWPF4.DataPresenter.ExcelExporter.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbm iInfragisticsWPF4.Editors.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbu qInfragisticsWPF4.Documents.Excel.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbp lInfragisticsWPF4.OutlookBar.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbe aInfragisticsWPF4.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbs oInfragisticsWPF4.DataPresenter.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb>igWPF(http://schemas.infragistics.com/xaml/wpf       � �InfragisticsWPF4.Controls.Editors.XamDateTimeInput.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb�	 |InfragisticsWPF4.Controls.Editors.XamSlider.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbn
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
Horizontal=�6A   .+�58      �� -   5        $��Btn_Add��58   M   $	��120��6   $��35��6!   $��15,0,0,0q�6-   $��15(�6?   ���	#FF00B9FF.��59      �� $ 
Horizontal=�6   .+�5:      �� $��/images\新增.png��6   $��25��64   �� 5;      .��6   上传6!   5<      5=      �� 5P      -   5        $��
Btn_Active��5P   M   $	��120��6   $��35��6!   $��15,0,0,0q�6-   $��15(�6?   ���	#FF00B9FF.��5Q      �� $ 
Horizontal=�6   .+�5R      �� $��/Images\激活.png��6   $��25��64   �� 5S      .��6   激活6    5T      5U      �� 5V      -   5        $��	Btn_Close��5V   M   $	��120��6   $��35��6!   $��15,0,0,0q�6-   $��15(�6?   ���	#FF00B9FF.��5W      �� $ 
Horizontal=�6   .+�5X      �� $��/Images\取消.png��6   $��25��64   �� 5Y      .��6   关闭6    5Z      5[      �� 5c      -   5        $��Btn_See��5c   M   $	��120��6   $��35��6!   $��15,0,0,0q�6-   $��15(�6?   ���	#FF00B9FF.��5d      �� $ 
Horizontal=�6   .+�5e      �� $��/Images\查询_黑.png��6   $��25��66   �� 5f      .��6   查看6    5g      5h      5i      � 5k   
   $��1��6   $��1��6   $��2��6,   .+�5l      �� -	   5         ��Name$ TB_Title��5l   f   $��*产品-工位未选择��6   $��20(�6)   $
��Bold"�67   $��Center=�6I   $	��Top=�6x   4  .Infragistics.Windows.DataPresenter.XamDataGrid  5m      -
   5          Name$ Xdg_MainGrid��5n   N   $��10,35,10,10q�5m       $��1��66   $��1��6C   $��2��6S   :  4Infragistics.Windows.DataPresenter.DataPresenterBase	   ThemeJ  DInfragistics.Windows.Themes.Internal.DataPresenterThemeTypeConverter$	 
Office2013 5n       
   GroupByAreaLocation$

 None=�63   $��18(�6�      ViewSettings 5o      9  3Infragistics.Windows.DataPresenter.GridViewSettings  5p         FieldSettings 5r      6  0Infragistics.Windows.DataPresenter.FieldSettings  5s         AllowRecordFiltering�  �Infragistics.Windows.Helpers.NullableConverter`1[[System.Boolean, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]$
 True 6*      	AllowEdit$ False 6F      Width$
 autoX�6X      AllowResize$
 True 6e      LabelTextAlignment�  �Infragistics.Windows.Helpers.NullableConverter`1[[System.Windows.TextAlignment, PresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35]]$ Center 6x      FieldLayoutSettings 5u      <  6Infragistics.Windows.DataPresenter.FieldLayoutSettings  5v         AutoGenerateFields$
 trueY�60      RecordSelectorLocation$ LeftOfCellArea=�6K   "   HeaderPrefixAreaDisplayMode$
 None=�6s      HeaderPlacementInGroupBy$ 	OnTopOnly=�6�      HeaderPlacement$ 	OnTopOnly=�6�      RecordSelectorNumberType$ DataItemIndex=�5w      !   RecordSelectorNumberFormat$ IncludeParentRecordNumber=�6@       RecordSelectorNumberStart$ 1X�6x      FieldLayouts 5y      4	  .Infragistics.Windows.DataPresenter.FieldLayout	  5z       	  
FieldItems. 5{      .
  (Infragistics.Windows.DataPresenter.Field
  2  ,Infragistics.Windows.DataPresenter.FieldItem   Name$ ID��6&      Label$ ID��60      
Visibility$ 	Collapsed=�6;   
  5|      $ ProductName��6&   $ 产品规格��69   !  
  HorizontalContentAlignment�  �Infragistics.Windows.Helpers.NullableConverter`1[[System.Windows.HorizontalAlignment, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35]]$  Center 6F   !   Width$
! autoX�6j   
  5}      $ PartName��6&   $ 工位��66   $  Center 6A   $
! autoX�6e   
  5~      $ 	ProductID��6&   $ 	ProductID��67   $ 	Collapsed=�6I   
  5      $ PartID��6&   $ PartID��64   $ 	Collapsed=�6C   
  5�      $ FileName��6&   $ 文件名称��66   $  Center 6C   $
! autoX�6g   
  5�      $ FilePath��6&   $ 文件路径��66   $  Center 6C   $
! autoX�6g   $ 	Collapsed=�6t   
  5�      $ UploadTimeText��6&   $ 上传时间��6<   $  Center 6I   $
! autoX�6m   
  5�      $ TypeText��6&   $ 文件类型��66   $  Center 6C   $
! autoX�6g   
  5�      $ Operator��6&   $ 	操作人��66   $  Center 6B   $
! autoX�6f   
  5�      $ ValidTimeText��6&   $ 生效时刻��6;   $  Center 6H   $
! autoX�6l   
  5�      $ 
ActiveText��6&   $ 启用状态��68   $  Center 6E   $
! autoX�6i   
  5�      $
 Type��6&   $
 Type��62   $ 	Collapsed=�6?   
  5�      $ 
UploadTime��6&   $ 
UploadTime��68   $ 	Collapsed=�6K   
  5�      $ 
OperatorID��6&   $ 
OperatorID��68   $ 	Collapsed=�6K   
  5�      $ 	ValidTime��6&   $ 	ValidTime��67   $ 	Collapsed=�6I   
  5�      $ Active��6&   $ Active��64   $ 	Collapsed=�6C   5�      5�      5�      5�      5�      