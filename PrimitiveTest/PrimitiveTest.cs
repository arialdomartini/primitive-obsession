using System.Configuration;
using FluentAssertions;
using Primitive;
using Xunit;

namespace PrimitiveTest
{
    public class PrimitiveTest
    {
        private class MaxUsers : Primitive<int> {}
        
        [Fact]
        public void should_implicitly_convert_to_primitive()
        {
            var sut = new MaxUsers {Value = 42};

            int result1 = sut;
            result1.Should().Be(42);
            
            var result = sut + 100;
            result.Should().Be(142);
        }

        [Fact]
        public void should_explicitly_convert_to_primitive()
        {
            var sut = new MaxUsers {Value = 999};

            var result = (int)sut;
            result.Should().Be(999);
        }

        [Fact]
        public void it_should_be_seen_with_its_natural_type_if_no_conversion_is_performed()
        {
            var sut = new MaxUsers {Value = 100};

            sut.Should().BeOfType<MaxUsers>();
        }

        private class Foo
        {
            public ConnectionString ConnectionString { get; set; }
        }

        private class ConnectionString : Primitive<string>
        {
            public static implicit operator ConnectionString(string value) =>
                new ConnectionString {Value = value};
        }

        [Fact]
        public void it_should_be_possible_to_define_an_implicit_conversion_for_the_construction()
        {
            var foo = new Foo
            {
                ConnectionString = "some value"
            };

            string result = foo.ConnectionString;

            result.Should().Be("some value");
        }
    }
}