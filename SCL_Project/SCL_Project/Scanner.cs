using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SCL_Project
{
    public class Scanner
    {
        private string _source;
        private List<Token> _tokens = new List<Token>();

        private int _start = 0;
        private int _current = 0;
        private int _line = 1;

        public Scanner(string source)
        {
            _source = source;
        }

        public List<Token> ScanTokens()
        {
            while (!AtEnd())
            {
                _start = _current;
                
                ScanToken();
            }

            _tokens.Add(new Token(TokenType.EOF, "", null, _line));

            return _tokens;
        }

        public void ScanToken()
        {
            char c = Adv();

            switch (c)
            {
                // single characters
                case '=':
                    AddToken(TokenType.EQUALS);
                    break;
                case ',':
                    AddToken(TokenType.COMMA);
                    break;
                case '"':
                    AddToken(TokenType.QUOTATION);
                    break;
                case '\t':
                    AddToken(TokenType.TAB);
                    break;
                case '+':
                    AddToken(TokenType.PLUS);
                    break;
                case '-':
                    AddToken(TokenType.MINUS);
                    break;
                case '*':
                    AddToken(TokenType.MUL);
                    break;
                case '/':
                    if(Match('*'))
                        // comment
                        AddToken(TokenType.DIV);
                    break;
                case ' ':
                    AddToken(TokenType.SPACE);
                    break;
                
                
            }
        }

        private char Adv()
        {
            return _source[_current++];
        }

        private void AddToken(TokenType type)
        {
            AddToken(type, null);
        }

        private void AddToken(TokenType type, object literal)
        {
            string text = _source.Substring(_start, _current); 
            _tokens.Add(new Token(type, text, literal, _line));
        }

        private Boolean Match(char expected)
        {
            if (AtEnd()) return false;
            if (_source[_current] != expected)
                return false;
            _current++;

            return true;
        }
        private bool AtEnd()
        {
            return _current >= _source.Length;
        }
    }
}
