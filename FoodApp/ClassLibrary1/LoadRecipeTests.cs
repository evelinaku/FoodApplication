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

namespace Library.Tests
{
    public class LoadRecipeTests
    {
        [Fact]
        public async void ShouldReturnRecipes()
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{ \"offset\": 0, \"number\": 2, \"results\": [{\"id\": 716429, \"calories\": 584, \"carbs\": \"84g\", \"fat\": \"20g\", \"image\": \"https://spoonacular.com/recipeImages/716429-312x231.jpg\", \"imageType\": \"jpg\", \"protein\": \"19g\", \"title\": \"Pasta with Garlic, Scallions, Cauliflower & Breadcrumbs\" }, { \"id\": 715538, \"calories\": 521, \"carbs\": \"69g\", \"fat\": \"10g\", \"image\": \"https://spoonacular.com/recipeImages/715538-312x231.jpg\", \"imageType\": \"jpg\", \"protein\": \"35g\", \"title\": \"What to make for dinner tonight?? Bruschetta Style Pork & Pasta\" }], \"totalResults\": 86}"),
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
            var results = await processor.LoadRecipes("tomato,cheese", "keto");

            
            Recipe pastaRecipe = new Recipe(716429, "Pasta with Garlic, Scallions, Cauliflower & Breadcrumbs", 584, "https://spoonacular.com/recipeImages/716429-312x231.jpg");
            Recipe porkRecipe = new Recipe(715538, "What to make for dinner tonight?? Bruschetta Style Pork & Pasta", 521, "https://spoonacular.com/recipeImages/715538-312x231.jpg");
            Recipes recipes = new Recipes();
            List<Recipe> testRecipes = new List<Recipe>();
            testRecipes.Add(pastaRecipe);
            testRecipes.Add(porkRecipe);
            recipes.results = testRecipes;

            var testResults = JsonConvert.SerializeObject(recipes);
            var retrievedRecipes = JsonConvert.SerializeObject(results);
            
            Assert.Equal(testResults, retrievedRecipes);
            handlerMock.Protected().Verify(
               "SendAsync",
               Times.Exactly(1),
               ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
               ItExpr.IsAny<CancellationToken>());
        }

    }

}
