using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AliasGeometry;

namespace Projector
{
    public class TextLabel2d
    {
        Point2d _location;
        string _text;
        string _type;

        public enum Limits { None, Top, Bottom, Left, Right }
        Limits _limit;

        public TextLabel2d(Point2d location, string type, string text, Limits limit = Limits.None)
        {
            _location = location;
            _type = type;
            _text = text;
            _limit = limit;
        }

        public Point2d Location
        {
            get { return _location; }
            set { _location = value; }
        }

        public string Type { get { return _type; } }

        public string Text { get { return _text; } }

        public Limits Limit { get { return _limit; } }
    }
}
