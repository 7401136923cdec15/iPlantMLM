   M S B A M L   `   `   `  ����E  AMoonPdfLib, Version=0.2.3.0, Culture=neutral, PublicKeyToken=null;,clr-namespace:MoonPdfLib;assembly=MoonPdfLib
MoonPdfLib  5   	    	iPlantMLM!   iPlantMLM.SOPFileLookWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   <mpp,clr-namespace:MoonPdfLib;assembly=MoonPdfLib     5   	   -   5          � Title$  文件查看��5   	   $	��800��6   $
��1440��6#    � WindowStartupLocation$ CenterScreen=�60   .��5      �  � RowDefinitions 5   
   �� 5      $��40 �6   �� 5	      $��* �6   .+�5   
   �� -   5         ��Name$ TB_Title��5   |   $��作业指导文件查看��6   $��25(�6$   $
��Bold"�62   $��Center=�6D   $��Center=�6a   6|   �� 5   
   -   5         ��Name$ GB_Main��5   U   $��1��6   $��0��6    $��5,0,5,5q�60   
���   ���5      �� 5      .+�5      �� -   5         ��Name$ Lable_Title��5   K   $��文件标题��6   $
��Bold"�6+   $��20(�6=   6K   5      .��5      � .+�5         MoonPdfLib.MoonPdfPanel  -   5          Name$ moonPdfPanel��5   #   
�������   ViewType$ 
SinglePage=�6P      PageRowDisplay$ ContinuousPageRows=�6f   	   
PageMargin$	 0,2,4,2q�6�   
 J� 	AllowDrop
 . 6�   �� 5      -   5         ��Name$ SV_DateList��5      $y�	Collapsed=�64   $
��Auto=�6K   $
��Auto=�6p    �� PanningMode$
 Both=�6�   $��10q�6�   5      5      5      5      5      