namespace AutoLot.Dal.Tests;

public class SampleTests
{
    [Fact]
    public void SimpleTestSum()
    {
        Assert.Equal(5, 2 + 3);
    }

    [Theory]
    [InlineData(2, 3, 5)]
    [InlineData(1, -1, 0)]
    public void SimpleTheoryTestSum(int addend1, int addend2, int expectedResult)
    {
        Assert.Equal(expectedResult, addend1 + addend2);
    }
}
