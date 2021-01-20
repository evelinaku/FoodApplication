using Xunit;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Library.Tests
{
    public class LoadStepsTests
    {
        [Fact]
        public async void ShouldReturnSteps()
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("[ { \"name\": \"\", \"steps\": [ { \"equipment\": [ { \"id\": 404784, \"image\": \"oven.jpg\", \"name\": \"oven\", \"temperature\": { \"number\": 200.0, \"unit\": \"Fahrenheit\" } } ], \"ingredients\": [], \"number\": 1,  \"step\": \"Preheat the oven to 200 degrees F.\" }, { \"equipment\": [ { \"id\": 404661, \"image\": \"whisk.png\", \"name\": \"whisk\" }, { \"id\": 404783, \"image\": \"bowl.jpg\",\"name\": \"bowl\" } ], \"ingredients\": [ { \"id\": 19334, \"image\": \"light-brown-sugar.jpg\", \"name\": \"light brown sugar\" }, { \"id\": 19335, \"image\": \"sugar-in-bowl.png\", \"name\": \"granulated sugar\" }, { \"id\": 18371, \"image\": \"white-powder.jpg\", \"name\": \"baking powder\" }, { \"id\": 18372, \"image\": \"white-powder.jpg\", \"name\": \"baking soda\" }, { \"id\": 12142, \"image\": \"pecans.jpg\", \"name\": \"pecans\" }, { \"id\": 20081, \"image\": \"flour.png\", \"name\": \"all purpose flour\"  }, { \"id\": 2047, \"image\": \"salt.jpg\", \"name\": \"salt\" } ], \"number\": 2, \"step\": \"Whisk together the flour, pecans, granulated sugar, light brown sugar, baking powder, baking soda, and salt in a medium bowl.\" } ] }]"),
            };

            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(response);
            var httpClient = new HttpClient(handlerMock.Object);

            RecipeProcessor processor = new RecipeProcessor(httpClient);
            var results = await processor.LoadSteps(324694);

            Steps testSteps = new Steps();
            List<Step> testStepsList = new List<Step>();
            Step testFirstStep = new Step();
            testFirstStep.number = 1;
            testFirstStep.step = "Preheat the oven to 200 degrees F.";
            Step testSecodStep = new Step();
            testSecodStep.number = 2;
            testSecodStep.step = "Whisk together the flour, pecans, granulated sugar, light brown sugar, baking powder, baking soda, and salt in a medium bowl.";
            testStepsList.Add(testFirstStep);
            testStepsList.Add(testSecodStep);
            testSteps.steps = testStepsList;



            var testStepsResults = JsonConvert.SerializeObject(testSteps);
            var retrievedResults = JsonConvert.SerializeObject(results);

            Assert.Equal(testStepsResults, retrievedResults);
            handlerMock.Protected().Verify(
               "SendAsync",
               Times.Exactly(1),
               ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
               ItExpr.IsAny<CancellationToken>());
        }
    }
}
