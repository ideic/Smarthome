#include "DeviceHandler.h"
#define PIN_SWITCH 2
#define PIN_RELAY 4

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
  pinMode(PIN_RELAY, OUTPUT); 
  digitalWrite(PIN_RELAY, LOW);
};

bool RelayDevice::IsDeviceOn(){
  return _isRelayOn;
};

void RelayDevice::SetDeviceState (bool isOn) {
  digitalWrite(PIN_RELAY, isOn ? HIGH : LOW);
  _isRelayOn = isOn;
};

