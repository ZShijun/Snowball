using Newtonsoft.Json.Linq;
using Snowball.Core.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Snowball.Core.UnitTests.Utils
{
    public class JsonUtilTests
    {
        private const string CamelCaseUserJson = "{\"name\":\"zs\",\"address\":{\"province\":\"广东省\",\"city\":\"深圳市\"},\"id\":1}";
        private const string PascalCaseUserJson = "{\"Name\":\"zs\",\"Address\":{\"Province\":\"广东省\",\"City\":\"深圳市\"},\"Id\":1}";

        [Theory]
        [InlineData(null)]
        public void Serialize_ShouldBeEmptyString_WhenGivenNull(string sourceString)
        {
            var resultJson = JsonUtil.Serialize(sourceString);
            Assert.Equal("", resultJson);
        }

        [Theory]
        [InlineData("")]
        [InlineData("a")]
        [InlineData(CamelCaseUserJson)]
        [InlineData(PascalCaseUserJson)]
        public void Serialize_ShouldBeSourceString_WhenGivenString(string sourceString)
        {
            var resultJson = JsonUtil.Serialize(sourceString);
            Assert.Equal(sourceString, resultJson);
        }

        [Theory]
        [InlineData(CamelCaseUserJson, true)]
        [InlineData(PascalCaseUserJson, false)]
        public void Serialize_ShouldBeExpectedCamelCaseUserJson_WhenGivenUserObjectAndCamelCase(string expectedJson, bool camelCase)
        {
            User user = MockUser();
            var resultJson = JsonUtil.Serialize(user, camelCase);
            Assert.Equal(expectedJson, resultJson);
        }

        [Theory]
        [InlineData(PascalCaseUserJson)]
        public void Serialize_ShouldBePascalCaseUserJson_WhenGivenUserObject(string expectedJson)
        {
            User user = MockUser();
            var resultJson = JsonUtil.Serialize(user);
            Assert.Equal(expectedJson, resultJson);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("abc")]
        public void Deserialize_ShouldBeNull_WhenGivenNullOrWhiteSpace(string jsonString)
        {
            User user = JsonUtil.Deserialize<User>(jsonString);
            Assert.Null(user);
        }

        [Theory]
        [InlineData("abc")]
        public void Deserialize_ShouldBeNull_WhenGivenInvalidString(string jsonString)
        {
            User user = JsonUtil.Deserialize<User>(jsonString);
            Assert.Null(user);
        }

        [Theory]
        [InlineData(CamelCaseUserJson)]
        [InlineData(PascalCaseUserJson)]
        public void Deserialize_ShouldBeUserObject_WhenGivenUserJsonString(string jsonString)
        {
            User user = JsonUtil.Deserialize<User>(jsonString);
            Assert.Equal("广东省", user.Address.Province);
        }

        [Fact]
        public void ToObject_ShouldBeNull_WhenGivenNull()
        {
            var user = JsonUtil.ToObject<User>(null);
            Assert.Null(user);
        }

        [Fact]
        public void ToObject_ShouldBeString_WhenGivenSimpleStringAndExpectString()
        {
            var str = JsonUtil.ToObject<string>("123");
            Assert.Equal("123", str);
        }

        [Fact]
        public void ToObject_ShouldBeDecimal_WhenGivenSimpleStringAndExpectDecimal()
        {
            var d = JsonUtil.ToObject<decimal>("123");
            Assert.Equal(123, d);
        }

        [Theory]
        [InlineData(CamelCaseUserJson)]
        [InlineData(PascalCaseUserJson)]
        public void ToObject_ShouldBeUser_WhenGivenUserJsonAndExpectUser(string userJson)
        {
            var user = JsonUtil.ToObject<User>(userJson);
            Assert.Equal(1, user.Id);
            Assert.Equal("zs", user.Name);
            Assert.Equal("广东省", user.Address.Province);
        }

        [Fact]
        public void ToObject_ShouldBeEmptyUser_WhenGivenEmptyObjectAndExpectUser()
        {
            var user = JsonUtil.ToObject<User>(new { });
            Assert.Equal(0, user.Id);
            Assert.Null(user.Name);
            Assert.Null(user.Address);
        }

        [Fact]
        public void ToObject_ShouldBeEmptyUser_WhenGivenAddressAndExpectUser()
        {
            var user = JsonUtil.ToObject<User>(new Address
            {
                Province = "广东省",
                City = "深圳市"
            });
            Assert.Equal(0, user.Id);
            Assert.Null(user.Name);
            Assert.Null(user.Address);
        }

        [Fact]
        public void ToObject_ShouldBeUser_WhenGivenObjectWithIdAndExpectUser()
        {
            var user = JsonUtil.ToObject<User>(new { id = 1 });
            Assert.Equal(1, user.Id);
            Assert.Null(user.Name);
            Assert.Null(user.Address);
        }

        [Fact]
        public void ToObject_ShouldBeUser_WhenGivenPersonAndExpectUser()
        {
            var user = JsonUtil.ToObject<User>(new Person { Id = 1 });
            Assert.Equal(1, user.Id);
            Assert.Null(user.Name);
            Assert.Null(user.Address);
        }

        [Fact]
        public void ToObject_ShouldBePerson_WhenGivenUserAndExpectPerson()
        {
            var person = JsonUtil.ToObject<Person>(MockUser());
            Assert.Equal(1, person.Id);
        }

        [Fact]
        public void ToObject_ShouldBeUser_WhenGivenJObjectWithIdAndExpectUser()
        {
            var user = JsonUtil.ToObject<User>(JObject.FromObject(new { id = 1 }));
            Assert.Equal(1, user.Id);
            Assert.Null(user.Name);
            Assert.Null(user.Address);
        }

        [Fact]
        public void ToObject_ShouldBeUserList_WhenGivenPersonListAndExpectUserList()
        {
            var users = JsonUtil.ToObject<List<User>>(new List<Person> {
            new Person { Id = 1 },
            new Person { Id = 2 }
            });

            Assert.Equal(2, users.Count);
        }

        [Fact]
        public void ToObject_ShouldBeUserList_WhenGivenJArrayWithIdAndExpectUserList()
        {
            var users = JsonUtil.ToObject<List<User>>(JArray.FromObject(new List<object> { new { id=1}, new { id = 2 } }));
            Assert.Equal(2, users.Count);
        }

        [Fact]
        public void IsJsonArray_ShouldBeTrue_WhenGivenNull()
        {
            var actual = JsonUtil.IsJsonArray(null);
            Assert.True(actual);
        }

        [Fact]
        public void IsJsonArray_ShouldBeFalse_WhenGivenSimpleString()
        {
            var actual = JsonUtil.IsJsonArray("abc");
            Assert.False(actual);
        }

        [Fact]
        public void IsJsonArray_ShouldBeFalse_WhenGivenUser()
        {
            var user = MockUser();
            var actual = JsonUtil.IsJsonArray(user);
            Assert.False(actual);
        }

        [Fact]
        public void IsJsonArray_ShouldBeTrue_WhenGivenUserList()
        {
            var users = new List<User> { MockUser() };
            var actual = JsonUtil.IsJsonArray(users);
            Assert.True(actual);
        }

        private User MockUser()
        {
            return new User
            {
                Id = 1,
                Name = "zs",
                Address = new Address
                {
                    Province = "广东省",
                    City = "深圳市"
                }
            };
        }
    }

    public class Person
    {
        public int Id { get; set; }
    }

    public class User : Person
    {
        public string Name { get; set; }

        public Address Address { get; set; }
    }

    public class Address
    {
        public string Province { get; set; }

        public string City { get; set; }
    }
}
