using System;

namespace CareBreeze.Data.Domain
{
    public class Flu : Condition
    {
        internal Flu() : base()
        {
            // EF
        }

        public Flu(string name, int value) : base(name, value)
        {
        }

        public override bool CanTreat(Role role) => role == Role.GeneralPractitioner;

        public override bool CanTreat(TreatmentRoom room)
        {
            return true;
        }

        public override bool CanTreat(TreatmentMachineCapability capability)
        {
            return true;
        }
    }
}
