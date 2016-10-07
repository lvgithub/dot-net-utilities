<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="DotNetUtilities.Index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<!--<meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />-->
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=gbk" />
    <title></title>
</head>
<frameset cols="*" rows="10, *,10" id="frame_main" framespacing="1" bordercolor="blue">
    <frame src="Top.aspx" noresize="noresize" name="header">
    <frameset cols="200, *" border="1px">
        <frame src="Left.aspx" scrolling="yes" name="left" noresize="noresize"  />
        <frame src="Commons/WebString.aspx" id="mainframe" name="mainframe" noresize="noresize">
    </frameset>
     <frame src="Top.aspx" noresize="noresize" name="header">
</frameset>
<noframes>
    <body>
    </body>
</noframes>
</html>
