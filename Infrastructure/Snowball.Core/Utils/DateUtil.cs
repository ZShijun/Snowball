using System;

namespace Snowball.Core.Utils
{
    public static class DateUtil
    {
        /// <summary>
        /// 获取精确到秒的时间戳
        /// </summary>
        /// <param name="dateTime">时间</param>
        /// <returns></returns>
        public static long ToSecondTimeStamp(this DateTime dateTime)
        {
            TimeSpan ts = dateTime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }

        /// <summary>
        /// 获取精确到毫秒的时间戳
        /// </summary>
        /// <param name="dateTime">时间</param>
        /// <returns></returns>
        public static long ToMillisecondTimeStamp(this DateTime dateTime)
        {
            TimeSpan ts = dateTime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds);
        }

        /// <summary>  
        /// 时间戳转换成日期  
        /// </summary>  
        /// <param name="timeStamp">时间戳</param>
        /// <param name="accurateToMilliseconds">是否精确到毫秒</param>
        /// <returns></returns>  
        public static DateTime GetDateTime(long timeStamp, bool accurateToMilliseconds)
        {
            TimeSpan timeSpan;
            if (accurateToMilliseconds)
            {
                timeSpan = TimeSpan.FromMilliseconds(timeStamp);
            }
            else
            {
                timeSpan = TimeSpan.FromSeconds(timeStamp);
            }

            // DateTime.UnixEpoch对应的时间的时间戳为0
            return DateTime.UnixEpoch.Add(timeSpan).ToLocalTime();
        }
    }
}
