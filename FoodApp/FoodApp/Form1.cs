using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using CheckComboBoxTest;
using Library;

namespace FoodApp
{
    public partial class Form1 : Form
    {
        private string[] dairyProducts = { "butter", "egg", "cheese", "milk", "sour cream", "yogurt" };
        private string[] vegetableProducts = { "onion", "garlic", "tomato", "lettuce", "cabbage", "cucumber", "pickle", "sweet pepper", "corn", "potato", "brocolli", "mushroom" };
        private string[] meatProducts = { "salmon", "bacon", "pork", "chicken", "chicken breast", "beef", "shrimps", "lamb", "duck" };
        private string[] grainProducts = { "rice", "pasta", "flour", "bread", "oats", "tortilla" };

        Selection selection = new Selection();
        Recipes recipes = new Recipes();
        FoodData foodData = new FoodData();
        
        public Form1()
        {
            InitializeComponent();
            this.Load += new EventHandler(this.Form1_Load);
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            panel2.Visible = false;
            panel7.Visible = false;
            panel8.Visible = false;
            this.fill_combo_box(dairyProducts, this.checkedComboBox1);
            this.fill_combo_box(vegetableProducts, this.checkedComboBox2);
            this.fill_combo_box(meatProducts, this.checkedComboBox3);
            this.fill_combo_box(grainProducts, this.checkedComboBox4);

        }

        private void fill_combo_box(string[] values, CheckComboBoxTest.CheckedComboBox comboBox)
        {
            for (int i = 0; i < values.Length; i++)
            {
                CCBoxItem item = new CCBoxItem(values[i], i);
                comboBox.Items.Add(item);
            }
        }

        //Next button click, after user selects the diet.
        private void button1_Click(object sender, EventArgs e)
        {

            var checkedButton = panel1.Controls.OfType<RadioButton>()
                                      .FirstOrDefault(r => r.Checked);
            string value = checkedButton.Name;
            string diet = "";
            panel2.Visible = true;

            switch (value)
            {
                case "regularButton":
                    diet = "";
                    this.panel3.Visible = true;
                    this.panel4.Visible = true;
                    this.panel5.Visible = true;
                    this.panel6.Visible = true;
                    this.panel6.Location = new Point(60, 370);
                    break;
                case "ketoButton":
                    diet = "ketogenic";
                    this.panel3.Visible = true;
                    this.panel4.Visible = true;
                    this.panel5.Visible = true;
                    this.panel6.Visible = false;
                    this.panel6.Location = new Point(96, 571);
                    break;
                case "paleoButton":
                    diet = "paleo";
                    this.panel3.Visible = true;
                    this.panel4.Visible = true;
                    this.panel5.Visible = true;
                    this.panel6.Visible = false;
                    this.panel6.Location = new Point(50, 571);
                    break;
                case "vegetarianButton":
                    diet = "vegetarian";
                    this.panel3.Visible = true;
                    this.panel4.Visible = true;
                    this.panel5.Visible = false;
                    this.panel6.Visible = true;
                    this.panel6.Location = new Point(60, 270);
                    break;
                default:
                    diet = "";
                    break;
            }

            this.selection.DietType = diet;


        }

        //Back button click, returns to diet selection
        private void button3_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
        }

        //Adds selected ingredients by user to the selection object
        private void ManageUserSelection()
        {
            foreach (CCBoxItem item in checkedComboBox1.CheckedItems)
            {
                selection.Ingredients.Add(item.Name);
            }
            foreach (CCBoxItem item in checkedComboBox2.CheckedItems)
            {
                selection.Ingredients.Add(item.Name);
            }
            foreach (CCBoxItem item in checkedComboBox3.CheckedItems)
            {
                selection.Ingredients.Add(item.Name);
            }
            foreach (CCBoxItem item in checkedComboBox4.CheckedItems)
            {
                selection.Ingredients.Add(item.Name);
            }
        }

        //Next button click, after user selects main ingredient
        private async void button2_Click(object sender, EventArgs e)
        {
            this.UseWaitCursor = true;
            this.ManageUserSelection();
            await this.AddRecipes();
            panel7.Visible = true;
            this.UseWaitCursor = false;
        }

        //Unchecks all combo boxes, in case user would like to try find other recipes
        private void UncheckAll()
        {
            for(int i=0; i<checkedComboBox1.Items.Count; i++)
            {
                checkedComboBox1.SetItemChecked(i, false);
            }
            for (int i = 0; i < checkedComboBox2.Items.Count; i++)
            {
                checkedComboBox2.SetItemChecked(i, false);
            }
            for (int i = 0; i < checkedComboBox3.Items.Count; i++)
            {
                checkedComboBox3.SetItemChecked(i, false);
            }
            for (int i = 0; i < checkedComboBox4.Items.Count; i++)
            {
                checkedComboBox4.SetItemChecked(i, false);
            }
        }

        //Adds recipes according to user selected diet and ingredients to the form
        private async Task AddRecipes()
        {

            string ingredients = selection.GetIngredients();
            string dietType = selection.DietType;

            Recipes allRecipes = await foodData.GetRecipes(ingredients, dietType);
            List<Summary> summaries = await foodData.GetSummaries(allRecipes);
            this.recipes = allRecipes;

            var recipeCount = allRecipes.results.Count;
            string pictureControlName;
            string labelControlName;
            string textBoxControlName;
            string buttonName;

            if (recipeCount > 0)
            { 
                label23.Visible = false;
                pictureBox14.Visible = false;
            
                for (int i=0; i<recipeCount; i++)
                {
                pictureControlName = "pictureBox" + (i+9).ToString();
                PictureBox picture = (PictureBox)this.Controls.Find(pictureControlName, true)[0];
                picture.Load(allRecipes.results[i].image);
                labelControlName = "label" + (i + 13).ToString();
                Label titleLabel = (Label)this.Controls.Find(labelControlName, true)[0];
                titleLabel.Text = allRecipes.results[i].title;
                titleLabel.Visible = true;
                textBoxControlName = "label" + (i + 18).ToString();
                Label textLabel = (Label)this.Controls.Find(textBoxControlName, true)[0];
                textLabel.Text = summaries[i].summary;
                textLabel.Visible = true;
                buttonName = "button" + (i + 5).ToString();
                Button moreButton = (Button)this.Controls.Find(buttonName, true)[0];
                moreButton.Tag = allRecipes.results[i].id;
                moreButton.Visible = true;
                 }
            }
            else
            {
                this.ShowNoRecipesMessage();

            }
        }

        //Adds recipe steps of selected recipe to the form
        private async Task AddSteps(int id)
        {
            Steps recipeSteps = await foodData.GetSteps(id);
            int counter = 0;
            Label stepsTitleLabel = new Label();
            stepsTitleLabel.AutoSize = true;
            stepsTitleLabel.Font = new System.Drawing.Font("Franklin Gothic Medium Cond", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            stepsTitleLabel.Location = new System.Drawing.Point(180, 10);
            stepsTitleLabel.Size = new System.Drawing.Size(43, 21);
            stepsTitleLabel.TabIndex = 0;
            stepsTitleLabel.Text = "Steps";
            panel10.Controls.Add(stepsTitleLabel);
            if (recipeSteps != null)
            {
                    Label numberLabel = new Label();
                    numberLabel.AutoSize = true;
                    numberLabel.Location = new System.Drawing.Point(15, 34);
                    numberLabel.TabIndex = 0;
                    numberLabel.Font = new System.Drawing.Font("Franklin Gothic Medium Cond", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    numberLabel.Text = recipeSteps.GetStepsString();
                    panel10.Controls.Add(numberLabel);
                    numberLabel.Name = "numberLabel" + counter.ToString();
                    numberLabel.MaximumSize = new Size(400, 0);
            }
        }

        //Adds ingredients of selected recipe to the form
        private async Task AddIngredients(int id)
        {
            Ingredients ingredientsResults = await foodData.GetIngredients(id);

            Label titleLabel = new Label();
            titleLabel.AutoSize = true;
            titleLabel.Font = new System.Drawing.Font("Franklin Gothic Medium Cond", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            titleLabel.Location = new System.Drawing.Point(125, 5);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new System.Drawing.Size(75, 5);
            titleLabel.TabIndex = 0;
            titleLabel.Text = "Ingredients";
            panel9.Controls.Add(titleLabel);
            int counter = 0;
            int height = 26;

            foreach (var item in ingredientsResults.ingredients)
            {
               
                Label ingredientLabel = new Label();
                ingredientLabel.AutoSize = true;
                ingredientLabel.MaximumSize = new Size(160, 0);
                ingredientLabel.Location = new System.Drawing.Point(70, height);
                ingredientLabel.TabIndex = 0;
                ingredientLabel.Font = new System.Drawing.Font("Franklin Gothic Medium Cond", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                ingredientLabel.Text = item.name;
                panel9.Controls.Add(ingredientLabel);
                ingredientLabel.Name = "ingLabel" + counter.ToString();
                Label valueLabel = new Label();
                valueLabel.AutoSize = true;
                valueLabel.Location = new System.Drawing.Point(170, height);
                valueLabel.TabIndex = 0;
                valueLabel.Font = new System.Drawing.Font("Franklin Gothic Medium Cond", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                valueLabel.Text = item.amount.metric.value.ToString() + " " + item.amount.metric.unit;
                panel9.Controls.Add(valueLabel);
                valueLabel.Name = "valueLabel" + counter.ToString();
                counter++;
                height = height + 15;

            }

            panel8.Visible = true;
        }

        //Returns a name of recipe
        private string GetName(int id)
        {
            string name;
            Recipe recipe =  this.recipes.results.Find(i => i.id == id);
            name = recipe.title;
            return name;
        }

        //Displays "No recipes" label and picture in case any recipes of given ingredients and diet weren't found.
        private void ShowNoRecipesMessage()
        {
            label12.Visible = false;
            label23.Visible = true;
            pictureBox14.Visible = true;
            label23.Text = "Sorry! We were unable to find this king of recipes. Please try again.\r\n";
            pictureBox14.Image = global::FoodApp.Properties.Resources._5299079;
        }

        //Clears all recipe panel controls
        private void ClearResults()
        {
            foreach (Control control in panel7.Controls)
            {

                if (control.GetType() == typeof(Label) && control.Name != "label12")
                {

                    control.Text = "";

                }
                else if(control.GetType() == typeof(PictureBox))
                {
                   PictureBox box = (PictureBox)control;
                    box.Image = null;
                }
                else if(control.GetType() == typeof(Button) && control.Name != "button4")
                {
                    control.Visible = false; 
                }

            }
        }

        //Try again button
        private void button4_Click(object sender, EventArgs e)
        {
            this.UncheckAll();
            this.ClearResults();
            Selection newSelection = new Selection();
            this.selection = newSelection;
            panel2.Visible = false;
            panel7.Visible = false;

        }

        //More button, displays steps and ingredients of selected recipe
        private async void moreButtonClick(object sender, EventArgs e)
        {
            Button button = sender as Button;
            int buttonId = (int)button.Tag;
            label24.Text = this.GetName(buttonId);
            await AddIngredients(buttonId);
            await AddSteps(buttonId);

        }

        //Back button, returns user to recipes window, user can select other recipes ingredients and steps.
        private void button10_Click(object sender, EventArgs e)
        {
            panel8.Visible = false;
            panel9.Controls.Clear();
            panel10.Controls.Clear();
        }
    }
}
