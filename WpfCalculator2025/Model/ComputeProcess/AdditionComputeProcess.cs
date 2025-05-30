﻿namespace WpfCalculator2025.Model.ComputeProcess
{
    /// <summary>
    /// 加算
    /// </summary>
    class AdditionComputeProcess : IComputeProcess
    {
        public string Operator => "+";

        public decimal Compute(decimal x, decimal y)
        {
            return x + y;
        }
    }
}
