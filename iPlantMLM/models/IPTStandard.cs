using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlantMLM
{
    public class IPTStandard
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
        public int PartPointID { get; set; }
        public int ItemID { get; set; }
        public int PartID { get; set; }
        public int Type { get; set; }
        public double UpperLimit { get; set; }
        public string UpperLimitText { get; set; }
        public double LowerLimit { get; set; }
        public string LowerLimitText { get; set; }
        public String UnitText { get; set; }
        public String DefaultValue { get; set; }
        public String TextDescription { get; set; }
        public int EditorID { get; set; }
        public DateTime EditTime { get; set; }
        public int Active { get; set; }
        public int OrderID { get; set; }
        //辅助属性
        public String ProductName { get; set; }
        public String ItemCode { get; set; }
        public String ItemName { get; set; }
        public String PartName { get; set; }
        public String TypeName { get; set; }
        public String Editor { get; set; }
        public String EditTimeText { get; set; }
        public String ActiveText { get; set; }
    }
}
