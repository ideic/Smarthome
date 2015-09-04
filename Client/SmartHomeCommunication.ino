#include "SmartHomeCommunication.h"

#define CMD_GET B11000011
#define START B11110000
#define STOP B00001111

#define MESSAGE_SIZE 6
#define MESSAGE_POS_START 0
#define MESSAGE_POS_FROM 1
#define MESSAGE_POS_TO 2
#define MESSAGE_POS_MESSAGE 3
#define MESSAGE_POS_STOP 4
#define MESSAGE_POS_LRC 6

SHCommunication::SHCommunication() {
  _rs485Reader = RS485Handler();
};

void SHCommunication::SetUp() {
   _rs485Reader.SetUp();
};

bool SHCommunication::WeGotMessage (int myAddress) {
  byte dataChunks[MESSAGE_SIZE];
  
  for (int i = 0 ; i< MESSAGE_SIZE; i++){
    int data = _rs485Reader.Read();
    if (data == -1) return false;
    
    dataChunks[i] = data;
  }
  
  if (DataValidationFailed(dataChunks)) return false;
  
  return  (dataChunks[MESSAGE_POS_FROM] == myAddress && dataChunks[MESSAGE_POS_MESSAGE] == CMD_GET); 
};


bool SHCommunication::DataValidationFailed(byte dataChunks[]) {

   if (dataChunks[MESSAGE_POS_START] != START) return true;
   if (dataChunks[MESSAGE_POS_STOP] != STOP) return true;
   
   return CalculateCRC(dataChunks) != dataChunks[MESSAGE_POS_LRC] ;
};


byte SHCommunication::CalculateCRC(byte chunks[]){
  
  byte result = chunks[0];
  for (int i = 1; i< MESSAGE_SIZE -1 ; i++) {
    result = result ^ chunks[i];
  }
  
  return result; 
};

void  SHCommunication::SendMessage(byte from, byte to,  byte msg) {
   byte message [MESSAGE_SIZE]; 
   message[MESSAGE_POS_START] = START;
   message[MESSAGE_POS_FROM] = from;
   message[MESSAGE_POS_TO] = to;
   message[MESSAGE_POS_MESSAGE] = msg;
   message[MESSAGE_POS_STOP] = from;
   message[MESSAGE_POS_LRC] = CalculateCRC(message);

   _rs485Reader.Write(message, MESSAGE_SIZE);
 
};


