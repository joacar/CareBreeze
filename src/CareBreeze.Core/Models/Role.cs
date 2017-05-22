using Newtonsoft.Json;
using System.Collections.Generic;
using System;

namespace CareBreeze.Core.Models
{
    public class DoctorRoleJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Role);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Enumeration.FromName<Role>(reader.Value as string);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

    [JsonConverter(typeof(DoctorRoleJsonConverter))]
    public class Role : Enumeration
    {
        public static readonly Role Oncologist = new Role("Oncologist", 0);
        public static readonly Role GeneralPractitioner = new Role("GeneralPractitioner", 1);

        public Role(string name, int value) : base(name, value)
        {
        }
    }
}
