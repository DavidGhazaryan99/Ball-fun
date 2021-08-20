using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ball_Fun
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static Random random;
        private static object syncObj = new object();
        List<Border> Bariers = new List<Border>();
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        int ballKeySpeed = 15;
        int balldownSpeed = 5;
        int barierSpeed = 10;
        int barierCount = 3;
        int point = 0;
        int barierSpeedlevel = 500;
        int barierCountLevel = 1000;

        bool collision = true;

        public MainWindow()
        {
            InitRandomNumber(40);
            InitializeComponent();
            addNewRandBarier();
            this.KeyDown += new KeyEventHandler(OnKeyDown);
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(500000);
            dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            collisionBall();
            LevelSpeed();
            ballGravity(ball);

            foreach (var barier in Bariers)
            {
                transferBorder(barier);
            }
            if (Bariers[0].Margin.Left < 0)
            {
                grid.Children.Remove(Bariers[0]);
                Bariers.Remove(Bariers[0]);
            }
            double addLenght = main.Width / barierCount;
            //add barier logic
            if (Bariers.Count / 2 <= barierCount)
            {
                if (Bariers[Bariers.Count - 1].Margin.Left + Bariers[Bariers.Count - 1].Width < main.Width - addLenght)
                {
                    addNewRandBarier();
                }
            }
        }
        private void LevelSpeed()
        {
            if (point == barierSpeedlevel)
            {
                barierSpeed += 3;
                barierSpeedlevel += 500;
            }
            if (point == barierCountLevel)
            {
                barierCount++;
                barierCountLevel += 1000;
            }
        }

        private void addNewRandBarier()
        {
            Border newBorder = new Border();
            newBorder.Height = GenerateRandomNumber(50, Convert.ToInt32(main.Height - ball.Height * 2));
            newBorder.BorderBrush = System.Windows.Media.Brushes.Black;
            newBorder.Background = System.Windows.Media.Brushes.Black;
            newBorder.Width = 30;
            newBorder.HorizontalAlignment = HorizontalAlignment.Left;
            newBorder.VerticalAlignment = VerticalAlignment.Top;
            newBorder.Margin = new Thickness(main.Width - newBorder.Width, 0, 0, 0);
            grid.Children.Add(newBorder);

            Border newBorder2 = new Border();
            newBorder2.Height = GenerateRandomNumber(0, Convert.ToInt32((main.Height - newBorder.Height) - ball.Height * 2));
            newBorder2.BorderBrush = System.Windows.Media.Brushes.Black;
            newBorder2.Background = System.Windows.Media.Brushes.Black;
            newBorder2.Width = 30;
            newBorder2.HorizontalAlignment = HorizontalAlignment.Left;
            newBorder2.VerticalAlignment = VerticalAlignment.Top;
            newBorder2.Margin = new Thickness(main.Width - newBorder2.Width, main.Height - newBorder2.Height, 0, 0);
            grid.Children.Add(newBorder2);
            Bariers.Add(newBorder);
            Bariers.Add(newBorder2);
            point += 50;
            textBlock.Text = Convert.ToString(point);
        }
        private void transferBorder(Border border1)
        {
            var left = border1.Margin.Left;
            var top = border1.Margin.Top;
            if (left - barierSpeed <= 0 - border1.Width)
            {
                left = 0;
            }
            else
            {
                left -= barierSpeed;
            }
            border1.Margin = new Thickness(left, top, 0, 0);
        }
        private static void InitRandomNumber(int seed)
        {
            random = new Random(seed);
        }
        private static int GenerateRandomNumber(int min, int max)
        {
            lock (syncObj)
            {
                if (random == null)
                    random = new Random(); // Or exception...
                return random.Next(min, max);
            }
        }
        private void collisionBall()
        {
            for (int i = 0; i < Bariers.Count; i++)
            {
                if (ball.Margin.Left + ball.Width >= Bariers[i].Margin.Left && ball.Margin.Left <= Bariers[i].Margin.Left + Bariers[i].Width)
                {
                    if (ball.Margin.Top <= Bariers[i].Height && Bariers[i].Margin.Top == 0)
                    {
                        EndText.Text = "Game Over";
                        textBlock.Text = collision.ToString();
                        dispatcherTimer.Stop();
                    }
                }
                if (ball.Margin.Left + ball.Width >= Bariers[i].Margin.Left && ball.Margin.Left <= Bariers[i].Margin.Left + Bariers[i].Width)
                {
                    if (ball.Margin.Top + ball.Height >= Bariers[i].Margin.Top && Bariers[i].Margin.Top != 0 && Bariers[i].Margin.Top <= 405)
                    {
                        EndText.Text = "Game Over";
                        textBlock.Text = collision.ToString();
                        dispatcherTimer.Stop();
                    }
                }
            }
            //if (ball.Margin.Left + ball.Width >= Bariers[1].Margin.Left && ball.Margin.Left + ball.Width <= Bariers[1].Margin.Left + Bariers[1].Width)
            //{
            //    if (ball.Margin.Top >= Bariers[0].Margin.Top)
            //    {
            //        EndText.Text = "Game Over";
            //        textBlock.Text = collision.ToString();
            //        dispatcherTimer.Stop();
            //    }
            //}
            //if (ball.Margin.Left + ball.Width >= Bariers[0].Margin.Left && ball.Margin.Left <= Bariers[0].Margin.Left + Bariers[0].Width)
            //{
            //    if (ball.Margin.Top >= Bariers[0].Height && Bariers[0].Margin.Top == 0)
            //    {
            //        EndText.Text = "Game Over";
            //        textBlock.Text = collision.ToString();
            //        dispatcherTimer.Stop();
            //    }
            //    if (ball.Margin.Top >= Bariers[0].Margin.Top)
            //    {
            //        EndText.Text = "Game Over";
            //        textBlock.Text = collision.ToString();
            //        dispatcherTimer.Stop();
            //    }
            //}
        }
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            var left = ball.Margin.Left;
            var top = ball.Margin.Top;
            switch (e.Key.ToString())
            {
                case "Left":
                    if (left - ballKeySpeed <= 0)
                    {
                        left = 0;
                    }
                    else
                    {
                        left -= ballKeySpeed;
                    }
                    ball.Margin = new Thickness(left, top, 0, 0);
                    break;
                case "Right":
                    if (left + ballKeySpeed >= main.Width - ball.Width)
                    {
                        left = main.Width - ball.Width - 15;
                    }
                    else
                    {
                        left += ballKeySpeed;
                    }
                    ball.Margin = new Thickness(left, top, 0, 0);
                    break;
                case "Up":
                    if (top - ballKeySpeed <= 0)
                    {
                        top = 0;
                    }
                    else
                    {
                        top -= ballKeySpeed;
                    }
                    ball.Margin = new Thickness(left, top, 0, 0);
                    break;
                case "Down":
                    if (top + ballKeySpeed > main.Height - ball.Height)
                    {
                        top = main.Height - ball.Height - 15;
                    }
                    else
                    {
                        top += ballKeySpeed;
                    }
                    ball.Margin = new Thickness(left, top, 0, 0);
                    break;
            }
        }
        private void ballGravity(Ellipse ball)
        {
            var ballTop = ball.Margin.Top;
            if (ballTop + balldownSpeed > main.Height - ball.Height)
            {
                ballTop = main.Height - ball.Height - 15;
            }
            else
            {
                ballTop += balldownSpeed;
            }
            ball.Margin = new Thickness(ball.Margin.Left, ballTop, 0, 0);
        }
    }
}
