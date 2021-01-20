using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodApp
{
    public class Selection
    {
        public string DietType { get; set; }
        public List<string> Ingredients = new List<string>();

        public string GetIngredients()
        {
            string ingredientList = string.Join(",", this.Ingredients);
            return ingredientList;
        }
    }
}
