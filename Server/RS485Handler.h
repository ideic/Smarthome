#ifndef RS485HANDLER_H
#define RS485HANDLER_H

#define RXPin        10  //Serial Receive pin
#define TXPin        11  //Serial Transmit pin
#define TxControl     3   //RS485 Direction control

#define RS485Transmit    HIGH
#define RS485Receive     LOW

class RS485Handler {
  public:
     void SetUp();
     int Read();
     void Write(byte data[], int dataSize);
};

#endif
