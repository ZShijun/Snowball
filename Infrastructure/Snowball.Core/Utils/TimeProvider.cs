using System;

namespace Snowball.Core.Utils
{
    public abstract class TimeProvider
    {
        public abstract DateTime Now { get; }
        public DateTime UtcNow => Now.ToUniversalTime();
    }
}
