<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebString.aspx.cs" Inherits="DotNetUtilities.Commons.WebString" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table width="100%" border="1" cellpadding="1" cellspacing="1">
       
        <tr>
            <td style="width: 100px">
            </td>
            <td >
               
                <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="字符串过长截取" />
                <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
               
            </td>
        </tr>
     
        <tr>
            <td style="width: 100px">
            </td>
            <td >
              
                <asp:Button ID="Button2" runat="server" Text="全角半角" 
                    onclick="Button2_Click" />
                <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>
              
            </td>
        </tr>
        <tr>
            <td style="width: 100px">
            </td>
            <td>
               
                <asp:Button ID="Button3" runat="server" Text="Button" onclick="Button3_Click" />
                <asp:Label ID="Label3" runat="server" Text="Label"></asp:Label>
               
            </td>
        </tr>

        <tr>
            <td style="width: 100px">
            </td>
            <td>
               
                <asp:Button ID="Button4" runat="server" Text="Button" />
                <asp:Label ID="Label4" runat="server" Text="Label"></asp:Label>
               
            </td>
        </tr>

        <tr>
            <td style="width: 100px">
            </td>
            <td>
              
                <asp:Button ID="Button5" runat="server" Text="Button" />
                <asp:Label ID="Label5" runat="server" Text="Label"></asp:Label>
              
            </td>
        </tr>

        <tr>
            <td style="width: 100px">
            </td>
            <td>
               
                <asp:Button ID="Button6" runat="server" Text="Button" />
                <asp:Label ID="Label6" runat="server" Text="Label"></asp:Label>
               
            </td>
        </tr>
    </table>
    </div>
    </form>
</body>
</html>
