#ifndef SmartHomeCommunication_H
#define SmartHomeCommunication_H

#include "RS485Handler.h"

#define MSG_LIGHT_ON B00110011
#define MSG_LIGHT_OFF B00111100


class SHCommunication {
  public:
     SHCommunication();
     bool WeGotMessage (int myAddress);
     void  SendMessage(byte from, byte to,  byte msg);
     void SetUp();
     
  private:
    bool DataValidationFailed(byte dataChunks[]);
    byte CalculateCRC(byte chunks[]);
  
    RS485Handler _rs485Reader;
};

#endif
