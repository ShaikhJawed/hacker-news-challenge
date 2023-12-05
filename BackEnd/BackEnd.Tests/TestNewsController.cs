using BackEnd.Controllers;
using BackEnd.Models;
using BackEnd.Services;
using BackEnd.Tests.MockData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Tests
{

    public class TestNewsController
    {
        private NewsController _newsController;
        private Mock<INewsService> _newsServiceMock;
        private Mock<IConfiguration> _configurationMock;
        private Mock<IUriService> _uriServiceMock;
        private Mock<ICacheWrapper> _memoryCacheMock;

        [SetUp]
        public void Setup()
        {
            _newsServiceMock = new Mock<INewsService>();
            _configurationMock = new Mock<IConfiguration>();
            _uriServiceMock = new Mock<IUriService>();
            _memoryCacheMock = new Mock<ICacheWrapper>(); //new Mock<IMemoryCache>();
            _configurationMock.Setup(c => c.GetSection("Limits").GetSection("MaxPageSize").Value).Returns("50");
            _configurationMock.Setup(c => c.GetSection("Limits").GetSection("MinPageNumber").Value).Returns("1");
            _configurationMock.Setup(c => c.GetSection("Limits").GetSection("TopStories").Value).Returns("200");
            _configurationMock.Setup(c => c.GetSection("Limits").GetSection("CacheExpiryMinutes").Value).Returns("5");

            _newsController = new NewsController(_newsServiceMock.Object, _configurationMock.Object, _uriServiceMock.Object, _memoryCacheMock.Object);

            var request = new Mock<HttpRequest>();
            request.Setup(x => x.Scheme).Returns("http");
            request.Setup(x => x.Host).Returns(HostString.FromUriComponent("http://localhost:8080"));
            request.Setup(x => x.PathBase).Returns(PathString.FromUriComponent("/api"));

            var httpContext = Mock.Of<HttpContext>(_ =>
                _.Request == request.Object
            );

            //Controller needs a controller context 
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            _newsController.ControllerContext = controllerContext;

        }

        [Test]
        public async Task Get_NonCache_ReturnsPagedResponse()
        {
            // Arrange
            var pageNumber = 1;
            var pageSize = 10;
            var searchText = "sample";

            var fakeStories = StoryMockData.GetEmptyStories();
            
            
            _memoryCacheMock.Setup(m => m.IsAvailableInCache("NewsStories", out fakeStories)).Returns(false);
            _newsServiceMock.Setup(s => s.GetTopStories()).ReturnsAsync(new int[] { 38515779, 38505211, 38516123 });
            _newsServiceMock.Setup(s => s.GetStory(It.IsAny<int>())).ReturnsAsync(StoryMockData.GetStories(1).FirstOrDefault());

            // Act
            var result = await _newsController.Get(pageNumber, pageSize, searchText)as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public async Task Get_Cache_ReturnsPagedResponse()
        {
            // Arrange
            var pageNumber = 1;
            var pageSize = 10;
            var searchText = "sample";

            var fakeStories = StoryMockData.GetEmptyStories();


            _memoryCacheMock.Setup(m => m.IsAvailableInCache("NewsStories", out fakeStories)).Returns(true);
            _newsServiceMock.Setup(s => s.GetTopStories()).ReturnsAsync(new int[] { 38515779, 38505211, 38516123 });
            _newsServiceMock.Setup(s => s.GetStory(It.IsAny<int>())).ReturnsAsync(StoryMockData.GetStories(1).FirstOrDefault());

            // Act
            var result = await _newsController.Get(pageNumber, pageSize, searchText) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }
    }
}
