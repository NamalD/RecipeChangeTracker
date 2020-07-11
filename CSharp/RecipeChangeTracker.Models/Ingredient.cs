using Newtonsoft.Json;

namespace RecipeChangeTracker.CSharp.Models
{
    public class Ingredient
    {
        public int Quantity { get; }

        public string Unit { get; }

        public string Name { get; }

        public Ingredient(int quantity, string name)
        {
            Quantity = quantity;
            Name = name;
        }

        [JsonConstructor]
        public Ingredient(int quantity, string unit, string name)
        {
            Quantity = quantity;
            Name = name;
            Unit = unit;
        }
    }
}