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

        

       public BacklogItem()
        {
            _tags = new List<string>();
        }

        

        public BacklogItem(IEnumerable<Cell> cells,Dictionary<string,string> column_indices,uint rowindex,string[] sharedstrings)
        {
            _tags = new List<string>();

        

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
