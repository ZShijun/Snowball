using Snowball.Core.Utils;
using System;
using System.Net;
using System.Text;
using Xunit;

namespace Snowball.Core.UnitTests.Utils
{
    public class SecurityUtilTests
    {
        private static readonly string _encodingAESKey = "UEswJHZLXlYyTXFRS1FJcERVVTRYaUxjeCNJM0B5MTk=";
        private static readonly string _iv = "M2Z3QzVWRGxlVW1vWk9pNA==";
        [Fact]
        public void Sha1_ShouldBeNull_WhenGivenNull()
        {
            var actual = SecurityUtil.Sha1(null);
            Assert.Null(actual);
        }

        [Theory]
        [InlineData("", "da39a3ee5e6b4b0d3255bfef95601890afd80709")]
        [InlineData("123456", "7c4a8d09ca3762af61e59520943dc26494f8941b")]
        public void Sha1_ShouldBe40LowerCaseHashString_WhenGivenString(string input, string expected)
        {
            var actual = SecurityUtil.Sha1(input);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("", "r83F/lYihXu5giV3/VXE/w==")]
        [InlineData("123456", "vksV7DrenRFyA/IUac7aSg==")]
        public void AesEncrypt_ShouldBeEncrypted_WhenGivenPlaintextAndEncodingAESKey(string plaintext, string encrypttext)
        {
            string result = SecurityUtil.AesEncrypt(plaintext, _encodingAESKey);
            Assert.Equal(encrypttext, result);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("", "j+0zOd8vsQzav3xiDkbA5w==")]
        [InlineData("123456", "OG5b/sHPhGQQ+9t7iOhU/Q==")]
        public void AesEncrypt_ShouldBeEncrypted_WhenGivenPlaintextAndKeyAndIv(string plaintext, string encrypttext)
        {
            string result = SecurityUtil.AesEncrypt(plaintext, _encodingAESKey, _iv);
            Assert.Equal(encrypttext, result);
        }

        [Fact]
        public void AesDecrypt_ShouldBeNull_WhenGivenNullAndEncodingAESKey()
        {
            byte[] buffer = SecurityUtil.AesDecrypt(null, _encodingAESKey);
            Assert.Null(buffer);
        }

        [Theory]
        [InlineData("r83F/lYihXu5giV3/VXE/w==", "")]
        [InlineData("vksV7DrenRFyA/IUac7aSg==", "123456")]
        public void AesDecrypt_ShouldBeDecrypted_WhenGivenEncryptedTextAndEncodingAESKey(string encryptedText, string plaintext)
        {
            byte[] buffer = SecurityUtil.AesDecrypt(encryptedText, _encodingAESKey);
            string result = Encoding.UTF8.GetString(buffer);
            Assert.Equal(plaintext, result);
        }

        [Fact]
        public void AesDecrypt_ShouldBeNull_WhenGivenNullAndKeyAndIv()
        {
            byte[] buffer = SecurityUtil.AesDecrypt(null, _encodingAESKey, _iv);
            Assert.Null(buffer);
        }

        [Theory]
        [InlineData("j+0zOd8vsQzav3xiDkbA5w==", "")]
        [InlineData("OG5b/sHPhGQQ+9t7iOhU/Q==", "123456")]
        public void AesDecrypt_ShouldBeDecrypted_WhenGivenEncryptedTextAndKeyAndIv(string encryptedText, string plaintext)
        {
            byte[] buffer = SecurityUtil.AesDecrypt(encryptedText, _encodingAESKey, _iv);
            string result = Encoding.UTF8.GetString(buffer);
            Assert.Equal(plaintext, result);
        }
    }
}
