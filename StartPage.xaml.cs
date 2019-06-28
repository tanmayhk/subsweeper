using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SubSweeper
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StartPage : Page
    {
        public StartPage()
        {
            this.InitializeComponent();
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.FullScreen;
            string urisource1 = $"/Assets/4x4.png";
            Image img1 = new Image()
            {
                Source = new BitmapImage(new Uri(this.BaseUri, urisource1)),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            Size4.Padding = new Thickness(0, 0, 0, 0);
            Size4.BorderThickness = new Thickness(0, 0, 0, 0);
            Size4.Content = img1;

            string urisource2 = $"/Assets/5x5.png"; 
            Image img2 = new Image()
            {
                Source = new BitmapImage(new Uri(this.BaseUri, urisource2)),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            Size5.Padding = new Thickness(0, 0, 0, 0);
            Size5.BorderThickness = new Thickness(0, 0, 0, 0);
            Size5.Content = img2;

            string urisource3 = $"/Assets/6x6.png";
            Image img3 = new Image()
            {
                Source = new BitmapImage(new Uri(this.BaseUri, urisource3)),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            Size6.Padding = new Thickness(0, 0, 0, 0);
            Size6.BorderThickness = new Thickness(0, 0, 0, 0);
            Size6.Content = img3;

            string urisource4 = $"/Assets/7x7.png";
            Image img4 = new Image()
            {
                Source = new BitmapImage(new Uri(this.BaseUri, urisource4)),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            Size7.Padding = new Thickness(0, 0, 0, 0);
            Size7.BorderThickness = new Thickness(0, 0, 0, 0);
            Size7.Content = img4;
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

        private void Size7_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage), 7);
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }

        private void HowToPlayButton_Click(object sender, RoutedEventArgs e)
        {
            string message = "SubSweeper is a game where you have to find four submarines that are hidden throughout the board. Each game has four submarine sizes that span one, two, three, and four squares, as well as four different board sizes: a 4 x 4, a 5 x 5, a 6 x 6, and a 7 x 7. \nThe goal of the game is to click the buttons and hit all the submarines while maximizing your final score or accuracy. It is dependent on the size of the board and the number of moves you have made. The maximum is hundred, while the minimum is 0. To hit, you must dwell or click on a certain button on the board. \n________________\nWhen you hit a part of a submarine, you will see ____. When you hit every part, you will see the entire sub. If you miss, you will not see anything. So, you must remember where you have already clicked. After you have sunk all four subs, the whole board will turn blue, and you will see your final score or accuracy. \nDuring the game, if you need to stop and think you can use the pause button (____). It lets you pause and plan your moves. Press play (_____) to resume. The exit button (____) allows you to close the game entirely. Finally, the New Game button takes you back to the home page so you can select a board size and start a new game. \nEnjoy!";
            HowtoPlay.Text = message;
            HowtoPlay.Visibility = Visibility.Visible;
            HowtoPlayBackground.Visibility = Visibility.Visible;
        }
    }
}
