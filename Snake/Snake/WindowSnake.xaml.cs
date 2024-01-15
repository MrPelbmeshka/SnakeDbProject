using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Snake
{
    public partial class WindowSnake : Window
    {

        public bool IsDark { get; set; }
        private readonly PaletteHelper paletteHelper = new PaletteHelper();

        private void themeToggle_Click(object sender, RoutedEventArgs e)
        {
            ITheme theme = paletteHelper.GetTheme();
            if (IsDark = theme.GetBaseTheme() == BaseTheme.Dark)
            {
                IsDark = false;
                theme.SetBaseTheme(Theme.Light);
            }
            else
            {
                IsDark = true;
                theme.SetBaseTheme(Theme.Dark);
            }
            paletteHelper.SetTheme(theme);
        }

        private void btn_exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        private const int CellSize = 20;
        private List<Point> snake;
        private Point food;
        private Direction currentDirection;
        private DispatcherTimer gameTimer;
        private int applesEaten = 0;
        string a;


        public WindowSnake(string enteredName, bool usertr)
        {
            InitializeComponent();

            a = enteredName;

            welcome.Text = $"Добро пожаловать {a}";
            if (usertr)
            {
                test.Visibility = Visibility.Visible;
            }
        }

        private void StartGame_Click(object sender, RoutedEventArgs e)
        {
            InitializeGame();
            StartGameButton.Visibility = Visibility.Hidden;
            StopGameButton.Visibility = Visibility.Visible;
            applesEaten = 0; 
            UpdateApplesEaten();
        }

        private void StopGame_Click(object sender, RoutedEventArgs e)
        {
            StopGame();
        }
        private void StopGame()
        {
            speed.IsEnabled = true;
            if (gameTimer != null)
            {
                gameTimer.Stop();
            }

            StartGameButton.Visibility = Visibility.Visible;
            StopGameButton.Visibility = Visibility.Hidden;
            MessageBox.Show("Игра остановлена!");
        }

        private void InitializeGame()
        {
            maxsc.Text = "0";
            var context = new AppDbContext();
            var user = context.Users.SingleOrDefault(x => x.Login == a);

            sc.Text = user.Score.ToString();

            GameCanvas.Children.Clear();
            snake = new List<Point>
            {
                new Point(10, 10),
                new Point(9, 10),
                new Point(8, 10)
            };

            currentDirection = Direction.Right;
            DrawSnake();

            
            CreateFood();


            gameTimer = new DispatcherTimer();
            gameTimer.Tick += GameTimer_Tick;
            if (speed.Text == string.Empty)
            {
                gameTimer.Interval = TimeSpan.FromMilliseconds(200);
            }
            else
            {
                gameTimer.Interval = TimeSpan.FromMilliseconds(int.Parse(speed.Text));
            }

            
            gameTimer.Start();

            GameCanvas.Focus();

            GameCanvas.Visibility = Visibility.Visible;
            applesEaten = 0;
            speed.IsEnabled = false;
        }

        private void DrawSnake()
        {
            foreach (var point in snake)
            {
                DrawRectangle(point, Brushes.Green);
            }
        }

        private void DrawFood()
        {
            DrawRectangle(food, Brushes.Red);
        }
        private void DrawRectangle(Point position, Brush color)
        {
            Rectangle rectangle = new Rectangle
            {
                Width = CellSize,
                Height = CellSize,
                Fill = color
            };

            Canvas.SetLeft(rectangle, position.X * CellSize);
            Canvas.SetTop(rectangle, position.Y * CellSize);

            GameCanvas.Children.Add(rectangle);
        }

        private void CreateFood()
        {
            Random random = new Random();
            int x = random.Next(0, (int)(GameCanvas.ActualWidth / CellSize));
            int y = random.Next(0, (int)(GameCanvas.ActualHeight / CellSize));

            food = new Point(x, y);
            DrawFood();
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            MoveSnake();
            CheckCollision();
            CheckFoodCollision();
        }

        private void MoveSnake()
        {
            Point newHead = snake.First();

            switch (currentDirection)
            {
                case Direction.Up:
                    newHead.Y--;
                    break;
                case Direction.Down:
                    newHead.Y++;
                    break;
                case Direction.Left:
                    newHead.X--;
                    break;
                case Direction.Right:
                    newHead.X++;
                    break;
            }

            snake.Insert(0, newHead);

            
            if (snake.Count > 1)
            {
                snake.RemoveAt(snake.Count - 1);
            }

            GameCanvas.Children.Clear();
            DrawSnake();
            DrawFood();
        }

        private void CheckCollision()
        {
            Point head = snake.First();

            if (head.X < 0 || head.X >= GameCanvas.ActualWidth / CellSize ||
                head.Y < 0 || head.Y >= GameCanvas.ActualHeight / CellSize)
            {
                GameOver();
            }

            if (snake.Skip(1).Any(bodyPart => bodyPart == head))
            {
                GameOver();
            }
        }

        private void CheckFoodCollision()
        {
            Point head = snake.First();

            if (head == food)
            {
                snake.Add(new Point(-1, -1)); 
                CreateFood();
                applesEaten++; 
                UpdateApplesEaten(); 
            }
        }


        private void UpdateApplesEaten()
        {
            var context = new AppDbContext();
            var user = context.Users.SingleOrDefault(x => x.Login == a);

            maxsc.Text = applesEaten.ToString();
            if (applesEaten > user.Score)
            {
                sc.Text = applesEaten.ToString();
            } 
        }

        private void GameOver()
        {
            if (gameTimer != null)
            {
                gameTimer.Stop();
            }

            StartGameButton.Visibility = Visibility.Visible;
            StopGameButton.Visibility = Visibility.Hidden;


            // Обновляем максимальное количество съеденных яблок для текущего пользователя
            var context = new AppDbContext();
            var user = context.Users.SingleOrDefault(x => x.Login == a);

            if (user != null)
            {
                if (applesEaten > user.Score)
                {
                    user.Score = applesEaten;
                    context.SaveChanges();
                    //maxsc.Text = user.Score.ToString();
                    MessageBox.Show($"Поздравляю! Ты установил новый рекорд: {applesEaten} яблок.");
                }
                else
                {
                    MessageBox.Show($"Ты проиграл. Твой лучший результат: {user.Score} яблок.");
                }
            }

        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    if (currentDirection != Direction.Down)
                        currentDirection = Direction.Up;
                    break;
                case Key.Down:
                    if (currentDirection != Direction.Up)
                        currentDirection = Direction.Down;
                    break;
                case Key.Left:
                    if (currentDirection != Direction.Right)
                        currentDirection = Direction.Left;
                    break;
                case Key.Right:
                    if (currentDirection != Direction.Left)
                        currentDirection = Direction.Right;
                    break;
            }
        }

        private enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }
    }
}
