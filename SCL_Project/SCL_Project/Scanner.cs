using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.CompilerServices;
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

        private static Dictionary<string, TokenType> _keywords = new Dictionary<string, TokenType>()
        {
            {"import", TokenType.IMPORT},
            {"implementations", TokenType.IMPLEMENTATION},
            {"function", TokenType.FUNCTION},
            {"is", TokenType.IS},
            {"variables", TokenType.VARIABLES},
            {"define", TokenType.DEFINE},
            {"of", TokenType.OF},
            {"type", TokenType.TYPE},
            {"integer", TokenType.INTEGER},
            {"string", TokenType.STRING},
            {"double", TokenType.DOUBLE},
            {"begin", TokenType.BEGIN},
            {"endfun", TokenType.ENDFUN},
            {"display", TokenType.DISPLAY},
            {"set", TokenType.SET},
            {"input", TokenType.INPUT},
            {"if", TokenType.IF},
            {"then", TokenType.THEN},
            {"elseif", TokenType.ELSEIF},
            {"else", TokenType.ELSE},
            {"endif", TokenType.ENDIF},
            {"return", TokenType.RETURN},
            {"exit", TokenType.EXIT},
            {"struct", TokenType.STRUCT},
            {"endstruct", TokenType.ENDSTRUCT},
            {"structures", TokenType.STRUCTURES},
            {"pointer", TokenType.POINTER},
            {"array", TokenType.ARRAY},
            {"definetype", TokenType.DEFINETYPE}
        };

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
                case ',':
                    AddToken(TokenType.COMMA);
                    break;
                case '"':
                    String();
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
                {
                    if (Match('/'))
                    {
                        while(Peek() != '\n' && !AtEnd())
                        {
                            Adv();
                        }
                    }
                    else if (Match('*'))
                    {
                        while (Peek() != '*' && PeekNext() !='/' && !AtEnd())
                        {
                            //Console.Write(_source[_current]);
                            Adv();
                        }

                        Adv();
                        Adv();

                    }
                    else
                    {
                        AddToken(TokenType.DIV);
                    } 
                    break;
                }
                case '[':
                {
                    while (Peek() != ']' && !AtEnd())
                    {
                        Adv();
                    }
                    break;
                }
                case '!':
                    AddToken(Match('=') ? TokenType.NOTEQUAL : TokenType.NOT);
                    break;
                case '=':
                    AddToken(Match('=') ? TokenType.EQUALTO : TokenType.EQUALS);
                    break;
                case '<':
                    AddToken(Match('=') ? TokenType.LESSTHANEQUAL : TokenType.LESSTHAN);
                    break;
                case '>':
                    AddToken(Match('=') ? TokenType.GREATERTHANEQUAL: TokenType.GREATERTHAN);
                    break;

                // skip whitespace
                case ' ':
                case '\r':

                case '\t':
                    break;
                case '\n':
                    _line++;
                    break;

                default:
                    if (IsDigit(c))
                    {
                        Number();
                    }
                    else if (IsAlpha(c))
                    {
                        Identifier();
                    }
                    else
                    {
                        Console.WriteLine($"line: {_line} Unexpected character {c} {Peek()} {PeekNext()}");
                    }
                    break;

            }
        }

        private void Identifier()
        {
            while (IsAlphaNumeric(Peek()))
                Adv();

            //Console.WriteLine($"start: {_start}: {_source[_start]}, current: {_current}: {_source[_current]}");
            
            string text = _source.Substring(_start, _current - _start);
            //Console.WriteLine($"Keyword: {text}");

            TokenType type = GetKeywordValue(text);

            AddToken(type);
        }

        public TokenType GetKeywordValue(string text)
        {
            if (_keywords.ContainsKey(text))
                return _keywords[text];
            return TokenType.IDENTIFIER;
        }

        private void Number()
        {
            //Console.WriteLine($"{_start} {_current} {_line}");
            while (IsDigit(Peek())) Adv();

            if (Peek() == '.' && IsDigit(PeekNext()))
            {
                Adv();

                while (IsDigit(Peek()))
                    Adv();
            }

            var valueString = _source.Substring(_start, _current - _start);
            //Console.WriteLine(valueString);
            var value = double.Parse(valueString); 
            
            AddToken(TokenType.DOUBLE, value);
        }

        private bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        private bool IsAlpha(char c)
        {
            return (c >= 'a' && c <= 'z') ||
                   (c >= 'A' && c <= 'Z') ||
                   c == '_';
        }

        private bool IsAlphaNumeric(char c)
        {
            return IsAlpha(c) || IsDigit(c); 
        }

        private void String()
        {
            while (Peek() != '"' && !AtEnd())
            {
                if (Peek() == '\n')
                {
                    _line++;
                }
                Adv();
            }

            if (AtEnd())
            {
                Console.WriteLine($"{_line} Unterminated string");
                return;
            }

            Adv();
            
            var value = _source.Substring(_start + 1, _current - _start - 1); 
            AddToken(TokenType.STRING, value);
        }

        private char Adv()
        {
            return _source[_current++];
        }

        private char Peek()
        {
            if (AtEnd())
                return '\0';
            return _source[_current]; 
        }

        private char PeekNext()
        {
            if (_current + 1 >= _source.Length)
                return '\0';
            return _source[_current + 1];
        }

        private void AddToken(TokenType type)
        {
            AddToken(type, null);
        }

        private void AddToken(TokenType type, object literal)
        {
            string text = _source.Substring(_start, _current - _start); 
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
