using CareBreeze.Core.Models;
using System.Collections.Generic;

namespace CareBreeze.Core
{
    public interface IDataImportReader
    {
        IEnumerable<Doctor> Doctors(string filePath);

        IEnumerable<TreatmentMachine> Machines(string filePath);

        IEnumerable<TreatmentRoom> Rooms(string filePath);
    }
}
