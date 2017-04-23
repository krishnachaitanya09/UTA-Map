var map = L.map('map', {
    center: [32.730531, -97.114570],
    zoom: 16
});

var layers = [];
var colorDictionary = {};
colorDictionary["Academic Building"] = "#F44336";
colorDictionary["Administrative Building"] = "#9C27B0";
colorDictionary["Off Campus Apartments"] = "#673AB7";
colorDictionary["On Campus Apartments"] = "#3F51B5";
colorDictionary["Playground"] = "#009688";
colorDictionary["Residence Hall"] = "#2196F3";


var pois = [];

function loadPOIs() {
    $.ajax({
        url: "/api/POI/GetAllPOIs",
        method: "get",
        contentType: 'application/json',
        dataType: 'json',

        success: function (response) {
            for (var i = 0; i < response.length; i++) {
                pois.push(response[i]);
            }

            var substringMatcher = function (strs) {
                return function findMatches(q, cb) {
                    var matches, substringRegex;

                    // an array that will be populated with substring matches
                    matches = [];

                    // regex used to determine if a string contains the substring `q`
                    substrRegex = new RegExp(q, 'i');

                    // iterate through the pool of strings and for any string that
                    // contains the substring `q`, add it to the `matches` array
                    $.each(strs, function (i, str) {
                        if (substrRegex.test(str)) {
                            matches.push(str);
                        }
                    });

                    cb(matches);
                };
            };

            $('#search').typeahead({
                hint: true,
                highlight: true,
                minLength: 1
            },
                {
                    name: 'pois',
                    source: substringMatcher(pois)
                }
            );
        }
    });
}


$(document).ready(function () {

    $('#mobile-nav').click(function (event) {
        $('nav').toggleClass('active');
    });


    L.tileLayer('http://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: 'UTA Map',
        maxZoom: 18,
    }).addTo(map);

    loadPOIs();
});

function searchPOI() {

    var data = {
        "AcademicBuildings": $("#academicBuildingsCheckbox").prop("checked"),
        "AdministrativeBuildings": $("#administrativeBuildingsCheckbox").prop("checked"),
        "OffCampus": $("#offCampusCheckbox").prop("checked"),
        "OnCampus": $("#onCampusCheckbox").prop("checked"),
        "Playgrounds": $("#playgroundsCheckbox").prop("checked"),
        "ResidenceHalls": $("#residenceHallsCheckbox").prop("checked"),
        "SearchText": $("#search").prop("value"),
        "Radius": $("#radius").prop("value")
    };

    if ((data["AcademicBuildings"] || data["AdministrativeBuildings"] || data["OffCampus"] || data["OnCampus"]
        || data["Playgrounds"] || data["ResidenceHalls"] || data["Radius"] != "") && data["SearchText"] != "") {
        getNearestPOI(data);
    }
    else {
        getPOI(data);
    }
}

function getNearestPOI(data) {
    $.ajax({
        url: "/api/POI/GetNearestPOI",
        method: "post",
        contentType: 'application/json',
        dataType: 'json',
        data: JSON.stringify(data),

        success: function (response) {
            clearAllLayers();
            var allBounds = [];
            for (var i = 0; i < response.length; i++) {
                var name = response[i].name;

                var coordinates = JSON.parse(response[i].geom);
                for (var j = 0; j < coordinates.coordinates.length; j++) {

                    var bounds = [];
                    for (var k = 0; k < coordinates.coordinates[j][0].length; k++) {
                        var r = coordinates.coordinates[j][0][k];
                        var c = [];
                        c.push(r[1]);
                        c.push(r[0]);
                        bounds.push(c);
                        allBounds.push(c)
                    }

                    var layer = L.polygon(bounds, { color: colorDictionary[response[i].category], weight: 2 }).addTo(map).bindPopup(name);
                    layers.push(layer);
                    if (i == 0) {
                        if (j == 0) {
                            var circle = L.circleMarker(layer.getBounds().getCenter(), { radius: 12, weight: 4, fillOpacity: 1.0, color: 'Black', fillColor: 'White' }).addTo(map);
                            layers.push(circle);
                            if (data["Radius"] != "") {
                                var radiusCircle = L.circle(layer.getBounds().getCenter(), { color: 'red', fillColor: '#f03', opacity: 0.5,  fillOpacity: 0.1, radius: data["Radius"] * 1609.344 }).addTo(map);
                                layers.push(radiusCircle);
                            }
                        }
                    }
                    else {
                        var marker = L.marker(layer.getBounds().getCenter()).addTo(map).bindPopup(name);
                        layers.push(marker);
                    }
                }
            }
            map.fitBounds(allBounds);
        }
    });
}

function getPOI(data) {
    $.ajax({
        url: "/api/POI/GetPOIs",
        method: "post",
        contentType: 'application/json',
        dataType: 'json',
        data: JSON.stringify(data),

        success: function (response) {
            clearAllLayers();
            for (var i = 0; i < response.length; i++) {
                var name = response[i].name;

                var coordinates = JSON.parse(response[i].geom);
                for (var j = 0; j < coordinates.coordinates.length; j++) {

                    var bounds = [];
                    for (var k = 0; k < coordinates.coordinates[j][0].length; k++) {
                        var r = coordinates.coordinates[j][0][k];
                        var c = [];
                        c.push(r[1]);
                        c.push(r[0]);
                        bounds.push(c);
                    }

                    var layer = L.polygon(bounds, { color: colorDictionary[response[i].category], weight: 2 }).addTo(map).bindPopup(name);
                    layers.push(layer);

                    if (response.length == 1) {
                        var marker = L.marker(layer.getBounds().getCenter()).addTo(map).bindPopup(name);
                        layers.push(marker);
                        map.fitBounds(bounds)
                    }
                }
            }
        }
    });
}

function clearAllLayers() {
    for (var i = 0; i < layers.length; i++) {
        map.removeLayer(layers[i]);
    }
    layers.length = 0;
}
