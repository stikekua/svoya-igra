
#include <ESP8266WiFi.h>
#include <espnow.h>
#include <GyverButton.h>
#include <timer.h>
#include <EEPROM.h>

#define EEPROM_MAC_OFFSET 4
#define CHANNEL 1
uint8_t masterAddress[6];
uint8_t myMac[6];

#define BTN_ID 1 
// 1  Red
// 2  Green
// 4  Blue
// 8  Yellow

#define MAINBUTTON_PIN 13
#define PAIRBTN_PIN 5
#define LED_PIN 4
#define LED_BLINK_TIME 500
#define LED_BLINK2_TIME 250
#define LED_PAIRING_TIME 100
#define TIMER1_TIME 5000

GButton mainBtn(MAINBUTTON_PIN);  // (по умолч. HIGH_PULL и NORM_OPEN)
//GButton pairBtn(PAIRBTN_PIN); // (по умолч. HIGH_PULL и NORM_OPEN)

#define BLOCK_TIME 1000
unsigned long currentMillis;
unsigned long blockTimerMillis;
bool blocked = false;

// Current LED status
enum LED { OFF,
           ON,
           BLINK,
           BLINK_ERROR,
           BLINK_PAIRING };
LED LEDMode = OFF;

Timer blinkTimer(LED_BLINK_TIME);  // timer blink
Timer blink2Timer(LED_BLINK2_TIME);
Timer blinkPairingTimer(LED_PAIRING_TIME);
Timer Timer1(TIMER1_TIME);

enum MessageType { PAIRING = 17,
                   DATA = 1};
MessageType messageType;

//Structures to send data
typedef struct struct_message_in {
  uint8_t msgType;
  uint8_t id;
  bool enable;
  bool select;
  bool deselect;
  bool release;
} struct_message_in;

typedef struct struct_message_out {
  uint8_t msgType;
  uint8_t id;
  bool pressed;
  bool falsestart;
  bool selected;
  bool deselected;
} struct_message_out;

typedef struct struct_pairing {
  uint8_t msgType;
  uint8_t id;
  uint8_t macAddr[6];
  uint8_t channel;
} struct_pairing;

// Create a structure objects
struct_message_in msgIn;
struct_message_out msgOut;
struct_pairing pairing;

void setup() {
  Serial.begin(115200);
  delay(10);

  EEPROM.begin(64);  // возвращает void на ESP8266
  Serial.println("EEPROM initialized");

  pinMode(BUILTIN_LED, OUTPUT);
  pinMode(LED_PIN, OUTPUT);
  digitalWrite(LED_PIN, HIGH);

  // Set device as a Wi-Fi Station
  WiFi.mode(WIFI_STA);
  Serial.println();
  Serial.print("My MAC Address:  ");
  Serial.println(WiFi.macAddress());
  WiFi.macAddress(myMac);
  WiFi.disconnect();

  if (!espnow_init()) {
    Serial.println("Error initializing ESP-NOW");
    LEDMode = BLINK_ERROR;
    delay(500);
    ESP.restart();
  }

  //Set button id
  msgOut.id = BTN_ID;
  Serial.print("Button ID ");
  Serial.println(BTN_ID);

  //read eeprom
  getMACFromEEPROM(masterAddress);
  Serial.print("MAC from EEPROM ");
  printMAC(masterAddress);
  Serial.println();

  //mode
  if (digitalRead(PAIRBTN_PIN) == LOW || isMACEmpty(masterAddress)) {
    //pairing
    Serial.println("Pairing mode");
    LEDMode = BLINK_PAIRING;
    makePairingMessage();
    brodcastPairing();
  } else {
    //normal mode
    Serial.println("Normal mode");
    espnow_register(masterAddress);
    msgOut.msgType = DATA;
    sendMsg();
    led_off();
  }
}

void loop() {
  mainBtn.tick();
  currentMillis = millis();
  exec_Led(LEDMode);

  if (LEDMode == BLINK_PAIRING) {
    //pairing
    if (Timer1.ready()) {      
      makePairingMessage();
      brodcastPairing();
    }
  } else {
    //normal mode

    if (blocked) {
      if (currentMillis - blockTimerMillis >= BLOCK_TIME) {
        blocked = false;
      }
    }

    //Button pressed
    if (mainBtn.isPress()) {
      if (!msgIn.enable && !blocked) {
        blocked = true;
        blockTimerMillis = currentMillis;
      }

      //Set values to send
      msgOut.pressed = true;
      msgOut.falsestart = !msgIn.enable || blocked;
      // Send message via ESP-NOW
      msgOut.msgType = DATA;
      sendMsg();
      //Set led mode
      if (msgIn.enable && !msgOut.selected) LEDMode = ON;
    }

    //Set led mode, according actual status
    if (msgOut.deselected) {
      LEDMode = OFF;
    } else if (msgOut.selected) {
      LEDMode = BLINK;
    }
    //Set led mode off, if no state and no error.
    if (!msgOut.pressed && !msgOut.selected && !msgOut.deselected) {
      if (LEDMode != BLINK_ERROR) {
        LEDMode = OFF;
      }
    }
  }
}

void makePairingMessage(){
  pairing.msgType = PAIRING;
  pairing.id = BTN_ID;
  memcpy(&pairing.macAddr, myMac, sizeof(pairing.macAddr));
  pairing.channel = CHANNEL;
}
