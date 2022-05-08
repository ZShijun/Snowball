using System;
using System.Collections.Generic;
using System.Text;

namespace Snowball.Core
{
    public abstract class TimeProvider
    {
        private static TimeProvider _current =
            DefaultTimeProvider.Instance;
        public static TimeProvider Current
        {
            get { return _current; }
            set
            {
                _current = value ?? throw new ArgumentNullException("value");
            }
        }

        public abstract DateTime Now { get; }

        public static void ResetToDefault()
        {
            _current = DefaultTimeProvider.Instance;
        }
    }

    internal class DefaultTimeProvider : TimeProvider
    {
        private readonly static DefaultTimeProvider _instance =
            new DefaultTimeProvider();

        private DefaultTimeProvider() { }

        public override DateTime Now
        {
            get { return DateTime.Now; }
        }

        public static DefaultTimeProvider Instance
        {
            get { return _instance; }
        }
    }
}
