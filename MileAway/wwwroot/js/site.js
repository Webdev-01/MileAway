// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.

function switchCaret() {
    var element = document.getElementById("accordionIcon");
    if (document.querySelector(".accordion .btn").classList.contains('collapsed')) {
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

function tooltip () {
    $('[data-toggle="tooltip"]').tooltip()
}