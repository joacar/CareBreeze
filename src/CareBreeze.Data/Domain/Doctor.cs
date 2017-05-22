using System;
using System.Collections.Generic;
using System.Linq;

namespace CareBreeze.Data.Domain
{
    public class Doctor
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<DoctorRole> Roles { get; set; }

        public bool CanTreat(Patient patient)
        {
            foreach (var role in Roles.Select(r => r.Role))
            {
                if (patient.Condition.CanTreat(role))
                {
                    return true;
                }
            }
            return false;
        }

        public bool CanBook(Patient patient, TreatmentRoom room)
        {
            foreach (var role in Roles.Select(r => r.Role))
            {
                if (role.CanBook(room, patient.Condition))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
