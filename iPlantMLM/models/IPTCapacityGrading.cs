using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlantMLM
{
    /// <summary>
    /// 容量分档
    /// </summary>
    public class IPTCapacityGrading
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 产品型号
        /// </summary>
        public int ProductID { get; set; }
        /// <summary>
        /// 产品型号
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 档位
        /// </summary>
        public string Gear { get; set; }
        /// <summary>
        /// 下限
        /// </summary>
        public double LowerLimit { get; set; }
        /// <summary>
        /// 下限
        /// </summary>
        public string LowerLimitText { get; set; }
        /// <summary>
        /// 上限
        /// </summary>
        public double UpLimit { get; set; }
        /// <summary>
        /// 上限
        /// </summary>
        public string UpLimitText { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string Explain { get; set; }
    }
}
