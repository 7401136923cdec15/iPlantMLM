   M S B A M L   `   `   `  ����  	iPlantMLM$    iPlantMLM.ReportStatisticsPage   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      � �InfragisticsWPF4.Controls.Editors.XamDateTimeInput.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb� |InfragisticsWPF4.Controls.Editors.XamSlider.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbn jInfragisticsWPF4.DragDrop.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbw sInfragisticsWPF4.DataVisualization.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb�	 |InfragisticsWPF4.Controls.Menus.XamDataTree.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb�
 �InfragisticsWPF4.Controls.Editors.XamMaskedInput.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb� ~InfragisticsWPF4.Controls.Charts.XamDataChart.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbe aInfragisticsWPF4.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb� �InfragisticsWPF4.Controls.Editors.XamPropertyGrid.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbq mInfragisticsWPF4.DataManager.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb?ig$http://schemas.infragistics.com/xaml
     	 
     5      Ld2http://schemas.microsoft.com/expression/blend/2008
     	 
     5      � }InfragisticsWPF4.DataPresenter.ExcelExporter.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbm iInfragisticsWPF4.Editors.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbu qInfragisticsWPF4.Documents.Excel.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbp lInfragisticsWPF4.OutlookBar.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbs oInfragisticsWPF4.DataPresenter.v16.2, Version=16.2.20162.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb>igWPF(http://schemas.infragistics.com/xaml/wpf       5      -   5          N� Title$  报表统计��5
       N� FontSize$ 20(�6   .��5      �� .@�5   
   �� $��日产量报表��6   .��5      �  � RowDefinitions 5      �� 5      $	��100 �6$   �� 5      $��* �6$   .+�5      �� $��0��6   $��0��6(   $��5,0,5,5q�68   
���   ���5      �� 5      .+�5      �� $��查询条件��6$   $
��Bold"�63   5      .��5      ��  �� Orientation$ 
Horizontal=�6!   $
��Left=�6:   $��Center=�6U   $��2��6p   $��40��6�   $	��0,5q�6�   .+�5      �� $��日期范围：��6    $��Center=�60   $��20(�6R   $��10,0,0,0q�6`   (  "System.Windows.Controls.DatePicker  5      -   5          Name$ Dpk_StartDateModule��5   %   $��20(�6C   $	��150��6Q   $��Center=�6]   $��Center=�6�   �� 5      $��--��6    $��Center=�6-     5      -   5        $ Dpk_EndDateModule��5   %   $��20(�6A   $	��150��6P   $��Center=�6\   $��Center=�6~   �� 5       -   5         ��Name$ Btn_QueryChart��5    !   $	��5,0q�69   ���	#FF00B9FF.��5!      �� $ 
Horizontal=�6)   .+�5"   "   �� $��/Images\查询.png��6(   $��30��6@   $��6,0,0,0q�6L   �� 5#   "   $��20(�6(   $	��6,0q�66   $��Center=�6C   $��查询��6e   5$      5%      5&      5'      �� 5)      $��1��6   $��0��6(   $��5,0,5,5q�68   
���   �$��12(�6]   ��5*      �� 5+      .+�5,      �� $��查询结果��6$   $
��Bold"�63   $��20(�6E   5-      .��5/      � $��20q�6   $��1��6'   .+�50      /  )Infragistics.Controls.Charts.XamDataChart  -   5          Name$ XDC_ColumnarDisplay��50   7   $��1��6*   $��40q�6T   )  #Infragistics.Controls.Charts.Legend  51      -   5          Name$ Legend_ColumnarDisplay��51   1   $��0��6$   $��Right=�6Q      Orientation$ 
Horizontal=�6m   $��0,-20,-10,0q�6�   52      53      54      55      �� 56   
   $��模组检测数据查询��6   .��57      �  58      �� 59      $	��150 �6$   �� 5:      $��* �6$   .+�5=      �� $��0��6   $��0��6(   $��5,0,5,5q�68   
���   ���5>      �� 5?      .+�5@      �� $��查询条件��6$   $
��Bold"�63   5A      .��5C      � 	 � Orientation$	 
Horizontal=�6    $
��Left=�69   $��Center=�6T   $��2��6o   $	��0,5q�6�   .+�5D      �� $ 
Horizontal=�6%   $��35��6>   $��0,5,0,0q�6J   .+�5E      �� $��检测日期：��6$   $��Center=�64   $��20(�6V   $��10,0,0,0q�6d     5F      -   5        $ Dpk_StartDateModule1��5F   )   $��20(�6H   $	��150��6V   $��Center=�6b   $��Center=�6�   $��35��6�   �� 5G      $��--��6$   $��Center=�61     5H      -   5        $ Dpk_EndDateModule1��5H   )   $��20(�6F   $	��150��6U   $��Center=�6a   $��Center=�6�   $��35��6�   5I      �� 5K      $ 
Horizontal=�6%   $��35��6>   $��0,5,0,0q�6J   .+�5L      �� $%��电容包编号/模组编码：��6$   $��Center=�6:   $��20(�6\   $��10,0,0,0q�6j   �� 5M      -	   5        
 ��Name$
 
Txt_Module��5M   &   $	��200��6:   $��Center=�6F   $��20(�6h   5N      �� 5P      $ 
Horizontal=�6%   $��35��6>   $��0,5,0,0q�6J   .+�5Q      �� $��装箱条码：��6$   $��Center=�64   $��20(�6V   $��10,0,0,0q�6d   �� 5R      -
   5        $
 
Txt_ZXCode��5R   &   $	��200��6:   $��Center=�6F   $��20(�6h   5S      �� 5U      $ 
Horizontal=�6%   $��0,5,0,0q�6>   .+�5V      �� $��装托条码：��6$   $��Center=�64   $��20(�6V   $��10,0,0,0q�6d   �� 5W      -   5        $
 
Txt_ZTCode��5W   &   $	��200��6:   $��Center=�6F   $��20(�6h   5X      �� 5Z      $ 
Horizontal=�6%   $��0,5,0,0q�6>   .+�5[      �� $��	档位：��6$   $��Center=�62   $��20(�6T   $��10,0,0,0q�6b   �� 5\      -   5        $
 Txt_Gear��5\   &   $	��100��68   $��Center=�6D   $��20(�6f   5]      �� 5_      -   5        $ Btn_QueryByModule1��5_   !   $��10,5,0,0q�6=   ���	#FF00B9FF.��5`      �� $ 
Horizontal=�6)   .+�5a   "   �� $��/Images\查询.png��6(   $��30��6@   $��6,0,0,0q�6L   �� 5b   "   $��20(�6(   $	��6,0q�66   $��Center=�6C   $��查询��6e   5c      5d      �� 5f      -   5        $��
Btn_Export��5f   M   $	��100��6!   $��15,5,0,0q�6-   $��15(�6?   ���	#FF00B9FF.��5g      �� $ 
Horizontal=�6)   .+�5h   "   �� $��/Images\保存_黑.png��6(   $��25��6B   �� 5i   "   .��6(   导出6,   5j      5k      �� 5m      -   5        $��Btn_SeeSingle��5m   M   $	��140��6!   $��15,5,0,0q�6-   $��15(�6?   ���	#FF00B9FF.��5n      �� $ 
Horizontal=�6)   .+�5o   "   �� $��/Images\查询.png��6(   $��25��6@   �� 5p   "   .��6(   查看单体信息60   5q      5r      �� 5t      -   5        $��Btn_CapacityExport��5t   M   $	��140��6!   $��15,5,0,0q�6-   $��15(�6?   ���	#FF00B9FF.��5u      �� $ 
Horizontal=�6)   .+�5v   "   �� $��/Images\保存_黑.png��6(   $��25��6B   �� 5w   "   .��6(   容量内阻导出60   5x      5y      �� 5{      -   5        $��Btn_ClearData��5{   M   $	��160��6!   $��15,5,0,0q�6-   $��15(�6?   ���	#FF00B9FF.��5|      �� $ 
Horizontal=�6)   .+�5}   "   �� $��/Images\清空.png��6(   $��25��6@   �� 5~   "   .��6(   删除当前工位数据62   5      5�      5�      5�      �� 5�      $��1��6   $��0��6(   $��5,0,5,5q�68   
���   ���5�      �� 5�      .+�5�      �� $��查询结果��6$   $
��Bold"�63   5�      .��5�      4  .Infragistics.Windows.DataPresenter.XamDataGrid  -   5          Name$ Xdg_MainGrid��5�   N   $��10q�5�   (   $��1��65   $��5��6B   :  4Infragistics.Windows.DataPresenter.DataPresenterBase   ThemeJ  DInfragistics.Windows.Themes.Internal.DataPresenterThemeTypeConverter$ 
Office2013 5�          GroupByAreaLocation$
 None=�63   $��20(�6�      ViewSettings 5�      9  3Infragistics.Windows.DataPresenter.GridViewSettings  5�         FieldSettings 5�      6  0Infragistics.Windows.DataPresenter.FieldSettings  5�         AllowRecordFiltering�	  �Infragistics.Windows.Helpers.NullableConverter`1[[System.Boolean, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]$
 True	 62      	AllowEdit$ False	 6N      Width$
 autoX�6`      AllowResize$
 True	 6m      LabelTextAlignment�
  �Infragistics.Windows.Helpers.NullableConverter`1[[System.Windows.TextAlignment, PresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35]]$ Center
 6�      FieldLayoutSettings 5�      <  6Infragistics.Windows.DataPresenter.FieldLayoutSettings  5�         AutoGenerateFields$
 trueY�68      RecordSelectorLocation$ LeftOfCellArea=�6S   "   HeaderPrefixAreaDisplayMode$
 None=�6{      HeaderPlacementInGroupBy$ 	OnTopOnly=�6�      HeaderPlacement$ 	OnTopOnly=�6�      RecordSelectorNumberType$ DataItemIndex=�5�      !   RecordSelectorNumberFormat$ IncludeParentRecordNumber=�6@       RecordSelectorNumberStart$ 1X�6x      FieldLayouts 5�      4  .Infragistics.Windows.DataPresenter.FieldLayout  5�         
FieldItems. 5�   "   .  (Infragistics.Windows.DataPresenter.Field  2  ,Infragistics.Windows.DataPresenter.FieldItem    Name$  CapacitorPackageNo��6.   !   Label$! 电容包编号��6H   !"   HorizontalContentAlignment�  �Infragistics.Windows.Helpers.NullableConverter`1[[System.Windows.HorizontalAlignment, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35]]$" Center 6V   #   Width$
# autoX�6z     5�   "   $  ModuleNumber��6.   $! 模组编码��6B   $" Center 6O   $
# autoX�6s     5�   "   $  
CreateTime��6.   $! 检测日期��6@   $" Center 6M   $
# autoX�6q     5�   "   $  AppearanceQuality��6.   $! 外观质量��6G   $" Center 6T   $
# autoX�6x     5�   "   $  PCBNo��6.   $! 	PCB编号��6;   $" Center 6I   $
# autoX�6m     5�   "   $  SingleNo��6.   $! 单体编号��6>   $" Center 6K   $
# autoX�6o     5�   "   $#  InsulationInternalResistance1��6.   $! 绝缘电阻1��6S   $" Center 6a   $
# autoX�6�     5�   "   $  InternalImpedance1��6.   $! 交流内阻1��6H   $" Center 6V   $
# autoX�6z     5�   "   $  R1Number��6.   $! 	R61阻值��6>   $" Center 6L   $
# autoX�6p     5�   "   $  R2Number��6.   $! 	R62阻值��6>   $" Center 6L   $
# autoX�6p     5�   "   $  StandingVoltage��6.   $! 静置电压��6E   $" Center 6R   $
# autoX�6v     5�   "   $  StandingHour��6.   $! 静置时间��6B   $" Center 6O   $
# autoX�6s     5�   "   $  MonomerVoltage��6.   $! 单体电压检测结果��6D   $" Center 6U   $
# autoX�6y     5�   "   $  Capacity��6.   $! 容量��6>   $" Center 6I   $
# autoX�6m     5�   "   $  InternalResistance��6.   $! 直流内阻��6H   $" Center 6U   $
# autoX�6y     5�   "   $  CapacityCompletion��6.   $! 电容包装配完成度��6H   $" Center 6Y   $
# autoX�6}     5�   "   $  AssemblyCompletion��6.   $! 模组装配完成度��6H   $" Center 6X   $
# autoX�6|     5�   "   $"  InsulationInternalResistance��6.   $! 绝缘电阻2��6R   $" Center 6`   $
# autoX�6�     5�   "   $  InternalImpedance��6.   $! 交流内阻2��6G   $" Center 6U   $
# autoX�6y     5�   "   $  WithstandVoltageTest��6.   $! 耐压测试��6J   $" Center 6W   $
# autoX�6{     5�   "   $  	LableInfo��6.   $! 模组标签/档位信息��6?   $" Center 6Q   $
# autoX�6u     5�   "   $
  Gear��6.   $! 档位��6:   $" Center 6E   $
# autoX�6i     5�   "   $  SCTestResult��6.   $! 三串测试结果��6B   $" Center 6Q   $
# autoX�6u     5�   "   $  BarCode��6.   $! 装箱条码��6=   $" Center 6J   $
# autoX�6n     5�   "   $  TrustBarCode��6.   $! 装托条码��6B   $" Center 6O   $
# autoX�6s     5�   "   $  CurrentPartName��6.   $! 当前工位��6E   $" Center 6R   $
# autoX�6v     5�   "   $  IsQualityText��6.   $! 是否合格��6C   $" Center 6P   $
# autoX�6t     5�   "   $  	IsQuality��6.   $!  ��6?   $   
Visibility$$ 	Collapsed=�6H     5�   "   $  ID��6.   $! ID��68   $$ 	Collapsed=�6C     5�   "   $  SerialNumber��6.   $!  ��6B   $$ 	Collapsed=�6K     5�   "   $  
OnlineTime��6.   $!  ��6@   $$ 	Collapsed=�6I     5�   "   $  OfflineTime��6.   $!  ��6A   $$ 	Collapsed=�6J     5�   "   $  CurrentPartID��6.   $!  ��6C   $$ 	Collapsed=�6L     5�   "   $  Times��6.   $!  ��6;   $$ 	Collapsed=�6D     5�   "   $  ShiftID��6.   $!  ��6=   $$ 	Collapsed=�6F     5�   "   $  CreateID��6.   $!  ��6>   $$ 	Collapsed=�6G     5�   "   $  Creator��6.   $!  ��6=   $$ 	Collapsed=�6F     5�   "   $  	ProductID��6.   $!  ��6?   $$ 	Collapsed=�6H     5�   "   $  Active��6.   $!  ��6<   $$ 	Collapsed=�6E     5�   "   $  
ActiveText��6.   $!  ��6@   $$ 	Collapsed=�6I   5�      5�      5�      5�      5�      �� 5�   
   $��不合格数据管理��6   .��5�      �  5�      �� 5�      $	��150 �6$   �� 5�      $��* �6$   .+�5�      �� $��0��6   $��0��6(   $��5,0,5,5q�68   
���   ���5�      �� 5�      .+�5�      �� $��查询条件��6$   $
��Bold"�63   5�      .��5�      � $	 
Horizontal=�6    $
��Left=�69   $��Center=�6T   $��2��6o   $	��0,5q�6�   .+�5�      �� $��检测日期：��6    $��Center=�60   $��20(�6R   $��10,0,0,0q�6`     5�      -   5        $ Dpk_StartDateModule2��5�   %   $��20(�6D   $	��150��6R   $��Center=�6^   $��Center=�6�   �� 5�      $��--��6    $��Center=�6-     5�      -   5        $ Dpk_EndDateModule2��5�   %   $��20(�6B   $	��150��6Q   $��Center=�6]   $��Center=�6   �� 5�      $%��电容包编号/模组编码：��6    $��Center=�66   $��20(�6X   $��10,0,0,0q�6f   �� 5�      -   5        $
 Txt_Module1��5�   "   $	��200��67   $��Center=�6C   $��20(�6e   �� 5�      -   5        $ Btn_QueryByModule2��5�   !   $	��5,0q�6=   ���	#FF00B9FF.��5�      �� $ 
Horizontal=�6)   .+�5�   "   �� $��/Images\查询.png��6(   $��30��6@   $��6,0,0,0q�6L   �� 5�   "   $��20(�6(   $	��6,0q�66   $��Center=�6C   $��查询��6e   5�      5�      �� 5�      -   5        $��Btn_SeeSingleBad��5�   M   $	��140��6!   $��10,0,0,0q�6-   $��15(�6?   ���	#FF00B9FF.��5�      �� $ 
Horizontal=�6)   .+�5�   "   �� $��/Images\查询.png��6(   $��25��6@   �� 5�   "   .��6(   查看单体信息60   5�      5�      �� 5�      -   5        $��Btn_Sign��5�   M   $	��100��6!   $��15,0,0,0q�6-   $��15(�6?   ���	#FF00B9FF.��5�      �� $ 
Horizontal=�6)   .+�5�   "   �� $��/Images\标签.png��6(   $��25��6@   �� 5�   "   .��6(   标定6,   5�      5�      �� 5�      -   5        $��Btn_Export1��5�   M   $	��100��6!   $��15,0,0,0q�6-   $��15(�6?   ���	#FF00B9FF.��5�      �� $ 
Horizontal=�6)   .+�5�   "   �� $��/Images\保存_黑.png��6(   $��25��6B   �� 5�   "   .��6(   导出6,   5�      5�      �� 5�      -   5        $��Btn_ClearData_Bad��5�   M   $	��160��6!   $��15,5,0,0q�6-   $��15(�6?   ���	#FF00B9FF.��5�      �� $ 
Horizontal=�6)   .+�5�   "   �� $��/Images\清空.png��6(   $��25��6@   �� 5�   "   .��6(   删除当前工位数据62   5�      5�      5�      5�      �� 5     $��1��6   $��0��6(   $��5,0,5,5q�68   
���   ���5     �� 5     .+�5     �� $��查询结果��6$   $
��Bold"�63   5     .��5       -   5        $ Xdg_MainGrid1��5  N   $��10q�5  (   $��1��65   $��5��6B   $ 
Office2013 5      $
 None=�63   $��20(�6�    5	       5
      5       5     $
 True	 62   $ False	 6N   $
 autoX�6`   $
 True	 6m   $ Center
 6�    5       5     $
 trueY�68   $ LeftOfCellArea=�6S   $
 None=�6{   $ 	OnTopOnly=�6�   $ 	OnTopOnly=�6�   $ DataItemIndex=�5     $ IncludeParentRecordNumber=�6@   $ 1X�6x    5       5     . 5  "     $  CapacitorPackageNo��6.   $! 电容包编号��6H   $" Center 6V   $
# autoX�6z     5  "   $  ModuleNumber��6.   $! 模组编码��6B   $" Center 6O   $
# autoX�6s     5  "   $  
CreateTime��6.   $! 检测日期��6@   $" Center 6M   $
# autoX�6q     5  "   $  AppearanceQuality��6.   $! 外观质量��6G   $" Center 6T   $
# autoX�6x     5  "   $  PCBNo��6.   $! 	PCB编号��6;   $" Center 6I   $
# autoX�6m     5  "   $  SingleNo��6.   $! 单体编号��6>   $" Center 6K   $
# autoX�6o     5  "   $#  InsulationInternalResistance1��6.   $! 绝缘电阻1��6S   $" Center 6a   $
# autoX�6�     5  "   $  InternalImpedance1��6.   $! 交流内阻1��6H   $" Center 6V   $
# autoX�6z     5  "   $  R1Number��6.   $! 	R61阻值��6>   $" Center 6L   $
# autoX�6p     5  "   $  R2Number��6.   $! 	R62阻值��6>   $" Center 6L   $
# autoX�6p     5  "   $  StandingVoltage��6.   $! 静置电压��6E   $" Center 6R   $
# autoX�6v     5   "   $  StandingHour��6.   $! 静置时间��6B   $" Center 6O   $
# autoX�6s     5!  "   $  MonomerVoltage��6.   $! 单体电压检测结果��6D   $" Center 6U   $
# autoX�6y     5"  "   $  Capacity��6.   $! 容量��6>   $" Center 6I   $
# autoX�6m     5#  "   $  InternalResistance��6.   $! 直流内阻��6H   $" Center 6U   $
# autoX�6y     5$  "   $  CapacityCompletion��6.   $! 电容包装配完成度��6H   $" Center 6Y   $
# autoX�6}     5%  "   $  AssemblyCompletion��6.   $! 模组装配完成度��6H   $" Center 6X   $
# autoX�6|     5&  "   $"  InsulationInternalResistance��6.   $! 绝缘电阻2��6R   $" Center 6`   $
# autoX�6�     5'  "   $  InternalImpedance��6.   $! 交流内阻2��6G   $" Center 6U   $
# autoX�6y     5(  "   $  WithstandVoltageTest��6.   $! 耐压测试��6J   $" Center 6W   $
# autoX�6{     5)  "   $  	LableInfo��6.   $! 模组标签/档位信息��6?   $" Center 6Q   $
# autoX�6u     5*  "   $
  Gear��6.   $! 档位��6:   $" Center 6E   $
# autoX�6i     5+  "   $  SCTestResult��6.   $! 三串测试结果��6B   $" Center 6Q   $
# autoX�6u     5,  "   $  BarCode��6.   $! 装箱条码��6=   $" Center 6J   $
# autoX�6n     5-  "   $  TrustBarCode��6.   $! 装托条码��6B   $" Center 6O   $
# autoX�6s     5.  "   $  CurrentPartName��6.   $! 当前工位��6E   $" Center 6R   $
# autoX�6v     5/  "   $  IsQualityText��6.   $! 是否合格��6C   $" Center 6P   $
# autoX�6t     50  "   $  	IsQuality��6.   $!  ��6?   $$ 	Collapsed=�6H     51  "   $  ID��6.   $! ID��68   $$ 	Collapsed=�6C     52  "   $  SerialNumber��6.   $!  ��6B   $$ 	Collapsed=�6K     53  "   $  
OnlineTime��6.   $!  ��6@   $$ 	Collapsed=�6I     54  "   $  OfflineTime��6.   $!  ��6A   $$ 	Collapsed=�6J     55  "   $  CurrentPartID��6.   $!  ��6C   $$ 	Collapsed=�6L     56  "   $  Times��6.   $!  ��6;   $$ 	Collapsed=�6D     57  "   $  ShiftID��6.   $!  ��6=   $$ 	Collapsed=�6F     58  "   $  CreateID��6.   $!  ��6>   $$ 	Collapsed=�6G     59  "   $  Creator��6.   $!  ��6=   $$ 	Collapsed=�6F     5:  "   $  	ProductID��6.   $!  ��6?   $$ 	Collapsed=�6H     5;  "   $  Active��6.   $!  ��6<   $$ 	Collapsed=�6E     5<  "   $  
ActiveText��6.   $! 激活状态��6@   $" Center 6M   $
# autoX�6q   5=     5?     5@     5A     5B     5C     5D     