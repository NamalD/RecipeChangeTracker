using FluentAssertions;
using NUnit.Framework;
using RecipeChangeTracker.CSharp.Models;
using RecipeChangeTracker.Models;
using RecipeChangeTracker.Tree;
using System;
using System.Collections.Generic;

namespace Tests
{
    public class Tests
    {
        private RecipeTree _recipeTree;

        [SetUp]
        public void TestSetup()
        {
            _recipeTree = new RecipeTree("test");
        }

        [Test]
        public void Update_NewRecipe_SetsHeadToRecipe()
        {
            var newRecipe = new Recipe(
                "Sugar Mouth",
                TimeSpan.FromMinutes(30),
                new List<Ingredient>
                {
                    new Ingredient(5, "grams", "Sugar")
                },
                new List<string>
                {
                    "Pour sugar into mouth"
                });

            _recipeTree.Update(newRecipe);

            _recipeTree.Head.Recipe.Should().Be(newRecipe);
        }

        [Test]
        public void Update_UpdatedRecipe_SetsPreviousState()
        {
            var oldRecipe = new Recipe(
                "Beetroot",
                TimeSpan.FromMinutes(60),
                new List<Ingredient>
                {
                    new Ingredient(1, "Beetroot")
                },
                new[]
                {
                    "Put beetroot in oven",
                    "Cook for 60 minutes"
                });

            var newRecipe = oldRecipe.With(
                cookTime: TimeSpan.FromMinutes(90),
                steps: new[]
                {
                    "Put beetroot in oven",
                    "Cook for 90 minutes"
                });

            _recipeTree.Update(oldRecipe);
            _recipeTree.Update(newRecipe);

            _recipeTree.Head.PreviousState.Recipe.Should().Be(oldRecipe);
            _recipeTree.Head.Recipe.Should().Be(newRecipe);
        }
    }
}