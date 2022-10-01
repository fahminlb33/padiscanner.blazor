﻿const namespace = "PadiScanner.Components.Geolocation";

async function dispatchResponse(id, location) {
    await DotNet.invokeMethodAsync(namespace, 'ReceiveResponse', id, location.latitude, location.longitude, location.accuracy);
}

async function dispatchErrorResponse(id, error) {
    await DotNet.invokeMethodAsync(namespace, 'ReceiveErrorResponse', id, error.code, error.message);
}

window.PadiScannerGeolocation = {
    GetLocation: (requestId) => {
        if (navigator.geolocation) {
            const options = { timeout: 10000 };
            navigator.geolocation.getCurrentPosition(
                (position) => dispatchResponse(requestId, position.coords),
                (error) => dispatchErrorResponse(requestId, error),
                options
            );
        } else {
            const errorData = {
                code: 4,
                message: "Geolocation not available",
                PERMISSION_DENIED: 1,
                POSITION_UNAVAILABLE: 2,
                TIMEOUT: 3
            };
            dispatchErrorResponse(requestId, errorData);
        }
    }
};
