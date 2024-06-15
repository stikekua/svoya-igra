
// THE MAC Address of your receiver
// master     8c:aa:b5:63:29:42
// devboard   38:2B:78:05:13:D8
// Solvinden server - BC:FF:4D:2A:BA:87
uint8_t masterAddress[] = {0xBC, 0xFF, 0x4D, 0x2A, 0xBA, 0x87};

void OnDataSent(uint8_t *mac_addr, uint8_t sendStatus);
void OnDataRecv(uint8_t * mac, uint8_t *incomingData, uint8_t len);

bool esnow_init() {
  // Init ESP-NOW
  if (esp_now_init() != 0) {
    return false;
  }

  // Set ESP-NOW Role
  esp_now_set_self_role(ESP_NOW_ROLE_COMBO);

  // Once ESPNow is successfully Init, we will register for Send CB to
  // get the status of Trasnmitted packet
  esp_now_register_send_cb(OnDataSent);

  // Register peer
  esp_now_add_peer(masterAddress, ESP_NOW_ROLE_COMBO, 1, NULL, 0);

  // Register for a callback function that will be called when data is received
  esp_now_register_recv_cb(OnDataRecv);

  return true;
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

// Callback when data is received
void OnDataRecv(uint8_t * mac, uint8_t *incomingData, uint8_t len) {
  memcpy(&msgIn, incomingData, sizeof(msgIn));
  Serial.print("Bytes received: ");
  Serial.println(len);
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
  //feedback
  //sendMsg();
}

void sendMsg() {
  // Send message via ESP-NOW
  esp_now_send(masterAddress, (uint8_t *) &msgOut, sizeof(msgOut));
}
