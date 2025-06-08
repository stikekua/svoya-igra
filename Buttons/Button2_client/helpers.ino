
// ********************* Serial **********************

void printMAC(uint8_t *mac_addr) {
  char macStr[18];
  snprintf(macStr, sizeof(macStr), "%02x:%02x:%02x:%02x:%02x:%02x",
           mac_addr[0], mac_addr[1], mac_addr[2], mac_addr[3], mac_addr[4], mac_addr[5]);
  Serial.print(macStr);
}

// ********************* EEPROM **********************

void getMACFromEEPROM(uint8_t* mac) {
  for (int i = 0; i < 6; i++) {
    mac[i] = EEPROM.read(EEPROM_MAC_OFFSET + i);
  }
}

void setMACToEEPROM(uint8_t *mac) {
  for (int i = 0; i < 6; i++) {
    EEPROM.write(EEPROM_MAC_OFFSET + i, mac[i]);
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
  uint8_t cnt = 0;
  for (int i = 0; i < 6; i++) {
    if (mac[i] == 255) cnt++;
  }
  return cnt == 6;
}