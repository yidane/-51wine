﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="action_user.aspx.cs" Inherits="WeiXinPF.Web.admin.cashred.action_user" %>

<%@ Import Namespace="WeiXinPF.Common" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>现金红包活动管理</title>
    <script type="text/javascript" src="../../scripts/jquery/jquery-1.10.2.min.js"></script>
    <script type="text/javascript" src="../../scripts/lhgdialog/lhgdialog.js?skin=idialog"></script>
    <script type="text/javascript" src="../js/layout.js"></script>
       <link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
     <link href="../skin/mystyle.css" rel="stylesheet" type="text/css" />
    <link href="../../css/pagination.css" rel="stylesheet" type="text/css" />

    
</head>

<body class="mainbody">
    <form id="form1" runat="server">
        <!--导航栏-->
        <div class="location">
            <a href="javascript:;" class="home"><i></i><span>现金红包活动管理</span></a>
        </div>
        <!--/导航栏-->

        <!--工具栏-->
        <div class="toolbar-wrap">
            <div id="floatHead" class="toolbar">
                <div class="l-list">
                    <ul class="icon-list">
                       
                        <li><a class="all" href="javascript:;" onclick="checkAll(this);"><i></i><span>全选</span></a></li>
                        <li>
                            <asp:LinkButton ID="btnDelete" runat="server" CssClass="del" OnClientClick="return ExePostBack('btnDelete');" OnClick="btnDelete_Click"><i></i><span>删除</span></asp:LinkButton></li>
                    </ul>
                </div>
                <div class="l-list">
                活动名称：<asp:Label ID="lblActName" runat="server" Text=""></asp:Label>
                    </div>
                <div class="r-list">
                   
                    <asp:TextBox ID="txtKeywords" runat="server" CssClass="keyword" />
                    <asp:LinkButton ID="lbtnSearch" runat="server" CssClass="btn-search" OnClick="btnSearch_Click">查询</asp:LinkButton>

                     
                </div>
            </div>
        </div>
        <!--/工具栏-->

        <!--列表-->

        <asp:Repeater ID="rptList" runat="server" >
            <HeaderTemplate>
                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="ltable">
                    <thead>
                        <tr>
                            <th width="5%">选择</th>
                             <th width="12%">粉丝用户的openid</th>
                              <th width="10%">用户昵称</th>
                              <th width="10%">获得红包金额</th>
                            <th width="10%">获得时间</th>
                            <th width="12%">商户订单号</th>
                            <th width="5%">红包状态</th>
                            <th >失败原因</th>
                        </tr>
                    </thead>
                    <tbody class="ltbody">
            </HeaderTemplate>
            <ItemTemplate>
                <tr class="td_c">
                    <td>
                        <asp:CheckBox ID="chkId" CssClass="checkall" runat="server" Style="vertical-align: middle;" />
                        <asp:HiddenField ID="hidId" Value='<%#Eval("id")%>' runat="server" />
                    </td>
                    <td>
                        <%# Eval("openid") %>
                    </td>
                      <td>
                        <%# Eval("userNick") %>
                    </td>
                     <td>
                        <%#jine( Eval("total_amount")) %>
                    </td>
                     
                     <td>
                        <%# Eval("Send_time") %> 
                    
                    </td>
                    
                    <td>
                          <%# Eval("mch_billno") %>
                    </td>
                     <td>
                          <%# Eval("hbstatus") %>
                    </td>
                    
                     <td>
                       <p title=" <%#   Eval("reason").ToString() %>"> <%#Utils.CutString( Utils.ShowXmlStr( Eval("reason").ToString()),120) %><%# Eval("Refund_time") %>
                           </p>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                <%#rptList.Items.Count == 0 ? "<tr><td align=\"center\" colspan=\"8\">暂无记录</td></tr>" : ""%>
                 </tbody>
                </table>
            </FooterTemplate>
        </asp:Repeater>

        <!--/列表-->

        <!--内容底部-->
        <div class="line20"></div>
        <div class="pagelist">
            <div class="l-btns">
                <span>显示</span><asp:TextBox ID="txtPageNum" runat="server" CssClass="pagenum" onkeydown="return checkNumber(event);" OnTextChanged="txtPageNum_TextChanged" AutoPostBack="True"></asp:TextBox><span>条/页</span>
            </div>
            <div id="PageContent" runat="server" class="default"></div>
        </div>
        <!--/内容底部-->
    </form>
</body>
</html>

