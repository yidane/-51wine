﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="czGuanhuai.aspx.cs" Inherits="MxWeiXinPF.Web.weixin.wqiche.czGuanhuai" %>


<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <meta content="no-cache,must-revalidate" http-equiv="Cache-Control">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
    <meta name="viewport" content="width=device-width,height=device-height,inital-scale=1.0,maximum-scale=1.0,user-scalable=no">
    <meta name="apple-mobile-web-app-capable" content="yes">
    <meta name="apple-mobile-web-app-status-bar-style" content="black">
    <meta name="format-detection" content="telephone=no">
    <link rel="stylesheet" type="text/css" href="css/reset.css" media="all" />
    <link rel="stylesheet" type="text/css" href="css/common.css" />
    <script type="text/javascript" src="js/jquery_easing.js"></script>
    <script type="text/javascript" src="js/com_clanmo_gallery_min.js"></script>
    <title>车主关怀</title>
    <meta content="text/html; charset=utf-8" http-equiv="Content-Type" />
    <meta content="width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" name="viewport">

    <style>
        img {
            width: 100%!important;
        }
    </style>
</head>
<body onselectstart="return true;" ondragstart="return false;">
    <style>
        .masklayer {
            display: none;
        }

        .on {
            z-index: 1999;
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background: rgba(0,0,0,0.3);
            text-align: right;
            display: block;
        }
    </style>
    <body class="portrait">
        <div class="top">
            <div class="stage_container relative">
                <div class="ofh relative">

                    <ul class="">
                        <li>
                            <img alt="" src="<%=ghModel.newscover %>" style="width: 100%; max-height: 480px;" />
                        </li>
                    </ul>
                    <ol style="position: absolute; line-height: 25px; bottom: 0; z-index: 10; width: 100%; background: rgba(0,0,0,0.2); padding: 5px 10px; color: #fff;">
                        <p><%=ghModel.title %></p>
                    </ol>
                </div>
            </div>
        </div>
        <div class="main" style="padding-top: 10px;">
            <ul class="car_detail">
                <div style="background: -webkit-gradient(linear, 0 0, 0 100%, from(#fff), to(#fff)); padding: 5px;">
                    <%=ghModel.remark %>
                </div>
                <div style="margin: 5px; border: 1px solid #ccc; line-height: 35px; text-indent: 10px; border-radius: 5px; background: -webkit-gradient(linear, 0 0, 0 100%, from(#fff), to(#eee)); -webkit-box-shadow: 0 1px 1px #fff;">
                    <a href="czInfo.aspx?wid=<%=wid %>&openid=<%=openid %>" style="color: #666; display: block; text-align: center;">修改(查看)您的爱车信息</a>
                </div>
                <div>
                    <ul class="list3">
                        <li>
                            <div>
                                <p>保养公里</p>
                                <p>
                                    <img src="img/ok-day.jpg" />
                                </p>
                                <p>公里</p>
                            </div>
                        </li>
                        <li>
                            <div>
                                <p>保险到期</p>
                                <p>
                                    <img src="img/ok-next.jpg" />
                                </p>
                                <p></p>
                            </div>
                        </li>
                        <li>
                            <div>
                                <p>下次年检</p>
                                <p>
                                    <img src="img/ok-360.png" />
                                </p>
                                <p></p>
                            </div>
                        </li>
                    </ul>
                </div>
                <li class="box group_btn">
                    <div><a href="xiaoshouMgr.aspx?wid=<%=wid %>&openid=<%=openid %>" class="gray">联系销售</a></div>
                    <div><a href="yyBaoyang.aspx?type=1&wid=<%=wid %>&openid=<%=openid %>" class="gray">预约保养</a></div>
                </li>
            </ul>
        </div>
        <p>
            <br>
            <br>
            <br>
            <br>
        </p>
    </body>
</html>
