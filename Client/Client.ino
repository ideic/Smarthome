#define PIN_SWITCH 2

void setup() {
  Serial.begin(9600, SERIAL_8E1);
  while (!Serial) {
    ; // wait for serial port to connect. Needed for Leonardo only
  }
  pinMode(PIN_SWITCH, INPUT); 
  digitalWrite(PIN_SWITCH, HIGH);  
}

void loop() {
  if (digitalRead(PIN_SWITCH) == LOW) {
    Serial.println("SwitchedOn");
  } else{
    Serial.println("SwitchedOff");
  }
  delay(2000);
}

