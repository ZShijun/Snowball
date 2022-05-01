using System;

namespace Snowball.Core
{
    /// <summary>
    /// 自定义业务异常
    /// </summary>
    [Serializable]
    public class BizException : Exception
    {
        public BizException() { }
        public BizException(string message) : base(message) { }
        public BizException(string message, Exception inner) : base(message, inner) { }
        protected BizException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
