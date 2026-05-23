using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Project2.models
{
    public class Patient
    {
        [Key]
        public int Patient_ID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string Surname { get; set; } = null!;

        [MaxLength(100)]
        public string? Patronymic { get; set; }

        public string FullName => $"{Surname} {Name} {Patronymic}";
        public DateTime Date_birth { get; set; }

        [MaxLength(10)]
        public string Gender { get; set; } = null!;

        [MaxLength(200)]
        public string? Address { get; set; }

        [Phone]
        [MaxLength(20)]
        public string? Phone { get; set; }

        [MaxLength(50)]
        public string? Insurance_Number { get; set; }

        [EmailAddress]
        [MaxLength(100)]
        public string? Email { get; set; }
        public virtual ICollection<Appointment> Appointment { get; set; }
        public virtual ICollection<MedicalRecord> MedicalRecord { get; set; }
    }
}
