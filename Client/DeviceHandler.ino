#include "DeviceHandler.h"

DeviceBase::DeviceBase(int address, int pinNumber) : _address(-1), _pinNumber(-1) {};

LightSwitchDevice::LightSwitchDevice(int address, int pinNumber) :  DeviceBase (address, pinNumber) {
};

void LightSwitchDevice::SetUp(){
  pinMode(GetPinNumber(), INPUT); 
  digitalWrite(GetPinNumber(), HIGH);
};

bool LightSwitchDevice::IsDeviceOn(){
  bool isLightOn = false;
  if (digitalRead(GetPinNumber()) == LOW) {
    // Wait 5 miliseconds, because of perk effect
    delay(5);
    if (digitalRead(GetPinNumber()) == LOW) {
      isLightOn = true;
    }
  }
  return isLightOn; 
}

RelayDevice::RelayDevice(int address, int pinNumber) :  DeviceBase (address, pinNumber), _isRelayOn(false){
};

void RelayDevice::SetUp(){
  pinMode(GetPinNumber(), OUTPUT); 
  digitalWrite(GetPinNumber(), LOW);
};

bool RelayDevice::IsDeviceOn(){
  return _isRelayOn;
};

void RelayDevice::SetDeviceState (bool isOn) {
  digitalWrite(GetPinNumber(), isOn ? HIGH : LOW);
  _isRelayOn = isOn;
};

EmptyDevice::EmptyDevice () : DeviceBase(-1, -1) {};
