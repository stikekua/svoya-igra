
#include <ESP8266WiFi.h>
#include <espnow.h>
#include <GyverButton.h>
#include <timer.h>

#define BTN_ID 1 // 1=R, 2=G, 4=B, 8=Y

#define MAINBUTTON_PIN 13
#define PAIRBTN_PIN 5
#define LED_PIN 4
#define LED_BLINK_TIME 500
#define LED_BLINK2_TIME 250

GButton mainBtn(MAINBUTTON_PIN); // (по умолч. HIGH_PULL и NORM_OPEN)
GButton pairBtn(PAIRBTN_PIN); // (по умолч. HIGH_PULL и NORM_OPEN)

#define BLOCK_TIME 1000
unsigned long currentMillis;
unsigned long blockTimerMillis;
bool blocked = false;

// Current LED status
enum LED { OFF, ON, BLINK, BLINK_ERROR };
LED LEDMode = OFF;

Timer blinkTimer(LED_BLINK_TIME);          // timer blink
Timer blink2Timer(LED_BLINK2_TIME);

//Structures to send data
typedef struct struct_message_in {
  bool enable;
  bool select;
  bool deselect;
  bool release;
} struct_message_in;
typedef struct struct_message_out {
  int id;
  bool pressed;
  bool falsestart;
  bool selected;
  bool deselected;
} struct_message_out;

// Create a structure objects
struct_message_in msgIn;
struct_message_out msgOut;

void setup() {
  Serial.begin(115200);
  pinMode(LED_PIN, OUTPUT);
  digitalWrite(LED_PIN, HIGH);

  // Set device as a Wi-Fi Station
  WiFi.mode(WIFI_STA);
  WiFi.disconnect();

  if (!esnow_init()) {
    Serial.println("Error initializing ESP-NOW");
    LEDMode = BLINK_ERROR;
    ESP.restart();
  }

  //Set button id to send
  msgOut.id = BTN_ID;
  sendMsg();
}

void loop() {
  mainBtn.tick();
  currentMillis = millis();
  
  if(blocked){
    if (currentMillis - blockTimerMillis >= BLOCK_TIME)
    {
      blocked = false;
    }
  }  

  //Button pressed
  if (mainBtn.isPress()) {
    if (!msgIn.enable && !blocked){
      blocked = true;
      blockTimerMillis = currentMillis;
    }
    
    //Set values to send
    msgOut.pressed = true;
    msgOut.falsestart = !msgIn.enable || blocked;
    // Send message via ESP-NOW
    sendMsg();
    //Set led mode
    if (msgIn.enable && !msgOut.selected) LEDMode = ON;
  }

  //Set led mode, according actual status
  if (msgOut.deselected) {
    LEDMode = OFF;
  }
  else if (msgOut.selected) {
    LEDMode = BLINK;
  }
  //Set led mode off, if no state and no error.
  if (!msgOut.pressed && !msgOut.selected && !msgOut.deselected) {
    if(LEDMode != BLINK_ERROR){
      LEDMode = OFF;
    }
  }

  exec_Led(LEDMode);
}
