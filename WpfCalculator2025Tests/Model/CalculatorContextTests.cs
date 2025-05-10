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

            Assert.AreEqual(_context.DisplayText, "0");
        }

        [TestMethod()]
        public void 桁追加を初回行うと追加した桁の表示文字列になること()
        {
            var _context = new CalculatorContext();

            _context.AddDigit("1");

            Assert.AreEqual(_context.DisplayText, "1");
        }

        [TestMethod()]
        public void 桁追加を行うと追加した文字列が末尾に追加された表示文字列になること()
        {
            var _context = new CalculatorContext();

            _context.AddDigit("1");
            _context.AddDigit("2");
            _context.AddDigit("3");

            Assert.AreEqual(_context.DisplayText, "123");
        }

        [TestMethod()]
        public void 桁追加でゼロ表示に小数点を入力すると正しく表示文字列になること()
        {
            var _context = new CalculatorContext();
            _context.AddDigit(".");

            Assert.AreEqual(_context.DisplayText, "0.");
        }

        [TestMethod()]
        public void 桁追加でゼロ表示に小数点を連続入力すると正しく表示文字列になること()
        {
            var _context = new CalculatorContext();
            _context.AddDigit(".");
            _context.AddDigit(".");
            _context.AddDigit(".");
            _context.AddDigit(".");

            Assert.AreEqual(_context.DisplayText, "0.");
        }

        [TestMethod()]
        public void 全クリアを行うと表示文字列がゼロになること()
        {
            var _context = new CalculatorContext();
            _context.AddDigit("1");
            _context.AddDigit("2");

            _context.ClearAll();

            Assert.AreEqual(_context.DisplayText, "0");
        }

        [TestMethod()]
        public void 全クリアを行うと前入力値が空になること()
        {
            var _context = new CalculatorContext();
            _context.AddDigit("1");
            _context.AddDigit("2");

            _context.SetOperator("+");

            _context.ClearAll();

            Assert.AreEqual(_context.PreviousInput, "");
        }

        [TestMethod()]
        public void オペレターは初期未入力であること()
        {
            var _context = new CalculatorContext();

            Assert.AreEqual(_context.Operator, "");
        }

        [TestMethod()]
        public void オペレターは常に最後の入力値であること()
        {
            var _context = new CalculatorContext();

            _context.SetOperator("+");

            Assert.AreEqual(_context.Operator, "+");

            _context.SetOperator("-");

            Assert.AreEqual(_context.Operator, "-");
        }

        [TestMethod()]
        public void オペレターは全クリアで未入力になること()
        {
            var _context = new CalculatorContext();

            _context.SetOperator("+");

            _context.ClearAll();

            Assert.AreEqual(_context.Operator, "");
        }

        [TestMethod()]
        public void オペレータを入力すると現在入力値がある場合は現在入力値が前入力値にセットされること()
        {
            var _context = new CalculatorContext();

            _context.AddDigit("1");
            _context.AddDigit("2");

            _context.SetOperator("+");

            Assert.AreEqual(_context.PreviousInput, "12");
        }


        [TestMethod()]
        public void オペレータを入力すると現在入力値が無い場合は前入力値にゼロがセットされること()
        {
            var _context = new CalculatorContext();

            _context.SetOperator("+");

            Assert.AreEqual(_context.PreviousInput, "0");
        }

        [TestMethod()]
        public void オペレータを入力しても表示文字列は変わらないこと()
        {
            var _context = new CalculatorContext();

            _context.AddDigit("1");
            _context.AddDigit("1");
            _context.SetOperator("+");

            Assert.AreEqual(_context.DisplayText, "11");
        }

        [TestMethod()]
        public void オペレータを連続入力しても表示は変わらないこと()
        {
            var _context = new CalculatorContext();

            _context.AddDigit("1");
            _context.AddDigit("1");
            _context.SetOperator("+");

            _context.SetOperator("+");

            _context.SetOperator("+");

            Assert.AreEqual(_context.DisplayText, "11");
        }

        [TestMethod()]
        public void 計算可能状態でオペレーターを入力すると計算結果が表示されること()
        {
            var _context = new CalculatorContext();

            _context.AddDigit("1");
            _context.AddDigit("1");
            _context.SetOperator("+");

            _context.AddDigit("2");
            _context.SetOperator("+");

            // 13

            _context.SetOperator("+");

            // 13変わらず
            Assert.AreEqual(_context.DisplayText, "13");
        }

        [TestMethod()]
        public void オペレータを入力した後に桁追加すると表示文字列は入力値に変わること()
        {
            var _context = new CalculatorContext();

            _context.AddDigit("1");
            _context.AddDigit("1");
            _context.SetOperator("+");

            _context.AddDigit("2");

            Assert.AreEqual(_context.DisplayText, "2");
        }

        [TestMethod()]
        public void 演算実行で1回加算できること()
        {
            var _context = new CalculatorContext();

            _context.AddDigit("1");
            _context.SetOperator("+");
            _context.AddDigit("2");
            _context.Compute();

            Assert.AreEqual(_context.DisplayText, "3");
        }

        [TestMethod()]
        public void 演算実行結果に演算子を入力してさらに演算実行できること()
        {
            var _context = new CalculatorContext();
            _context.AddDigit("1");
            _context.SetOperator("+");
            _context.AddDigit("2");
            _context.Compute();

            // 3 +
            _context.SetOperator("+");

            // 3 + 5
            _context.AddDigit("5");
            _context.Compute();

            Assert.AreEqual(_context.DisplayText, "8");
        }

        [TestMethod()]
        public void 演算実行で連続演算を行うことができること()
        {
            var _context = new CalculatorContext();
            _context.AddDigit("1");
            _context.SetOperator("+");

            // 変わらない
            _context.AddDigit("2");

            // 3
            _context.Compute();

            // 3+2
            _context.Compute();

            // 5+2
            _context.Compute();

            Assert.AreEqual(_context.DisplayText, "7");
        }

        [TestMethod()]
        public void 演算実行で小数点を含んだ計算ができること()
        {
            var _context = new CalculatorContext();
            _context.AddDigit("1");
            _context.SetOperator("+");
            _context.AddDigit("2.3");

            _context.Compute();

            Assert.AreEqual(_context.DisplayText, "3.3");

            _context.ClearAll();

            _context.AddDigit(".2");
            _context.SetOperator("+");
            _context.AddDigit("3");

            _context.Compute();

            Assert.AreEqual(_context.DisplayText, "3.2");
        }

        [TestMethod()]
        public void 演算実行で小数点で終わる入力で計算ができること()
        {
            var _context = new CalculatorContext();
            _context.AddDigit("1.");
            _context.SetOperator("+");
            _context.AddDigit("2.");

            _context.Compute();

            Assert.AreEqual(_context.DisplayText, "3");
        }

        [TestMethod()]
        public void オペレータが未設定の状態で演算実行すると現在入力が結果になること()
        {
            var _context = new CalculatorContext();
            _context.AddDigit("1");
            _context.Compute();

            Assert.AreEqual(_context.DisplayText, "1");
        }

        [TestMethod()]
        public void オペレータを初期状態で連続入力すると表示文字列が初期値になること()
        {
            var _context = new CalculatorContext();
            _context.Compute();
            _context.Compute();
            _context.Compute();

            Assert.AreEqual(_context.DisplayText, "0");
        }

    }
}