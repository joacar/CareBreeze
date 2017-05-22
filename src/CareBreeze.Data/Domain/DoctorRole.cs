namespace CareBreeze.Data.Domain
{
    /// <summary>
    /// Join table for <see cref="Doctor"/> and <see cref="Role"/>
    /// </summary>
    public class DoctorRole
    {
        public int DoctorId { get; set; }

        public virtual Doctor Doctor { get; set; }

        public int RoleId { get; set; }

        public virtual Role Role { get; set; }
    }
}
