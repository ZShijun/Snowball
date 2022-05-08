using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
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

        [Fact]
        public void BuildNormalTextMessage()
        {
            //IWechatService wechatService = CreateWechatService();
            //string actual = wechatService.BuildNormalTextMessage("fromUser", "toUser", "content");
            //string expected = "<xml><ToUserName><![CDATA[toUser]]></ToUserName><FromUserName><![CDATA[fromUser]]></FromUserName><CreateTime>1652008118</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[content]]></Content></xml>";
            //Assert.Equal(expected, actual);
        }

        private IWechatService CreateWechatService()
        {
            IOptions<WechatOption> options = Substitute.For<IOptions<WechatOption>>();
            options.Value.Returns(new WechatOption
            {
                EncodingAESKey = "bLM1FertsdCrmJDOjWEQPQf8mEt22y5E69vTzX9YO1X=",
                Token = "touzimanmanlai"
            });

            ILogger<WechatService> logger = Substitute.For<ILogger<WechatService>>();
            return new WechatService(options, logger);
        }
    }
}
