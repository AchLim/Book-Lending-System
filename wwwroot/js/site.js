// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    $('#account_datatable_responsive').DataTable({
        paging: true,
        ordering: true,
        info: true,
        responsive: true,
        order: [[0, 'desc']],
    });


    $('#user_datatable_responsive').DataTable({
        paging: true,
        ordering: true,
        info: true,
        responsive: true,
        order: [[0, 'desc']],
    });

    $('#student_datatable_responsive').DataTable({
        paging: true,
        ordering: true,
        info: true,
        responsive: true,
        order: [[0, 'desc']],
    });

    $('#book_datatable_responsive').DataTable({
        paging: true,
        ordering: true,
        info: true,
        responsive: true,
        order: [[0, 'desc']],
    });

    $('#role_datatable_responsive').DataTable({
        paging: true,
        ordering: true,
        info: true,
        responsive: true,
        order: [[0, 'desc']],
    });


    $('.carousel').slick({
        dots: false,
        infinite: false,
        speed: 300,
        slidesToShow: 6,
        slidesToScroll: 6,
        responsive: [
            {
                breakpoint: 1200,
                settings: {
                    slidesToShow: 5,
                    slidesToScroll: 5,
                    infinite: false,
                    dots: false
                }
            },
            {
                breakpoint: 1024,
                settings: {
                    slidesToShow: 4,
                    slidesToScroll: 4,
                    infinite: false,
                    dots: false
                }
            },
            {
                breakpoint: 600,
                settings: {
                    slidesToShow: 3,
                    slidesToScroll: 3,
                    infinite: false,
                    dots: false
                }
            },
            {
                breakpoint: 480,
                settings: {
                    slidesToShow: 2,
                    slidesToScroll: 2,
                    infinite: false,
                    dots: false
                }
            },
            {
                breakpoint: 360,
                settings: {
                    slidesToShow: 1,
                    slidesToScroll: 1,
                    infinite: false,
                    dots: false
                }
            }
        ]
    })
});

