#include "DeviceHandler.h"

DeviceBase::DeviceBase(int address, int pinNumber) : _address(address), _pinNumber(pinNumber) {};

LightSwitchDevice::LightSwitchDevice(int address, int pinNumber) :  DeviceBase (address, pinNumber) {
};

void LightSwitchDevice::SetUp(){
  pinMode(GetPinNumber(), INPUT); 
  digitalWrite(GetPinNumber(), HIGH);
};

bool LightSwitchDevice::IsDeviceOn(){
  bool prevValue = digitalRead(GetPinNumber());
  delay(5);
  bool currentValue = digitalRead(GetPinNumber());

  // Wait 5 miliseconds, because of perk effect
  while (prevValue != currentValue) {
    delay(5);
    prevValue = currentValue;
    currentValue = digitalRead(GetPinNumber());
  }
  
  return currentValue;
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
