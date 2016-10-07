using System;
using System.Collections.Specialized;

namespace Core.Common
{
    /// <summary>
    /// 表达式解析以及运算类
    /// </summary>
    public class FormulaCalculator
    {
        /// <summary>
        /// 最高运算级别常量
        /// </summary>
        protected const int MAX_LEVEL = 99;

        /// <summary>
        /// 获取运算符的级别
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private static int GetOperatorLevel(string o)
        {
            for (int i = 0; i < Level.Length; i++)
                if ((string)Level[i][0] == o)
                    return (int)Level[i][1];
            return -1;
        }

        /// <summary>
        /// 如果字符串是已运算符开头,则返回该运算符,否则返回null
        /// </summary>
        /// <param name="v">要检查的字符串</param>
        /// <returns></returns>
        private static string GetOperator(string v)
        {
            for (int i = 0; i < Level.Length; i++)
                if (v.StartsWith((string)Level[i][0]))
                    return (string)Level[i][0];
            return null;
        }

        #region 运算符与支持的函数
        private static object[][] Level = new object[][]
            {
                new object[]{",",0},
                new object[]{"=",1},
                new object[]{">=",1},
                new object[]{"<=",1},
                new object[]{"<>",1},
                new object[]{">",1},
                new object[]{"<",1},
                new object[]{"+",2},
                new object[]{"-",2},
                new object[]{"*",3},
                new object[]{"/",3},
                new object[]{"%",3},
                new object[]{"NEG",4},
                new object[]{"^",5},
                new object[]{"(",MAX_LEVEL},

                //数学函数
                new object[]{"ROUND(",MAX_LEVEL},
                new object[]{"TRUNC(",MAX_LEVEL},
                new object[]{"MAX(",MAX_LEVEL},
                new object[]{"MIN(",MAX_LEVEL},
                new object[]{"ABS(",MAX_LEVEL},
                new object[]{"SUM(",MAX_LEVEL},
                new object[]{"AVERAGE(",MAX_LEVEL},
                new object[]{"SQRT(",MAX_LEVEL},
                new object[]{"EXP(",MAX_LEVEL},
                new object[]{"LOG(",MAX_LEVEL},
                new object[]{"LOG10(",MAX_LEVEL},

                //三角函数
                new object[]{"SIN(",MAX_LEVEL},
                new object[]{"COS(",MAX_LEVEL},
                new object[]{"TAN(",MAX_LEVEL},

                //条件函数
                new object[]{"IF(",MAX_LEVEL},
                new object[]{"NOT(",MAX_LEVEL},
                new object[]{"AND(",MAX_LEVEL},
                new object[]{"OR(",MAX_LEVEL},
    };

        #endregion

        #region 解析表达式
        /// <summary>
        /// 运算符
        /// </summary>
        private string _opt = null;

        /// <summary>
        /// 运算符右边的表达式
        /// </summary>
        private string _expression = null;

        /// <summary>
        /// 运算符左边的值
        /// </summary>
        private string _leftValue;

        public static decimal[] CalculateExpression(string expression, NameValueCollection dataProvider)
        {
            FormulaCalculator calc = new FormulaCalculator(expression, dataProvider);
            decimal[] r = calc.Calculate();
            return r;
        }

        private NameValueCollection _data;

        public FormulaCalculator(string expression, NameValueCollection dataProvider)
        {
            _data = dataProvider;
            _expression = expression.ToUpper();
            if (GetIndex(_expression) != -1)
                throw new Exception("缺少\"(\"");
            Initialize();
        }

        /// <summary>初始化对象(将表达式拆分为左边的值、运算符和右边的表达式)</summary>
        private void Initialize()
        {
            string right;
            GetNext(_expression, out _leftValue, out _opt, out right);
            _expression = right;
        }

        /// <summary>
        /// 拆分表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <param name="left">左边的值</param>
        /// <param name="opt">运算符</param>
        /// <param name="right">右边的值</param>
        private void GetNext(string expression, out string left, out string opt, out string right)
        {
            right = expression;
            left = string.Empty;
            opt = null;

            while (right != string.Empty)
            {
                opt = GetOperator(right);
                if (opt != null)
                {
                    right = right.Substring(opt.Length, right.Length - opt.Length);
                    break;
                }
                else
                {
                    left += right[0];
                    right = right.Substring(1, right.Length - 1);
                }
            }
            right = right.Trim();
        }

        /// <summary>
        /// 计算表达式
        /// </summary>
        /// <returns>返回值(可能有多个,如逗号表达式会返回多个值)</returns>
        public decimal[] Calculate()
        {
            //如果运算符为空，则直接返回左边的值
            if (_opt == null)
            {
                decimal r = decimal.Zero;
                try
                {
                    r = decimal.Parse(_leftValue);
                }
                catch
                {
                    try
                    {
                        r = decimal.Parse(_data[_leftValue]);
                    }
                    catch
                    {
                        throw new Exception("错误的格式:" + _leftValue);
                    }
                }
                return new decimal[1] { r };
            }

            //判断是否是最高优先级的运算符(括号和函数)
            if (GetOperatorLevel(_opt) != MAX_LEVEL)
            {
                //四则运算符中，只有当-左边无值的时候是单目运算
                if (_opt != "-" && _leftValue == string.Empty)
                    throw new Exception("\"" + _opt + "\"运算符的左边需要值或表达式");
                if (_opt == "-" && _leftValue == string.Empty)
                    _opt = "NEG";
                if (_expression == string.Empty)
                    throw new Exception("\"" + _opt + "\"运算符的右边需要值或表达式");

                return CalculateTwoParms();
            }
            else
            {
                //括号和函数左边都不需要值
                if (_leftValue != string.Empty)
                    throw new Exception("\"" + _opt + "\"运算符的左边不需要值或表达式");
                return CalculateFunction();
            }
        }

        /// <summary>计算函数(括号运算符被当作函数计算,所有的函数必须已右括号结尾)</summary>        
        private decimal[] CalculateFunction()
        {
            //查找对应的右括号
            int inx = GetIndex(_expression);
            if (inx == -1)
                throw new Exception("缺少\")\"");

            string l = _expression.Substring(0, inx);
            //如果表达式已经完成，则返回计算结果，否则计算当前结果
            //并修改左值、运算符、右边表达式的值，然后调用Calculate继续运算
            if (inx == _expression.Length - 1)
                return Calc(_opt, l);
            else
            {
                string left, right, op;
                _expression = _expression.Substring(inx + 1, _expression.Length - inx - 1);
                GetNext(_expression, out left, out op, out right);
                decimal[] r = Calc(_opt, l);
                _leftValue = r[r.Length - 1].ToString();
                if (op == null)
                    throw new Exception("\")\"运算符的右边需要运算符");
                _opt = op;
                _expression = right;
                return Calculate();
            }
        }

        /// <summary>
        /// 获取第一个未封闭的右括号在字符串中的位置
        /// </summary>
        /// <param name="expression">传入的字符串</param>        
        private int GetIndex(string expression)
        {
            int count = 0;
            for (int i = 0; i < expression.Length; i++)
            {
                if (expression[i] == ')')
                {
                    if (count == 0)
                        return i;
                    else
                        count--;
                }
                if (expression[i] == '(')
                    count++;
            }
            return -1;
        }
        /// <summary>计算四则表达式</summary>
        private decimal[] CalculateTwoParms()
        {
            string left, right, op;
            GetNext(_expression, out left, out op, out right);
            decimal[] result, r;

            //如果下一个运算符的级别不大于当前运算符，则计算当前的值
            if (op == null || (GetOperatorLevel(_opt) >= GetOperatorLevel(op)))
                r = Calc(_opt, _leftValue, left);
            else
            {
                //如果下一个运算符的级别大于当前运算符
                string ex = left;
                //则一直找到低于当前运算符级别的运算符，然后将该运算符和当前运算符中间的表达式
                //提取出来，新构造一个对象，运算中间级别高的表达式的值
                //然后将新对爱的结果当作右边的值于当前的左值以及运算符进行运算
                while ((GetOperatorLevel(_opt) < GetOperatorLevel(op) && right != string.Empty))
                {
                    ex += op;
                    if (GetOperatorLevel(op) == MAX_LEVEL)
                    {
                        int pos = GetIndex(right);
                        ex += right.Substring(0, pos + 1);
                        right = right.Substring(pos + 1);
                    }
                    GetNext(right, out left, out op, out right);
                    ex += left;
                }
                FormulaCalculator calc = new FormulaCalculator(ex, _data);
                decimal[] rl = calc.Calculate();
                r = Calc(_opt, _leftValue, rl[rl.Length - 1].ToString());
            }
            //将上一步计算出来的结果作为当前的左值，然后将表达式剩下的部分作为当前的右边的表达式
            //然后将下一个运算符作为当前运算符，然后递归运算
            _leftValue = r[r.Length - 1].ToString();
            _opt = op;
            _expression = right;

            decimal[] rr = Calculate();
            result = new decimal[r.Length - 1 + rr.Length];
            for (int i = 0; i < r.Length - 1; i++)
                result[i] = r[i];
            for (int i = 0; i < rr.Length; i++)
                result[r.Length - 1 + i] = rr[i];

            return result;
        }
        #endregion

        #region 运算
        private decimal[] Calc(string opt, string expression)
        {
            FormulaCalculator calc = new FormulaCalculator(expression, _data);
            decimal[] values = calc.Calculate();

            decimal v = values[values.Length - 1];
            decimal r = decimal.Zero;
            switch (_opt)
            {
                case "(":
                    r = v;
                    break;
                case "ROUND(":
                    if (values.Length > 2)
                        throw new Exception("Round函数需要一个或两个参数!");
                    if (values.Length == 1)
                        r = decimal.Round(v, 0);
                    else
                        r = decimal.Round(values[0], (int)values[1]);
                    break;
                case "TRUNC(":
                    if (values.Length > 1)
                        throw new Exception("Trunc函数只需要一个参数!");
                    r = decimal.Truncate(v);
                    break;
                case "MAX(":
                    if (values.Length < 2)
                        throw new Exception("Max函数至少需要两个参数!");
                    r = values[0];
                    for (int i = 1; i < values.Length; i++)
                        if (values[i] > r)
                            r = values[i];
                    break;
                case "MIN(":
                    if (values.Length < 2)
                        throw new Exception("Min函数至少需要两个参数!");
                    r = values[0];
                    for (int i = 1; i < values.Length; i++)
                        if (values[i] < r)
                            r = values[i];
                    break;
                case "ABS":
                    if (values.Length > 1)
                        throw new Exception("Abs函数只需要一个参数!");
                    r = Math.Abs(v);
                    break;
                case "SUM(":
                    foreach (decimal d in values)
                        r += d;
                    break;
                case "AVERAGE(":
                    foreach (decimal d in values)
                        r += d;
                    r /= values.Length;
                    break;
                case "IF(":
                    if (values.Length != 3)
                        throw new Exception("IF函数需要三个参数!");
                    if (GetBoolean(values[0]))
                        r = values[1];
                    else
                        r = values[2];
                    break;
                case "NOT(":
                    if (values.Length != 1)
                        throw new Exception("NOT函数需要一个参数!");
                    if (GetBoolean(values[0]))
                        r = 0;
                    else
                        r = 1;
                    break;
                case "OR(":
                    if (values.Length < 1)
                        throw new Exception("OR函数至少需要两个参数!");
                    foreach (decimal d in values)
                        if (GetBoolean(d))
                            return new decimal[1] { 1 };
                    break;
                case "AND(":
                    if (values.Length < 1)
                        throw new Exception("AND函数至少需要两个参数!");
                    foreach (decimal d in values)
                        if (!GetBoolean(d))
                            return new decimal[1] { decimal.Zero };
                    r = 1;
                    break;
                case "SQRT(":
                    if (values.Length != 1)
                        throw new Exception("SQRT函数需要一个参数!");
                    r = (decimal)Math.Sqrt((double)v);
                    break;
                case "SIN(":
                    if (values.Length != 1)
                        throw new Exception("Sin函数需要一个参数!");
                    r = (decimal)Math.Sin((double)v);
                    break;
                case "COS(":
                    if (values.Length != 1)
                        throw new Exception("Cos函数需要一个参数!");
                    r = (decimal)Math.Cos((double)v);
                    break;
                case "TAN(":
                    if (values.Length != 1)
                        throw new Exception("Tan函数需要一个参数!");
                    r = (decimal)Math.Tan((double)v);
                    break;
                case "EXP(":
                    if (values.Length != 1)
                        throw new Exception("Exp函数需要一个参数!");
                    r = (decimal)Math.Exp((double)v);
                    break;
                case "LOG(":
                    if (values.Length > 2)
                        throw new Exception("Log函数需要一个或两个参数!");
                    if (values.Length == 1)
                        r = (decimal)Math.Log((double)v);
                    else
                        r = (decimal)Math.Log((double)values[0], (double)values[1]);
                    break;
                case "LOG10(":
                    if (values.Length != 1)
                        throw new Exception("Log10函数需要一个参数!");
                    r = (decimal)Math.Log10((double)v);
                    break;
            }
            return new decimal[1] { r };
        }

        private bool GetBoolean(decimal d)
        {
            return (int)d == 1;
        }

        private decimal[] Calc(string opt, string leftEx, string rightEx)
        {
            decimal r = decimal.Zero;
            decimal left = decimal.Zero;
            decimal right = 0;

            try
            {
                left = decimal.Parse(leftEx);
            }
            catch
            {
                if (opt != "NEG")
                {
                    try
                    {
                        left = decimal.Parse(_data[leftEx]);
                    }
                    catch
                    {
                        throw new Exception("错误的格式:" + leftEx);
                    }
                }
            }

            try
            {
                right = decimal.Parse(rightEx);
            }
            catch
            {
                try
                {
                    right = decimal.Parse(_data[rightEx]);
                }
                catch
                {
                    throw new Exception("错误的格式:" + leftEx);
                }
            }

            switch (_opt)
            {
                case "NEG":
                    r = decimal.Negate(right);
                    break;
                case "+":
                    r = left + right;
                    break;
                case "-":
                    r = left - right;
                    break;
                case "*":
                    r = left * right;
                    break;
                case "/":
                    r = left / right;
                    break;
                case "%":
                    r = decimal.Remainder(left, right);
                    break;
                case "^":
                    r = (decimal)Math.Pow((double)left, (double)right);
                    break;
                case ",":
                    return new decimal[2] { left, right };
                case "=":
                    r = left == right ? 1 : 0;
                    break;
                case "<>":
                    r = left != right ? 1 : 0;
                    break;
                case "<":
                    r = left < right ? 1 : 0;
                    break;
                case ">":
                    r = left > right ? 1 : 0;
                    break;
                case ">=":
                    r = left >= right ? 1 : 0;
                    break;
                case "<=":
                    r = left <= right ? 1 : 0;
                    break;
            }
            return new decimal[1] { r };
        }
        #endregion


    }

    //使用方法： 
    //1、不含变量：
    //string expression = "1+32*9+Round(12*(1+9))";
    //decimal[] results = Calculator.CalculateExpression( expression,null);
    //results[0]就是返回值

    //2、含变量：
    //string expression = "a+23*b";
    //NameValueCollection parameters = new NameValueCollection();
    //parameters.Add("a","12.234");
    //parameters.Add("b","34");
    //decimal[] results = Calculator.CalculateExpression( expression,parameters);
    //results[0]返回值
}
