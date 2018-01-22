<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AIM.aspx.cs" Inherits="CyberArkAIM.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" style="width: 100%; height: 100%; background-image: url('images/backgroud_aim.jpg');">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 72%;
        }
        .auto-style2 {
            width: 72%;
            height: 30px;
        }
        .auto-style3 {
            width: 36%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
        <asp:RadioButtonList ID="rblProviderFlavor" runat="server" Font-Bold="True" ForeColor="White">
            <asp:ListItem Value="CCP">Central Credential Provider</asp:ListItem>
            <asp:ListItem Value="CP">Credential Provider</asp:ListItem>
        </asp:RadioButtonList>
        
     <table>
         <tr>
             <td style="color: #FFFFFF;text-align:right" class="auto-style3">AppId:</td>
             <td>
                 <asp:TextBox ID="txtAppId" runat="server" ToolTip="Application Name in the CyberArk Vault"></asp:TextBox>
             </td>
         </tr>
         <tr>
            <td style="color: #FFFFFF;text-align:right" class="auto-style3">Safe:</td>
             <td><asp:TextBox ID="txtSafe" runat="server" ToolTip="Name of the safe where the secret is stored in CyberArk."></asp:TextBox></td>
         </tr>
         <tr>
             <td style="color: #FFFFFF;text-align:right" class="auto-style3">Folder:</td>
             <td><asp:TextBox ID="txtFolder" runat="server" ToolTip="Do not change unless you have setup folders in the safe.">Root</asp:TextBox></td>
         </tr>
         <tr>
             <td style="color: #FFFFFF;text-align:right" class="auto-style3">Object:</td>
             <td><asp:TextBox ID="txtObject" runat="server" ToolTip="Object Name of the account in the CyberArk Vault. If using dual accounts, enter the virtual username." />
             </td>
         </tr>
                  <tr>
             <td style="color: #FFFFFF;text-align:right" class="auto-style3">Base URL:</td>
             <td><asp:TextBox ID="txtBaseUrl" runat="server" ToolTip="Only needed for CCP calls, http not required."></asp:TextBox></td>
         </tr>

     </table>
        <table>
                     <tr>
             <td class="auto-style2" ><asp:Button ID="btnSubmit" runat="server" Text="Retrieve Database Time" OnClick="btnSubmit_Click" />&nbsp;&nbsp;&nbsp;
                 <asp:CheckBox ID="cbDualAcct" runat="server" Text ="Dual Account?" ToolTip="CP only" ForeColor="White" /> 
             </td>
         </tr>
         <tr>
             <td class="auto-style1">
                 <asp:Label ID="lblDBResult" runat="server" ForeColor ="White" />
             </td>
         </tr>
        </table>
    </form>
</body>
</html>
