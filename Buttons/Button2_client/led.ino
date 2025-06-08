// *******************************************************************
// ********************* L E D  H A N D L E R S **********************
// *******************************************************************

void exec_Led(LED ledmode) {
  static bool LEDStatus;
  static bool LEDBuildin;  

  switch (ledmode) {
    case OFF  : {
        LEDStatus = false;
        digitalWrite(LED_PIN, LEDStatus);
        break;
      }
    case ON   : {
        LEDStatus = true;
        digitalWrite(LED_PIN, LEDStatus);
        break;
      }
    case BLINK : {
        if (blinkTimer.ready()) {
          LEDStatus = !LEDStatus;
          digitalWrite(LED_PIN, LEDStatus);
        }
        break;
      }
    case BLINK_ERROR : {
        if (blink2Timer.ready()) {
          LEDStatus = !LEDStatus;
          digitalWrite(LED_PIN, LEDStatus);
        }
        break;
      }
    case BLINK_PAIRING : {
        if (blinkPairingTimer.ready()) {
          LEDBuildin = !LEDBuildin;
          digitalWrite(BUILTIN_LED, LEDBuildin);
        }
        break;
      }
  }
}

void led_off(){
  digitalWrite(BUILTIN_LED, true);
  digitalWrite(LED_PIN, false);
}
