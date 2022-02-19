using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlantMLM
{
    /// <summary>
    /// 参数
    /// </summary>
    public class MESParams
    {
        /// <summary>
        /// 充电电压
        /// </summary>
        public double ChargingVoltage { get; set; }
        /// <summary>
        /// 充电时长
        /// </summary>
        public double ChargingTime { get; set; }
        /// <summary>
        /// 放电时长
        /// </summary>
        public double DischargeDuration { get; set; }
    }
}
