using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AliasGeometry;
using Projector;

namespace ProjecterSetup.Models
{
    public class ProjectorModel :  INotifyPropertyChanged
    {
        private CubeView _cube;

        public ProjectorModel()
        {
            _cube = null;
        }
        
        public CubeView Cube 
        { 
            get
            {
                return _cube;
            }
            set
            {
                _cube = value;
                OnPropertyRaised("Cube");
            }
         }
        public Vector3d Normal { get; set; }
        double Div { get; set; }
        double Mult { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyRaised(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }



    }
}
