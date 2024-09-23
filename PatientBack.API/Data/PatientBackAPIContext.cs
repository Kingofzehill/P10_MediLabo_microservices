using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PatientBack.API.Domain;

namespace PatientBack.API.Data
{
    public class PatientBackAPIContext : DbContext
    {
        public PatientBackAPIContext (DbContextOptions<PatientBackAPIContext> options)
            : base(options)
        {
        }

        public DbSet<PatientBack.API.Domain.Patient> Patient { get; set; } = default!;
    }
}
