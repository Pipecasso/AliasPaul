using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoSrpintReview
{
    public class PowerPointBacklogItem : BacklogItem
    {
        public string IDLink { get; set; }
        public string ImagePath { get; set; }

        public PowerPointBacklogItem()
        {
            ImagePath = string.Empty;
        }

        public PowerPointBacklogItem(BacklogItem backlogItem)
        {
            ImagePath = string.Empty;
            CopyFrom(backlogItem);
        }

        public bool solo
        {
            get
            {
                return ImagePath == string.Empty;
            }
        }
    }
}
