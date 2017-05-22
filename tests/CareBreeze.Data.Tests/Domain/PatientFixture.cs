using CareBreeze.Data.Domain;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CareBreeze.Data.Tests.Domain
{
    public class PatientFixture : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _database;
        private readonly CareBreezeDbContext _context;


        public PatientFixture(DatabaseFixture database)
        {
            _database = database;
            _context = _database.CreateContext;
            _context.Set<Patient>().RemoveRange(_context.Set<Patient>());
            _context.SaveChanges();
        }

        [Fact]
        public void InsertPatient_MissingCondition()
        {
            var patient = new Patient
            {
                Name = "Joakim"
            };
            _context.Add(patient);
            Assert.Throws<DbUpdateException>(() => _context.SaveChanges());
        }

        [Fact]
        public void InsertPatient()
        {
            var patient = new Patient
            {
                Name = "Joakim",
                ConditionId = Condition.Flu.Value
            };
            _context.Add(patient);
            var affected = _context.SaveChanges();
            Assert.Equal(1, affected);

        }

        [Fact]
        public void InsertTreatmentRoom_InvalidMachine()
        {
            var room = new TreatmentRoom
            {
                Name = "Test",
                TreatmentMachineId = "Invalid"
            };
            _context.Add(room);
            Assert.Throws<DbUpdateException>(() => _context.SaveChanges());
        }
    }
}
