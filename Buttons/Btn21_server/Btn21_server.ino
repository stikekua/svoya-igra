
#include <ESP8266WiFi.h>
#include <ESP8266WiFiMulti.h>
#include <WebSocketsServer.h>
#include <espnow.h>
#include <Hash.h>
#include <ESP8266WebServer.h>
#include <ESP8266mDNS.h>
#include <timer.h>
#include <Adafruit_NeoPixel.h>

#define LED_PIN 2
#define LED_COUNT 32
bool ledon = false;

IPAddress _apIP(192, 168, 5, 1);
String _ssidAP = "SvoyaIgraAP2";
String _passwordAP = "12345678";

MDNSResponder mdns;
ESP8266WebServer server(80);
WebSocketsServer webSocket = WebSocketsServer(81);

Timer printTimer(1000);

Adafruit_NeoPixel strip(LED_COUNT, LED_PIN, NEO_GRB + NEO_KHZ800);

// Web2Server
const char SG_NEXT[] = "SGNext";
const char SG_RESET[] = "SGReset";
char ButtonStateMsg[20];

enum cButton { RED = 1, GREEN = 2, BLUE = 4, YELLOW = 8 };
enum cCommand { CMD_NULL, CMD_SELECT, CMD_DESELECT, CMD_RELEASE };
uint32_t buttonColors[] = {
  strip.Color(125, 0, 0), // red
  strip.Color(0,  125, 0), // green
  strip.Color(0,  0, 125), // blue
  strip.Color(125, 125, 0)  // yellow
};

//game status
bool started = false;
uint8_t duration = 0;
//memory queue
int queue[4] = {0, 0, 0, 0};
//queue selector
int queueSelector = 0;

//Structures to send data
typedef struct struct_message_in {
  int id;
  bool pressed;
  bool selected;
  bool deselected;
} struct_message_in;
typedef struct struct_message_out {
  bool select;
  bool deselect;
  bool release;
} struct_message_out;

// Create a structure objects
struct_message_in msgIn;
struct_message_out msgOut;

void setup() {
  Serial.begin(115200);

  WiFi.persistent(false);
  WiFi.mode(WIFI_AP_STA);
  WiFi.disconnect();
  delay(1000);

  WiFi.softAPConfig(_apIP, _apIP, IPAddress(255, 255, 255, 0));
  WiFi.softAP(_ssidAP.c_str(), _passwordAP.c_str(), 1);

  WiFi.printDiag(Serial);

  for (uint8_t t = 4; t > 0; t--) {
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
  if (!espnowInit()) {
    Serial.println("Error initializing ESP-NOW");
  }

  //LED
  ledInit();

  //reset
  EVH_reset();
}

void loop() {
  webSocket.loop();
  server.handleClient();

  if (printTimer.ready()) {
//    Serial.print("msgIn.pressed ");
//    Serial.println(msgIn.pressed);
//    Serial.print("msgIn.selected ");
//    Serial.println(msgIn.selected);
    
    if(started){      
      duration++;
      LED_showTimer(duration);
      if(duration > 15) duration = 15;
    }
  }
  
}
