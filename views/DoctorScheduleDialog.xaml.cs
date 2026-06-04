using Project2.models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Project2.views
{
    public partial class DoctorScheduleDialog : Window
    {
        private int _doctorId;

        public DoctorScheduleDialog()
        {
            InitializeComponent();

            dpDate.SelectedDate = DateTime.Today;

            using (var db = new AppDbContext())
            {
                cbDoctor.ItemsSource = db.Doctors.ToList();
            }

            cbStatus.SelectedIndex = 0;
        }

        public DoctorScheduleDialog(int doctorId) : this()
        {
            _doctorId = doctorId;

            cbDoctor.SelectedValuePath = "Doctor_ID";
            cbDoctor.SelectedValue = doctorId;
        }

        public DoctorScheduleDialog(DoctorSchedule schedule) : this()
        {
            cbDoctor.SelectedValue = schedule.DoctorId;
            dpDate.SelectedDate = schedule.WorkDate;
            txtSlotStart.Text = schedule.SlotStart.ToString(@"hh\:mm");

            foreach (ComboBoxItem item in cbStatus.Items)
            {
                if (item.Content.ToString() == schedule.Status)
                {
                    cbStatus.SelectedItem = item;
                    break;
                }
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cbDoctor.SelectedItem == null)
            {
                MessageBox.Show(
                    "Выберите врача!",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            if (dpDate.SelectedDate == null)
            {
                MessageBox.Show(
                    "Выберите дату!",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            if (!TimeSpan.TryParse(txtSlotStart.Text.Trim(), out TimeSpan slotStart))
            {
                MessageBox.Show(
                    "Введите время в формате HH:MM!",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            var selectedDoctor = (Doctor)cbDoctor.SelectedItem;

            using (var db = new AppDbContext())
            {
                var schedule = new DoctorSchedule
                {
                    DoctorId = selectedDoctor.Doctor_ID,
                    WorkDate = DateTime.SpecifyKind(dpDate.SelectedDate.Value,DateTimeKind.Utc),
                    SlotStart = slotStart,
                    Status = ((ComboBoxItem)cbStatus.SelectedItem).Content.ToString()
                };

                db.DoctorSchedules.Add(schedule);
                db.SaveChanges();
            }

            MessageBox.Show(
                "Расписание успешно сохранено!",
                "Успех",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

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