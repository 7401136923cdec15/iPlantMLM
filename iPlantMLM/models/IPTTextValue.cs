using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlantMLM
{
    /// <summary>
    /// 文本类型值
    /// </summary>
    public class IPTTextValue
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 流水号
        /// </summary>
        public string SerialNumber { get; set; }
        /// <summary>
        /// 标准ID
        /// </summary>
        public int StandardID { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 填写人
        /// </summary>
        public int CreateID { get; set; }
        /// <summary>
        /// 填写时刻
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 班次
        /// </summary>
        public int ShiftID { get; set; }
        /// <summary>
        /// 工位
        /// </summary>
        public int PartID { get; set; }
    }
}
