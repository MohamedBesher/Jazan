function initMapSearchBox(map, markers, selectedAddress) {
    if (selectedAddress != '') {
        $('#pac-input').val(selectedAddress);
    }

    var geocoder = new google.maps.Geocoder();
    if (geocoder) {
        geocoder.geocode({
            'address': selectedAddress
        }, function (results, status) {
            if (status == google.maps.GeocoderStatus.OK) {
                if (status != google.maps.GeocoderStatus.ZERO_RESULTS) {
                    map.setCenter(results[0].geometry.location);

                    markers = [];

                    // Create a marker for each place.
                    markers.push(new google.maps.Marker({
                        map: map,
                        title: results[0].formatted_address,
                        position: results[0].geometry.location
                    }));


                    var lat = results[0].geometry.location.lat();
                    var lang = results[0].geometry.location.lng();

                    localStorage.setItem('tourist-details-address-latitude', lat);
                    localStorage.setItem('tourist-details-address-address-longtitude', lang);
                    localStorage.setItem('tourist-details-address', $('#pac-input').val());

                    var infoWindow = new google.maps.InfoWindow();

                    for (var i = 0; i < markers.length; i++) {
                        var data = markers[i];
                        var myLatlng = new google.maps.LatLng(data.position.lat(), data.position.lng());
                        var marker = new google.maps.Marker({
                            position: myLatlng,
                            map: map,
                            title: data.title
                        });

                        (function (marker, data) {
                            google.maps.event.addListener(marker, "click", function (e) {
                                infoWindow.setContent("<div style = 'width:200px;min-height:40px'>" + data.title + "</div>");
                                infoWindow.open(map, marker);
                            });
                        })(marker, data);
                    }

                }
                else {
                    alert("No results found");
                }
            } else {
                alert("Geocode was not successful for the following reason: " + status);
            }
        });
    }
}

function initMap(divId, markers, selectedAddress, callback) {
    var mapDiv = document.getElementById(divId);
    var flightPlanCoordinates = [];

    var map = new google.maps.Map(mapDiv, {
        center: new google.maps.LatLng(markers[0].lat, markers[0].lng),
        zoom: 7,
        mapTypeId: 'terrain'
    });

    if (markers.length > 0) {
        map = new google.maps.Map(mapDiv, {
            center: new google.maps.LatLng(markers[0].lat, markers[0].lng),
            zoom: 7,
            mapTypeId: 'terrain'
        });

        var input = document.getElementById('pac-input');
        if (input) {
            var searchBox = new google.maps.places.SearchBox(input);
            map.controls[google.maps.ControlPosition.TOP_LEFT].push(input);

            map.addListener('bounds_changed', function () {
                searchBox.setBounds(map.getBounds());
            });

            searchBox.addListener('places_changed', function () {
                var places = searchBox.getPlaces();

                if (places.length == 0) {
                    return;
                }

                markers = [];

                // For each place, get the icon, name and location.

                var bounds = new google.maps.LatLngBounds();
                places.forEach(function (place) {
                    if (!place.geometry) {
                        console.log("Returned place contains no geometry");
                        return;
                    }

                    // Create a marker for each place.
                    markers.push(new google.maps.Marker({
                        map: map,
                        title: place.name,
                        position: place.geometry.location
                    }));


                    var lat = place.geometry.location.lat();
                    var lang = place.geometry.location.lng();
                    localStorage.setItem('tourist-details-address-latitude', lat);
                    localStorage.setItem('tourist-details-address-address-longtitude', lang);
                    localStorage.setItem('tourist-details-address', $('#pac-input').val());

                    if (place.geometry.viewport) {
                        bounds.union(place.geometry.viewport);
                    } else {
                        bounds.extend(place.geometry.location);
                    }
                });
                map.fitBounds(bounds);
            });
        }



        var infoWindow = new google.maps.InfoWindow();

        for (var i = 0; i < markers.length; i++) {
            var data = markers[i];
            var myLatlng = new google.maps.LatLng(data.lat, data.lng);
            //flightPlanCoordinates.push(new google.maps.LatLng(data.lat, data.lng));
            var marker = new google.maps.Marker({
                position: myLatlng,
                map: map,
                title: data.title
            });

            (function (marker, data) {
                google.maps.event.addListener(marker, "click", function (e) {
                    infoWindow.setContent("<div style = 'width:200px;min-height:40px'>" + data.title + "</div>");
                    infoWindow.open(map, marker);
                });
            })(marker, data);
        }
    }
    else {
        var input = document.getElementById('pac-input');

        var searchBox = new google.maps.places.SearchBox(input);
        map.controls[google.maps.ControlPosition.TOP_LEFT].push(input);

        map.addListener('bounds_changed', function () {
            searchBox.setBounds(map.getBounds());
        });

        searchBox.addListener('places_changed', function () {
            var places = searchBox.getPlaces();

            if (places.length == 0) {
                return;
            }

            markers = [];

            // For each place, get the icon, name and location.

            var bounds = new google.maps.LatLngBounds();
            places.forEach(function (place) {
                if (!place.geometry) {
                    console.log("Returned place contains no geometry");
                    return;
                }

                // Create a marker for each place.
                markers.push(new google.maps.Marker({
                    map: map,
                    title: place.name,
                    position: place.geometry.location
                }));


                var lat = place.geometry.location.lat();
                var lang = place.geometry.location.lng();

                localStorage.setItem('tourist-details-address-latitude', lat);
                localStorage.setItem('tourist-details-address-address-longtitude', lang);
                localStorage.setItem('tourist-details-address', $('#pac-input').val());

                if (place.geometry.viewport) {
                    bounds.union(place.geometry.viewport);
                } else {
                    bounds.extend(place.geometry.location);
                }

            });
            map.fitBounds(bounds);
        });

    }
    callback(map);


}

function intializeSearchMapLocation(title, lat, lng, description, mapDiv) {
    markers = [];

    markers = [
         {
             "title": title,
             "lat": lat,
             "lng": lng,
             "description": description
         }];

    initMap(mapDiv, markers, '', function (map) {
        $$('body').on('touchend', '.pac-item', function (e) {
            var selectedAddress = $(this).text();
            initMapSearchBox(map, markers, selectedAddress);
        });
    });
}

function initMapWithCoordinates(divId, lat, lng, title, showMarkerOnDrawing) {
    var mapDiv = document.getElementById(divId);

    var map = new google.maps.Map(mapDiv, {
        center: new google.maps.LatLng(lat, lng),
        zoom: 7,
        mapTypeId: 'terrain'
    });


   

    if (showMarkerOnDrawing) {
        var myLatlng = new google.maps.LatLng(lat, lng);
        var marker = new google.maps.Marker({
            position: myLatlng,
            map: map,
            title: title
        });
    } else {
        map.addListener('click', function (e) {
            placeMarkerAndPanTo(e.latLng, map);
        });
    }

}


function placeMarkerAndPanTo(latLng, map) {
    deleteMarkers();
    var marker = new google.maps.Marker({
        position: latLng,
        map: map
    });
    map.panTo(latLng);
    markers.push(marker);
    localStorage.setItem('tourist-details-address-latitude', latLng.lat());
    localStorage.setItem('tourist-details-address-address-longtitude', latLng.lng());
}


function deleteMarkers() {
    clearMarkers();
    markers = [];
}

function clearMarkers() {
    setMapOnAll(null);
}

function setMapOnAll(map) {
    for (var i = 0; i < markers.length; i++) {
        markers[i].setMap(map);
    }
}

var markers = [];
var map;