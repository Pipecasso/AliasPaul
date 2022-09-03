using GeoFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveCharts;
using LiveCharts.Wpf;
using System.Windows.Media;

namespace MathsFilter.ViewModels
{
    internal class AnalysisViewModel
    {
        private HistoTransformMatrix _transformMatrix;
        private SeriesCollection _seriesCollection;
        public AnalysisViewModel(HistoTransformMatrix transformMatrix)
        {
            _transformMatrix = transformMatrix;

            Maximum = transformMatrix.maximum;
            Minimum = transformMatrix.minimum;

            double tl, th, ir;
            transformMatrix.RangeValues(out tl, out th, out ir);
            InRange = ir;
            TooLow = tl;
            TooHigh = th;
            ColourCount = transformMatrix.ColourCount;

            _seriesCollection = new SeriesCollection();
            LineSeries red = new LineSeries();
            red.Title = "red";
            red.Stroke = new SolidColorBrush(Colors.Red);
            red.PointGeometry = null;
            red.Values = new ChartValues<int>();
            for (int i = 1; i < 256; i++)
            {
                red.Values.Add(_transformMatrix.Red[i]);
            }
            _seriesCollection.Add(red);

            LineSeries green = new LineSeries();
            green.Title = "green";
            green.PointGeometry = null;
            green.Stroke = new SolidColorBrush(Colors.Green);
            green.Values = new ChartValues<int>();
            for (int i = 1; i < 256; i++)
            {
                green.Values.Add(_transformMatrix.Green[i]);
            }
            _seriesCollection.Add(green);

            LineSeries blue = new LineSeries();
            blue.Title = "blue";
            blue.PointGeometry = null;
            blue.Stroke = new SolidColorBrush(Colors.Blue);
            blue.Values = new ChartValues<int>();
            for (int i = 1; i < 256; i++)
            {
                blue.Values.Add(_transformMatrix.Blue[i]);
            }
            _seriesCollection.Add(blue);
        }

        public double InRange { get; private set; }
        public double TooLow { get; private set; }
        public double TooHigh { get; private set; }
        public double Maximum { get; private set; }
        public double Minimum { get; private set; }

        public int ColourCount { get; private set; }

        public SeriesCollection SeriesCollection
        {
            get => _seriesCollection;
        }
    }
}
