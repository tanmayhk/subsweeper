using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Battleship
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StartPage : Page
    {
        public StartPage()
        {
            this.InitializeComponent();
        }

        private void Size4_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage), 4);
        }

        private void Size5_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage), 5);
        }

        private void Size6_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage), 6);
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }
    }
}
