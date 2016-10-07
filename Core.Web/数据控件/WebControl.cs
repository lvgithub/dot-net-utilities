using System.Data;
using System.Web.UI.WebControls;

namespace Core.Web
{
    public static class WebControls
    {
        #region CheckBoxList

        /// <summary>
        /// 绑顶数据源到指定的CheckBoxList
        /// </summary>
        /// <param name="drp">CheckBoxList控件</param>
        /// <param name="dataSource">数据【DataTable】</param>
        /// <param name="textField">Test</param>
        /// <param name="valueField">Value</param>
        public static void BindCheckBoxList(System.Web.UI.WebControls.CheckBoxList chkbit, DataTable dataSource, string textField, string valueField)
        {
            chkbit.Items.Clear();
            foreach (DataRow dr in dataSource.Rows)
            {
                chkbit.Items.Add(new ListItem(dr[textField].ToString(), dr[valueField].ToString()));
            }
            //chkbit.DataSource = dataSource;
            //chkbit.DataTextField = textField;
            //chkbit.DataValueField = valueField;
            //chkbit.DataBind();
        }
        /// <summary>
        /// 使CheckBoxList中指定项选中
        /// </summary>
        /// <param name="drp">CheckBoxList控件</param>
        /// <param name="itemValue">要匹配的值【Value】</param>
        /// <param name="textValue">匹配项的Text还是Value</param>
        public static void SetCheckBoxList(System.Web.UI.WebControls.CheckBoxList chk, string itemValue)
        {
            foreach (ListItem item in chk.Items)
            {
                if (item.Value == itemValue)
                {
                    item.Selected = true;
                    return;
                }
            }
        }
        /// <summary>
        /// 使CheckBoxList中指定项选中
        /// </summary>
        /// <param name="chk">CheckBoxList的id</param>
        /// <param name="arrayValue">CheckBoxList的值的数组</param>
        public static void SetCheckBoxList(System.Web.UI.WebControls.CheckBoxList chk, string[] arrayValue)
        {
            if (arrayValue.Length > 0)
            {
                for (int i = 0; i < arrayValue.Length; i++)
                {
                    foreach (ListItem item in chk.Items)
                    {
                        if (item.Value == arrayValue[i].ToString())
                        {
                            item.Selected = true;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 使CheckBoxList中指定集合的项选中
        /// </summary>
        /// <param name="chklst">CheckBoxList控件</param>
        /// <param name="itemValues">要匹配的值的集合</param>
        public static void SetSelectedList(System.Web.UI.WebControls.CheckBoxList chklst, string[] itemValues)
        {
            chklst.SelectedIndex = -1;
            foreach (string item in itemValues)
            {
                if (item != null)
                {
                    foreach (ListItem it in chklst.Items)
                    {
                        if (it.Value == item)
                        {
                            it.Selected = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 返回选中CheckBoxList的string[]数组
        /// </summary>
        /// <param name="chklst">CheckBoxList控件</param>
        /// <param name="textValue">返回Value</param>
        public static string GetSelectedList(System.Web.UI.WebControls.CheckBoxList chklst)
        {
            if (chklst.Items.Count == 0)
                return null;
            string items = "";
            foreach (ListItem item in chklst.Items)
            {
                if (item.Selected)
                {
                    items += "," + item.Value;
                }
            }
            if (items == string.Empty)
                return null;
            items = items.Substring(1);
            return items;
        }
        #endregion

        #region DropDownList
        /// <summary>
        /// 绑顶数据源到指定的DropDownList
        /// </summary>
        /// <param name="drp">DropDownList控件</param>
        /// <param name="dataSource">数据【DataTable】</param>
        /// <param name="textField">Test</param>
        /// <param name="valueField">Value</param>
        public static void BindList(System.Web.UI.WebControls.DropDownList drp, DataTable dataSource, string textField, string valueField)
        {
            drp.Items.Clear();
            foreach (DataRow dr in dataSource.Rows)
            {
                drp.Items.Add(new ListItem(dr[textField].ToString(), dr[valueField].ToString()));
            }
        }
        /// <summary>
        /// 自定义绑定DropDownList
        /// </summary>
        /// <param name="drp">DropDownList控件</param>
        /// <param name="dataSource">数据【DataTable】</param>
        /// <param name="textField">Test</param>
        /// <param name="valueField1">Value1</param>
        /// <param name="valueField2">Value2</param>
        public static void BindCustomDropDownList(System.Web.UI.WebControls.DropDownList drp, DataTable dataSource, string textField, string valueField1, string valueField2)
        {
            drp.Items.Clear();
            foreach (DataRow dr in dataSource.Rows)
            {
                drp.Items.Add(new ListItem(dr[textField].ToString(), dr[valueField1].ToString() + "," + dr[valueField2].ToString()));
            }
        }
        /// <summary>
        /// 使DropDownList中指定项选中
        /// </summary>
        /// <param name="drp">DropDownList控件</param>
        /// <param name="itemValue">要匹配的值【Value】</param>
        /// <param name="textValue">匹配项的Text还是Value</param>
        public static void SetSelectedList(System.Web.UI.WebControls.DropDownList drp, string itemValue)
        {
            drp.SelectedIndex = -1;
            foreach (ListItem item in drp.Items)
            {
                if (item.Value == itemValue)
                {
                    item.Selected = true;
                    return;
                }
            }
        }
        /// <summary>
        /// 自定义方式使DropDownList中指定项选中
        /// </summary>
        /// <param name="drp">DropDownList控件</param>
        /// <param name="itemValue">要匹配的值【Value】</param>
        /// <param name="textValue">匹配项的Text还是Value</param>
        public static void SetCustomDropDownList(System.Web.UI.WebControls.DropDownList drp, string itemValue)
        {
            drp.SelectedIndex = -1;
            string[] temtStr;
            foreach (ListItem item in drp.Items)
            {
                temtStr = item.Value.Split(',');
                if (temtStr[1].ToString() == itemValue)
                {
                    item.Selected = true;
                    return;
                }
            }
        }
        /// <summary>
        /// 通过Text 使DropDownList中指定项选中
        /// </summary>
        public static void SetSelectedListByText(System.Web.UI.WebControls.DropDownList drp, string itemText)
        {
            drp.SelectedIndex = -1;
            foreach (ListItem item in drp.Items)
            {
                if (item.Text == itemText)
                {
                    item.Selected = true;
                    return;
                }
            }
        }
        /// <summary>
        /// 返回选中DropDownList的string
        /// </summary>
        /// <param name="drp">DropDownList控件</param>
        /// <param name="textValue">返回Value</param>
        public static string GetSelectedList(System.Web.UI.WebControls.DropDownList drp)
        {
            if (drp.Items.Count == 0)
                return "";
            foreach (ListItem item in drp.Items)
            {
                if (item.Selected)
                {
                    return item.Value;
                }
            }
            return "";
        }
        #endregion

        #region RadioButtonList

        /// <summary>
        /// 绑顶数据源到指定的RadioButtonList
        /// </summary>
        /// <param name="drp">RadioButtonList控件</param>
        /// <param name="dataSource">数据【DataTable】</param>
        /// <param name="textField">Test</param>
        /// <param name="valueField">Value</param>
        public static void BindRadioButtonList(System.Web.UI.WebControls.RadioButtonList rblst, DataTable dataSource, string textField, string valueField)
        {
            rblst.Items.Clear();
            foreach (DataRow dr in dataSource.Rows)
            {
                rblst.Items.Add(new ListItem(dr[textField].ToString(), dr[valueField].ToString()));
            }

            //rblst.DataSource = dataSource;
            //rblst.DataTextField = textField;
            //rblst.DataValueField = valueField;
            //rblst.DataBind();
        }

        /// <summary>
        /// 使RadioButtonList中指定项选中[RadioButtonList只能单选哦]
        /// </summary>
        /// <param name="drp">DropDownList控件</param>
        /// <param name="itemValue">要匹配的值【Value】</param>
        /// <param name="textValue">匹配项的Text还是Value</param>
        public static void SeRadioButtonList(System.Web.UI.WebControls.RadioButtonList rb, string itemValue)
        {
            rb.SelectedIndex = -1;
            foreach (ListItem item in rb.Items)
            {
                if (item.Value == itemValue)
                {
                    item.Selected = true;
                    return;
                }
            }
        }

        /// <summary>
        /// 返回选中RadioButtonList的string
        /// </summary>
        /// <param name="radlst">RadioButtonList控件</param>
        /// <param name="textValue">返回Value</param>
        public static string GetSelectedList(System.Web.UI.WebControls.RadioButtonList radlst)
        {
            if (radlst.Items.Count == 0)
                return "";
            foreach (ListItem item in radlst.Items)
            {
                if (item.Selected)
                {
                    return item.Value;
                }
            }
            return "";
        }
        #endregion

        #region JS让DropdownList、RadioButtonList某一项选中
        //<script type="text/javascript">
        //    $("select#ddl_Store option[value='<%=store %>']").attr('selected', 'true');
        //    $("#rbl_Flag input[value='<%=flag %>']").attr("checked", "checked");
        //</script>

        //HTML:
        //<asp:RadioButtonList runat="server" ID="rbl_Flag" RepeatDirection="Horizontal">
        //    <asp:ListItem Text="不限" Value="-1"></asp:ListItem>
        //    <asp:ListItem Text="入库" Value="10"></asp:ListItem>
        //    <asp:ListItem Text="出库" Value="20"></asp:ListItem>
        //</asp:RadioButtonList>
        #endregion

        #region JS方法获取DropDownList下拉项
        // <script type="text/javascript">
        //    function check()
        //    {
        //        var job_num = document.getElementById("<%=txt_job_num.ClientID %>").value;
        //        var city_select = document.getElementById("<%=DropDownList1.ClientID %>");
        //        var city = city_select.options[city_select.selectedIndex].value;
        //        if(city<0)
        //        {
        //            alert("请选中城市入城市!");
        //            return false;
        //        }
        //        if(job_num=="请输入编号")
        //        {
        //            alert("请输入编号");
        //            return false;
        //        }
        //    }
        //</script>
        #endregion

        #region JS方法获给DropDownList增加一项
        //<script type="text/javascript">
        //    function addOption(){
        //        //根据id查找对象，
        //        var obj=document.getElementById('ddlRoleType');
        //        //添加一个选项(兼容IE与firefox.谷歌)
        //        obj.options.add(new Option("哈哈哈哈","25"));
        //    }
        //</script>

        //<asp:DropDownList runat="server" ID="ddlRoleType">
        //    <asp:ListItem Text="——请选择——" Value="-1" />
        //</asp:DropDownList>
        //<input type="button" id="btn" value="添加" onclick="addOption();" />
        #endregion

        #region JS方法判断CheckBoxList是否有被选中
        //var flag1=0;
        //var checkobj=document.getElementById("chkStoreName");
        //var checks=checkobj.getElementsByTagName("input");
        //for(var n=0;n<checks.length;n++){
        //    if(checks[n].type=="checkbox"&&checks[n].checked==true){
        //        flag1=1
        //    }
        //};
        //if(flag1==0){
        //    alert("请选择管理店铺！");
        //    return false
        //}

        ////绑定的方法上面有的
        //<asp:CheckBoxList runat="server" ID="chkStoreName" RepeatDirection="Horizontal"  Font-Size="Small">                            
        //</asp:CheckBoxList> 
        #endregion

        #region JS方法获取RadioButtonList的Value值
        //var flag1=0;
        //var objZG=document.getElementById("txtPrice").value;
        //var checkobj=document.getElementById("rbConsume");
        //var checks=checkobj.getElementsByTagName("input");
        //for(var n=0;n<checks.length;n++){
        //    if(checks[n].type=="radio"&&checks[n].checked==true){
        //        flag1=parseInt(checks[n].value);
        //    }
        //};
        //if(flag1==10){
        //    if(parseInt(objZG)<=100){
        //        alert("当前类型为充值，优惠折扣不能小于100%哦！");
        //        return false
        //    }
        //}
        //if(flag1==20){
        //    if(parseInt(objZG)>=100){
        //        alert("当前类型为消费，优惠折扣不能大于100%哦！");
        //        return false
        //    }
        //}

        //<asp:RadioButtonList runat="server" ID="rbConsume" RepeatDirection="Horizontal" Width="125px">
        //    <asp:ListItem Value="10" Text="充值" Selected="True" />
        //    <asp:ListItem Value="20" Text="消费" />
        //</asp:RadioButtonList>
        #endregion

        #region Repeater控件添加LinkButton按钮进行删除判断
        //后台方法

        // 页面
        //<asp:LinkButton runat="server" CssClass="LinkButton_sty" ID="lblDelete" CommandName="del">删除</asp:LinkButton>
        //<asp:HiddenField ID="hfGetId" Value='<%#Eval("id") %>' runat="server" />

        #region 删除
        ///// <summary>
        ///// 删除
        ///// </summary>
        ///// <param name="source"></param>
        ///// <param name="e"></param>
        //protected void rpt_Act_ItemCommand(object source, RepeaterCommandEventArgs e)
        //{
        //    if (e.CommandName == "del")
        //    {
        //        HiddenField hfGetId = (HiddenField)e.Item.FindControl("hfGetId");//获取隐藏控件的Id,这是每条新闻的对应Id
        //        int id = BoardGame_Tools.StringHelp.GetInt(hfGetId.Value);
        //        try
        //        {
        //            if (bllActivity.Delete(id))
        //            {
        //                this.rpt_Act.DataSource = GetModelList();
        //                this.rpt_Act.DataBind();
        //                foreach (RepeaterItem rpItem in rpt_Act.Items)
        //                {
        //                    LinkButton lbbDel = (LinkButton)rpItem.FindControl("lblDelete");
        //                    lbbDel.Attributes.Add("onclick", "if(!confirm('确定删除？')) return false;");
        //                }
        //            }
        //        }
        //        catch
        //        {
        //            Response.Write("<script>alert('删除失败');</script>");
        //        }
        //    }
        //}
        #endregion

        //this.rpt_Act.DataSource = GetModelList();
        //this.rpt_Act.DataBind();
        //foreach (RepeaterItem rpItem in rpt_Act.Items)
        //{
        //    LinkButton lbbDel = (LinkButton)rpItem.FindControl("lblDelete");
        //    lbbDel.Attributes.Add("onclick", "if(!confirm('确定删除？')) return false;");
        //}

        //前台JS方法 OnClientClick="return confirm('确定要删除吗?')"
        //<asp:LinkButton runat="server" CssClass="LinkButton_sty" ID="lblDelete" CommandName="del"
        //OnClientClick="return confirm('确定要删除吗?')">删除</asp:LinkButton>
        #endregion

    }
}