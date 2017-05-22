namespace CareBreeze.Data.Tests
{
    public sealed class EnumerationMock : Enumeration
    {
        public static EnumerationMock A = new EnumerationMock("A", 0);
        public static EnumerationMock B = new EnumerationMock("B", 1);

        private EnumerationMock(string name, int value) : base(name, value)
        {
        }

        public static implicit operator EnumerationMock(int value)
            => FromValue<EnumerationMock>(value);

        public static implicit operator EnumerationMock(string name)
            => FromName<EnumerationMock>(name);
    }
}
