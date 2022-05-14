using System;

namespace Snowball.Application.Dtos
{
    public class WechatCommand
    {
        public WechatCommand(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return;
            }

            string[] splits = content.Split(new char[] { ':', '：' });
            if (splits.Length != 2)
            {
                return;
            }

            if (Enum.TryParse(splits[0].Trim(), true, out WechatCommandType commandType))
            {
                CommandType = commandType;
                Content = splits[1].Trim();
            }
        }

        public WechatCommandType CommandType { get; set; } = WechatCommandType.Unknow;

        public string Content { get; set; }
    }
}
