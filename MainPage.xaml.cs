using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using System.Threading.Tasks;
using System.ComponentModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PacMan
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Ellipse _ellipse = new Ellipse();
        const int N = 8;
        int[,] m_walls;

        double m_sleepTimer;

        public MainPage()
        {
            this.InitializeComponent();

            var seed = ((DateTime.UtcNow.Millisecond + DateTime.UtcNow.Second));
            _seed.Text = seed.ToString();

            var pmc = new PacManCreator();
            m_walls = pmc.SetWalls(seed);


            for (var i = 0; i < N; i++)
            {
                for (var j = 0; j < N; j++)
                {
                    var r = new Rectangle();
                    var v = m_walls.GetValue(i, j);
                    if ((int)v == 1)
                    {
                        r.Fill = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 0, 0));
                    }

                    pacManGrid.Children.Add(r);
                    Grid.SetColumn(r, j);
                    Grid.SetRow(r, i);
                }

            }

            _ellipse.Name = "_ellipse";
            _ellipse.Fill = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 225, 225, 0));
            pacManGrid.Children.Add(_ellipse);

            var end = new TextBlock();
            end.Text = "End";
            end.FontSize = 32;

            pacManGrid.Children.Add(end);
            Grid.SetColumn(end, 7);
            Grid.SetRow(end, 7);

        }

        private void _startButton_Click(object sender, RoutedEventArgs e)
        {
            m_sleepTimer = _slider.Value;

            _resetButton.IsEnabled = false;
            _startButton.IsEnabled = false;

            Grid.SetColumn(_ellipse, 0);
            Grid.SetRow(_ellipse, 0);

            var task = Task<bool>.Run(() => this.Solve(m_walls));
            task.ContinueWith((result) =>
            {
                var a = Windows.ApplicationModel.Core.CoreApplication.MainView;
                if (null != a)
                {
                    var dump = a.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                     () =>
                     {
                         _resetButton.IsEnabled = true;
                         _startButton.IsEnabled = true;
                     });
                }
            });
        }

        private bool Solve(int[,] maze)
        {
            int[,] sol = new int[N, N];
            return this.SolveHelper(maze, 0, 0, sol);
        }

        private bool SolveHelper(int[,] maze, int x, int y, int[,] sol)
        {
            //var a = Windows.ApplicationModel.Core.CoreApplication.MainView;

            if (x == N - 1 && y == N - 1)
            {
                UpdateEllipseOnMap(x, y, (int)m_sleepTimer);
                sol[x, y] = 1;
                return true;
            }

            if (isSafe(maze, x, y))
            {
                UpdateEllipseOnMap(x, y, (int)m_sleepTimer);

                if (SolveHelper(maze, x + 1, y, sol))
                {
                    sol[x, y] = 1;
                    return true;
                }
                else if (SolveHelper(maze, x, y + 1, sol))
                {
                    sol[x, y] = 1;
                    return true;
                }
                else
                {
                    sol[x, y] = 0;
                    return false;
                }
            }

            return false;
        }

        private bool isSafe(int[,] maze, int x, int y)
        {
            if (x >= 0 && x < N && y >= 0 && y < N && maze[x, y] == 0)
                return true;

            return false;
        }

        /// <summary>
        /// Updates the pac-man char on the maze
        /// </summary>
        /// <param name="row">X value</param>
        /// <param name="column">Y value</param>
        private void UpdateEllipseOnMap(int row, int column, int sleepTimer=100)
        {
            var a = Windows.ApplicationModel.Core.CoreApplication.MainView;
            if (null != a)
            {
                var ignored = a.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                  () =>
                  {
                      Grid.SetColumn(_ellipse, column);
                      Grid.SetRow(_ellipse, row);
                  });


                System.Threading.Thread.Sleep(sleepTimer);
            }

        }

        private void _resetButton_Click(object sender, RoutedEventArgs e)
        {
            pacManGrid.Children.Clear();

            _ellipse.Name = "_ellipse";
            _ellipse.Fill = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 225, 225, 0));
            pacManGrid.Children.Add(_ellipse);
            Grid.SetColumn(_ellipse, 0);
            Grid.SetRow(_ellipse, 0);

            var seed = ((DateTime.UtcNow.Millisecond + DateTime.UtcNow.Second));
            _seed.Text = seed.ToString();

            var pmc = new PacManCreator();
            m_walls = pmc.SetWalls(seed);

            for (var i = 0; i < N; i++)
            {
                for (var j = 0; j < N; j++)
                {
                    var r = new Rectangle();
                    var v = m_walls.GetValue(i, j);
                    if ((int)v == 1)
                    {
                        r.Fill = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 0, 0));
                    }

                    pacManGrid.Children.Add(r);
                    Grid.SetColumn(r, j);
                    Grid.SetRow(r, i);
                }
            }
        }
    }
}
