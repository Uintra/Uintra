export default function () {
    $('.mention').each(function (i, e) {
        $(this).on('click', function (e) {
            var id = $(this).data('id');
            location.href = '/profile?id=' + id;
            e.preventDefault();
        })
    });
}