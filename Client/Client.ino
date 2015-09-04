#define PIN_SWITCH 2
#define MyAddress B11110001
#define ServerAddress B11110000

#include <Arduino.h>
#include "SmartHomeCommunication.h"

SHCommunication _com = SHCommunication();

void setup() {
  Serial.begin(9600, SERIAL_8E1);
  while (!Serial) {
    ; // wait for serial port to connect. Needed for Leonardo only
  }
  pinMode(PIN_SWITCH, INPUT); 
  digitalWrite(PIN_SWITCH, HIGH);

  _com.SetUp();  
}

void loop() {
  
  if (_com.WeGotMessage(MyAddress)) 
  {
     bool isLightOn = false;
     isLightOn = GetLightState();
     _com.SendMessage(MyAddress, ServerAddress,   isLightOn ? MSG_LIGHT_ON : MSG_LIGHT_OFF);
  }
  delay(20);
}

bool GetLightState() {
  bool isLightOn = false;
  if (digitalRead(PIN_SWITCH) == LOW) {
    // Wait 5 miliseconds, because of perk effect
    delay(5);
    if (digitalRead(PIN_SWITCH) == LOW) {
      isLightOn = true;
    }
  }
  return isLightOn; 
}

