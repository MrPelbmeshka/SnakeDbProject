using MaterialDesignThemes.Wpf;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Snake
{

    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
        }

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

        private void signupBtn_2_Click(object sender, RoutedEventArgs e)
        {
            var login = txtUsername_2.Text;

            var passwo = txtPassword_2.Password;

            var isad = 1;

            var sc = 0;

            var context = new AppDbContext();

            var user_exists = context.Users.FirstOrDefault(x => x.Login == login);
            if (user_exists is not null)
            {
                MessageBox.Show("Такой пользователь уже имеется");
                return;
            }
            var user = new User { Login = login, Password = passwo, IsAdmin = isad, Score = sc };
            context.Users.Add(user);
            context.SaveChanges();
            MessageBox.Show("Добро пожаловать пупсик");
        }


        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            var login = txtUsername.Text;

            var passwo = txtPassword.Password;

            var context = new AppDbContext();

            var user = context.Users.SingleOrDefault(x => x.Login == login && x.Password == passwo);
            if (user is null)
            {
                MessageBox.Show("Неправильный логин или пароль");
                return;
            }


            MessageBox.Show("Ты зашёл в аккаунт пупсик");

            string enteredName = txtUsername.Text;

            WindowSnake gameWindow = new WindowSnake(enteredName);
            gameWindow.Show();
            this.Close();
        }

        
    }
}