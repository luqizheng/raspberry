// Write your JavaScript code.
window.GarbageTerminal = {
    pulverizer: function (bEnable, callback) {
        Turn("Pulverizer", bEnable, callback);
    },

    grayFan: function (bEnable, callback) {
        Turn("GrayFan", bEnable, callback);
    },


    pump: function (bEnable, callback) {
        Turn("Pump", bEnable, callback);
    },

    exhaustMain: function (bEnable, callback) {
        Turn("ExhaustMain", bEnable, callback);
    },

    exhaustSlave: function (bEnable, callback) {
        Turn("ExhaustSlave", bEnable, callback);
    },

    plasmaGenerator: function (bEnable, callback) {
        Turn("PlasmaGenerator", bEnable, callback);
    },

    transfer: function (bEnable, callback) {
        Turn("Transfer", bEnable, callback);
    },

    RefreshPowerStatus: function (callback) {
        $.get("/device/PowerControllerStatus", function (data) {
            callback(data);
        })
    },

    turn: function (bEnable, callback) {
        $.get("/device/Turn" + bEnable ? "On" : "Off", function (data) {
            callback(data);
        })
    },

    startTransfer: function (doNotStop, callback) {
        $.get("/device/StartTransfer/" + doNotStop ? "true" : "false", function (data) {
            callback(data);
        })
    },
    stopTransfer: function (callback) {
        $.get("/device/StopTransfer", function (data) {
            callback(data);
        })

    }
}

function Turn(device, bEnable, callback) {

    $.post("/Device/" + device, {
        power: bEnable ? 1 : 0
    }, function (data) {
        callback(data);
    });
}


function subcriptStatus(onStatucChange) {

    var connection = new signalR.HubConnectionBuilder()
        .withUrl("/GarbageTerminal")
        .configureLogging(signalR.LogLevel.Debug)
        .build();
    //connection.start().catch(err => console.error(err.toString()));

    connection.on("status", (statusData) => {
        console.log(statusData);
        onStatucChange(statusData);
    });

    connection.start().catch(err => console.error(err.toString()));;
}