using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareBreeze.Data.Domain
{
    public class Consultation
    {
        public int Id { get; set; }

        public int PatientId { get; set; }

        public virtual Patient Patient { get; set; }

        public int DoctorId { get; set; }

        public virtual Doctor Doctor { get; set; }

        public string TreatmentRoomId { get; set; }

        public virtual TreatmentRoom TreatmentRoom { get; set; }

        public DateTime RegistrationDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime ConsultationDate { get; set; }
    }
}
