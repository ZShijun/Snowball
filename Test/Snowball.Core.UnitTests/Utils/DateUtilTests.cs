using Snowball.Core.Utils;
using System;
using Xunit;

namespace Snowball.Core.UnitTests.Utils
{
    public class DateUtilTests
    {
        [Fact]
        public void ToSecondTimeStamp_ShouldBeLong_WhenGivenDateTime()
        {
            var dateTime = new DateTime(2022, 5, 3, 12, 34, 56, 789);
            long timeStamp = DateUtil.ToSecondTimeStamp(dateTime);
            long expected = 1651552497;
            Assert.Equal(expected, timeStamp);
        }

        [Fact]
        public void ToMillisecondTimeStamp_ShouldBeLong_WhenGivenDateTime()
        {
            var dateTime = new DateTime(2022, 5, 3, 12, 34, 56, 789);
            long timeStamp = DateUtil.ToMillisecondTimeStamp(dateTime);
            long expected = 1651552496789;
            Assert.Equal(expected, timeStamp);
        }
    }
}
