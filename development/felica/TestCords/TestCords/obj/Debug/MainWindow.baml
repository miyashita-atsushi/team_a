   M S B A M L   `   `   `  ����D  @TestCords, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:TestCords	TestCords  5   	       TestCords.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:TestCords     5   	     � Title$  
MainWindow��5   	   $	��450��6   $	��800��6)   .��5	      � .+�5
   
   �� -   5         ��Name$ Button1��5
      $��DB作成確認��6"   $
��Left=�63   $��	34,66,0,0q�6N   $	��Top=�6a   $��74��6y   6�   �� 5   
   -   5        $ Button2��5      $��DB表示��6"   $
��Left=�61   $��
113,66,0,0q�6L   $	��Top=�6`   $��74��6x   6�   �� 5   
   -   5        $ Button3��5      $��DB書き込み��6"   $
��Left=�63   $��
575,66,0,0q�6N   $	��Top=�6b   $��74��6z   6�   &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ DataGridTest��5         
IsReadOnly . $
��Left=�6a   $	��267��6|   $��
34,119,0,0q�6�   $	��Top=�6�   $	��671��6�   ��6)   ��  �� Mode$ OneWay=�   Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ 日時��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$	 150 6L   1  +System.Windows.Controls.DataGridBoundColumn   Binding 61   �� *date+6L     5      $ 費目��6%   $	 100 6L    61   �� *type+6L     5      $ 金額��6%   $	 100 6M    61   �� *money+6M     5      $ 乗車��6%   $	 100 6h    61   �� *getonStation+	 �� TargetNullValue$	  ��6h     5      $ 降車��6%   $	 100 6U    61   �� *getoffStation+6U   5      �� 5   
   -   5        
 ��Name$
 TextBoxType��5      $
��Left=�6'   $��23��6B   $��
295,63,0,0q�6N    �� TextWrapping$
 Wrap=�6b   $	��Top=�6v   $��65��6�   �� 5   
   -   5        $
 TextBoxMoney��5      $
��Left=�6(   $��23��6C   $��
365,63,0,0q�6O   $
 Wrap=�6c   $	��Top=�6w   $��65��6�   �� 5   
   $��費目��6   $
��Left=�6   $��
295,37,0,0q�68   $	��Top=�6L   �� 5   
   $��金額��6   $
��Left=�6   $��
365,37,0,0q�68   $	��Top=�6L   �� 5   
   -   5        $
 TextBoxGetonStation��5      $
��Left=�6/   $��23��6J   $��
435,63,0,0q�6V   $
 Wrap=�6j   $	��Top=�6~   $��65��6�   �� 5   
   $��乗車��6   $
��Left=�6   $��
435,37,0,0q�68   $	��Top=�6L   �� 5   
   -   5        $
 TextBoxGetoffStation��5      $
��Left=�60   $��23��6K   $��
505,63,0,0q�6W   $
 Wrap=�6k   $	��Top=�6   $��65��6�   �� 5   
   $��降車��6   $
��Left=�6   $��
505,37,0,0q�68   $	��Top=�6L   5      5       