using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjecterSetup.Models;
using AliasGeometry;




namespace ProjecterSetup.ViewModels
{
    public class ProjectorViewModel : INotifyPropertyChanged
    {

        private ProjectorModel _projectorModel;

     

        public ProjectorViewModel()
        {
            _projectorModel = new ProjectorModel();
        }

        public string GetCentre
        {
            get
            {
                string ret = string.Empty;
                if (projectorModel.Cube != null)
                {
                    ret = projectorModel.Cube.CenterDisplay;
                }
                return ret;
            }
        }

        public double updown
        {
            get
            {
                double ret = 0;
                if (_projectorModel.Cube !=null)
                {
                    ret = _projectorModel.Cube.TopBottomDistance();
                }
                return ret;
            }
        }

        public double leftright
        {
            get
            {
                double ret = 0;
                if (_projectorModel.Cube != null)
                {
                    ret = _projectorModel.Cube.LeftRightDistance();
                }
                return ret;
            }
        }

        public double frontback
        {
            get
            {
                double ret = 0;
                if (_projectorModel.Cube != null)
                {
                    ret = projectorModel.Cube.FrontBackDistance();
                }
                return ret;
            }
        }

        public ProjectorModel projectorModel
        {
            get
            {
                return _projectorModel;
            }
        }

        public CubeView ModelCube
        {
            get
            {
                return _projectorModel.Cube;
            }
            set
            {
                _projectorModel.Cube = value;
                OnPropertyRaised("ModelCube");
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyRaised(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }




        /* private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
         {
             if (PropertyChanged != null)
             {
                 PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
             }
         }*/

        /* private void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
         {
             OnPropertyChanged(PropertySupport.ExtractPropertyName(propertyExpression));
         }

         private void OnPropertyChanged(string propertyName)
         {
             PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
         }*/

    }
}
