
uint32_t warmColor = strip.Color(252, 113, 25);

void ledInit() {
  strip.begin();           // INITIALIZE NeoPixel strip object (REQUIRED)
  strip.show();            // Turn OFF all pixels ASAP
  strip.setBrightness(50); // Set BRIGHTNESS to about 1/5 (max = 255)
}

void LED_On(){
  strip.clear();
  strip.fill(warmColor, 0, LED_COUNT);
  strip.show();
}

void LED_Off(){
   strip.clear(); // Set all pixel colors to 'off'
   strip.show();  //load to hw
   LED_showQueue();
}

void LED_setColor(cButton btn){
  strip.clear();
  strip.fill(findColor(btn), 5, LED_COUNT-5);
  strip.show();
  LED_showQueue();
}

void LED_showQueue(){
  for(int i=0; i<4; i++) {
    if (queue[i] != 0){
      strip.setPixelColor(i, findColor(static_cast<cButton>( queue[i] )) );
    } else {
      strip.setPixelColor(i, 0);
    }
  }
  strip.setPixelColor(4, 0); //led separator
  strip.show();
}

uint32_t findColor(cButton btn){
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
