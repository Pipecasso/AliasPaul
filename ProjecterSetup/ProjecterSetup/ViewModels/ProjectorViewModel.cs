using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjecterSetup.Models;
using AliasGeometry;
using Prism.Mvvm;




namespace ProjecterSetup.ViewModels
{
    public class ProjectorViewModel : BindableBase
    {

        private ProjectorModel _projectorModel;

     

        public ProjectorViewModel()
        {
            _projectorModel = new ProjectorModel();
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
                PropertyChangedEventArgs propertyChangedEventArgs = new PropertyChangedEventArgs("ModelCube");
                OnPropertyChanged(propertyChangedEventArgs);
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
