using Xunit;
using static CareBreeze.Data.Enumeration;

namespace CareBreeze.Data.Tests
{
    public class EnumerationFixture
    {
        [Fact]
        public void InvalidParse_ThrowsException()
        {
            Assert.Throws<InvalidValueException>(() => FromValue<EnumerationMock>(4711));
        }

        [Fact]
        public void FromValue()
        {
            var value = 1;
            var @enum = FromValue<EnumerationMock>(value);
            Assert.Equal("B", @enum.Name);
            Assert.IsType<EnumerationMock>(@enum);
        }

        [Fact]
        public void FromName()
        {
            var name = "A";
            var @enum = FromName<EnumerationMock>(name);
            Assert.Equal(0, @enum.Value);
            Assert.IsType<EnumerationMock>(@enum);
        }

        [Fact]
        public void ImplicitOperator_int()
        {
            var value = 1;
            var @enum = (EnumerationMock)value;
            Assert.Equal("B", @enum.Name);
            Assert.IsType<EnumerationMock>(@enum);
        }

        [Fact]
        public void ImplicitOperator_string()
        {
            var name = "A";
            var @enum = (EnumerationMock)name;
            Assert.Equal(0, @enum.Value);
            Assert.IsType<EnumerationMock>(@enum);
        }
    }
}
