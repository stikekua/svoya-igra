// THE MAC Addresses of your clients
// master 8c:aa:b5:63:29:42
// red    e8:db:84:96:f4:e1
// green  e8:db:84:9a:8c:a1
// blue   E8:DB:84:9A:89:8E
// yellow E8:DB:84:9A:92:BC

//uint8_t masterAddress[] = {0x8C, 0xAA, 0xB5, 0x63, 0x29, 0x42};

  //{0xE8, 0xDB, 0x84, 0x9A, 0x8C, 0xA1}, // green
  
uint8_t clientsCount = 4;
uint8_t clientAddress[][6] = {
  {0xE8, 0xDB, 0x84, 0x96, 0xF4, 0xE1}, // red
  {0x38, 0x2B, 0x78, 0x05, 0x13, 0xD8},
  {0xE8, 0xDB, 0x84, 0x9A, 0x89, 0x8E}, // blue
  {0xE8, 0xDB, 0x84, 0x9A, 0x92, 0xBC}  // yellow
};

void OnDataSent(uint8_t *mac_addr, uint8_t sendStatus);
void OnDataRecv(uint8_t * mac, uint8_t *incomingData, uint8_t len);

bool espnowInit() {
  // Init ESP-NOW
  if (esp_now_init() != 0) {
    return false;
  }

  // Set ESP-NOW Role
  esp_now_set_self_role(ESP_NOW_ROLE_COMBO);

  // Once ESPNow is successfully Init, we will register for Send CB to
  // get the status of Trasnmitted packet
  esp_now_register_send_cb(OnDataSent);


  for (int i = 0; i < clientsCount; i++) {
    // Register peer
    esp_now_add_peer(clientAddress[i], ESP_NOW_ROLE_COMBO, 1, NULL, 0);
  }

  // Register for a callback function that will be called when data is received
  esp_now_register_recv_cb(OnDataRecv);

  return true;
}

// Callback when data is received
void OnDataRecv(uint8_t * mac, uint8_t *incomingData, uint8_t len) {
  memcpy(&msgIn, incomingData, sizeof(msgIn));
  Serial.println();
  Serial.print("Bytes received: ");
  Serial.println(len);
  Serial.print("Mac address: ");
  for (int i = 0; i < 6; i++) {
    Serial.print(mac[i], HEX);
    Serial.print(":");
  }
  Serial.println();
  Serial.print("pressed "); Serial.println(msgIn.pressed ? "true" : "false");
  Serial.print("selected "); Serial.println(msgIn.selected ? "true" : "false");
  Serial.print("deselected "); Serial.println(msgIn.deselected ? "true" : "false");

  if (msgIn.pressed) {
    EVH_buttonPressed(msgIn.id);
  }
}

// Callback when data is sent
void OnDataSent(uint8_t *mac_addr, uint8_t sendStatus) {
  Serial.print("Last Packet Send Status: ");
  if (sendStatus == 0) {
    Serial.println("Delivery success");
  }
  else {
    Serial.print("Delivery fail: ");
    Serial.println(sendStatus);
  }
}

void EN_sendMsg(cCommand cmd, cButton btn) {
  Serial.print("EN_sendMsg: cmd - ");
  Serial.print(cmd);
  Serial.print(", btn - ");
  Serial.println(btn);
  //Find button's MAC id
  int btnMac = findMacId(btn);
  //Set values to send
  switch (cmd) {
    case CMD_SELECT:
      msgOut.select = true;
      msgOut.deselect = false;
      msgOut.release = false;
      break;
    case CMD_DESELECT:
      msgOut.select = false;
      msgOut.deselect = true;
      msgOut.release = false;
      break;
  }
  // Send message via ESP-NOW
  esp_now_send(clientAddress[btnMac], (uint8_t *) &msgOut, sizeof(msgOut));
}

void EN_broadcastMsg(cCommand cmd) {
  if (cmd == CMD_RELEASE) {
    //Set values to send
    msgOut.select = false;
    msgOut.deselect = false;
    msgOut.release = true;

    for (int i = 0; i < clientsCount; i++) {
      // Send message via ESP-NOW
      esp_now_send(clientAddress[i], (uint8_t *) &msgOut, sizeof(msgOut));
    }
  }
}
int findMacId(cButton btn) {
  switch (btn) {
    case RED:
      return 0;
    case GREEN:
      return 1;
    case BLUE:
      return 2;
    case YELLOW:
      return 3;
  }
}
