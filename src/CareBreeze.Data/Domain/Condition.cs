namespace CareBreeze.Data.Domain
{
    public abstract class Condition : Enumeration
    {
        public static readonly Condition Flu = new Flu("Flu", 1);
        public static readonly Condition Breast = new Cancer("Breast", 2);
        public static readonly Condition HeadNeck = new Cancer("Head & Neck", 3);

        internal Condition() : base()
        {
            // EF
        }

        public Condition(string name, int value) :
            base(name, value)
        {

        }

        public abstract bool CanTreat(Role role);

        public abstract bool CanTreat(TreatmentRoom room);

        public abstract bool CanTreat(TreatmentMachineCapability capability);

        public static implicit operator Condition(int id) => FromValue<Condition>(id);
    }
}
