using CareBreeze.Data.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Xunit;

namespace CareBreeze.Data.Tests.Domain
{
    public class ConsultationFixture : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _database;
        private readonly CareBreezeDbContext _context;

        public ConsultationFixture(DatabaseFixture database)
        {
            _database = database;
            _context = database.CreateContext;
            // Clear all prior each test
            _context.Set<Consultation>().RemoveRange(_context.Set<Consultation>());
            _context.SaveChanges();
        }


        [Fact]
        public void InsertConsultation()
        {
            var patient = _context.Set<Patient>().First();
            var consultation = new Consultation
            {
                ConsultationDate = DateTime.UtcNow.Date,
                TreatmentRoomId = "One",
                DoctorId = 1,
                Patient = patient
            };
            _context.Add(consultation);
            var affected = _context.SaveChanges();
            Assert.Equal(1, affected);
        }

        [Fact]
        public void InsertConsultation_InvalidTreatmentRoom()
        {
            var p1 = _context.Set<Patient>().First();
            var c1 = new Consultation
            {
                ConsultationDate = DateTime.UtcNow.Date,
                TreatmentRoomId = "One",
                DoctorId = 1,
                Patient = p1
            };
            _context.Add(c1);
            _context.SaveChanges();
            var p2 = _context.Set<Patient>().Last();
            var c2 = new Consultation
            {
                ConsultationDate = DateTime.UtcNow.Date,
                TreatmentRoomId = "One",
                DoctorId = 2,
                Patient = p2
            };
            Assert.Throws<InvalidOperationException>(() => _context.Add(c2));
        }
    }
}
