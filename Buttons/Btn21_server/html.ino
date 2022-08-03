extern const char INDEX_HTML[];

void htmlInit() {
  server.on("/", handleRoot);
  server.onNotFound(handleNotFound);

  server.begin();
}

void handleRoot()
{
  server.send_P(200, "text/html", INDEX_HTML);
}

void handleNotFound()
{
  String message = "File Not Found\n\n";
  message += "URI: ";
  message += server.uri();
  message += "\nMethod: ";
  message += (server.method() == HTTP_GET) ? "GET" : "POST";
  message += "\nArguments: ";
  message += server.args();
  message += "\n";
  for (uint8_t i = 0; i < server.args(); i++) {
    message += " " + server.argName(i) + ": " + server.arg(i) + "\n";
  }
  server.send(404, "text/plain", message);
}

const char PROGMEM INDEX_HTML[] = R"rawliteral(
<!DOCTYPE html>
<html>
<head>
<meta name = "viewport" content = "width = device-width, initial-scale = 1.0, maximum-scale = 1.0, user-scalable=0">
<title>Buttons</title>
<style>
body { background-color: #CDE0F2; font-family: Arial, Helvetica, Sans-Serif; Color: #000000; }
#Led { font-size: 30px; }
button { margin: 2px; padding: 20px 40px;}
.container{
  display: flex;
  flex-wrap: wrap;
  justify-content: center;
  align-items: center;
}
.container2{
  display: flex;
  flex-direction: column;
  align-items: center;
}
.myButton {
    display: flex;
    justify-content: center;
    align-items: center;
    width:100px;
    height:50px;
    margin: 10px;    
    border-radius: 5px;
    border: 1px solid black;
}
.red { background-color: red; color: white }
.green { background-color: green; color: white }
.blue { background-color: blue; color: white }
.yellow { background-color: yellow; color: black }
.selected {border: 10px solid gold;}
.ready {border: 5px solid gray;}
</style>
<script>
var websock;
function start() {
  websock = new WebSocket('ws://' + window.location.hostname + ':81/');
  websock.onopen = function(evt) { console.log('websock open'); };
  websock.onclose = function(evt) { console.log('websock close'); };
  websock.onerror = function(evt) { console.log(evt); };
  websock.onmessage = function(evt) {
    console.log(evt);
    var ledR = document.getElementById('Led');
    const myArray = evt.data.split(";");
    if(myArray.length === 5){
      let btnId = myArray[myArray[0]];
      
      let x = document.getElementsByClassName("selected");
      for(var i=0;i<x.length;i++){
        x[i].classList.remove("selected");
      }
      let y = document.getElementsByClassName("ready");
      for(var i=0;i<y.length;i++){
        y[i].classList.remove("ready");
      }
      
      //hightlight selected player
      if(btnId == 0) ledR.style.color = 'black';
      if(btnId == 1) {
        ledR.style.color = 'red';
        let r = document.getElementsByClassName("red");        
        r[0].classList.remove("ready");
        r[0].classList.add("selected");
      }
      if(btnId == 2) {
        ledR.style.color = 'green';
        let r = document.getElementsByClassName("green");        
        r[0].classList.remove("ready");
        r[0].classList.add("selected");
      }
      if(btnId == 4) {
        ledR.style.color = 'blue';
        let r = document.getElementsByClassName("blue");        
        r[0].classList.remove("ready");
        r[0].classList.add("selected");
      }
      if(btnId == 8) {
        ledR.style.color = 'yellow';
        let r = document.getElementsByClassName("yellow");        
        r[0].classList.remove("ready");
        r[0].classList.add("selected");
      }     
      
      //hightlight next players
      let next = Number(myArray[0]) + 1;
      while (next <= 4){
        if(myArray[next] != 0){
          if(myArray[next] == 1) document.getElementsByClassName("red")[0].classList.add("ready");
          if(myArray[next] == 2) document.getElementsByClassName("green")[0].classList.add("ready");
          if(myArray[next] == 4) document.getElementsByClassName("blue")[0].classList.add("ready");
          if(myArray[next] == 8) document.getElementsByClassName("yellow")[0].classList.add("ready");
        }
        next++;
      }

      
    }
    else {
      console.log('unknown event');
    }
  };
}
function buttonclick(e) {
  websock.send(e.id);
}
</script>
</head>
<body onload="javascript:start();">
  <div id="Led"><b>LED</b></div>  
  <div class="container">
    <div class="myButton red">RED</div>
    <div class="myButton green">GREEN</div>
    <div class="myButton blue">BLUE</div>
    <div class="myButton yellow">YELLOW</div>
  </div>  
  <div class="container2">
    <p>
      <button id="SGNext" type="button" onclick="buttonclick(this);">NEXT</button>
      Select next player in the queue
    </p>
    <p>
      <button id="SGReset" type="button" onclick="buttonclick(this);">RESET</button> 
      Inicialize new cycle
    </p>
  </div>
</body>
</html>
)rawliteral";
