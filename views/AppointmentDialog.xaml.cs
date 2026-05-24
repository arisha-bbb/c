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
using System.Windows.Shapes;
using System.Xml.Linq;

namespace Project2.views
{
    /// <summary>
    /// Логика взаимодействия для Appointment.xaml
    /// </summary>
    public partial class AppointmentDialog : Window
    {
        public AppointmentDialog()
        {
            InitializeComponent();
            using (var db = new AppDbContext())
            {
                cbPatient.ItemsSource = db.Patients.ToList();
                cbDoctor.ItemsSource = db.Doctors.ToList();
            }
        }

        public AppointmentDialog(Appointment appointment) : this()
        {
            cbPatient.SelectedItem = cbPatient.Items.Cast<Patient>().FirstOrDefault(p => p.Patient_ID == appointment.PatientId);
            cbDoctor.SelectedItem = cbDoctor.Items.Cast<Doctor>().FirstOrDefault(d => d.Doctor_ID == appointment.DoctorId);
            dpDate.SelectedDate = appointment.Date;
            txtTime.Text = appointment.Time.ToString(@"hh\:mm");
            cbStatus.SelectedItem = cbStatus.Items.Cast<ComboBoxItem>().FirstOrDefault(i => i.Content.ToString() == appointment.Status);
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cbPatient.SelectedItem == null)
            {
                MessageBox.Show("Выберите пациента!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (cbDoctor.SelectedItem == null)
            {
                MessageBox.Show("Выберите доктора!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (dpDate.SelectedDate == null)
            {
                MessageBox.Show("Выберите дату!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!TimeSpan.TryParse(txtTime.Text, out TimeSpan time))
            {
                MessageBox.Show("Неверный формат времени! Используйте HH:MM", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var patient = cbPatient.SelectedItem as Patient;
            var doctor = cbDoctor.SelectedItem as Doctor;
            var newAppointment = new Appointment
            {
                PatientId = patient.Patient_ID,
                DoctorId = doctor.Doctor_ID,
                Date = DateTime.SpecifyKind(dpDate.SelectedDate.Value, DateTimeKind.Utc),
                Time = time,
                Status = (cbStatus.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Запланирован"
            };

            using (var db = new AppDbContext())
            {
                db.Appointments.Add(newAppointment);
                db.SaveChanges();
            }

            MessageBox.Show("Запись добавлена!", "Успех",
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
