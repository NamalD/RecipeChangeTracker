<script>
  import { getLatestRecipes } from "../Persistence/Recipes";
  const recipes = getLatestRecipes();
</script>

<h2>Your recipes</h2>

<ul>
  {#each recipes as recipe}
    <li>
      <a href="#a">{recipe}</a>
    </li>
  {/each}
</ul>
