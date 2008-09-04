Unfortunately it's uncommented... but it is written in ruby, a fairly
clean and concise language.

To test it, Just go to the rtmp_implementation folder and type "ruby
launcher.rb" (you'll need ruby 1.8)

You will find a quick test application located at
"./appli_root/test/main.rb". Then, make an empty flash movie and copy
this code into the first frame:


trace($version);
_root.main_nc = new NetConnection();
_root.main_nc.onStatus = function(info){
       trace("onStatus = "+info.code);
}
_root.main_nc.connect("rtmp://localhost:1935/test");
function EntryResult(targetObject){
       this.targetObject = targetObject;
       this.onResult = function(result){
           trace("last entry = " + result);
           this.targetObject.processEntry(result + 1);
       }
}
_root.main_nc.call("appendText", new EntryResult(this),"Hello
","world"); // Should return "Hello World"

This implementation is surely buggy as hell. So don't hesitate to
contact me for further information.

Yannick
yannick@dazzlebox.com