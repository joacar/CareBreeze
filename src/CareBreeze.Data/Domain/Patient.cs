using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareBreeze.Data.Domain
{
    public class Patient
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Registered { get; set; }

        public int ConditionId { get; set; }

        public virtual Condition Condition { get; set; }

        public virtual ICollection<Consultation> Consultations { get; set; }

    }
}
