using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.System.Threading;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Dodge_Game_Project
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Gameboard gameBoard;
        Rectangle p1;
        Rectangle[] enemies;
        DispatcherTimer timer;
        const int playerSpeed = 15;
        int enemySpeed = 5;
        const int enemyEnemyDistance = 15;
        bool isPaused = false;

        public MainPage()
        {
            this.InitializeComponent();
            Rect windowRectangle = ApplicationView.GetForCurrentView().VisibleBounds;
            gameBoard = new Gameboard(windowRectangle.Width - 10, windowRectangle.Height - 10);

            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            timer.Tick += Timer_Tick;

        }
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            timer.Start();
            p1 = addNewRectangle(gameBoard.p1);

            enemies = new Rectangle[10];
            for (int i = 0; i < gameBoard.enemies.Length; i++)
            {
                Enemy currentEnemy = gameBoard.enemies[i];
                enemies[i] = addNewRectangle(currentEnemy);
            }
        }
        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            isPaused = true;

        }
        private void btnContinue_Click(object sender, RoutedEventArgs e)
        {
            timer.Start();
            isPaused = false;
        }

        //a timer who manages the game
        async void Timer_Tick(object sender, object e)
        {
            moveEnemies();
            enemyCollisions();

            if (gameBoard.gameOver() == true)
            {
                txtGameStatus.Text = "You Lost :(";
                isPaused = true;
                timer.Stop();
            }
            if (gameBoard.isWinner() == true)
            {
                txtGameStatus.Text = "You Won!";
                isPaused = true;
                timer.Stop();
            }
        }

        //moving the player with the keyboard
        private void CoreWindow_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case VirtualKey.Left:
                    if (gameBoard.p1._locationX < 115 || isPaused == true)
                    {
                        break;
                    }
                    else
                    {
                        Canvas.SetLeft(p1, Canvas.GetLeft(p1) - playerSpeed);
                        gameBoard.p1._locationX = (int)Canvas.GetLeft(p1) - playerSpeed;
                        break;
                    }
                case VirtualKey.Right:
                    if (gameBoard.p1._locationX > gameBoard._boardWidth - 15 || isPaused == true)
                    {
                        break;
                    }
                    else
                    {
                        Canvas.SetLeft(p1, Canvas.GetLeft(p1) + playerSpeed);
                        gameBoard.p1._locationX = (int)Canvas.GetLeft(p1) + playerSpeed;
                        break;
                    }
                case VirtualKey.Up:
                    if (gameBoard.p1._locationY < 65 || isPaused == true)
                    {
                        break;
                    }
                    else
                    {
                        Canvas.SetTop(p1, Canvas.GetTop(p1) - playerSpeed);
                        gameBoard.p1._locationY = (int)Canvas.GetTop(p1) - playerSpeed;
                        break;
                    }
                case VirtualKey.Down:
                    if (gameBoard.p1._locationY > gameBoard._boardHeight - 15 || isPaused == true)
                    {
                        break;
                    }
                    else
                    {
                        Canvas.SetTop(p1, Canvas.GetTop(p1) + playerSpeed);
                        gameBoard.p1._locationY = (int)Canvas.GetTop(p1) + playerSpeed;
                        break;
                    }

            }
        }

        //moving enemies
        public void moveEnemies()
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                if (enemies[i] != null)
                {
                    if (gameBoard.enemies[i]._locationX > gameBoard.p1._locationX)
                    {
                        Canvas.SetLeft(enemies[i], Canvas.GetLeft(enemies[i]) - enemySpeed);
                        gameBoard.enemies[i]._locationX = (int)Canvas.GetLeft(enemies[i]) - enemySpeed;
                    }
                    if (gameBoard.enemies[i]._locationX < gameBoard.p1._locationX)
                    {
                        Canvas.SetLeft(enemies[i], Canvas.GetLeft(enemies[i]) + enemySpeed);
                        gameBoard.enemies[i]._locationX = (int)Canvas.GetLeft(enemies[i]) + enemySpeed;
                    }
                    if (gameBoard.enemies[i]._locationY > gameBoard.p1._locationY)
                    {
                        Canvas.SetTop(enemies[i], Canvas.GetTop(enemies[i]) - enemySpeed);
                        gameBoard.enemies[i]._locationY = (int)Canvas.GetTop(enemies[i]) - enemySpeed;
                    }
                    if (gameBoard.enemies[i]._locationY < gameBoard.p1._locationY)
                    {
                        Canvas.SetTop(enemies[i], Canvas.GetTop(enemies[i]) + enemySpeed);
                        gameBoard.enemies[i]._locationY = (int)Canvas.GetTop(enemies[i]) + enemySpeed;
                    }
                }
            }
        }

        //checking if enemies touch each other
        public void enemyCollisions()
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                for (int j = 0; j < enemies.Length; j++)
                {
                    if (i != j && enemies[i] != null && enemies[j] != null)
                    {
                        if (Math.Abs(Canvas.GetTop(enemies[i]) - Canvas.GetTop(enemies[j])) < enemyEnemyDistance
                            && Math.Abs(Canvas.GetLeft(enemies[i]) - Canvas.GetLeft(enemies[j])) < enemyEnemyDistance)
                        {
                            layoutRoot.Children.Remove(enemies[i]);
                            enemies[i] = null;
                            gameBoard.enemies[i] = null;
                            gameBoard.countCollisions++;
                            enemySpeed += 2;
                            txtDeadEnemies.Text = $"Dead Enemies: {gameBoard.countCollisions}/9";
                        }
                    }
                }
            }
        }

        //adding 10 enemies
        public Rectangle addNewRectangle(Gamepiece piece)
        {
            Rectangle rec = new Rectangle();
            if (piece is Player)
            {
                rec.RadiusX = 50;
                rec.RadiusY = 50;
                rec.Fill = new ImageBrush
                {
                    ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/man.png"))
                };
            }
            else
            {
                rec.RadiusX = 50;
                rec.RadiusY = 50;
                rec.Fill = new ImageBrush
                {
                    ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/Corona.jpg"))
                };
            }
            rec.Width = piece._pieceWidth;
            rec.Height = piece._pieceHeight;
            Canvas.SetLeft (rec, piece._locationX);
            Canvas.SetTop (rec, piece._locationY);

            layoutRoot.Children.Add(rec);
            return rec;
        }
    }
}