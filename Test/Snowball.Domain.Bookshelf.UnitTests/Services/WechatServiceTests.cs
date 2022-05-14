using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using Snowball.Core.Utils;
using Snowball.Domain.Bookshelf.Dtos;
using Snowball.Domain.Bookshelf.Dtos.Options;
using Snowball.Domain.Bookshelf.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Snowball.Domain.Bookshelf.UnitTests.Services
{
    public class WechatServiceTests
    {
        [Fact]
        public void CheckSignature_ShouldBeTrue_WhenGivenCorrectTimestampAndNonceWithoutEncryptMessage()
        {
            IWechatService wechatService = CreateWechatService();
            string signature = "187ba7507d307cd6ddf6347673971cc48a2209c3";
            string timestamp = "1651997793";
            string nonce = "67628012";
            bool actual = wechatService.CheckSignature(signature, timestamp, nonce);
            Assert.True(actual);
        }

        [Fact]
        public void CheckSignature_ShouldBeFalse_WhenGivenIncorrectNonceWithoutEncryptMessage()
        {
            IWechatService wechatService = CreateWechatService();
            string signature = "187ba7507d307cd6ddf6347673971cc48a2209c3";
            string timestamp = "1651997793";
            string nonce = "67628013";
            bool actual = wechatService.CheckSignature(signature, timestamp, nonce);
            Assert.False(actual);
        }

        [Fact]
        public void CheckSignature_ShouldBeTrue_WhenGivenCorrectTimestampAndNonceWithEncryptMessage()
        {
            IWechatService wechatService = CreateWechatService();
            string msgSignature = "4dc755f17cb7277337ad9355f7b44192063d1b13";
            string timestamp = "1651997816";
            string nonce = "1904323700";
            string encryptMessage = "pPGBBC+cqY/cs1X2EZTVliYHzK77aDh241c1FM0VKUD1lWM8YQkeRADqIr29OGgDPBM6Tp3Tms5FOB6TNMe9UrxwwxLjNRwHsB/u3TjJNEuotClU+B+J9Moq6axHigihNeji8uC8EmzR65Zs19JbHNr9TwHDKkMvZPh8+n1BUzewj6qkRqA9M0TwOMM0nPHjW9iZOS9gR8tw44O7FdkRJ9bHiF3YMQty1+HsKhHEyjE2mjTxPrU341e+oLk6w+0I/yznPlwMmyRb7sA7X0fYpRAUyKHO+dfP9s7e69AqpX7GhOXG+IJeFUC6o328QpevHyHFDaTAWQqBMaZ1Kjn28XdLwMlK/Ta1gFRam0VFlmQvI7610jiLeKkr/ryqWO05BlIyIANwWF+Z7FbTfM+iM3iN1y76x9Cz3EYPtSAE/mY=";
            bool actual = wechatService.CheckSignature(msgSignature, timestamp, nonce, encryptMessage);
            Assert.True(actual);
        }

        [Fact]
        public void CheckSignature_ShouldBeFalse_WhenGivenIncorrectNonceWithEncryptMessage()
        {
            IWechatService wechatService = CreateWechatService();
            string msgSignature = "4dc755f17cb7277337ad9355f7b44192063d1b13";
            string timestamp = "1651997816";
            string nonce = "1904323701";
            string encryptMessage = "pPGBBC+cqY/cs1X2EZTVliYHzK77aDh241c1FM0VKUD1lWM8YQkeRADqIr29OGgDPBM6Tp3Tms5FOB6TNMe9UrxwwxLjNRwHsB/u3TjJNEuotClU+B+J9Moq6axHigihNeji8uC8EmzR65Zs19JbHNr9TwHDKkMvZPh8+n1BUzewj6qkRqA9M0TwOMM0nPHjW9iZOS9gR8tw44O7FdkRJ9bHiF3YMQty1+HsKhHEyjE2mjTxPrU341e+oLk6w+0I/yznPlwMmyRb7sA7X0fYpRAUyKHO+dfP9s7e69AqpX7GhOXG+IJeFUC6o328QpevHyHFDaTAWQqBMaZ1Kjn28XdLwMlK/Ta1gFRam0VFlmQvI7610jiLeKkr/ryqWO05BlIyIANwWF+Z7FbTfM+iM3iN1y76x9Cz3EYPtSAE/mY=";
            bool actual = wechatService.CheckSignature(msgSignature, timestamp, nonce, encryptMessage);
            Assert.False(actual);
        }

        [Fact]
        public void GetEncryptMessage_ShouldBeNull_WhenGivenXmlWithoutEncryptElement()
        {
            IWechatService wechatService = CreateWechatService();
            string messageBody = @"<xml>
    <ToUserName><![CDATA[gh_123456]]></ToUserName>
    <Content><![CDATA[abc]]></Content></xml>";
            var actual = wechatService.GetEncryptMessage(messageBody);
            Assert.Null(actual);
        }

        [Fact]
        public void GetEncryptMessage_ShouldBeSting_WhenGivenXmlWithEncryptElement()
        {
            IWechatService wechatService = CreateWechatService();
            string messageBody = @"<xml>
    <ToUserName><![CDATA[gh_123456]]></ToUserName>
    <Encrypt><![CDATA[abc]]></Encrypt></xml>";
            string expected = "abc";
            var actual = wechatService.GetEncryptMessage(messageBody);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetPlaintextMessage_ShouldDecrypt_WhenGivenAesEncryptedMessage()
        {
            IWechatService wechatService = CreateWechatService();
            string encrypted = "pPGBBC+cqY/cs1X2EZTVliYHzK77aDh241c1FM0VKUD1lWM8YQkeRADqIr29OGgDPBM6Tp3Tms5FOB6TNMe9UrxwwxLjNRwHsB/u3TjJNEuotClU+B+J9Moq6axHigihNeji8uC8EmzR65Zs19JbHNr9TwHDKkMvZPh8+n1BUzewj6qkRqA9M0TwOMM0nPHjW9iZOS9gR8tw44O7FdkRJ9bHiF3YMQty1+HsKhHEyjE2mjTxPrU341e+oLk6w+0I/yznPlwMmyRb7sA7X0fYpRAUyKHO+dfP9s7e69AqpX7GhOXG+IJeFUC6o328QpevHyHFDaTAWQqBMaZ1Kjn28XdLwMlK/Ta1gFRam0VFlmQvI7610jiLeKkr/ryqWO05BlIyIANwWF+Z7FbTfM+iM3iN1y76x9Cz3EYPtSAE/mY=";
            var plaintext = wechatService.GetPlaintextMessage(encrypted);
            Assert.Equal("2:10001", plaintext.Content);
        }

        [Theory]
        [InlineData(null, "toUser", "content")]
        [InlineData("", "toUser", "content")]
        [InlineData("  ", "toUser", "content")]
        [InlineData("fromUser", null, "content")]
        [InlineData("fromUser", "", "content")]
        [InlineData("fromUser", "  ", "content")]
        [InlineData("fromUser", "toUser", null)]
        [InlineData("fromUser", "toUser", "")]
        [InlineData("fromUser", "toUser", "  ")]
        public void BuildNormalReplayMessage_ShouldBeEmptyString_WhenAnyParameterIsNullOrWhiteSpace(string fromUser, string toUser, string content)
        {
            IWechatService wechatService = CreateWechatService();
            string actual = wechatService.BuildNormalReplayMessage(fromUser, toUser, content);

            Assert.Empty(actual);
        }

        [Fact]
        public void BuildNormalReplayMessage_ShouldBeXmlMessage_WhenAllParameterIsValid()
        {
            IWechatService wechatService = CreateWechatService();
            string actual = wechatService.BuildNormalReplayMessage("fromUser", "toUser", "content");
            string expected = "<xml><ToUserName><![CDATA[toUser]]></ToUserName><FromUserName><![CDATA[fromUser]]></FromUserName><CreateTime>1652502505</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[content]]></Content></xml>";
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null, "toUser")]
        [InlineData("", "toUser")]
        [InlineData("  ", "toUser")]
        [InlineData("fromUser", null)]
        [InlineData("fromUser", "")]
        [InlineData("fromUser", "  ")]
        public void BuildSearchReplayMessage_ShouldBeEmptyString_WhenFromUserOrToUserIsNullOrWhiteSpace(string fromUser, string toUser)
        {
            IWechatService wechatService = CreateWechatService();
            var books = new List<BookDto>
            {
                new BookDto()
                {
                    Id=1
                }
            };

            var actual = wechatService.BuildSearchReplayMessage(fromUser, toUser, books);
            Assert.Empty(actual);
        }

        [Fact]
        public void BuildSearchReplayMessage_ShouldBeNoBookXmlMessage_WhenGivenNullBooks()
        {
            IWechatService wechatService = CreateWechatService();
            var actual = wechatService.BuildSearchReplayMessage("fromUser", "toUser", null);
            string expected = "<xml><ToUserName><![CDATA[toUser]]></ToUserName><FromUserName><![CDATA[fromUser]]></FromUserName><CreateTime>1652502505</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[尴尬了，没有你想要的书籍！]]></Content></xml>";
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void BuildSearchReplayMessage_ShouldBeNoBookXmlMessage_WhenGivenEmptyBooks()
        {
            IWechatService wechatService = CreateWechatService();
            var books = new List<BookDto>();
            var actual = wechatService.BuildSearchReplayMessage("fromUser", "toUser", books);
            string expected = "<xml><ToUserName><![CDATA[toUser]]></ToUserName><FromUserName><![CDATA[fromUser]]></FromUserName><CreateTime>1652502505</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[尴尬了，没有你想要的书籍！]]></Content></xml>";
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void BuildSearchReplayMessage_ShouldBeBookListXmlMessage_WhenGivenMoreThenOneBook()
        {
            IWechatService wechatService = CreateWechatService();
            var books = new List<BookDto>()
            {
                new BookDto
                {
                    Id = 10001,
                    Name = "聪明的投资者",
                    OriginalName="The intelligent investor",
                    Author = "本杰明·格雷厄姆",
                    Translator="王中华 / 黄一义",
                    DoubanScore=8.9f
                },
                new BookDto
                {
                    Id = 10002,
                    Name = "投资最重要的事",
                    Author = "霍华德·马克斯"
                }
            };
            var actual = wechatService.BuildSearchReplayMessage("fromUser", "toUser", books);
            string expected = "<xml><ToUserName><![CDATA[toUser]]></ToUserName><FromUserName><![CDATA[fromUser]]></FromUserName><CreateTime>1652502505</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[我们帮你找到了如下书籍：\r\n编　号：10001\t评　分：8.9\r\n书　名：聪明的投资者\r\n原书名：The intelligent investor\r\n作　者：本杰明·格雷厄姆\r\n译　者：王中华 / 黄一义\r\n\r\n编　号：10002\r\n书　名：投资最重要的事\r\n作　者：霍华德·马克斯\r\n\r\n编辑发送【2:编号】就可以获取书籍下载链接啦!\r\n]]></Content></xml>";
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null, "toUser")]
        [InlineData("", "toUser")]
        [InlineData("  ", "toUser")]
        [InlineData("fromUser", null)]
        [InlineData("fromUser", "")]
        [InlineData("fromUser", "  ")]
        public void BuildDownloadReplayMessage_ShouldBeEmptyString_WhenFromUserOrToUserIsNullOrWhiteSpace(string fromUser, string toUser)
        {
            IWechatService wechatService = CreateWechatService();
            var book = new BookDto()
            {
                Id = 1
            };

            var actual = wechatService.BuildDownloadReplayMessage(fromUser, toUser, book);
            Assert.Empty(actual);
        }

        [Fact]
        public void BuildDownloadReplayMessage_ShouldBeNoBookXmlMessage_WhenGivenBookIsNull()
        {
            IWechatService wechatService = CreateWechatService();
            var actual = wechatService.BuildDownloadReplayMessage("fromUser", "toUser", null);
            string expected = "<xml><ToUserName><![CDATA[toUser]]></ToUserName><FromUserName><![CDATA[fromUser]]></FromUserName><CreateTime>1652502505</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[很遗憾，没有找到您想要的书籍！]]></Content></xml>";
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void BuildDownloadReplayMessage_ShouldBeDownloadXmlMessage_WhenGivenBookIsNotNull()
        {
            IWechatService wechatService = CreateWechatService();
            var book = new BookDto
            {
                Id = 1,
                DownloadUrl = "DownloadUrl",
                ExtractionCode = "ExtractionCode"
            };
            var actual = wechatService.BuildDownloadReplayMessage("fromUser", "toUser", book);
            string expected = "<xml><ToUserName><![CDATA[toUser]]></ToUserName><FromUserName><![CDATA[fromUser]]></FromUserName><CreateTime>1652502505</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[下载地址：DownloadUrl\r\n提 取 码：ExtractionCode\r\n很高兴能帮到您，祝您投资路上一片坦途！\r\n]]></Content></xml>";
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null, "toUser")]
        [InlineData("", "toUser")]
        [InlineData("  ", "toUser")]
        [InlineData("fromUser", null)]
        [InlineData("fromUser", "")]
        [InlineData("fromUser", "  ")]
        public void BuildDefaultReplayMessage_ShouldBeEmptyString_WhenFromUserOrToUserIsNullOrWhiteSpace(string fromUser, string toUser)
        {
            IWechatService wechatService = CreateWechatService();
            var actual = wechatService.BuildDefaultReplayMessage(fromUser, toUser);
            Assert.Empty(actual);
        }

        [Fact]
        public void BuildDefaultReplayMessage_ShouldBeDefaultXmlMessage_WhenFromUserAndToUserIsValid()
        {
            IWechatService wechatService = CreateWechatService();
            var actual = wechatService.BuildDefaultReplayMessage("fromUser", "toUser");
            string expected = "<xml><ToUserName><![CDATA[toUser]]></ToUserName><FromUserName><![CDATA[fromUser]]></FromUserName><CreateTime>1652502505</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[您的指令让我有些为难，你可以尝试如下形式：\r\n1、【1:书籍名称】：按书籍名称搜索相关书籍，不记得全名也能搜哦！\r\n2、【2:书籍编号】：获取书籍下载地址，编号可以通过【1:书籍名称】查询！\r\n0、【0:强烈建议】：别太难为人哦！\r\n祝您生活愉快，每天都能跳着踢踏舞去工作！\r\n]]></Content></xml>";
            Assert.Equal(expected, actual);
        }

        private IWechatService CreateWechatService()
        {
            TimeProvider timeProvider = Substitute.For<TimeProvider>();
            timeProvider.Now.Returns(new DateTime(2022, 5, 14, 12, 28, 25));
            IOptions<WechatOption> options = Substitute.For<IOptions<WechatOption>>();
            options.Value.Returns(new WechatOption
            {
                EncodingAESKey = "bLM1FertsdCrmJDOjWEQPQf8mEt22y5E69vTzX9YO1X=",
                Token = "touzimanmanlai"
            });

            ILogger<WechatService> logger = Substitute.For<ILogger<WechatService>>();
            return new WechatService(timeProvider, options, logger);
        }
    }
}
