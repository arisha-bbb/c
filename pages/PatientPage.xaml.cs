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
    /// Логика взаимодействия для PatientPage.xaml
    /// </summary>
    public partial class PatientPage : Page
    {
        private Patient? _selectedPatient;
        public PatientPage()
        {
            InitializeComponent();
            LoadPatient();
        }

        private void LoadPatient()
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    var patiens = db.Patients.OrderBy(p => p.Patient_ID).ToList();
                    dgPatients.ItemsSource = patiens;
                    txtStatus.Text = $"✅ Загружено пациентов: {patiens.Count}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения к БД: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                txtStatus.Text = "❌ Ошибка подключения к базе данных";
            }
        }


        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new views.PatientDialog();
            //dialog.Owner = this;

            if (dialog.ShowDialog() == true)
            {
                LoadPatient();
            }
        }

        private void DgPatients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedPatient = (Patient)dgPatients.SelectedItem;

            if (_selectedPatient != null)
            {
                txtName.Text = _selectedPatient.FullName;
                txtDate.Text = _selectedPatient.Date_birth.ToString();
                txtInsurance.Text = _selectedPatient.Insurance_Number?.ToString() ?? "";
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedPatient == null)
            {
                MessageBox.Show("Сначала выберите пациента из таблицы!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var dialog = new views.PatientDialog(_selectedPatient);
            if (dialog.ShowDialog() == true)
                LoadPatient();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedPatient == null)
            {
                MessageBox.Show("Сначала выберите пациента из таблицы!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Удалить пациента \"{_selectedPatient.Name}\"?",
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                using (var db = new AppDbContext())
                {
                    var patientToDelete = db.Patients.Find(_selectedPatient.Patient_ID);
                    if (patientToDelete != null)
                    {
                        db.Patients.Remove(patientToDelete);
                        db.SaveChanges();
                    }
                }

                LoadPatient();
                ClearFields();
                _selectedPatient = null;

                MessageBox.Show("Пациент удалён!", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadPatient();
            txtStatus.Text = "🔄 Список пациентов";
            ClearFields();
        }

        private void ClearFields()
        {
            txtName.Text = "";
            txtInsurance.Text = "";
            txtDate.Text = "";
        }
    }
}
