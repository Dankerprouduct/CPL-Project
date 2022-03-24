using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCL_Project
{
    internal class ASTPrinter: Expr.IVisitor<string>, Stmt.IVisitor<string>
    {
        String print(Expr expr)
        {
            return expr.Accept(this);
        }
        //> Statements and State omit

        String print(Stmt stmt)
        {
            return stmt.Accept(this);
        }

        public string Print(Expr expr)
        {
            return expr.Accept(this);
        }

        public string Print(Stmt stmt)
        {
            return stmt.Accept(this); 
        }

        public string VisitAssignExpr(Expr.Assign expr)
        {
            throw new NotImplementedException();
        }

        public string VisitBinaryExpr(Expr.Binary expr)
        {
            return parenthesize(expr.Operator.Lexeme,
            expr.Left, expr.Right);
        }

        public string VisitCallExpr(Expr.Call expr)
        {
            return parenthesize2("call", expr.Callee, expr.Arguments);
        }

        public string visitGetExpr(Expr.Get expr)
        {
            return parenthesize2(".", expr.Object, expr.Name.Lexeme);
        }

        public string visitGroupingExpr(Expr.Grouping expr)
        {
            return parenthesize("group", expr.Expression);
        }

        public string visitLiteralExpr(Expr.Literal expr)
        {
            if (expr.Value == null) return "nil";
            return expr.Value.ToString();
        }

        public string visitLogicalExpr(Expr.Logical expr)
        {
            return parenthesize(expr.Operator.Lexeme, expr.Left, expr.Right);
        }

        public string visitSetExpr(Expr.Set expr)
        {
            return parenthesize2("=",
                expr.Object, expr.Name.Lexeme, expr.Value);
        }

        public string visitSuperExpr(Expr.Super expr)
        {
            return parenthesize2("super", expr.Method);
        }

        public string visitThisExpr(Expr.This expr)
        {
            return "this";
        }

        public string visitUnaryExpr(Expr.Unary expr)
        {
            return parenthesize(expr.Operator.Lexeme, expr.Right);
        }

        public string visitVariableExpr(Expr.Variable expr)
        {
            return expr.Name.Lexeme;
        }

        public string visitBlockStmt(Stmt.Block stmt)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("(block ");

            foreach (Stmt statement in stmt.Statements)
            {
                builder.Append(statement.Accept(this));
            }

            builder.Append(")");
            return builder.ToString();
        }

        public string visitClassStmt(Stmt.Class stmt)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("(class " + stmt.Name.Lexeme);
            //> Inheritance omit

            if (stmt.SuperClass != null)
            {
                builder.Append(" < " + print(stmt.SuperClass));
            }
            //< Inheritance omit

            foreach (Stmt.Function method in stmt.Methods)
            {
                builder.Append(" " + print(method));
            }

            builder.Append(")");
            return builder.ToString();
        }

        public string visitExpressionStmt(Stmt.Expression stmt)
        {
            return parenthesize(";", stmt.Expres);
        }

        public string visitFunctionStmt(Stmt.Function stmt)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("(fun " + stmt.Name.Lexeme + "(");

            foreach (Token param in stmt.Params)
            {
                if (param != stmt.Params[0]) builder.Append(" ");
                builder.Append(param.Lexeme);
            }

            builder.Append(") ");

            foreach (Stmt body in stmt.Body) {
                builder.Append(body.Accept(this));
            }

            builder.Append(")");
            return builder.ToString();
        }

        public string visitIfStmt(Stmt.If stmt)
        {
            if (stmt.ElseBranch == null)
            {
                return parenthesize2("if", stmt.Condition, stmt.ThenBranch);
            }

            return parenthesize2("if-else", stmt.Condition, stmt.ThenBranch,
                stmt.ElseBranch);
        }

        public string visitDisplayStmt(Stmt.Display stmt)
        {
            return parenthesize("print", stmt.Expression);
        }

        public string visitReturnStmt(Stmt.Return stmt)
        {
            if (stmt.Value == null) return "(return)";
            return parenthesize("return", stmt.Value);
        }

        public string visitVarStmt(Stmt.Var stmt)
        {
            if (stmt.Initializer == null)
            {
                return parenthesize2("var", stmt.Name);
            }

            return parenthesize2("var", stmt.Name, "=", stmt.Initializer);
        }

        public string visitImportStmt(Stmt.Import stmt)
        {
            return parenthesize("import", stmt.Expression);
        }

        private String parenthesize(String name, params Expr[] exprs)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("(").Append(name);
            foreach (Expr expr in exprs)
            {
                builder.Append(" ");
                builder.Append(expr.Accept(this));
            }
            builder.Append(")");

            return builder.ToString();
        }

        private String parenthesize2(String name, params object[] parts)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("(").Append(name);
            transform(builder, parts);
            builder.Append(")");

            return builder.ToString();
        }

        private void transform(StringBuilder builder, params object[] parts)
        {
            foreach (Object part in parts)
            {
                builder.Append(" ");
                if (part is Expr) {
                    builder.Append(((Expr)part).Accept(this));
                    //> Statements and State omit
                } else if (part is Stmt) {
                    builder.Append(((Stmt)part).Accept(this));
                    //< Statements and State omit
                } else if (part is Token) {
                    builder.Append(((Token)part).Lexeme);
                } else
                {
                    builder.Append(part);
                }
            }
        }
}
}
