@echo off

move /Y ../src/*.exe
ffmpeg -i img\%03d.jpg -ab 56 -ar 22050 -b 500 -r 15 -s 320×240 flv\output.flv

pause