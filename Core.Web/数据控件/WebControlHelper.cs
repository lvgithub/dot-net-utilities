using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

namespace Core.Web
{
    /// <summary>
    ///     控件类
    /// </summary>
     public static class WebControlHelper
    {
        #region 控件类
        /// <summary>
        /// 绑定数据表格到服务器下拉列表控件
        /// </summary>
        /// <param name="dpt">控件对象</param>
        /// <param name="strValField">值字段</param>
        /// <param name="strTxtField">文本字段</param>
        /// <param name="dTab">数据表格源</param>
        /// <param name="strValSel">默认选择值</param>
        public static void BindDptData(DropDownList dpt, string strValField, string strTxtField, DataTable dTab, string strValSel)
        {
            dpt.DataSource = dTab;
            dpt.DataValueField = strValField;
            dpt.DataTextField = strTxtField;
            dpt.DataBind();
            try
            {
                dpt.SelectedValue = strValSel;
            }
            catch (Exception) { }
        }

        /// <summary>
        /// 根据表单的记录行设置相应服务器文本标签控件的文本
        /// </summary>
        /// <param name="dRow">记录行</param>
        /// <param name="Columns">记录行集参数</param>
        /// <param name="labels">相关文本标签控件</param>
        public static void SetLabelText(ref DataRow dRow, object[] Columns,
            params Label[] labels)
        {
            if (Columns.Length < labels.Length)
            {
                throw new Exception("记录集参数和相关文本标签控件参数不匹配！");
            }
            for (int i = 0; i < labels.Length; i++)
            {
                if (Columns[i].GetType() == typeof(object[]))
                {
                    labels[i].Text = GetFormatStr((object[])Columns[i], ref dRow);
                }
                else
                {
                    labels[i].Text = dRow[Columns[i].ToString()].ToString();
                }
            }
        }

        /// <summary>
        /// 根据表单的记录行设置相应服务器文本框控件的文本
        /// </summary>
        /// <param name="dRow">记录行</param>
        /// <param name="Columns">记录行集参数</param>
        /// <param name="tbxes">相关文本框控件</param>
        public static void SetTextBoxText(ref DataRow dRow, object[] Columns,
            params TextBox[] tbxes)
        {
            if (Columns.Length < tbxes.Length)
            {
                throw new Exception("记录集参数和相关文本标签控件参数不匹配！");
            }
            for (int i = 0; i < tbxes.Length; i++)
            {
                if (Columns[i].GetType() == typeof(object[]))
                {
                    tbxes[i].Text = GetFormatStr((object[])Columns[i], ref dRow);
                }
                else
                {
                    tbxes[i].Text = dRow[Columns[i].ToString()].ToString();
                }
            }
        }

        /// <summary>
        /// 强制客户端控件自动屏蔽HTML标签的 &lt; 和 &gt;
        /// </summary>
        /// <param name="tbxes">相关TextBox服务端控件</param>
        public static void ForbiddenHtmlTag(params TextBox[] tbxes)
        {
            for (int i = 0; i < tbxes.Length; i++)
            {
                tbxes[i].Attributes.Add("onchange", @"this.value=this.value.replace(/\<.[^\<]*\>/gi,'')");
            }
        }

        /// <summary>
        /// 强制客户端控件的值为相关数字
        /// </summary>
        /// <param name="EnableNegative">是否允许负数</param>
        /// <param name="iDefault">默认数字</param>
        /// <param name="tbxes">相关TextBox服务端控件</param>
        public static void ForceNumberValue(bool EnableNegative, int iDefault, params TextBox[] tbxes)
        {
            string strJs = (EnableNegative) ? "javascript:{if (!/^\\-?\\d+$/.test(this.value)) { this.value='" + iDefault.ToString() + "';}}" : "javascript:{if (!/^\\d+$/.test(this.value)) { this.value='" + iDefault.ToString() + "';}}";
            for (int i = 0; i < tbxes.Length; i++)
            {
                tbxes[i].Attributes.Add("onchange", strJs);
            }
        }

        /// <summary>
        /// 根据键值集合设置相应服务器文本框控件的文本
        /// </summary>
        /// <param name="nCol">键值集合</param>
        /// <param name="Columns">键集</param>
        /// <param name="tbxes">相关文本框控件</param>
        public static void SetTextBoxText(ref NameValueCollection nCol, object[] Columns,
            params TextBox[] tbxes)
        {
            if (Columns.Length < tbxes.Length)
            {
                throw new Exception("记录集参数和相关文本标签控件参数不匹配！");
            }
            for (int i = 0; i < tbxes.Length; i++)
            {
                if (Columns[i].GetType() == typeof(object[]))
                {
                    tbxes[i].Text = GetFormatStr((object[])Columns[i], ref nCol);
                }
                else
                {
                    tbxes[i].Text = nCol[Columns[i].ToString()].ToString();
                }
            }
        }

        /// <summary>
        /// 去异常选中服务端下拉列表值
        /// </summary>
        /// <param name="dpt">下拉列表控件</param>
        /// <param name="sValue">下拉列表选中的值</param>
        public static void SetDropDownlistSelect(ref DropDownList dpt, string sValue)
        {
            try
            {
                dpt.SelectedValue = sValue;
            }
            catch (Exception)
            { }
        }

        /// <summary>
        /// 根据表单的记录行设置相应服务器超级链接控件的文本、图片链接、锚属性
        /// </summary>
        /// <param name="dRow">记录行</param>
        /// <param name="Columns">记录行集参数</param>
        /// <param name="links">超级链接控件</param>
        public static void SetHyperLink(ref DataRow dRow, object[] Columns,
            params HyperLink[] links)
        {
            if (Columns.Length < links.Length)
            {
                throw new Exception("记录行参数和相关超级链接控件参数不匹配！");
            }
            for (int i = 0; i < links.Length; i++)
            {
                if (Columns[i].GetType() == typeof(object[]))
                {
                    links[i].Text = GetFormatStr((object[])Columns[i], ref dRow);
                }
                else
                {
                    links[i].Text = dRow[Columns[i].ToString()].ToString();
                }

                // 图片地址
                string strCheck = links[i].ImageUrl;
                if (IsMatchBinding(strCheck))
                {
                    links[i].ImageUrl = GetMatchBinding(strCheck, ref dRow);
                }

                // 链接
                strCheck = links[i].NavigateUrl;
                if (IsMatchBinding(strCheck))
                {
                    links[i].NavigateUrl = GetMatchBinding(strCheck, ref dRow);
                }
            }
        }

        /// <summary>
        /// 查找是否有匹配类似于{"列名"}或{'列名'}项
        /// </summary>
        /// <param name="strBindObject">需要检测的文本串</param>
        /// <returns>是否绑定相关列数据</returns>
        private static bool IsMatchBinding(string strBindObject)
        {
            if (strBindObject == string.Empty)
            {
                return false;
            }
            else
            {
                // 匹配字符中的{"列名"}或{'列名'}项
                // Source: \{(\"|\')[^\"\']+(\"|\')\}
                // Escaped: \\{(\\\"|\\')[^\\\"\\']+(\\\"|\\')\\}
                return Regex.IsMatch(strBindObject, "\\{(\\\"|\\')[^\\\"\\']+(\\\"|\\')\\}", RegexOptions.IgnoreCase);
            }
        }

        /// <summary>
        /// 根据行集转化相应的列数据绑定
        /// </summary>
        /// <param name="strBindObject">需要装化的文本串</param>
        /// <param name="dRow">记录行集参数</param>
        /// <returns>绑定文本输出</returns>
        private static string GetMatchBinding(string strBindObject, ref DataRow dRow)
        {
            // Source:  \{(\"|\')([^\"\']+)(\"|\')\}
            // Escaped: \\{(\\\"|\\')([^\\\"\\']+)(\\\"|\\')\\}
            string strPattern = "\\{(\\\"|\\')([^\\\"\\']+)(\\\"|\\')\\}";
            MatchCollection mc = Regex.Matches(strBindObject, strPattern, RegexOptions.IgnoreCase);
            string strColumnName = string.Empty;
            int iTotal = mc.Count;
            for (int i = 0; i < iTotal; i++)
            {
                if (mc[i].Groups.Count == 4)
                {
                    strColumnName = mc[i].Groups[2].Value;
                    strBindObject = strBindObject.Replace(mc[i].Groups[0].Value, dRow[strColumnName].ToString());
                }
            }
            return strBindObject;
        }

        /// <summary>
        /// 获取相应记录行的格式化字符
        /// </summary>
        /// <param name="objParams">第1个参数为格式化字符参数，其余参数为列名称或其他数据类型</param>
        /// <param name="dRow">记录行引用</param>
        /// <returns>符合相关参数的字符</returns>
        public static string GetFormatStr(object[] objParams, ref System.Data.DataRow dRow)
        {
            string strFmt = objParams[0].ToString();
            object[] paramValue = new object[objParams.Length - 1];
            for (int i = 1; i < objParams.Length; i++)
            {
                if (objParams[i].GetType() == typeof(string))
                {
                    paramValue[i - 1] = dRow[objParams[i].ToString()].ToString();
                }
                else
                {
                    paramValue[i - 1] = objParams[i];
                }
            }
            return String.Format(strFmt, paramValue);
        }

        /// <summary>
        /// 获取相应记录行的格式化字符
        /// </summary>
        /// <param name="objParams">第1个参数为格式化字符参数，其余参数为列名称或其他数据类型</param>
        /// <param name="nCol">键值访问集合</param>
        /// <returns>符合相关参数的字符</returns>
        public static string GetFormatStr(object[] objParams, ref NameValueCollection nCol)
        {
            string strFmt = objParams[0].ToString();
            object[] paramValue = new object[objParams.Length - 1];
            for (int i = 1; i < objParams.Length; i++)
            {
                if (objParams[i].GetType() == typeof(string))
                {
                    paramValue[i - 1] = nCol[objParams[i].ToString()].ToString();
                }
                else
                {
                    paramValue[i - 1] = objParams[i];
                }
            }
            return String.Format(strFmt, paramValue);
        }

        #endregion  控件类

        #region 获取控件文本
        /// <summary>
        /// 获取下拉列表的选项HTML
        /// </summary>
        /// <param name="dTab">数据表格源</param>
        /// <param name="strValField">值字段</param>
        /// <param name="strTxtField">文本字段</param>
        /// <param name="strValSel">默认选择值</param>
        /// <returns>所有option标签的构建</returns>
        public static string GetOptionData(DataTable dTab, string strValField, string strTxtField, string strValSel)
        {
            if (dTab == null)
            {
                return string.Empty;
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                string optfmt = "<option value=\"{0}\"{2}>{1}</option>";
                for (int i = 0; i < dTab.Rows.Count; i++)
                {
                    sb.AppendFormat(optfmt, dTab.Rows[i][strValField],
                        dTab.Rows[i][strTxtField],
                        ((dTab.Rows[i][strValField].ToString() == strValSel) ? " selected" : "")
                        );
                }
                return sb.ToString();
            }
        }


        public static string GetCheckedValue(CheckBoxList cbxList, string Separator)
        {
            ArrayList objList = new ArrayList();
            foreach (ListItem item in cbxList.Items)
            {
                if (item.Selected == true) objList.Add(item.Value);
            }
            return string.Join(Separator, (string[])objList.ToArray(typeof(string)));
        }

        public static void SetCheckedValue(CheckBoxList cbxList, string chkedValue, string Separator)
        {
            string strSearchSource = Separator + chkedValue + Separator;
            foreach (ListItem item in cbxList.Items)
            {
                if (strSearchSource.IndexOf(Separator + item.Value + Separator) != -1) item.Selected = true;
            }
        }
        #endregion
    }
}
