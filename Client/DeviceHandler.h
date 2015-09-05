#ifndef DEVICEHANDLER_H
#define DEVICEHANDLER_H

#define LightSwitchDeviceType 0
#define RelayDeviceType 1


class DeviceBase {
  public:
     virtual void SetUp(){};
     virtual bool IsDeviceOn() {return false;}
     virtual void SetDeviceState (bool isOn) {};
};

class LightSwitchDevice: public DeviceBase {
    public:
     virtual void SetUp();
     virtual bool IsDeviceOn();
 };

class RelayDevice: public DeviceBase {
    public:
     RelayDevice();
     virtual void SetUp();
     virtual bool IsDeviceOn();
     virtual void SetDeviceState (bool isOn);
    private:
      bool _isRelayOn;
 };

class EmptyDevice: public DeviceBase {
    public:
     virtual void SetUp() {
       return;
     }
 };

class DeviceFactory {
  public:
    static DeviceBase* GetDevice (int deviceType) {
      switch (deviceType) {
        case   LightSwitchDeviceType : return new LightSwitchDevice(); break;
        case   RelayDeviceType : return new RelayDevice(); break;
        default : return new EmptyDevice(); break;
      }
    }    
};

#endif
