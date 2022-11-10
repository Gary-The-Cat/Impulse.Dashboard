namespace Impulse.Dashboard.Tests;

public class SortTests_Unit
{
    [Theory]
    [InlineData(new int[] { }, new int[] { })]
    [InlineData(new[] { 4, 3, 2 }, new[] { 2, 3, 4 })]
    [InlineData(new[] { 1, -1 }, new[] { -1, 1, })]
    public void Sort_ShouldReturnSortedList_WhenProvidedAnyList(int[] input, int[] expectedOutput)
    {
        var result = Sorter.Sort(input);
        result.Should().BeEquivalentTo(expectedOutput);
    }
}
