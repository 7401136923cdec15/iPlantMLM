using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlantMLM
{
    public class SFCUploadSOP
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 产品规格
        /// </summary>
        public int ProductID { get; set; }
        /// <summary>
        /// 工位
        /// </summary>
        public int PartID { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// 文件类型(目前只支持1pdf,2jpg)
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 上传时间
        /// </summary>
        public DateTime UploadTime { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public int OperatorID { get; set; }
        /// <summary>
        /// 生效时刻
        /// </summary>
        public DateTime ValidTime { get; set; }
        /// <summary>
        /// 启用状态
        /// </summary>
        public int Active { get; set; }

        //辅助属性
        /// <summary>
        /// 产品规格
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 工位名称
        /// </summary>
        public string PartName { get; set; }
        /// <summary>
        /// 上传时刻
        /// </summary>
        public string UploadTimeText { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        public string TypeText { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string Operator { get; set; }
        /// <summary>
        /// 生效时刻
        /// </summary>
        public string ValidTimeText { get; set; }
        /// <summary>
        /// 启用状态
        /// </summary>
        public string ActiveText { get; set; }
    }
}
