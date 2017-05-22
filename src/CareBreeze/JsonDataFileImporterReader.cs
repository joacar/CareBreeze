using CareBreeze.Core;
using CareBreeze.Core.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;

namespace CareBreeze
{
    public class JsonDataFileImporterReader : IDataImportReader
    {
        public JsonDataFileImporterReader()
        {
        }

        public IEnumerable<Doctor> Doctors(string filePath)
        {
            using (StreamReader stream = File.OpenText(filePath))
            using (var reader = new JsonTextReader(stream))
            {
                var data = (JObject)JToken.ReadFrom(reader);
                var doctors = data.SelectToken("doctors");
                foreach (var doctor in doctors)
                {
                    yield return doctor.ToObject<Doctor>();
                }
            }
        }

        public IEnumerable<TreatmentMachine> Machines(string filePath)
        {
            using (StreamReader stream = File.OpenText(filePath))
            using (var reader = new JsonTextReader(stream))
            {
                var data = (JObject)JToken.ReadFrom(reader);
                var machines = data.SelectToken("treatmentMachines");
                foreach (var machine in machines)
                {
                    yield return machine.ToObject<TreatmentMachine>();
                }
            }
        }

        public IEnumerable<TreatmentRoom> Rooms(string filePath)
        {
            using (StreamReader stream = File.OpenText(filePath))
            using (var reader = new JsonTextReader(stream))
            {
                var data = (JObject)JToken.ReadFrom(reader);
                var doctors = data.SelectToken("treatmentRooms");
                foreach (var doctor in doctors)
                {
                    yield return doctor.ToObject<TreatmentRoom>();
                }
            }
        }
    }
}
