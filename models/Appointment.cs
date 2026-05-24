using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Project2.models
{
    public class Appointment
    {
        [Key]
        public int Appointment_ID { get; set; }

        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }

        [MaxLength(50)]
        public string Status { get; set; } = null!;

        public int PatientId { get; set; }
        public int DoctorId { get; set; }

        public Patient Patient { get; set; }
        public Doctor Doctor { get; set; }

        public string PatientFullName => Patient?.FullName ?? "";
        public string DoctorFullName => Doctor?.FullName ?? "";

    }
}
