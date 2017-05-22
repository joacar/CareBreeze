using System;

namespace CareBreeze.Core.Models
{
    public class Patient
    {
        public string Name { get; set; }

        public string Condition { get; set; }

        public DateTime Registered { get; set; }
    }
}
