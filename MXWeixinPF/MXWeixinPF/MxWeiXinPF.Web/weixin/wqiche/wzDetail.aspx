﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wzDetail.aspx.cs" Inherits="MxWeiXinPF.Web.weixin.wqiche.wzDetail" %>

<%@ Import Namespace="MxWeiXinPF.Common" %>

<!DOCTYPE html>
<html class=" clickberry-extension clickberry-extension-standalone">
<head>
    <meta charset="utf-8">
    <meta content="no-cache,must-revalidate" http-equiv="Cache-Control">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
    <meta name="viewport" content="width=device-width,height=device-height,inital-scale=1.0,maximum-scale=1.0,user-scalable=no;">
    <meta name="apple-mobile-web-app-capable" content="yes">
    <meta name="apple-mobile-web-app-status-bar-style" content="black">
    <meta name="format-detection" content="telephone=no">
    <link rel="stylesheet" type="text/css" href="css/common.css" media="all">
    <link rel="stylesheet" type="text/css" href="css/car_reset.css" media="all">
    <link rel="stylesheet" type="text/css" href="css/list-8.css" media="all">
    <link rel="stylesheet" type="text/css" href="css/menu-2.css" media="all">
    <script type="text/javascript" src="js/jQuery.js"></script>
    <title><%=aModel.title %></title>
    <style>
        img {
            width: 100%!important;
        }
    </style>
</head>
<body onselectstart="return true;" ondragstart="return false;">
    <link rel="stylesheet" type="text/css" href="./tpl/Wap/default/common/car/css/font-awesome.css" media="all">

    <div class="body" style="padding-bottom: 60px;">
        <footer class="nav_footer">
            <ul class="box">
                <li><a href="javascript:history.go(-1);">返回</a></li>
                <li><a href="javascript:history.go(1);">前进</a></li>
                <li><a href="index.aspx?wid=<%=wid %>&openid=<%=openid %>">首页</a></li>
                <li><a href="javascript:location.reload();">刷新</a></li>
            </ul>
        </footer>

        <section class="news_article">
            <header>
                <h3 style="font-size: 14px;"><%=aModel.title %></h3>
                <small class="gray"><%=MyCommFun.Obj2DateTime(aModel.add_time).ToString("yyyy-MM-dd") %></small>
            </header>
            <article>
                <p>
                    <img src="<%=aModel.img_url %>" style="width: 100%;">
                </p>
                <p>
                    <%=aModel.zhaiyao %>
                </p>
                <p></p>
            </article>
        </section>
        <div style="padding-bottom: 0!important;">
            <a href="javascript:window.scrollTo(0,0);" style="font-size: 12px; margin: 5px auto; display: block; color: #fff; text-align: center; line-height: 35px; background: #333; margin-bottom: -10px;">返回顶部</a>
        </div>
    </div>
    <div mark="stat_code" style="width: 0px; height: 0px; display: none;">
    </div>
</body>
</html>
