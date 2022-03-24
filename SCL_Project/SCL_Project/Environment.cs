﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCL_Project
{
    public  class Environment
    {
        public Dictionary<string, object> Values = new Dictionary<string, object>();
        public Environment Enclosing = null;

        public Environment()
        {
            Enclosing = null; 
        }

        public Environment(Environment enclosing)
        {
            this.Enclosing = enclosing;
        }

        public void Define(string name, object value)
        {
            Values.Add(name, value);
        }

        public object Get(Token name)
        {
            if (Values.ContainsKey(name.Lexeme))
            {
                return Values[name.Lexeme];
            }

            if (Enclosing != null)
                return Enclosing.Get(name);

            throw new Exception($"{name}, Undefined variable {name.Lexeme}.");
        }

        public void Assign(Token name, object value)
        {
            if (Values.ContainsKey(name.Lexeme))
            {
                Values[name.Lexeme] = value;
                return;
            }

            if (Enclosing != null)
            {
                Enclosing.Assign(name, value);
                return;
            }

            Console.WriteLine("undefined variable");
        }
    }
}
