
#include <Arduino.h>
#include "SmartHomeCommunication.h"
#include "DeviceHandler.h"
#include "Addresses.h"

#define DevicesNumber 5

SHCommunication _com = SHCommunication();
DeviceBase* _devices[DevicesNumber];


void setup() {
  Serial.begin(9600, SERIAL_8E1);
  while (!Serial) {
    ; // wait for serial port to connect. Needed for Leonardo only
  }
  _devices[0] = DeviceFactory::GetDevice(LightSwitchDeviceType, NK1, 2);
  _devices[1] = DeviceFactory::GetDevice(LightSwitchDeviceType, NK2, 3);
  _devices[2] = DeviceFactory::GetDevice(LightSwitchDeviceType, NK3, 4);
  
  _devices[3] = DeviceFactory::GetDevice(RelayDeviceType, NL1, 5);
  _devices[4] = DeviceFactory::GetDevice(RelayDeviceType, NL2, 6);

  for (int i = 0; i<DevicesNumber; i++) {
      _devices[i]->SetUp();  
  }
  _com.SetUp();
}

void loop() {

  for (int i = 0; i<DevicesNumber; i++) {
    if (_com.WeGotMessage(_devices[i]->GetAddress())) 
    {
       Serial.println(F("We got Message"));
       if (_com.WeGotGETMessage()) {
         Serial.println(F("We got GET Message"));
         // in case of get
         bool isDeviceOn = _devices[i]->IsDeviceOn();
         _com.SendMessage(_devices[i]->GetAddress(), _com.From(),   isDeviceOn ? MSG_DEVICE_ON : MSG_DEVICE_OFF);
       } 
       else if (_com.WeGotSETMessage()) {
         Serial.println(F("We got SET Message"));
         _devices[i]->SetDeviceState(_com.SetMessageIsOn());
       }
    }
  }
  delay(20);
}


