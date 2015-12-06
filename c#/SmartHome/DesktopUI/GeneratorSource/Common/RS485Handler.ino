#include <SoftwareSerial.h>
#include "RS485Handler.h"

#define RXPin        2  //Serial Receive pinm - RO on RS485
#define TXPin        3  //Serial Transmit pin, - DI on RS485
#define TxControl     4   //RS485 Direction control, - DE,RE on RS485

#define RS485Transmit    HIGH
#define RS485Receive     LOW

//Serial Communication on Rx Tx pins (not on standards
SoftwareSerial RS485Serial(RXPin, TXPin); // RX, TX

void RS485Handler::SetUp(){
    pinMode(TxControl, OUTPUT);    
    digitalWrite(TxControl, RS485Receive);  // Init Transceiver to Read     
    RS485Serial.begin(4800);   // set the data rate 
    
  };
  
int RS485Handler::Read() {
    digitalWrite(TxControl, RS485Receive);  
    
    if (RS485Serial.available())  //Look for data from master
    {
      return RS485Serial.read();    // Read received byte
    }   
    return -1;  
};

void RS485Handler::Write(byte data[], int dataSize) {
   digitalWrite(TxControl, RS485Transmit);  
    
    for (int i = 0; i< dataSize; i++ ){
      RS485Serial.write(data[i]);  
    }
    digitalWrite(TxControl, RS485Receive);  // Init Transceiver to Read     
};
