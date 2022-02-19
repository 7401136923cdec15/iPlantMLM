using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlantMLM
{
    /// <summary>
    /// 模组编号表
    /// </summary>
    public class SFCModuleNo
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 模组编号
        /// </summary>
        public string ModuleNo { get; set; }
        /// <summary>
        /// 班次
        /// </summary>
        public int ShiftID { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
