using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Library;

namespace FoodApp
{
    public class FoodData
    {
        public RecipeProcessor processor;

        public FoodData()
        {
            HttpClient httpClient = RecipeProcessor.InitializeClient();
            processor = new RecipeProcessor(httpClient);

        }
        public async Task<Recipes> GetRecipes(string ingredients, string dietType)
        {
            Task<Recipes> allRecipes = processor.LoadRecipes(ingredients, dietType);
            var results = await allRecipes;
            return results;
        }

        public async Task<List<Summary>> GetSummaries(Recipes recipes)
        {
            List<Summary> summaries = new List<Summary>();
            foreach (var item in recipes.results)
            {
                Task<Summary> summary = processor.LoadSummary(item.id);
                var summaryResult = await summary;
                summaries.Add(summaryResult);
            }
            return summaries;
        }

        public async Task<Ingredients> GetIngredients(int id)
        {
            Ingredients ingredients =  await processor.LoadIngredients(id);
            return ingredients;
        }

        public async Task<Steps> GetSteps(int id)
        {
            Steps steps = await processor.LoadSteps(id);
            return steps;
        }
    }
}
