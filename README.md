# Tests
using Microsoft.Extensions.Logging;
using ListCats.Controllers;
using ListCats.Repositories;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace FindCats.Test
{
    public class ControllerTests
    {
        private Mock<ICatRepo> catRepoMock;
        private Mock<ILogger<FetchCatController>> logger;
        [SetUp]
        public void Setup()
        {
           catRepoMock = new Mock<ICatRepo>();
           logger = new Mock<ILogger<FetchCatController>>();
           
        }

        [Test]
        public void Multiple_Names_Under_Same_Gender_ShouldReturn_Only_One_Name()
        {
            string s = @"[{""name"":""Bob"",""gender"":""Male"",""age"":23,""pets"":[{""name"":""Garfield"",""type"":""Cat""},{""name"":""Fido"",""type"":""Dog""},{""name"":""Garfield"",""type"":""Cat""}]}]";
            Dictionary<string, List<object>> expected = new Dictionary<string, List<object>>();
            expected.Add("Male", new List<object>() { "Garfield" });
            catRepoMock.Setup(c => c.ProcessRepositories()).ReturnsAsync(s);
            FetchCatController ctrl = new FetchCatController(logger.Object, catRepoMock.Object);
            var actionResult = ctrl.Get();
            var okObjectResult = actionResult.Result;
            Assert.NotNull(okObjectResult);
            var model = (okObjectResult as OkObjectResult).Value;
            Assert.NotNull(model); 
            Assert.AreEqual(expected, model);
        }
    }
}
