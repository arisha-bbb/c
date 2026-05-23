using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Project2;

namespace Project2.views
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        
        
        public LoginWindow()
        {
            InitializeComponent();
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string login = txtLogin.Text.Trim();
            string password = txtPassword.Password;


            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Заполните поле логин и/или пароль!", 
                                "Заполните поля!",
                                MessageBoxButton.OK, 
                                MessageBoxImage.Warning);
            }

            btnLogin.IsEnabled = false;

            using (var db = new AppDbContext())
            {

                var user = await db.AuthAsync(login, password);

                if (user != null && user.IsActive)
                {
 
                    MainWindow mainWindow = new MainWindow(user);
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Логин или пароль не верен",
                                "Ошибка",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                    txtPassword.Password = "";
                    btnLogin.IsEnabled = true;  
                }
            }
                


        }
    }
}
