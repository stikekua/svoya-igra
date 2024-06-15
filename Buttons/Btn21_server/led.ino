
uint32_t warmColor = strip.Color(252, 113, 25);
uint8_t LedBtn_cnt = 11;
uint8_t LedBtn_offset = 5;
uint8_t LedDur_cnt = 16;
uint8_t LedDur_offset = 16;

void ledInit() {
  strip.begin();           // INITIALIZE NeoPixel strip object (REQUIRED)
  strip.clear();
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
   if(started){
    LED_showQueue();    
    LED_showTimer(duration);
   }
}

void LED_setColor(cButton btn){
  strip.clear();
  strip.fill(findColor(btn), LedBtn_offset, LedBtn_cnt);
  strip.show();
  LED_showQueue();  
  LED_showTimer(duration);
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

void LED_showTimer(uint8_t duration){
  strip.fill(0, LedDur_offset, LedDur_cnt); //off dur
  
  strip.fill(warmColor, LedDur_offset, map(duration,0,15,16,1));
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
