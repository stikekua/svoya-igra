#include <Adafruit_NeoPixel.h>

#define LED_PIN    4
#define LED2_PIN   5
#define LED_COUNT 32
#define LED2_COUNT 4

Adafruit_NeoPixel mainStrip(LED_COUNT, LED_PIN, NEO_GRB + NEO_KHZ800);
Adafruit_NeoPixel stateStrip(LED2_COUNT, LED2_PIN, NEO_GRB + NEO_KHZ800);

long firstPixelHue = 0;
long ledCounter = 0;
int mode = 0;
long stateCounter = 0;

#define PERIOD1 500
#define PERIOD2 1000
#define PERIOD3 100
unsigned long currentMillis;
unsigned long timer1Millis;
unsigned long timer2Millis;
unsigned long timer3Millis;

#include "GyverButton.h"
#define BTN_PIN 12
GButton btn(BTN_PIN, LOW_PULL, NORM_OPEN);

int batteryValue;

void setup() {
  Serial.begin(115200);
  delay(10);

  mainStrip.begin();           // INITIALIZE NeoPixel strip object (REQUIRED)
  mainStrip.show();            // Turn OFF all pixels ASAP
  mainStrip.setBrightness(50); // Set BRIGHTNESS to about 1/5 (max = 255)

  stateStrip.begin();           // INITIALIZE NeoPixel strip object (REQUIRED)
  stateStrip.show();            // Turn OFF all pixels ASAP
  stateStrip.setBrightness(50); // Set BRIGHTNESS to about 1/5 (max = 255)

}

void loop() {

  currentMillis = millis();
  if (currentMillis - timer1Millis >= PERIOD1)
  {
    //
    if(mode == 0){
      colorWipe(mainStrip.Color(0, 150, 0));
    }

    timer1Millis = currentMillis;
  }

  currentMillis = millis();
  if (currentMillis - timer2Millis >= PERIOD2)
  {
    //ADC
    batteryValue = analogRead(A0); // 0..1023
    Serial.print("batteryValue = ");
    Serial.println(batteryValue);

    timer2Millis = currentMillis;
  }

  currentMillis = millis();
  if (currentMillis - timer3Millis >= PERIOD3)
  {
    //
    if(mode == 1){
      rainbow();
    }

    timer3Millis = currentMillis;
  }

  btn.tick();
  if (btn.isClick()) {
    Serial.println("Click");

    //main strip mode
    mode++;
    if(mode > 1) mode = 0;
    Serial.print("mode = ");
    Serial.println(mode);
    firstPixelHue = 0;
    ledCounter = 0;
    mainStrip.clear();
    mainStrip.show();

    //state strip
    stateCounter++;
    if(stateCounter >= LED2_COUNT) {
      stateStrip.clear();
      stateCounter = 0;
    }
    stateStrip.setPixelColor(stateCounter, stateStrip.Color(150, 150, 150));
    stateStrip.show();

  }

  if (btn.isSingle()) Serial.println("Single");       // проверка на один клик
  if (btn.isDouble()) Serial.println("Double");       // проверка на двойной клик
  if (btn.isTriple()) Serial.println("Triple");       // проверка на тройной клик

  if (btn.isPress()) Serial.println("Press");         // нажатие на кнопку (+ дебаунс)
  if (btn.isRelease()) Serial.println("Release");     // отпускание кнопки (+ дебаунс)
  if (btn.isHolded()) Serial.println("Holded");       // проверка на удержание

}


void colorWipe(uint32_t color) {
  ledCounter++;
  if(ledCounter >= mainStrip.numPixels()) {
    mainStrip.clear();
    ledCounter = 0;
  }

  mainStrip.setPixelColor(ledCounter, color);
  mainStrip.show(); 
}

void rainbow() {
  firstPixelHue += 256;
  if(firstPixelHue >= 5*65536) firstPixelHue = 0;
  mainStrip.rainbow(firstPixelHue);
  mainStrip.show();
}
