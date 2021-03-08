#include <SoftwareSerial.h>
#include <SerialCommand.h>      // Steven Cogswell ArduinoSerialCommand library from http://GitHub.com
#include <Keyboard.h>
#define ONE_WIRE_BUS 2
#define LED_LOW      3
#define LED_HIGH     6
#define DBGMSG(A)    if (dbg){ Serial.print("DBG: "); Serial.println(A);}

//
// Globals
//
SerialCommand     serialCommand;
boolean           dbg = true;

//
// Initialize the serial command table, I/O pins, and the temperature sensor
//
void setup() {
  Serial.begin(9600);
  serialCommand.addCommand("type", TypeCommand);
  serialCommand.addCommand("press", PressKeyCommand);
  
  serialCommand.addCommand("debug", SetDebug );
}

//
// Read and respond to commands recieved over the serial port
//
void loop() {
  serialCommand.readSerial();
}

void PressKeyCommand() {
  String arg = serialCommand.next();

  int holdMilliseconds = 50;
  String nextArg = serialCommand.next();
  while(nextArg != NULL) {
    if(isValidNumber(nextArg)) {
      holdMilliseconds = nextArg.toInt();
      break;
    } else {
      nextArg = serialCommand.next();      
    }
  }
  
  Keyboard.begin();
  Keyboard.releaseAll();
  if(arg == "space") {
    Keyboard.press(' ');
  }
  else if (arg == "shift") {
    Keyboard.press(KEY_LEFT_SHIFT);
  }
  else if (arg == "ctrl") {
    Keyboard.press(KEY_LEFT_CTRL);
  }
  else if (arg == "alt") {
    Keyboard.press(KEY_LEFT_ALT);
  }
  else if (arg == "tab") {
    Keyboard.press(KEY_TAB);
  }
  else if (arg == "esc") {
    Keyboard.press(KEY_ESC);
  }
  else {
    
    Keyboard.press(arg[0]);
  }
  delay(holdMilliseconds);   
  Keyboard.releaseAll();
  Keyboard.end();
}
void TypeCommand() {
  Keyboard.begin();
  Keyboard.releaseAll();
  char *arg = serialCommand.next();
  while(arg != NULL) {
    Keyboard.print(" ");
    Keyboard.print(arg);
    delay(10);   
    arg = serialCommand.next();
  }
  Keyboard.releaseAll();
  Keyboard.end();
}
boolean isValidNumber(String str){
   boolean isNum=false;
   for(byte i=0;i<str.length();i++)
   {
       isNum = isDigit(str.charAt(i)) || str.charAt(i) == '+' || str.charAt(i) == '.' || str.charAt(i) == '-';
       if(!isNum) return false;
   }
   return isNum;
}
//
// Enable or disable debug messages from being printed
// on the serial terminal
//
void SetDebug() {
  char *arg = serialCommand.next();

  if (arg != NULL) {
    if ( strcmp(arg, "on" ) == 0) {
      dbg = true;
      DBGMSG(F("Turn on debug"));
    }
    if ( strcmp(arg, "off" ) == 0) {
      DBGMSG(F("Turn off debug"));
      dbg = false;
    }
  }
}

//
// An unrecognized command was recieved
//
void UnrecognizedCommand() {
  DBGMSG(F("Unrecognized command"));
  DBGMSG(F(" ledon 3  - turn on led connected to digital I/O 3"));
  DBGMSG(F(" ledon 4  - turn on led connected to digital I/O 4"));
  DBGMSG(F(" ledon 5  - turn on led connected to digital I/O 5"));
  DBGMSG(F(" ledon 6  - turn on led connected to digital I/O 6"));
  DBGMSG(F(" ledoff 3 - turn off led connected to digital I/O 3"));
  DBGMSG(F(" ledoff 4 - turn off led connected to digital I/O 4"));
  DBGMSG(F(" ledoff 5 - turn off led connected to digital I/O 5"));
  DBGMSG(F(" ledoff 6 - turn off led connected to digital I/O 6"));
  DBGMSG(F(" temp     - read temperature" ));
  DBGMSG(F(" debug on - turn on debug messages" ));
  DBGMSG(F(" debug off- turn off debug messages" ));
}
