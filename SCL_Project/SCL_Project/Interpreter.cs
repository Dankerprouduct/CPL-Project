using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace SCL_Project
{
    public class Interpreter: Expr.IVisitor<object>, Stmt.IVisitor<object>
    {

        public void Interpret(List<Stmt> statements)
        {
            try
            {
                foreach (Stmt statement in statements)
                {
                    Execute(statement);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        public void Interpret(Expr expression)
        {
            try
            {
                object value = Evaluate(expression);
                Console.WriteLine(Stringify(value));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Program.HadRuntimeError = true; 
            }
        }

        private void Execute(Stmt stmt)
        {
            stmt.Accept(this);
        }

        public string Stringify(object obj)
        {
            if (obj == null) return "null";

            if (obj is double)
            {
                string text = obj.ToString();
                if (text.EndsWith(".0"))
                {
                    text = text.Substring(0, text.Length - 2);
                }

                return text;
            }

            return obj.ToString();
        }

        private object Evaluate(Expr expr)
        {
            return expr.Accept(this);
        }
        public object VisitAssignExpr(Expr.Assign expr)
        {
            object value = Evaluate(expr.Value);
            Program.Environment.Assign(expr.Name, expr.Value);
            return value;
        }

        public object VisitBinaryExpr(Expr.Binary expr)
        {
            object left = Evaluate(expr.Left);
            object right = Evaluate(expr.Right);

            switch (expr.Operator.Type)
            {
                case TokenType.MINUS:
                    CheckNumberOperand(expr.Operator, right);
                    return (double) left - (double) right;
                case TokenType.DIV:
                    CheckNumberOperand(expr.Operator, right);
                    return (double)left / (double) right;
                case TokenType.MUL:
                    CheckNumberOperand(expr.Operator, right);
                    return (double) left * (double) right;
                case TokenType.PLUS:
                {
                    if (left is double && right is double)
                    {
                        return (double) left / (double) right;
                    }

                    if (left is string && right is string)
                    {
                        return (string) left + (string) right;
                    }

                    throw new Exception("Operands must be two number or two strings");

                    break;
                }
                case TokenType.GREATERTHAN:
                    CheckNumberOperand(expr.Operator, right);
                    return (double) left > (double) right;
                case TokenType.GREATERTHANEQUAL:
                    CheckNumberOperand(expr.Operator, right);
                    return (double) left >= (double) right;
                case TokenType.LESSTHAN:
                    CheckNumberOperand(expr.Operator, right);
                    return  (double)left < (double) right;
                case TokenType.LESSTHANEQUAL:
                    CheckNumberOperand(expr.Operator, right);
                    return (double) left <= (double) right;
                case TokenType.NOTEQUAL:
                    return !IsEqual(left, right);
                case TokenType.EQUALTO:
                    return IsEqual(left, right);
            }

            return null;
        }

        

        public object VisitCallExpr(Expr.Call expr)
        {
            throw new NotImplementedException();
        }

        public object visitGetExpr(Expr.Get expr)
        {
            throw new NotImplementedException();
        }

        public object visitGroupingExpr(Expr.Grouping expr)
        {
            return Evaluate(expr.Expression);
        }

        public object visitLiteralExpr(Expr.Literal expr)
        {
            return expr.Value;
        }

        public object visitLogicalExpr(Expr.Logical expr)
        {
            throw new NotImplementedException();
        }

        public object visitSetExpr(Expr.Set expr)
        {
            throw new NotImplementedException();
        }

        public object visitSuperExpr(Expr.Super expr)
        {
            throw new NotImplementedException();
        }

        public object visitThisExpr(Expr.This expr)
        {
            throw new NotImplementedException();
        }

        private void CheckNumberOperand(Token op, object operand)
        {
            if(operand is double) return;
            throw new Exception($"{operand} Operand must be a number"); 
        }


        public object visitUnaryExpr(Expr.Unary expr)
        {
            object right = Evaluate(expr.Right);
            switch (expr.Operator.Type)
            {
                case TokenType.MINUS:
                    return -(double) right;
                case TokenType.NOT:
                    return !IsTruthy(right);
            }

            //unreachable
            return null;
        }

        private bool IsEqual(object a, object b)
        {
            if(a == null && b == null)
                return true;
            if (a == null)
                return false;
            return a == b;
        }
        private bool IsTruthy(object obj)
        {
            if (obj == null) return false;
            if (obj is bool) return (bool)obj;
            return true;
        }
        public object visitVariableExpr(Expr.Variable expr)
        {
            return Program.Environment.Get(expr.Name);
        }

        // statements

        public object visitBlockStmt(Stmt.Block stmt)
        {
            ExecuteBlock(stmt.Statements, new Environment(Program.Environment));
            return null;
        }

        private void ExecuteBlock(List<Stmt> statements, Environment environment)
        {
            Environment previous = Program.Environment;
            try
            {
                Program.Environment = environment;
                foreach (Stmt stmt in statements)
                {
                    Execute(stmt);
                }
            }
            finally
            {
                Program.Environment = previous;
            }
        }

        public object visitClassStmt(Stmt.Class stmt)
        {
            throw new NotImplementedException();
        }

        public object visitExpressionStmt(Stmt.Expression stmt)
        {
            Evaluate(stmt.Expres);
            return null;
        }

        public object visitFunctionStmt(Stmt.Function stmt)
        {
            throw new NotImplementedException();
        }

        public object visitIfStmt(Stmt.If stmt)
        {
            throw new NotImplementedException();
        }

        public object visitDisplayStmt(Stmt.Display stmt)
        {
            object value = Evaluate(stmt.Expression); 
            Console.WriteLine(Stringify(value));
            return null;
        }

        public object visitReturnStmt(Stmt.Return stmt)
        {
            throw new NotImplementedException();
        }

        public object visitVarStmt(Stmt.Var stmt)
        {
            object value = null;
            if (stmt.Initializer != null)
            {
                value = Evaluate(stmt.Initializer); 
            }

            Program.Environment.Define(stmt.Name.Lexeme, value);
            return null;
        }

        public object visitImportStmt(Stmt.Import stmt)
        {

            return null;
        }
    }
}
