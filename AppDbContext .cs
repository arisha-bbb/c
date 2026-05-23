using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Project2.models;

namespace Project2
{
    public class AppDbContext : DbContext
    {
        private readonly string connectionString = "Host=localhost; Port=5432; Database=ARMDb; Username=armdb; Password=armdb;";
        public DbSet<Patient> Patients {  get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(connectionString);
        }
        public async Task<User?> AuthAsync(string login, string password)
        {
            var user = await Users.FirstOrDefaultAsync
                (u => u.Login == login && u.Password == password);
            return user;
        }
        
    }
    
}
