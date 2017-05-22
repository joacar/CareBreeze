using CareBreeze.Data.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq;
using System.Threading.Tasks;

namespace CareBreeze.Data
{
    public class CareBreezeDbContext : DbContext
    {
        public CareBreezeDbContext(DbContextOptions<CareBreezeDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>(_ =>
            {
                _.Property(e => e.Registered)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("GETUTCDATE()");
            });

            modelBuilder.Entity<DoctorRole>(_ =>
            {
                _.HasKey(e => new { e.DoctorId, e.RoleId });

                _.HasOne(d => d.Doctor)
                .WithMany(r => r.Roles)
                .HasForeignKey(dr => dr.DoctorId);

                _.HasOne(r => r.Role)
                .WithMany(d => d.Doctors)
                .HasForeignKey(dr => dr.RoleId);
            });

            modelBuilder.Entity<Condition>(_ =>
            {
                _.HasKey(c => c.Value);
                _.Property(c => c.Value)
                .ValueGeneratedNever();
                _.Property(c => c.Name)
                .IsRequired();

                _.HasDiscriminator<string>("Type")
                .HasValue<Cancer>(nameof(Cancer))
                .HasValue<Flu>(nameof(Flu));
            });

            modelBuilder.Entity<Role>(_ =>
            {
                _.HasKey(r => r.Value);

                _.Property(r => r.Value)
                .ValueGeneratedNever();

                _.HasDiscriminator<string>("Type")
                .HasValue<Oncologist>(nameof(Oncologist))
                .HasValue<GeneralPractitioner>(nameof(GeneralPractitioner));
            });

            modelBuilder.Entity<TreatmentMachine>(_ =>
            {
                _.HasKey(e => new { e.Name });
            });

            modelBuilder.Entity<TreatmentMachineCapability>(_ =>
            {
                _.HasKey(e => new { e.Value });

                _.Property(r => r.Value)
                .ValueGeneratedNever();
            });

            modelBuilder.Entity<Consultation>(_ =>
            {
                _.HasAlternateKey(e => new
                {
                    e.TreatmentRoomId,
                    e.ConsultationDate
                });

                _.HasAlternateKey(e => new
                {
                    e.DoctorId,
                    e.ConsultationDate
                });

                _.HasAlternateKey(e => new
                {
                    e.PatientId,
                    e.ConsultationDate
                });

                _.Property(e => e.RegistrationDate)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("GETUTCDATE()");

            });

            base.OnModelCreating(modelBuilder);
        }

        public async Task SeedEnumeration()
        {
            try
            {
                // Seed conditions
                foreach (var condition in Enumeration.All<Condition>().Cast<Condition>())
                {
                    Add(condition);
                }
                // Seed roles
                foreach (var role in Enumeration.All<Role>())
                {
                    Add(role);
                }
                // Seed treatment machine capabilities
                foreach (var capability in Enumeration.All<TreatmentMachineCapability>())
                {
                    Add(capability);
                }
                await SaveChangesAsync();
            }
            catch
            {
            }
        }
    }
}
