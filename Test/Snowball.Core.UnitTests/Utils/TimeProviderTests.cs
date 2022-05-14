using NSubstitute;
using Snowball.Core.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Snowball.Core.UnitTests.Utils
{
    public class TimeProviderTests
    {
        [Fact]
        public void UtcNow_ShouldMinus8Hours_WhenGivenDateTime()
        {
            TimeProvider timeProvider = Substitute.For<TimeProvider>();
            timeProvider.Now.Returns(new DateTime(2022, 5, 14, 12, 28, 25));
            var expected = new DateTime(2022, 5, 14, 4, 28, 25);
            Assert.Equal(expected, timeProvider.UtcNow);
        }
    }
}
