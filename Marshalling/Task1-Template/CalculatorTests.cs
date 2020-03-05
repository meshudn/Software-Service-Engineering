using Xunit;

namespace Task1
{
    public class CalculatorTests
    {
        [Fact]
        public void TestMultiply()
        {
            var c = new Calculator();
            var result = c.Multiply(3, 2);
            Assert.Equal(result, 6);
        }

        [Fact]
        public void TestDivide()
        {
            var c = new Calculator();
            var result = c.Divide(6, 3);
            Assert.Equal(result, 2);
        }

        [Fact]
        public void TestDivideByZero()
        {
            var c = new Calculator();
            var result = c.Divide(6, 0);
            Assert.Equal(result, double.NaN);
        }
    }
}
