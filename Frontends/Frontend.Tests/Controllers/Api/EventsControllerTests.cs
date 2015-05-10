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

namespace Frontend.Tests.Controllers.Api
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Http.Results;

    using Blaze.DSP.Library.Interfaces.SimpleInjector;

    using Frontend.Controllers.Api;

    using NSubstitute;

    using Xunit;

    using DatabaseEvent = Blaze.DSP.Library.Models.Database.Event;
    using EmberEvent = Blaze.DSP.Library.Models.Ember.Event;

    public class EventsControllerTests
    {
        // GET
        [Fact]
        public async Task GetShouldReturnOkResult()
        {
            // Arrange
            var fakeSql = Substitute.For<IDatabase>();

            var controller = new EventsController(fakeSql);

            // Act
            var result = await controller.Get();

            // Assert
            var viewResult = Assert.IsType<OkNegotiatedContentResult<IEnumerable<EmberEvent>>>(result);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task GetShouldReturnConvertedIpAdress()
        {
            // Arrange
            AutomapperConfig.Init();

            var ipAddress = IPAddress.Loopback;

            var modelList = new List<DatabaseEvent>
            {
                new DatabaseEvent
                {
                    //Id = 1,
                    
                    //UserId = Guid.NewGuid(),
                    //UserAgent = "UserAgent",
                    //UserLanguage = "UserLanguage",
                    UserHostAddress = ipAddress.GetAddressBytes(),
                    UserProxyAddress = ipAddress.GetAddressBytes(),

                    //Time = DateTimeOffset.UtcNow,

                    //Referer = "Referer",

                    //Flight = 1,
                    //Destination = 1
                }
            };

            var fakeSql = Substitute.For<IDatabase>();
            fakeSql.QueryAsync<DatabaseEvent>("SELECT * FROM Events")
                   .Returns(Task.FromResult<IEnumerable<DatabaseEvent>>(modelList));

            var controller = new EventsController(fakeSql);

            // Act
            var result = await controller.Get();

            // Assert
            var viewResult = Assert.IsType<OkNegotiatedContentResult<IEnumerable<EmberEvent>>>(result);
            Assert.NotNull(viewResult);
            Assert.True(viewResult.Content.All(a => a.UserHostAddress == ipAddress.ToString()));
            Assert.True(viewResult.Content.All(a => a.UserProxyAddress == ipAddress.ToString()));
        }

        // GET BY ID
        [Fact]
        public async Task GetByIdShouldReturnOkResult()
        {
            // Arrange
            AutomapperConfig.Init();

            var fakeSql = Substitute.For<IDatabase>();
            fakeSql.ReadAsync<DatabaseEvent>(1)
                   .Returns(Task.FromResult(new DatabaseEvent()));

            var controller = new EventsController(fakeSql);

            // Act
            var result = await controller.Get(1);

            // Assert
            var viewResult = Assert.IsType<OkNegotiatedContentResult<EmberEvent>>(result);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task GetByIdShouldReturnBadRequestErrorMessageResultOnNullFromDatabase()
        {
            // Arrange
            var fakeSql = Substitute.For<IDatabase>();
            fakeSql.ReadAsync<DatabaseEvent>(1)
                   .Returns(Task.FromResult<DatabaseEvent>(null));

            var controller = new EventsController(fakeSql);

            // Act
            var result = await controller.Get(1);

            // Assert
            var viewResult = Assert.IsType<BadRequestErrorMessageResult>(result);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task GetByIdShouldReturnConvertedIpAdress()
        {
            // Arrange
            AutomapperConfig.Init();

            var ipAddress = IPAddress.Loopback;

            var model = new DatabaseEvent
            {
                //Id = 1,

                //UserId = Guid.NewGuid(),
                //UserAgent = "UserAgent",
                //UserLanguage = "UserLanguage",
                UserHostAddress = ipAddress.GetAddressBytes(),
                UserProxyAddress = ipAddress.GetAddressBytes(),

                //Time = DateTimeOffset.UtcNow,

                //Referer = "Referer",

                //Flight = 1,
                //Destination = 1
            };

            var fakeSql = Substitute.For<IDatabase>();
            fakeSql.ReadAsync<DatabaseEvent>(1)
                   .Returns(Task.FromResult(model));

            var controller = new EventsController(fakeSql);

            // Act
            var result = await controller.Get(1);

            // Assert
            var viewResult = Assert.IsType<OkNegotiatedContentResult<EmberEvent>>(result);
            Assert.NotNull(viewResult);
            Assert.True(viewResult.Content.UserHostAddress == ipAddress.ToString());
            Assert.True(viewResult.Content.UserProxyAddress == ipAddress.ToString());
        }

        // GET BY FLIGHT
        [Fact]
        public async Task GetByFlightShouldReturnOkResult()
        {
            // Arrange
            var fakeSql = Substitute.For<IDatabase>();

            var controller = new EventsController(fakeSql);

            // Act
            var result = await controller.Get("Flight");

            // Assert
            var viewResult = Assert.IsType<OkNegotiatedContentResult<IEnumerable<EmberEvent>>>(result);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task GetByFlightShouldReturnBadRequestErrorMessageResultOnEmptyFlight()
        {
            // Arrange
            var fakeSql = Substitute.For<IDatabase>();

            var controller = new EventsController(fakeSql);

            // Act
            var result = await controller.Get(string.Empty);

            // Assert
            var viewResult = Assert.IsType<BadRequestErrorMessageResult>(result);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task GetByFlightShouldReturnConvertedIpAdress()
        {
            // Arrange
            AutomapperConfig.Init();

            var ipAddress = IPAddress.Loopback;

            var modelList = new List<DatabaseEvent>
            {
                new DatabaseEvent
                {
                    //Id = 1,
                    
                    //UserId = Guid.NewGuid(),
                    //UserAgent = "UserAgent",
                    //UserLanguage = "UserLanguage",
                    UserHostAddress = ipAddress.GetAddressBytes(),
                    UserProxyAddress = ipAddress.GetAddressBytes(),

                    //Time = DateTimeOffset.UtcNow,

                    //Referer = "Referer",

                    //Flight = 1,
                    //Destination = 1
                }
            };

            var fakeSql = Substitute.For<IDatabase>();
            fakeSql.QueryAsync<DatabaseEvent>("SELECT * FROM Events")
                   .Returns(Task.FromResult<IEnumerable<DatabaseEvent>>(modelList));

            var controller = new EventsController(fakeSql);

            // Act
            var result = await controller.Get("Flight");

            // Assert
            var viewResult = Assert.IsType<OkNegotiatedContentResult<IEnumerable<EmberEvent>>>(result);
            Assert.NotNull(viewResult);
            Assert.True(viewResult.Content.All(a => a.UserHostAddress == ipAddress.ToString()));
            Assert.True(viewResult.Content.All(a => a.UserProxyAddress == ipAddress.ToString()));
        }
    }
}