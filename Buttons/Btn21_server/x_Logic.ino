// *******************************************************************
// ********************* EVENT HANDLERS ******************************
// *******************************************************************

//event handler for start
void EVH_start(){
   Serial.println("start>>");
  if (!started) {
    started = true;
    LED_showQueue();
    //enable buttons
    espnow_broadcastMsg(CMD_ENABLE);
  }
}

//event handler for next button or web cmd
void EVH_next() {
  Serial.println("next>>");

  if (!started) {
    return;
  }

  if (queueSelector >= 3) {
    Serial.println("End of the queue");
    queueSelector = 3;
  } else if (queue[queueSelector] == 0) {
    Serial.println("Nobody else in the queue");
    LED_Off();
  } else if (queueSelector < 3 && queue[queueSelector] != 0) {
    //deselect current
    espnow_sendMsg(CMD_DESELECT, static_cast<cButton>(queue[queueSelector]));
    //move queue
    queueSelector++;
    duration = 0;
    //select next in queue if someone
    if (queue[queueSelector] != 0) {
      espnow_sendMsg(CMD_SELECT, static_cast<cButton>(queue[queueSelector]));
      LED_setColor(static_cast<cButton>(queue[queueSelector]));
    }
    else {
      LED_Off();
    }
  }
  //send actulal status
  makeWSMessage();
  webSocket.broadcastTXT(ButtonStateMsg, strlen(ButtonStateMsg));
}

//event handler for reset button or web cmd
void EVH_reset() {
  Serial.println("reset>>");
  //reset
  started = false;
  duration = 0;
  //reset selector
  queueSelector = 0;
  //clear queue
  for (int i = 0; i < (sizeof(queue) / sizeof(queue[0])); i++) {
    queue[i] = 0;
  }
  //release buttons
  espnow_broadcastMsg(CMD_RELEASE);
  LED_Off();
  //send actulal status
  makeWSMessage();
  webSocket.broadcastTXT(ButtonStateMsg, strlen(ButtonStateMsg));
}

void EVH_buttonPressed(int button) {
  if (!started) {
    return;
  }

  // add button to queue
  add2Queue(button);
  // added button is first in queue
  if (queue[queueSelector] == button) {
    espnow_sendMsg(CMD_SELECT, static_cast<cButton>(queue[queueSelector]));
    LED_setColor(static_cast<cButton>(queue[queueSelector]));
    duration = 0;
  }
  //send actulal status
  makeWSMessage();
  webSocket.broadcastTXT(ButtonStateMsg, strlen(ButtonStateMsg));
  //led
  LED_showQueue();
}

void EVH_connected() {
  //send actulal status
  makeWSMessage();
  webSocket.broadcastTXT(ButtonStateMsg, strlen(ButtonStateMsg));
}

// *******************************************************************
// ****************** F U N C T I O N S ******************************
// *******************************************************************

void add2Queue(int button) {
  for (int i = 0; i < (sizeof(queue) / sizeof(queue[0])); i++) {
    if (queue[i] == button) {
      Serial.print("Already in the queue. ButtonId: ");
      Serial.println(button);
      return;
    }
    if (queue[i] == 0) {
      queue[i] = button;
      Serial.print("Added to the queue. ButtonId: ");
      Serial.println(button);
      return;
    }
  }
  Serial.println("add2Queue some error");
}

void makeWSMessage() {
  String str = "";
  str += (queueSelector + 1);
  for (int i = 0; i < (sizeof(queue) / sizeof(queue[0])); i++) {
    str += ";";
    str += queue[i];
  }
  Serial.println(str);
  str.toCharArray(ButtonStateMsg, 20);
}


// ********************* Serial **********************

void printMAC(uint8_t *mac_addr) {
  char macStr[18];
  snprintf(macStr, sizeof(macStr), "%02x:%02x:%02x:%02x:%02x:%02x",
           mac_addr[0], mac_addr[1], mac_addr[2], mac_addr[3], mac_addr[4], mac_addr[5]);
  Serial.print(macStr);
}

// ********************* EEPROM **********************

void getMACFromEEPROM(uint8_t* mac, uint8_t offset) {
  for (int i = 0; i < 6; i++) {
    mac[i] = EEPROM.read(offset + i);
  }
}

void setMACToEEPROM(uint8_t *mac, uint8_t offset) {
  for (int i = 0; i < 6; i++) {
    EEPROM.write(offset + i, mac[i]);
  }  
}

// uint8_t getChannelFromEEPROM() {
//   return EEPROM.read(EEPROM_CHANNEL_OFFSET);
// }

// void setChannelToEEPROM(uint8_t channel) {
//   EEPROM.write(EEPROM_CHANNEL_OFFSET, channel);
// }

void commitEEPROM(){
  Serial.println("Writing MAC");
  EEPROM.commit();  // Нужен только на ESP
  delay(250);
}


// ********************* Helpers **********************

bool isMACEmpty(uint8_t *mac) {
  uint8_t cntFF = 0;
  uint8_t cnt00 = 0;
  for (int i = 0; i < 6; i++) {
    if (mac[i] == 255) cntFF++;
    if (mac[i] == 0) cnt00++;
  }
  return cntFF == 6 || cnt00 == 6;
}

uint8_t buttonId2Index(cButton btn) {
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
  return 0;
}

