namespace Task1
{
    public class Calculator : ICalculator
    {
        public double Multiply(double lhs, double rhs)
        {
            return lhs * rhs;
        }

        public double Divide(double lhs, double rhs)
        {
            if (rhs == 0)
            {
                return double.NaN;
            }
            return lhs / rhs;
        }
    }
}
