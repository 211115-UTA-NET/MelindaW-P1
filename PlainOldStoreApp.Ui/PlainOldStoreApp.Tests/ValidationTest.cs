using Xunit;
using PlainOldStoreApp.Ui;
using System;

namespace PlainOldStoreApp.Tests
{
    public class ValidationTest
    {
        [Theory]
        [InlineData("melinda@g.com")]
        [InlineData("melinda@gmail.com")]
        [InlineData("example@gmail.com")]
        [InlineData("example@aol.com")]
        [InlineData("example@example.net")]
        [InlineData("e@e.n")]
        public void ValidationTestIsEmailOrName_IsEmail_True(string value)
        {
            //arrange
            Tuple<string, string> expected = new Tuple<string, string>("email", value.ToUpper());
            //act
            var result = ValidateInput.ValidateNameOrEmail(value);
            //assert
            Assert.Equal(result, expected);
        }

        [Theory]
        [InlineData("One Name")]
        [InlineData("name two")]
        [InlineData("s b")]
        [InlineData("this name")]
        [InlineData("Kim Hall")]
        [InlineData("James Doe")]
        public void ValidationTestIsEmailOrName_IsName_True(string value)
        {
            //arrange
            var firstName = value.Split(' ')[0].ToUpper();
            var lastName = value.Split(' ')[1].ToUpper();
            Tuple<string, string> expected = new Tuple<string, string>(firstName, lastName);
            //act
            var result = ValidateInput.ValidateNameOrEmail(value);
            //assert
            Assert.Equal(result, expected);
        }

        [Theory]
        [InlineData("melindag.com")]
        [InlineData("melinda@gmailcom")]
        [InlineData("example")]
        [InlineData("example@aol.")]
        [InlineData("example@.net")]
        [InlineData("a")]
        [InlineData("1")]
        [InlineData("m a w")]
        [InlineData("m & w")]
        public void ValidationTestIsEmailOrName_False(string value)
        {
            //arrange
            Tuple<string, string> expected = new Tuple<string, string>("false", "");
            //act
            var result = ValidateInput.ValidateNameOrEmail(value);
            //assert
            Assert.Equal(result, expected);
        }
    }
}