using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AutoSrpintReview
{
    public class SR_Config
    {
        private string _iteration;
        private DateTime _date;
        private DateTime _datenext;
        private string _logopath;
        private string _teamname;
        private string _teamdescription;
        private string _screenshotpath;
        private string _templatepath;
        private string _burndowpath;
        private string _backlogpath;
        private List<string> _goals;
        private List<string> _demos;

        
        private void ProbeAttributes(XmlAttributeCollection xAtts)
        {
            foreach (XmlAttribute xAtt in xAtts)
            {
                switch (xAtt.Name)
                {
                    case "Iteration": _iteration = xAtt.Value; break;
                    case "Date": _date = DateTime.Parse(xAtt.Value); break;
                    case "NextDate": _datenext = DateTime.Parse(xAtt.Value); break;
                    case "Logo": _logopath = xAtt.Value; break;
                    case "Name": _teamname = xAtt.Value;break;
                    case "Description": _teamdescription = xAtt.Value;break;
                    case "Screenshots": _screenshotpath = xAtt.Value;break;
                    case "Burndown": _burndowpath = xAtt.Value;break;
                    case "Template":_templatepath = xAtt.Value;break;
                    case "SprintBacklog":_backlogpath = xAtt.Value;break;
                }

            }
        }

        public SR_Config(string config_path)
        {
            _iteration = "0";
            _date = DateTime.Now;
            _datenext = DateTime.Now;
            _teamname = "Incognito";

            _demos = new List<string>();
            _goals = new List<string>();
      
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(config_path);
            XmlNode xSprint = xDoc.SelectSingleNode("/AutoSprintReview/Sprint");
            ProbeAttributes(xSprint.Attributes);
            XmlNode xTeam = xDoc.SelectSingleNode("/AutoSprintReview/Team");
            ProbeAttributes(xTeam.Attributes);
            XmlNode xPath = xDoc.SelectSingleNode("/AutoSprintReview/Paths");
            ProbeAttributes(xPath.Attributes);

            XmlNodeList xGoals = xDoc.SelectNodes("/AutoSprintReview/Goals/Goal");
            foreach (XmlNode xGoal in xGoals)
            {
                _goals.Add(xGoal.InnerText);
            }

            XmlNodeList xDemos = xDoc.SelectNodes("/AutoSprintReview/Demos/Demo");
            foreach (XmlNode xDemo in xDemos)
            {
                _demos.Add(xDemo.InnerText);
            }
        }

        public string Interation { get => _iteration; }
        public string TeamName { get => _teamname; }
        public string TeamDescription { get => _teamdescription; }
        public DateTime Date { get => _date; }
        public DateTime DataNext { get => _datenext; }
        public string LogoPath { get => _logopath; }
        public string BacklogPath { get => _backlogpath; }
        public string BurndownPath { get => _burndowpath; }
        public string TemplatePath { get => _templatepath; }
        public string ScreenshotPath { get => _screenshotpath; }

        public IEnumerable<string> Goals { get => _goals; }
        public IEnumerable<string> Demos { get => _demos; }

    
    }
}
