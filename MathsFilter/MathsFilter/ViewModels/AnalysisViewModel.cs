using GeoFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathsFilter.ViewModels
{
    internal class AnalysisViewModel
    {
        private TransformMatrix _transformMatrix;
        public AnalysisViewModel(TransformMatrix transformMatrix)
        {
            _transformMatrix = transformMatrix;
            double r, g, b, o;
            _transformMatrix.FlatComposition(out r, out g, out b, out o);
            FlatRed = r*100;
            FlatGreen = g*100;
            FlatBlue = b*100;
            OutOfBounds = o*100;
        }
        
        public double FlatRed { get; private set; }
        public double FlatGreen { get; private set; }   
        public double FlatBlue { get; private set; }
        public double OutOfBounds { get; private set; }
    }
}
