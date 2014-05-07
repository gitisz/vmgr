var signalrScriptLoaded[SERVERID] = false;

signalrFunctionHubRegistry.add('[UNIQUEID]', function () {

    if (!signalrScriptLoaded[SERVERID]) {

        $.getScript('[HUBCONNECTION]/signalr/hubs')
            .done(function (script, status) {

                signalrScriptLoaded[SERVERID] = true;

                var connectionMoveProgress = $.hubConnection("[HUBCONNECTION]" + "/signalr");
                var moveProgressHub = connectionMoveProgress.createHubProxy('VmgrHub');
                var reconnectAttempts = 0;

                connectionMoveProgress.reconnected(function () {
                    reconnectAttempts = 0;
                }
                )
                ;

                connectionMoveProgress.disconnected(function () {

                    if (reconnectAttempts < 5) {
                        reconnectAttempts++;
                        setTimeout(function () {
                            connectionMoveProgress.start({})
                                .then(function () {
                                    moveProgressHub.invoke("addToGroup", "[GROUPKEY]");
                    }
                    )
                    ;
                }, 1000);
                }

                }
                )
                ;


                moveProgressHub.on('moveProgress', (function (obj) {
                    messageFunctionArray.execute(selectedServerUniqueId, obj.Message);
                    progressFunctionArray.execute(selectedServerUniqueId, obj);
                    }
                    )
                    )
                    ;

                var start = function () {
                    connectionMoveProgress.start({})
                        .then(function () {
                            moveProgressHub.invoke("addToGroup", "[GROUPKEY]");
                    });
                    };

                start();

            }
            )
            .fail(function (jqxhr, settings, exception) {
            }
            )
            ;
            }
            }
            )
            ;

statusFunctionArray.add('[UNIQUEID]', function () {
    $.ajax({
        type: "POST",
        url: "[POLLINGSERVICEURL]",
        cache: false,
        contentType: "application/json; charset=utf-8",
        data: "{ 'serverId':'[UNIQUEID]' }",
        dataType: "json",
        success: function (data) {
        var imageServerStatus = $("#[IMAGESERVERSTATUSCLIENTID]");

        signalrFunctionHubRegistry.execute('[UNIQUEID]', null); 

        if (data.d) {
            imageServerStatus.attr("src", "/_layouts/images/Vmgr/trafficlight-green-icon-32.png");
        } else {
            imageServerStatus.attr("src", "/_layouts/images/Vmgr/trafficlight-red-icon-32.png");
        }
        },
        error: function (msg) {
            var imageServerStatus = $("#[IMAGESERVERSTATUSCLIENTID]");
            imageServerStatus.attr("src", "/_layouts/images/Vmgr/trafficlight-red-icon-32.png");
            alert(msg);
        }
    });
    }
    )
    ;

messageFunctionArray.add('[UNIQUEID]', function (message) {

    var progressUpdateMessage = "#[PROGRESSUPDATEMESSAGECLIENTID]";

    $(progressUpdateMessage).empty();
    $(progressUpdateMessage).append(message);
}
)
;

progressFunctionArray.add('[UNIQUEID]', function (obj) {
                            
    var progressBar = $("#[PROGRESSUPDATEBARCLIENTID]");
    var progressbarValue = progressBar.find(".ui-progressbar-value");

    if (obj != null) {

        progressBar.show();

        progressBar.progressbar({
            value: obj.PrimaryValue / obj.PrimaryTotal * 100
        });

        if (obj.IsFaulted) {
            progressbarValue.css({
                "background": 'url(/_layouts/images/vmgr/red-gradient-background.png) repeat-x scroll 0px'
            });
        } else {
            if (obj.PrimaryValue == 0) {
                progressBar.progressbar("option", "value", false);
                var progressbarOverlay = progressBar.find(".ui-progressbar-overlay");
                progressbarOverlay.css({
                    "background": 'url(/_layouts/images/vmgr/progress-indeterminate.gif) repeat-x scroll 0px'
                });
                messageFunctionArray.execute('[UNIQUEID]', 'Attempting to contact server.');
            } else {
                progressbarValue.css({
                    "background": 'url(/_layouts/images/vmgr/green-gradient-background.png) repeat-x scroll 0px'
                });
            }
        }

        if (obj.PrimaryValue == obj.PrimaryTotal) {
            var parentWindow = getParentWindow();

            if (parentWindow != null) {
                parentWindow.BrowserWindow.OnRefreshPackages();

            }

            var ajaxPanelMovePackage = $find('[AJAXPANELMOVEPACKAGECLIENTID]');
            ajaxPanelMovePackage.ajaxRequest('REFRESH,');

            previousServerUniqueId = obj.ServerUniqueId;
            $get('[HIDDENFIELDSELECTEDSERVERCLIENTID]').value = obj.ServerUniqueId;

            resetPackageSelection();
        }
    }
    else {
        progressBar.hide();
    }
}
)
;

statusFunctionArray.execute('[UNIQUEID]', null);
messageFunctionArray.execute('[UNIQUEID]', ' ');

