   M S B A M L   `   `   `  ����G  CFericaReader, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:FelicaLib	FelicaLib  5   	       FericaReader.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:FelicaLib     5   	   R NOxyPlot.Wpf, Version=2.0.0.0, Culture=neutral, PublicKeyToken=75e952ba404cdbb0 oxyhttp://oxyplot.org/wpf  5   	     � Title$  
MainWindow��5
   	   $	��500��6   $	��800��6)   .��5      � .+�5   
   �� -   5         ��Name$ TabControlViews��5      .@�5      �� $��CardRead��6   .��5      � ���	#FFE5E5E5 � ColumnDefinitions 5      �� 5      $
��580* �6+   �� 5      $
��206* �6+   .+�5      �� -   5         ��Name$ ButtonReadFerica��5      $��Ferica読込��67   $��106,405,3,5q�6J   $��1��6~   �� 5      -   5         ��Name$ LabelResult��5      $��読込結果��61   $��12,405,105,5q�6@   
���   �
�������$��0q�6|   $��Center=�6�   $��Center=�6�   $��1��6�   �� 5      -   5         ��Name$ TextBoxCalcValue��5      $
��Left=�68   $	��160��6S   $��8,10,0,0q�6`    �� TextWrapping$
 Wrap=�6r   $	��Top=�6�   $	��186��6�   $ �� HorizontalScrollBarVisibility$
 Auto=�6�   $��1��6�   � 5      $��1��6   $��0��6/   .+�5      &   System.Windows.Controls.DataGrid  -   5          Name$ DataGridCardResult��5   #   $��5q�6?   5      5      5      �� 5      -   5        	 ��Name$	 TabDataView��5      $��DataView��6+   .��5       � ���	#FFE5E5E5 5!      �� 5"      $
��580* �6+   �� 5#      $
��206* �6+   
 � RowDefinitions
 5%      �� 5&      $
��251* �6(   �� 5'      $
��190* �6(   .+�5)      � $��0��6   $��0��6(   $��1��68   $��1��6I   .+�5*        -   5        $ DataGridDBResult��5*   #   $��5q�6=   $	��241��6H   5+      �� 5,      -   5        $ ButtonWeekly��5,      $��週集計(2019.11)��63   $
��Left=�6J   $��25��6e   $��
10,160,0,0q�6q   $	��Top=�6�   $��92��6�   $��1��6�   $��1��6�   �� 5-      -	   5        $ ButtonMakeChart��5-      $��Graph作成��66   $
��Left=�6H   $��25��6c   $��126,160,0,0q�6o   $	��Top=�6�   $��70��6�   $��1��6�   $��1��6�   � 5.      $��1��6   $��1��6,   $��1��69   $��0��6M   $��5q�6^   .+�5/        OxyPlot.Wpf.PlotView   � DataContext 6'   �� *	graphView+   Model 6I   �� *Model+   
Controller 6a   �� *
Controller+50      &   System.Windows.Controls.Calendar  52      -
   5          Name$ CalendarDaysSelect��52      $��	0,23,6,63q�6;   $	��200��6N   $	��165��6Z   $��1��6g   �� 53      -   5        $ ButtonDBViewMonth��53      $��履歴表示��68   $
��Left=�6G   $��29��6b   $��	9,126,0,0q�6n   $	��Top=�6�   $��93��6�   $��1��6�   $��1��6�   �� 54      -   5         ��Name$ ComboSelectProcess��54      $
��Left=�6;   $��
105,10,0,0q�6V   $	��Top=�6j   $��80��6�   $��1��6�   $��1��6�   �� 55      -   5        $ ButtonResultPeriod��55      $��	週集計��69   $��1��6G   $
��Left=�6W   $��22��6r   $��	105,1,0,0q�6~   $	��Top=�6�   $��80��6�   6�   58      59      5;      5<      5I      