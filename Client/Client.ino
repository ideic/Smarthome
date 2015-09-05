
#include <Arduino.h>
#include "SmartHomeCommunication.h"
#include "DeviceHandler.h"
#include "Addresses.h"

SHCommunication _com = SHCommunication();
DeviceBase* _device;

void setup() {
  Serial.begin(9600, SERIAL_8E1);
  while (!Serial) {
    ; // wait for serial port to connect. Needed for Leonardo only
  }
  _device = DeviceFactory::GetDevice(LightSwitchDeviceType);
  _com.SetUp();
  _device->SetUp();  
}

void loop() {
  
  if (_com.WeGotMessage(NK1)) 
  {
     if (_com.WeGotGETMessage()) {
       // in case of get
       bool isDeviceOn = _device->IsDeviceOn();
       _com.SendMessage(NK1, _com.From(),   isDeviceOn ? MSG_DEVICE_ON : MSG_DEVICE_OFF);
     } 
     else if (_com.WeGotSETMessage()) {
       _device->SetDeviceState(_com.SetMessageIsOn());
         
     }
  }
  delay(20);
}


