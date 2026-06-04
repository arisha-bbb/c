using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Project2.models
{
    public class Doctor
    {
        [Key]
        public int Doctor_ID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string Surname { get; set; } = null!;

        [MaxLength(100)]
        public string? Patronymic { get; set; }

        public string FullName => $"{Surname} {Name} {Patronymic}";

        [Required]
        [MaxLength(100)]
        public string Specialization { get; set; } = null!;

        [Required]
        [MaxLength(10)]
        public string Office_Number { get; set; }

        [Phone]
        [MaxLength(20)]
        public string? Phone { get; set; }

        public virtual ICollection<Appointment> Appointment { get; set; }
        public virtual ICollection<DoctorSchedule> DoctorSchedules { get; set; }
    }
}