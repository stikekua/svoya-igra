//Button2 hw test

#define LED_PIN    4
#define MAINBUTTON_PIN    13
#define PAIRBTN_PIN    5

bool led = false;
bool led2 = false;
int mode = 0;

#define PERIOD1 500
#define PERIOD2 1000
#define PERIOD3 1500
unsigned long currentMillis;
unsigned long timer1Millis;
unsigned long timer2Millis;
unsigned long timer3Millis;

#include "GyverButton.h"
GButton mainBtn(MAINBUTTON_PIN, HIGH_PULL, NORM_OPEN); // с привязкой к пину и указанием типа подключения (HIGH_PULL / LOW_PULL) и типа кнопки (NORM_OPEN / NORM_CLOSE)
GButton pairBtn(PAIRBTN_PIN, HIGH_PULL, NORM_OPEN); // с привязкой к пину и указанием типа подключения (HIGH_PULL / LOW_PULL) и типа кнопки (NORM_OPEN / NORM_CLOSE)

int batteryValue;

void setup() {
  Serial.begin(115200);
  delay(10);

  pinMode(LED_BUILTIN, OUTPUT);
  pinMode(LED_PIN, OUTPUT);
}

void loop() {

  currentMillis = millis();
  if (currentMillis - timer1Millis >= PERIOD1)
  {
    //
    if(mode == 1){
      led = !led;
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
    if(mode == 2){
      led2 = !led2;
    }

    timer3Millis = currentMillis;
  }

  digitalWrite(LED_BUILTIN, led);
  digitalWrite(LED_PIN, led2);

  mainBtn.tick();
  pairBtn.tick();
  if (mainBtn.isClick()) {
    Serial.println("mainBtn Click");

    //main strip mode
    mode++;
    if(mode > 2) mode = 0;
    Serial.print("mode = ");
    Serial.println(mode);

  }

  if (pairBtn.isClick()) {
    mode = 2;
  }


  if (mainBtn.isSingle()) Serial.println("Single");       // проверка на один клик
  if (mainBtn.isDouble()) Serial.println("Double");       // проверка на двойной клик
  if (mainBtn.isTriple()) Serial.println("Triple");       // проверка на тройной клик

  if (mainBtn.isPress()) Serial.println("Press");         // нажатие на кнопку (+ дебаунс)
  if (mainBtn.isRelease()) Serial.println("Release");     // отпускание кнопки (+ дебаунс)
  if (mainBtn.isHolded()) Serial.println("Holded");       // проверка на удержание

}
