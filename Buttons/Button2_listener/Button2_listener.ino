//Button2 listener

#include <ESP8266WiFi.h>
#include <espnow.h>
#include <timer.h>

uint8_t myMac[6];
#define CHANNEL 1

int flag = 0;

void OnDataSent(uint8_t *mac_addr, uint8_t sendStatus);
void OnDataRecv(uint8_t *mac, uint8_t *incomingData, uint8_t len);

enum MessageType { PAIRING = 17,
                   DATA = 1};
MessageType messageType;

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

struct_message_out msgIn;
struct_pairing pairing;

void setup() {
  Serial.begin(115200);
  delay(10);

  // Set device as a Wi-Fi Station
  WiFi.mode(WIFI_STA);
  Serial.println();
  Serial.print("My MAC Address:  ");
  Serial.println(WiFi.macAddress());

  WiFi.macAddress(myMac);
  printMAC(myMac);
  Serial.println();

  WiFi.disconnect();

  // Init ESP-NOW
  if (esp_now_init() != 0) {  //ESP_OK
    Serial.println("Error initializing ESP-NOW");
    delay(500);
    ESP.restart();
  }
  esp_now_set_self_role(ESP_NOW_ROLE_COMBO);

  esp_now_register_send_cb(OnDataSent);
  esp_now_register_recv_cb(OnDataRecv);

}

void loop() {

  // send data only when you receive data:
  if (Serial.available() > 0) {
    // read the incoming byte:
    int incomingByte = Serial.read();

    // say what you got:
    Serial.print("I received: ");
    Serial.println(incomingByte, DEC);

    if(incomingByte == 49){ // 1 = ascii
      flag = 1;
    }
  }

}

// Callback when data is sent
void OnDataSent(uint8_t *mac_addr, uint8_t status) {
  Serial.print("Last Packet Send Status: ");
  Serial.print(status == 0 ? "Delivery Success to " : "Delivery Fail to ");  //0=ESP_NOW_SEND_SUCCESS
  printMAC(mac_addr);
  Serial.println();
}

// Callback when data is received
void OnDataRecv(uint8_t *mac, uint8_t *incomingData, uint8_t len) {
  Serial.print(len);
  Serial.print(" bytes of data received from : ");
  printMAC(mac);
  Serial.println();

  uint8_t type = incomingData[0];  // first message byte is the type of message
  switch(type) {
    case DATA:  // the message is data
      memcpy(&msgIn, incomingData, sizeof(msgIn));

      Serial.println("DATA");

      Serial.print("ID: ");
      Serial.print(msgIn.id);
      Serial.print(", ");
      Serial.print(msgIn.pressed);
      Serial.print(", ");
      Serial.print(msgIn.falsestart);
      Serial.print(", ");
      Serial.print(msgIn.selected);
      Serial.print(", ");
      Serial.print(msgIn.deselected);
      Serial.println();
      break;
    case PAIRING:  // the message is a pairing
      memcpy(&pairing, incomingData, sizeof(pairing));
      Serial.println("PAIRING");

      Serial.print("ID: ");
      Serial.print(pairing.id);
      Serial.print(" MAC: ");
      printMAC(pairing.macAddr);
      Serial.print(" Channel: ");
      Serial.print(pairing.channel);
      Serial.println();

      if(flag == 1){
        flag = 0;
        
        esp_now_add_peer(mac, ESP_NOW_ROLE_COMBO, CHANNEL, NULL, 0);
        sendMsg(mac);

      }

      break;
  }
}

void printMAC(uint8_t *mac_addr) {
  char macStr[18];
  snprintf(macStr, sizeof(macStr), "%02x:%02x:%02x:%02x:%02x:%02x",
           mac_addr[0], mac_addr[1], mac_addr[2], mac_addr[3], mac_addr[4], mac_addr[5]);
  Serial.print(macStr);
}

void sendMsg(uint8_t *mac_addr) {
  pairing.id = 0;
  pairing.msgType = PAIRING;
  memcpy(&pairing.macAddr, myMac, sizeof(pairing.macAddr));
  //pairingOut.macAddr = myMac;
  pairing.channel = CHANNEL;

  // Send message via ESP-NOW
  esp_now_send(mac_addr, (uint8_t *)&pairing, sizeof(pairing));
}