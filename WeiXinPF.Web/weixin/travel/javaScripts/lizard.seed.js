/*
* Ctrip Lizard JavaScript Framework
* Copyright(C) 2008 - 2015, All rights reserved,ctrip.com.
* Date:2015-08-14 14:05:31
* tag:w-2.1-201508141405
*/
function getWebResources(i){for(var t,e=!1,a=i.split(","),n=/(http:|https:)?\/\/webresource[\w\W]+com/g,c=0;c<a.length;c++)t=a[c],n.test(t)&&(comboModules.push(t.replace(n,"")),e=!0);return e}function isH5(){for(var i=["Ctrip_CtripWireless","Unicom_CtripWireless","Pro_CtripWireless","Youth_CtripWireless","gs_wireless"],t=RegExp,e=window.navigator.userAgent,a={},n=0;n<i.length;n++)if(new t(i[n]+"_([\\d.]+)$").test(e)){a.isInCtrip=!0,n==i.length-1&&(a.isGS=!0);break}return a}var isDebug=!!(window.location.href.indexOf("debug=1")>0),scripts=document.getElementsByTagName("script")||[],reg=/lizard\.seed\.(src\.)*js.*$/gi,cdnComboUrl="/res/concat?f=",comboModules=isDebug?["/code/lizard/2.1/web/lizard.core.src.js"]:["/code/lizard/2.1/web/lizard.core.js"],lizardConfig,cruPath="",env=isH5();env.isInCtrip?(comboModules.push("/code/lizard/2.1/web/lizard.hybrid.js"),env.isGS&&comboModules.push("/code/lizard/2.1/web/lizard.web.js")):isDebug||comboModules.push("/code/lizard/2.1/web/lizard.web.js");for(var i=0,tempScript;i<scripts.length;i++){tempScript=scripts[i];var src=tempScript.getAttribute("src");if(src&&reg.test(src)){var filePath=src.replace(reg,"");cruPath=filePath.substr(0,filePath.lastIndexOf("/code/lizard")),cdnComboUrl=cruPath+cdnComboUrl;var pdConfig=tempScript.getAttribute("pdConfig"),lConfig=tempScript.getAttribute("lizardConfig");if(lConfig)try{eval("lizardConfig = {"+lConfig+"}")}catch(e){lizardConfig={}}lizardConfig&&lizardConfig.plainloading?(comboModules.push("/code/lizard/2.1/web/ui/ui.loading.layer.js"),comboModules.push("/code/lizard/2.1/web/ui/ui.warning404.js")):(comboModules.push("/ResCRMOnline/R5/basewidget/ui.loadFailed.js"),comboModules.push("/ResCRMOnline/R5/basewidget/ui.loading.js"));break}}if(isDebug)for(var frameworkModules=comboModules.slice(0,3),i=0;i<frameworkModules.length;i++)document.write("<script src='"+cruPath+frameworkModules[i]+"'></script>");else cdnComboUrl+=comboModules.join(","),document.write("<script src='"+cdnComboUrl+"'></script>");