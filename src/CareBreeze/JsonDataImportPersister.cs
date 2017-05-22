using CareBreeze.Core;
using CareBreeze.Data;
using CareBreeze.Data.Domain;

namespace CareBreeze
{
    public class JsonDataImportPersister : IDataImportPersister
    {
        private readonly CareBreezeDbContext _context;
        private readonly IDataImportReader _reader;

        private string _filePath;

        public JsonDataImportPersister(CareBreezeDbContext context, IDataImportReader reader)
        {
            _context = context;
            _reader = reader;
        }

        public void Persist(string filePath)
        {
            _filePath = filePath;
            var importer = new JsonDataFileImporterReader();
            ImportDoctors(importer);
            ImportMachines(importer);
            ImportRooms(importer);
        }

        private void ImportRooms(JsonDataFileImporterReader importer)
        {
            var rooms = importer.Rooms(_filePath);
            foreach (var room in rooms)
            {
                var r = new TreatmentRoom
                {
                    Name = room.Name,
                    TreatmentMachineId = room.TreatmentMachine
                };
                _context.Add(r);
            }
            _context.SaveChanges();
        }

        private void ImportMachines(JsonDataFileImporterReader importer)
        {
            var machines = importer.Machines(_filePath);
            foreach (var machine in machines)
            {
                var m = new TreatmentMachine
                {
                    Name = machine.Name,
                    CapabilityId = Data.Enumeration.FromName<TreatmentMachineCapability>(machine.Capability).Value,
                };
                _context.Add(m);
            }
            _context.SaveChanges();
        }

        private void ImportDoctors(JsonDataFileImporterReader importer)
        {
            var doctors = importer.Doctors(_filePath);
            foreach (var doctor in doctors)
            {
                var d = new Doctor
                {
                    Name = doctor.Name
                };
                foreach (var role in doctor.Roles)
                {
                    var r = new DoctorRole
                    {
                        Doctor = d,
                        RoleId = role.Value
                    };
                    _context.Add(r);
                }
                _context.Add(d);
            }
            _context.SaveChanges();
        }
    }
}
