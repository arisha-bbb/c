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
    public partial class EditMedicalRecordDialog : Window
    {
        private int _patientId;

        public EditMedicalRecordDialog(int patientId)
        {
            InitializeComponent();
            _patientId = patientId;
            dpDate.SelectedDate = DateTime.Today;
        }


        public EditMedicalRecordDialog(MedicalRecord record) : this(record.PatientId)
        {
            dpDate.SelectedDate = record.Record_Date;
            txtDiagnosis.Text = record.Diagnosis;
            txtTreatment.Text = record.Treatment;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (dpDate.SelectedDate == null)
            {
                MessageBox.Show("Выберите дату!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDiagnosis.Text))
            {
                MessageBox.Show("Введите диагноз!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtTreatment.Text))
            {
                MessageBox.Show("Введите назначение!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var db = new AppDbContext())
            {
                var record = new MedicalRecord
                {
                    PatientId = _patientId,
                    Record_Date = DateTime.SpecifyKind(dpDate.SelectedDate.Value, DateTimeKind.Utc),
                    Diagnosis = txtDiagnosis.Text.Trim(),
                    Treatment = txtTreatment.Text.Trim()
                };

                db.MedicalRecords.Add(record);
                db.SaveChanges();
            }

            MessageBox.Show("Запись сохранена!", "Успех",
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