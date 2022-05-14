using System;

namespace Snowball.Core.Utils
{
    public class SystemTimeProvider : TimeProvider
    {
        public override DateTime Now => DateTime.Now;
    }
}
