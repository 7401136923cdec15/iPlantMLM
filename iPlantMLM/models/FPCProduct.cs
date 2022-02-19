using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlantMLM
{
    public class FPCProduct
    {
        /// <summary>
        /// 产品ID
        /// </summary>
        public int ProductID { get; set; }
        /// <summary>
        /// 产品规格
        /// </summary>
        public String ProductName { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        public String Name { get; set; }
        /// <summary>
        /// 型号
        /// </summary>
        public String Model { get; set; }
        /// <summary>
        /// 产品描述
        /// </summary>
        public String DescribeInfo { get; set; }
        /// <summary>
        /// 生产阶段代码
        /// </summary>
        public String ProductCode { get; set; }
        /// <summary>
        /// 金风二维码前缀
        /// </summary>
        public string BarCodePrefix { get; set; }
        /// <summary>
        /// 电容包编码前缀
        /// </summary>
        public string PackagePrefix { get; set; }
    }
}
