   M S B A M L   `   `   `  ���� H  DAudioRecorder, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:AudioRecorderAudioRecorder      AudioRecorder.MainWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     -localclr-namespace:AudioRecorder     V RMaterialDesignThemes.Wpf, Version=2.5.1.1345, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  Dmc;http://schemas.openxmlformats.org/markup-compatibility/2006  -     � Title$  Recorder��$	��600��$	��350��   MaterialDesignPaper#���    � 
ResizeMode$ NoResize=�  MaterialDesignBody#���   � WindowStartupLocation$ CenterScreen=� � WindowStyle$
 None=� � 	Resources   ChangeWidthWhenRecord �� 
TargetType(  "MaterialDesignThemes.Wpf.ColorZone �� Property �� Value �� Triggers	 �� Binding
 �� ElementName �� Path �� Value  ToggleButton_Triggers P� Property P� Value��%I  &	       &	 �     �� "  .���  � ��$	 250�� �� 	 �� $
 ToggleButton_Record��$ 	IsChecked� True.a���  � ��$	 450���� " d� P�  �@d�	IsChecked$
 TrueY�. ���  � �� �� *StartRecordCommand+P�  �@d�	IsChecked$ FalseY�. ���  � �� �� *StopRecordCommand+.��  -     Name$ MainBody��$��Stretch=�$��Stretch=�   Mode$ PrimaryDark=�.���  � RowDefinitions �� $��40 ��� $��* ��� $��40 �.+�  $��5q�$
��Left=�$��Center=�+  %MaterialDesignThemes.Wpf.ShadowAssist   ShadowDepth$ Depth3=�   ShadowEdges$ Right=�$ PrimaryDark=�.���� -   $��Button_LoadFile��$��0��$	��100��$
��10,0q�$��Center=�
�������  MaterialDesignToolButton#���  ���� *OpenFileCommand+����  �� Orientation$ 
Horizontal=�.+��� 
Load Files'  !MaterialDesignThemes.Wpf.PackIcon  $��30��$��25���  �ControlzEx.PackIconBase`1[[MaterialDesignThemes.Wpf.PackIconKind, MaterialDesignThemes.Wpf, Version=2.5.1.1345, Culture=neutral, PublicKeyToken=null]]   Kind$ DownloadMultiple=��� $��0��$��	-25,0,0,0q�$��Center=�$��Center=�$��Audio Recorder��$��24(�$��DemiBold"�  $��40��$��40��$��	200,0,0,0q�$��Center=�$
 Play=��� $��Right=�$��RightToLeft=�$ 
Horizontal=�.+�  $��40��$ Depth2=�   CornerRadius$ 1��$ PrimaryDark=�.���� -   $��Button_Close��$��0��$��Center=�
�������#���  ���� *CloseWindowCommand+��  $��30��$��25��$ CloseBox=�  $��40�� K� ZIndex$	 999��$ 25��$ PrimaryDark=�.���� -   $��Button_Maximize��$��0��$��Center=�
�������#���  ��  -   $��PackMaximize��$��30��$��25��$ 
Fullscreen=�  $��40��$ Depth1=�$ 2��$ PrimaryDark=�.���� -   $��Button_Minimize��$��0��$��Center=�
�������#���  ��  $��30��$��25��$ WindowMinimize=�  -   $ MainScreenColorZone��$��1��$��5q�$��Stretch=�$��Stretch=�$ Depth2=�$ 15��$
 Dark=�.��]� .+�9� -	   $��ExpanderListBoxSound�� 9� ExpandDirection$ Right=�����  J� RenderTransformOrigin$ 2.0,2.0&�$��
SOUND LIST�� � LayoutTransform ��  �� Angle$ 90Q�.���� $	��5,0q�$ Vertical=�.+��� -
    ��Name$ ListBox_Sound��$
��Auto��$��Stretch=���. $
��Auto=����� $
 ExpanderListBoxSound��$ ActualHeight����� *	MyRecords+ �� SelectedItem �� *SelectRecord+ �� Mode$ TwoWay=����� .R��� $ 
Horizontal=�.+�  $��5,0,5,0q�$ Music=��� ���� $
 Name��� $��2��$��Stretch=�  MaterialDesignDivider#���   J� SnapsToDevicePixels . � -     �Name$  
GridScreen��! � ColumnDefinitions! �� -   " ��Name$" 
ColumnLeft���� -   $" 	ColumnMid���� -   $" ColumnRight�� �� $	��100 ��� $��* ��� $��50 �.+��� $��2��$��Center=�.+��� $��5q�$��Center=�$ 
Horizontal=�.+��� -   $��LabelBatteryLevel��
��� � �  -   $��BatteryIconStats��$��Center=�$ Battery=��� -   # ��Name$# Button_ChargeBattery��$��15,0,0,0q�$
��10,5q�
������ $��0q�.���� $ 
Horizontal=�.+�  
��� � �$ BatteryCharging=�  
��� � �$ ArrowUpBold=��� -   $��CaptureDeviceComboBox��$��5q�)  #MaterialDesignThemes.Wpf.HintAssist$   Hint$$ Select Device������ *CaptureDevices+ �� *SelectedDevice+$ TwoWay=��� $��1��$��2��$��Right=�$	��Top=�.+��� -   $��	SoundName��$��Right=�$��18(�$��DemiBold"�
���2�2����� *SelectRecord.Name+�� $��Right=�$ 
Horizontal=�.+��� -   $��SoundLengthValue��$��Right=�$��12(�$��Light"����� *SelectRecord.TrackLenght+�� -   $��SoundBitRate��$��Right=�$��12(�$��Light"����� *SelectRecord.BitRate+�� $��Right=�$ 
Horizontal=�.+��� -   $��SoundTypeValue��$��Right=�$��12(�$��Light"����� *SelectRecord.Type+�� -   $��SoundSizeName��$��Right=�$��Size :��$��12(�$��Light"��� -   $��SoundSizeValue��$��Right=�$��12(�$��Light"����� *SelectRecord.Size+]� -   $��SoundVizualization��$��1��$��0��$��3��.+��� $��5q����� *Visualization+�� -   $��ProgressSlider��$��2��$��3��$
��10,0q�
���2�2�% � Maximum$	% 100Q�& � Minimum$& 0Q�' �� Orientation$' 
Horizontal=�( � Value( �� *SliderPosition+$ TwoWay=��� -   $��RecordAlertLabel��$��2��$��0��$��Center=�$��Center=�
��� E������ *	Message+�� $��2��$��2��$��Center=�$ 
Horizontal=�.+�  -   $��
VolumeIcon��$ 	VolumeLow=��� -   $��VolumeSlider��$	��155��$	% 1.0Q�$	& 0.0Q�$' 
Horizontal=�) �� TickFrequency$
) 0.01Q�* �� TickPlacement$
* None=�( �� *	Volumne+$ TwoWay=�  -   $ MediaPanelColorZone��$��2��$��30��$��Center=�+   Darken+ . $ Depth3=�$ Bottom=�$ 15��$ PrimaryLight=�#��[ .���� -   $��MediaPanelStackPanel��$ 
Horizontal=�.+��� -    $��Button_Previous��$��12,0,0,0q� # MaterialDesignRaisedLightButton#��[ , � ToolTip$', !Предедущая запись������ *PreviousSound+��  $ SkipPrevious=��� -!   $��Button_Play��#��[ $, 
Старт������ *PlayCommand+��  
��� d �$
 Play=�� -"   - �Name$- ProgresVoiceLeft��$��2��$	��100��$��8��$��RightToLeft=�
��� E��$	% 0.5Q�y��� $
 ToggleButton_Record��$ 	IsChecked�   BooleanToVisibilityConverter. �� 	Converter#. [ ( �� *Peak+$ OneWay=�d� -#   $��ToggleButton_Record�� $  MaterialDesignActionToggleButton#��[ $, Записать��/ d� 	IsChecked/ �� *StartStopRecord+.��  $��Center=�$��Center=�
��� E��$ Record=�� -$   $- ProgresVoiceRight��$��2��$	��100��$��8��
��� E��$	% 0.5Q�y��� $
 ToggleButton_Record��$ 	IsChecked�#. [ ( �� *Peak+$ OneWay=��� -%   $��Button_Stop��#��[ $', !Остановить запись������ *StopCommand+��  
���  ��$
 Stop=��� -&   $��Button_Next��#��[ $%, Следующая запись������ *	NextSound+��  $ SkipNext=�