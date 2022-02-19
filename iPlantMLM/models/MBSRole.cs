using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlantMLM
{
    public class MBSRole
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int OwnerID { get; set; }
        public string ExplainText { get; set; }
        public int Active { get; set; }
        public string ActiveText { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreateTimeText { get; set; }
    }
}
