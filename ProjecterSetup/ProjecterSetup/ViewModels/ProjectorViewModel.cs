using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjecterSetup.Models;




namespace ProjecterSetup.ViewModels
{
    public class ProjectorViewModel
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

    }
}
