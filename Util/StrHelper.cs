using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DroidLord.Util
{
    public static class StrHelper
    {
        public static string URLEncode(string str)
        {
            return System.Web.HttpUtility.UrlEncode(str, Encoding.UTF8);
        }

        public static string URLDecode(string str)
        {
            return System.Web.HttpUtility.UrlDecode(str, Encoding.UTF8);
        }

        public static string Remove(string src, string s, string e)
        {
            int ss = src.IndexOf(s);
            int ee = src.IndexOf(e, ss) + 1;
            return src.Remove(ss, ee - ss);
        }
        /// <summary>
        /// 分离匹配的括号对中间的字符串
        /// </summary>
        /// <param name="Contents">要进行处理的字符串</param>
        /// <param name="left">括号左边界</param>
        /// <param name="right">括号右边界</param>
        /// <param name="nPosStart">开始位置</param>
        /// <param name="nLayer">层次，0代表第一层</param>
        /// <returns>指定层次的括号对中的字符串的集合</returns>
        static public StringCollection GetStrsInMatchedPair(string Contents, string left, string right, int nPosStart, int nLayer)
        {
            KeyValuePair<string, int> Pair;
            StringCollection strList = new StringCollection();
            Stack<KeyValuePair<string, int>> stk = new Stack<KeyValuePair<string, int>>();
            string tmp;

            for (int nPos = 0;
                nPos <= Contents.Length - (left.Length > right.Length ? left.Length : right.Length);
                ++nPos)
            {
                tmp = Contents.Substring(nPos, left.Length);
                if (tmp == left)
                {
                    Pair = new KeyValuePair<string, int>(left, nPos);
                    stk.Push(Pair);

                    nPos += left.Length - 1;
                }

                tmp = Contents.Substring(nPos, right.Length);
                if (tmp == right)
                {
                    if (stk.Count <= 0)
                    {
                        break;
                    }

                    Pair = stk.Pop();
                    if (Pair.Key != left)
                    {
                        break;
                    }

                    if (stk.Count == nLayer)
                    {
                        strList.Add(Contents.Substring(Pair.Value + left.Length, nPos - (Pair.Value + left.Length)));
                    }

                }
            }

            return strList;
        }
        static public StringCollection GetStrsInMatchedPair(string Contents, string left, string right, int nPosStart)
        {
            return GetStrsInMatchedPair(Contents, left, right, nPosStart, 0);
        }

        /// <summary>
        /// 用分隔符来分割最外层的括号对
        /// </summary>
        /// <param name="Contents"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="Separtor"></param>
        /// <param name="nPosStart"></param>
        /// <param name="nLayer"></param>
        /// <returns>如果没有执行分割，返回字符串本身</returns>
        static public StringCollection SplitPairBySepartor(string Contents, string left, string right, string Separtor, int nPosStart)
        {
            KeyValuePair<string, int> Pair;
            StringCollection strList = new StringCollection();
            Stack<KeyValuePair<string, int>> stk = new Stack<KeyValuePair<string, int>>();
            string tmp;
            int nPosTemp = nPosStart;

            for (int nPos = 0;
                nPos <= Contents.Length - (left.Length > right.Length ? left.Length : right.Length);
                ++nPos)
            {
                tmp = Contents.Substring(nPos, left.Length);
                if (tmp == Separtor)
                {
                    // 遇到分隔符
                    if (stk.Count == 0)
                    {
                        tmp = Contents.Substring(nPosTemp, nPos - nPosTemp);
                        strList.Add(tmp);

                        nPosTemp = nPos + Separtor.Length;
                    }

                }

                tmp = Contents.Substring(nPos, left.Length);
                if (tmp == left)
                {
                    Pair = new KeyValuePair<string, int>(left, nPos);
                    stk.Push(Pair);

                    nPos += left.Length - 1;
                }

                tmp = Contents.Substring(nPos, right.Length);
                if (tmp == right)
                {
                    if (stk.Count <= 0)
                    {
                        break;
                    }

                    Pair = stk.Pop();
                    if (Pair.Key != left)
                    {
                        break;
                    }
                }
            }

            if (Contents.Length != nPosTemp || nPosTemp == 0)
            {
                if (stk.Count == 0)
                {
                    tmp = Contents.Substring(nPosTemp, Contents.Length - nPosTemp);
                    strList.Add(tmp);
                }
            }

            return strList;
        }

        /// <summary>
        /// 提取多个在左右边界中间的子串
        /// </summary>
        /// <param name="Contents">要进行提取的字符串</param>
        /// <param name="left">左边界字符串</param>
        /// <param name="right">右边界字符串</param>
        /// <param name="nPosStart">开始位置</param>
        /// <param name="nNum">要提取的子串的个数。如果为０，无限</param>
        /// <returns>子串列表</returns>
        static public StringCollection GetStrsBetween(string Contents, string left, string right, int nPosStart, uint nNum)
        {
            StringCollection lstRet = new StringCollection();

            if (Contents == null || left == null || right == null)
                return lstRet;

            if (nNum == 0)
            {
                for (int nPosEnd = 0;
                    -1 != (nPosStart = Contents.IndexOf(left, nPosStart)) &&
                    -1 != (nPosEnd = Contents.IndexOf(right, nPosStart += left.Length))
                    ;
                    )
                {
                    lstRet.Add(Contents.Substring(nPosStart, nPosEnd - nPosStart));
                }
            }
            else
            {
                for (int nPosEnd = 0;
                    nPosStart < Contents.Length &&
                    -1 != (nPosStart = Contents.IndexOf(left, nPosStart)) &&
                    -1 != (nPosEnd = Contents.IndexOf(right, nPosStart += left.Length)) &&
                    nNum != 0;
                    --nNum, nPosStart = nPosEnd + left.Length
                    )
                {
                    lstRet.Add(Contents.Substring(nPosStart, nPosEnd - nPosStart));
                }
            }

            return lstRet;
        }
        static public StringCollection GetStrsBetween(string Contents, string left, string right, int nPosStart)
        {
            return GetStrsBetween(Contents, left, right, nPosStart, 0);
        }
        static public StringCollection GetStrsBetween(string Contents, string left, string right)
        {
            return GetStrsBetween(Contents, left, right, 0, 0);
        }

        /// <summary>
        /// 提取在左右边界中间的子串
        /// </summary>
        /// <param name="Contents">要进行提取的字符串</param>
        /// <param name="left">左边界字符串</param>
        /// <param name="right">右边界字符串</param>
        /// <returns>子串</returns>
        static public string GetStrBetween(string Contents, string left, string right, int nPosStart)
        {
            StringCollection lstStrings = GetStrsBetween(Contents, left, right, nPosStart, 1);
            if (lstStrings == null || lstStrings.Count == 0)
                return string.Empty;

            return lstStrings[0];
        }
        static public string GetStrBetween(string Contents, string left, string right)
        {
            return GetStrBetween(Contents, left, right, 0);
        }

        /// <summary>
        /// Html解码
        /// </summary>
        /// <param name="Contents">要进行解码的字符串</param>
        /// <param name="strPrefix">包含编码左边界的字符串</param>
        /// <param name="strPostfix">包含编码右边界的字符串</param>
        /// <returns></returns>
        static public string HtmlDecode(string Contents, string strPrefix, string strPostfix)
        {
            StringBuilder strRet = new StringBuilder(100);
            int nNum, i, j;

            for (i = 0; i < Contents.Length;)
            {
                for (j = 0; j < strPrefix.Length && i + j < Contents.Length; ++j)
                {
                    if (strPrefix[j] != Contents[i + j])
                        break;
                }
                if (j == strPrefix.Length)
                {
                    // 匹配前缀

                    nNum = 0;
                    for (i = i + j; i < Contents.Length; ++i)
                    {

                        for (j = 0; j < strPostfix.Length && i + j < Contents.Length; ++j)
                        {
                            if (strPostfix[j] != Contents[i + j])
                                break;
                        }
                        if (j == strPostfix.Length)
                        {
                            // 匹配后缀

                            strRet.Append((char)nNum);
                            i = i + j;
                            break;
                        }
                        else
                        {
                            nNum = nNum * 10 + (Contents[i] - '0');
                        }

                    }
                    // end for

                }
                else
                {
                    strRet.Append(Contents[i]);
                    ++i;
                }
            }
            return strRet.ToString();
        }
        static public string HtmlDecode(string Contents)
        {
            return HtmlDecode(Contents, "&#", ";");
        }

        /// <summary>
        /// char数组转换成string字符串
        /// </summary>
        /// <param name="CharArray"></param>
        /// <returns></returns>
        static public string CharArrayToString(char[] CharArray)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < CharArray.Length; ++i)
                sb.Append(CharArray[i]);

            return sb.ToString();
        }

        /// <summary>
        /// string字符串转换成char数组
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        static public char[] StringToCharArray(string str)
        {
            char[] chArr = new char[str.Length];
            for (int i = 0; i < str.Length; ++i)
                chArr[i] = str[i];

            return chArr;
        }

        enum eState
        {
            Normal,        // 普通
            Integer,        // 整数部分
            Decimal,       // 小数部分
            Negative,     // 负数
        }

        /// <summary>
        /// 取得连续的数字字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        static public StringCollection GetSequentialNums(string str, int nMax)
        {
            eState State = eState.Normal;
            int nPos = 0;
            StringCollection sc = new StringCollection();

            int nCounter = 0;
            int i;
            for (i = 0; i < str.Length; ++i)
            {
                switch (State)
                {
                    case eState.Normal:
                        {
                            // 普通状态

                            if (str[i] >= '0' && str[i] <= '9')
                            {
                                nPos = i;

                                State = eState.Integer;
                            }
                            else if (str[i] == '-')
                            {
                                nPos = i;

                                State = eState.Negative;
                            }
                        }
                        break;

                    case eState.Integer:
                        {
                            // 进入整数部分状态

                            if (!(str[i] >= '0' && str[i] <= '9' || str[i] == '.'))
                            {
                                sc.Add(str.Substring(nPos, i - nPos));
                                nPos = i;

                                State = eState.Normal;

                                ++nCounter;
                                if (nCounter >= nMax)
                                    break;
                            }
                            else if (str[i] == '.')
                            {
                                State = eState.Decimal;
                            }
                        }
                        break;

                    case eState.Decimal:
                        {
                            // 进入小数部分

                            if (!(str[i] >= '0' && str[i] <= '9'))
                            {
                                // 遇到在 0 - 9 闭区间外的字母

                                sc.Add(str.Substring(nPos, i - nPos));
                                nPos = i;

                                State = eState.Normal;

                                ++nCounter;
                                if (nCounter >= nMax)
                                    break;
                            }
                        }
                        break;

                    case eState.Negative:
                        {
                            // 负数

                            if (!(str[i] >= '0' && str[i] <= '9'))
                            {
                                State = eState.Normal;
                            }
                            else
                            {
                                State = eState.Integer;
                            }
                        }
                        break;
                }
            }
            // end for

            // 终结时
            if (State == eState.Integer || State == eState.Decimal)
            {
                sc.Add(str.Substring(nPos, i - nPos));
            }

            return sc;
        }

        static public string GetSequentialNum(string str)
        {
            StringCollection sc = GetSequentialNums(str, 1);
            if (sc.Count == 0)
                return "";

            return sc[0];
        }

        static public string[] Explode(string Seperator, string Haystack)
        {
            return System.Text.RegularExpressions.Regex.Split(Haystack, "(\\r|\\n)+?");
            //Haystack.Replace("\r", ""), Seperator);
        }

        static public string Implode(string[] Elements, string Seperator)
        {
            string Result = "";
            for (int i = 0; i < Elements.Length; i++)
            {
                Result += Elements[i];
                if (i < Elements.Length - 1) Result += Seperator;
            }
            return Result;
        }

        static public string SliceElement(string Str, string Seperator, string Needle)
        {
            string[] Elements = Explode(Seperator, Str);
            List<string> Remain = new List<string>();
            for (int i = 0; i < Elements.Length; i++)
            {
                if (Elements[i] == Needle) continue;
                Remain.Add(Elements[i]);
            }
            string Result = "";
            for (int i = 0; i < Remain.Count; i++)
            {
                Result += Remain[i];
                if (i < Remain.Count - 1) Result += Seperator;
            }
            return Result;
        }

        

        static public bool HasElement(string Str, string Seperator, string Needle)
        {
            string[] Elements = Explode(Seperator, Str);
            List<string> Remain = new List<string>(Elements);
            return Remain.Contains(Needle);
        }
    }
}
