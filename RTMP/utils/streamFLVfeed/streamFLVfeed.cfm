<cfsetting enablecfoutputonly="NO">
<!--- 
Description: streams Flash FLV movie files.

Credits:

This code was created by Steve Savage
  Streaming FLV data via ColdFusion MX 7 (perhaps 6.1, but untested): 
	http://www.realitystorm.com/experiments/flash/streamingFLV/index.cfm
	http://www.realitystorm.com/contact/index.cfm

Based on work by:

Stefan Richter
	Streaming FLV data via PHP: http://www.flashcomguru.com/index.cfm/2005/11/2/Streaming-flv-video-via-PHP-take-two

Christian Cantrell
	Byte Arrays: http://weblogs.macromedia.com/cantrell/archives/2004/01/byte_arrays_and_1.cfm
	Write Out Binary Data to Browser: http://weblogs.macromedia.com/cantrell/archives/2003/06/using_coldfusio.cfm

Buraks:
	Frameposition metadata encoder: http://www.buraks.com/flvmdi/ 
	
Modifications:
1.0 - initial release code place on realitystorm.com and given to the http://www.rich-media-project.com/
	
--->
	
<!--- the function that sends the feed --->
<cfscript>
	function f_StreamFLV(s_file,i_seek) {
		var i_position = i_seek;
		var i_buffer = 10000;
		var byteClass = createObject("java", "java.lang.Byte"); //
		var byteArray = createObject("java","java.lang.reflect.Array").newInstance(byteClass.TYPE, i_buffer);
		var context = getPageContext();
		var response = context.getResponse().getResponse(); 
		var flvinstream = createObject("java", "java.io.FileInputStream"); 
		var flvoutstream = response.getOutputStream(); // take over control of the feed to the browser
		byteClass.Init(1);
		flvinstream.init(s_file);
		context.setFlushOutput(false);
		try {
			if(i_seek GT 0) {
				flvinstream.skip(i_seek);
				flvoutstream.write(toBinary('RkxWAQEAAAAJAAAACQ==')); // output the header bytes
			}
			do {
				i_length = flvinstream.read(byteArray,0,i_buffer);
				if (i_length neq -1) {
					flvoutstream.write(byteArray);
					flvoutstream.flush();
				}
			} while (i_length neq -1); // keep going until there's nothing left to read.
		}
		catch(any excpt) {}
		flvoutstream.flush(); // send any remaining bytes
		response.reset(); // reset the feed to the browser
		flvoutstream.close(); // close the stream to flash
		flvinstream.close(); // close the file stream
	}
</cfscript>

<!--- set to false to allow the file name to be passed, true for a numeric ID --->
<cfset b_secure = true>
<cfparam name="vidPosition" default="0">
<cfif isNumeric(vidPosition) AND vidPosition GT 0><cfset i_seek = vidPosition><cfelse><cfset i_seek = 0></cfif>

<!--- create an array for possible movies, or this could read an XML file, or the vidID could be used to reference a database record --->
<cfset s_path = "#Replace(application.root,"\","/","All")#experiments/flash/streamingFLV/"> 
<cfset a_movies=ArrayNew(1)>
<cfset a_movies[1] = "#s_path#golfers1.flv">
<cfset s_file = a_movies[1]> <!--- set the default movie ---> 

<cfif b_secure>
	<!--- I decided to use a file ID instead of a file name to make the script more secure --->
	<cfif isDefined("url.vidID") and isNumeric(url.vidID) and url.vidID LTE arrayLen(a_movies)>
		<cfset s_file = a_movies[url.vidID]>
	</cfif>
<cfelseif isDefined("url.vidFile") AND NOT b_secure AND fileExists("#s_path##url.vidFile#")>
	<cfset s_file = "#s_path##url.vidFile#">
</cfif>
<cfset f_StreamFLV(s_file,i_seek)>
<cfsetting enablecfoutputonly="YES">

