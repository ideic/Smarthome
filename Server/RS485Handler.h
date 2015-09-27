#ifndef RS485HANDLER_H
#define RS485HANDLER_H



class RS485Handler {
  public:
     void SetUp();
     int Read();
     void Write(byte data[], int dataSize);
};

#endif
