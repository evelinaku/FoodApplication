using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class Steps
    {
        public List<Step> steps { get; set; }

        public string GetStepsString()
        { 
            string stepsString;
            StringBuilder sb = new StringBuilder();
            foreach(var item in this.steps)
            {
                sb.Append("Step " + item.number.ToString() + ":   " + item.step + "\n");
            }
            stepsString = sb.ToString();
            return stepsString;
        }
    }

    public class Step
    {
       public int number { get; set; }
       public string step { get; set; }
    }



}
