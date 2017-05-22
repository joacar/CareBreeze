using System;

namespace CareBreeze.Data.Domain
{
    public class TreatmentMachineCapability : Enumeration
    {
        public static TreatmentMachineCapability Simple = new TreatmentMachineCapability("Simple", 0);

        public static TreatmentMachineCapability Advanced = new TreatmentMachineCapability("Advanced", 1);

        internal TreatmentMachineCapability() : base()
        {
            // EF
        }

        private TreatmentMachineCapability(string name, int value) : base(name, value)
        {
        }

        public static implicit operator TreatmentMachineCapability(int value)
            => FromValue<TreatmentMachineCapability>(value);

        public static implicit operator TreatmentMachineCapability(string name)
            => FromName<TreatmentMachineCapability>(name);
    }
}
