using Project2.models;
using Project2.pages;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace Project2
{
    public partial class MainWindow : Window
    {
        private bool _isAdmin = false; 

        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(User user) : this()
        {
            InitializeComponent();


            btnPatients.Click += BtnPatients_Click;
            btnDoctors.Click += BtnDoctors_Click;
            btnMedicalRecord.Click += BtnMedicalRecord_Click;
            btnAppointment.Click += BtnAppointment_Click;
            btnReports.Click += BtnReports_Click;
            btnUsers.Click += BtnUsers_Click;
            btnSettings.Click += BtnSettings_Click;
            btnNotifications.Click += BtnNotifications_Click;
            btnLogout.Click += BtnLogout_Click;

            if (user.Role == "Admin")
            {
                _isAdmin = true;
            }

            adminMenuPanel.Visibility = _isAdmin ? Visibility.Visible : Visibility.Collapsed;

            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (s, e) => UpdateCurrentTime();
            timer.Start();

            UpdateCurrentTime();

            txtHeaderUserName.Text = user.FullName;
            txtHeaderUserRole.Text = user.Role; 
        }

        private void UpdateCurrentTime()
        {
            txtCurrentTime.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void BtnPatients_Click(object sender, RoutedEventArgs e)
        {

            contentControl.Content = new PatientPage();
        }

        private void BtnDoctors_Click(object sender, RoutedEventArgs e)
        {

            contentControl.Content = new DoctorPage();
        }

        private void BtnMedicalRecord_Click(object sender, RoutedEventArgs e)
        {
   
            contentControl.Content = new MedicalRecordPage();
        }

        private void BtnAppointment_Click(object sender, RoutedEventArgs e)
        {
      
            contentControl.Content = new AppointmentPage();
        }

        private void BtnReports_Click(object sender, RoutedEventArgs e)
        {
       
            contentControl.Content = new ReportPage();
        }

        private void BtnUsers_Click(object sender, RoutedEventArgs e)
        {

            // contentControl.Content = new UsersPage();
        }

        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {

            // contentControl.Content = new SettingsPage();
        }

        private void BtnNotifications_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Новых уведомлений нет.", "Уведомления");
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Выйти из системы?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
                Application.Current.Shutdown();
        }
    }
}