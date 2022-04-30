using Snowball.Core.Utils;
using System.Net;
using Xunit;

namespace Snowball.Core.UnitTests.Utils
{
    public class SecurityUtilTests
    {
        [Fact]
        public void Sha1_ShouldBeNull_WhenGivenNull()
        {
            var actual = SecurityUtil.Sha1(null);
            Assert.Null(actual);
        }

        [Theory]
        [InlineData("", "da39a3ee5e6b4b0d3255bfef95601890afd80709")]
        [InlineData("123456", "7c4a8d09ca3762af61e59520943dc26494f8941b")]
        public void Sha1_ShouldBe40LowerCaseHashString_WhenGivenString(string input, string expected)
        {
            var actual = SecurityUtil.Sha1(input);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Test()
        {
           int j = IPAddress.HostToNetworkOrder(3276);
        }
    }
}
