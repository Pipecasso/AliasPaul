using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using org.mariuszgromada.math.mxparser;

namespace GeoFilterFunctions
{
    class FunctionsXY
    {
        public static Function LavaLamp = new Function("f(x,y) = (sin(x/5) + cos(3*y) + (sin((2*x+y)))^2)*200");
        public static Function Harlequin = new Function("f(x, y) = ((cot(rad(x)) * tan(rad(y)) + sec(rad(x)) * cosec(rad(y))) * sqrt((x / 10) ^ 2 + (y / 10) ^ 2))");
        public static Function Pschadellic = new Function("f(x, y) = abs((x*y/97)^3 - 4*(x*y/70)^2 + 2*x*y/77 - 7*x^2/5 + 4*y^2/17 - 3*x + 7*y)/10");
        public static Function Onion = new Function("f(x,y) = sqrt(x^2 + y^2)*sqrt(abs((x+y)))");
        public static Function YinYang = new Function("f(x,y) = (x+y / x*y)");


    }
}
