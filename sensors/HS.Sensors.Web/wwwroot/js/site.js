﻿// Write your JavaScript code.
window.GarbageTerminal = {
    pulverizer: function (bEnable, callback) {
        Turn("Pulverizer", bEnable, callback);
    },

    grayFan: function (bEnable, callback) {
        Turn("GrayFan", bEnable, callback);
    },

    primaryPump: function (bEnable, callback) {
        Turn("primaryPump", bEnable, callback);
    },

    secondaryPump: function (benabe, callback) {
        Turn("secondaryPump", benabe, callback);
    },

    exhaustMain: function (bEnable, callback) {
        Turn("ExhaustMain", bEnable, callback);
    },

    exhaustSlave: function (bEnable, callback) {
        Turn("ExhaustSlave", bEnable, callback);
    },

    primaryPlasmaGenerator: function (bEnable, callback) {
        Turn("PrimaryPlasmaGenerator", bEnable, callback);
    },
    secondaryPlasmaGenerator: function (bEnable, callback) {
        Turn("SecondaryPlasmaGenerator", bEnable, callback);
    },
    transfer: function (bEnable, callback) {
        Turn("Transfer", bEnable, callback);
    },
    uvLight: function (bEnable, callback) {
        Turn("uvLight", bEnable, callback);
    },
    RefreshPowerStatus: function (callback) {
        $.get("/device/PowerControllerStatus", function (data) {
            callback(data);
        }).fail(function (resp) {
            var data = resp.responseJSON
            alert(data.message);
        })
    },

    turn: function (bEnable, callback) {
        $.get("/device/Turn" + (bEnable ? "On" : "Off"), function (data) {
            callback(data);
        }).fail(function (resp) {
            var data = resp.responseJSON
            alert(data.message);
        })
    },

    startTransfer: function (doNotStop, callback) {
        $.post("/device/StartTransfer", {
            RunningSeconds: doNotStop ? 60 * 60 * 3 : 60 * 3
        }, function (data) {
            callback(data);
        }).fail(function (resp) {
            var data = resp.responseJSON
            alert(data.message);
        })
    },
    stopTransfer: function (callback) {
        $.get("/device/StopTransfer", function (data) {
            callback(data);
        }).fail(function (resp) {
            var data = resp.responseJSON
            //{"success":true,"message":"还没启动，请启动后再执行传输"}
            alert(data.message);
        })
    }
}

function Turn(device, bEnable, callback) {
    $.post("/Device/" + device, {
        power: bEnable ? 1 : 0
    }, function (data) {
        callback(data);
    }).fail(function (resp) {
        var data = resp.responseJSON
        alert(data.message);
    });
}

var connection
function subcriptStatus(onStatucChange) {
    connection = new signalR.HubConnectionBuilder()
        .withUrl("/GarbageTerminal")
        .configureLogging(signalR.LogLevel.Trace)
        .build();
    //connection.start().catch(err => console.error(err.toString()));

    connection.on("status", (statusData) => {
        //console.log(statusData);
        onStatucChange(statusData);
    });

    connection.start().catch(err => console.error(err.toString()));
    //connection.invoke("SendMessage", user, message).catch(err => console.error(err.toString()));
}