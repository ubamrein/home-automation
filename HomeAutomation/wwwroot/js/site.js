// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function serviceSelected(element) {
  var ele = $(element);
  $('.dropdown[data-service][data-player="' + ele.data('player') + '"]').hide();
  $('.dropdown[data-service="' + ele.data("service-action") + '"][data-player="' + ele.data("player") + '"]').show();
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
            info.html("<img width=\"200\" src=\"" + url + "\"/><p>"+ data.title + "</p>")
        });
    })
}