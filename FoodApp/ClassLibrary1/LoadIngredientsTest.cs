using System;
using Xunit;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Library.Class
{
    public class LoadIngredientsTest
    {
        [Fact]
        public async void ShouldReturnIngredients()
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{ \"ingredients\": [ { \"amount\": { \"metric\": { \"unit\": \"g\", \"value\": 222.0 }, \"us\": { \"unit\": \"cups\", \"value\": 1.5 } }, \"image\": \"blueberries.jpg\", \"name\": \"blueberries\" }, { \"amount\": { \"metric\": { \"unit\": \"\", \"value\": 1.0 }, \"us\": { \"unit\": \"\", \"value\": 1.0 } }, \"image\": \"egg-white.jpg\", \"name\": \"egg white\" } ]}"),
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
            var results = await processor.LoadIngredients(1003464);

            Ingredients testIngredients = new Ingredients();
            List<Ingredient> testIngredientsList = new List<Ingredient>();

            Ingredient testFirstIngredient = new Ingredient();
            Us firstIngredientUs = new Us();
            Metric firstIngredientMetric = new Metric();
            firstIngredientUs.unit = "cups";
            firstIngredientUs.value = 1.5;
            firstIngredientMetric.unit = "g";
            firstIngredientMetric.value = 222.0;
            Amount testFirstAmount = new Amount();
            testFirstAmount.metric = firstIngredientMetric;
            testFirstAmount.us = firstIngredientUs;
            testFirstIngredient.amount = testFirstAmount;
            testFirstIngredient.name = "blueberries";
            testFirstIngredient.image = "blueberries.jpg";

            Ingredient testSecondIngredient = new Ingredient();
            Us secondIngredientUs = new Us();
            Metric secondIngredientMetric = new Metric();
            secondIngredientUs.unit = "";
            secondIngredientUs.value = 1.0;
            secondIngredientMetric.unit = "";
            secondIngredientMetric.value = 1.0;
            Amount testSecondAmount = new Amount();
            testSecondAmount.metric = secondIngredientMetric;
            testSecondAmount.us = secondIngredientUs;
            testSecondIngredient.amount = testSecondAmount;
            testSecondIngredient.name = "egg white";
            testSecondIngredient.image = "egg-white.jpg";

            testIngredientsList.Add(testFirstIngredient);
            testIngredientsList.Add(testSecondIngredient);
            testIngredients.ingredients = testIngredientsList;

            var testIngredientsResults = JsonConvert.SerializeObject(testIngredients);
            var retrievedIngredients = JsonConvert.SerializeObject(results);

            Assert.Equal(testIngredientsResults, retrievedIngredients);
            handlerMock.Protected().Verify(
               "SendAsync",
               Times.Exactly(1),
               ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
               ItExpr.IsAny<CancellationToken>());
        }
    }
}
