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
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Results;

    using Blaze.DSP.Library.Interfaces.SimpleInjector;
    using Blaze.DSP.Library.Models.Database;

    using Frontend.Controllers.Api;

    using Microsoft.AspNet.Identity;

    using NSubstitute;

    using Telerik.JustMock;

    using Xunit;

    public class CampaignsControllerTests
    {
        // GET
        [Fact]
        public async Task GetShouldReturnOkResult()
        {
            // Arrange
            var fakeSql = Substitute.For<IDatabase>();

            var fakeContext = Mock.Create<HttpContext>(Constructor.Mocked);
            Mock.Arrange(() => HttpContext.Current).Returns(fakeContext);

            var controller = new CampaignsController(fakeSql);

            // Act
            var result = await controller.Get();

            // Assert
            var viewResult = Assert.IsType<OkNegotiatedContentResult<IEnumerable<Campaign>>>(result);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task GetWithParamsShouldReturnOkResult()
        {
            // Arrange
            var fakeSql = Substitute.For<IDatabase>();

            var fakeContext = Mock.Create<HttpContext>(Constructor.Mocked);
            Mock.Arrange(() => HttpContext.Current).Returns(fakeContext);

            var controller = new CampaignsController(fakeSql);

            // Act
            var result = await controller.Get(1);

            // Assert
            var viewResult = Assert.IsType<OkNegotiatedContentResult<Campaign>>(result);
            Assert.NotNull(viewResult);
        }

        // POST
        [Fact]
        public async Task PostShouldReturnOkResultWhenResultGreaterThanOne()
        {
            // Arrange
            var model = new Campaign();

            var fakeContext = Mock.Create<HttpContext>(Constructor.Mocked);
            Mock.Arrange(() => HttpContext.Current).Returns(fakeContext);

            var fakeSql = Substitute.For<IDatabase>();
            fakeSql.CreateAsync(model).Returns(Task.FromResult(1));

            var controller = new CampaignsController(fakeSql);

            // Act
            var result = await controller.Post(model);

            // Assert
            var viewResult = Assert.IsType<OkNegotiatedContentResult<Campaign>>(result);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task PostShouldReturnBadRequestResultWhenResultLessThanOne()
        {
            // Arrange
            var model = new Campaign();

            var fakeContext = Mock.Create<HttpContext>(Constructor.Mocked);
            Mock.Arrange(() => HttpContext.Current).Returns(fakeContext);

            var fakeSql = Substitute.For<IDatabase>();
            fakeSql.CreateAsync(model).Returns(Task.FromResult(0));

            var controller = new CampaignsController(fakeSql);

            // Act
            var result = await controller.Post(model);

            // Assert
            var viewResult = Assert.IsType<BadRequestResult>(result);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task PostShouldReturnInvalidModelStateResultWhenInvalidModel()
        {
            // Arrange
            var model = new Campaign();

            var fakeSql = Substitute.For<IDatabase>();
            fakeSql.CreateAsync(model).Returns(Task.FromResult(1));

            var controller = new CampaignsController(fakeSql)
            {
                Configuration = new HttpConfiguration()
            };
            controller.ModelState.AddModelError("FakeError", "This is a Fake Error!");

            // Act
            var result = await controller.Post(model);

            // Assert
            var viewResult = Assert.IsType<InvalidModelStateResult>(result);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task PostShouldReturnOkResultWithModelDataUpdated()
        {
            // Arrange
            var model = new Campaign();

            var datetimeoffset = DateTimeOffset.UtcNow;
            Mock.Arrange(() => DateTimeOffset.UtcNow).Returns(datetimeoffset);

            var userid = Guid.NewGuid().ToString();
            var fakeContext = Mock.Create<HttpContext>(Constructor.Mocked);
            Mock.Arrange(() => HttpContext.Current).Returns(fakeContext);
            Mock.Arrange(() => HttpContext.Current.User.Identity.GetUserId()).Returns(userid);

            var fakeSql = Substitute.For<IDatabase>();
            fakeSql.CreateAsync(model).Returns(Task.FromResult(1));

            var controller = new CampaignsController(fakeSql);

            // Act
            var result = await controller.Post(model);

            // Assert
            var viewResult = Assert.IsType<OkNegotiatedContentResult<Campaign>>(result);
            Assert.Equal(viewResult.Content.UserId, userid);
            Assert.Equal(viewResult.Content.AddedDate, datetimeoffset);
            Assert.Equal(viewResult.Content.ModifiedDate, null);
        }

        // PUT
        [Fact]
        public async Task PutShouldReturnOkResultWhenResultTrue()
        {
            // Arrange
            var model = new Campaign();

            var fakeContext = Mock.Create<HttpContext>(Constructor.Mocked);
            Mock.Arrange(() => HttpContext.Current).Returns(fakeContext);

            var fakeSql = Substitute.For<IDatabase>();
            fakeSql.UpdateAsync(model).Returns(Task.FromResult(true));

            var controller = new CampaignsController(fakeSql);

            // Act
            var result = await controller.Put(1, model);

            // Assert
            var viewResult = Assert.IsType<OkNegotiatedContentResult<Campaign>>(result);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task PutShouldReturnBadRequestResultWhenResultFalse()
        {
            // Arrange
            var model = new Campaign();

            var fakeContext = Mock.Create<HttpContext>(Constructor.Mocked);
            Mock.Arrange(() => HttpContext.Current).Returns(fakeContext);

            var fakeSql = Substitute.For<IDatabase>();
            fakeSql.UpdateAsync(model).Returns(Task.FromResult(false));

            var controller = new CampaignsController(fakeSql);

            // Act
            var result = await controller.Put(1, model);

            // Assert
            var viewResult = Assert.IsType<BadRequestResult>(result);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task PutShouldReturnInvalidModelStateResultWhenInvalidModel()
        {
            // Arrange
            var model = new Campaign();

            var fakeSql = Substitute.For<IDatabase>();
            fakeSql.CreateAsync(model).Returns(Task.FromResult(1));

            var controller = new CampaignsController(fakeSql)
            {
                Configuration = new HttpConfiguration()
            };
            controller.ModelState.AddModelError("FakeError", "This is a Fake Error!");

            // Act
            var result = await controller.Put(1, model);

            // Assert
            var viewResult = Assert.IsType<InvalidModelStateResult>(result);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task PutShouldReturnReturnOkResultWithModelDataUpdated()
        {
            // Arrange
            var model = new Campaign { Id = 1 };

            var datetimeoffset = DateTimeOffset.UtcNow;
            Mock.Arrange(() => DateTimeOffset.UtcNow).Returns(datetimeoffset);

            var userid = Guid.NewGuid().ToString();
            var fakeContext = Mock.Create<HttpContext>(Constructor.Mocked);
            Mock.Arrange(() => HttpContext.Current).Returns(fakeContext);
            Mock.Arrange(() => HttpContext.Current.User.Identity.GetUserId()).Returns(userid);

            var fakeSql = Substitute.For<IDatabase>();
            fakeSql.UpdateAsync(model).Returns(Task.FromResult(true));

            var controller = new CampaignsController(fakeSql);

            // Act
            var result = await controller.Put(2, model);

            // Assert
            var viewResult = Assert.IsType<OkNegotiatedContentResult<Campaign>>(result);
            Assert.Equal(viewResult.Content.UserId, userid);
            Assert.Equal(viewResult.Content.ModifiedDate, datetimeoffset);
            Assert.Equal(viewResult.Content.Id, 2);
        }

        // DELETE
        [Fact]
        public async Task DeleteShouldReturnOkResultWhenResultTrue()
        {
            // Arrange
            var model = new Campaign();

            var fakeSql = Substitute.For<IDatabase>();
            fakeSql.DeleteAsync(model).ReturnsForAnyArgs(Task.FromResult(true));

            var controller = new CampaignsController(fakeSql);

            // Act
            var result = await controller.Delete(1);

            // Assert
            var viewResult = Assert.IsType<OkNegotiatedContentResult<int>>(result);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task DeleteShouldReturnBadRequestResultWhenResultFalse()
        {
            // Arrange
            var model = new Campaign();

            var fakeSql = Substitute.For<IDatabase>();
            fakeSql.DeleteAsync(model).ReturnsForAnyArgs(Task.FromResult(false));

            var controller = new CampaignsController(fakeSql);

            // Act
            var result = await controller.Delete(1);

            // Assert
            var viewResult = Assert.IsType<BadRequestResult>(result);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task DeleteShouldReturnOkResultWithModelId()
        {
            // Arrange
            var model = new Campaign();

            var fakeSql = Substitute.For<IDatabase>();
            fakeSql.DeleteAsync(model).ReturnsForAnyArgs(Task.FromResult(true));

            var controller = new CampaignsController(fakeSql);

            // Act
            var result = await controller.Delete(1);

            // Assert
            var viewResult = Assert.IsType<OkNegotiatedContentResult<int>>(result);
            Assert.Equal(viewResult.Content, 1);
        }
    }
}