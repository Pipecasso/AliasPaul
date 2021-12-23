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
        private class PaulAction<BacklogItem, T>
        { 
            Action<BacklogItem,T>
        
        }



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


             t
         };

       

        private static state StringToState(string sstate)
        {
            state statetogo = state.unset;
            switch (sstate)
            {
                case "Approved":statetogo = state.approved; break; 
                case "Commited":statetogo = state.commited;break;
                case "Done": statetogo = state.done;break;
                case "New":statetogo = state.nnew;break;
            }
            return statetogo;
        }

        public BacklogItem(IEnumerable<Cell> cells,Dictionary<string,string> column_indices,uint rowindex)
        {
            _tags = new List<string>();

            DoTheThing("ID", cells, column_indices, rowindex, ActionID);
            DoTheThing("Title", cells, column_indices, rowindex, ActionTitle);
            DoTheThing("State", cells, column_indices, rowindex, ActionState);
            DoTheThing("Points", cells, column_indices, rowindex, ActionPoints);

        }

        private void DoTheThing(string columnname, IEnumerable<Cell> cells, Dictionary<string, string> column_indices,uint rowindex,Action<BacklogItem,string> action)
        {
            string celref = column_indices[columnname] + rowindex.ToString();
            Cell cell = cells.Where(x => x.CellReference == celref).FirstOrDefault();
            action(this, celref);
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
