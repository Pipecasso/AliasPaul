using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoSrpintReview
{
    class BacklogItems : List<BacklogItem>
    {
        public string Name { get; set; }
        
        public int TotalPoints()
        {
            return this.Sum(x => x.Points);  
        }

        public int OriginalPoints()
        {
            return this.Where(x => x.AddedDuringSprint == false).Sum(y => y.Points);
        }

        public int AdditionalItems()
        {
            return this.Where(x => x.AddedDuringSprint).Sum(y => y.Points);
        }

        public IEnumerable<BacklogItem> Done()
        {
            return this.Where(x => x.Done);
        }

        public IEnumerable<BacklogItem> InProgress()
        {
            return this.Where(x => !x.Done);
        }

        public int DonePoints()
        {
            return Done().Sum(x => x.Points);
        }

        public int NotDonePoints()
        {
            return InProgress().Sum(x => x.Points);
        }
    
    }
}
