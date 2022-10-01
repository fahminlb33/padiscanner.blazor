export let LeafletMap = {

    Map: {

        setView: function (map, center, zoom) {
            map.setView(center, zoom);
        },

        fitBounds: function (map, bounds) {
            map.fitBounds(bounds);
        }

    },

    Layer: {

        addTo: function (layer, map) {
            console.log(layer);
            console.log(map);
            layer.addTo(map);
        },

        remove: function (layer) {
            layer.remove();
        }

    },

    Polyline: {

        addLatLng: function (polyline, latlng, latlngs) {
            polyline.addLatLng(latlng, latlngs);
        }

    }

}