using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PatientBackAPI.Domain;

namespace PatientBackAPI.Data
{
    public class PatientBackAPIContext : DbContext
    {
        public PatientBackAPIContext (DbContextOptions<PatientBackAPIContext> options)
            : base(options)
        {
        }

        public DbSet<PatientBackAPI.Domain.Patient> Patient { get; set; } = default!;
    }
}
