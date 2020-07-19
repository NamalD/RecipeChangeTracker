<script>
  import { getLatestRecipes, getLatestRecipesAsync } from "../Persistence/Recipes";
  import App from "../App.svelte";
  let recipesPromise = getLatestRecipesAsync();
</script>

<h2>Your recipes</h2>

<ul>
  {#await recipesPromise}
    <p>Loading recipes...</p>
  {:then response}
    <!-- {#each recipes as recipe}
      <li>
        <a href="#a">{recipe}</a>
      </li>
    {/each} -->
    <p>{response}</p>
  {:catch error}
      <p>Couldn't load recipes :(</p>
  {/await}
</ul>
