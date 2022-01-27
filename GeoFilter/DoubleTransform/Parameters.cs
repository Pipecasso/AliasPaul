using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoFilter;
using org.mariuszgromada.math.mxparser;


namespace TransformParameters
{
    internal class Parameters
    {
        internal enum MatrixOperation
        {
            add,
            minus,
            multiply
        }

        private int _dimension = 500;
        private bool _abs = false;
        private MatrixOperation _mo = MatrixOperation.add;
        private Function _f = TransformFunctions.Circle();
        private Function _g = null;
        private BitmapBox.Colour _col = BitmapBox.Colour.None;
        private BitmapBox.OutOfBounds _oob = BitmapBox.OutOfBounds.Reject;
        private bool _standard = false;
        private bool _flat = false;
        private bool _valid = true;
        private bool _scaletomatch = true;
        private bool _scale = false;
        private double _scalefactor = 1;
        public List<string> _errorlist;


        internal Parameters(string[] args)
        {
            _errorlist = new List<string>();
            TransformFunctions transformers = new TransformFunctions();
            for (int i = 0; i < args.Length; i++)
            {
                string current = args[i];
                if (i < args.Length - 1 && current == "-d")
                {
                    string next = args[i + 1];
                    _dimension = Convert.ToInt32(next);
                    i++;
                    continue;
                }

                if (current == "-abs")
                {
                    _abs = true;
                    continue;
                }

                if (i < args.Length - 1 && current == "-o")
                {
                    string next = args[i + 1];
                    if (next == "multipy" || next == "Multiply" || next == "*")
                    {
                        _mo = MatrixOperation.multiply;
                    }
                    else if (next == "Minus" || next == "minus" || next == "-")
                    {
                        _mo = MatrixOperation.minus;
                    }
                    i++;
                    continue;

                }

                if (i < args.Length - 1 && (current == "-fn" || current == "-gn"))
                {
                    string next = args[i + 1];
                    Function myfunc = transformers.GetFunction(next);
                    if (current == "-fn")
                    {
                        _f = myfunc;
                    }
                    else
                    {
                       if (myfunc == null)
                        {
                            _valid = false;
                            _errorlist.Add(string.Format("Function {0} not found", next));
                        }
                        _g = myfunc;
                       
                    }
                    i++;
                    continue;
                }

                if (i < args.Length - 1 && (current == "-f" || current == "-g"))
                {
                    string fxy = "f(x,y)=" + args[i + 1];
                    Function myfunc = new Function(fxy);
                    double fout = myfunc.calculate(0.24242424, -0.32352352);
                    if (!double.IsNaN(fout))
                    {
                        if (current == "-f")
                        {
                            _f = myfunc;
                        }
                        else
                        {
                            _g = myfunc;
                        }
                    }
                    else
                    {
                        if (_valid) _valid = false;
                        string error = string.Format("function {0} {1} is not valid", current, fxy);
                        _errorlist.Add(error);
                    }
                    i++;
                    continue;
                }

                if (i < args.Length - 1 && current == "-c")
                {
                    string next = args[i + 1];
                    if (next.Contains("r"))
                    {
                        _col = _col | BitmapBox.Colour.Red;
                    }
                    if (next.Contains("g"))
                    {
                        _col = _col | BitmapBox.Colour.Green;
                    }
                    if (next.Contains("b"))
                    {
                        _col = _col | BitmapBox.Colour.Blue;
                    }
                    i++;
                    continue;
                }

                if (i < args.Length - 1 && current == "-oob")
                {
                    string next = args[i + 1];
                    switch (next)
                    {
                        case "re":
                        case "reject":
                            _oob = BitmapBox.OutOfBounds.Reject;
                            break;
                        case "ro":
                        case "rollover":
                            _oob = BitmapBox.OutOfBounds.Rollover;
                            break;
                        case "b":
                        case "bounce":
                            _oob = BitmapBox.OutOfBounds.Bounce;
                            break;
                        case "s":
                        case "stop":
                            _oob = BitmapBox.OutOfBounds.Stop;
                            break;
                    }
                    i++;
                    continue;
                }

                if (current == "-standard")
                {
                    _standard = true;
                    continue;
                }

                if (current == "-flat")
                {
                    _flat = true;
                    continue;
                }

                if (current == "-noscm")
                {
                    _scaletomatch = false;
                }

  
                if (i < args.Length - 1 && current == "-scl")
                {
                    string next = args[i + 1];
                    _scale = true;
                    _scalefactor = Convert.ToDouble(next);
                }
            }





            if (_col == BitmapBox.Colour.None)
            {
                _col = BitmapBox.Colour.Red;
            }

        }

        internal int dimension
        {
            get
            {
                return _dimension;
            }
        }

        internal bool abs
        {
            get
            {
                return _abs;
            }
        }

        internal MatrixOperation moperation
        {
            get
            {
                return _mo;
            }

        }

        internal Function f
        {
            get
            {
                return _f;
            }
        }

        internal Function g
        {
            get
            {
                return _g;
            }
        }

        internal BitmapBox.Colour Colours
        {
            get
            {
                return _col;
            }
        }

        internal BitmapBox.OutOfBounds oob
        {
            get
            {
                return _oob;

            }
        }

        internal bool Standard
        {
            get
            {
                return _standard;
            }
        }

        internal bool Flat
        {
            get
            {
                return _flat;
            }
        }
        
        internal bool Valid
        {
            get
            {
                return _valid;
            }
        }

        internal bool ScaleToMatch
        {
            get
            {
                return _scaletomatch;
            }
        }

        internal bool IsDoubleTransform()
        {
            return _g != null;
        }

        internal bool Scale
        {
            get
            {
                return _scale;
            }
        }

        internal double ScaleFactor
        {
            get
            {
                return _scalefactor;
            }
        }






    }
}
