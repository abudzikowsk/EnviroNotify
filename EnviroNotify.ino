#include <Arduino.h>
#include <RTC.h>
#include <File.h>
#include <Flash.h>
#include "DFRobot_SHTC3.h"
#include "ESP8266.h"

#define SSID        "ASUS_46EB3P50X4"
#define PASSWORD    "AbyCa9Vr2!j.XxZ.7KDM"

DFRobot_SHTC3 SHTC3;

uint32_t id = 0;
float temperature,  humidity;

char homeServerEndpoint[] = "192.168.50.23";

File file;
ESP8266 wifi;

void setup() {
  pinMode(LED0, OUTPUT);
  pinMode(LED1, OUTPUT);
  pinMode(LED2, OUTPUT);
  pinMode(LED3, OUTPUT);

  digitalWrite(LED0, HIGH);
  digitalWrite(LED1, HIGH);
  digitalWrite(LED2, HIGH);
  digitalWrite(LED3, HIGH);

  digitalWrite(LED0, LOW);
  Serial.begin(115200);

  digitalWrite(LED1, LOW);
  initializeHumidityAndTemperatureSensor();
  initializeWifi();

  digitalWrite(LED2, LOW);
  setupRTCClock();
//  setupFlashDir();

  digitalWrite(LED3, LOW);
}

void initializeWifi(){
  wifi.begin(Serial2, 115200);
  wifi.setOprToStation()

  if(wifi.joinAP(SSID, PASSWORD)){
    Serial.println("Wifi initalized.");
    Serial.print("IP: ");
    Serial.println(wifi.getLocalIP().c_str());
  }
  else{
    Serial.println("Wifi initalization failed.");
  }
}


void initializeHumidityAndTemperatureSensor(){
  SHTC3.begin();
  SHTC3.wakeup();

  while((id = SHTC3.getDeviceID()) == 0){
    Serial.println("ID retrieval error. Please verify the correct connection of the device.");
    delay(1000);
  }
  delay(1000);  
}

void setupRTCClock(){
  RTC.begin();
  RtcTime compiledDateTime(__DATE__, __TIME__);
  RTC.setTime(compiledDateTime);
}

void setupFlashDir(){
  if(!Flash.exists("readings/")){
    Flash.mkdir("readings/");
  }
}

void loop(){
  SHTC3.wakeup();
  temperature = SHTC3.getTemperature(PRECISION_HIGH_CLKSTRETCH_ON);
  humidity = SHTC3.getHumidity(PRECISION_HIGH_CLKSTRETCH_OFF);

  if(temperature == MODE_ERR){
    Serial.println("Error reading temperature. Please check the sensor connection and try again.");
  } else{
    Serial.print("Temperature: "); Serial.print(temperature); Serial.println(" C");
  }

  if(humidity == MODE_ERR){
    Serial.println("Error reading humidity. Please check the sensor connection and try again.");
  } else{
    Serial.print("Humidity: "); Serial.print(humidity); Serial.println(" %RH");
  }

  SHTC3.sleep();
  delay(1000);

  // RtcTime currentTime = RTC.getTime();
  // String filePath = "readings/" +
  //                 String(currentTime.year()) + '-' +
  //                 String(currentTime.month()) + '-' +
  //                 String(currentTime.day()) + '-' +
  //                 String(currentTime.hour()) + ':' +
  //                 String(currentTime.minute()) + ':' +
  //                 String(currentTime.second()) + ".txt";
  // file = Flash.open(filePath, FILE_WRITE);
  
  // if(file) {
  //   file.print("T: ");
  //   file.println(temperature);
  //   file.printf("H: ");
  //   file.println(humidity);
  //   file.flush();

  //   file.close();
  // }

  // delay(1000 * 60 * 30);
}