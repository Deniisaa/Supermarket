   M S B A M L   `   `   `  ����D  @Tema3MVVM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:Tema3MVVM.ViewModelsTema3MVVM.ViewModels  5   	       Tema3MVVM.CasierWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   4local"clr-namespace:Tema3MVVM.ViewModels     5   	     � Title$  Casier��5   	   $	��680��6   $	��800��6%   .��5      � ��5   
   �� 5       �� 
StartPoint$	 0,0&�6"    �� EndPoint$	 1,1&�63   .��5	      �  � Color$ #A48CE9��6    � Offset$ 0Q�6/   � 5
      $ #A48CE9��6   $ 1Q�6/   5      .+�5   
   � $��52,10,10,31q�6    � RowDefinitions 5      �� 5      $
��Auto �6    �� 5      �� 5      $
��Auto �6    $��54.32��6.   �� 5      $
��Auto �6    �� 5      $
��Auto �6    .+�5      � $��0��6   $��0,0,0,16q�6     � ColumnDefinitions 5      �� 5      $
��Auto �6'   �� 5      $��* �6'   .+�5      �� -   5         ��Name$ SearchTextBox��5      $��0��61   $��30��6B   $	��160��6N   $��
0 -150 0 0q�6Z   ��6n   �� *
SearchText+ �� UpdateSourceTrigger$ PropertyChanged=��� 5"      $��1��6   	 �� Orientation$	 Vertical=�6-   $��5,10,-5,-10q�6D   .+�5#      �� $��
SearchNume��6   $	��0 5q�6Z   ��62   �� *SearchByNameCommand+6Z   �� 5$      $��SearchCodDeBare��6   $	��0 5q�6b   ��67   �� *SearchByBarcodeCommand+6b   �� 5%      $��SearchDataExpirare��6   $	��0 5q�6h   ��6:   �� *SearchByExpirationCommand+6h   �� 5&      $��SearchProducator��6   $	��0 5q�6d   ��68   �� *SearchByProducerCommand+6d   �� 5'      $��SearchCategorie��6   $	��0 5q�6c   ��67   �� *SearchByCategoryCommand+6c   �� 5(      $��
ReloadData��6   $	��0 5q�6]   ��62   �� *ReloadTableDataCommand+6]   �� 5)      $��Add to Receipt��6   $��30��67   $	��160��6C   $��-790 10 1 0q�6x   ��6P   �� *AddToReceiptCommand+6x   5*      5+      &   System.Windows.Controls.DataGrid  5/      -   5        $��1��5/      
   AutoGenerateColumns
 . $	��150��6�   $	��Top=�6�   ��6$   �� *	TableData+ �� SelectedValue 6F   �� *SelectedTableRow+6�   �� 52      $��1��6   $��Add to Receipt��6"   $��10,176,0,115q�6c   $
��Left=�6y   $��82��6�   ��6;   �� *AddToReceiptCommand+6�   �� 55      $��2��6   $��	0,17,0,10q�6&   $��2��69   .+�56      �� $��Receipt:��6   $
��Bold"�6,   $��0 0 0 5q�6>   �� 57      ��6   �� *CurrentReceipt+��58      �� 59      .R�5:      �� $	 
Horizontal=�6)   .+�5;   "   �� $	��200��6Q   ��6,   �� *Product.nume_produs+6Q   �� 5<   "   $��50��6F   ��6,   �� *
Quantity+6F   �� 5=   "   $	��100��6H   ��6,   �� *
TotalPrice+6H   5>      5?      5A      �� 5B      $
��Bold"�6:   $	��0 5q�6L   ��6   �� *ReceiptTotal+6L   5C      �� 5F      $��3��6   $��Finalize Receipt��6"   $��	-3,22,0,0q�6�   $
��Left=�6�   $��85��6�   ��6=   �� *FinalizeReceiptCommand+{�6h   �� *CanFinalizeReceipt+6�   5G      5H      5I      