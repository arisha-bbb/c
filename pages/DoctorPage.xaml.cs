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
    /// Логика взаимодействия для DoctorPage.xaml
    /// </summary>
    public partial class DoctorPage : Page
    {
        private Doctor? _selectedDoctor;
        public DoctorPage()
        {
            InitializeComponent();
            LoadDoctor();
        }

        private void LoadDoctor()
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    var doctors = db.Doctors.OrderBy(p => p.Doctor_ID).ToList();
                    dgDoctors.ItemsSource = doctors;
                    txtStatus.Text = $"✅ Загружено докторов: {doctors.Count}";
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
            var dialog = new views.DoctorDialog();
            //dialog.Owner = this;

            if (dialog.ShowDialog() == true)
            {
                LoadDoctor();
            }
        }

        private void DgDoctors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedDoctor = (Doctor)dgDoctors.SelectedItem;

            if (_selectedDoctor != null)
            {
                txtName.Text = _selectedDoctor.FullName;
                txtSpecialization.Text = _selectedDoctor.Specialization;
                txtOffice.Text = _selectedDoctor.Office_Number?.ToString();
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedDoctor == null)
            {
                MessageBox.Show("Сначала выберите доктора из таблицы!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var dialog = new views.DoctorDialog(_selectedDoctor);
            if (dialog.ShowDialog() == true)
                LoadDoctor();

            
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedDoctor == null)
            {
                MessageBox.Show("Сначала выберите доктора из таблицы!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Удалить доктора \"{_selectedDoctor.Name}\"?",
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                using (var db = new AppDbContext())
                {
                    var doctorToDelete = db.Doctors.Find(_selectedDoctor.Doctor_ID);
                    if (doctorToDelete != null)
                    {
                        db.Doctors.Remove(doctorToDelete);
                        db.SaveChanges();
                    }
                }

                LoadDoctor();
                ClearFields();
                _selectedDoctor = null;

                MessageBox.Show("Доктор удалён!", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadDoctor();
            txtStatus.Text = "🔄 Список докторов";
            ClearFields();
        }

        private void ClearFields()
        {
            txtName.Text = "";
            txtSpecialization.Text = "";
            txtOffice.Text = "";
        }
    }
}
