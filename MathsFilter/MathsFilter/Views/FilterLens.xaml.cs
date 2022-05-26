using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MathsFilter.Views
{
    /// <summary>
    /// Interaction logic for FilterLens.xaml
    /// </summary>
    public partial class FilterLens : UserControl
    {
        public double CrossFraction { get; set; }
        
        public FilterLens()
        {
            InitializeComponent();
            CrossFraction = 0.15;
        }


        protected override void OnRender(DrawingContext drawingContext)
        {
            Pen myPen = new Pen(Brushes.WhiteSmoke, 5);
            Point centre = new Point(_Canvas.ActualWidth / 2, _Canvas.ActualHeight / 2);
            double crossLength = CrossFraction*_Canvas.ActualHeight;
            Point top = new Point(centre.X, centre.Y + crossLength / 2);
            Point bottom = new Point(centre.X, centre.Y - crossLength / 2);
            Point left = new Point(centre.X - crossLength / 2, centre.Y);  
            Point right = new Point(centre.X + crossLength / 2, centre.Y);
            drawingContext.DrawLine(myPen, top, bottom);
            drawingContext.DrawLine(myPen, left, right);

            Point p1 = new Point(0, 0);
            Point p2 = new Point(_Canvas.ActualWidth,_Canvas.ActualHeight);
            Point p3 = new Point(0, _Canvas.ActualHeight);
            Point p4 = new Point(_Canvas.ActualWidth, 0);

            drawingContext.DrawLine(myPen, p1, p2);
            drawingContext.DrawLine(myPen, p3, p4);

            base.OnRender(drawingContext);

        }


    }
}
