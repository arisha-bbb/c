using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Project2.models
{
    public class MedicalRecord
    {
        [Key]
        public int Record_ID { get; set; }

        [MaxLength(500)]
        public string? Diagnosis { get; set; }

        [MaxLength(1000)]
        public string? Treatment { get; set; }

        public DateTime Record_Date { get; set; }

        public int PatientId { get; set; }

        public virtual Patient? Patient { get; set; }

        public string PatientFullName => Patient?.FullName ?? "";
    }
}
