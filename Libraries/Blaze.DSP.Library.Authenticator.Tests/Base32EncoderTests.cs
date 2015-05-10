// The MIT License (MIT)
// 
// Copyright (c) 2015 Daniel Franklin. http://blazedsp.com/
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

namespace Blaze.DSP.Library.Authenticator.Tests
{
    using System;
    using System.Text;

    using Xunit;

    public class Base32EncoderTests
    {
        [Fact]
        public void ToBase32StringShouldReturnBase32EncodedString()
        {
            // Arrange
            const string data = "test string"; // EQ: ORSXG5BAON2HE2LOM4 / orsxg5baon2he2lom4 as Base32
            var bytes = Encoding.UTF8.GetBytes(data);

            // Act
            var result = Base32Encoder.ToBase32String(bytes);

            // Assert
            var viewResult = Assert.IsType<string>(result);
            Assert.Equal("ORSXG5BAON2HE2LOM4", viewResult);
            Assert.Equal("orsxg5baon2he2lom4", viewResult, true);
        }

        [Fact]
        public void FromBase32StringShouldReturnBase32DecodedBytesForLowercaseString()
        {
            // Arrange
            var bytes = Encoding.UTF8.GetBytes("test string");

            const string data = "orsxg5baon2he2lom4"; // EQ: 'test string' as bytes

            // Act
            var result = Base32Encoder.FromBase32String(data);

            // Assert
            var viewResult = Assert.IsType<byte[]>(result);
            Assert.Equal(bytes, viewResult);
        }

        [Fact]
        public void FromBase32StringShouldReturnBase32DecodedBytesForUppercaseString()
        {
            // Arrange
            var bytes = Encoding.UTF8.GetBytes("test string");

            const string data = "ORSXG5BAON2HE2LOM4"; // EQ: 'test string' as bytes

            // Act
            var result = Base32Encoder.FromBase32String(data);

            // Assert
            var viewResult = Assert.IsType<byte[]>(result);
            Assert.Equal(bytes, viewResult);
        }


        [Fact]
        public void FromBase32StringShould_DO_STUFF()
        {
            // Arrange
            var bytes = Encoding.UTF8.GetBytes("test string.");

            const string data = "ORSXG5BAON2HE2LOM4XA";

            // Act
            var result = Base32Encoder.FromBase32String(data);

            // Assert
            //var viewResult = Assert.IsType<byte[]>(result);
            //Assert.Equal(bytes, viewResult);
        }

        [Fact]
        public void FromBase32StringShouldThrowArgumentNullExceptionOnInputParamWhiteSpace()
        {
            // Arrange
            const string data = "";

            // Act


            // Assert
            Assert.Throws<ArgumentNullException>(() => Base32Encoder.FromBase32String(data));
        }

        [Fact]
        public void FromBase32StringShouldThrowArgumentNullExceptionOnInputParamNull()
        {
            // Arrange
            const string data = null;

            // Act


            // Assert
            Assert.Throws<ArgumentNullException>(() => Base32Encoder.FromBase32String(data));
        }

        [Fact]
        public void FromBase32StringShouldThrowArgumentExceptionOnInvalidBase32InputChar()
        {
            // Arrange
            const string data = "0"; // NOTE: 0 is an invalid char

            // Act


            // Assert
            Assert.Throws<ArgumentException>(() => Base32Encoder.FromBase32String(data));
        }
    }
}