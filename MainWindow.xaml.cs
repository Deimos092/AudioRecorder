using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using AudioRecorder.ViewModel;
using Timer = System.Windows.Threading.DispatcherTimer;
using WPF = MaterialDesignThemes.Wpf;

namespace AudioRecorder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Timer  BatteryTimer;
        static Stack<WPF.PackIconKind> PackIconKind { get; set; }
        protected static byte batterylvl = 100;
        protected static WPF.PackIconKind[] packIcons = {WPF.PackIconKind.Battery0, WPF.PackIconKind.Battery10, WPF.PackIconKind.Battery20,
                                            WPF.PackIconKind.Battery30,WPF.PackIconKind.Battery40,WPF.PackIconKind.Battery50,
                                            WPF.PackIconKind.Battery60,WPF.PackIconKind.Battery70,WPF.PackIconKind.Battery80,
                                            WPF.PackIconKind.Battery90};

        
        //
        public MainWindow()
        {

            InitializeComponent();
            DataContext = new MainViewModel();

            //----------------- BATTERY WORK -------------------
            PackIconKind = new Stack<WPF.PackIconKind>();
            BatteryTimer = new Timer();

            ReloadBattery();
            BatteryTimer.Interval = new TimeSpan(0, 0, 10);//10 секунд на 10% батареи
            BatteryTimer.Tick += new EventHandler(NextLevelBattery);
            BatteryTimer.Start();

            LabelBatteryLevel.Content = "Battery " + batterylvl + " %";
            //---------------------------------------------------

        }
        private void ReloadBattery()
        {
            batterylvl = 100;
            LabelBatteryLevel.Foreground = new SolidColorBrush(Colors.Lime);
            BatteryIconStats.Kind = WPF.PackIconKind.Battery100;
            LabelBatteryLevel.Content = "Battery " + batterylvl + " %";

            PackIconKind.Clear();
            
            foreach (var item in packIcons)
                PackIconKind.Push(item);
        }

        #region ------- Other Options ------------
        private bool clicked = false;
        private Point lmAbs = new Point();
        void PnMouseMove(object sender, MouseEventArgs e)
        {
            if (clicked)
            {
                Point MousePosition = e.GetPosition(this);
                Point MousePositionAbs = new Point();
                MousePositionAbs.X = Convert.ToInt16(this.Left) + MousePosition.X;
                MousePositionAbs.Y = Convert.ToInt16(this.Top) + MousePosition.Y;
                this.Left = this.Left + (MousePositionAbs.X - this.lmAbs.X);
                this.Top = this.Top + (MousePositionAbs.Y - this.lmAbs.Y);
                this.lmAbs = MousePositionAbs;
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            clicked = true;
            this.lmAbs = e.GetPosition(this);
            this.lmAbs.Y = Convert.ToInt16(this.Top) + this.lmAbs.Y;
            this.lmAbs.X = Convert.ToInt16(this.Left) + this.lmAbs.X;
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            clicked = false;
        }

        private void Button_Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Button_Maximize_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
                PackMaximize.Kind = MaterialDesignThemes.Wpf.PackIconKind.Fullscreen;
            }
            else
            {
                WindowState = WindowState.Maximized;
                PackMaximize.Kind = MaterialDesignThemes.Wpf.PackIconKind.FullscreenExit;
            }
        }
        #endregion

        private void Button_Stop_Click(object sender, RoutedEventArgs e)
        {
            Button_Play.Visibility = Visibility.Visible;
        }

        private void NextLevelBattery(object sender, EventArgs e)
        {
            try
            {
                if (batterylvl == 0)
                {
                    this.WindowState = WindowState.Minimized;
                    BatteryTimer.Stop();
                    LabelBatteryLevel.Content = "Charge battery!";
                }
                else
                {
                    BatteryIconStats.Kind = PackIconKind.Pop();                    
                    batterylvl -= 10;
                    LabelBatteryLevel.Content = "Battery " + batterylvl + " %";

                    if (batterylvl <= 80)
                        LabelBatteryLevel.Foreground = new SolidColorBrush(Colors.YellowGreen);
                    if (batterylvl <= 60)
                        LabelBatteryLevel.Foreground = new SolidColorBrush(Colors.Yellow);
                    if (batterylvl <= 40)
                        LabelBatteryLevel.Foreground = new SolidColorBrush(Colors.OrangeRed);
                    if (batterylvl <= 20)
                        LabelBatteryLevel.Foreground = new SolidColorBrush(Colors.Red);   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void Button_ChargeBattery_Click(object sender, RoutedEventArgs e)
        {
            ReloadBattery();
            if (!BatteryTimer.IsEnabled)
                BatteryTimer.Start();
        }
    }
}
