﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="logo_edit.aspx.cs" Inherits="WeiXinPF.Web.admin.wqiche.logo_edit" ValidateRequest="false" %>

<%@ Import Namespace="WeiXinPF.Common" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>编辑</title>
    <script type="text/javascript" src="../../scripts/jquery/jquery-1.10.2.min.js"></script>
    <script type="text/javascript" src="../../scripts/jquery/Validform_v5.3.2_min.js"></script>
    <script type="text/javascript" src="../../scripts/lhgdialog/lhgdialog.js?skin=idialog"></script>
    <script type="text/javascript" src="../../scripts/datepicker/WdatePicker.js"></script>
    <script type="text/javascript" src="../../scripts/swfupload/swfupload.js"></script>
    <script type="text/javascript" src="../../scripts/swfupload/swfupload.queue.js"></script>
    <script type="text/javascript" src="../../scripts/swfupload/swfupload.handlers.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../editor/kindeditor-min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../editor/lang/zh_CN.js"></script>
    <script type="text/javascript" src="../js/layout.js"></script>
    <link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
    <link href="../skin/mystyle.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(function () {
            //初始化表单验证
            $("#form1").initValidform();

            //初始化上传控件
            $(".upload-img").each(function () {
                $(this).InitSWFUpload({ sendurl: "../../tools/upload_ajax.ashx", flashurl: "../../scripts/swfupload/swfupload.swf" });
            });
            $(".upload-album").each(function () {
                $(this).InitSWFUpload({ btntext: "批量上传", btnwidth: 66, single: false, water: true, thumbnail: true, filesize: "2048", sendurl: "../../tools/upload_ajax.ashx", flashurl: "../../scripts/swfupload/swfupload.swf", filetypes: "*.jpg;*.jpge;*.png;*.gif;" });
            });
            $(".attach-btn").click(function () {
                showAttachDialog();
            });

            //初始化编辑器
            var editor = KindEditor.create('.editor', {
                width: '75%',
                height: '100px',
                resizeType: 1,
                uploadJson: '../../tools/upload_ajax.ashx?action=EditorFile&IsWater=1',
                fileManagerJson: '../../tools/upload_ajax.ashx?action=ManagerFile',
                allowFileManager: true
            });
            var editorMini = KindEditor.create('.editor-mini', {
                width: '75%',
                height: '100px',
                resizeType: 1,
                uploadJson: '../../tools/upload_ajax.ashx?action=EditorFile&IsWater=1',
                items: [
                    'fontname', 'fontsize', '|', 'forecolor', 'hilitecolor', 'bold', 'italic', 'underline',
                    'removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
                    'insertunorderedlist', '|', 'emoticons', 'image', 'link']
            });
        });
    </script>
</head>

<body class="mainbody">
    <form id="form1" runat="server">
        <!--导航栏-->
        <div class="location">
            <a href="logoMgr.aspx" class="home"><i></i><span>品牌列表</span></a>
            <i class="arrow"></i>
            <span>编辑品牌</span>
        </div>
        <div class="line10"></div>
        <!--/导航栏-->

        <!--内容-->
        <div class="content-tab-wrap">
            <div id="floatHead" class="content-tab">
                <div class="content-tab-ul-wrap">
                    <ul>
                        <li><a href="javascript:;" onclick="tabs(this);" class="selected">基本内容</a></li>
                    </ul>
                </div>
            </div>
        </div>

        <div class="tab-content">
            <dl>
                <dt>*品牌名称：</dt>
                <dd>
                    <asp:TextBox ID="txtName" runat="server" CssClass="input normal" datatype="*1-500" sucmsg=" " Text=""  />
                    <span class="Validform_checktip">* 尽量简单，不要超过20字</span>
                </dd>
            </dl>
            <dl>
                <dt>官方网站：</dt>
                <dd>
                    <asp:TextBox ID="txtWebsite" runat="server" CssClass="input normal"  sucmsg=" " Text="" />
                    <span class="Validform_checktip">请输入品牌官方网站的完成url地址,如：http://www.bmw.com.cn (没有可以留空)</span>
                </dd>
            </dl>
            <dl>
                <dt>LOGO:</dt>
                <dd>
                    <asp:Image ID="imgLogo" runat="server" ImageUrl="~/images/noneimg.jpg" Style="max-height: 80px;" />
                    <asp:TextBox ID="txtLogo" runat="server" CssClass="input normal upload-path" />
                    <div class="upload-box upload-img"></div>
                    <span class="Validform_checktip">（建议图片尺寸420*60，不超过300K</span>
                </dd>
            </dl>
            <dl>
                <dt>显示顺序:</dt>
                <dd>
                    <asp:TextBox ID="txtSort_id" runat="server" CssClass="input small" datatype="n" sucmsg=" " Text="99" />
                    <span class="Validform_checktip"></span>
                </dd>
            </dl>
            <dl>
                <dt>品牌简介：</dt>
                <dd>
                    <textarea id="txtSummary" class="editor" style="visibility: hidden;" runat="server"></textarea>
                </dd>
            </dl>
        </div>
        <!--/内容-->

        <!--工具栏-->
        <div class="page-footer">
            <div class="btn-list">
                <asp:Button ID="btnSubmit" runat="server" Text="提交保存" CssClass="btn" OnClick="btnSubmit_Click" />
                <a href="javascript:history.back(-1);"><span class="btn yellow">返回上一页</span></a>
            </div>
            <div class="clear"></div>
        </div>
        <!--/工具栏-->
    </form>
</body>
</html>
