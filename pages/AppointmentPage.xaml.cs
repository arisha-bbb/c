using Microsoft.EntityFrameworkCore;
using Project2.models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Project2.pages
{
    public partial class AppointmentPage : Page
    {
        private Appointment? _selectedAppointment;

        public AppointmentPage()
        {
            InitializeComponent();
            LoadAppointment();
        }

        private void LoadAppointment()
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    var appointments = db.Appointments
                        .Include(a => a.Patient)
                        .Include(a => a.Doctor)
                        .OrderBy(a => a.Appointment_ID)
                        .ToList();

                    dgAppointment.ItemsSource = appointments;
                    txtStatus.Text = $"✅ Загружено записей: {appointments.Count}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения к БД: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                txtStatus.Text = "❌ Ошибка подключения к базе данных";
            }
        }

        private void DgAppointment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedAppointment = dgAppointment.SelectedItem as Appointment;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new views.AppointmentDialog();
            if (dialog.ShowDialog() == true)
                LoadAppointment();
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedAppointment == null)
            {
                MessageBox.Show("Сначала выберите запись!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var dialog = new views.AppointmentDialog(_selectedAppointment);
            if (dialog.ShowDialog() == true)
                LoadAppointment();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedAppointment == null)
            {
                MessageBox.Show("Сначала выберите запись!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Удалить запись №{_selectedAppointment.Appointment_ID}?",
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                using (var db = new AppDbContext())
                {
                    var item = db.Appointments.Find(_selectedAppointment.Appointment_ID);
                    if (item != null)
                    {
                        db.Appointments.Remove(item);
                        db.SaveChanges();
                    }
                }

                _selectedAppointment = null;
                LoadAppointment();
                MessageBox.Show("Запись удалёна!", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            _selectedAppointment = null;
            LoadAppointment();
        }
    }
}