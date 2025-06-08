
void OnDataSent(uint8_t *mac_addr, uint8_t sendStatus);
void OnDataRecv(uint8_t *mac, uint8_t *incomingData, uint8_t len);

bool espnow_init() {
  // Init ESP-NOW
  if (esp_now_init() != 0) {  //ESP_OK
    return false;
  }
  esp_now_set_self_role(ESP_NOW_ROLE_COMBO);

  esp_now_register_send_cb(OnDataSent);
  esp_now_register_recv_cb(OnDataRecv);
  return true;
}

void espnow_register(uint8_t *mac) {
  // Register peer
  esp_now_add_peer(mac, ESP_NOW_ROLE_COMBO, CHANNEL, NULL, 0);
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

      if (msgIn.enable) {
        Serial.println("enable");
      }
      if (msgIn.select) {
        Serial.println("select");
        msgOut.selected = true;
      }
      if (msgIn.deselect) {
        Serial.println("deselect");
        msgOut.deselected = true;
      }
      if (msgIn.release) {
        Serial.println("release");
        msgOut.pressed = false;
        msgOut.selected = false;
        msgOut.deselected = false;
      }
      break;
    case PAIRING:  // the message is a pairing
      memcpy(&pairing, incomingData, sizeof(pairing));
      if (pairing.id == 0) {  // the message comes from server
        Serial.print("Pairing done for ");
        printMAC(pairing.macAddr);
        Serial.print(" on channel ");
        Serial.println(pairing.channel);  // channel used by the server
        //esp_now_del_peer(pairing.macAddr);
        //esp_now_del_peer(mac);
        esp_now_add_peer(pairing.macAddr, ESP_NOW_ROLE_COMBO, CHANNEL, NULL, 0);  // add the server to the peer list
        
        setMACToEEPROM(pairing.macAddr);
        //setChannelToEEPROM(pairing.channel);
        commitEEPROM();
        
        ESP.restart();
      }
      break;
  }
}

void sendMsg() {
  // Send message via ESP-NOW
  esp_now_send(masterAddress, (uint8_t *)&msgOut, sizeof(msgOut));
}

void brodcastPairing() {
  uint8_t broadcastAddr[] = { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
  esp_now_send(broadcastAddr, (uint8_t *)&pairing, sizeof(pairing));
}
