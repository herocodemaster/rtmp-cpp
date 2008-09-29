@echo off

move /Y ../src/*.exe
del flv\output.flv
del log.txt
REM del img\images.txt
REM del img\image_*.jpg

del img\images.txt
copy img\images_en_cero.txt img\images.txt



REM -- Guarda: hay que escapear el caracter '%' usando doble '%%'

ffmpeg -i img\%%03d.jpg -an -r 24 -s 768×576 flv\output.flv
REM ffmpeg -i img\%%03d.jpg -an -r 24 -s 768×576 F:\Development\CPP\RTMP\utils\FFmpeg-svn-14277\FFmpeg-svn-14277\test\output.flv


REM jugar con el parametro '-b 500' --> bitrate
REM -r rate             set frame rate (Hz value, fraction or abbreviation)




pause






REM ffmpeg -i img\%%03d.jpg -ab 56 -ar 22050 -b 500 -r 15 -s 320×240 flv\output.flv
REM ffmpeg -i img\%%03d.jpg -ab 56 -ar 22050 -b 500 -r 15 flv\output.flv
REM ffmpeg -i img\%%03d.jpg -ab 56 -ar 22050 -b 500 -r 15 -s 768×576 flv\output.flv
REM ffmpeg -i img\%%03d.jpg -b 500 -r 15 -s 768×576 flv\output.flv
REM ffmpeg -i img\%%03d.jpg -b 500 -r 1 -s 768×576 flv\output.flv
REM ffmpeg -i img\%%03d.jpg -r 1 -s 768×576 flv\output.flv
