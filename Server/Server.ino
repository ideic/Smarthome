#include <Arduino.h>
#include "Addresses.h"
#include "SmartHomeCommunication.h"

#define SegmentN 0
#define SegmentE 1
#define SegmentK 2


struct SegmentState {  
   SegmentState ():_id(false), _isSegmentOn(false){};
    
   SegmentState (byte id, bool isSegmentOn){
       _id = id;
       _isSegmentOn = isSegmentOn;
    }
    byte _id;
    bool _isSegmentOn;    
  };
  
struct Device {
    Device (): _id(0), _address(0), _state(0){}; 
    Device(byte id, byte address, byte state) {
      _id = id;
      _address = address;
      _state = state;
    }

    byte _id;
    byte _address; 
    byte _state;
  };



  
SegmentState _deviceSegments[3];
Device _switches[8];
Device _relays[10];
SHCommunication _com = SHCommunication();

void setup() {
  Serial.begin(9600, SERIAL_8E1);
  while (!Serial) {
    ; // wait for serial port to connect. Needed for Leonardo only
  }
  
  _deviceSegments [0] = SegmentState(SegmentN, false);
  _deviceSegments [1] = SegmentState(SegmentE, false);
  _deviceSegments [2] = SegmentState(SegmentK, false);
  
  _switches[0] = Device(SegmentN, NK1, MSG_DEVICE_OFF);
  _switches[1] = Device(SegmentN, NK2, MSG_DEVICE_OFF);
  _switches[2] = Device(SegmentN, NK3, MSG_DEVICE_OFF);

  _switches[3] = Device(SegmentE, EK1, MSG_DEVICE_OFF);
  _switches[4] = Device(SegmentE, EK2, MSG_DEVICE_OFF);

  _switches[5] = Device(SegmentK, EK3, MSG_DEVICE_OFF);
  _switches[6] = Device(SegmentK, KU1, MSG_DEVICE_OFF);
  _switches[7] = Device(SegmentK, KK2, MSG_DEVICE_OFF);
  
  _relays[0] = Device(SegmentN, NL1, MSG_DEVICE_OFF);
  _relays[1] = Device(SegmentN, NL2, MSG_DEVICE_OFF);

  _relays[2] = Device(SegmentE, EL1, MSG_DEVICE_OFF);
  
  _relays[3] = Device(SegmentK, KUL1, MSG_DEVICE_OFF);
  _relays[4] = Device(SegmentK, KUL2, MSG_DEVICE_OFF);
  _relays[5] = Device(SegmentK, KUL3, MSG_DEVICE_OFF);
  _relays[6] = Device(SegmentK, KUL4, MSG_DEVICE_OFF);
  _relays[7] = Device(SegmentK, KUL5, MSG_DEVICE_OFF);
  _relays[8] = Device(SegmentK, KUL6, MSG_DEVICE_OFF);
  _relays[9] = Device(SegmentK, KUL7, MSG_DEVICE_OFF);
  
}

void loop() {
  
  bool segmentStateChanged = false;
  
  int segmentId = -1;
  for (int switchIndex = 0; switchIndex<8; switchIndex++) {
    
    //segment changed
    if (segmentId != _switches[switchIndex]._id) {
        if (segmentStateChanged) {
          _deviceSegments [segmentId]._isSegmentOn =  !_deviceSegments [segmentId]._isSegmentOn;
          Serial.println(F("Change Releay state"));
          ChangeSegmentRelaysState(segmentId);  
          segmentId = _switches[switchIndex]._id;
        }
        
        segmentStateChanged = false;
    }

    byte deviceState = GetDeviceState (_switches[switchIndex]._address);
    if (deviceState != 0 && deviceState != _switches[switchIndex]._state) {
      Serial.println(F("deviceState changed"));
      _switches[switchIndex]._state = deviceState;
      segmentStateChanged = !segmentStateChanged;
    }
  }
}

byte GetDeviceState(byte address) {
  _com.SendMessage(ServerAddress, address,  CMD_GET);
  
  unsigned long currentTime = millis();
  //overflow
  if ((currentTime + 100) < currentTime) {
    delay(100);  
    currentTime = millis();
  }
  
  bool timeOut = false;
  while (!_com.WeGotMessage(ServerAddress)) {
    if ((currentTime + 100) < millis()) {
         timeOut = true;
        break;
    } 
  }

  if (!timeOut && (_com.From() == address)) {
      return _com.Message();
  }

  return 0;  
  
}

void ChangeSegmentRelaysState(int segmentId) {
  int relayIndex = FindRelay(segmentId);
  
  for (;_relays[relayIndex]._id == segmentId; relayIndex++) {
    byte newState = _deviceSegments [segmentId]._isSegmentOn ?  CMD_SET_ON : CMD_SET_OFF;
    _com.SendMessage(ServerAddress, _relays[relayIndex]._address, newState);
    if (GetDeviceState(_relays[relayIndex]._address) != newState == CMD_SET_ON ?  MSG_DEVICE_ON : MSG_DEVICE_OFF) {
      _com.SendMessage(ServerAddress, _relays[relayIndex]._address, newState);
    }
  }
}

int FindRelay (int segmentId) {
  int index = 0;
  while (_relays[index]._id != segmentId) {
    index++;  
  }
  
  return index;
}
