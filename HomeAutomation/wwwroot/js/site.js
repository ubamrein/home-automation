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
});