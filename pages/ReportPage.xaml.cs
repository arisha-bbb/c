using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Project2.models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Project2.pages
{
    public partial class ReportPage : Page
    {
        public ReportPage()
        {
            InitializeComponent();
            dpStart.SelectedDate = DateTime.Today.AddMonths(-1);
            dpEnd.SelectedDate = DateTime.Today;
            LoadAll();
        }

        private void LoadAll()
        {
            var start = DateTime.SpecifyKind(dpStart.SelectedDate ?? DateTime.Today.AddMonths(-1), DateTimeKind.Utc);
            var end = DateTime.SpecifyKind(dpEnd.SelectedDate ?? DateTime.Today, DateTimeKind.Utc);

            try
            {
                using (var db = new AppDbContext())
                {
                    dgPatients.ItemsSource = db.Patients
                        .OrderBy(p => p.Surname)
                        .ToList();

                    dgAppointments.ItemsSource = db.Appointments
                        .Include(a => a.Patient)
                        .Include(a => a.Doctor)
                        .Where(a => a.Date >= start && a.Date <= end)
                        .OrderBy(a => a.Date)
                        .ToList();

                    dgMedical.ItemsSource = db.MedicalRecords
                        .Include(r => r.Patient)
                        .Where(r => r.Record_Date >= start && r.Record_Date <= end)
                        .OrderBy(r => r.Record_Date)
                        .ToList();

                    txtStatus.Text = "✅ Данные загружены";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                txtStatus.Text = "❌ Ошибка загрузки";
            }
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadAll();
        }

        private void BtnExcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string csv = "";

                using (var db = new AppDbContext())
                {
                    switch (tabReports.SelectedIndex)
                    {
                        case 0:
                            csv = "ФИО;Дата рождения;Телефон\n";

                            foreach (var p in db.Patients.OrderBy(x => x.Surname))
                            {
                                csv += $"{p.FullName};{p.Date_birth:dd.MM.yyyy};{p.Phone}\n";
                            }
                            break;

                        case 1:
                            csv = "Пациент;Доктор;Дата;Статус\n";

                            var appointments = db.Appointments
                                .Include(a => a.Patient)
                                .Include(a => a.Doctor)
                                .ToList();

                            foreach (var a in appointments)
                            {
                                csv += $"{a.PatientFullName};{a.DoctorFullName};{a.Date:dd.MM.yyyy};{a.Status}\n";
                            }
                            break;

                        case 2:
                            csv = "Пациент;Дата;Диагноз;Назначение\n";

                            var records = db.MedicalRecords
                                .Include(r => r.Patient)
                                .ToList();

                            foreach (var r in records)
                            {
                                csv += $"{r.PatientFullName};{r.Record_Date:dd.MM.yyyy};{r.Diagnosis};{r.Treatment}\n";
                            }
                            break;
                    }
                }

                var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                var filePath = System.IO.Path.Combine(
                    desktop,
                    $"отчёт_{DateTime.Now:dd-MM-yyyy}.csv");

                System.IO.File.WriteAllText(
                    filePath,
                    csv,
                    new System.Text.UTF8Encoding(true));

                MessageBox.Show(
                    $"Отчёт сохранён:\n{filePath}",
                    "Экспорт",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ошибка экспорта: {ex.Message}",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}

