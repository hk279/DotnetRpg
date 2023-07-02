namespace dotnet_rpg;

public static class RNG
{
    private readonly static Random rng = new();

    public static bool GetBoolean(double probability)
    {
        if (probability < 0 || probability > 1) throw new ArgumentException("Probability must be between 0 and 1.");

        return rng.NextDouble() < probability;
    }

    public static int GetIntInRange(int min, int max)
    {
        if (min >= max) throw new ArgumentException("Invalid range: min must be less than max.");

        return rng.Next(min, max + 1);
    }

    public static T PickRandom<T>(List<T> items)
    {
        if (items == null) throw new ArgumentNullException(nameof(items), "The collection cannot be null.");

        var itemCount = items.Count;

        if (itemCount == 0) throw new ArgumentException("The collection must not be empty.");

        var randomIndex = rng.Next(itemCount);

        return items[randomIndex];
    }
}
