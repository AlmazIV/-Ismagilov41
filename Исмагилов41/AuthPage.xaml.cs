using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Исмагилов41
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        private string _currentCaptcha = string.Empty;

        public AuthPage()
        {
            InitializeComponent();
        }

        private async void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTB.Text;
            string password = PassTB.Text;

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Есть пустые поля");
                return;
            }

            if (CaptchaPanel.Visibility == Visibility.Visible)
            {
                if (CaptchaInput.Text != _currentCaptcha)
                {
                    MessageBox.Show("Капча введена неверно!");
                    LoginBtn.IsEnabled = false;
                    await Task.Delay(10000);
                    LoginBtn.IsEnabled = true;
                    GenerateCaptcha();
                    return;
                }
            }

            User user = Исмагилов41Entities.GetContext().User
                .ToList()
                .Find(p => p.UserLogin == login && p.UserPassword == password);

            if (user != null)
            {
                Manager.MainFrame.Navigate(new ProductPage(user));
                // Очистка текстбоксов при успешном входе
                LoginTB.Text = "";
                PassTB.Text = "";
                CaptchaPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                MessageBox.Show("Введены неверные данные");
                // Очистка текстбоксов при неверном вводе логина/пароля
                LoginTB.Text = "";
                PassTB.Text = "";
                CaptchaPanel.Visibility = Visibility.Visible;
                GenerateCaptcha();
            }
        }

        private void GenerateCaptcha()
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random rand = new Random();
            _currentCaptcha = new string(
                Enumerable.Range(0, 5)
                          .Select(x => chars[rand.Next(chars.Length)])
                          .ToArray()
            );

            CaptchaText.Text = _currentCaptcha;
            CaptchaInput.Text = "";
        }

        private void LoginGuestBtn_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new ProductPage());
        }
    }
}
