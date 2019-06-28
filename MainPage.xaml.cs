using Microsoft.Toolkit.Uwp.Input.GazeInteraction;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409


namespace SubSweeper
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public class Ship
    {
        public int Row;
        public int Column;
        public int Size;
        public bool IsVertical;
        public int HitCount;

        public Ship(int Row, int Column, int Size, bool IsVertical)
        {
            this.Row = Row;
            this.Column = Column;
            this.Size = Size;
            this.IsVertical = IsVertical;
            this.HitCount = 0;
        }
    }

    public sealed partial class MainPage : Page
    {
        public int BOARD_SIZE = 4;
        public bool isPaused = false;
        List<Ship> shipList = new List<Ship>();
        SolidColorBrush[] shipColors = new SolidColorBrush[] {
                new SolidColorBrush(Color.FromArgb(255, (byte)255, (byte)192, (byte)0)),
                new SolidColorBrush(Color.FromArgb(255, (byte)204, (byte)51, (byte)0)),
                new SolidColorBrush(Color.FromArgb(255, (byte)145, (byte)158, (byte)22)),
                new SolidColorBrush(Color.FromArgb(255, (byte)44, (byte)208, (byte)212)),
            };
        bool gameOver = false;
        private bool[,] hasShip;
        private bool[,] hasBeenHit;
        SolidColorBrush miss_grey = new SolidColorBrush(Color.FromArgb(255, (byte)82, (byte)82, (byte)82));
        SolidColorBrush miss_text = new SolidColorBrush(Color.FromArgb(255, (byte)255, (byte)255, (byte)255));
        List<int> sizes = new List<int>();
        List<bool> isSunk = new List<bool>() { false, false, false, false };
        int shipsHit = 0;
        int shipCount = 4;
        int moves = 0;
        Random rnd;
        public MainPage()
        {
            this.InitializeComponent();
            rnd = new Random();
            Loaded += MainPage_Loaded;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            BOARD_SIZE = (int)e.Parameter;
        }
        private void CreateBoard(int boardSize)
        {
            ShipGrid.Children.Clear();
            ShipGrid.RowDefinitions.Clear();
            ShipGrid.ColumnDefinitions.Clear();

            for (int row = 0; row < BOARD_SIZE; row++)
            {
                ShipGrid.RowDefinitions.Add(new RowDefinition());
                ShipGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            Button[,] buttonGrid = new Button[BOARD_SIZE, BOARD_SIZE];
            for (int row = 0; row < BOARD_SIZE; row++)
            {
                for (int col = 0; col < BOARD_SIZE; col++)
                {
                    var button = new Button();
                    button.Click += OnFired;
                    button.HorizontalAlignment = HorizontalAlignment.Stretch;
                    button.VerticalAlignment = VerticalAlignment.Stretch;
                    button.Margin = new Thickness(2, 2, 2, 2);
                    Grid.SetRow(button, row);
                    Grid.SetColumn(button, col);
                    ShipGrid.Children.Add(button);
                }
            }
            ShipGrid.UpdateLayout();
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            hasShip = new bool[BOARD_SIZE, BOARD_SIZE];
            hasBeenHit = new bool[BOARD_SIZE, BOARD_SIZE];
            CreateBoard(BOARD_SIZE);
            InitializeBoard();
            ResetLayout();
        }

        private Ship CreateRandomShip(int size)
        {
            int x = rnd.Next(0, BOARD_SIZE);
            int y = rnd.Next(0, BOARD_SIZE);
            bool isVertical = rnd.Next(0, 2) == 1;
            Ship ship = new Ship(x, y, size, isVertical);
            return ship;
        }

        private bool CheckIntersections(Ship ship)
        {
            int check_counter = 0;
            while (check_counter < ship.Size)
            {
                if (ship.IsVertical == true)
                {
                    if (ship.Row + check_counter >= BOARD_SIZE)
                    {
                        return true;
                    }

                    if (hasShip[ship.Row + check_counter, ship.Column] == true)
                    {
                        return true;
                    }
                }
                if (ship.IsVertical == false)
                {
                    if (ship.Column + check_counter >= BOARD_SIZE)
                    {
                        return true;
                    }

                    if (hasShip[ship.Row, ship.Column + check_counter] == true)
                    {
                        return true;
                    }
                }
                check_counter += 1;
            }
            return false;
        }

        private void ShowLayout()
        {
            var children = ShipGrid.Children;
            foreach (var child in children)
            {
                Button button = child as Button;
                if (button == null)
                {
                    continue;
                }
                int row = Grid.GetRow(button);
                int column = Grid.GetColumn(button);
                if (hasShip[row, column] == true)
                {
                    button.Content = "X";
                }
            }
        }

        private void ResetLayout()
        {
            var children = ShipGrid.Children;
            foreach (var child in children)
            {
                Button button = child as Button;
                if (button == null)
                {
                    continue;
                }
                int row = Grid.GetRow(button);
                int column = Grid.GetColumn(button);
                button.Content = "";
                button.Background = new SolidColorBrush(Color.FromArgb(255, (byte)175, (byte)171, (byte)171));
            }
        }

        private void InitializeBoard()
        {
            hasShip = new bool[BOARD_SIZE, BOARD_SIZE];
            shipList = new List<Ship>();
            int[] shipSizes = { 1, 2, 3, 4 };
            int curSize = 0;
            while (curSize < shipSizes.Length)
            {
                Ship ship = CreateRandomShip(shipSizes[curSize]);
                if (CheckIntersections(ship) == false)
                {
                    int counter = 0;
                    while (counter < ship.Size && ship.IsVertical == true)
                    {
                        hasShip[ship.Row + counter, ship.Column] = true;
                        counter += 1;

                    }
                    counter = 0;
                    while (counter < ship.Size && ship.IsVertical == false)
                    {
                        hasShip[ship.Row, ship.Column + counter] = true;
                        counter += 1;
                    }
                    shipList.Add(ship);
                    curSize += 1;
                }
            }
            hasBeenHit = new bool[BOARD_SIZE, BOARD_SIZE];
            shipsHit = 0;
            shipCount = 4;
            moves = 0;
            ShipScore.Text = $"{shipCount}";
            Moves.Text = $"{moves}";

        }

        private int FindShip(int row, int column)
        {
            int length = shipList.Count();
            for (int i = 0; i < length; i++)
            {
                var ship = shipList[i];
                if (ship.IsVertical == true)
                {
                    if (ship.Column == column && row >= ship.Row && row < ship.Row + ship.Size)
                    {
                        return i;
                    }
                }
                else
                {
                    if (ship.Row == row && column >= ship.Column && column < ship.Column + ship.Size)
                    {
                        return i;
                    }
                }
            }
            return 4;
        }

        private Button GetShipButton(int row, int column)
        {
            foreach (var child in ShipGrid.Children)
            {
                var button = child as Button;
                if (button != null)
                {
                    int i = Grid.GetRow(button);
                    int j = Grid.GetColumn(button);
                    if (i == row && j == column)
                    {
                        return button;
                    }
                }
            }
            return null;
        }

        private void AssignImages(Ship ship)
        {
            int h_counter = 0;
            int v_counter = 0;
            int counter = 0;
            string ship_type = "";
            while (counter < ship.Size)
            {
                if (ship.IsVertical)
                {
                    ship_type = "v";
                    v_counter = counter;
                }
                else
                {
                    ship_type = "h";
                    h_counter = counter;
                }
                Button button = GetShipButton(ship.Row + v_counter, ship.Column + h_counter);
                string urisource = $"/Assets/ship_{ship.Size}_{counter}_{ship_type}.png";
                button.Padding = new Thickness(0, 0, 0, 0);
                button.BorderThickness = new Thickness(0, 0, 0, 0);
                Image img = new Image()
                {
                    Source = new BitmapImage(new Uri(this.BaseUri, urisource)),
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                button.Content = img;
                counter += 1;
            }
        }
        private void SinkShip(int index)
        {
            Ship ship = shipList[index];
            int i = ship.Row;
            int j = ship.Column;
            int count = ship.Size;
            var button = GetShipButton(i, j);
            while (count > 0)
            {
//                button.Background = shipColors[index];
                if (ship.IsVertical == true)
                {
                    i += 1;
                }

               if (ship.IsVertical == false)
                {
                    j += 1;
                }
                count -= 1;
            }
            AssignImages(ship);
        }

        private void OnNewGame(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(StartPage), null);
        }

        private void OnlySea()
        {
            for (int i = 0; i < BOARD_SIZE; i++)
            {
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    Button shipButton = GetShipButton(i, j);
                    if (hasShip[i, j] == false)
                    {
                        shipButton.Background = new SolidColorBrush(Color.FromArgb(255, (byte)43, (byte)57, (byte)144));
                    }
                }
            }
        }

        private async void OnFired(object sender, RoutedEventArgs e)
        {
            if (gameOver)
            {
                return;
            }
            var button = sender as Button;
            int row = Grid.GetRow(button);
            int column = Grid.GetColumn(button);
            if (hasShip[row, column] == false)
            {
                if (hasBeenHit[row, column] == false)
                {
                    hasBeenHit[row, column] = true;
                    moves += 1;
                    Moves.Text = $"{moves}";
                }
            }
            else
            {
                int index = FindShip(row, column);
                Ship ship = shipList[index];
                if (hasBeenHit[row, column] == false)
                {
                    ship.HitCount += 1;
                    hasBeenHit[row, column] = true;
                    moves += 1;
                    Moves.Text = $"{moves}";
                    string urisource = $"/Assets/boom.png";
                    button.Padding = new Thickness(0, 0, 0, 0);
                    button.BorderThickness = new Thickness(0, 0, 0, 0);
                    Image img = new Image()
                    {
                        Source = new BitmapImage(new Uri(this.BaseUri, urisource)),
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center
                    };
                    button.Content = img;
                }

                if (ship.HitCount == ship.Size && isSunk[index] == false)
                {
                    shipsHit += 1;
                    ShipScore.Text = $"{4 - shipsHit}";
                    SinkShip(index);
                    isSunk[index] = true;
                }

                if (shipsHit == 4)
                {
                    gameOver = true;
                    int bsqaured = BOARD_SIZE * BOARD_SIZE;
                    double accuracy = (100 / (10 - bsqaured)) * (moves - bsqaured);
                    string message = $"Game Over! \nAccuracy: {accuracy:g}%";
                    ShipsLeftText.Visibility = Visibility.Collapsed;
                    ShipScore.Visibility = Visibility.Collapsed;
                    MovesText.Visibility = Visibility.Collapsed;
                    Moves.Visibility = Visibility.Collapsed;
                    Accuracy.Text = message;
                    Accuracy.FontWeight = FontWeights.SemiBold;
                    Accuracy.Visibility = Visibility.Visible;
                    OnlySea();
                    
                }
            }
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }

        private void Pause_Button_Click(object sender, RoutedEventArgs e)
        {
            isPaused = ! isPaused;
            if (! isPaused)
            {
                GazeInput.SetInteraction(ShipGrid, Interaction.Disabled);
                Pause_Button.Content = "\uE769";
                foreach (var child in ShipGrid.Children)
                {
                    Button button = child as Button;
                    button.IsEnabled = true;
                }
            }
            if (isPaused)
            {
                GazeInput.SetInteraction(ShipGrid, Interaction.Enabled);
                Pause_Button.Content = "\uE768";
                foreach (var child in ShipGrid.Children)
                {
                    Button button = child as Button;
                    button.IsEnabled = false;
                }
            }
        }
    }
}