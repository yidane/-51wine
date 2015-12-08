define(["TicketModel","TicketStore","TicketView","cUtility","UIScrollLayer","cGeoService","TicketCommon","UIWarning404","TicketConfig","tLazyLoad","GUrlUtil","GFormat","GScrollDrag","GBulkLazyLoad","ctm","ubt","cMemberService","CommonStore","TicketCityService","cHybridFacade"],function(t,i,a,e,s,r,n,o,d,c,l,h,p,f,g,u,m,v,k,T){var L,y,w=require("cGuider"),b=t.ResourceInfoModel.getInstance(),I=require("UIScrollLayer"),C=i.TicketDetailStore.getInstance(),U=(i.TicketListParamStore.getInstance(),i.TicketDetailParamStore.getInstance()),x=i.TicketGeoLocationStore.getInstance(),A=t.SHXProductListSearchV2Model.getInstance(),D=t.ActivitySearch.getInstance(),P=t.TicketViewSuggest.getInstance(),S=t.WeatherReal.getInstance(),B=!1,z=k.getInstance(),E=d.h5.favorite,H=a.extend({name:"detail",pageid:n.pageid("detail"),hpageid:n.hpageid("detail"),hasAd:!0,getAppUrl:function(){return l.schema()},render:function(){this.els={select:this.$el.find("#c-ticket-select"),shx:this.$el.find("#ticket-detail-shx-con"),shxTpl:Lizard.T("c-ticket-shx-temp"),dialog:Lizard.T("c-ticket-dialog-template"),weatherLayerTpl:Lizard.T("c-ticket-detail-weather-layer"),weatherTip:this.$el.find("#c-ticket-detail-weather"),detailContainer:this.$el.find("#ticket-detail-con"),activityContainer:this.$el.find("#ticket-detail-activity-con"),activityTpl:Lizard.T("c-ticket-activity-tmp"),suggestContainer:this.$el.find("#ticket-detail-suggest-con"),suggestTpl:Lizard.T("c-ticket-suggest")}},events:{"click .detail-resinfo-trigger":"showResDetail","click .ticket_book_click":"forwardBooking","click .btn_res":"forwardBooking","click .booking_notice_click_area":"forwardBookingNotice","click #li_user_comment":"forwardComment","click #c-ticket-img-list":"forwardPictureList","click #ticket-detail-map":"forwardMap","click #ticket-detail-attr":"forwardAttr","click #myTry":"errorHandler","click #cui-grayc":"errorCallHandler","click #ticket_shx_list":"jumpToSHX","click .ticket_res_title":"toggleResContent","click .ticket_suggest_li":"ticketSuggestClick","click #tck_get_more_ticket":"getMoreTicketByCid","click .ticket_cancel_btn":"forwardIndex"},onCreate:function(){},initParams:function(){this.favoriteData=[{BizType:"TICKET",ProductType:"",ProductID:this.datas.id,FromCityID:this.datas.gscid}],this.favoriteIds=[],this.isFavorite=!1,C.setAttr({data:this.datas});var t=Lizard.P("from")||"nopage";U.setAttr({from:t}),y=v.UserStore.getInstance().isLogin(),this._initParam()},onShow:function(){if(this.pageManager.setPageName(this.name),this.render(),this.datas.nodata){var t=new cPublic.network({parent:"#ticket-detail-con"});return t.noSearch(function(){var t=['<div class="ticket_info_no">',"暂无相关产品","</div>",'<div class="btn_book mlr15">','<a class="ticket_cancel_btn">查看更多门票</a>',"</div>"].join("");this.$el.find(".loadNosearch-box p").html(t),this.$el.find(".loadNosearch-box").addClass("ticket-nosearch-info")}.bind(this)),this.initParams(),void this.setHeader()}this.init(),this.setHeader();var i=z.getLastGeoCity(),a={viewspotid:this.datas.id,cityid:this.datas.gscid,hash:"detail"};i&&i.id&&(a.cityidfrom=i.id),this.traceLog.log(u.UBT_URL.DETAIL.YYONLOADE,a,u.UBT_KEY.DETAIL_PUB),Lizard.P("traceid")&&(a.pretraceid=Lizard.P("traceid"),a.clickindex=Lizard.P("clickindex"),a.label=Lizard.P("label"),this.traceLog.log(u.UBT_URL.DETAIL.ONLOADE,a,u.UBT_KEY.DETAIL)),this.routerAdapter.triggerAppDragAnimation({ifRegister:!0}),this.scrollDragInit()},onHide:function(){this.scrollDrag&&this.scrollDrag.clear(),A&&A.abort(),L&&L.hide&&L.hide(),B=!1,this.lazyLoad&&this.lazyLoad.destroy()},requestDyWeather:function(){var t=this;t.datas.gscid&&(S.setParam("did",t.datas.gscid),S.excute(function(i){if(i=i.data,i.tpe&&i.wname){var a;a=t.datas.cmtscore>0?['<i class="icon_w'+i.wno+'"></i>']:['<div class="weather_tips"><i class="icon_w'+i.wno+'"></i></div>','<div class="ticket_blue f15">'+i.wname+" "+i.tpe+"℃</div>"],t.els.weatherTip.html(a.join("")),t.els.weatherTip.show()}},function(){},!0))},requestActivity:function(){var t=this,i=t.datas.hasRess;D.setParam({stype:0,sval:this.datas.id,size:"C_130_130",sort:6,limit:20}),D.excute(function(a){if(a=a.data.actses,a&&a.length){for(var s,r,n=a,o=window.location.href,d=o.indexOf("?"),c={},l=[],h=0,p=n.length;p>h;h++){if(!c.hasOwnProperty("k"+n[h].catyid)){var f={catyname:n[h].catyname,catyid:n[h].catyid,sales:0,items:[]};c["k"+n[h].catyid]=f}c["k"+n[h].catyid].sales+=n[h].sovl,c["k"+n[h].catyid].items.push(n[h])}for(var g in c)if(c.hasOwnProperty(g)){if(38===c[g].catyid){for(var u={},m=c[g].items,v=[],k=0;k<m.length;k++)u.hasOwnProperty(m[k].depid)||(u[m[k].depid]=[]),m[k].depname=m[k].depname+"出发",u[m[k].depid].push(m[k]);for(var T in u)u.hasOwnProperty(T)&&(v=v.concat(u[T]));c[g].items=v}l.push(c[g])}l.sort(function(t,i){return t.sales<i.sales?1:t.sales>i.sales?-1:0}),r=e.isInApp?"":o.substring(0,d>0?d:o.length),s=_.template(t.els.activityTpl,{actGrp:l,isInApp:e.isInApp,from:encodeURIComponent(r),hasRess:i}),t.els.activityContainer.html(s)}},function(){},!0)},requestTicketSuggest:function(){var t=this;P.setParam({vsid:this.datas.id,vid:this.getVid(),limit:10,imgsize:"C_280_158"}),P.excute(function(i){if(i=i.data,i&&i.spots){var a=i.spots.length,e=a-a%2,s=i.spots.slice(0,e);t.els.suggestContainer.html(_.template(t.els.suggestTpl,{list:s,hasMore:!t.datas.prds[0].ress.length&&!t.datas.isactpro,from:encodeURIComponent(window.location.href)})),t.updateLazyLoad();for(var r="",n=0;n<s.length;n++)r+=s[n].id,n<s.length-1&&(r+=",");t.traceLog.log(u.UBT_URL.DETAIL.SUGGEST_LOAD,{viewspotid:t.datas.id,hash:"detail",viewspotidlist_recommend:r},u.UBT_KEY.DETAIL)}},function(){},this)},getMoreTicketByCid:function(){this._goto(Lizard.appBaseUrl+"dest/ct-"+(this.datas.prds[0].destcname||"cityname")+"-"+this.datas.prds[0].destcid+"/s-tickets?target=ticket")},updateLazyLoad:function(){this.lazyLoad.destroy(),this.lazyLoad=new f(this,{scrollContainer:this.$el.find(".js_scroll-box")[0],container:this.els.detailContainer,selector:"",type:"",autoProxy:!1,loadingCss:"",attrName:"data-src",diff:200}),this.lazyLoad.load()},ticketSuggestClick:function(t){var i=$(t.currentTarget),a=i.attr("data-id"),e=this;e.traceLog.log(u.UBT_URL.DETAIL.SUGGEST_CLICK,{viewspotid:e.datas.id,hash:"detail",viewspotid_recommend:a},u.UBT_KEY.DETAIL)},init:function(){this.lazyLoad=new f(this,{scrollContainer:this.$el.find(".js_scroll-box")[0],container:this.els.detailContainer,selector:"",type:"",autoProxy:!1,loadingCss:"",attrName:"data-src",diff:200}),this.lazyLoad.load(),this.initParams(),this.requestDyWeather(),this.requestActivity(),this.requestSHX(),this.requestTicketSuggest()},requestSHX:function(){var t=this;A.setParam({viewid:this.datas.id,pIndex:1,pSize:1}),A.excute(function(i){var a=i.data.sdps;if(a&&a.length){var e=_.template(t.els.shxTpl,a[0]);t.els.shx.html(e)}},function(){},!0,this)},jumpToSHX:function(t){var i=t.currentTarget.dataset,a=i.url;e.isInApp?w.jump({targetModel:"app",url:a,title:""}):this.cross(a,{targetModel:2})},showResDetail:function(t){if(B!==!0){var i,a=$(t.currentTarget),s=a.attr("data-resid"),r=a.attr("data-rfactsidx"),n=a.attr("data-index"),o=this.datas.prds[0].ress[n],d=this.datas.prds[0].rfcats[r].rfcatname,c=this,l=!0,h=!0,p=!0;if(o.terminals)for(var f=0;f<o.terminals.length;f++)5==o.terminals[f].tmalid&&(h=o.terminals[f].issale),7==o.terminals[f].tmalid&&(l=o.terminals[f].issale),2==o.terminals[f].tmalid&&(p=o.terminals[f].issale);i=Lizard.isHybrid?h:l,b.setParam({resids:[parseInt(s)],comp:1}),B=!0,c.showLoading(),b.excute(function(t){B=!1,c.hideLoading(),o.rainfos=t.data.ress,o.title=d+"-"+o.name;var a=_.template(c.els.dialog,o),s=['<div class="online_pay">','<span class="ticket_tag_price"><dfn>¥</dfn><i>'+o.price+"</i></span>"];s.push(Lizard.isHybrid&&h===!0||!Lizard.isHybrid&&l===!0?'<button class="btn_res" data-isTip="true" id="ticket-detail-dialogbtn-'+o.id+'" data-rfactsIdx="'+r+'" data-index="'+n+'" data-issale="'+i+'" data-resid="'+o.id+'">预订</button>':h===!1?'<button class="btn_res btn_res_dis" data-isTip="true" id="ticket-detail-dialogbtn-'+o.id+'" data-rfactsIdx="'+r+'" data-index="'+n+'" data-issale="'+i+'" data-resid="'+o.id+'">预订</button>':'<a href="http://m.ctrip.com/m/c362" lizard-catch="off" class="ticket_download">下载APP</a>'),s=s.concat(['<span class="online_pay_text">'+("P"===o.paymode?"在线支付":"景点付款")+"</span>","</div>"]),L=new I({datamodel:{title:'<div class="cui-text-center">详细信息</div>',btns:s.join("")},html:a,events:{"click .btn_res":$.proxy(c.forwardBooking,c),"click a":function(t){var i,a=t.currentTarget;t.preventDefault(),i=$(a).attr("href"),e.isInApp?w.jump({targetModel:"h5",url:i,title:""}):window.location.href=i}}}),L.show()},function(){c.hideLoading(),B=!1,c.showToast({datamodel:{content:"加载失败，请稍后再试"}})},!0)}},forwardPictureList:function(){for(var t,i=this.datas.prds,a=0,e=i.length;e>a;a++)if(i[a].ismaster){t=i[a].id;break}t&&(this.traceLog.log(u.UBT_URL.DETAIL.PIC_CLICK,{viewspotid:this.datas.id,isavailable:this.datas.prds[0].ress.length>0,hash:"detail"},u.UBT_KEY.DETAIL),this._goto(Lizard.appBaseUrl+"picturelist?spotid="+this.datas.id+"&prdid="+t+"&imgs="+this.datas.imgcount))},forwardIndex:function(){this._goto(Lizard.appBaseUrl+"index")},forwardBooking:function(t){var i,a=$(t.currentTarget),e=a.attr("data-resid"),s=a.attr("data-isSale"),r=a.attr("data-isTip");"false"!==s&&(L&&L.hide&&L.hide(),i=Lizard.appBaseUrl+"booking?tid="+e+"&spotid="+this.datas.id,i="true"==r?g.buildCtmUrl(i,"DETAIL_POPBTN"):g.buildCtmUrl(i,"DETAIL_BTN"),this._goto(i))},forwardBookingNotice:function(){for(var t=this.datas.prds[0].painfos,i=0,a=["10","16","31"],e=0;e<t.length;e++)t[e].astcode&&a.indexOf(t[e].astcode)>-1&&i++;i>1&&this._goto(Lizard.appBaseUrl+"bookingnotice")},forwardComment:function(t){if(t.currentTarget.dataset.cmtscore>0){for(var i,a=this.datas.prds,e=0,s=a.length;s>e;e++)if(a[e].ismaster){i=a[e].id;break}i&&this._goto(Lizard.appBaseUrl+"dest/t"+this.datas.id+"/p"+i+"/comment.html?mpid="+i+"&cmtscore="+this.datas.cmtscore+"&cmtusertotal="+this.datas.cmtusertotal+"&name="+this.datas.name+"&from=detail")}},forwardMap:function(){var t,i,a=this.datas.lon,s=this.datas.lat,r=x.get();if(e.isInApp){if(r){var n=r.lng,o=r.lat;t=h.getDistanceByLngLat(a,s,n,o),t=t>=1?"相距"+Math.floor(t)+"km":t>=0?"相距"+Math.floor(1e3*t)+"m":""}var d=require("cHybridShell");return void d.Fn("show_map").run(this.datas.lat,this.datas.lon,this.datas.name,t||"")}i=Lizard.appBaseUrl+"map?name="+this.datas.name+"&lng="+this.datas.lon+"&lat="+this.datas.lat,this.forward(i)},forwardAttr:function(){this._goto(Lizard.appBaseUrl+"jianjie/"+this.datas.id+".html?gscid="+this.datas.gscid+"&destid="+this.datas.prds[0].destcid+"&destname="+this.datas.prds[0].destcname)},toggleResContent:function(){},errorHandler:function(){window.location.reload(!0)},errorCallHandler:function(t){var i="4000086666";return w.apply({hybridCallback:function(){return t.preventDefault(),w.callPhone({tel:i}),!1},callback:function(){window.location.href;return w.jump({tel:i}),setTimeout(function(){window.location.reload()},800),!0}})},scrollDragInit:function(){this.scrollDrag=new p({boxEl:$("#main"),boxOffset:{h5:48,hi:0}})},registerViewBack:function(){var t=this;e.isInApp&&T.register({tagname:T.METHOD_WEB_VEW_DID_APPEAR,callback:function(){T.unregister(T.METHOD_WEB_VEW_DID_APPEAR),t.pageid=n.pageid("detail"),t.hpageid=n.hpageid("detail"),t.sendUbt&&t.sendUbt(),t.pageid=0,t.hpageid=0}})},setHeader:function(){var t=this.datas.name||"门票";this.headerConfig={title:t,back:!0,view:this,tel:null,events:{returnHandler:this.returnHandler.bind(this)}},this.datas.nodata||(e.isInApp?this.headerConfig.right=[{tagname:"favorite",callback:$.proxy(this.favoriteHandler,this)},{tagname:"share",callback:$.proxy(this.shareHandler,this)}]:E&&(this.headerConfig.right=[{tagname:"love",callback:$.proxy(this.favoriteHandler,this)}])),this.header.set(this.headerConfig),!this.datas.nodata&&E&&this.initFavorite()},initFavorite:function(){var t=this;this.collect||(this.collect=new cPublic.collect),"1"==Lizard.P("favorite")&&y&&!this.favoriteTip?(this.favoriteTip=!0,this.favoriteHandler()):y&&this.collect.isMyFavorites({QueryList:this.favoriteData,Channel:e.isInApp?3:2},function(i,a){if(i);else{var e=a.result[0];e===!0&&(t.favoriteIds=a.FavoriteIDs),t.updateFavorite(e)}})},updateFavorite:function(t){var i;i=e.isInApp?[{tagname:t?"favorited":"favorite",callback:$.proxy(this.favoriteHandler,this)},{tagname:"share",callback:$.proxy(this.shareHandler,this)}]:[{tagname:t?"loved":"love",callback:$.proxy(this.favoriteHandler,this)}],this.isFavorite=t,this.header.updateHeader("right",i)},goSignin:function(){var t=this;param="t=1&from="+encodeURIComponent(l.setQueryString(l.local(),"favorite","1")),m.memberLogin({param:param,callback:function(){y=v.UserStore.getInstance().isLogin(),y&&t.favoriteHandler()}})},shareHandler:function(){var t=this.datas.name+"_携程旅行",i="我在携程旅行上发现“"+this.datas.name+"”很赞，推荐给你！",a="http://m.ctrip.com/webapp/ticket/dest/t"+this.datas.id+".html",e=this.datas&&this.datas.imgurls&&this.datas.imgurls[0]||"",s=[{shareType:"Copy",imageUrl:e,title:t,text:"",linkUrl:a},{shareType:"Default",imageUrl:e,title:t,text:i,linkUrl:a}];CtripShare.app_call_custom_share(s)},favoriteHandler:function(){if(!y)return void this.goSignin();if(this.collect){this.traceLog.log(u.UBT_URL.DETAIL.FAVORITE,{viewspotid:this.datas.id,hash:"detail"},u.UBT_KEY.DETAIL);var t=this;this.isFavorite?this.collect.cancel(this.favoriteIds,function(i){i?t.showToast({datamodel:{content:"取消收藏失败，请稍后再试"}}):(t.favoriteIds=[],t.updateFavorite(!1))}):this.collect.save({FavoriteList:this.favoriteData,Channel:e.isInApp?3:2},function(i,a){i?t.showToast({datamodel:{content:"收藏失败，请稍后再试"}}):(t.favoriteIds=a.FavoriteIDs,t.updateFavorite(!0))})}},getVid:function(){var t="",i=this.readCookie("_bfa");if(localStorage.CTRIP_UBT_M)t=JSON.parse(localStorage.CTRIP_UBT_M).vid;else if(i){var a=i.split(".");t=a[1]+"."+a[2]}return t},readCookie:function(t){var i,a,e;i=document.cookie.split("; ");var s={};for(e=i.length-1;e>=0;e--)a=i[e].split("="),s[a[0]]=a[1];return s[t]},returnHandler:function(){var t=this;if(L&&L.$el&&"none"!==L.$el.css("display"))return void L.hide();if(!t._returnHandler()){var i=(U.getAttr("from")||Lizard.P("from")||"nopage").toLowerCase();w.apply({callback:function(){0===i.indexOf("http")||0===i.indexOf("/webapp/")?t.back():i.indexOf("orderdetail")>=0?t.back(Lizard.appBaseUrl+decodeURIComponent(i)):t.pageManager.back()},hybridCallback:function(){w.backToLastPage()}})}},_goto:function(t,i){i=i||{},i.url=t,i.trigger=!0,this.registerViewBack(),this.historyManager.forward(i),this.routerAdapter.forward(i)}});return H});