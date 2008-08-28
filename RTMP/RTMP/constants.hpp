//package org.red5.server.net.rtmp.message;

/**
 * Class for AMF and RTMP marker values constants
 */
struct constants 
{

    /**
     * Medium integer max value
     */
    static const int MEDIUM_INT_MAX = 16777215;

    /**
     * RTMP chunk size constant
     */
    static const byte TYPE_CHUNK_SIZE = 0x01;

	// Unknown: 0x02

    /**
     * Send every x bytes read by both sides
     */
    static const byte TYPE_BYTES_READ = 0x03;

    /**
     * Ping is a stream control message, has subtypes
     */
    static const byte TYPE_PING = 0x04;

    /**
     * Server (downstream) bandwidth marker
     */
    static const byte TYPE_SERVER_BANDWIDTH = 0x05;

    /**
     * Client (upstream) bandwidth marker
     */
    static const byte TYPE_CLIENT_BANDWIDTH = 0x06;

	// Unknown: 0x07

    /**
     * Audio data marker
     */
    static const byte TYPE_AUDIO_DATA = 0x08;

    /**
     * Video data marker
     */
    static const byte TYPE_VIDEO_DATA = 0x09;

	// Unknown: 0x0A ...  0x0E

    /**
     * AMF3 stream send
     */
    static const byte TYPE_FLEX_STREAM_SEND = 0x0F;
    
    /**
     * AMF3 shared object
     */
    static const byte TYPE_FLEX_SHARED_OBJECT = 0x10;
    
    /**
     * AMF3 message
     */
    static const byte TYPE_FLEX_MESSAGE = 0x11;

    /**
     * Notification is invocation without response
     */
    static const byte TYPE_NOTIFY = 0x12;

    /**
     * Stream metadata
     */
    static const byte TYPE_STREAM_METADATA = 0x12;

    /**
     * Shared Object marker
     */
    static const byte TYPE_SHARED_OBJECT = 0x13;

    /**
     * Invoke operation (remoting call but also used for streaming) marker
     */
    static const byte TYPE_INVOKE = 0x14;

    /**
     * New header marker
     */
    static const byte HEADER_NEW = 0x00;

    /**
     * Same source marker
     */
    static const byte HEADER_SAME_SOURCE = 0x01;

    /**
     * Timer change marker
     */
    static const byte HEADER_TIMER_CHANGE = 0x02;

    /**
     * There's more to encode
     */
    static const byte HEADER_CONTINUE = 0x03;

    /**
     * Size of initial handshake between client and server
     */
    static const int HANDSHAKE_SIZE = 1536;

    /**
     * Client Shared Object data update
     */
    static const byte SO_CLIENT_UPDATE_DATA = 0x04; //update data

    /**
     * Client Shared Object attribute update
     */
    static const byte SO_CLIENT_UPDATE_ATTRIBUTE = 0x05; //5: update attribute

    /**
     * Send SO message flag
     */
    static const byte SO_CLIENT_SEND_MESSAGE = 0x06; // 6: send message

    /**
     * Shared Object status marker
     */
    static const byte SO_CLIENT_STATUS = 0x07; // 7: status (usually returned with error messages)

    /**
     * Clear event for Shared Object
     */
    static const byte SO_CLIENT_CLEAR_DATA = 0x08; // 8: clear data

    /**
     * Delete data for Shared Object
     */
    static const byte SO_CLIENT_DELETE_DATA = 0x09; // 9: delete data

    /**
     * Initial SO data flag
     */
    static const byte SO_CLIENT_INITIAL_DATA = 0x0B; // 11: initial data

    /**
     * Shared Object connection
     */
    static const byte SO_CONNECT = 0x01;

    /**
     * Shared Object disconnection
     */
    static const byte SO_DISCONNECT = 0x02;

    /**
     * Set Shared Object attribute flag
     */
    static const byte SO_SET_ATTRIBUTE = 0x03;

    /**
     * Send message flag
     */
    static const byte SO_SEND_MESSAGE = 0x06;

    /**
     * Shared Object attribute deletion flag
     */
    static const byte SO_DELETE_ATTRIBUTE = 0x0A;

	static const String ACTION_CONNECT = "connect";

	static const String ACTION_DISCONNECT = "disconnect";

	static const String ACTION_CREATE_STREAM = "createStream";

	static const String ACTION_DELETE_STREAM = "deleteStream";

	static const String ACTION_CLOSE_STREAM = "closeStream";

	static const String ACTION_RELEASE_STREAM = "releaseStream";

	static const String ACTION_PUBLISH = "publish";

	static const String ACTION_PAUSE = "pause";

	static const String ACTION_SEEK = "seek";

	static const String ACTION_PLAY = "play";

	static const String ACTION_STOP = "disconnect";

	static const String ACTION_RECEIVE_VIDEO = "receiveVideo";

	static const String ACTION_RECEIVE_AUDIO = "receiveAudio";

}
