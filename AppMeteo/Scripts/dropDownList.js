$(document).ready(function () {

    $('#ChoixStations').hide();
    $('#ChoixDates').hide();
    $('#BoutonSubmit').hide();

    $('#Pays').change(function () {
        var URL = "ListeStations";
        $.getJSON(URL + '/' + $('#Pays').val(), function (data) {
            var items = '<option>Sélectionnez une station</option>';
            $.each(data, function (i, mesure) {
                items += "<option value='" + mesure.Value + "'>" + mesure.Text + "</option>";
            });
            $('#Stations').html(items);
            $('#ChoixStations').show();
        });
    });
    if (typeof $('#Mesures').val() === 'undefined') {
        $('#ChoixStations').change(function () {
            $('#BoutonSubmit').show();
        });
    } else {

        $('#ChoixStations').change(function () {
            var URL = "ListeDates";
            $.getJSON(URL + '/' + $('#Stations').val(), function (data) {
                var items = '<option>Sélectionnez une date</option>';
                $.each(data, function (i, station) {
                    items += "<option value='" + station.Value + "'>" + station.Text + "</option>";
                });
                $('#Mesures').html(items);
                $('#ChoixDates').show();
            });
        });
        $('#Mesures').change(function () {
            $('#BoutonSubmit').show();
        });
    }
});