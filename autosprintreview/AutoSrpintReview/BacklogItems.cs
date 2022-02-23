using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoSrpintReview
{
    public class PowerpointBacklogItems : List<PowerPointBacklogItem>
    {
        public PowerpointBacklogItems(IEnumerable<BacklogItem> backlogItems)
        {
            foreach (BacklogItem backlogItem in backlogItems)
            {
                Add(new PowerPointBacklogItem(backlogItem));
            }
        }
            
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

       public IEnumerable<PowerPointBacklogItem> Done()
       {
           return this.Where(x => x.Done).Cast<PowerPointBacklogItem>();
       }

       public IEnumerable<PowerPointBacklogItem> InProgress()
       {
            return this.Where(x => !x.Done).Cast<PowerPointBacklogItem>();
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
