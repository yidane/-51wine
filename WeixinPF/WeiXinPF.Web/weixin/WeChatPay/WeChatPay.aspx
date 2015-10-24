﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WeChatPay.aspx.cs" Inherits="WeiXinPF.Web.weixin.WeChatPay.WeChatPay" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>微支付</title>
    <meta http-equiv="Content-type" content="text/html; charset=GBK" />
    <meta content="application/xhtml+xml;charset=GBK" http-equiv="Content-Type" />
    <meta content="telephone=no, address=no" name="format-detection" />
    <meta content="width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" name="viewport" />
    <script src="js/zepto.min.js"></script>
    <script src="http://res.wx.qq.com/open/js/jweixin-1.0.0.js"></script>
    <script src="js/WeChatPay_3.0.js"></script>
    <script type="text/javascript">
        var WeChatPayIsReady = true;
        
        function Pay() {
            var unifiedOrderRequest = {
                orderId: <%=OrderID%>,
                wid: parseInt(<%=wid%>),
                body: "<%=body%>",
                attach: "<%=attach%>",
                out_trade_no: "<%=out_trade_no%>",
                total_fee: parseInt(<%=total_fee%>),
                openid: "<%=openid%>"
            };
            WeChatPay(unifiedOrderRequest,'<%=BeforePay%>','<%=PaySuccess%>','<%=PayFail%>','<%=PayCancel%>','<%=PayComplete%>');
            
            if (1=="<%=IsRegister%>") {
                wx.config({
                    debug: false,
                    appId: "<%=appId%>",
                    timestamp: "<%=timestamp%>",
                    nonceStr: "<%=nonceStr%>",
                    signature: "<%=signature%>",
                    jsApiList: [
                            'chooseWXPay',
                            'getNetworkType',
                            'onMenuShareAppMessage',
                            'onMenuShareTimeline',
                            'onMenuShareQQ',
                            'hideOptionMenu'
                    ]
                });
            }

            
            function WeChatPay(UnifiedOrderRequest, beforePay, paySuccess, payFail, payCancel, payComplete) {
                $.ajax({
                    url: "../WeChatPay/WeChatService.asmx/UnifiedOrder",
                    type: "post",
                    dataType: "json",
                    async: false,
                    data: { request: JSON.stringify(UnifiedOrderRequest) },
                    success: function (result) {
                        if (result != null && result.IsSuccess) {
                            wx.chooseWXPay({
                                timestamp: result.Data.timeStamp,
                                nonceStr: result.Data.nonceStr,
                                package: result.Data.package,
                                signType: 'MD5',
                                paySign: result.Data.paySign,
                                success: function (res) {
                                    document.location.href = paySuccess;
                                },
                                fail: function (res) {
                                    document.location.href = payFail;
                                },
                                cancel: function (res) {
                                    document.location.href = payCancel;
                                },
                                complete: function (res) {
                                    //不做处理
                                }
                            });
                        }
                    },
                    error: function (error) {
                    }
                });
            }

            wx.ready(function () {
                Pay();
                //wx.hideOptionMenu();

                //wx.onMenuShareAppMessage({
                //    title: window.share.title,
                //    desc: window.share.desc,
                //    link: window.share.link,
                //    imgUrl: window.share.imgUrl,
                //    success: function () {
                //        //成功回调
                //        window.share.appcallback();

                //    }
                //});

                //wx.onMenuShareTimeline({
                //    title: window.share.title,
                //    link: window.share.link,
                //    imgUrl: window.share.imgUrl
                //});

                //wx.onMenuShareQQ({
                //    title: window.share.title,
                //    desc: window.share.desc,
                //    link: window.share.link,
                //    imgUrl: window.share.imgUrl
                //});

                //wx.error(function (res) {
                //    alert(res.err_code + "______" + res.err_desc + "______" + res.err_msg);
                //});
            });
        }
    </script>
</head>
<body>
    <input type="hidden" id="wid" value="<%=wid %>" />
</body>
</html>
