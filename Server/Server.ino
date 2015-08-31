#define SegmentA 0
#define SegmentB 1
#define SegmentC 2


struct LightState {  
    LightState (){};
    
   LightState (byte id, bool isLightOn){
       _id = id;
       _isLightOn = isLightOn;
    }
    byte _id;
    bool _isLightOn;    
  };
  
struct Part {
  
    byte id;
    byte part; // 2 bit for its state, 6 bit for the identify it
  };

  
LightState _lightSegments[3];

void setup() {
  _lightSegments [0] = LightState(SegmentA, false);
  _lightSegments [1] = LightState(SegmentB, false);
  _lightSegments [2] = LightState(SegmentC, false);

}

void loop() {
  // put your main code here, to run repeatedly:

}
