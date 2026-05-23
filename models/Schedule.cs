using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Project2.models
{
    public class Schedule
    {
        [Key]
        public int Schedule_ID { get; set; }

        public DateTime Date { get; set; }
        public TimeSpan Start_Time { get; set; }
        public TimeSpan End_Time { get; set; }
        public int DoctorId { get; set; }


        public virtual Doctor? Doctor { get; set; }
    }
}