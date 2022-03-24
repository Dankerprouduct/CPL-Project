using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCL_Project
{
    public abstract class Stmt
    {
        public interface IVisitor<R>
        {
            R visitBlockStmt(Block stmt);
            R visitClassStmt(Class stmt);
            R visitExpressionStmt(Expression stmt);
            R visitFunctionStmt(Function stmt);
            R visitIfStmt(If stmt);
            R visitDisplayStmt(Display stmt);
            R visitReturnStmt(Return stmt);
            R visitVarStmt(Var stmt);

            R visitImportStmt(Import stmt); 
        }

        public abstract T Accept<T>(IVisitor<T> visitor);


        public class Block : Stmt
        {
            public List<Stmt> Statements;

            public Block(List<Stmt> statements)
            {
                Statements = statements;
            }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.visitBlockStmt(this); 
            }
        }

        public class Class : Stmt
        {
            public Token Name;
            public Expr.Variable SuperClass;
            public List<Function> Methods;

            public Class(Token name, Expr.Variable superClass, List<Function> methods)
            {
                Name = name; 
                SuperClass = superClass;
                Methods = methods;
            }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.visitClassStmt(this); 
            }
        }

        public class Expression : Stmt
        {
            public Expr Expres;

            public Expression(Expr expr)
            {
                Expres = expr;
            }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.visitExpressionStmt(this); 
            }
        }

        public class Function : Stmt
        {
            public Token Name;
            public List<Token> Params;
            public List<Stmt> Body;
            public Function(Token name, List<Token> pms, List<Stmt> body)
            {
                Name = name;
                Params = pms;
                Body = body;
            }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.visitFunctionStmt(this);
            }
        }

        public class If : Stmt
        {
            public Expr Condition;
            public Stmt ThenBranch;
            public Stmt ElseBranch;

            public If(Expr cond, Stmt thenBranch, Stmt elseBranch)
            {
                Condition = cond;
                ThenBranch = thenBranch;
                ElseBranch = elseBranch; 
            }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.visitIfStmt(this);
            }
        }

        public class Display : Stmt
        {
            public Expr Expression;
            public Display(Expr expr)
            {
                Expression = expr;
            }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.visitDisplayStmt(this);
            }
        }

        public class Return : Stmt
        {
            public Token Keyword;
            public Expr Value;

            public Return(Token keyword, Expr val)
            {
                Keyword = keyword;
                Value = val; 
            }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.visitReturnStmt(this); 
            }
        }

        public class Var : Stmt
        {
            public Token Name;

            public Expr Initializer;

            public Var(Token name, Expr initializer)
            {
                Name = name;
                Initializer = initializer; 
            }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.visitVarStmt(this); 
            }
        }

        public class Import: Stmt
        {
            public Expr Expression;

            public Import(Expr expr)
            {
                Expression = expr;
            }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.visitImportStmt(this);
            }
        }
    }
}
