using Microsoft.Extensions.Options;
using NSubstitute;
using Snowball.Domain.Bookshelf.Dtos;
using Snowball.Domain.Bookshelf.Dtos.Options;
using Snowball.Domain.Bookshelf.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Snowball.Domain.Bookshelf.UnitTests.Services
{
    public class BookServiceTests
    {
        [Fact]
        public async Task ReplyAsync()
        {
            var service = CreateWechatService();
            var content = @"<xml>
    <ToUserName><![CDATA[gh_bd0deed3c3d6]]></ToUserName>
    <Encrypt><![CDATA[lfrAp2YImd5PsVxm1LvYvstYl1n16SoFLoiiQCoqzPCkwXQSsqn+MxQIopM75HH2r4BeOduKhJ302qbryr7SYLajZBjVRHGh06hHfCKWHUrVREYq8/DMDvGk8FpgAsNdh1Pw0wulgDSw6wAgswAME/K9C5IQFEw1cDMtUxH+/nTu01aIn0RM7M7Ov3tHAw43Gw27v0oYOfDWBWnI12HrvkCXSjvf7GE+HDu3RNBgrrjKXK2/BTSsa+GOYAHgWL6AX25/udF03VLe3GpD+kDIDgYma738NjH3JTl1y3RphGCo9qhXo/ttzYS0dedoT0aQ2IS8qQfk7R+SInJfb9TiZHZotxaURfqUNCzNDQ2cLr+2askZEGLn4/56MeKSfr6fG+PjPvtujZ0dizkICCjSeArBwJmFYNCbNM8WrFaZqCM=]]></Encrypt></xml> ";
            
            var result = await service.ReplyAsync(content, "aes", "1650698731", "1104985027", "07105dd196725554add9a7b84fcc5ce6d72addd3");
        }

        private IWechatService CreateWechatService()
        {
            var options = Substitute.For<IOptions<WechatOption>>();
            options.Value.Returns(new WechatOption
            {
                Token = "touzimanmanlai",
                EncodingAESKey = "bLM1FertsdCrmJDOjWEQPQf8mEt22y5E69vTzX9YO1X="
            });
            return new WechatService(options,null);
        }
    }
}
