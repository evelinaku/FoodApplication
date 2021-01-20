//using Newtonsoft.Json;
using System;
using System.Collections.Generic;
//using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
//using System.Text;
using System.Threading.Tasks;
//using System.Web;
using System.Web.Script.Serialization;

namespace Library
{
    public class RecipeProcessor
    {
        public HttpClient httpClient;

        public RecipeProcessor(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public static HttpClient InitializeClient()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return httpClient;
           
        }
        public async Task<Summary> LoadSummary(int recipeNr = 1 )
        {
            string url = $"https://api.spoonacular.com/recipes/{ recipeNr }/summary/?apiKey=7b7f2d752ff041f4935b0b1ebd1ede27";

            using (HttpResponseMessage response = await this.httpClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    /*
                    Summary summary = await response.Content.ReadAsAsync<Summary>();
                    return summary;
                    */

                    var summaryJsonString = await response.Content.ReadAsStringAsync();
                    Summary summary = new JavaScriptSerializer().Deserialize<Summary>(summaryJsonString);

                    return summary;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }


        public async Task<Recipes> LoadRecipes(string ingredients, string diet )
        {
            string url = $"https://api.spoonacular.com/recipes/complexSearch?includeIngredients={ ingredients }&diet={ diet }&number=5&apiKey=7b7f2d752ff041f4935b0b1ebd1ede27";
            Console.WriteLine(url);

            using (HttpResponseMessage response = await this.httpClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    var recipesJsonString = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(recipesJsonString);
                    Recipes recipes = new JavaScriptSerializer().Deserialize<Recipes>(recipesJsonString);
                    
                    return recipes;
                }
                else
                 {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<Ingredients> LoadIngredients(int id)
        {
            string url = $"https://api.spoonacular.com/recipes/{ id }/ingredientWidget.json?apiKey=7b7f2d752ff041f4935b0b1ebd1ede27";

            using (HttpResponseMessage response = await this.httpClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    var ingredientsJsonString = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(ingredientsJsonString);
                    Ingredients ingredients = new JavaScriptSerializer().Deserialize<Ingredients>(ingredientsJsonString);

                    return ingredients;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<Steps> LoadSteps(int id)
        {
            string url = $"https://api.spoonacular.com/recipes/{ id }/analyzedInstructions/?apiKey=7b7f2d752ff041f4935b0b1ebd1ede27";

            using (HttpResponseMessage response = await this.httpClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    var stepsJsonString = await response.Content.ReadAsStringAsync();
                    var stepsArr = new JavaScriptSerializer().Deserialize<List<Steps>>(stepsJsonString);
                    var steps = stepsArr.Count == 1 ? stepsArr[0] : null;
                    return steps;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

    }
}
