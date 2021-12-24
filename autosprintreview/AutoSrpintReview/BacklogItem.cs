using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoSrpintReview
{
    public class BacklogItem
    {
        private List<string> _tags;
        
        public enum state {unset,approved,commited,nnew,done };
        
        public string ID { get; set; }
        public string Title { get; set; }
        public state State { get; set; }
        public int Points { get; set;}
        public List<string> Tags { get => _tags; }

        private Action<BacklogItem, string> ActionID = (x, y) => x.ID = y;
        private Action<BacklogItem, string> ActionTitle = (x, y) => x.Title = y;
        private Action<BacklogItem, string> ActionState = (x, y) => x.State = StringToState(y);
        private Action<BacklogItem, string> ActionPoints = (x, y) => x.Points = Convert.ToInt32(y);
        private Action<BacklogItem, string> ActionTags = (x, y) =>
         {
             List<string> tagList = new List<string>();
             string[] tags = y.Split(';');
             foreach (string tag in tags)
             {
                 x._tags.Add(tag);
             }
         };

       

        private static state StringToState(string sstate)
        {
            state statetogo = state.unset;
            switch (sstate)
            {
                case "Approved":statetogo = state.approved; break; 
                case "Committed":statetogo = state.commited;break;
                case "Done": statetogo = state.done;break;
                case "New":statetogo = state.nnew;break;
            }
            return statetogo;
        }

        public BacklogItem(IEnumerable<Cell> cells,Dictionary<string,string> column_indices,uint rowindex,string[] sharedstrings)
        {
            _tags = new List<string>();

            DoTheThing("ID", cells, column_indices, rowindex, ActionID,sharedstrings);
            DoTheThing("Title", cells, column_indices, rowindex, ActionTitle,sharedstrings);
            DoTheThing("State", cells, column_indices, rowindex, ActionState,sharedstrings);
            DoTheThing("Effort", cells, column_indices, rowindex, ActionPoints,sharedstrings);
            DoTheThing("Tags", cells, column_indices, rowindex, ActionTags,sharedstrings);

        }

        private void DoTheThing(string columnname, IEnumerable<Cell> cells, Dictionary<string, string> column_indices,uint rowindex,Action<BacklogItem,string> action, string[] sharedstrings)
        {
            string celref = column_indices[columnname] + rowindex.ToString();
            Cell cell = cells.Where(x => x.CellReference == celref).FirstOrDefault();
            string actionstring = string.Empty;
            if (cell.DataType != null && cell.DataType == CellValues.SharedString)
            {
                int index = Convert.ToInt32(cell.CellValue.Text);
                actionstring = sharedstrings[index];
            }
            else
            {
                actionstring = cell.CellValue.Text;
            }
            action(this, actionstring);
        }

    

        public bool AddedDuringSprint
        {
            get
            {
                return _tags.Contains("AddedDuringSprint");
            }
        }

        public bool Done
        {
            get
            {
                return State == state.done;
            }
        }
    }
}
