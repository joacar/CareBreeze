using System.IO;
using System.Linq;
using Xunit;

namespace CareBreeze.Data.Tests
{
    public class JsonDataFileImporterFixture
    {
        private const string FileName = "InitializationData.json";

        private readonly MockCareBreezeDbContextFactory _factory;
        private readonly CareBreezeDbContext _context;

        public JsonDataFileImporterFixture()
        {
            _factory = new MockCareBreezeDbContextFactory();
            _context = _factory.Create();
        }

        [Fact]
        public void ReadDoctors()
        {
            var filePath = Path.Combine(_factory.BasePath, FileName);
            var importer = new JsonDataFileImporterReader();
            var doctors = importer.Doctors(filePath).ToList();
            Assert.Equal(3, doctors.Count());
        }

        [Fact]
        public void ReadRooms()
        {
            var filePath = Path.Combine(_factory.BasePath, FileName);
            var importer = new JsonDataFileImporterReader();
            var rooms = importer.Rooms(filePath).ToList();
            Assert.Equal(5, rooms.Count());
        }

        [Fact]
        public void ReadMachines()
        {
            var filePath = Path.Combine(_factory.BasePath, FileName);
            var importer = new JsonDataFileImporterReader();
            var machines = importer.Machines(filePath).ToList();
            Assert.Equal(3, machines.Count());
        }
    }
}
