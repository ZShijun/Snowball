namespace Snowball.Application.Dtos
{
    public enum WechatCommandType
    {
        Suggest = 0,     // 建议
        BookSearch = 1, // 书籍搜索
        Download = 2,   // 下载(获取下载链接)
        Unknow = -1     // 未知
    }
}
