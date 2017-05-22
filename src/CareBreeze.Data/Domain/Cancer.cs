using System;

namespace CareBreeze.Data.Domain
{
    public class Cancer : Condition
    {

        internal Cancer() : base()
        {
            // EF
        }

        public Cancer(string name, int value) : base(name, value)
        {
        }

        public override bool CanTreat(Role role) => role == Role.Oncologist;

        public override bool CanTreat(TreatmentRoom room)
        {
            if (room.TreatmentMachine == null)
            {
                return false;
            }
            return CanTreat(room.TreatmentMachine.Capability);
        }

        public override bool CanTreat(TreatmentMachineCapability capability)
        {
            if (this == HeadNeck && capability == TreatmentMachineCapability.Advanced)
            {
                return true;
            }
            if (this == Breast && (capability == TreatmentMachineCapability.Advanced ||
                capability == TreatmentMachineCapability.Simple))
            {
                return true;
            }
            return false;
        }
    }
}
