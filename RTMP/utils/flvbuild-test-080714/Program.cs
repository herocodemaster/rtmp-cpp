/*
FLV Builder
Copyright (C) 2008  zappa_engine

This program is free software; you can redistribute it and/or modify it
under the terms of the GNU General Public License as published by the
Free Software Foundation; either version 2 of the License, or (at your
option) any later version.

This program is distributed in the hope that it will be useful, but
WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
for more details.

You should have received a copy of the GNU General Public License along
with this program; if not, write to the Free Software Foundation, Inc.,
59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.

*/
using System;
using System.IO;
using CommandLine.Utility;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;
// Here's an idea for high bit rate videos. Load a still frame first, and do padding in front of the video
// instead of the backend of the video. This first frame can act as a "Loading..." screen.
// EXAMPLE
// 0-15 sec: Pre-loading high bitrate video. Please enjoy these screen shots while waiting...
// 16-30 sec: First screen shot
// 31-45 sec: Second screen shot
// ... etc
// 90-105 sec: Sixth screen shot
// 106-120 sec: Thank you for waiting. The featured video is about to start...
// Based on a calculated download speed of say 640kbps, you can guess how long the pre-load will show
// and inform the watcher. That way there is no jerky video.
// A 27MB file may have a pre-load time of 5 minutes, 30 seconds. (27000kB/80kB=seconds)
// Of course this makes more work for the user to generate those screens, or it could be
// generated automatically from the FLV itself ;)

// Okay trying other experimental techniques
// Thumb frame with photo
// Audio frame (MPEG V2.5, L3, Bitrate type 1, Mono?)
// Repeat? Not necessary I think...
// Rejector frame (1x2, 4x3, any others that work)
// Audio frame (MPEG V2.5, L3, Bitrate type 1, Mono?)
// Main Video copied here
// Audio pad frame at the end = skip to end
// HMM, actually I don't see anything new here except audio frames interwined.

// Determine which keyframe/interframe plays if two of them occur on same timestamp? Is one not shown?
// Determine timestamp location of thumbnails for injection of custom thumbnails without affecting main video



namespace FLV_Builder
{
    class Program
    {
        static string input_flv_file;
        static string output_flv_file;
        static bool padding_needed = false;
        static double expected_duration;
        static long duration;
        static int flg_tag_count;
        static Arguments CommandLine;
        struct flv_file_info
        {
            public static int width;
            public static int height;
        }

        static void Main(string[] args)
        {
            const string VERSION = "080707";
        //    const string PROGRAM_EXE = "\"FLV Reader.exe\"";
            Console.WriteLine("FLV Builder, Released under the GNU General Public License.");
            Console.WriteLine("Version " + VERSION + ". Copyright (c) 2008  zappa_engine");
            Console.WriteLine("\n\n***** (private build - there are compatibility issues between flash 8 and 9)\n");
            Console.WriteLine("");

            // http://www.codeproject.com/KB/recipes/command_line.aspx
            CommandLine = new Arguments(args);

            // Look for specific arguments values and display them if they exist (return null if they don't)
            if (CommandLine["i"] != null)
            {
                input_flv_file = CommandLine["i"];
                Console.WriteLine("Input file: " + CommandLine["i"]);
            }
            else
            {
                /*
                Console.WriteLine("Usage: " + PROGRAM_EXE + " -input <file.flv>");
                Console.WriteLine("Options: -output <file.flv>");
                Console.WriteLine("Options: -thumbsize 480x360");
                Console.WriteLine("Options: -padsize 480x360");
                Console.WriteLine("Options: -youtube"); // Thumbsize defaults calculated
                Console.WriteLine("Options: -info");
                Console.WriteLine("Options: -video_audio_presence 5");
                return;
                */
                input_flv_file = "video.flv";
                Console.WriteLine("No input FLV file detected. Using default " + input_flv_file);
            }
            if (CommandLine["o"] != null)
            {
                output_flv_file = CommandLine["o"];
                Console.WriteLine("Output file: " + CommandLine["o"]);
            }
            else
            {
                output_flv_file = "video_only.flv";
                Console.WriteLine("No output FLV file detected. Using default "+output_flv_file);
            }

            /*
            if (CommandLine["o"] != null) Console.WriteLine("Output file: " + CommandLine["o"]);
            else Console.WriteLine("Output file not defined !");

            if (CommandLine["thumbsize"] != null) Console.WriteLine("thumbsize value: " + CommandLine["thumbsize"]);
            else Console.WriteLine("Thumbnail frame resolution not defined !");

            if (CommandLine["padsize"] != null) Console.WriteLine("padsize value: " + CommandLine["padsize"]);
            else Console.WriteLine("Padding resolution not defined !");

            if (CommandLine["padtime"] != null) Console.WriteLine("padtime value: " + CommandLine["padtime"]);
            else Console.WriteLine("Padding time not defined !");

            if (CommandLine["youtube"] != null) Console.WriteLine("youtube value: " + CommandLine["youtube"]);
            else Console.WriteLine("Youtube not defined !");

            if (CommandLine["info"] != null) Console.WriteLine("info value: " + CommandLine["info"]);
            else Console.WriteLine("info not defined !");

            if (CommandLine["video_audio_presence"] != null) Console.WriteLine("video_audio_presence value: " + CommandLine["video_audio_presence"]);
            else Console.WriteLine("video_audio_presence not defined !");

            if (CommandLine["keep_metadata"] != null) Console.WriteLine("keep_metadata value: " + CommandLine["keep_metadata"]);
            else Console.WriteLine("keep_metadata not defined !");
            */
            Console.WriteLine("");

            BinaryReader binaryIn;
            char signature_one;
            char signature_two;
            char signature_three;
            byte flv_version;
            byte video_audio_presence;
            uint flv_header_size;

            uint tag_size;
            byte tag_type;

            byte[] tag_data_size;

            byte[] timestamp;
            byte timestamp_extended;

            byte[] stream_id;

            byte[] tag_data;

            // Open the FLV file.
            try
            {
                binaryIn = new BinaryReader(new FileStream(input_flv_file, FileMode.Open));
            }
            catch(FileNotFoundException exc)
            {
                Console.WriteLine(exc.Message + "\nCan not open file.");
                return;
            }



            binaryIn.BaseStream.Position = 0;
            Console.WriteLine();



            try
            {
                /*
                Console.WriteLine("Reading FLV header...");

                signature_one = binaryIn.ReadChar();
                Console.Write("Signature " + signature_one);
                signature_two = binaryIn.ReadChar();
                Console.Write(signature_two);
                signature_three = binaryIn.ReadChar();
                Console.WriteLine(signature_three);
                flv_version = binaryIn.ReadByte();
                Console.WriteLine("Version " + flv_version);
                video_audio_presence = binaryIn.ReadByte();
                Console.WriteLine("Video/Audio presence code " + video_audio_presence);

                if (("" + signature_one + signature_two + signature_three) != "FLV")
                {
                    Console.WriteLine("Not a FLV input file detected. Exiting...");
                    return;
                }
                flv_header_size = binaryIn.ReadUInt32();
                //Console.WriteLine("Reading " + flv_header_size);

                // Flipping the bit order for 32 bit
                uint tmpInt = flv_header_size;
                tmpInt = (uint)(((tmpInt & 0x000000ff) << 24) +
                  ((tmpInt & 0x0000ff00) << 8) +
                  ((tmpInt & 0x00ff0000) >> 8) +
                  ((tmpInt & 0xff000000) >> 24));
                Console.WriteLine("Header size " + tmpInt);

                Console.WriteLine("Done reading FLV header!");
                Console.WriteLine();

                Console.WriteLine("Reading first tag size...");
                tag_size = binaryIn.ReadUInt32();
                //Flipping
                tmpInt = tag_size;
                tmpInt = (uint)(((tmpInt & 0x000000ff) << 24) +
                  ((tmpInt & 0x0000ff00) << 8) +
                  ((tmpInt & 0x00ff0000) >> 8) +
                  ((tmpInt & 0xff000000) >> 24));
                Console.WriteLine("Tag size " + tmpInt);
                Console.WriteLine();






                Console.WriteLine("Now reading first FLV tag...");
                tag_type = binaryIn.ReadByte();
                Console.Write("Tag type " + tag_type + " " );
                if (tag_type == 18)
                {
                    Console.WriteLine("[METADATA]");
                } else if (tag_type == 8) {
                    Console.WriteLine("[AUDIO]");
                } else if (tag_type == 9)
                {
                    Console.WriteLine("[VIDEO]");
                }
                else
                {
                    Console.WriteLine("[Unknown tag type!]");
                    return;
                }
                tag_data_size = binaryIn.ReadBytes(3);
                long result_tds = getBitValueFromByteArray(tag_data_size, 0, 24);
                Console.WriteLine("Tag Size of Data " + result_tds);

                
                timestamp = binaryIn.ReadBytes(3);
                long result_timestamp = getBitValueFromByteArray(timestamp, 0, 24);
                Console.WriteLine("Timestamp " + result_timestamp);
                timestamp_extended = binaryIn.ReadByte();
                Console.WriteLine("Timestamp extended " + timestamp_extended);

                stream_id = binaryIn.ReadBytes(3);
                Console.WriteLine("StreamID (always 0) " + stream_id[0] + "," + stream_id[1] + "," + stream_id[2]);

                
                parse_tag_type(binaryIn, tag_type, binaryIn.BaseStream.Position);
                // The rest is now TAG DATA
                tag_data = binaryIn.ReadBytes((int)result_tds);
                Console.WriteLine("Tag Data " + tag_data[0] + "," + tag_data[1] + "," + tag_data[2] + ", etc...");

                Console.WriteLine("Reading previous tag size...");
                tag_size = binaryIn.ReadUInt32();
                //Flipping
                tmpInt = tag_size;
                tmpInt = (uint)(((tmpInt & 0x000000ff) << 24) +
                  ((tmpInt & 0x0000ff00) << 8) +
                  ((tmpInt & 0x00ff0000) >> 8) +
                  ((tmpInt & 0xff000000) >> 24));
                Console.WriteLine("Tag size " + tmpInt);
                Console.WriteLine();
                */
                if (CommandLine["info"] != null)
                {
                    binaryIn.BaseStream.Position = 13;
                    flg_tag_count = 0;
                    while (read_FLV_tag(binaryIn, binaryIn.BaseStream.Position) > 0)
                    {
                        flg_tag_count++;
                    }
                    Console.WriteLine("There were " + flg_tag_count + " total FLV tags found.");
                }

            }
            catch (IOException exc)
            {
                Console.WriteLine(exc.Message + "\nCan not read file.");
            }


            binaryIn.Close();


            calculate_flv_info(input_flv_file);
            youtube_patch();
            //calculate_flv_info(output_flv_file);
        }
        static void youtube_patch3()
        {
            create_flv_stub(output_flv_file, 5);
            for (int i = 1; i<2597; i++) {
                append_flv_tag(".\\tags\\"+i + ".flv.tag", output_flv_file);
                
            }
            generate_custom_audio_flv_tag("audio.flv.tag", 120000, 128);
            //append_flv_tag("audio.flv.tag", output_flv_file);

        }
        static void youtube_patch2()
        {
            //Pulling out tags from sucessful videos,
            //Marking out the unique ones,
            //Combining them to test things out

            //a442.flv.tag,v788.flv.tag,a26480.flv.tag,a29590.flv.tag
            //v29762.flv.tag,a30569.flv.tag,a33679.flv.tag
            //v33851.flv.tag,a34400.flv.tag,a37337.flv.tag
            //MAIN,v37510.flv.tag,a37570.flv.tag
            create_flv_stub(output_flv_file, 5);
            
            //
            append_flv_tag(".\\htags\\audio\\a442.flv.tag", output_flv_file);
            append_flv_tag(".\\htags\\audio\\a615.flv.tag", output_flv_file);
            append_flv_tag(".\\htags\\video\\v788.flv.tag", output_flv_file);
            append_flv_tag(".\\htags\\audio\\a26480.flv.tag", output_flv_file);
            append_flv_tag(".\\htags\\audio\\a26653.flv.tag", output_flv_file);
            append_flv_tag(".\\htags\\audio\\a26826.flv.tag", output_flv_file);
            append_flv_tag(".\\htags\\audio\\a26999.flv.tag", output_flv_file);
            append_flv_tag(".\\htags\\audio\\a27171.flv.tag", output_flv_file);
            append_flv_tag(".\\htags\\audio\\a27344.flv.tag", output_flv_file);
            append_flv_tag(".\\htags\\audio\\a27517.flv.tag", output_flv_file);
            append_flv_tag(".\\htags\\audio\\a27690.flv.tag", output_flv_file);
            append_flv_tag(".\\htags\\audio\\a27862.flv.tag", output_flv_file);
            append_flv_tag(".\\htags\\audio\\a28035.flv.tag", output_flv_file);
            append_flv_tag(".\\htags\\audio\\a28208.flv.tag", output_flv_file);
            append_flv_tag(".\\htags\\audio\\a28381.flv.tag", output_flv_file);
            append_flv_tag(".\\htags\\audio\\a28553.flv.tag", output_flv_file);
            append_flv_tag(".\\htags\\audio\\a28726.flv.tag", output_flv_file);
            append_flv_tag(".\\htags\\audio\\a28899.flv.tag", output_flv_file);
            append_flv_tag(".\\htags\\audio\\a29071.flv.tag", output_flv_file);
            append_flv_tag(".\\htags\\audio\\a29244.flv.tag", output_flv_file);
            append_flv_tag(".\\htags\\audio\\a29417.flv.tag", output_flv_file);
            append_flv_tag(".\\htags\\audio\\a29590.flv.tag", output_flv_file);
            //

            
            //
            append_flv_tag(".\\htags\\video\\v29762.flv.tag", output_flv_file);
            append_flv_tag(".\\htags\\audio\\a30569.flv.tag", output_flv_file);
            append_flv_tag(".\\htags\\audio\\a33679.flv.tag", output_flv_file);
            //

            
            //
            append_flv_tag(".\\htags\\video\\v33851.flv.tag", output_flv_file);
            append_flv_tag(".\\htags\\audio\\a34400.flv.tag", output_flv_file);
            append_flv_tag(".\\htags\\audio\\a37337.flv.tag", output_flv_file);
            //



            /*
            // Main Video
            append_flv_tag(".\\htags\\video\\v37510.flv.tag", output_flv_file);
            append_flv_tag(".\\htags\\audio\\a37570.flv.tag", output_flv_file);

            append_flv_tag(".\\htags\\video\\v37592.flv.tag", output_flv_file);
            append_flv_tag(".\\htags\\audio\\a75442.flv.tag", output_flv_file);
            append_flv_tag(".\\htags\\audio\\a75476.flv.tag", output_flv_file);
            append_flv_tag(".\\htags\\video\\v75562.flv.tag", output_flv_file);
            append_flv_tag(".\\htags\\audio\\a81404.flv.tag", output_flv_file);
            append_flv_tag(".\\htags\\audio\\a99324.flv.tag", output_flv_file);
            append_flv_tag(".\\htags\\video\\v99695.flv.tag", output_flv_file);
            */


            generate_custom_video_flv_tag("thumbnail.flv.tag", 300000, 0, 480, 360);
            append_flv_tag("thumbnail.flv.tag", output_flv_file);
            append_flv_file(input_flv_file, output_flv_file, 4000);
            // Final pad is an audio padded tag masked by a video tag of the real video (normal)duration
            //generate_custom_audio_flv_tag("audio.flv.tag", (long)Math.Ceiling(expected_duration / 1000) * 1000, 128);
            //append_flv_tag("audio.flv.tag", output_flv_file);
            //generate_custom_video_flv_tag("video.flv.tag", duration+4000, 0, 480,360);
            //append_flv_tag("video.flv.tag", output_flv_file);

        }

        static void youtube_patch()
        {

            int time_stamp_interval = 1; // 52 for H264, not needed for H263 but lazy to take it out and just set 1
            int pad_multiplier = 1; // 20 for H264
            Console.WriteLine("\n\n\n[RUNNING YOUTUBE PATCH SCRIPT]\n");
            create_flv_stub(output_flv_file, 5);

            generate_custom_video_flv_tag("thumbnail.flv.tag", 0, 0, 480, 360);
            append_flv_tag("thumbnail.flv.tag", output_flv_file);
            //generate_custom_video_flv_tag_with_ffmpeg(0, 640, 360);
            //append_flv_tag("custom.ffmpeg.flv.tag", output_flv_file);

            generate_custom_audio_flv_tag("audio.flv.tag", 0, 128);
            append_flv_tag("audio.flv.tag", output_flv_file);


            generate_custom_video_flv_tag("rejector.flv.tag", pad_multiplier * time_stamp_interval, 0, 4, 3);
            append_flv_tag("rejector.flv.tag", output_flv_file);
            //generate_custom_video_flv_tag_with_ffmpeg((pad_multiplier) * time_stamp_interval, 16, 12);
            //append_flv_tag("custom.ffmpeg.flv.tag", output_flv_file);

            for (int i = (pad_multiplier * 1); i < (pad_multiplier * 2); i++)
            {
                generate_custom_audio_flv_tag("audio.flv.tag", i * time_stamp_interval, 128);
                append_flv_tag("audio.flv.tag", output_flv_file);
            }



            generate_custom_video_flv_tag("aspect.flv.tag", (2*pad_multiplier) * time_stamp_interval, 0, 480, 360);
            append_flv_tag("aspect.flv.tag", output_flv_file);
            //generate_custom_video_flv_tag_with_ffmpeg((pad_multiplier * 2) * time_stamp_interval, 640, 272);
            //append_flv_tag("custom.ffmpeg.flv.tag", output_flv_file);

            for (int i = (pad_multiplier * 2); i < (pad_multiplier * 3); i++)
            {
            generate_custom_audio_flv_tag("audio.flv.tag", i * time_stamp_interval, 128);
            append_flv_tag("audio.flv.tag", output_flv_file);
            }

            append_flv_file(input_flv_file, output_flv_file, (pad_multiplier * 3) * time_stamp_interval);



            if (padding_needed)
            {
                Console.WriteLine("Padding needed");
                generate_custom_audio_flv_tag("audio.flv.tag", (long)Math.Ceiling(expected_duration / 1000) * 1000-1, 128);
                append_flv_tag("audio.flv.tag", output_flv_file);

                //append_flv_tag("custom.endofvideo.flv.tag", output_flv_file);
                generate_custom_video_flv_tag("padded_timestamp.flv.tag", duration, 0, 480, 360);
                append_flv_tag("padded_timestamp.flv.tag", output_flv_file);
                //File.Delete("padded_timestamp.flv.tag");
            }
            else
            {
                Console.WriteLine("Final padding is not needed");
                //generate_custom_audio_flv_tag("audio.flv.tag", duration+1, 128);
                //append_flv_tag("audio.flv.tag", output_flv_file);
                //generate_custom_video_flv_tag("normal_pad.flv.tag", duration, 0, 1, 2);
                //append_flv_tag("normal_pad.flv.tag", output_flv_file);
                //File.Delete("normal_pad.flv.tag");
            }



            File.Delete("audio.flv.tag");
            File.Delete("thumbnail.flv.tag");
            File.Delete("rejector.flv.tag");

        }
        static void calculate_flv_info(string inputFile)
        {
            Console.WriteLine("\n\n[CALCULATING FLV INFORMATION]\n");
            Console.WriteLine("Filename " + inputFile);
            BinaryReader binaryIn = open_file(inputFile);
            if (binaryIn == null) { return; }
            // Calculate file size and duration
            long filesize = binaryIn.BaseStream.Length;
            Console.WriteLine("Filesize " + filesize);
            if (filesize < 13) // 9 byte header, 4 byte tag size
            {
                Console.WriteLine("File size too small for a valid FLV file.");
                return;
            }
            binaryIn.BaseStream.Position = binaryIn.BaseStream.Length - 4;
            uint tag_size = binaryIn.ReadUInt32();
            //Flipping
            uint tmpInt = tag_size;
            tmpInt = (uint)(((tmpInt & 0x000000ff) << 24) +
              ((tmpInt & 0x0000ff00) << 8) +
              ((tmpInt & 0x00ff0000) >> 8) +
              ((tmpInt & 0xff000000) >> 24));
            binaryIn.BaseStream.Position = binaryIn.BaseStream.Length - 4 - tmpInt;

            byte tag_type = binaryIn.ReadByte();
            byte[] tag_data_size = binaryIn.ReadBytes(3);
            duration = getBitValueFromByteArray(binaryIn.ReadBytes(3), 0, 24);
            Console.WriteLine("Duration " + duration);
            double overall_bitrate = (double)(filesize) * (double)8 / (double)duration;
            // youtube calculates filesize as: filesize - metadatasize(varies) - metatagsize(11) - headersize(9+4)
            // We don't care though, the filesize may be slightly bigger which causes a bit more padding but it's flex room
            Console.WriteLine("Bitrate " + overall_bitrate);


            int max_bitrate_limit = 349;
            expected_duration = (double)(filesize) * (double)8 / (double)max_bitrate_limit;
            if (expected_duration < duration)
            {
                padding_needed = false;
                Console.WriteLine("Padding is not needed");
                //Console.WriteLine("Adding pad timestamp: " + Math.Ceiling(expected_duration/1000) + "(+" + Math.Ceiling((expected_duration - duration)/1000) + " sec)");
            }
            else
            {
                padding_needed = true;
                if (File.Exists("ffmpeg.exe"))
                {
                    Console.WriteLine("Program ffmpeg.exe detected. Using it to create an image frame.");
                    CreateBitmapImage("Th-th-th-that's all folks!\n(Actual Time " + (int)duration / 1000 / 60 + "m, " + Math.Ceiling((double)((double)duration / (double)1000 % 60)).ToString().PadLeft(2, '0') + "s)").Save("EndOfVideo.png", ImageFormat.Png);

                    // Start the child process.
                    Process p = new Process();
                    // Redirect the output stream of the child process.
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.FileName = "ffmpeg.exe";
                    p.StartInfo.Arguments = "-loop_input -f image2 -i EndOfVideo.png -vcodec flv -t 0.01 image.flv";
                    p.Start();
                    // Do not wait for the child process to exit before
                    // reading to the end of its redirected stream.
                    // p.WaitForExit();
                    // Read the output stream first and then wait.
                    string output = p.StandardOutput.ReadToEnd();
                    p.WaitForExit();

                    // Okay pull out the custom video frame from image.flv
                    Console.WriteLine("Pulling out the custom End Of Video frame we created for our video");
                    BinaryReader binaryImageFLV = new BinaryReader(new FileStream("image.flv", FileMode.Open));
                    binaryImageFLV.BaseStream.Position = 13;
                    binaryImageFLV.ReadByte(); // tag_type
                    long metasize = getBitValueFromByteArray(binaryImageFLV.ReadBytes(3), 0, 24); // datasize
                    binaryImageFLV.BaseStream.Position += (metasize + 4 + 3 + 4); // Jump past the timestamp, streamid, and metadata, tagsize


                    BinaryWriter binaryCustomFLVTag = new BinaryWriter(new FileStream("custom.endofvideo.flv.tag", FileMode.Create));
                    binaryCustomFLVTag.Write(binaryImageFLV.ReadByte()); // Write tagtype
                    long datasize = getBitValueFromByteArray(binaryImageFLV.ReadBytes(3), 0, 24); // datasize
                    //Console.WriteLine("DATASIZE: " + datasize);

                    string flv_tag = "";
                    string s_temp;
                    byte b_temp;
                    flv_tag += Convert.ToString(datasize, 2).PadLeft(24, '0');
                    flv_tag += Convert.ToString(duration + 1000, 2).PadLeft(24, '0'); // Add 1 second to end of video's timestamp for custom video
                    int write_how_many_bytes = flv_tag.Length / 8;
                    //Console.WriteLine("binary tag: " + video_flv_tag);
                    for (int i = 0; i < write_how_many_bytes; i++)
                    {
                        s_temp = flv_tag.Substring(i * 8, 8);
                        //Console.WriteLine("s_temp " + s_temp); 
                        b_temp = (byte)Convert.ToByte(s_temp, 2);
                        //Console.WriteLine("Writing byte: " + b_temp);
                        binaryCustomFLVTag.Write(b_temp);
                    }
                    byte filler = 0;
                    binaryCustomFLVTag.Write(filler);
                    binaryCustomFLVTag.Write(filler);
                    binaryCustomFLVTag.Write(filler);
                    binaryCustomFLVTag.Write(filler);

                    binaryImageFLV.BaseStream.Position += 7; // Jump past the timestamp, streamid
                    binaryCustomFLVTag.Write(binaryImageFLV.ReadBytes((int)datasize)); // Write video data


                    binaryCustomFLVTag.Close();
                    binaryImageFLV.Close();
                    File.Delete("image.flv");

                }
                Console.WriteLine("Adding pad timestamp: " + Math.Ceiling(expected_duration / 1000) + "(+" + Math.Ceiling((expected_duration - duration) / 1000) + " sec)");
                // Custom video flv tag: 9-video, 600 sec timestamp, 0 temporal, 480 width, 360 height
                //generate_custom_video_flv_tag("padded_timestamp.flv.tag", (long)Math.Ceiling(expected_duration / 1000)*1000, 0, 1, 2);
            }
            //timestamp_extended = binaryIn.ReadByte();
            //Console.WriteLine("Timestamp extended " + timestamp_extended);
            binaryIn.Close();



            
        }
        
        static void generate_custom_video_flv_tag_with_ffmpeg(long custom_timestamp, int WIDTH, int HEIGHT)
        {

            //objBmpImage = new Bitmap(objBmpImage, new Size(intWidth, intHeight));
            Bitmap objBmpImage = new Bitmap(WIDTH, HEIGHT);

            // Create a graphics object to measure the text’s width and height.
            Graphics objGraphics = Graphics.FromImage(objBmpImage);

            // Add the colors to the new bitmap.
            objGraphics = Graphics.FromImage(objBmpImage);
            // Set Background color
            objGraphics.Clear(Color.White);
            objGraphics.Flush();

            objBmpImage.Save("black-"+WIDTH+"-"+HEIGHT+".png", ImageFormat.Png);






            if (File.Exists("ffmpeg.exe"))
            {
                Console.WriteLine("Program ffmpeg.exe detected. Using it to create an image frame.");
                CreateBitmapImage("Th-th-th-that's all folks!\n(Actual Time " + (int)duration / 1000 / 60 + "m, " + Math.Ceiling((double)((double)duration / (double)1000 % 60)).ToString().PadLeft(2, '0') + "s)").Save("EndOfVideo.png", ImageFormat.Png);

                // Start the child process.
                Process p = new Process();
                // Redirect the output stream of the child process.
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.FileName = "ffmpeg.exe";
                p.StartInfo.Arguments = "-loop_input -f image2 -i " + "black-" + WIDTH + "-" + HEIGHT + ".png" + " -vcodec flv -t 0.01 image.flv";
                // "-i \"movie.wmv\" -an -ss 00:00:03 -s 120×90 -vframes 1 -f mjpeg \"thumbnail.jpg\""
                p.Start();
                // Do not wait for the child process to exit before
                // reading to the end of its redirected stream.
                // p.WaitForExit();
                // Read the output stream first and then wait.
                string output = p.StandardOutput.ReadToEnd();
                p.WaitForExit();

                // Okay pull out the custom video frame from image.flv
                Console.WriteLine("Pulling out the custom End Of Video frame we created for our video");
                BinaryReader binaryImageFLV = new BinaryReader(new FileStream("image.flv", FileMode.Open));
                binaryImageFLV.BaseStream.Position = 13;
                binaryImageFLV.ReadByte(); // tag_type
                long metasize = getBitValueFromByteArray(binaryImageFLV.ReadBytes(3), 0, 24); // datasize
                binaryImageFLV.BaseStream.Position += (metasize + 4 + 3 + 4); // Jump past the timestamp, streamid, and metadata, tagsize


                BinaryWriter binaryCustomFLVTag = new BinaryWriter(new FileStream("custom.ffmpeg.flv.tag", FileMode.Create));
                binaryCustomFLVTag.Write(binaryImageFLV.ReadByte()); // Write tagtype
                long datasize = getBitValueFromByteArray(binaryImageFLV.ReadBytes(3), 0, 24); // datasize
                //Console.WriteLine("DATASIZE: " + datasize);

                string flv_tag = "";
                string s_temp;
                byte b_temp;
                flv_tag += Convert.ToString(datasize, 2).PadLeft(24, '0');
                flv_tag += Convert.ToString(custom_timestamp, 2).PadLeft(24, '0'); // Add 1 second to end of video's timestamp for custom video
                int write_how_many_bytes = flv_tag.Length / 8;
                //Console.WriteLine("binary tag: " + video_flv_tag);
                for (int i = 0; i < write_how_many_bytes; i++)
                {
                    s_temp = flv_tag.Substring(i * 8, 8);
                    //Console.WriteLine("s_temp " + s_temp); 
                    b_temp = (byte)Convert.ToByte(s_temp, 2);
                    //Console.WriteLine("Writing byte: " + b_temp);
                    binaryCustomFLVTag.Write(b_temp);
                }
                byte filler = 0;
                binaryCustomFLVTag.Write(filler);
                binaryCustomFLVTag.Write(filler);
                binaryCustomFLVTag.Write(filler);
                binaryCustomFLVTag.Write(filler);

                binaryImageFLV.BaseStream.Position += 7; // Jump past the timestamp, streamid
                binaryCustomFLVTag.Write(binaryImageFLV.ReadBytes((int)datasize)); // Write video data


                binaryCustomFLVTag.Close();
                binaryImageFLV.Close();
                File.Delete("image.flv");

            }


        
        }

        static long read_FLV_tag(BinaryReader binaryIn, long flv_tag_start_position)
        {
            //TEST
            if (CommandLine["write_flv_tags"] != null)
            {
                write_flv_tags(binaryIn, binaryIn.BaseStream.Position);
            }

            long flv_tag_size_end_position = 0;
            // Open the FLV file.
            //BinaryReader binaryIn;
            try
            {
                 //binaryIn = new BinaryReader(new FileStream("16x12.flv", FileMode.Open));
                 binaryIn.BaseStream.Position = flv_tag_start_position;
            }
            catch (FileNotFoundException exc)
            {
                Console.WriteLine(exc.Message + "\nCan not open file.");
                return 0;
            }


            try
            {

                // THE REST IS REPEATING
                Console.WriteLine("Reading next FLV tag... ["+flg_tag_count+"]");
                byte tag_type = binaryIn.ReadByte();
                Console.Write("Tag type " + tag_type + " ");
                if (tag_type == 18)
                {
                    Console.WriteLine("[METADATA]");
                }
                else if (tag_type == 8)
                {
                    Console.WriteLine("[AUDIO]");
                }
                else if (tag_type == 9)
                {
                    Console.WriteLine("[VIDEO]");
                }
                else
                {
                    Console.WriteLine("[Unknown tag type!]");
                    return 0;
                }
                byte[] tag_data_size = binaryIn.ReadBytes(3);
                long result_tds = getBitValueFromByteArray(tag_data_size, 0, 24);
                //Console.WriteLine("Tag Size of Data " + tag_data_size[0] + "," + tag_data_size[1] + "," + tag_data_size[2]);
                Console.WriteLine("Tag Size of Data " + result_tds);

                byte[] timestamp = binaryIn.ReadBytes(3);
                long result_timestamp = getBitValueFromByteArray(timestamp, 0, 24);
                Console.WriteLine("Timestamp " + result_timestamp);
                byte timestamp_extended = binaryIn.ReadByte();
                Console.WriteLine("Timestamp extended " + timestamp_extended);

                binaryIn.ReadBytes(3); // streamID
                parse_tag_type(binaryIn, tag_type, binaryIn.BaseStream.Position);
                
                binaryIn.BaseStream.Position += result_tds; // Change file position +10 without reading unnessary tag data
                Console.WriteLine("Position in stream: " + binaryIn.BaseStream.Position);

                Console.WriteLine("Reading previous tag size...");
                uint tag_size = binaryIn.ReadUInt32();
                //Flipping
                uint tmpInt = tag_size;
                tmpInt = (uint)(((tmpInt & 0x000000ff) << 24) +
                  ((tmpInt & 0x0000ff00) << 8) +
                  ((tmpInt & 0x00ff0000) >> 8) +
                  ((tmpInt & 0xff000000) >> 24));
                Console.WriteLine("Tag size " + tmpInt);
                Console.WriteLine();


            }
            catch (IOException exc)
            {
                Console.WriteLine(exc.Message + "\nCan not read file.");
                return 0;
            }

            
            flv_tag_size_end_position = binaryIn.BaseStream.Position;
            //binaryIn.Close();
            return flv_tag_size_end_position;


        }
        static void parse_tag_type(BinaryReader binaryIn, byte tag_type, long tag_data_position)
        {
            if (tag_type == 18)
            {
                Console.WriteLine(">>[METADATA EXTENDED DETAILS START]");
                parse_meta_data(binaryIn, tag_data_position);
                Console.WriteLine("<<[METADATA EXTENDED DETAILS END]");
            }
            else if (tag_type == 8)
            {
                Console.WriteLine(">>[AUDIO DETAILS START]");
                parse_audio_data(binaryIn, tag_data_position);
                Console.WriteLine("<<[AUDIO DETAILS END]");
            }
            else if (tag_type == 9)
            {
                Console.WriteLine(">>[VIDEO DETAILS START]");
                parse_video_data(binaryIn, tag_data_position);
                Console.WriteLine("<<[VIDEO DETAILS END]");
            }
            else
            {
                Console.WriteLine("[Unknown tag type!]");
                return;
            }

            // Reset position back to where it was before this function call
            binaryIn.BaseStream.Position = tag_data_position;
        }
        static void parse_meta_data(BinaryReader binaryIn, long tag_data_position)
        {
            binaryIn.BaseStream.Position = tag_data_position;

            
            
            byte data_type = binaryIn.ReadByte();
            if (data_type == 2) // String
            {
                byte[] string_length = binaryIn.ReadBytes(2);
                long result_string_length = getBitValueFromByteArray(string_length, 0, 16);
                char[] charArray = binaryIn.ReadChars((int)result_string_length);
                Console.WriteLine("STRING[" + result_string_length + "] " + convert_char_array_to_string(charArray));
            }
            else if (data_type == 8) // ECMA Array
            {
                byte[] array_size = binaryIn.ReadBytes(4);
                byte keysize = binaryIn.ReadByte();
                char[] string_data = binaryIn.ReadChars(keysize);
                Console.Write("KEYNAME: " + string_data.ToString());
                byte keytype = binaryIn.ReadByte();
                // Ah, lots of types here, double, string, boolean, etc...
                // TODO
            }
            else
            {
                Console.WriteLine("Unknown or unimplemented type for metadata");
            }



            // REPEATING

            data_type = binaryIn.ReadByte();
            if (data_type == 2) // String
            {
                byte[] string_length = binaryIn.ReadBytes(2);
                long result_string_length = getBitValueFromByteArray(string_length, 0, 16);
                char[] charArray = binaryIn.ReadChars((int)result_string_length);
                Console.WriteLine("STRING[" + result_string_length + "] " + convert_char_array_to_string(charArray));
            }
            else if (data_type == 8) // ECMA Array
            {
                byte[] array_size = binaryIn.ReadBytes(4);
                long result_array_size = getBitValueFromByteArray(array_size, 0, 32);
                Console.WriteLine("ECMA ARRAY["+result_array_size+"]");
                //byte keysize = binaryIn.ReadByte();

                byte end_ecma_array_code = 0;
                long keysize;
                char[] charArray;
                byte keytype;
                while (end_ecma_array_code != 9) {
                    result_array_size--;
                    if (result_array_size < 0)
                    {
                        break;
                    }
                    // Reading individual keys
                    keysize = getBitValueFromByteArray(binaryIn.ReadBytes(2), 0, 16);
                    if (keysize == 0)
                    {
                        // Reading one more to verify it's 00,00,09 variable data end marker
                        end_ecma_array_code = binaryIn.ReadByte();
                        Console.WriteLine("Looks like end of array" + end_ecma_array_code);
                        break;
                    }
                    charArray = binaryIn.ReadChars((int)keysize);
                    Console.Write(" KEYNAME: " + convert_char_array_to_string(charArray));
                    keytype = binaryIn.ReadByte();
                    Console.Write(" = ");
                    // Ah, lots of types here, double, string, boolean, etc...
                    // TODO
                    if (keytype == 0)
                    { // DOUBLE, 8 bytes (64 bits)
                        byte[] byteArray = binaryIn.ReadBytes(8);
                        Console.WriteLine("(double) " + DoubleFromByteArray(byteArray));
                    }
                    else if (keytype == 1) {
                        byte boolean_byte = binaryIn.ReadByte();
                        Console.WriteLine("(boolean) " + boolean_byte);
                    }
                    else if (keytype == 2)
                    {
                        byte string_length = (byte)getBitValueFromByteArray(binaryIn.ReadBytes(2), 0, 16);
                        char[] another_charArray = binaryIn.ReadChars(string_length);
                        Console.WriteLine("(string) " + convert_char_array_to_string( another_charArray));

                    }
                    else
                    {
                        Console.WriteLine("Unknown keytype: " + keytype);
                    }
                }
                

            }
            else
            {
                Console.WriteLine("Unknown or unimplemented type for metadata");
            }




        }
        static void parse_audio_data(BinaryReader binaryIn, long tag_data_position)
        {
            binaryIn.BaseStream.Position = tag_data_position;
            byte[] Sound__Format4_Rate2_Size1_Type1 = binaryIn.ReadBytes(1);
            byte Format = (byte)getBitValueFromByteArray(Sound__Format4_Rate2_Size1_Type1, 0, 4);
            byte Rate = (byte)getBitValueFromByteArray(Sound__Format4_Rate2_Size1_Type1, 4, 2);
            byte Size = (byte)getBitValueFromByteArray(Sound__Format4_Rate2_Size1_Type1, 6, 1);
            byte Type = (byte)getBitValueFromByteArray(Sound__Format4_Rate2_Size1_Type1, 7, 1);
            switch (Format)
            {
                case 0:
                    Console.Write("Linear PCM, platform endian");
                    break;
                case 1:
                    Console.Write("ADPCM");
                    break;
                case 2:
                    Console.Write("MP3");
                    break;
                case 3:
                    Console.Write("Linear PCM, little endian");
                    break;
                case 10:
                    Console.Write("AAC");
                    break;
                case 14:
                    Console.Write("MP3 8-KHz");
                    break;
                case 15:
                    Console.Write("Device-specific sound");
                    break;
                default:
                    Console.WriteLine("Unknown Sound Format. Is it MP3/ACC/other?");
                    break;
            }
            Console.Write(" (");
            switch (Rate)
            {
                case 0:
                    Console.Write("5.5-kHz");
                    break;
                case 1:
                    Console.Write("11-kHz");
                    break;
                case 2:
                    Console.Write("22-kHz");
                    break;
                case 3:
                    Console.Write("44-kHz");
                    break;
                default:
                    Console.WriteLine("Unknown Sound Sampling Rate. Is it 5.5kHz/11kHz/22kHz,44kHz?");
                    break;
            }
            Console.Write(", ");
            switch (Size)
            {
                case 0:
                    Console.Write("8 Bit");
                    break;
                case 1:
                    Console.Write("16 Bit");
                    break;
                default:
                    Console.WriteLine("Unknown Sound Size. Is it 8 bit/16 bit?");
                    break;
            }
            Console.Write(", ");
            switch (Type)
            {
                case 0:
                    Console.Write("Mono");
                    break;
                case 1:
                    Console.Write("Stereo");
                    break;
                default:
                    Console.WriteLine("Unknown Sound Type. Is it mono/stereo?");
                    break;
            }

            Console.WriteLine(")");

            // SoundData
            if (Format == 2)
            {
                Console.WriteLine("MPEG Audio Packet Info");
                byte[] MPEGSoundData = binaryIn.ReadBytes(4);
                int FrameSync = (int)getBitValueFromByteArray(MPEGSoundData, 0, 11);
                byte MPEGAudioVersionID = (byte)getBitValueFromByteArray(MPEGSoundData, 11, 2);
                byte LayerDescription = (byte)getBitValueFromByteArray(MPEGSoundData, 13, 2);
                byte ProtectionBit = (byte)getBitValueFromByteArray(MPEGSoundData, 15, 1);
                byte BitrateIndex = (byte)getBitValueFromByteArray(MPEGSoundData, 16, 4);
                byte SamplingRateFrequencyIndex = (byte)getBitValueFromByteArray(MPEGSoundData, 20, 2);
                byte PaddingBit = (byte)getBitValueFromByteArray(MPEGSoundData, 22, 1);
                byte PrivateBit = (byte)getBitValueFromByteArray(MPEGSoundData, 23, 1);
                byte ChannelMode = (byte)getBitValueFromByteArray(MPEGSoundData, 24, 2);
                byte ModeExtension_JointStereo = (byte)getBitValueFromByteArray(MPEGSoundData, 26, 2);
                byte Copyright = (byte)getBitValueFromByteArray(MPEGSoundData, 28, 1);
                byte Original = (byte)getBitValueFromByteArray(MPEGSoundData, 29, 1);
                byte Emphasis = (byte)getBitValueFromByteArray(MPEGSoundData, 30, 2);
                // Just displaying important stuff now
                Console.WriteLine("FrameSync " + FrameSync); // Should always be 2047 (that's how we find start of fragmented streams)
                Console.Write("MPEG Audio Version ");
                switch (MPEGAudioVersionID)
                {
                    case 0:
                        Console.Write("2.5");
                        break;
                    case 1:
                        Console.Write("Reserved");
                        break;
                    case 2:
                        Console.Write("2");
                        break;
                    case 3:
                        Console.Write("1");
                        break;
                    default:
                        Console.WriteLine("-- Unknown Audio Version");
                        break;
                }

                Console.Write(", Layer ");
                switch (LayerDescription)
                {
                    case 0:
                        Console.Write("Reserved");
                        break;
                    case 1:
                        Console.Write("3");
                        break;
                    case 2:
                        Console.Write("2");
                        break;
                    case 3:
                        Console.Write("1");
                        break;
                    default:
                        Console.WriteLine("-- Unknown Layer");
                        break;
                }
                Console.WriteLine();

                Console.Write("Bit Rate ");
                if (MPEGAudioVersionID == 3 && LayerDescription == 1) // Version 1, Layer 3 (Yes, they look reversed. But check specs...)
                {
                    switch(BitrateIndex) {
                        case 0:
                            Console.Write("free");
                            break;
                        case 1:
                            Console.Write("32");
                            break;
                        case 2:
                            Console.Write("40");
                            break;
                        case 3:
                            Console.Write("48");
                            break;
                        case 4:
                            Console.Write("56");
                            break;
                        case 5:
                            Console.Write("64");
                            break;
                        case 6:
                            Console.Write("80");
                            break;
                        case 7:
                            Console.Write("96");
                            break;
                        case 8:
                            Console.Write("112");
                            break;
                        case 9:
                            Console.Write("128");
                            break;
                        case 10:
                            Console.Write("160");
                            break;
                        case 11:
                            Console.Write("192");
                            break;
                        case 12:
                            Console.Write("224");
                            break;
                        case 13:
                            Console.Write("256");
                            break;
                        case 14:
                            Console.Write("320");
                            break;
                        case 15:
                            Console.Write("bad");
                            break;
                        default:
                            Console.Write("??");
                            break;
                    }
                    Console.WriteLine(" kbps");
                }
                else
                {
                    Console.WriteLine(BitrateIndex + " -- Bitrate calculation not implemented yet for this MPEG audio variation");
                }
                Console.WriteLine("Sampling Rate (bit-result) " + SamplingRateFrequencyIndex);
                Console.Write("Channel Mode ");
                switch (ChannelMode)
                {
                    case 0:
                        Console.Write("Stereo");
                        break;
                    case 1:
                        Console.Write("Joint Stereo");
                        break;
                    case 2:
                        Console.Write("Dual Channel (2 Mono Channels)");
                        break;
                    case 3:
                        Console.Write("Single Channel (Mono)");
                        break;
                    default:
                        Console.WriteLine("-- Unknown Channel Mode");
                        break;
                }
                Console.WriteLine();
                // http://www.mp3-tech.org/programmer/frame_header.html
            }

        }
        static void parse_video_data(BinaryReader binaryIn, long tag_data_position)
        {
            binaryIn.BaseStream.Position = tag_data_position;
            byte[] FrameType_And_CodecID = binaryIn.ReadBytes(1);
            byte FrameType = (byte)getBitValueFromByteArray(FrameType_And_CodecID, 0, 4);
            byte CodecID = (byte)getBitValueFromByteArray(FrameType_And_CodecID, 4, 4);
            Console.WriteLine("FrameType: " + FrameType + ", CodecID: " + CodecID);
            if (CodecID == 2)
            {
                Console.WriteLine("Sorenson H.263 (H263VIDEOPACKET)");
                // Just read in the whole header+junk, it's only 9 bytes. We'll parse it.
                byte[] h263videopacket_header = binaryIn.ReadBytes(9);
                // Reading in individual bits now
                byte H263_PictureStartCode = (byte)getBitValueFromByteArray(h263videopacket_header, 0, 17);
                byte H263_Version = (byte)getBitValueFromByteArray(h263videopacket_header, 17, 5);
                byte H263_TemporalReference = (byte)getBitValueFromByteArray(h263videopacket_header, 22, 8);
                byte H263_PictureSize = (byte)getBitValueFromByteArray(h263videopacket_header, 30, 3);
                Console.WriteLine("PictureStartCode " + H263_PictureStartCode);
                Console.WriteLine("Version " + H263_Version);
                Console.WriteLine("TemporalReference " + H263_TemporalReference);
                Console.WriteLine("PictureSize " + H263_PictureSize);
                if (H263_PictureSize == 0)
                {
                    byte H263_CustomWidth = (byte)getBitValueFromByteArray(h263videopacket_header, 33, 8);
                    byte H263_CustomHeight = (byte)getBitValueFromByteArray(h263videopacket_header, 41,8);
                    Console.WriteLine("[8 bit definition] " + H263_CustomWidth + "x" + H263_CustomHeight);
                }
                else if (H263_PictureSize == 1)
                {
                    UInt16 H263_CustomWidth = (UInt16)getBitValueFromByteArray(h263videopacket_header, 33, 16);
                    UInt16 H263_CustomHeight = (UInt16)getBitValueFromByteArray(h263videopacket_header, 49, 16);
                    Console.WriteLine("[16 bit definition] "+H263_CustomWidth+"x"+H263_CustomHeight);
                }
                else
                {
                    Console.WriteLine("Using a predefined picture size. TODO: Reference predefined resolution");
                }
                //byte H263_PictureStartCode = (byte)getBitValueFromByteArray(h263videopacket_header, 0, 17);
                //byte H263_PictureStartCode = (byte)getBitValueFromByteArray(h263videopacket_header, 0, 17);
            }
            else
            {
                Console.WriteLine("Unimplemented codec (UNIMPLEMENTED)");
            }
        }
        static void generate_custom_audio_flv_tag(string output_flv_tag_filename, long timestamp, byte bitrate)
        {
            string audio_flv_tag = "";
            Console.WriteLine("Generating custom audio " + output_flv_tag_filename + " with timestamp "+timestamp);

            // Type: 0x08 (00001000)
            audio_flv_tag += "00001000";
            // Data size: 0x00, 0x00, 0x05 (5)**
            audio_flv_tag += "00000000"; audio_flv_tag += "00000000"; audio_flv_tag += "00110101";
            // *Timestamp: 0x00, 0x00, 0x00
            //string timestamp_string = Convert.ToString(timestamp, 2);
            //timestamp_string = timestamp_string.PadLeft(24, '0');
            //audio_flv_tag += timestamp_string;
            audio_flv_tag += Convert.ToString(timestamp, 2).PadLeft(24, '0');
            // Timestamp Ext: 0x00
            audio_flv_tag += "00000000";
            // StreamID: 0x00, 0x00, 0x00
            audio_flv_tag += "00000000"; audio_flv_tag += "00000000"; audio_flv_tag += "00000000";

            // Audio Data (in binary):
            /*
            Handcrafting the smallest valid FLV frames.
            HEX: ??
                Sound__Format4_Rate2_Size1_Type1 = binaryIn.ReadBytes(1);
                byte[] MPEGSoundData = binaryIn.ReadBytes(4);
                int FrameSync = (int)getBitValueFromByteArray(MPEGSoundData, 0, 11);
                byte MPEGAudioVersionID = (byte)getBitValueFromByteArray(MPEGSoundData, 11, 2);
                byte LayerDescription = (byte)getBitValueFromByteArray(MPEGSoundData, 13, 2);
                byte ProtectionBit = (byte)getBitValueFromByteArray(MPEGSoundData, 15, 1);
                byte BitrateIndex = (byte)getBitValueFromByteArray(MPEGSoundData, 16, 4);
                byte SamplingRateFrequencyIndex = (byte)getBitValueFromByteArray(MPEGSoundData, 20, 2);
                byte PaddingBit = (byte)getBitValueFromByteArray(MPEGSoundData, 22, 1);
                byte PrivateBit = (byte)getBitValueFromByteArray(MPEGSoundData, 23, 1);
                byte ChannelMode = (byte)getBitValueFromByteArray(MPEGSoundData, 24, 2);
                byte ModeExtension_JointStereo = (byte)getBitValueFromByteArray(MPEGSoundData, 26, 2);
                byte Copyright = (byte)getBitValueFromByteArray(MPEGSoundData, 28, 1);
                byte Original = (byte)getBitValueFromByteArray(MPEGSoundData, 29, 1);
                byte Emphasis = (byte)getBitValueFromByteArray(MPEGSoundData, 30, 2);

            */

            // SoundFormat: 0010 - MP3
            audio_flv_tag += "0010";
            // SoundRate: 01 - 11kHz
            audio_flv_tag += "01";
            // SoundSize: 1 - 16 bit
            audio_flv_tag += "1";
            // SoundSize: 0 - Mono
            audio_flv_tag += "0";

            audio_flv_tag += "11111111111"; // FrameSync (all 1's)
            audio_flv_tag += "00"; // Audio Version 2.5
            audio_flv_tag += "01"; // Layer 3
            audio_flv_tag += "1"; //Protection (sure, why not?)
            audio_flv_tag += "0001"; // Bitrate, I'm not going to look up what 1 is now, but it's probably the default low rate
            audio_flv_tag += "00"; // SamplingRate, 00 = 11025hz, 1=12000, 2=8000, 3=reserved
            audio_flv_tag += "0"; // Padded
            audio_flv_tag += "0"; // Private (sure)
            audio_flv_tag += "11"; // 0=Stereo, 1=Joint, 2=Dual Mono, 3=Mono
            audio_flv_tag += "00"; // Mode extension Joint Stereo
            audio_flv_tag += "0"; // Copyright
            audio_flv_tag += "1"; // Original
            audio_flv_tag += "00"; // Emphasis, The emphasis indication is here to tell the decoder that the file must be de-emphasized, ie the decoder must 're-equalize' the sound after a Dolby-like noise supression. It is rarely used. 

            //audio_flv_tag += Convert.ToString(temporalreference, 2).PadLeft(8, '0');
            // *Picture Size: 001 - This could be customized

            
            // Filling out final byte... 0000
            //audio_flv_tag += "";


            BinaryWriter binaryOut = new BinaryWriter(new FileStream(output_flv_tag_filename, FileMode.Create));
            byte b_temp;
            string s_temp;
            int write_how_many_bytes = audio_flv_tag.Length / 8;
            //Console.WriteLine("binary tag: " + audio_flv_tag);
            for (int i = 0; i < write_how_many_bytes; i++)
            {
                s_temp = audio_flv_tag.Substring(i * 8, 8);
                //Console.WriteLine("s_temp " + s_temp); 
                b_temp = (byte)Convert.ToByte(s_temp, 2);
                //Console.WriteLine("Writing byte: " + b_temp);
                binaryOut.Write(b_temp);
            }

            
            //0x00,0x00,0x00,0x03    0x48,0x00,0x00,0x00    00 00 CC 0F    FF FF FC D3
            //0xBF,0xFF,0xFF,0x8A    0x87,0x0D,0x00,0xC2    22 AC 36 EF    FF C5 8C 84
            //15 0C 3F FF      FF 82 1E EC      48 EF 2E 7F   E2 0F 58 3E
            // I have no clue on the data structure of MP3, just the header. This data is copied from a valid MP3, same type
            byte[] mp3data1 = { 0x00,0x00,0x00,0x03, 0x48,0x00,0x00,0x00, 0x00,0x00,0xCC,0x0F, 0xFF,0xFF,0xFC,0xD3};
            byte[] mp3data2 = { 0xBF,0xFF,0xFF,0x8A, 0x87,0x0D,0x00,0xC2, 0x22,0xAC,0x36,0xEF, 0xFF,0xC5,0x8C,0x84};
            byte[] mp3data3 = { 0x15,0x0C,0x3F,0xFF, 0xFF,0x82,0x1E,0xEC, 0x48,0xEF,0xE2,0x7F, 0xE2,0x0F,0x58,0x3E};

            binaryOut.Write(mp3data1);
            binaryOut.Write(mp3data2);
            binaryOut.Write(mp3data3);
            
            binaryOut.Close();

        }
        static void generate_custom_video_flv_tag(string output_flv_tag_filename, long timestamp, byte temporalreference, int width, int height)
        {
            string video_flv_tag = "";
            //if (tag_type == 9)
            //{
            Console.Write("Generating " + output_flv_tag_filename + " with timestamp " + timestamp);
            Console.WriteLine(" and resolution, " + width + "x" + height);

            // Type: 0x09 (00001001)
            video_flv_tag += "00001001";
            // Data size: 0x00, 0x00, 0x0B (11)
            video_flv_tag += "00000000"; video_flv_tag += "00000000"; video_flv_tag += "00001011";
            // *Timestamp: 0x00, 0x00, 0x00
            //string timestamp_string = Convert.ToString(timestamp, 2);
            //timestamp_string = timestamp_string.PadLeft(24, '0');
            //video_flv_tag += timestamp_string;
            video_flv_tag += Convert.ToString(timestamp, 2).PadLeft(24, '0');
            // Timestamp Ext: 0x00
            video_flv_tag += "00000000";
            // StreamID: 0x00, 0x00, 0x00
            video_flv_tag += "00000000"; video_flv_tag += "00000000"; video_flv_tag += "00000000";

            // Video Data (in binary):
            /*
            Handcrafting the smallest valid FLV frames.
            HEX: 12 00 00 84 00 81 e0 01 68 70 20
            0001 0010 - Key frame, Sorenson 263
            0000 0000 0000 0000 1 - Picture Type
            000 01 - Version 1
            00 0000 00 - Temporal Reference 0
            00 1 - 16 bit resolution
            000 0001 1110 0000 0 - 960 width
            000 0001 0110 1000 0 - 720 height
            11 - reserved, 10 = disposable inter frame
            1 - deblock flag
            00000 - quantizer
            0 - no extra
            1 - no macroblock
            0 0000 - picture stuffing ?
            */

            // Frame type: 0001 - Key Frame, forcing YouTube to recognize and process these custom frames
            // Codec ID: 0010 - Sorenson H.263
            video_flv_tag += "00010010";
            // Picture Type: 0000 0000 0000 0000 1
            video_flv_tag += "00000000000000001";
            // Version: 00001
            video_flv_tag += "00001";
            // *TemporalReference: 0000 - This could be customized
            //video_flv_tag += "00000000";
            video_flv_tag += Convert.ToString(temporalreference, 2).PadLeft(8, '0');
            // *Picture Size: 001 - This could be customized
            video_flv_tag += "001";
            // *Width: 0000 0000 0000 0000
            //video_flv_tag += "0000000111100000";//256+128=384+64=448+32=480
            video_flv_tag += Convert.ToString(width, 2).PadLeft(16, '0');
            // *Height: 0000 0000 0000 0000
            //video_flv_tag += "0000000101101000";//256+64=320+32=352+8=360
            video_flv_tag += Convert.ToString(height, 2).PadLeft(16, '0');
            // *Frame: 11 or 10 or 00 or 01 - Reserved/DisposableInter/Intra/Inter
            video_flv_tag += "11";
            // Deblock: 1
            video_flv_tag += "1";
            // Quantizer: 00000 - ???
            video_flv_tag += "00000";
            // ExtraInformation: 0 - No extra information
            video_flv_tag += "0";
            // Macroblock: 1 - No macroblock
            video_flv_tag += "1";
            // PictureStuffing - 0????
            // Filling out final byte... 0000
            video_flv_tag += "00000";


            BinaryWriter binaryOut = new BinaryWriter(new FileStream(output_flv_tag_filename, FileMode.Create));
            byte b_temp;
            string s_temp;
            int write_how_many_bytes = video_flv_tag.Length / 8;
            //Console.WriteLine("binary tag: " + video_flv_tag);
            for (int i = 0; i < write_how_many_bytes; i++)
            {
                s_temp = video_flv_tag.Substring(i * 8, 8);
                //Console.WriteLine("s_temp " + s_temp); 
                b_temp = (byte)Convert.ToByte(s_temp, 2);
                //Console.WriteLine("Writing byte: " + b_temp);
                binaryOut.Write(b_temp);
            }
            binaryOut.Close();

            //}
            //else
            //{
            //    Console.WriteLine("Other tag types generation are unimplemented at this time.");
            //}
        }
        static BinaryWriter create_file(string outputFile)
        {
            BinaryWriter binaryOut;
            try
            {
                binaryOut = new BinaryWriter(new FileStream(outputFile, FileMode.Create));
                return binaryOut;
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message + "\nUnable to create file.");
                return null;
            }

        }
        static byte create_flv_stub (string flv_stub_filename, byte audio_video_code)
        {
            Console.WriteLine("Creating FLV stub file: "+flv_stub_filename);
            BinaryWriter binaryOut = create_file(flv_stub_filename);
            if (binaryOut == null) { return 1; }
            byte[] flv_header = { 0x46, 0x4c, 0x56, 0x01, audio_video_code, 0x00, 0x00, 0x00, 0x09 };
            binaryOut.Write(flv_header);
            byte[] flv_initial_tag_size = { 0x00, 0x00, 0x00, 0x00 };
            binaryOut.Write(flv_initial_tag_size);
            binaryOut.Close();
            return 0;
        }
        static BinaryReader open_file(string inputFile)
        {
            BinaryReader binaryIn;
            try
            {
                binaryIn = new BinaryReader(new FileStream(inputFile, FileMode.Open));
                return binaryIn;
            }
            catch (FileNotFoundException exc)
            {
                Console.WriteLine(exc.Message + "\nCan not open file.");
                return null;
            }

        }
        static BinaryWriter append_file(string outputFile)
        {
            BinaryWriter binaryOut;
            try
            {
                binaryOut = new BinaryWriter(new FileStream(outputFile, FileMode.Append));
                return binaryOut;
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message + "\nCan not append file.");
                return null;
            }

        }
        static byte[] convert_number_to_byte_array(long number, int array_size)
        {

            //Initialize array and set all bytes to 0
            byte[] myByteArray = new byte[array_size];
            for (int i = 0; i < array_size; i++ ) {
                myByteArray[i] = 0x00;
            }

            //Convert number to binary string
            string binaryString = Convert.ToString(number, 2).PadLeft(array_size*8,'0');
//            Console.WriteLine(binaryString);
            
            /* Why did I program this??? Tired...
            //Find out how many bytes are needed
            long bytes_to_write = binaryString.Length / 8;
            if (binaryString.Length % 8>0)
            {
                bytes_to_write++;
            }
            Console.WriteLine("Bytes to write: " + bytes_to_write);
            */
            for (int i = 0; i < array_size; i++) {
                myByteArray[i] = Convert.ToByte(
                    binaryString.Substring(i * 8, 8),2
                    );
            }


            return myByteArray;
        }

        static void append_flv_file(string inputFile, string outputFile, int timestamp_offset)
        {
            BinaryReader binaryIn = open_file(inputFile);
            if (binaryIn == null) { return; }
            // First test in copying only specific portions of the file
            Console.WriteLine("Appending FLV video (" + binaryIn.BaseStream.Length + " bytes)");

            BinaryWriter binaryOut;
            binaryOut = new BinaryWriter(new FileStream(outputFile, FileMode.Append));

            long savePosition = binaryIn.BaseStream.Position;

            binaryIn.BaseStream.Position = 13;
            long tag_data_size;
            long timestamp;
            try
            {
                while (true) // exception will occur if file can't be read anymore
                {
                    byte tag_type = binaryIn.ReadByte();
                    tag_data_size = getBitValueFromByteArray(binaryIn.ReadBytes(3), 0, 24);
                    timestamp = getBitValueFromByteArray(binaryIn.ReadBytes(3), 0, 24);
                    //Console.WriteLine("Timestamp "+timestamp);

                    //Console.WriteLine("Tag Size of Data " + tag_data_size);
                    if (tag_type == 9 || tag_type == 8)
                    //if (tag_type == 9)
                    {
                        // Backup position... 3 bytes for data size, 1 byte for tag type, 3 bytes for timestamp
                        // binaryIn.BaseStream.Position -= 7;
                        binaryOut.Write(tag_type);
                        binaryOut.Write(convert_number_to_byte_array(tag_data_size,3));
                        binaryOut.Write(convert_number_to_byte_array(timestamp+timestamp_offset,3));
                        // Read full flv tag, 1 byte tag type, 3 bytes data size, 3 bytes timestamp, 1 byte timestamp_extended, 3 bytes streamid, 4 bytes final tag size
                        byte[] flv_tag = binaryIn.ReadBytes(4 + (int)tag_data_size + 4); // Tag header, Data, Size
                        binaryOut.Write(flv_tag);
                    }
                    else
                    {
                        //Console.WriteLine("[Unknown tag type!]");
                        binaryIn.BaseStream.Position += (3 + 1 + tag_data_size + 4);
                    }
                }

            }
            catch (IOException exc)
            {
                //Console.WriteLine(exc.Message + "\nCan not read file.");
            }
            binaryOut.Close();
            //binaryIn.BaseStream.Position = savePosition;
            binaryIn.Close();
        }
        /*
        static void append_flv_file(string inputFile, string outputFile, int timestamp_offset)
        {
            BinaryReader binaryIn = open_file(inputFile);
            if (binaryIn == null) { return; }
            // First test in copying only specific portions of the file
            Console.WriteLine("Appending FLV video (" + binaryIn.BaseStream.Length + " bytes)");

            BinaryWriter binaryOut;
            binaryOut = new BinaryWriter(new FileStream(outputFile, FileMode.Append));

            long savePosition = binaryIn.BaseStream.Position;

            binaryIn.BaseStream.Position = 13;
            long tag_data_size;
            try
            {
                while (true) // exception will occur if file can't be read anymore
                {
                    byte tag_type = binaryIn.ReadByte();
                    tag_data_size = getBitValueFromByteArray(binaryIn.ReadBytes(3), 0, 24);
                    //Console.WriteLine("Tag Size of Data " + tag_data_size);
                    if (tag_type == 9 || tag_type == 8)
                    //if (tag_type == 9)
                    {
                        // Backup position... 3 bytes for data size, 1 byte for tag type = 4 bytes.
                        binaryIn.BaseStream.Position -= 4;
                        // Read full flv tag, 1 byte tag type, 3 bytes data size, 3 bytes timestamp, 1 byte timestamp_extended, 3 bytes streamid, 4 bytes final tag size
                        byte[] flv_tag = binaryIn.ReadBytes(11 + (int)tag_data_size + 4); // Tag header, Data, Size
                        binaryOut.Write(flv_tag);
                    }
                    else
                    {
                        //Console.WriteLine("[Unknown tag type!]");
                        binaryIn.BaseStream.Position += (3 + 1 + 3 + tag_data_size + 4);
                    }
                }

            }
            catch (IOException exc)
            {
                //Console.WriteLine(exc.Message + "\nCan not read file.");
            }
            binaryOut.Close();
            //binaryIn.BaseStream.Position = savePosition;
            binaryIn.Close();
        }
        */

        static void write_flv_tags(BinaryReader binaryIn, long flv_tag_start_position)
        {

            
            // Open the FLV file.
            //BinaryReader binaryIn;
            try
            {
                 //binaryIn = new BinaryReader(new FileStream("16x12.flv", FileMode.Open));
                 binaryIn.BaseStream.Position = flv_tag_start_position;
            }
            catch (FileNotFoundException exc)
            {
                Console.WriteLine(exc.Message + "\nCan not open file.");
                return;
            }


            try
            {

                // THE REST IS REPEATING
                Console.WriteLine("Reading next FLV tag...");
                byte tag_type = binaryIn.ReadByte();
                Console.Write("Tag type " + tag_type + " ");
                if (tag_type == 18)
                {
                    Console.WriteLine("[METADATA]");
                }
                else if (tag_type == 8)
                {
                    Console.WriteLine("[AUDIO]");
                }
                else if (tag_type == 9)
                {
                    Console.WriteLine("[VIDEO]");
                }
                else
                {
                    Console.WriteLine("[Unknown tag type!]");
                    return;
                }
                byte[] tag_data_size = binaryIn.ReadBytes(3);
                long result_tds = getBitValueFromByteArray(tag_data_size, 0, 24);
                Console.WriteLine("Tag Size of Data " + result_tds);

                // Okay let's read from start to end of tag data now
                if (tag_type == 9) // Only write video tags out
                {
                    BinaryWriter binaryOut;
                    //binaryOut = new BinaryWriter(new FileStream("v" + flv_tag_start_position + ".flv.tag", FileMode.Create));
                    binaryOut = new BinaryWriter(new FileStream(".\\tags\\" + flg_tag_count + ".flv.tag", FileMode.Create));
                    Console.WriteLine("Writing FLV tag: v" + flv_tag_start_position + ".flv.tag");
                    binaryIn.BaseStream.Position = flv_tag_start_position;
                    byte[] flv_tag = binaryIn.ReadBytes(11 + (int)result_tds);
                    binaryOut.Write(flv_tag);
                    binaryOut.Close();
                }
                else if (tag_type == 8)
                {
                    BinaryWriter binaryOut;
                    //binaryOut = new BinaryWriter(new FileStream("a" + flv_tag_start_position + ".flv.tag", FileMode.Create));
                    binaryOut = new BinaryWriter(new FileStream(".\\tags\\"+flg_tag_count + ".flv.tag", FileMode.Create));
                    Console.WriteLine("Writing FLV tag: a" + flv_tag_start_position + ".flv.tag");
                    binaryIn.BaseStream.Position = flv_tag_start_position;
                    byte[] flv_tag = binaryIn.ReadBytes(11 + (int)result_tds);
                    binaryOut.Write(flv_tag);
                    binaryOut.Close();
                }
                else if (tag_type == 18)
                {
                    BinaryWriter binaryOut;
                    //binaryOut = new BinaryWriter(new FileStream("m" + flv_tag_start_position + ".flv.tag", FileMode.Create));
                    binaryOut = new BinaryWriter(new FileStream(".\\tags\\" + flg_tag_count + ".flv.tag", FileMode.Create));
                    Console.WriteLine("Writing FLV tag: m" + flv_tag_start_position + ".flv.tag");
                    binaryIn.BaseStream.Position = flv_tag_start_position;
                    byte[] flv_tag = binaryIn.ReadBytes(11 + (int)result_tds);
                    binaryOut.Write(flv_tag);
                    binaryOut.Close();
                }

            }
            catch { }
            
            
        }
        static byte append_flv_tag(string flv_tag_file, string output_flv)
        {
            if (File.Exists(flv_tag_file))
            { } else { Console.WriteLine("Can't append flv_tag that doesn't exist: "+flv_tag_file); return 1; }

            BinaryWriter binaryOut = append_file(output_flv);
            if (binaryOut == null) { return 1; }
            //binaryOut = new BinaryWriter(new FileStream(output_flv, FileMode.Append));

            BinaryReader binaryIn = open_file(flv_tag_file);
            if (binaryIn == null) { return 1; }
            //binaryIn = new BinaryReader(new FileStream(flv_tag_file, FileMode.Open));

            long results_tds = binaryIn.BaseStream.Length;
            Console.WriteLine("Appending " + flv_tag_file + " (" + results_tds + " bytes) to " + output_flv);
            binaryOut.Write(binaryIn.ReadBytes((int)results_tds));

            uint tag_size = (uint)results_tds;
            uint tmpInt = tag_size;
            tmpInt = (uint)(((tmpInt & 0x000000ff) << 24) +
              ((tmpInt & 0x0000ff00) << 8) +
              ((tmpInt & 0x00ff0000) >> 8) +
              ((tmpInt & 0xff000000) >> 24));
            binaryOut.Write(tmpInt);




            binaryOut.Close();
            binaryIn.Close();
            return 0;
        }
        static Bitmap CreateBitmapImage(string sImageText)
        {
            // Create text image
            /*
            int WIDTH = 480; int HEIGHT = 360;
            using (Bitmap b = new Bitmap(WIDTH, HEIGHT))
            {

                Font f = new Font("Arial", 40F);
                Graphics g = Graphics.FromImage(b);

                SolidBrush whiteBrush = new SolidBrush(Color.Black);
                SolidBrush blackBrush = new SolidBrush(Color.White);

                RectangleF canvas = new RectangleF(0, 0, WIDTH, HEIGHT);

                g.FillRectangle(whiteBrush, canvas);

                g.DrawString("End Of Video\n3:25", f, blackBrush, canvas);

                //Response.ContentType = "image/gif";
                //b.Save(Response.OutputStream, ImageFormat.Gif);
                b.Save("text.bmp");
            }
            */
            Bitmap objBmpImage = new Bitmap(1, 1);
            int intWidth = 0;
            int intHeight = 0;

            // Create the Font object for the image text drawing.
            Font objFont = new Font("Arial", 36, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);

            // Create a graphics object to measure the text’s width and height.
            Graphics objGraphics = Graphics.FromImage(objBmpImage);

            // This is where the bitmap size is determined.
            intWidth = (int)objGraphics.MeasureString(sImageText, objFont).Width;
            intHeight = (int)objGraphics.MeasureString(sImageText, objFont).Height;

            // Create the bmpImage again with the correct size for the text and font.
            //objBmpImage = new Bitmap(objBmpImage, new Size(intWidth, intHeight));
            objBmpImage = new Bitmap(objBmpImage, new Size(480, 360));

            // Add the colors to the new bitmap.
            objGraphics = Graphics.FromImage(objBmpImage);

            // Set Background color
            objGraphics.Clear(Color.Black);
            //objGraphics.SmoothingMode = SmoothingMode.AntiAlias;
            //objGraphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            objGraphics.DrawString(sImageText, objFont, new SolidBrush(Color.FromArgb(255, 255, 255)), (480 - intWidth) / 2, (360 - intHeight) / 2);
            objGraphics.Flush();

            return (objBmpImage);
        }
        static long getBitValueFromByteArray(byte[] byteArray, int bit_starting_point, int bits_to_read)
        {
            long returnValue;
            if (bits_to_read < 1)
                return 0;
            //            Console.WriteLine("Bit Starting Point: " + bit_starting_point);
            //            Console.WriteLine("Bits to read: " + bits_to_read);
            int how_many_bits_left_to_read = (bit_starting_point + bits_to_read);
            /*//Console.WriteLine("How many bits left to read: " + how_many_bits_left_to_read);
            */

            string binaryString = "";
            for (int i = 0; i < byteArray.GetLength(0); i++)
            {
                string bits = Convert.ToString(byteArray[i], 2);
                bits = bits.PadLeft(8, '0');

                // If there is a starting point
                if (bit_starting_point > 0)
                {
                    // If that starting point is 8 or greater
                    if (bit_starting_point > 7)
                    {
                        // Don't write this next char to binary string
                        //                        Console.WriteLine("Skipping read because startpoint is further along. Bits_to_read: " + bits_to_read);
                    }
                    // Else that starting point is lesser than 8
                    //If bits to read < 8, don't read the whole line
                    else if (bits_to_read < 8)
                    {
                        //                        Console.WriteLine("Start point adjustment/Bits to read<8 : "+ bits_to_read + binaryString);
                        if (bits_to_read > bits.Substring(bit_starting_point % 8).Length)
                        {
                            //                            Console.WriteLine("greater read than available");
                            binaryString = binaryString + bits.Substring(bit_starting_point % 8);
                        }
                        else
                        {
                            binaryString = binaryString + bits.Substring(bit_starting_point % 8, bits_to_read);
                        }
                    }
                    else
                    {
                        binaryString = binaryString + bits.Substring(bit_starting_point % 8);
                        //                        Console.WriteLine("Start point adjustment: "+binaryString);
                    }
                    bit_starting_point = bit_starting_point - 8;
                }
                else if (bits_to_read < 8)
                {
                    binaryString = binaryString + bits.Substring(0, bits_to_read);
                }
                else
                {
                    binaryString = binaryString + bits;
                }

                //                Console.WriteLine("How many bits left to read: " + how_many_bits_left_to_read);
                how_many_bits_left_to_read = how_many_bits_left_to_read - 8;
                string hex = Convert.ToString(byteArray[i], 16);
                hex = hex.PadLeft(2, '0');
                //                Console.Write("Byte[" + (i + 1) + "] DEC: " + byteArray[i]);
                //                Console.Write(" HEX: 0x" + hex);
                //                Console.WriteLine(" BIN: " + bits);
                if (how_many_bits_left_to_read < 1)
                {
                    //                    Console.WriteLine("Done reading bits...");
                    break;
                }
            }
            if (how_many_bits_left_to_read > 0)
            {
                //Console.WriteLine("Problem, byte array too short: " + binaryString );
                Console.WriteLine("Problem, byte array too short: " + binaryString + " -> " + Convert.ToUInt64(binaryString, 2));
                returnValue = 0;
            }
            else
            {
                // HACK to fix broken read code above :(
                binaryString = binaryString.Substring(0, bits_to_read);
                // End HACK
                //                Console.WriteLine("Binary String: " + binaryString + " -> " + Convert.ToUInt64(binaryString, 2));
                returnValue = (long)Convert.ToUInt64(binaryString, 2);
            }

            return returnValue;
        }
        static string convert_char_array_to_string(char[] charArray)
        {
            string return_string = "";
            for (int i = 0; i < charArray.Length; i++)
            {
                return_string = return_string + charArray[i];
            }
            return return_string;
        }
        static double DoubleFromHexString(string hex)
        {
            //string hex = "403E000000000000";
            byte[] b = new byte[hex.Length / 2];
            for (int i = (hex.Length - 2), j = 0; i >= 0; i -= 2, j++)
            {
                b[j] = byte.Parse(hex.Substring(i, 2),
                System.Globalization.NumberStyles.HexNumber);
            }
            double d = BitConverter.ToDouble(b, 0);
            return d;
        }
        static double DoubleFromByteArray(byte[] byteArray)
        {
            byte[] b = new byte[byteArray.Length];
            // Flip and read little-endian.. Thanks to http://bytes.com/forum/thread629092.html
            for (int i = (byteArray.Length - 1), j = 0; i >= 0; i--, j++)
            {
                b[j] = byteArray[i];
            }
            double d = BitConverter.ToDouble(b, 0);
            return d;
        }
    }
}
