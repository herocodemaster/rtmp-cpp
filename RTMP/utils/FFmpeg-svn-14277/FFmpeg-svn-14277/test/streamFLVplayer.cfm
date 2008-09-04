<cfsetting enablecfoutputonly="YES">
<cfparam name="vidID" default="1">
<cfparam name="vidFile" default="golfers1.flv">
<cfparam name="bgcolor" default="##ffffff">
<cfoutput>
<div class="right">
<object classid="clsid:d27cdb6e-ae6d-11cf-96b8-444553540000" codebase="http://fpdownload.macromedia.com/pub/shockwave/cabs/flash/swflash.cab##version=7,0,0,0" width="320" height="274" id="streamFLVplayer#vidID#">
<param name="allowScriptAccess" value="sameDomain" />
<param name="movie" value="/_interface/common/flash/streamFLVplayer.swf" />
<param name="FlashVars" value="vidID=#vidID#&amp;vidFile=#vidFile#&amp;streamerURL=/_interface/common/flash/streamFLVfeed.php" />
<param name="quality" value="high" />
<param name="bgcolor" value="#bgcolor#" />
<param name="wmode" value="transparent" />
<embed src="/_interface/common/flash/streamFLVplayer.swf"
	FlashVArs="vidID=#vidID#&amp;vidFile=#vidFile#&amp;streamerURL=/_interface/common/flash/streamFLVfeed.php"
	quality="high" bgcolor="#bgcolor#" width="320" height="274" name="streamFLVplayer#vidID#" 
	allowScriptAccess="sameDomain" 
	type="application/x-shockwave-flash" pluginspage="http://www.macromedia.com/go/getflashplayer" />
</object>
</div>
</cfoutput>
<cfsetting enablecfoutputonly="NO">