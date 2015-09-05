#include "DeviceHandler.h"
#define PIN_SWITCH 2

void LightSwitchDevice::SetUp(){
  pinMode(PIN_SWITCH, INPUT); 
  digitalWrite(PIN_SWITCH, HIGH);
};

bool LightSwitchDevice::IsDeviceOn(){
  bool isLightOn = false;
  if (digitalRead(PIN_SWITCH) == LOW) {
    // Wait 5 miliseconds, because of perk effect
    delay(5);
    if (digitalRead(PIN_SWITCH) == LOW) {
      isLightOn = true;
    }
  }
  return isLightOn; 
}

RelayDevice::RelayDevice(): _isRelayOn(false) {
};

void RelayDevice::SetUp(){
};

bool RelayDevice::IsDeviceOn(){
  return _isRelayOn;
}
