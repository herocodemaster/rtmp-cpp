<?
/*/
security improved by by TRUI
www.trui.net

Originally posted at www.flashcomguru.com

//*/


//full path to dir with video.
$path = 'C:/.../clips/';


$seekat = $_GET["position"];
$filename = htmlspecialchars($_GET["file"]);
$ext=strrchr($filename, ".");
$file = $path . $filename;


if((file_exists($file)) && ($ext==".flv") && (strlen($filename)>2) && (!eregi(basename($_SERVER['PHP_SELF']), $filename)) && (ereg('^[^./][^/]*$', $filename)))
{
        header("Content-Type: video/x-flv");
        if($seekat != 0) {
                print("FLV");
                print(pack('C', 1 ));
                print(pack('C', 1 ));
                print(pack('N', 9 ));
                print(pack('N', 9 ));
        }
        $fh = fopen($file, "rb");
        fseek($fh, $seekat);
        while (!feof($fh)) {
          print (fread($fh, filesize($file))); 
        }
        fclose($fh);
}
	else
{
        print("ERORR: The file does not exist"); }
?>