using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shris.NewEnergy.iPlant.Device
{
    public enum SelectType
    {
        /// <summary>
        /// 默认
        /// </summary>
        [Description("默认")]
        Default = 0,

        /// <summary>
        /// 单选
        /// </summary>
        [Description("单选")]
        SingleSelect = 1,

        /// <summary>
        /// 多选
        /// </summary>
        [Description("多选")]
        MultiSelect = 2
    }
}
