#ifndef SmartHomeCommunication_H
#define SmartHomeCommunication_H

#include "RS485Handler.h"

#define MSG_DEVICE_ON B00110011
#define MSG_DEVICE_OFF B00111100

#define CMD_GET B11000011
#define CMD_SET_ON B11001100
#define CMD_SET_OFF B11001111


class SHCommunication {
  public:
     SHCommunication();
     bool WeGotMessage ();
     bool WeGotGETMessage();
     bool WeGotSETMessage();
     byte From();
     byte Message();
     byte Address();
     bool IsMessageSetOn();
     void SendMessage(byte from, byte to,  byte msg);
     void SetUp();
     
  private:
    bool DataValidationFailed(byte dataChunks[]);
    byte CalculateCRC(byte chunks[]);
  
    RS485Handler _rs485Reader;
    byte _lastMessage;
    byte _lastFrom;
    byte _lastAddress;
};

#endif
