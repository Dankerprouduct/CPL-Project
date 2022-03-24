using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SCL_Project
{
    public abstract class Expr
    {
        public interface IVisitor<R>
        {
            R VisitAssignExpr(Assign expr);
            R VisitBinaryExpr(Binary expr);
            R VisitCallExpr(Call expr);
            R visitGetExpr(Get expr);
            R visitGroupingExpr(Grouping expr);
            R visitLiteralExpr(Literal expr);
            R visitLogicalExpr(Logical expr);
            R visitSetExpr(Set expr);
            R visitSuperExpr(Super expr);
            R visitThisExpr(This expr);
            R visitUnaryExpr(Unary expr);
            R visitVariableExpr(Variable expr);
        }
        
        public abstract T Accept<T>(IVisitor<T> visitor);
        public class Assign : Expr
        {
            public Token Name;
            public Expr Value;
            public Assign(Token name, Expr value)
            {
                this.Name = name;
                this.Value = value;
            }


            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.VisitAssignExpr(this);
            }
        }

        public class Binary : Expr
        {
            public Expr Left;
            public Expr Right;
            public Token Operator; 
            public Binary(Expr left, Token op, Expr right)
            {
                Left = left;
                Right = right;
                Operator = op;
            }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.VisitBinaryExpr(this); 
            }
        }

        public class Call : Expr
        {
            public Expr Callee;
            public Token Paren;
            public List<Expr> Arguments; 

            public Call(Expr callee, Token paren, List<Expr> arguments)
            {
                Callee = Callee;
                Paren = paren;
                Arguments = arguments;
            }


            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.VisitCallExpr(this);
            }
        }

        public class Get : Expr
        {
            public Expr Object;
            public Token Name; 
            public Get(Expr obj, Token name)
            {
                Object = obj;
                Name = name;
            }


            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.visitGetExpr(this); 
            }
        }

        public class Grouping : Expr
        {
            public Expr Expression;

            public Grouping(Expr expression)
            {
                Expression = expression;
            }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.visitGroupingExpr(this); 
            }
        }

        public class Literal : Expr
        {
            public object Value; 
            public Literal(object value)
            {
                Value = value; 
            }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.visitLiteralExpr(this); 
            }
        }

        public class Logical : Expr
        {
            public Expr Left;
            public Token Operator;
            public Expr Right;
            public Logical(Expr left, Token op, Expr right)
            {
                Left = left;
                Operator = op;
                Right = right;
            }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.visitLogicalExpr(this);
            }
        }

        public class Set : Expr
        {
            public Expr Object;
            public Token Name;
            public Expr Value;

            public Set(Expr obj, Token name, Expr value)
            {
                Object = obj;
                Name = name;
                Value = value;
            }
            
            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.visitSetExpr(this);
            }
        }

        public class Super : Expr
        {
            public Token Keyword;
            public Token Method;

            public Super(Token keyword, Token method)
            {
                Keyword = keyword;
                Method = method;
            }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.visitSuperExpr(this);
            }
        }

        public class This : Expr
        {
            public Token Keyword;
            public This(Token keyword)
            {
                Keyword = keyword; 
            }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.visitThisExpr(this);
            }

        }

        public class Unary : Expr
        {
            public Token Operator;
            public Expr Right; 
            
            public Unary(Token op, Expr right)
            {
                Operator = op;
                Right = right;
            }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.visitUnaryExpr(this);
            }
        }

        public class Variable : Expr
        {
            public Token Name;
            public Variable(Token name)
            {
                Name = name;
            }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.visitVariableExpr(this);
            }
        }


    }
}
