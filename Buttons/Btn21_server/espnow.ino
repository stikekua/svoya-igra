// THE MAC Addresses of your clients
// master 8c:aa:b5:63:29:42
// red    e8:db:84:96:f4:e1
// green  e8:db:84:9a:8c:a1
// blue   E8:DB:84:9A:89:8E
// yellow E8:DB:84:9A:92:BC

//uint8_t masterAddress[] = {0x8C, 0xAA, 0xB5, 0x63, 0x29, 0x42};

// {0x38, 0x2B, 0x78, 0x05, 0x13, 0xD8}

void OnDataSent(uint8_t *mac_addr, uint8_t sendStatus);
void OnDataRecv(uint8_t * mac, uint8_t *incomingData, uint8_t len);

bool espnow_init() {
  // Init ESP-NOW
  if (esp_now_init() != 0) {
    return false;
  }

  // Set ESP-NOW Role
  esp_now_set_self_role(ESP_NOW_ROLE_COMBO);

  // Once ESPNow is successfully Init, we will register for Send CB to
  // get the status of Trasnmitted packet
  esp_now_register_send_cb(OnDataSent);

  // Register for a callback function that will be called when data is received
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
void OnDataRecv(uint8_t * mac, uint8_t *incomingData, uint8_t len) {
  Serial.print(len);
  Serial.print(" bytes of data received from : ");
  printMAC(mac);
  Serial.println();

  uint8_t type = incomingData[0];  // first message byte is the type of message
  switch(type) {
    case DATA:  // the message is data
      memcpy(&msgIn, incomingData, sizeof(msgIn));

      Serial.print("pressed "); Serial.println(msgIn.pressed ? "true" : "false");
      Serial.print("falsestart "); Serial.println(msgIn.falsestart ? "true" : "false");
      Serial.print("selected "); Serial.println(msgIn.selected ? "true" : "false");
      Serial.print("deselected "); Serial.println(msgIn.deselected ? "true" : "false");

      if (msgIn.pressed && !msgIn.falsestart) {
        EVH_buttonPressed(msgIn.id);
      }
      break;
    case PAIRING:  // the message is a pairing
      memcpy(&pairing, incomingData, sizeof(pairing));
      Serial.print("Pairing message from "); Serial.print(pairing.id); Serial.println(" recieved.");
      if(permitPairing){        
        //add peer
        esp_now_add_peer(mac, ESP_NOW_ROLE_COMBO, CHANNEL, NULL, 0);  // add the client to the peer list
        
        //save to eeprom  
        uint8_t idx = buttonId2Index(static_cast<cButton>(pairing.id));
        uint8_t eAddress = EEPROM_MAC_START + idx * 6;
        setMACToEEPROM(mac, eAddress);
        //setChannelToEEPROM(pairing.channel);
        commitEEPROM();
        
        //send to client
        espnow_sendPairingMessage(mac);
        Serial.print("Pairing for "); Serial.print(pairing.id); Serial.println(" confirmed.");
      }
      break;
    default:
      Serial.print("Unknown messageType "); Serial.println(type);
  }  
}

void espnow_sendMsg(cCommand cmd, cButton btn) {
  Serial.print("EN_sendMsg: cmd - ");
  Serial.print(cmd);
  Serial.print(", btn - ");
  Serial.println(btn);
  //Find button's MAC id
  uint8_t btnMac = buttonId2Index(btn);
  //Set values to send
  switch (cmd) {
    case CMD_SELECT:
      msgOut.enable = true;
      msgOut.select = true;
      msgOut.deselect = false;
      msgOut.release = false;
      break;
    case CMD_DESELECT:
      msgOut.enable = true;
      msgOut.select = false;
      msgOut.deselect = true;
      msgOut.release = false;
      break;
  }
  // Send message via ESP-NOW
  
  //TODO check client isMACEmpty
  esp_now_send(clientAddress[btnMac], (uint8_t *) &msgOut, sizeof(msgOut));
}

void espnow_broadcastMsg(cCommand cmd) {
  switch (cmd) {
    case CMD_ENABLE:
      msgOut.enable = true;
      msgOut.select = false;
      msgOut.deselect = false;
      msgOut.release = false;
      break;
    case CMD_RELEASE:
      msgOut.enable = false;
      msgOut.select = false;
      msgOut.deselect = false;
      msgOut.release = true;
      break;
  }

  for (int i = 0; i < BUTTON_CLIENT_COUNT; i++) {
    // Send message via ESP-NOW

    //TODO check client isMACEmpty
    esp_now_send(clientAddress[i], (uint8_t *)&msgOut, sizeof(msgOut));
  }
}

void espnow_sendPairingMessage(uint8_t *mac){
  pairing.msgType = PAIRING;
  pairing.id = 0; //server
  memcpy(&pairing.macAddr, myMac, sizeof(pairing.macAddr));
  pairing.channel = CHANNEL;

  esp_now_send(mac, (uint8_t *)&pairing, sizeof(pairing));
}
