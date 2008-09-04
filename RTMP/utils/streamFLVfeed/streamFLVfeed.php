<?
/*/

Further updated by Steve Savage
Added support for using video ID numbers instead of file names.
www.realitystorm.com/experiments/flash/streamingFLV/index.cfm

security improved by by TRUI
www.trui.net

Originally posted at www.flashcomguru.com

//*/


function f_StreamFLV($file,$seekat) {
  $fh = fopen($file, "rb");
  header("Content-Type: video/x-flv");
  if($seekat != 0) {
   fseek($fh, $seekat);
          print("FLV");
          print(pack('C', 1 ));
          print(pack('C', 1 ));
          print(pack('N', 9 ));
          print(pack('N', 9 ));
  }
  while (!feof($fh)) {
    print (fread($fh, 10000)); 
  }
  fclose($fh);
}

function isInteger($x) {
   return ( is_numeric ($x ) ?  intval(0+$x ) ==  $x  :  false ); 
}

$b_secure = true;

$path = '/home/.../';
$a_movies[1] = $path . 'movie1.flv';
$a_movies[2] = $path . 'movie2.flv';
$a_movies[3] = $path . 'movie3.flv';

$vidID = $_GET["vidID"];
$vidPosition = $_GET["vidPosition"];
if($b_secure) {
	// I decided to use a file ID instead of a file name to make the script more secure
	if(isInteger($vidID) && $vidID <= sizeof($a_movies)) {
		$s_file = $a_movies[$vidID];
		f_StreamFLV($s_file,$vidPosition);
	}
}
?>