   M S B A M L   `   `   `  ����  	iPlantMLM#    iPlantMLM.ModuleHistoryWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   � �InfragisticsWPF4.Controls.Editors.XamDateTimeInput.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb� |InfragisticsWPF4.Controls.Editors.XamSlider.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbn jInfragisticsWPF4.DragDrop.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbw sInfragisticsWPF4.DataVisualization.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb�	 |InfragisticsWPF4.Controls.Menus.XamDataTree.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb�
 �InfragisticsWPF4.Controls.Editors.XamMaskedInput.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb� ~InfragisticsWPF4.Controls.Charts.XamDataChart.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbe aInfragisticsWPF4.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb� �InfragisticsWPF4.Controls.Editors.XamPropertyGrid.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbq mInfragisticsWPF4.DataManager.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb?ig$http://schemas.infragistics.com/xaml
     	 
     5   	   � }InfragisticsWPF4.DataPresenter.ExcelExporter.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbm iInfragisticsWPF4.Editors.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbu qInfragisticsWPF4.Documents.Excel.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbp lInfragisticsWPF4.OutlookBar.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbs oInfragisticsWPF4.DataPresenter.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb>igWPF(http://schemas.infragistics.com/xaml/wpf       5   	     � Title$  模组历史数据��5   	   $	��800��6   $
��1440��6%    � WindowStartupLocation$ CenterScreen=�62   .��5      �  � RowDefinitions 5   
   �� 5	      $��40 �6   �� 5
      $��* �6   .+�5   
   �� -   5         ��Name$ TB_Title��5      $ ��JC2111A0001LZ 历史记录��6&   $
��Bold"�6@   $��20(�6R   $��Center=�6`   $��Center=�6}   �� 5   
   $��1��6   $��0��6    $��5,0,5,5q�60   ��5      �� 5      .+�5      �� $��数据列表��6   $
��Bold"�6+   $��20(�6=   5      .��5      4  .Infragistics.Windows.DataPresenter.XamDataGrid  -   5          Name$ Xdg_MainGrid��5   N   $��10q�5       $��1��6-   $��5��6:   :  4Infragistics.Windows.DataPresenter.DataPresenterBase   ThemeJ  DInfragistics.Windows.Themes.Internal.DataPresenterThemeTypeConverter$ 
Office2013 5          GroupByAreaLocation$
 None=�63   $��20(�6d      ViewSettings 5      9  3Infragistics.Windows.DataPresenter.GridViewSettings  5         FieldSettings 5      6  0Infragistics.Windows.DataPresenter.FieldSettings  5      	   AllowRecordFiltering�  �Infragistics.Windows.Helpers.NullableConverter`1[[System.Boolean, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]$
	 True 6*   
   	AllowEdit$
 False 6F      Width$
 autoX�6X      AllowResize$
 True 6e      LabelTextAlignment�  �Infragistics.Windows.Helpers.NullableConverter`1[[System.Windows.TextAlignment, PresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35]]$ Center 6x      FieldLayoutSettings 5      <  6Infragistics.Windows.DataPresenter.FieldLayoutSettings  5         AutoGenerateFields$
 trueY�60      RecordSelectorLocation$ LeftOfCellArea=�6K   "   HeaderPrefixAreaDisplayMode$
 None=�6s      HeaderPlacementInGroupBy$ 	OnTopOnly=�6�      HeaderPlacement$ 	OnTopOnly=�6�      RecordSelectorNumberType$ DataItemIndex=�5      !   RecordSelectorNumberFormat$ IncludeParentRecordNumber=�6@       RecordSelectorNumberStart$ 1X�6x      FieldLayouts 5      4	  .Infragistics.Windows.DataPresenter.FieldLayout	  5        	  
FieldItems. 5!      .
  (Infragistics.Windows.DataPresenter.Field
  2  ,Infragistics.Windows.DataPresenter.FieldItem   Name$ CapacitorPackageNo��6&      Label$ 电容包编号��6@   ! 
  HorizontalContentAlignment�  �Infragistics.Windows.Helpers.NullableConverter`1[[System.Windows.HorizontalAlignment, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35]]$ Center 6N      Width$
 autoX�6r   
  5"      $ ModuleNumber��6&   $ 模组编码��6:   $ Center 6G   $
 autoX�6k   
  5#      $ 
CreateTime��6&   $ 检测日期��68   $ Center 6E   $
 autoX�6i   
  5$      $ AppearanceQuality��6&   $ 外观质量��6?   $ Center 6L   $
 autoX�6p   
  5%      $ PCBNo��6&   $ 	PCB编号��63   $ Center 6A   $
 autoX�6e   
  5&      $ SingleNo��6&   $ 单体编号��66   $ Center 6C   $
 autoX�6g   
  5'      $# InsulationInternalResistance1��6&   $ 绝缘电阻1��6K   $ Center 6Y   $
 autoX�6}   
  5(      $ InternalImpedance1��6&   $ 交流内阻1��6@   $ Center 6N   $
 autoX�6r   
  5)      $ R1Number��6&   $ 	R61阻值��66   $ Center 6D   $
 autoX�6h   
  5*      $ R2Number��6&   $ 	R62阻值��66   $ Center 6D   $
 autoX�6h   
  5+      $ StandingVoltage��6&   $ 静置电压��6=   $ Center 6J   $
 autoX�6n   
  5,      $ StandingHour��6&   $ 静置时间��6:   $ Center 6G   $
 autoX�6k   
  5-      $ MonomerVoltage��6&   $ 单体电压检测结果��6<   $ Center 6M   $
 autoX�6q   
  5.      $ Capacity��6&   $ 容量��66   $ Center 6A   $
 autoX�6e   
  5/      $ InternalResistance��6&   $ 直流内阻��6@   $ Center 6M   $
 autoX�6q   
  50      $ CapacityCompletion��6&   $ 电容包装配完成度��6@   $ Center 6Q   $
 autoX�6u   
  51      $ AssemblyCompletion��6&   $ 模组装配完成度��6@   $ Center 6P   $
 autoX�6t   
  52      $" InsulationInternalResistance��6&   $ 绝缘电阻2��6J   $ Center 6X   $
 autoX�6|   
  53      $ InternalImpedance��6&   $ 交流内阻2��6?   $ Center 6M   $
 autoX�6q   
  54      $ WithstandVoltageTest��6&   $ 耐压测试��6B   $ Center 6O   $
 autoX�6s   
  55      $ 	LableInfo��6&   $ 模组标签/档位信息��67   $ Center 6I   $
 autoX�6m   
  56      $
 Gear��6&   $ 档位��62   $ Center 6=   $
 autoX�6a   
  57      $ SCTestResult��6&   $ 三串测试结果��6:   $ Center 6I   $
 autoX�6m   
  58      $ BarCode��6&   $ 装箱条码��65   $ Center 6B   $
 autoX�6f   
  59      $ TrustBarCode��6&   $ 装托条码��6:   $ Center 6G   $
 autoX�6k   
  5:      $ CurrentPartName��6&   $ 当前工位��6=   $ Center 6J   $
 autoX�6n   
  5;      $ IsQualityText��6&   $ 是否合格��6;   $ Center 6H   $
 autoX�6l   
  5<      $ 	IsQuality��6&   $  ��67      
Visibility$ 	Collapsed=�6@   
  5=      $ ID��6&   $ ID��60   $ 	Collapsed=�6;   
  5>      $ SerialNumber��6&   $  ��6:   $ 	Collapsed=�6C   
  5?      $ 
OnlineTime��6&   $  ��68   $ 	Collapsed=�6A   
  5@      $ OfflineTime��6&   $  ��69   $ 	Collapsed=�6B   
  5A      $ CurrentPartID��6&   $  ��6;   $ 	Collapsed=�6D   
  5B      $ Times��6&   $  ��63   $ 	Collapsed=�6<   
  5C      $ ShiftID��6&   $  ��65   $ 	Collapsed=�6>   
  5D      $ CreateID��6&   $  ��66   $ 	Collapsed=�6?   
  5E      $ Creator��6&   $  ��65   $ 	Collapsed=�6>   
  5F      $ 	ProductID��6&   $  ��67   $ 	Collapsed=�6@   
  5G      $ Active��6&   $  ��64   $ 	Collapsed=�6=   
  5H      $ 
ActiveText��6&   $  ��68   $ 	Collapsed=�6A   5I      5K      5L      5M      5N      