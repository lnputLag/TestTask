   M S B A M L   `   `   `  ����C  ?TestTask, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:TestTaskTestTask  5   	       TestTask.ClientEditWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:TestTask     5   	   b ^MaterialDesignThemes.Wpf, Version=3.2.0.1979, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	     � Title$   ��5	   	   $	��400��6   $	��400��6    � WindowStartupLocation$ CenterScreen=�6+    � 
ResizeMode$ NoResize=�6P      MaterialDesignBody#���   5
   	   .��5      � $��16q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� -   5         ��Name$ NameTextBox��5      $��0��6'   )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ Наименование��5       % !MaterialDesignFloatingHintTextBox#��[ 5      $��0,0,0,8q�5      �� 5   
   -   5        $ 
InnTextBox��5      $��1��6&   $ ИНН��5      #��[ 5      $��0,0,0,8q�5       �� 5#   
   -   5         ��Name$ ActivitySphereComboBox��5#      $��2��63   $) #Сфера деятельности��5$       & "MaterialDesignFloatingHintComboBox#��[ 5%      $��0,0,0,8q�5&      ��5'      �� 5(      .R�5)      �� #���   5*      ��5)       �� *name+5*      5+      5-      �� 50   
   -   5        $ NotesTextBox��50      $��3��6(   $ Примечание��51      #��[ 52       �� TextWrapping$
 Wrap=�53       �� AcceptsReturn . "	 �� VerticalScrollBarVisibility$
	 Auto=�55      $	��100��56      $��0,0,0,16q�57      �� 5:   
   $��6��6   
 �� Orientation$
 
Horizontal=�6"   $��Right=�6;   .+�5;      �� -   5        $��ОТМЕНА��5;         MaterialDesignOutlinedButton#��[ 5<      $��0,0,10,0q�5=      6'   �� 5>      -   5        $��СОХРАНИТЬ��5>        MaterialDesignRaisedButton#��[ 5?        PrimaryHueMidBrush#���  5@      5A      5B      5C      5D      