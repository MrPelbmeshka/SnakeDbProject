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
            void Pol()
            {
                txtUsername_2email.BorderBrush = Brushes.LightGray;
                txtUsername_2email.ToolTip = "Всё чики-пуки";
                txtUsername_2.BorderBrush = Brushes.LightGray;
                txtUsername_2.ToolTip = "Всё чики-пуки";
                txtPassword_3.BorderBrush = Brushes.LightGray;
                txtPassword_3.ToolTip = "Всё чики-пуки";
                txtPassword_2.BorderBrush = Brushes.LightGray;
                txtPassword_2.ToolTip = "Всё чики-пуки";
            }

            Pol();
            
            var login = txtUsername_2.Text;

            var email = txtUsername_2email.Text;

            bool tr_email = false;
            bool tr_pas = false;
            bool tr_pas_2 = false;
            bool tr_pas_3 = false;

            foreach (var item in email)
            {
                if (item == '@')
                    tr_email = true;
            }       

            var passwo = txtPassword_2.Password;
            var passworepeat = txtPassword_3.Password;

            if (passwo == passworepeat)
                tr_pas = true;
            if (passwo.Count() >= 8)
                tr_pas_2 = true;
            foreach (var item in passwo)
            {
                if (item == '@' || item == '`' || item == '!' || item == '*')
                    tr_pas_3 = true;
            }


            if (tr_email == false || tr_pas == false || tr_pas_2 == false || tr_pas_3 == false || txtUsername_2.Text == string.Empty || txtUsername_2email.Text == string.Empty || txtPassword_2.Password == string.Empty || txtPassword_3.Password == string.Empty) 
            {
                if (tr_pas_2 == false && tr_pas_3 == false)
                {
                    txtPassword_2.BorderBrush = Brushes.Red;
                    txtPassword_2.ToolTip = "Пароль должен быть равен 8 или более и в нём должен быть спец символ(! @ * `)";
                }
                else
                {
                    if (tr_pas_2 == false)
                    {
                        txtPassword_2.BorderBrush = Brushes.Red;
                        txtPassword_2.ToolTip = "Пароль равен или больше 8";

                    }
                    if (tr_pas_3 == false)
                    {
                        txtPassword_2.BorderBrush = Brushes.Red;
                        txtPassword_2.ToolTip = "Пароль должен содержать спец символ";
                    }
                }

                if (tr_pas == false)
                {
                    txtPassword_3.BorderBrush = Brushes.Red;
                    txtPassword_3.ToolTip = "Пароль должен быть одинаковым";
                }
                if (tr_email == false)
                {
                    txtUsername_2email.BorderBrush = Brushes.Red;
                    txtUsername_2email.ToolTip = "В Email должна присутстовать @";       
                }
                if(txtUsername_2.Text == string.Empty)
                {
                    txtUsername_2.BorderBrush = Brushes.Red;
                    txtUsername_2.ToolTip = "Поле не должно быть пустым";
                }
                if (txtUsername_2email.Text == string.Empty)
                {
                    txtUsername_2email.BorderBrush = Brushes.Red;
                    txtUsername_2email.ToolTip = "Поле не должно быть пустым";
                }
                if (txtPassword_2.Password == string.Empty)
                {
                    txtPassword_2.BorderBrush = Brushes.Red;
                    txtPassword_2.ToolTip = "Поле не должно быть пустым";
                }

                if (txtPassword_3.Password == string.Empty)
                {
                    txtPassword_3.BorderBrush = Brushes.Red;
                    txtPassword_3.ToolTip = "Поле не должно быть пустым";
                }

                return;
            }

            var isad = 1;

            var sc = 0;

            var context = new AppDbContext();

            var user_exists = context.Users.FirstOrDefault(x => x.Login == login);

            if (user_exists is not null)
            {
                MessageBox.Show("Такой пользователь уже имеется");
                return;
            }
            var user = new User { Login = login, Email =email, Password = passwo, IsAdmin = isad, Score = sc };
            context.Users.Add(user);
            context.SaveChanges();
            MessageBox.Show("Добро пожаловать пупсик");
            
            testik.IsSelected = true;
            Pol();
            txtUsername_2email.Text = string.Empty;
            txtPassword_3.Password = string.Empty;
            txtPassword_2.Password = string.Empty;
            txtUsername_2.Text = string.Empty;
        }


        bool img_vis = false;
        bool visibility_passwoed = false;

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            var login = txtUsername.Text;
            bool tr = false;

            string passwo;
            if (visibility_passwoed == true)
            {
                passwo = txtPassword_box.Text;
            }
            else
            {
                passwo = txtPassword.Password;
            }

            var context = new AppDbContext();

            foreach (var item in login)
            {
                if (item == '@')
                    tr = true;
            }
            var user = context.Users.SingleOrDefault(x => x.Email == login && x.Password == passwo);
            if (tr)
            {
                user = context.Users.SingleOrDefault(x => x.Email == login && x.Password == passwo);
                if (user is null)
                {
                    MessageBox.Show("Неправильный логин или пароль");
                    txtUsername.BorderBrush = Brushes.Red;
                    txtUsername.ToolTip = "Неправильный логин или пароль";
                    txtPassword.BorderBrush = Brushes.Red;
                    txtPassword.ToolTip = "Неправильный логин или пароль";
                    return;
                }
            }
            else
            {
                user = context.Users.SingleOrDefault(x => x.Login == login && x.Password == passwo);
                if (user is null)
                {
                    MessageBox.Show("Неправильный логин или пароль");
                    txtUsername.BorderBrush = Brushes.Red;
                    txtUsername.ToolTip = "Неправильный логин или пароль";
                    txtPassword.BorderBrush = Brushes.Red;
                    txtPassword.ToolTip = "Неправильный логин или пароль";
                    return;
                }
            }

            MessageBox.Show("Ты зашёл в аккаунт пупсик");

            string enteredName = txtUsername.Text;
            bool usertr = false;

            if (user.IsAdmin == 2)
                usertr = true;


            WindowSnake gameWindow = new WindowSnake(enteredName, usertr);
            gameWindow.Show();
            this.Close();
        }
        
        private void applyClick(object sender, RoutedEventArgs e)
        {

            txtPassword_box.Text = txtPassword.Password;
            txtPassword.Visibility = Visibility.Collapsed;
            txtPassword_box.Visibility = Visibility.Visible;
            visibility_passwoed = true; 
            but1.Visibility = Visibility.Collapsed;
            but2.Visibility = Visibility.Visible;
            text_eye.Text = "Cкрыть пароль";

        }
        private void applyClick_2(object sender, RoutedEventArgs e)
        {

            txtPassword.Password = txtPassword_box.Text;
            txtPassword_box.Visibility = Visibility.Collapsed;
            txtPassword.Visibility = Visibility.Visible;
            visibility_passwoed = false;
            but2.Visibility = Visibility.Collapsed;
            but1.Visibility = Visibility.Visible;
            text_eye.Text = "Показать пароль";

        }
    }

}