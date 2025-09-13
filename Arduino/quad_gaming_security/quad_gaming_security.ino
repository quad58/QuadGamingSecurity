#include <SoftwareSerial.h>
#include <NewPing.h>

// Settings
#define BLUETOOTH_TXD_PIN       8
#define BLUETOOTH_RXD_PIN       9
#define SONAR_TRIGGER_PIN       10
#define SONAR_ECHO_PIN          11
#define SONAR_MAX_DISTANCE      200
#define SONAR_TRIGGER_DISTANCE  50

NewPing sonar(SONAR_TRIGGER_PIN, SONAR_ECHO_PIN, SONAR_MAX_DISTANCE);
SoftwareSerial BTserial(BLUETOOTH_TXD_PIN, BLUETOOTH_RXD_PIN);

int last_distance = 0;
int last_registered_distance = 0;
int current_distance = 0;

void setup() {
  BTserial.begin(9600);
}

void loop() {
  current_distance = sonar.ping_cm();
  if (last_distance != current_distance)
  {
    if (abs(last_registered_distance - current_distance) > 1)
    {
      last_registered_distance = current_distance;
      if (current_distance < SONAR_TRIGGER_DISTANCE)
      {
        BTserial.println("0");
      }
    }
  }
  last_distance = current_distance;
}
