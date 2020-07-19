export async function getLatestRecipesAsync() {
  const response = await fetch("https://localhost:5001/recipes");

  if (response.ok) {
    return response.json();
  }
  else {
    console.error("Get recipes failed", response);
    return "Could not load recipes :(";
  }
}