using AutoMapper;
using Snowball.Domain.Wechat.Dtos;
using Snowball.Domain.Wechat.Entities;

namespace Snowball.Domain.Wechat.Profiles
{
    public class WechatProfile : Profile
    {
        public WechatProfile()
        {
            CreateMap<WechatSuggestionDto, WechatSuggestionEntity>();
        }
    }
}
