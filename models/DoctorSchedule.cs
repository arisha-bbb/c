using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project2.models
{
    public class DoctorSchedule
    {
        [Key]
        public int Schedule_ID { get; set; }

        public int DoctorId { get; set; }

        public DateTime WorkDate { get; set; }

        public TimeSpan SlotStart { get; set; }

        public TimeSpan SlotEnd => SlotStart.Add(TimeSpan.FromMinutes(20));

        [MaxLength(20)]
        public string Status { get; set; } = "Свободно";
        public Doctor Doctor { get; set; } = null!;

        public string DoctorFullName => Doctor?.FullName ?? "";
    }
}