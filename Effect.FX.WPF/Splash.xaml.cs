using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using NET.Tools;

namespace Effect.FX.WPF
{
    /// <summary>
    /// Interaktionslogik für Splash.xaml
    /// </summary>
    public partial class Splash : Window
    {
        private DispatcherTimer showTimer, closeTimer;

        public Splash()
        {
            InitializeComponent();
            lblVersion.Content = new AssemblyInfo(GetType().Assembly).AssemblyVersion;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            closeTimer = new DispatcherTimer();
            closeTimer.Interval = TimeSpan.FromMilliseconds(1000);
            closeTimer.Tick += (s, ee) =>
            {
                this.Close();

                closeTimer.Stop();
                closeTimer.IsEnabled = false;
            };

            showTimer = new DispatcherTimer();
            showTimer.Interval = TimeSpan.FromMilliseconds(2500);
            showTimer.Tick += (s, ee) =>
            {
                new MainWindow().Show();
                this.Activate();

                showTimer.Stop();
                showTimer.IsEnabled = false;

                closeTimer.IsEnabled = true;
                closeTimer.Start();
            };
            showTimer.IsEnabled = true;
            showTimer.Start();
        }
    }
}
