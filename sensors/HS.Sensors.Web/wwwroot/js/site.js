// Write your JavaScript code.
window.GarbageTerminal = {
    Pulverizer: function (bEnable, callback) {
        Turn("Pulverizer", bEnable, callback);
    },

    GrayFan: function (bEnable, callback) {
        Turn("GrayFan", bEnable, callback);
    },


    Pump: function (bEnable, callback) {
        Turn("Pump", bEnable, callback);
    },

    ExhaustMain: function (bEnable, callback) {
        Turn("ExhaustMain", bEnable, callback);
    },

    ExhaustSlave: function (bEnable, callback) {
        Turn("ExhaustSlave", bEnable, callback);
    },

    PlasmaGenerator: function (bEnable, callback) {
        Turn("PlasmaGenerator", bEnable, callback);
    },

    Transfer: function (bEnable, callback) {
        Turn("Transfer", bEnable, callback);
    },

    RefreshPowerStatus: function (callback) {
        $.get("/device/PowerControllerStatus", function (data) {
            callback(data);
        })
    }
}

function Turn(device, bEnable, callback) {

    $.post("/Device/" + device,  {
        power: bEnable ? 1 : 0
    }, function (data) {
        callback(data);
    });
}