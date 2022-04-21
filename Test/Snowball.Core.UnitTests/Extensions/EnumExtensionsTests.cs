using Snowball.Core.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Xunit;

namespace Snowball.Core.UnitTests.Extensions
{
    public class EnumExtensionsTests
    {
        [Fact]
        public void GetDisplayName_ShouldBeDisplayName_WhenGivenDisplayAttribute()
        {
            TestEnum enum1 = TestEnum.Enum1;
            var displayName = enum1.GetDisplayName();
            Assert.Equal("枚举值1", displayName);
        }

        [Fact]
        public void GetDisplayName_ShouldBeEnumName_WhenNotGivenDisplayAttribute()
        {
            TestEnum enum2 = TestEnum.Enum2;
            var displayName = enum2.GetDisplayName();
            Assert.Equal("Enum2", displayName);
        }
    }

    public enum TestEnum
    {
        [Display(Name = "枚举值1")]
        Enum1 = 1,
        Enum2 = 2
    }
}
