using Library;
using Nito.AsyncEx.Synchronous;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoodApp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1()); 

            /*ApiHelper.InitializeClient();
            //Console.WriteLine("Hello");
            var task = RecipeProcessor.LoadRecipes("tomato,onion", "vegetarian", 300);
            var result = task.WaitAndUnwrapException();
            foreach (var item in result.results)
            {
                Console.WriteLine("idtest: {0}, name: {1}", item.id, item.title);
            }
            */


            //Console.WriteLine(result.Data);
            /*
            Console.WriteLine(result.Data.ElementAt(0).title);
            Console.WriteLine(result.Data.ElementAt(0).image);
            Console.WriteLine(result.Data.ElementAt(1).id);
            Console.WriteLine(result.Data.ElementAt(1).title);
            Console.WriteLine(result.Data.ElementAt(1).image);
            */

            //foreach (Recipe rec in result.Data)
            //Console.WriteLine(result.title);
            //Console.WriteLine(result.summary);
            // var result = AsyncContext.Run(MyAsyncMethod);



        }


    }
}
