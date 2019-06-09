using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using WebApp.Helpers;
using Xunit;

namespace WebAppTests
{
    public class PasswordEncodeTests
    {
        private readonly PasswordEncode _encoder;
        private readonly Random _random;

        public PasswordEncodeTests()
        {
            _encoder = new PasswordEncode();
            _random = new Random();
        }

        [Fact]
        public void EncodedPasswordNotEqualToActual()
        {
            //Arrange
            var passwordToEncode = _random.Next(int.MaxValue).ToString();

            //Act
            var encodedPassword = _encoder.Encoder(passwordToEncode);

            //Assert
            Assert.NotEqual(passwordToEncode, encodedPassword);
        }

        [Fact]
        public void HashesShouldBeEqual()
        {
            var expectedHash = "d9b16061b0ecf9b515324ea4a3c54263724a6f7ca03705304a922151c9b2a34c";
            var passwordToEncode = "123456";

            var actualHash = _encoder.Encoder(passwordToEncode);

            Assert.Equal(expectedHash, actualHash);
        }

        [Fact]
        public void NumberOfHashCharacters()
        {
            var passwordToEncode = _random.Next(int.MaxValue).ToString();

            var actualHash = _encoder.Encoder(passwordToEncode);
            
            Assert.Equal(64, actualHash.Length);
        }
    }
}
