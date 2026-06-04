using Project2.models;
using System;
using System.Collections.Generic;
using System.Net;
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
    /// Логика взаимодействия для PatientDialog.xaml
    /// </summary>
    public partial class PatientDialog : Window
    {
        public PatientDialog()
        {
            InitializeComponent();
        }

        public PatientDialog(Patient patient) : this()
        {
            txtSurname.Text = patient.Surname;
            txtName.Text = patient.Name;
            txtPatronymic.Text = patient.Patronymic;
            txtPhone.Text = patient.Phone;
            dpDateBirth.SelectedDate = patient.Date_birth;
            cbGender.SelectedItem = cbGender.Items.Cast<ComboBoxItem>().FirstOrDefault(i => i.Content.ToString() == patient.Gender);
            txtAddress.Text = patient.Address;
            txtPhone.Text = patient.Phone;
            txtEmail.Text = patient.Email;
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

            if (dpDateBirth.SelectedDate == null)
            {
                MessageBox.Show("Введите дату рождения!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Введите номер телефона!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var newPatient = new Patient
            {
                Surname = txtSurname.Text.Trim(),
                Name = txtName.Text.Trim(),
                Patronymic = txtPatronymic.Text.Trim(),
                Date_birth = DateTime.SpecifyKind(dpDateBirth.SelectedDate.Value, DateTimeKind.Utc),
                Gender = (cbGender.SelectedItem as ComboBoxItem)?.Content?.ToString(),
                Address = txtAddress.Text.Trim(),
                Phone = txtPhone.Text.Trim(),
                Email = txtEmail.Text.Trim()
            };

            using (var db = new AppDbContext())
            {
                db.Patients.Add(newPatient);
                db.SaveChanges();
            }

            MessageBox.Show("Пациент добавлен!", "Успех",
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

