using System;
using System.Collections.Generic;

namespace CareBreeze.Data.Domain
{
    public abstract class Role : Enumeration
    {
        public static readonly Role Oncologist = new Oncologist("Oncologist", 0);
        public static readonly Role GeneralPractitioner = new GeneralPractitioner("GeneralPractitioner", 1);

        public virtual ICollection<DoctorRole> Doctors { get; set; }

        internal Role() : base()
        {
            // EF
        }

        public Role(string name, int value) : base(name, value)
        {
        }

        public abstract bool CanBook(TreatmentRoom room, Condition condition);
    }

    public class Oncologist : Role
    {
        internal Oncologist() : base()
        {
            // EF
        }

        public Oncologist(string name, int value) : base(name, value)
        {
        }

        public override bool CanBook(TreatmentRoom room, Condition condition)
        {
            if (room.TreatmentMachine == null)
            {
                return false;
            }
            return condition.CanTreat(room.TreatmentMachine.Capability);
        }
    }

    public class GeneralPractitioner : Role
    {
        internal GeneralPractitioner() : base()
        {
            // EF
        }

        public GeneralPractitioner(string name, int value) : base(name, value)
        {
        }

        public override bool CanBook(TreatmentRoom room, Condition condition)
        {
            if(condition.CanTreat(this))
            {
                return true;
            }
            return false;
        }
    }
}
