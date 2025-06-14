
#include <ESP8266WiFi.h>
#include <ESP8266WiFiMulti.h>
#include <WebSocketsServer.h>
#include <espnow.h>
#include <Hash.h>
#include <ESP8266WebServer.h>
#include <ESP8266mDNS.h>
#include <timer.h>
#include <EEPROM.h>

#define BUTTON_CLIENT_COUNT 4

#define CHANNEL 1
uint8_t myMac[6];
#define EEPROM_MAC_START  4
uint8_t clientAddress[BUTTON_CLIENT_COUNT][6];
bool permitPairing = false;

IPAddress _apIP(192, 168, 5, 1);
String _ssidAP = "SvoyaIgraAP2";
String _passwordAP = "12345678";

MDNSResponder mdns;
ESP8266WebServer server(80);
WebSocketsServer webSocket = WebSocketsServer(81);

Timer printTimer(1000);
Timer gameTimer(500);
#define ROUND_TIME 32

#include <Adafruit_NeoPixel.h>
#define LED_PIN    4
#define LED_COUNT 32
#define LED2_PIN   5
#define LED2_COUNT 4

Adafruit_NeoPixel mainStrip(LED_COUNT, LED_PIN, NEO_GRB + NEO_KHZ800);
Adafruit_NeoPixel stateStrip(LED2_COUNT, LED2_PIN, NEO_GRB + NEO_KHZ800);

#include "GyverButton.h"
#define BTN_PIN 12
GButton btn(BTN_PIN, LOW_PULL, NORM_OPEN);

int batteryValue;

// Web2Server
const char SG_NEXT[] = "SGNext";
const char SG_RESET[] = "SGReset";
const char SG_START[] = "SGStart";
char ButtonStateMsg[20];

enum cButton { RED = 1, GREEN = 2, BLUE = 4, YELLOW = 8 };
enum cCommand { CMD_NULL, CMD_SELECT, CMD_DESELECT, CMD_RELEASE, CMD_ENABLE };
uint32_t buttonColors[] = {
  mainStrip.Color(125, 0, 0), // red
  mainStrip.Color(0,  125, 0), // green
  mainStrip.Color(0,  0, 125), // blue
  mainStrip.Color(150, 125, 0)  // yellow
};

//game status
bool started = false;
uint8_t duration = 0;
//memory queue
int queue[4] = {0, 0, 0, 0};
//queue selector
int queueSelector = 0;

enum MessageType { PAIRING = 17,
                   DATA = 1};
MessageType messageType;

//Structures to send data
typedef struct struct_message_in {
  uint8_t msgType;
  uint8_t id;
  bool pressed;
  bool falsestart;
  bool selected;
  bool deselected;
} struct_message_in;
typedef struct struct_message_out {
  uint8_t msgType;
  uint8_t id;
  bool enable;
  bool select;
  bool deselect;
  bool release;
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

  //LED
  ledInit();
  
  WiFi.persistent(false);
  WiFi.mode(WIFI_AP_STA);
  Serial.println();
  Serial.print("My MAC Address:  ");
  Serial.println(WiFi.macAddress());
  WiFi.macAddress(myMac);
  WiFi.disconnect();
  delay(1000);

  WiFi.softAPConfig(_apIP, _apIP, IPAddress(255, 255, 255, 0));
  WiFi.softAP(_ssidAP.c_str(), _passwordAP.c_str(), 1);

  WiFi.printDiag(Serial);

  for (uint8_t t = 4; t > 0; t--) {
    LED_stateLed(map(t,4,1,0,3));
    Serial.printf("[SETUP] BOOT WAIT %d...\r\n", t);
    Serial.flush();
    delay(1000);
  }

  if (mdns.begin("svoyaigra", WiFi.softAPIP())) {
    Serial.println("MDNS responder started");
    mdns.addService("http", "tcp", 80);
    mdns.addService("ws", "tcp", 81);
    Serial.print("Connect to http://svoyaigra.local or http://");
    Serial.println(WiFi.softAPIP());
  }
  else {
    Serial.println("MDNS.begin failed");
  }

  // Init html web server
  htmlInit();

  // Init webSocket server
  webSocketInit();
  
  // Init ESP-NOW
  if (!espnow_init()) {
    Serial.println("Error initializing ESP-NOW");
  }

  // Read Button clients mac and add it
  for (int i = 0; i < BUTTON_CLIENT_COUNT; i++) {
    uint8_t eAddress = EEPROM_MAC_START + i * 6;
    uint8_t tMac[6];
    getMACFromEEPROM(tMac, eAddress);
    Serial.print("MAC from EEPROM ");
    printMAC(tMac);
    Serial.println();

    if(!isMACEmpty(tMac)){      
      memcpy(&clientAddress[i], tMac, sizeof(clientAddress[i] ));
      espnow_register(clientAddress[i]);
    }    
  }

  LED_stateOff();

  //reset
  EVH_reset();
}

void loop() {
  webSocket.loop();
  server.handleClient();

  if (gameTimer.ready()) {
    if(!started){
      LED_stateWaiting(0);
    }
    if (started && (duration < ROUND_TIME * 2)) {
      duration++;
      LED_showTimer();
    }
  }

  if (printTimer.ready()) {
    //    Serial.print("msgIn.pressed ");
    //    Serial.println(msgIn.pressed);
    //    Serial.print("msgIn.selected ");
    //    Serial.println(msgIn.selected);

    //ADC
    batteryValue = analogRead(A0); // 0..1023
    Serial.print("batteryValue = ");
    Serial.println(batteryValue);
  }

  btn.tick();
  if (btn.isClick()) {
    Serial.println("Click");
    if(!started){
      EVH_start();
    } else {
      EVH_next();
    }
  }
  if (btn.isHolded()) {
    Serial.println("Holded");
    EVH_reset();
  }

}
