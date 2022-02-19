using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlantMLM
{
    public enum StandardType
    {
        文本 = 0,
        单选 = 1,
        全开区间 = 2,
        全包区间 = 3,
        右包区间 = 4,
        左包区间 = 5,
        小于 = 6,
        大于 = 7,
        小于等于 = 8,
        大于等于 = 9,
        等于 = 10,
        多选 = 11,
        是否 = 15
    }
}
