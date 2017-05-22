using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareBreeze.Data.Domain
{

    public class TreatmentMachine
    {
        public string Name { get; set; }

        public int CapabilityId { get; set; }

        [ForeignKey("CapabilityId")]
        public virtual TreatmentMachineCapability Capability { get; set; }

        public virtual ICollection<TreatmentRoom> Rooms { get; set; }
    }
}
