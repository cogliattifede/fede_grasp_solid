namespace Full_GRASP_And_SOLID;

public class Adapter : TimerClient
{
    private Recipe recipe;
    
    public Adapter(Recipe recipe)
    {
        this.recipe = recipe;
    }

    public void TimeOut()
    {
        this.recipe.MarkAsCooked();
    }
}