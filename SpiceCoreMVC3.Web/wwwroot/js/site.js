// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var posts = $('.post');

(function ($)
{
    $('#menu-filters li').click(function () {
        $('#menu-filters li').removeClass('active btn btn-secondary');
        $(this).addClass('active btn btn-secondary');

        var selectedFilter = $(this).data('filter');

        $(".menu-restaurant").fadeOut();
        setTimeout(function () {
            $(selectedFilter).slideDown();
        }, 300);
    });

});
