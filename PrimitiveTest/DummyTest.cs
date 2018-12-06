using FluentAssertions;
using Xunit;

namespace PrimitiveTest
{
    public class DummyTest
    {
        [Fact]
        public void should_pass()
        {
            "friends".Should().Be("friends");
        }
    }
}