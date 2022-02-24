﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCL_Project
{

    public enum TokenType
    {
        IMPORT,
        IMPLEMENTATION,
        FUNCTION,
        IS, VARIABLES, DEFINE,
        OF, TYPE, INTEGER,
        DOUBLE, BEGIN,
        ENDFUN, DISPLAY,
        SET, INPUT,
        IF, THEN, ELSEIF,
        ELSE, ENDIF, RETURN,
        EXIT,

        EQUALS, PLUS, MINUS, MUL, DIV,
        EQUALTO, NOTEQUAL, LESSTHAN, GREATERTHAN, 
        GREATERTHANEQUAL, LESSTHANEQUAL, 
        
        SPACE, COMMA, QUOTATION, TAB,

        EOF
    }

    public  class Token
    {
        public TokenType Type;
        public string Lexeme;
        public object Literal;
        public int Line;

        public Token(TokenType type, string lexeme, object literal, int line)
        {
            Type = type;
            Lexeme = lexeme;
            Literal = literal;
            Line = line;

        }

        public override string ToString()
        {
            return $"{Type} {Lexeme} {Literal}";
        }
    }


}