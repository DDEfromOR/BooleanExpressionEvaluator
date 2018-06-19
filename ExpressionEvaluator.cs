using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooleanExpressionReader
{
    class Program
    {
        static void Main(string[] args)
        {
            var testCaseA = "TRUE OR FALSE AND ( ( TRUE OR FALSE ) AND TRUE ) OR ( TRUE AND FALSE )"; //TRUE
            var testCaseB = "TRUE AND FALSE AND ( ( TRUE OR FALSE ) AND TRUE ) OR ( TRUE AND FALSE )"; //FALSE
            var resultA = EvaluateExpression(testCaseA);
            var resultB = EvaluateExpression(testCaseB);
            Console.WriteLine("The result for test case A was: " + resultA.ToString());
            Console.WriteLine("The result for test case B was: " + resultB.ToString());
            Console.ReadKey();
        }

        //"TRUE OR FALSE AND ( ( TRUE OR FALSE ) AND TRUE ) OR ( TRUE AND FALSE )" -> TRUE
        //" TRUE AND ( ( TRUE ) AND TRUE ) OR ( FALSE )
        // TRUE OR FALSE AND TRUE OR FALSE AND TRUE OR TRUE AND FALSE
        static private bool EvaluateExpression (string expression)
        {
            Stack<string> expressionStack = new Stack<string>();
            foreach(string word in expression.Split(' '))
            {
                expressionStack.Push(word);
            }

            return Reduce(expressionStack) == "TRUE";
        }

        static private string Reduce(Stack<string> expression)
        {
            Stack<string> values = new Stack<string>();
            Stack<string> operators = new Stack<string>();

            while (expression.Count > 0)
            {
                string token = expression.Pop();

                if(token == "TRUE" || token == "FALSE")
                {
                    values.Push(token);
                }
                if(token == ")" || token == "OR" || token == "AND")
                {
                    operators.Push(token);
                }
                if(token == "(")
                {
                    while (operators.Peek() != ")")
                    {
                        values.Push(Eval(values.Pop(), values.Pop(), operators.Pop()));
                    }
                    operators.Pop();
                }
            }

            while (operators.Count > 0)
            {
                values.Push(Eval(values.Pop(), values.Pop(), operators.Pop()));
            }
            return values.Pop();
            
        }

        static private string Eval(string OpA, string OpB, string Operator)
        {
            /*
             * T || F -> T 
             * T || T -> T
             * F || F -> F
             * F || T -> T
             */
            if (Operator == "OR" && (OpA == "TRUE" || OpB == "TRUE"))
            {
                return "TRUE";
            }

            /*
             * T && F -> F
             * T && T -> T
             * F && F -> F
             * F && T -> F
             */
            if (Operator == "AND" && (OpA == "TRUE" && OpB == "TRUE"))
            {
                return "TRUE";
            }

            return "FALSE";
        }
    }
}

