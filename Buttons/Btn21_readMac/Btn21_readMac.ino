#include <ESP8266WiFi.h>
 
void setup(){
  Serial.begin(115200);
  WiFi.mode(WIFI_STA);
  
  Serial.println();
  Serial.println();
  Serial.print("MAC address:");
  Serial.println(WiFi.macAddress());
}
 
void loop(){

}

// master 8c:aa:b5:63:29:42
// green  e8:db:84:9a:8c:a1
