using ShrisTool;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace iPlantMLM
{
    public class EnumTool
    {

        #region 单实例
        private EnumTool() { }
        private static EnumTool _Intance;

        public static EnumTool Intance
        {
            get
            {
                if (_Intance == null)
                    _Intance = new EnumTool();
                return EnumTool._Intance;
            }
        }
        #endregion

        /// <summary>
        /// 得到枚举的中文注释
        /// </summary>
        public string GetEnumDesc<T>(String wName)
        {
            string wResult = "未知";
            try
            {
                T wT = (T)Enum.Parse(typeof(T), wName);

                MemberInfo[] wMemberInfoArray = wT.GetType().GetMember(Enum.GetName(typeof(T), wT));
                if (wMemberInfoArray == null || wMemberInfoArray.Length < 1)
                    return wResult;

                DescriptionAttribute[] wAttrs = wMemberInfoArray[0].GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

                if (wAttrs.Length > 0)
                    wResult = wAttrs[0].Description;
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
            return wResult;
        }

        /// <summary>
        /// 得到枚举的中文注释
        /// </summary>
        public string GetEnumDesc<T>(object wValue)
        {
            string wResult = "未知";
            try
            {
                string wEnumName = Enum.GetName(typeof(T), wValue);
                wResult = GetEnumDesc<T>(wEnumName);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
            return wResult;
        }

        /// <summary>
        /// 得到枚举的中文注释
        /// </summary>
        public string GetEnumDesc<T>(T wT)
        {
            string wResult = "未知";
            try
            {
                MemberInfo[] wMemberInfoArray = wT.GetType().GetMember(Enum.GetName(typeof(T), wT));
                if (wMemberInfoArray == null || wMemberInfoArray.Length < 1)
                    return wResult;

                DescriptionAttribute[] wAttrs = wMemberInfoArray[0].GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

                if (wAttrs.Length > 0)
                    wResult = wAttrs[0].Description;
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
            return wResult;
        }

        public Dictionary<T, String> ToList<T>()
        {
            Dictionary<T, String> wList = new Dictionary<T, String>();
            try
            {
                MemberInfo[] wMemberInfos = typeof(T).GetMembers();
                foreach (MemberInfo wMemberInfo in wMemberInfos)
                {
                    DescriptionAttribute[] wAttrs = wMemberInfo.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

                    if (wAttrs == null || wAttrs.Length < 1)
                        continue;
                    T wT;
                    try
                    {
                        wT = (T)Enum.Parse(typeof(T), wMemberInfo.Name);
                    }
                    catch (Exception)
                    {
                        continue;
                    }

                    if (wT == null || wList.ContainsKey(wT))
                        continue;
                    wList.Add(wT, wAttrs[0].Description);
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
            return wList;
        }
    }
}
