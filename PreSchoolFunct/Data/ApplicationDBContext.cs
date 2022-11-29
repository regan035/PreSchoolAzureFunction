using Microsoft.EntityFrameworkCore;
using PreSchoolFunct.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreSchoolFunct.Data
{
    public class ApplicationDBContext:DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) {}

        public DbSet<CheckinRequest> CheckinRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CheckinRequest>(entity =>
            {
                entity.HasKey(c => c.Id);
            });


        }
    }
}
