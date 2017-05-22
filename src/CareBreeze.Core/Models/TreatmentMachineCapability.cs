using System;
using Newtonsoft.Json;

namespace CareBreeze.Core.Models
{
    public class TreatmentMachineCapabilityJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(TreatmentMachineCapability);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Enumeration.FromName<TreatmentMachineCapability>(reader.Value as string);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

    [JsonConverter(typeof(TreatmentMachineCapabilityJsonConverter))]
    public class TreatmentMachineCapability : Enumeration
    {
        public static TreatmentMachineCapability Simple = new TreatmentMachineCapability("Simple", 0);

        public static TreatmentMachineCapability Advanced = new TreatmentMachineCapability("Advanced", 1);

        private TreatmentMachineCapability(string name, int value) : base(name, value)
        {
        }

        public static implicit operator TreatmentMachineCapability(int value)
            => FromValue<TreatmentMachineCapability>(value);

        public static implicit operator TreatmentMachineCapability(string name)
            => FromName<TreatmentMachineCapability>(name);
    }
}
