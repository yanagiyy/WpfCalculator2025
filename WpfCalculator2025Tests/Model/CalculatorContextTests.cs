using Microsoft.VisualStudio.TestTools.UnitTesting;
using WpfCalculator2025.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfCalculator2025.Model.Tests
{
    [TestClass()]
    public class CalculatorContextTests
    {
        [TestMethod()]
        [TestCategory("CalculatorContext.DisplayText")]
        public void 表示文字列は初期表示がゼロであること()
        {
            var _context = new CalculatorContext();

            Assert.AreEqual(_context.DisplayText , "0");
        }
    }
}