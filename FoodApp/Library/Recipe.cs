using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class Recipes
    {
        public List<Recipe> results { get; set; }

        public Recipes()
        {

        }
    }

    public class Recipe
    {
        public int id { get; set; }
        public string title { get; set; }
        public int calories { get; set; }
        public string image { get; set; }

        public Recipe(int id, string title, int calories, string image)
        {
            this.id = id;
            this.title = title;
            this.calories = calories;
            this.image = image;

        }

        public Recipe()
        {

        }
    }
}
