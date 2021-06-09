#define LED_PIN 13
#define AIR_VALVE 9
#define AIR_PUMP 10
#define TEMP 11

bool flag, flag2;
int num, itr;
long data[50];

void setup()
{
  pinMode(LED_PIN, OUTPUT);
  pinMode(AIR_VALVE, OUTPUT);
  pinMode(AIR_PUMP, OUTPUT);
  pinMode(TEMP, OUTPUT);
  Serial.begin(9600);
  digitalWrite(LED_PIN, LOW);
  digitalWrite(AIR_PUMP, LOW);
  digitalWrite(AIR_VALVE, HIGH);
  //digitalWrite(AIR_VALVE, LOW);
  digitalWrite(TEMP, LOW);
  flag = false;
  flag2 = false;
  num = 0;
  itr = 0;
}

void loop()
{
  /*
  if(flag2){
    digitalWrite(AIR_PUMP, HIGH);
    digitalWrite(AIR_VALVE, LOW);
    delay(4000);
    digitalWrite(AIR_PUMP, LOW);
    flag2 = false;
  }
  */
  if(flag){
    if(itr%2 == 0){
        digitalWrite(AIR_PUMP, HIGH);
        digitalWrite(AIR_VALVE, LOW);
        digitalWrite(LED_PIN, HIGH);
        //digitalWrite(AIR_VALVE, HIGH);
      }
      else{
        digitalWrite(AIR_PUMP, LOW);
        digitalWrite(AIR_VALVE, HIGH);
        digitalWrite(LED_PIN, LOW);
        //digitalWrite(AIR_VALVE, LOW);
      }
      delay(data[itr] * 3);
      itr = (itr + 1) % (int)(max(1, num));
  }

  //if(data[0] == (long)157 && data[1] == (long)314 && data[2] == (long)157) digitalWrite(LED_PIN, HIGH);
  
  int inputchar;
  inputchar = Serial.read();
  if(inputchar != -1)
  {
    switch(inputchar)
    {
      case 'o':
        digitalWrite(LED_PIN, HIGH);
        digitalWrite(TEMP, HIGH);
        break;
      case 'p':
        digitalWrite(LED_PIN, LOW);
        digitalWrite(TEMP, LOW);
        break;
      case 'a':
        digitalWrite(AIR_PUMP, HIGH);
        digitalWrite(AIR_VALVE, LOW);
        //digitalWrite(AIR_VALVE, HIGH);
        flag = true;
        break;
      case 'b':
        digitalWrite(AIR_PUMP, LOW);
        digitalWrite(AIR_VALVE, HIGH);
        //digitalWrite(AIR_VALVE, LOW);
        flag = false;
        itr = 0;
        break;
      case 'd':
        long val = 0;
        while(true){
          int inputCommand;
          inputCommand = Serial.read();
          if(inputCommand != -1){
            if(inputCommand == 'e'){
              flag2 = true;
              break;
            }
            if(inputCommand == 'c'){
              data[num++] = val;
              val = 0;
            }
            if('0' <= inputCommand && inputCommand <= '9'){
              byte buf = inputCommand - '0';
              val = val * 10 + buf;
            }
          }
        }
        break;
      case 'r':
        for(int i = num; i >= 0; --i){
          data[i] = 0;
        }
        num = 0;
        break;
    }
  }
  
}
