using System.Collections.Generic;

namespace CareBreeze.Core.Models
{
    public class Doctor
    {
        public string Name { get; set; }

        public IList<Role> Roles { get; set; }
    }
}
