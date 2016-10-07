
namespace Core.Web
{
    public static class ControlHelp
    {
        #region jquery获得select option的值 和对select option的操作
        /*服务器控件dropdownlist再被服务器解析后，客户端呈现为select*/

        //获取Select ：
        //获取select 选中的 text :
        //$("#ddlRegType").find("option:selected").text();

        //获取select选中的 value:
        //$("#ddlRegType").val();

        // 获取select选中的索引:
        // $("#ddlRegType").get(0).selectedIndex;

        //设置select:
        //设置select 选中的索引：
        //$("#ddlRegType").get(0).selectedIndex=index;//index为索引值



        //设置select 选中的value：
        //$("#ddlRegType").attr("value","Normal");
        //$("#ddlRegType").val("Normal");
        //$("#ddlRegType").get(0).value = value;



        // 设置select 选中的text:

        //var count=$("#ddlRegTypeoption").length;
        //  for(var i=0;i<count;i++)  
        //     {
        //        if($("#ddlRegType").get(0).options[i].text == text)  
        //        {  
        //            $("#ddlRegType").get(0).options[i].selected = true;
        //            break;  
        //        }  
        //    }


        //$("#select_id option[text='jQuery']").attr("selected", true);


        //设置select option项:
        // $("#select_id").append("<option value='Value'>Text</option>");  //添加一项option
        // $("#select_id").prepend("<option value='0'>请选择</option>"); //在前面插入一项option
        // $("#select_id option:last").remove(); //删除索引值最大的Option
        // $("#select_id option[index='0']").remove();//删除索引值为0的Option
        // $("#select_id option[value='3']").remove(); //删除值为3的Option
        // $("#select_id option[text='4']").remove(); //删除TEXT值为4的Option

        //清空 Select:
        //$("#ddlRegType").empty();

        #endregion

        #region CheckBox父级与子级联动操作
        //□父 | □子1，□子2，□子3，...

        //外层被td套住
        /* 匹配所有复选框 $(":checkbox") */
        //if($(":checkbox:checked").length<1)
        //{
        //    alert("至少选择一个");
        //    return false;
        //}

        //function CheckChange(obj)
        //{
        //    var _checked = $(obj).attr("checked")?true:false;
        //    $(obj).parents('td').next().find('input').attr('checked',_checked);
        //}
        
        //function aaa(obj)
        //{
        //    var _checked = true;
        //    $(obj).parent().parent().find('input:checked').length==$(obj).parent().parent().find('input').length?true:false;            
        //    $(obj).parents('td').prev().find('input').attr('checked',_checked);
        //}

        //HTML:
        //<asp:Repeater runat="server" ID="rpt_RoleList" 
        //    onitemdatabound="rpt_RoleList_ItemDataBound">
        //    <ItemTemplate>
        //        <tr class=<%#(Container.ItemIndex%2==0)?"\"trnull\"":"\"tr1\""%> >
        //        <td align="right"  style="width: 15%">
        //            <asp:CheckBox runat="server" ID="parentId" Text='<%#Eval("MenuName") %>' Font-Bold="true" onclick="CheckChange(this);"/></td>
        //        <td align="left">
        //            <asp:CheckBoxList runat="server" ID="cbList" RepeatDirection="Horizontal" RepeatLayout="Flow" onclick="aaa(this);">
        //                </asp:CheckBoxList>
        //        </td>
        //    </tr>
        //    </ItemTemplate>
        //</asp:Repeater>

        ///// <summary>
        ///// 根据大类菜单获取小类菜单
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void rpt_RoleList_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        //{
        //    DataTable dt;
        //    string parentId = ((DataRowView)e.Item.DataItem).Row["Id"].ToString();
        //    CheckBoxList childRen = (CheckBoxList)e.Item.FindControl("cbList");
        //    if (childRen != null)
        //    {
        //        dt = common.GetDS("select * from dbo.board_game_SysFiles where ClassId = " + Convert.ToInt32(parentId) + "").Tables[0];
        //        childRen.DataSource = dt.DefaultView;
        //        childRen.DataTextField = "FilesName";
        //        childRen.DataValueField = "Id";
        //        childRen.DataBind();
        //    }
        //}
        #endregion

        #region 从js文件中读取信息
        //思路，构建一个对象写入js，然后读取改对象
        //比如js文件是
        //var JOB_ABC = new Object();
        //JOB_ABC.links = [
        //{text:'大唐无双之猛龙过江12.1再掀PK狂潮',link:'http://gs.163.com/news/2011/11/28/4052_256851.html'},
        //{text:'梦幻潮爆牛仔裤、饰品强力来袭！',link:'http://gift.163.com/2011/11/23/6498_256463.html'},
        //{text:'《精灵来找茬》第一期获奖玩家公布',link:'http://gs.163.com/news/2011/11/23/4052_256473.html'},
        //{text:'欢迎加入大唐无双QQ群119014875',link:'http://gs.163.com/news/2011/11/22/4052_256265.html'},
        //{text:'《精灵传说》预约入驻，注册可夺500万',link:'http://gs.163.com/news/2011/11/18/4052_255977.html'},
        //];

        /*===============读取插入到ul中=======fadeSmooth是一个特效函数，渐出渐隐=====*/
        // $(function() {
        //    var ul = $(".scroll_news > ul");
        //    var i = 0;
        //    for (i; i < JOB_ABC.links.length; i++) {
        //        if (!JOB_ABC.links[i]) continue;
        //        var li = $("<li>");
        //        $("<a>").attr("href", JOB_ABC.links[i].link).html(JOB_ABC.links[i].text).attr("target", "_blank").appendTo(li);
        //        li.appendTo(ul);
        //    }
        //    fadeSmooth($('.scroll_news li'),'onmouseover',4000);
        //    $("#sel option:nth-child(2)").attr("selected" , "selected");
        //    $("#ddlSex option:nth-child(2)").attr("selected","selected");
        //});


        //<ul class="scroll_news" style="margin: 0px; padding: 0px; overflow: hidden;">
        //</ul>

        #endregion

        #region JS特效函数[主要是JQ方面的]

        /*===============平滑过渡函数=================*/
        //说明：obj是对象，on事件，step延迟时间(毫秒)
        //function fadeSmooth(obj,on,step){
        //    var timer = 0,len = obj.length,index = 0;
        //    obj.eq(index).fadeIn('normal');
        //    timer = setInterval(fade,step);
        //    obj.mouseover(function(){
        //        clearInterval(timer);
        //    }).mouseout(function(){
        //        timer=setInterval(fade,step);
        //    });
        //    function fade(){
        //        obj.eq(index).fadeOut('normal',function(){
        //            index = index < len-1 ? index+1 : 0;
        //            obj.eq(index).fadeIn('normal');
        //        })	
        //    }
        //}

        /*===============无缝滚动函数=================*/
        //说明JQ中的hover函数原型:hover(over,out),over与out可以看做2个函数
        // :first =>匹配找到的第一个元素;
        // :first-child => 为每个父元素匹配一个子元素
        // animate动画函数

        //$(function() {
        //    var _wrap = $('ul.style_ul');
        //    var _interval = 3000;
        //    var _moving;
        //    _wrap.hover(function() {
        //        clearInterval(_moving);
        //    }, function() {
        //        _moving = setInterval(function() {
        //            var _field = _wrap.find('li:first');
        //            var _h = _field.height();
        //            _field.animate({ marginTop: -_h + 'px' }, 600, function() {
        //                _field.css('marginTop', 0).appendTo(_wrap);
        //            })
        //        }, _interval)
        //    }).trigger('mouseleave');
        //});

        //上面的函数可等同放到这里
        //$(document).ready(function(){
        //  ...
        //})
        #endregion

        #region 网页卷轴移动[返回首页]
        //多浏览器兼容
        //$(function(){
        //    $('a.abgne_gotoheader').click(function(){
        //        var $body = (window.opera) ? (document.compatMode == "CSS1Compat" ? $('html') : $('body')) : $('html,body');
        //        $body.animate({
        //            scrollTop: 0
        //        }, 600);

        //        return false;
        //    });
        //});

        //<style type="text/css">pre{height: 500px;}</style>

        //<a name="header" id="header">Header</a>
        //<pre></pre >
        //<a href="#header" class="abgne_gotoheader">Go To Header</a>
        //<pre></pre >
        //<a href="#header" class="abgne_gotoheader">Go To Header</a>
        //<pre></pre >
        //<a href="#header" class="abgne_gotoheader">Go To Header</a>
        #endregion

        #region 图片透明度更改
        //<style type="text/css">
        //    .black-div {
        //        background: #000;
        //        width: 426px;
        //        height: 640px;
        //    }
        //</style>

        //$(function(){
        //    // 預設的透明度值
        //    var _opacity = 0.3;

        //    // 讓 .black-div img 的透明度為預設的透明度值
        //    // 接著加入 .hover() 事件
        //    $('.black-div img').css('opacity', _opacity).hover(function(){
        //        // 當滑鼠移入時設為不透明
        //        $(this).stop(false, true).fadeTo(200, 1);
        //    }, function(){
        //        // 當滑鼠移出時設為透明度 0.3
        //        $(this).stop(false, true).fadeTo(200, _opacity);
        //    });
        //});

        //<div class="black-div">
        //    <img src="images/nami.jpg" />
        //</div>
        #endregion

        #region JQ方法处理Tab切换，

        //这两个东西很常用的
        //$(this).addClass('select').siblings().removeClass('select');
        //var index = tab_menu_li.index(this);

        //$(function() {
        //    var tab_menu_li = $('.tab_menu li');
        //    $('.tab_box div:gt(0)').hide();
        //    tab_menu_li.mouseover(function() {
        //        //siblings()在这里的作用是，筛选出除了本身含有class="select"属性的另一个含有该属性的内容
        //        $(this).addClass('select').siblings().removeClass('select');                
        //        var index = tab_menu_li.index(this);
        //        $('.tab_box div').eq(index).show().siblings().hide();
        //    });
        //});

        //<div class="tab">
        //    <div class="tab_menu">
        //        <ul>
        //            <li class="select">tab1</li>
        //            <li>tab2</li>
        //            <li>tab3</li>
        //        </ul>
        //    </div>
        //    <div class="tab_box">
        //        <div>
        //            tab1</div>
        //        <div>
        //            tab2</div>
        //        <div>
        //            tab3</div>
        //        <div>
        //            tab3</div>   
        //    </div>
        //</div>
        #endregion
    }
}
