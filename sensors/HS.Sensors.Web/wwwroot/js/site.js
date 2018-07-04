﻿// Write your JavaScript code.
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
    }
}

function Turn(device, bEnable, callback) {

    $.post("/Device/" + device,  {
        power: bEnable ? 1 : 0
    }, function (data) {
        callback(data);
    });
}