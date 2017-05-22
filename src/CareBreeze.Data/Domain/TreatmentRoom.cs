using System.ComponentModel.DataAnnotations;

namespace CareBreeze.Data.Domain
{
    public class TreatmentRoom
    {
        [Key]
        public string Name { get; set; }

        public string TreatmentMachineId { get; set; }

        public virtual TreatmentMachine TreatmentMachine { get; set; }
    }
}
