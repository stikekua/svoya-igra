// *******************************************************************
// ********************* EVENT HANDLERS ******************************
// *******************************************************************

//event handler for next button or web cmd
void EVH_next() {
  Serial.println("next>>");

  if (queueSelector >= 3) {
    Serial.println("End of the queue");
    queueSelector = 3;
  } else if (queue[queueSelector] == 0) {
    Serial.println("Nobody else in the queue");
  } else if (queueSelector < 3 && queue[queueSelector] != 0) {
    //deselect current
    EN_sendMsg(CMD_DESELECT, static_cast<cButton>(queue[queueSelector]));
    //move queue
    queueSelector++;
    //select next in queue if someone
    if (queue[queueSelector] != 0){
      EN_sendMsg(CMD_SELECT, static_cast<cButton>(queue[queueSelector]));
      LED_setColor(static_cast<cButton>(queue[queueSelector]));
    }
    else{
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

  //reset selector
  queueSelector = 0;
  //clear queue
  for (int i = 0; i < (sizeof(queue) / sizeof(queue[0])); i++) {
    queue[i] = 0;
  }
  //release buttons
  EN_broadcastMsg(CMD_RELEASE);
  LED_Off();
  //send actulal status
  makeWSMessage();
  webSocket.broadcastTXT(ButtonStateMsg, strlen(ButtonStateMsg));
}

void EVH_buttonPressed(int button) {
  // add button to queue
  add2Queue(button);
  // added button is first in queue
  if (queue[queueSelector] == button) {
    EN_sendMsg(CMD_SELECT, static_cast<cButton>(queue[queueSelector]));
    LED_setColor(static_cast<cButton>(queue[queueSelector]));
  }
  //send actulal status
  makeWSMessage();
  webSocket.broadcastTXT(ButtonStateMsg, strlen(ButtonStateMsg));
}

void EVH_connected(){
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
