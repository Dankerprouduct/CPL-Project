using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace SCL_Project
{
    public  class Parser
    {
        private List<Token> _tokens;
        private int _current = 0;

        public Parser(List<Token> tokens)
        {
            _tokens = tokens;
        }


        public List<Stmt> Parse()
        {
            List<Stmt> statements = new List<Stmt>();

            while (!IsAtEnd())
            {
                statements.Add(Declaration());
            }

            return statements;
        }

        private Stmt Declaration()
        {
            try
            {
                if (Match(new[] {TokenType.SET}))
                    return VarDeclaration();
                return Statement();
            }
            catch (Exception ex)
            {
                Synchronize();
                return null;
            }
        }

        private Stmt VarDeclaration()
        {
            Token name = Consume(TokenType.IDENTIFIER, "Expect variable name.");

            Expr init = null;
            if (Match(new[] {TokenType.EQUALS}))
            {
                init = Expression();
            }

            return new Stmt.Var(name, init); 
        }
        private Stmt Statement()
        {
            if (Match(new[] {TokenType.DISPLAY}))
                return PrintStatement();
            return ExpressionStatement(); 
        }

        private Stmt PrintStatement()
        {
            Expr value = Expression();
            return new Stmt.Display(value);
        }

        private Stmt ExpressionStatement()
        {
            Expr expr = Expression();
            return new Stmt.Expression(expr);
        }

        private Expr Expression()
        {
            return Equality();
        }

        private Expr Equality()
        {
            Expr expr = Comparision();

            while (Match(new TokenType[] {TokenType.NOTEQUAL, TokenType.EQUALTO}))
            {
                Token op = Previous();
                SCL_Project.Expr right = Comparision();
                expr = new Expr.Binary(expr, op, right);
            }

            return expr;
        }

        private bool Match(TokenType[] types)
        {
            foreach(TokenType type in types)
            {
                if (Check(type))
                {
                    Advance();
                    return true;
                }
            }

            return false;
        }

        private Expr Comparision()
        {
            Expr expr = Term();

            while (Match(new[]
                   {
                       TokenType.GREATERTHAN, TokenType.GREATERTHANEQUAL, TokenType.LESSTHAN, TokenType.LESSTHANEQUAL
                   }))
            {
                Token op = Previous();
                Expr right = Term();
                expr = new Expr.Binary(expr, op, right);
            }

            return expr; 
        }

        private Expr Term()
        {
            Expr expr = Factor();

            while (Match(new[] {TokenType.MINUS, TokenType.PLUS}))
            {
                Token op = Previous();
                Expr right = Factor();
                expr = new Expr.Binary(expr, op, right);
            }

            return expr;
        }


        private Expr Factor()
        {
            Expr expr = Unary();

            while (Match(new []{TokenType.DIV, TokenType.MUL}))
            {
                Token op = Previous();
                Expr right = Unary();
                expr = new Expr.Binary(expr, op, right);
            }

            return expr;
        }

        private Expr Unary()
        {
            if (Match(new TokenType[] {TokenType.NOT, TokenType.MINUS}))
            {
                Token op = Previous();
                Expr right = Unary();
                return new Expr.Unary(op, right);
            }

            return Primary();
        }

        private Expr Primary()
        {
            if (Match(new[] {TokenType.FALSE}))
                return new Expr.Literal(false);
            if(Match(new[] {TokenType.TRUE}))
                return new Expr.Literal(true);
            if (Match(new[] {TokenType.NULL}))
                return new Expr.Literal(null);

            if (Match(new[] {TokenType.INTEGER, TokenType.DOUBLE, TokenType.STRING, TokenType.IMPORT, TokenType.IMPLEMENTATION}))
            {
                return new Expr.Literal(Previous().Literal);
            }


            if (Match(new[] {TokenType.IDENTIFIER}))
            {
                return new Expr.Variable(Previous()); 
            }


            throw PError(Peek(), "Expect expression.");
        }

        private Expr Assignment()
        {
            Expr expr = Equality();
            if (Match(new[] {TokenType.EQUALS}))
            {
                Token equals = Previous();
                Expr value = Assignment();

                if (expr is Expr.Variable)
                {
                    Token name = ((Expr.Variable)expr).Name;
                    return new Expr.Assign(name, value); 
                }

                Error(equals, "invalid assignment target.");
            }

            return expr;
        }

        private bool Check(TokenType type)
        {
            if (IsAtEnd())
                return false;
            return Peek().Type == type;
        }

        public Token Advance()
        {
            if (!IsAtEnd())
                _current++;
            return Previous();
        }

        private bool IsAtEnd()
        {
            return Peek().Type == TokenType.EOF;
        }

        private Token Peek()
        {
            return _tokens[_current];
        }

        private Token Previous()
        {
            return _tokens[_current - 1];
        }

        private void Synchronize()
        {
            Advance();

            while (!IsAtEnd())
            {

                switch (Peek().Type)
                {
                    case TokenType.FUNCTION:
                    case TokenType.VARIABLES:
                    case TokenType.DEFINE:
                    case TokenType.IF:
                    case TokenType.EXIT:
                    case TokenType.DISPLAY:
                    case TokenType.SET:
                    case TokenType.RETURN:
                    case TokenType.IMPORT:
                    case TokenType.BEGIN:
                    case TokenType.IMPLEMENTATION:
                    case TokenType.ENDFUN:
                        return;
                }

                Advance();
            }
        }

        private Token Consume(TokenType type, String message)
        {
            if (Check(type))
                return Advance();
            throw PError(Peek(), message);
        }

        private ParseError PError(Token token, string message)
        {
            Console.WriteLine(token + " "+ message);
            return new ParseError();
        }

        static void Error(Token token, string message)
        {
            if (token.Type == TokenType.EOF)
            {
                throw new Exception($"{token.Line} at end {message}");
            }
            else
            {
                throw new Exception($"{token.Line} at {token.Lexeme} {message}");
            }
        }
        
        private class ParseError : Exception
        {

        }
    }
}
