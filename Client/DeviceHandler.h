#ifndef DEVICEHANDLER_H
#define DEVICEHANDLER_H

#define LightSwitchDeviceType 0
#define RelayDeviceType 1


class DeviceBase {
  public:
     DeviceBase(int address, int pinNumber);
     virtual void SetUp(){};
     virtual bool IsDeviceOn() {return false;}
     virtual void SetDeviceState (bool isOn) {};
     virtual int GetAddress() {
       return _address;
     }
     
     virtual int GetPinNumber() {
       return _pinNumber;
     }
     
   private:
     int _address;
     int _pinNumber;
};

class LightSwitchDevice: public DeviceBase {
    public:
     LightSwitchDevice(int address,  int pinNumber);
     virtual void SetUp();
     virtual bool IsDeviceOn();
 };

class RelayDevice: public DeviceBase {
    public:
     RelayDevice(int address, int pinNumber);
     virtual void SetUp();
     virtual bool IsDeviceOn();
     virtual void SetDeviceState (bool isOn);
    private:
      bool _isRelayOn;
 };

class EmptyDevice: public DeviceBase {
    public:
     EmptyDevice();
     virtual void SetUp() {
       return;
     }
 };

class DeviceFactory {
  public:
    static DeviceBase* GetDevice (int deviceType, int address, int pinNumber) {
      switch (deviceType) {
        case   LightSwitchDeviceType : return new LightSwitchDevice(address, pinNumber); break;
        case   RelayDeviceType : return new RelayDevice(address, pinNumber); break;
        default : return new EmptyDevice(); break;
      }
    }    
};

#endif
