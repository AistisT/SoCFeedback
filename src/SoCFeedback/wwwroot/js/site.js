$(document).ready(function () {
    $('#YearTable').DataTable({
        "order": [[0, "desc"]]
    });
    $(".table").DataTable();
    $(".autosize").textareaAutoSize();
});