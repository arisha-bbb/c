using Microsoft.EntityFrameworkCore;
using Project2.models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Project2.pages
{
    public partial class MedicalRecordPage : Page
    {
        private MedicalRecord? _selectedRecord;

        public MedicalRecordPage()
        {
            InitializeComponent();

            using (var db = new AppDbContext())
                cbPatient.ItemsSource = db.Patients.OrderBy(p => p.Surname).ToList();
        }

        private void CbPatient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var patient = cbPatient.SelectedItem as Patient;
            if (patient != null)
                LoadRecords(patient.Patient_ID);
        }

        private void LoadRecords(int patientId)
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    var records = db.MedicalRecords.Where(r => r.PatientId == patientId).OrderBy(r => r.Record_Date).ToList();
                    dgMedicalRecords.ItemsSource = records;
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

        private void DgMedicalRecords_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedRecord = dgMedicalRecords.SelectedItem as models.MedicalRecord;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var patient = cbPatient.SelectedItem as Patient;
            if (patient == null)
            {
                MessageBox.Show("Сначала выберите пациента!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var dialog = new views.EditMedicalRecordDialog(patient.Patient_ID);
            if (dialog.ShowDialog() == true)
                LoadRecords(patient.Patient_ID);
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedRecord == null)
            {
                MessageBox.Show("Сначала выберите запись!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var dialog = new views.EditMedicalRecordDialog(_selectedRecord);
            if (dialog.ShowDialog() == true)
            {
                var patient = cbPatient.SelectedItem as Patient;
                if (patient != null) LoadRecords(patient.Patient_ID);
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

            var result = MessageBox.Show($"Удалить запись №{_selectedRecord.Record_ID}?",
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                using (var db = new AppDbContext())
                {
                    var item = db.MedicalRecords.Find(_selectedRecord.Record_ID);
                    if (item != null)
                    {
                        db.MedicalRecords.Remove(item);
                        db.SaveChanges();
                    }
                }

                _selectedRecord = null;
                var patient = cbPatient.SelectedItem as Patient;
                if (patient != null) LoadRecords(patient.Patient_ID);

                MessageBox.Show("Запись удалёна!", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            _selectedRecord = null;
            var patient = cbPatient.SelectedItem as Patient;
            if (patient != null) LoadRecords(patient.Patient_ID);
            txtStatus.Text = "🔄 Список обновлён";
        }
    }
}
