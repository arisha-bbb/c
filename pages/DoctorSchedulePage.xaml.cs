using Project2.models;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Project2.pages
{
    /// <summary>
    /// Логика взаимодействия для DoctorSchedule.xaml
    /// </summary>
    public partial class DoctorSchedulePage : Page
    {
        private DoctorSchedule? _selectedRecord;
        public DoctorSchedulePage()
        {
            InitializeComponent();
            using (var db = new AppDbContext())
                cbDoctor.ItemsSource = db.Doctors.OrderBy(p => p.Surname).ToList();

            dpStart.SelectedDate = DateTime.Today.AddMonths(-1);
            dpEnd.SelectedDate = DateTime.Today;
        }
        private void CbDoctor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var doctor = cbDoctor.SelectedItem as Doctor;
            if (doctor != null)
                LoadRecords(doctor.Doctor_ID);
        }

        private void LoadRecords(int doctorId)
        {
            try
            {
                var start = DateTime.SpecifyKind(dpStart.SelectedDate ?? DateTime.Today.AddMonths(-1), DateTimeKind.Utc);
                var end = DateTime.SpecifyKind(dpEnd.SelectedDate ?? DateTime.Today, DateTimeKind.Utc);

                using (var db = new AppDbContext())
                {
                    var records = db.DoctorSchedules.Where(r => r.DoctorId == doctorId).Where(r => r.WorkDate >= start && r.WorkDate <= end).OrderBy(r => r.WorkDate).ThenBy(r => r.SlotStart)  .ToList();
                    dgDoctorSchedule.ItemsSource = records;
                    txtStatus.Text = $"✅ Загружено записей: {records.Count}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения к БД: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                txtStatus.Text = "❌ Ошибка подключения к базе данных";
            }
        }

        private void DgDoctorSchedule_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedRecord = dgDoctorSchedule.SelectedItem as models.DoctorSchedule;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var doctor = cbDoctor.SelectedItem as Doctor;
            if (doctor == null)
            {
                MessageBox.Show("Сначала выберите доктора!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var dialog = new views.DoctorScheduleDialog(doctor.Doctor_ID);
            if (dialog.ShowDialog() == true)
                LoadRecords(doctor.Doctor_ID);
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedRecord == null)
            {
                MessageBox.Show("Сначала выберите запись!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var dialog = new views.DoctorScheduleDialog(_selectedRecord);
            if (dialog.ShowDialog() == true)
            {
                var doctor = cbDoctor.SelectedItem as Doctor;
                if (doctor != null) LoadRecords(doctor.Doctor_ID);
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedRecord == null)
            {
                MessageBox.Show("Сначала выберите запись!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Удалить запись №{_selectedRecord.Schedule_ID}?",
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                using (var db = new AppDbContext())
                {
                    var item = db.DoctorSchedules.Find(_selectedRecord.Schedule_ID);
                    if (item != null)
                    {
                        db.DoctorSchedules.Remove(item);
                        db.SaveChanges();
                    }
                }

                _selectedRecord = null;
                var doctor = cbDoctor.SelectedItem as Doctor;
                if (doctor != null) LoadRecords(doctor.Doctor_ID);

                MessageBox.Show("Запись удалёна!", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            _selectedRecord = null;
            var doctor = cbDoctor.SelectedItem as Doctor;
            if (doctor != null) LoadRecords(doctor.Doctor_ID);
            txtStatus.Text = "🔄 Список обновлён";
        }
    }
}
