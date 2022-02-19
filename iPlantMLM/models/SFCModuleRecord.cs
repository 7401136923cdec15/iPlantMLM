using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlantMLM
{
    public class SFCModuleRecord
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 测试流水号
        /// </summary>
        public string SerialNumber { get; set; }
        /// <summary>
        /// 电容包编号
        /// </summary>
        public string CapacitorPackageNo { get; set; }
        /// <summary>
        /// 首工位上线时间
        /// </summary>
        public DateTime OnlineTime { get; set; }
        /// <summary>
        /// 模组编号
        /// </summary>
        public string ModuleNumber { get; set; }
        /// <summary>
        /// 末工位下线时间
        /// </summary>
        public DateTime OfflineTime { get; set; }
        /// <summary>
        /// 当前工位
        /// </summary>
        public int CurrentPartID { get; set; }
        /// <summary>
        /// 出现次数
        /// </summary>
        public int Times { get; set; }
        /// <summary>
        /// 装箱条码
        /// </summary>
        public string BarCode { get; set; }
        /// <summary>
        /// 装托条码
        /// </summary>
        public string TrustBarCode { get; set; }
        /// <summary>
        /// 档位
        /// </summary>
        public string Gear { get; set; }
        /// <summary>
        /// 班次信息
        /// </summary>
        public int ShiftID { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public int CreateID { get; set; }
        /// <summary>
        /// 创建时刻
        /// </summary>
        public DateTime CreateTime { get; set; }

        //辅助属性
        /// <summary>
        /// 当前工位
        /// </summary>
        public string CurrentPartName { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string Creator { get; set; }
        /// <summary>
        /// 产品型号
        /// </summary>
        public int ProductID { get; set; }
        /// <summary>
        /// 激活、禁用
        /// </summary>
        public int Active { get; set; }

        /// <summary>
        /// 容量
        /// </summary>
        public String Capacity { get; set; }
        /// <summary>
        /// 直流内阻
        /// </summary>
        public string InternalResistance { get; set; }
        /// <summary>
        /// 绝缘电阻1
        /// </summary>
        public string InsulationInternalResistance1 { get; set; }
        /// <summary>
        /// 绝缘电阻2
        /// </summary>
        public string InsulationInternalResistance { get; set; }
        /// <summary>
        /// 交流内阻1
        /// </summary>
        public string InternalImpedance1 { get; set; }
        /// <summary>
        /// 交流内阻2
        /// </summary>
        public string InternalImpedance { get; set; }
        /// <summary>
        /// 外观质量
        /// </summary>
        public string AppearanceQuality { get; set; }
        /// <summary>
        /// 单体电压检测结果
        /// </summary>
        public string MonomerVoltage { get; set; }
        /// <summary>
        /// 耐压测试
        /// </summary>
        public string WithstandVoltageTest { get; set; }
        /// <summary>
        /// 电容包装配完成度
        /// </summary>
        public string CapacityCompletion { get; set; }
        /// <summary>
        /// 装配完成度
        /// </summary>
        public string AssemblyCompletion { get; set; }

        /// <summary>
        /// 是否合格
        /// </summary>
        public int IsQuality { get; set; }
        /// <summary>
        /// 是否合格
        /// </summary>
        public string IsQualityText { get; set; }
        /// <summary>
        /// 激活状态
        /// </summary>
        public string ActiveText { get; set; }
        /// <summary>
        /// 三串测试结果
        /// </summary>
        public string SCTestResult { get; set; }

        //PCB编号
        public string PCBNo { get; set; }
        //单体编号
        public string SingleNo { get; set; }
        //R1阻值
        public string R1Number { get; set; }
        //R2阻值
        public string R2Number { get; set; }
        //静置电压
        public string StandingVoltage { get; set; }
        //静置时间
        public string StandingHour { get; set; }
        //模组标签/档位信息
        public string LableInfo { get; set; }
    }
}
