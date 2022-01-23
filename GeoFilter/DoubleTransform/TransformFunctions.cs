using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using org.mariuszgromada.math.mxparser;
using System.Reflection;

namespace TransformParameters
{
    class TransformFunctions
    {
        private Dictionary<string, Function> _FunkyKingston;

       
     

        static public Function Circle()
        {
            return new Function("f(x,y) = sqrt(x^2 + y^2)");
        }

        static public Function Multiply()
        {
            return new Function("f(x, y) = x * y");
        }

        static public Function LavaLamp()
        {
            return new Function("f(x,y) = (sin(rad(x/5)) + cos(rad(3*y)) + (sin((rad(2*x+y))))^2)*200");
        }

        static public Function Harlequin()
        {
            return new Function("f(x, y) = ((cot(rad(x)) * tan(rad(y)) + sec(rad(x)) * cosec(rad(y))) * sqrt((x / 10) ^ 2 + (y / 10) ^ 2))");
        }

        static public Function Psychedelic()
        {
            return new Function("f(x, y) = abs((x*y/97)^3 - 4*(x*y/70)^2 + 2*x*y/77 - 7*x^2/5 + 4*y^2/17 - 3*x + 7*y)/10");
        }

        static public Function Onion()
        {
            return new Function("f(x,y) = sqrt(x^2 + y^2)*sqrt(abs((x+y)))");
        }

        static public Function YinYang()
        {
           return new Function("f(x,y) = (x+y / x*y)");
        }

        static public Function Oil()
        {
            return new Function("f(x,y) = x*rad(y) + y*rad(2*x)");
        }

        static public Function RisingSun()
        {
            return new Function("f(x,y) = exp(x*y/x^2*y^2)");
        }

        public TransformFunctions()
        {
            _FunkyKingston = new Dictionary<string, Function>();
            Type cType = typeof(TransformFunctions);
            MethodInfo[] Funcs = cType.GetMethods(BindingFlags.Public | BindingFlags.Static);
            for (int i = 0; i < Funcs.Length; i++)
            {
                MethodInfo mi = Funcs[i];
                object fo = mi.Invoke(null, null);
                Function f = (Function)fo;
                _FunkyKingston.Add(mi.Name, f);
            }
        }

        public Function GetFunction(string sin)
        {
            if (_FunkyKingston.ContainsKey(sin))
            {
                return _FunkyKingston[sin];
            }
            else
            {
                return null;
            }

        }
    }
}
