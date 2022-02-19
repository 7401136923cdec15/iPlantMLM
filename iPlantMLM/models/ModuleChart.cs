using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlantMLM
{
    /// <summary>
    /// 模组日报表统计
    /// </summary>
    public class ModuleChart
    {
        public string Date { get; set; }
        public int Total { get; set; }
        public int DTGood { get; set; }
        public int DTBad { get; set; }
        public int MZGood { get; set; }
        public int MZBad { get; set; }
    }
}
