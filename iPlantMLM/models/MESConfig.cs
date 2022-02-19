using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlantMLM
{
    public class MESConfig
    {
        /// <summary>
        /// 当前产品规格
        /// </summary>
        public int CurrentProduct { get; set; }
        /// <summary>
        /// 当前工位
        /// </summary>
        public string CurrentPart { get; set; }
    }
}
