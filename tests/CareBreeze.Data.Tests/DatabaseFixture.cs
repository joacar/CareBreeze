using CareBreeze.Data.Domain;
using System;
using System.IO;

namespace CareBreeze.Data.Tests
{
    public class DatabaseFixture : IDisposable
    {
        public CareBreezeDbContext Context { get; private set; }

        private readonly string _basePath;
        private readonly string _filePath;
        private readonly MockCareBreezeDbContextFactory _factory;

        public DatabaseFixture()
        {
            _factory = new MockCareBreezeDbContextFactory();
            Context = _factory.Create();
            _basePath = _factory.BasePath;
            _filePath = Path.Combine(_basePath, "InitializationData.json");
            Setup();
        }

        public CareBreezeDbContext CreateContext => _factory.Create();

        public void Drop()
        {
            Context.Database.EnsureDeleted();
        }

        public void Create()
        {
            Context.Database.EnsureCreated();
        }

        public void Setup()
        {
            Drop();
            Create();
            Context.SeedEnumeration().Wait();
            // Import initialization data
            var importer = new JsonDataFileImporterReader();
            var persister = new JsonDataImportPersister(Context, importer);
            persister.Persist(_filePath);
            // Seed with patient
            Context.Add(new Patient
            {
                Name = "Jane",
                ConditionId = Condition.Flu.Value
            });
            Context.Add(new Patient
            {
                Name = "John",
                ConditionId = Condition.Breast.Value
            });
            Context.SaveChanges();
        }

        public void Dispose()
        {
            //Drop();
        }
    }
}
