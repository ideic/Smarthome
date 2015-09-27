
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
  _devices[0] = DeviceFactory::GetDevice(LightSwitchDeviceType, NK1, 4);
  _devices[1] = DeviceFactory::GetDevice(LightSwitchDeviceType, NK2, 2);
  _devices[2] = DeviceFactory::GetDevice(LightSwitchDeviceType, NK3, 3);
  
  _devices[3] = DeviceFactory::GetDevice(RelayDeviceType, NL1, 5);
  _devices[4] = DeviceFactory::GetDevice(RelayDeviceType, NL2, 6);

  for (int i = 0; i<DevicesNumber; i++) {
      _devices[i]->SetUp();  
  }
  _com.SetUp();
}

void loop() {
  DeviceBase* device = NULL;
 
  if (_com.WeGotMessage()) {
    device = FindDevice(_com.Address());
    if (device != NULL) {
       if (_com.WeGotGETMessage()) {
         // in case of get
         bool isDeviceOn = device->IsDeviceOn();
         
         _com.SendMessage(device->GetAddress(), _com.From(),   isDeviceOn ? MSG_DEVICE_ON : MSG_DEVICE_OFF);
       } 
       else if (_com.WeGotSETMessage()) {
         device->SetDeviceState(_com.IsMessageSetOn());
       }
    }
  } 

  delay(20);
}

DeviceBase* FindDevice(byte address) {
  for(int i = 0; i< DevicesNumber; i++) {
    if (_devices[i]->GetAddress() == address) {
      return _devices[i];
    }  
  }
  
  return NULL;  
}
