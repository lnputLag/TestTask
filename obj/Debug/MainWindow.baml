   M S B A M L   `   `   `  ����C  ?TestTask, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:TestTaskTestTask  5   	       TestTask.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:TestTask     5   	   b ^MaterialDesignThemes.Wpf, Version=3.2.0.1979, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   -   5          � Title$  
MainWindow��5	   	   $	��600��6   $
��1000��6)    � WindowStartupLocation$ CenterScreen=�6T   .��5
      � .+�5   
   �� .@�5      �� $��Клиенты��6   .��5      �  � RowDefinitions 5      �� 5      $
��Auto �6(   �� 5      $��* �6(   �� 5      $	��150 �6(   .+�5      �� $��0��6!    �� Orientation$ 
Horizontal=�6.   $��10q�6G   .+�5      �� -   5        $��Добавить��5   !   $	��100��64   $��0,0,10,0q�6@   6R   �� 5      -   5        $��Изменить��5   !   $	��100��64   $��0,0,10,0q�6@   6R   �� 5      -   5        $��Удалить��5   !   $	��100��63    � Cursor$
 Hand��6?   6M   5      � 5      $��1��6    � ColumnDefinitions 5      �� 5      $��2* �6/   �� 5      $��* �6/   .+�5#      &   System.Windows.Controls.DataGrid  -   5          Name$ ClientsGrid��5#   #   $��0��68   $��5q�6H      AutoGenerateColumns .     
IsReadOnly . 	   Columns	 5&      0  *System.Windows.Controls.DataGridTextColumn  5'   "   ,  &System.Windows.Controls.DataGridColumn
   Header$
 Наименование��65      Width5  /System.Windows.Controls.DataGridLengthConverter$ * 6d   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6K   �� *Name+6d     5(   "   $
 ИНН��65   $	 100 6Z    6B   �� *Inn+6Z     5)   "   $)
 #Сфера деятельности��65   $ * 6x    6Q   �� *ActivitySphereName+6x     5*   "   $'
 !Количество заявок��65   $ 80 6q    6P   �� *RequestCount+6q     5+   "   $.
 (Дата последней заявки��65   $	 120 6x    6T   �� *LastRequestDate+6x   5-        50      -   5        $ ClientRequestsGrid��50   #   $��1��6?   $��5q�6O    .   . 	 52        53   "   $
 Дата заявки��65   $	 100 6j    6J   �� *RequestDate+6j     54   "   $)
 #Наименование работ��65   $ * 6n    6Q   �� *
WorkName+6n     55   "   $!
 Описание работ��65   $ * 6q    6M   �� *WorkDescription+6q     56   "   $
 Статус��65   $ 80 6d    6E   �� *
StatusName+6d   58      59      �� 5<      $��2��6   
�������$��0,1,0,0q�6=   $��5q�6W   ._�5=      �  5>      �� 5?   "   $
��Auto �60   �� 5@   "   $��* �60   .+�5B      �� $��Примечание��6(   $
��Bold"�6:   $��5q�6L   �� 5C      -   5         ��Name$ ClientNotesTextBox��5C   &   $��1��6B   $��5q�6O    �� TextWrapping$
 Wrap=�5D   &    �� AcceptsReturn . " �� VerticalScrollBarVisibility$
 Auto=�6O    �� 
IsReadOnly . ��5F   &   �� *SelectedClient.Notes+ �� UpdateSourceTrigger$ PropertyChanged=�5G      5H      5I      5J      �� 5L      $��Заявки��6   .��5M      � $��5q�6    5N      �� 5O      $
��Auto �6(   �� 5P      $��* �6(   .+�5T      �� $��0��6!   $ 
Horizontal=�6.   $��0,0,0,10q�6G   $  ��6Y   .+�5U      �� -   5        $��Добавить��5U   !   $	��100��64   $��0,0,10,0q�6@   6R   �� 5V      -	   5        $��Удалить��5V   !   $	��100��63   $��0,0,10,0q�6?   6Q   �� 5W      $'��!Фильтр по клиенту:��6$   $��Center=�6>   $��0,0,10,0q�6Y   �� 5X      -
   5         ��Name$ ClientFilterComboBox��5X   #   $	��200��6A    �� DisplayMemberPath$
 Name��6M   5Y   #   �� 5Z      -   5        $#��Сбросить фильтр��5Z   !   $��10,0,0,0q�6;   5[   !   5\        5_      -   5        $ AllRequestsGrid��5_      $��1��68    .   .     CanUserAddRows .     CanUserDeleteRows .  	 5c        5d      $
 Клиент��61   $ * 6`      
IsReadOnly .  6A   �� *
ClientName+6j     5e      $
 Дата��61   $	 100 6_    .  6?   �� *RequestDate+6k     5f      $)
 #Наименование работ��61   $ * 6j    .  6M   �� *
WorkName+6t     5g      $
 Описание��61   $ * 6g    .  6C   �� *WorkDescription+6q   4  .System.Windows.Controls.DataGridComboBoxColumn  5h      $
 Статус��65      DisplayMemberPath$ 
StatusName��5j   3      SelectedValuePath$ Id��5k   3      SelectedValueBinding 5i   3   �� *
StatusId+5k   3   5m      5n      5o      5p      5q      5r      