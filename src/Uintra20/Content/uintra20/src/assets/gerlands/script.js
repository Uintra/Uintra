class Balls {
  constructor(context, buffer) {
    this.context = context;
    this.buffer = buffer;
  }
  setup() {
    this.gainNode = this.context.createGain();
    this.source = this.context.createBufferSource();
    this.source.buffer = this.buffer;
    this.source.connect(this.gainNode);
    this.gainNode.connect(this.context.destination);
    this.gainNode.gain.setValueAtTime(1, this.context.currentTime);
  }
  play() {
    this.setup();
    this.source.start(this.context.currentTime);
  }
  stop() {
    var ct = this.context.currentTime + 1;
    this.gainNode.gain.exponentialRampToValueAtTime(.1, ct);
    this.source.stop(ct);
  }
}

class Buffer {
  constructor(context, urls) {
    this.context = context;
    this.urls = urls;
    this.buffer = [];
  }
  loadSound(url, index) {
    let request = new XMLHttpRequest();
    request.open('get', url, true);
    request.responseType = 'arraybuffer';
    let thisBuffer = this;
    request.onload = function() {
      thisBuffer.context
        .decodeAudioData(request.response, function(buffer) {
          thisBuffer.buffer[index] = buffer;
          if(index == thisBuffer.urls.length-1) {
            thisBuffer.loaded();
          }
        });
    };
    request.send();
  };
  getBuffer() {
    this.urls.forEach((url, index) => {
      this.loadSound(url, index);
    })
  }
  loaded() {
    loaded = true;
  }
  getSound(index) {
    return this.buffer[index];
  }
}

let balls = null,
    preset = 0,
    loaded = false;
let path = 'wwwroot/assets/gerlands/audio/';
let sounds = [
  path + 'sound1.mp3',
  path + 'sound2.mp3',
  path + 'sound3.mp3',
  path + 'sound4.mp3',
  path + 'sound5.mp3',
  path + 'sound6.mp3',
  path + 'sound7.mp3',
  path + 'sound8.mp3',
  path + 'sound9.mp3',
  path + 'sound10.mp3',
  path + 'sound11.mp3',
  path + 'sound12.mp3',
  path + 'sound13.mp3',
  path + 'sound14.mp3',
  path + 'sound15.mp3',
  path + 'sound16.mp3',
  path + 'sound17.mp3',
  path + 'sound18.mp3',
  path + 'sound19.mp3',
  path + 'sound20.mp3',
  path + 'sound21.mp3',
  path + 'sound22.mp3',
  path + 'sound23.mp3',
  path + 'sound24.mp3',
  path + 'sound25.mp3',
  path + 'sound26.mp3',
  path + 'sound27.mp3',
  path + 'sound28.mp3',
  path + 'sound29.mp3',
  path + 'sound30.mp3',
  path + 'sound31.mp3',
  path + 'sound32.mp3',
  path + 'sound33.mp3',
  path + 'sound34.mp3',
  path + 'sound35.mp3',
  path + 'sound36.mp3'
];
let context = new (window.AudioContext || window.webkitAudioContext)();

function playBalls() {
  let index = parseInt(this.dataset.note) + preset;
  balls = new Balls(context, buffer.getSound(index));
  balls.play();
}

function stopBalls() {
  balls.stop();
}

let buffer = new Buffer(context, sounds);
let ballsSound = buffer.getBuffer();
let buttons = document.querySelectorAll('.b-ball_bounce');
buttons.forEach(button => {
  button.addEventListener('mouseenter', playBalls.bind(button));
  button.addEventListener('mouseleave', stopBalls);
})

function ballBounce(e) {
  var i = e;
  if (e.className.indexOf(" bounce") > -1) {
  return;
  }
  toggleBounce(i);
}

function toggleBounce(i){
  i.classList.add("bounce");
  function n() {
    i.classList.remove("bounce")
    i.classList.add("bounce1");
    function o() {
      i.classList.remove("bounce1")
      i.classList.add("bounce2");
      function p() {
        i.classList.remove("bounce2")
        i.classList.add("bounce3");
        function q() {
          i.classList.remove("bounce3");
        }
        setTimeout(q, 300)
      }
      setTimeout(p, 300)
    }
    setTimeout(o, 300)
  }
  setTimeout(n, 300)
}

var array1 = document.querySelectorAll('.b-ball_bounce')
var array2 = document.querySelectorAll('.b-ball_bounce .b-ball__right')

for(var i=0; i<array1.length; i++){
  array1[i].addEventListener('mouseenter', function(){
    ballBounce(this)
  })
}

for(var i=0; i<array2.length; i++){
  array2[i].addEventListener('mouseenter', function(){
    ballBounce(this)
  })
}

let l = ["49", "50", "51", "52", "53", "54", "55", "56", "57", "48", "189", "187", "81", "87", "69", "82", "84", "89", "85", "73", "79", "80", "219", "221", "65", "83", "68", "70", "71", "72", "74", "75", "76", "186", "222", "220"];
let k = ["90", "88", "67", "86", "66", "78", "77", "188", "190", "191"];
let a = {};
for (let e = 0, c = l.length; e < c; e++) {
    a[l[e]] = e
}
for (let e = 0, c = k.length; e < c; e++) {
    a[k[e]] = e
}

document.addEventListener('keydown', function (j) {
  let i = j.target;
  if (j.which in a) {
    let index = parseInt(a[j.which]);
    balls = new Balls(context, buffer.getSound(index));
    balls.play();
    let ball = document.querySelector('[data-note="' + index + '"]');
    toggleBounce(ball);
  }
});
