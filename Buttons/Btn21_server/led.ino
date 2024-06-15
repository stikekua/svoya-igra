
uint32_t warmColor = mainStrip.Color(252, 113, 25);
uint32_t miniColor = mainStrip.Color(63, 63, 63);

void ledInit() {
  mainStrip.begin();
  mainStrip.clear();
  mainStrip.show();
  mainStrip.setBrightness(50);

  stateStrip.begin();
  stateStrip.clear();
  stateStrip.show();
  stateStrip.setBrightness(50);
}

void LED_Off() {
  mainStrip.clear();
  mainStrip.show();
  LED_showQueue();
}

void LED_setColor(cButton btn) {
  mainStrip.clear();
  mainStrip.fill(findColor(btn), 0, LED_COUNT);
  mainStrip.show();
  LED_showQueue();
  LED_showTimer();
}

void LED_showQueue() {
  for (int i = 0; i < 4; i++) {
    if (queue[i] != 0) {
      uint32_t color = findColor(static_cast<cButton>( queue[i] ));
      stateStrip.setPixelColor((4 - i - 1), color);
    } else {
      if (started) {
        stateStrip.setPixelColor((4 - i - 1), miniColor);
      } else {
        stateStrip.setPixelColor((4 - i - 1), 0);
      }
    }
  }
  stateStrip.show();
}

void LED_showTimer() {
  if (duration < ROUND_TIME - 1) {
    uint8_t led_cnt = map(duration, 0, (ROUND_TIME - 1), LED_COUNT, 1);

    mainStrip.fill(0, 0, LED_COUNT); //off dur

    if (queue[queueSelector] > 0 ) {
      uint32_t color = findColor(static_cast<cButton>( queue[queueSelector] ));
      mainStrip.fill(color, 0, led_cnt);
    }
    else {
      mainStrip.fill(warmColor, 0, led_cnt);
    }
  }
  else if (duration > ROUND_TIME + 6 )
  {
    mainStrip.fill(0, 0, LED_COUNT); //off dur
  }
  else if (duration >= ROUND_TIME && ((duration % 2) == 0))
  {
    mainStrip.fill(0, 0, LED_COUNT); //off dur
  }
  else if (duration >= ROUND_TIME && ((duration % 2) == 1))
  {
    if (queue[queueSelector] > 0 ) {
      uint32_t color = findColor(static_cast<cButton>( queue[queueSelector] ));
      mainStrip.fill(color, 0, LED_COUNT);
    }
    else {
      mainStrip.fill(warmColor, 0, LED_COUNT);
    }
  }

  mainStrip.show();
}

uint32_t findColor(cButton btn) {
  switch (btn) {
    case RED:
      return buttonColors[0];
    case GREEN:
      return buttonColors[1];
    case BLUE:
      return buttonColors[2];
    case YELLOW:
      return buttonColors[3];
  }
}
