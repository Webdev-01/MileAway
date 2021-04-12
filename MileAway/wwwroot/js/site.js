// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.

var url = new URL(window.location.href);
if (url.searchParams.get("Year")) {
    setTimeout(function () {
        $("#collapseGraphs").collapse('toggle');
        switchCaret();
        setTimeout(function () {
            document.getElementById('accordionContent').scrollIntoView({
                behavior: "smooth"
            });
        }, 400);
    }, 500);
}

function switchCaret() {
    var element = document.getElementById("accordionIcon");
    if (document.querySelector(".accordion .btn").classList.contains('collapsed')) {
        element.classList.remove('fa-caret-down');
        element.classList.add('fa-caret-up');
        setTimeout(function () {
            document.getElementById('accordionContent').scrollIntoView({
                behavior: "smooth"
            });
        }, 400);
    } else {
        element.classList.remove('fa-caret-up');
        element.classList.add('fa-caret-down');
    }
}