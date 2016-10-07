<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Left.aspx.cs" Inherits="DotNetUtilities.Left" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>可折叠的自动伸缩菜单-百洋软件研究实验室</title>
    <script src="js/sdmenu.js" type="text/javascript"></script>
    <link href="css/css.css" rel="stylesheet" type="text/css" />
    
</head>
<body>
    <div id="my_menu" class="sdmenu">
        <div>
            <span>公用类</span> 
            <a href="Commons/WebString.aspx" target="mainframe">字符串操作</a>
             <a href="Default.aspx" target="mainframe">二级菜单</a>
              <a href="#">二级菜单</a>
        </div>
        <div>
            <span>手机配件</span> <a href="#">二级菜单</a> <a href="#">二级菜单</a> <a href="#">二级菜单</a>
        </div>
        <div>
            <span>平板电脑</span> <a href="#">二级菜单</a> <a href="#">二级菜单</a> <a href="#">二级菜单</a>
        </div>
        <div>
            <span>操作系统</span> <a href="#">二级菜单</a> <a href="#">二级菜单</a> <a href="#">二级菜单</a>
        </div>
        <div>
            <span>老人机</span> <a href="#">二级菜单</a>
        </div>
        <div>
            <span>插卡固话</span> <a href="#">二级菜单</a>
        </div>
    </div>
    <!--my_menu end-->
    <script type="text/javascript">
        var myMenu;
        window.onload = function () {
            myMenu = new SDMenu("my_menu");
            myMenu.init();
            var firstSubmenu = myMenu.submenus[0];
            myMenu.expandMenu(firstSubmenu);
        };
    </script>
</body>
</html>
