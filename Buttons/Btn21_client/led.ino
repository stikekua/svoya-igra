// *******************************************************************
// ********************* L E D  H A N D L E R S **********************
// *******************************************************************

void exec_Led(LED ledmode) {
  static bool LEDStatus;

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
  }
}
