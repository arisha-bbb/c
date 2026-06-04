using Project2.models;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Numerics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace Project2.views
{
    /// <summary>
    /// Логика взаимодействия для DoctorDialog.xaml
    /// </summary>
    public partial class DoctorDialog : Window
    {
        public DoctorDialog()
        {
            InitializeComponent();
        }
        public DoctorDialog(Doctor doctor) : this()
        {
            txtSurname.Text = doctor.Surname;
            txtName.Text = doctor.Name;
            txtPatronymic.Text = doctor.Patronymic;
            txtPhone.Text = doctor.Phone;
            txtOffice.Text = doctor.Office_Number;
            cbSpecialization.SelectedItem = cbSpecialization.Items.Cast<ComboBoxItem>().FirstOrDefault(i => i.Content.ToString() == doctor.Specialization);
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSurname.Text))
            {
                MessageBox.Show("Введите фамилию!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите имя!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Введите номер телефона!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var newDoctor = new Doctor
            {
                Surname = txtSurname.Text.Trim(),
                Name = txtName.Text.Trim(),
                Patronymic = txtPatronymic.Text.Trim(),
                Specialization = (cbSpecialization.SelectedItem as ComboBoxItem)?.Content?.ToString(),
                Phone = txtPhone.Text.Trim(),
                Office_Number = txtOffice.Text.Trim()
            };

            using (var db = new AppDbContext())
            {
                db.Doctors.Add(newDoctor);
                db.SaveChanges();
            }

            MessageBox.Show("Доктор добавлен!", "Успех",
                MessageBoxButton.OK, MessageBoxImage.Information);

            DialogResult = true;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
