void ledInit() {
  strip.begin();           // INITIALIZE NeoPixel strip object (REQUIRED)
  strip.show();            // Turn OFF all pixels ASAP
  strip.setBrightness(50); // Set BRIGHTNESS to about 1/5 (max = 255)
}

void LED_On(){
  strip.clear(); // Set all pixel colors to 'off'

  // The first NeoPixel in a strand is #0, second is 1, all the way up
  // to the count of pixels minus one.
  for(int i=0; i<LED_COUNT; i++) { // For each pixel...

    // pixels.Color() takes RGB values, from 0,0,0 up to 255,255,255
    // Here we're using a moderately bright green color:
    strip.setPixelColor(i, strip.Color(0, 150, 0));

    strip.show();   // Send the updated pixel colors to the hardware.
  }
}

void LED_Off(){
   strip.clear(); // Set all pixel colors to 'off'
   strip.show();
}

void LED_setColor(cButton btn){
  strip.clear();
  strip.fill(findColor(btn), 0, LED_COUNT);
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
