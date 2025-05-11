namespace WpfCalculator2025.Model.ComputeProcess
{
    /// <summary>
    /// 除算
    /// </summary>
    class DivisionComputeProcess : IComputeProcess
    {
        public string Operator => "/";

        public decimal Compute(decimal x, decimal y)
        {
            if (y == 0)
            {
                throw new ArgumentException("ゼロで割ることはできません");
            }

            return x / y;
        }
    }
}
