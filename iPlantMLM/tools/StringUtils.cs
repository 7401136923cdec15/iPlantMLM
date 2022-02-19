using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlantMLM
{
    public class StringUtils
    {
        public static int ParseInt(Object value)
        {
            if (value != null)
            {
                if (value is Int32)
                {
                    return (Int32)value;
                }
                else if (value is String)
                {
                    Int32 wResult = 0;
                    Int32.TryParse((String)value, out wResult);
                    return wResult;
                }
            }
            return 0;
        }

        public static float ParseFloat(Object value)
        {

            if (value != null)
            {
                if (value is Single)
                {
                    return (Single)value;
                }
                else if (value is String)
                {
                    Single wResult = 0;
                    Single.TryParse((String)value, out wResult);
                    return wResult;
                }
            }
            return 0;
        }

        public static long ParseLong(Object value)
        {
            if (value != null)
            {
                if (value is Int64)
                {
                    return (Int64)value;
                }
                if (value is Int32)
                {
                    return (Int32)value;
                }
                if (value is String)
                {
                    long wResult = 0L;
                    Int64.TryParse((String)value, out wResult);
                    return wResult;
                }
                if (value is DateTime)
                {
                    return Convert.ToInt64(((DateTime)value - new DateTime(1970, 1, 1)).TotalMilliseconds);
                }
            }
            return 0L;
        }

        public static Double ParseDouble(Object value)
        {
            if (value != null)
            {
                if (value is Double)
                {
                    return (Double)value;
                }
                if (value is Decimal)
                {
                    return (Double)Decimal.Parse(value.ToString());
                }
                if (value is Int32)
                {
                    return (Double)((Int32)value);
                }
                else if (value is String)
                {
                    Double wResult = 0;
                    Double.TryParse((String)value, out wResult);
                    return wResult;
                }
            }
            return (double)0;
        }

        public static String ParseString(Object value)
        {
            if (value != null)
            {
                return value.ToString();
            }
            return null;
        }

        public static List<T> ParseList<T>(Object value)
        {
            List<T> wTList = null;
            try
            {
                if (value != null)
                {
                    if (value is System.Collections.ArrayList)
                    {
                        wTList = new List<T>();
                        foreach (var obj in (System.Collections.ArrayList)value)
                        {
                            wTList.Add((T)obj);
                        }
                        return wTList;
                    }
                    else if (value is List<T>)
                    {
                        return (List<T>)value;
                    }
                    else if (value is Array)
                    {
                        wTList = new List<T>();
                        foreach (var obj in (Array)value)
                        {
                            wTList.Add((T)obj);
                        }
                        return wTList;
                    }
                    else
                    {
                        wTList = (List<T>)value;
                    }
                }
            }
            catch (Exception ex)
            {
                ShrisTool.LoggerTool.SaveException("StringUtils", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString());
            }
            return wTList;
        }

        public static Boolean ParseBoolean(Object value)
        {
            if (value != null)
            {
                if (value is Int32)
                {
                    return ((Int32)value) == 1;
                }
                else if (value is String)
                {
                    return "1" == ((String)value) || "true" == ((String)value);
                }
                else if (value is Boolean)
                {
                    return (Boolean)value;
                }
            }
            return false;
        }

        public static String JsonDateCutLong(String wJson)
        {

            // @"\\/Date\((\d+)(\+?)(\d+)\)\\/"
            String wRegexString = "\"\\\\/Date\\((\\d+)(\\+?)(\\d+)\\)\\\\/\"";

            wJson = System.Text.RegularExpressions.Regex.Replace(wJson, wRegexString, match =>
            {
                return match.Groups[1].Value;
            });
            return wJson;
        }

        public static String JsonDateCutString(String wJson)
        {

            wJson = System.Text.RegularExpressions.Regex.Replace(wJson, @"\\/Date\((\d+)(\+?)(\d+)\)\\/", match =>
            {
                DateTime wDt = new DateTime();
                wDt.AddMilliseconds(Convert.ToInt64(match.Groups[1].Value));

                return wDt.ToString("yyyy-MM-dd HH:mm:ss");
            });
            return wJson;
        }

        public static DateTime ParseDate(Object value)
        {
            DateTime wResult = new DateTime(1970, 1, 1);

            if (value is long)
            {
                wResult.AddMilliseconds((long)value);
            }
            else if (value is Int32)
            {
                wResult.AddMilliseconds((Int32)value);
            }
            else if (value is String)
            {
                DateTime.TryParse((string)value, out wResult);
            }
            else if (value is DateTime)
            {
                wResult = (DateTime)value;
            }


            return wResult;
        }

        public static List<Decimal> ParseNumberList(String[] wStringArray) 
        {
            List<Decimal>  wResult=new List<decimal>();
            if (wStringArray == null || wStringArray.Length < 1)
                return wResult;
            Decimal  wDecimalTemp=0;
            foreach (String wString in wStringArray)
            {
                if (Decimal.TryParse(wString, out wDecimalTemp))
                    wResult.Add(wDecimalTemp);
            }

            return wResult;
        }
    }
}
