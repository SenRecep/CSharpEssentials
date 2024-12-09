using System.Runtime.InteropServices;

namespace CSharpEssentials;
public static class RandomItemsExtensions
{
    public static T GetRandomItem<T>(this Span<T> source, Random? random = null)
    {
        random ??= Random.Shared;
        return source[random.Next(0, source.Length)];
    }
    public static T[] GetRandomItems<T>(this Span<T> source, int count, Random? random = null)
    {
        random ??= Random.Shared;
        int sourceLength = source.Length;
        if (count >= sourceLength)
        {
            T[] result = new T[sourceLength];
            source.CopyTo(result);
            random.Shuffle(result);
            return result;
        }

        Span<bool> selectedIndices = stackalloc bool[sourceLength];
        T[] resultArray = new T[count];
        int index = 0;

        while (index < count)
        {
            int randomIndex = random.Next(0, sourceLength);
            if (selectedIndices[randomIndex].IsFalse())
            {
                selectedIndices[randomIndex] = true;
                resultArray[index++] = source[randomIndex];
            }
        }

        return resultArray;
    }

    public static T[] GetRandomItems<T>(this List<T> source, int count, Random? random = null) =>
        CollectionsMarshal.AsSpan(source).GetRandomItems(count, random);

    public static T GetRandomItem<T>(this List<T> source, Random? random = null) =>
        CollectionsMarshal.AsSpan(source).GetRandomItem(random);

    public static T[] GetRandomItems<T>(this T[] source, int count, Random? random = null) =>
        source.AsSpan().GetRandomItems(count,random);

    public static T GetRandomItem<T>(this T[] source, Random? random = null) =>
        source.AsSpan().GetRandomItem(random);

}
