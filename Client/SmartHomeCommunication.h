#ifndef SmartHomeCommunication_H
#define SmartHomeCommunication_H

#include "RS485Handler.h"

#define MSG_DEVICE_ON B00110011
#define MSG_DEVICE_OFF B00111100


class SHCommunication {
  public:
     SHCommunication();
     bool WeGotMessage (int myAddress);
     bool WeGotGETMessage();
     bool WeGotSETMessage();
     bool SetMessageIsOn();
     void  SendMessage(byte from, byte to,  byte msg);
     void SetUp();
     
  private:
    bool DataValidationFailed(byte dataChunks[]);
    byte CalculateCRC(byte chunks[]);
  
    RS485Handler _rs485Reader;
    byte _lastMessage;
};

#endif
