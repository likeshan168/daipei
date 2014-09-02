$(document).ready(function () {
    $("#btn").click(function () {
        $.getJSON('/api/dpapi', function (data) {
            console.log(data);
        });
    });


    
});