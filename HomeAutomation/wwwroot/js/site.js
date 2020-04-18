// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var inputPressed = false;

function callApi(baseurl, controlleraction, arguments = []) {
  var argumentstring = '';
  for (var i = 0; i < arguments.length; i++) {
    argumentstring += '&arguments=' + encodeURIComponent(arguments[i]);
  }
  fetch(
      '/api/action?baseurl=' + encodeURIComponent(baseurl) +
      '&controlleraction=' + encodeURIComponent(controlleraction) +
      argumentstring)
      .then(function(resp) {
        if (resp.ok) {
          console.log('Volume set to: ' + ele.val());
        } else {
          throw new Error('Could not set volume:' + resp);
        }
      });
}

function serviceSelected(element) {
  var ele = $(element);
  $('.dropdown[data-service][data-player="' + ele.data('player') + '"]').hide();
  $('.dropdown[data-service="' + ele.data("service-action") + '"][data-player="' + ele.data("player") + '"]').show();
}

function radioSelected(element) {
  var ele = $(element).eq(0);
  callApi(ele.data('base-url'), ele.data('action'), [ele.data("service-action"),ele.data("url")]);
}

function buttonPress(element) {
  var ele = $(element).eq(0);
  callApi(ele.data('base-url'), ele.data('action'));
}

function setVolume(element) {
  var ele = $(element).eq(0);
  callApi(ele.data('base-url'), ele.data('action'), [ele.val(), 0])
}

$('body').ready(function() {
  $('.dropdown[data-service]').hide();  
  var players = $(".player");
  for (var player of players) {
      console.log(player)
    setInterval(getStatus, 1000, $(player),$(player).data("base-url"), $(player).data("action"));
  }
});

function getStatus(player, baseUrl, action) {
    fetch("/api/status?baseurl="+encodeURIComponent(baseUrl)+"&action="+encodeURIComponent(action)).then(function(resp) {
        resp.json().then(function(data){
            var name = player.data("player");
            var info = $('.display[data-player="'+ name+'"]').eq(0);
            var url = data.image;
            if (!url.startsWith("http")) {
                url = baseUrl + url;
            }
            info.html("<img width=\"200\" src=\"" + url + "\"/><p>"+ data.title.replace("\n", "<br />") + "</p>")
            if(data.state == "pause" || data.state == "stop") {
                $('.play[data-player="'+ name + '"]').eq(0).show();
                $('.pause[data-player="'+ name + '"]').eq(0).hide();
            } else {
                $('.play[data-player="'+ name + '"]').eq(0).hide();
                $('.pause[data-player="'+ name + '"]').eq(0).show();
            }
            if(!inputPressed) {
                $('input[data-player="'+ name+'"').eq(0).val(data.volume)
            }
            $('progress[data-player="'+ name+'"').eq(0).attr("max", data.totalLength);
            $('progress[data-player="'+ name+'"').eq(0).attr("value", data.position);
        });
    })
}

function mousedown(element) {
    inputPressed = true;
}
function mouseup(element) {
    inputPressed = false
} 