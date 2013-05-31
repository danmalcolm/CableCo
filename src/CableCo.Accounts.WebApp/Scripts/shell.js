$(function () {

    var $el = $('[data-section=alerts-panel]');
    var $list = $el.find('[data-section=list]');
    $el.on('click', 'div.alert button.close', function () {
        var $div = $(this).closest('div.alert');
        var id = $div.data('id');
        hideAlert(id);
    });
    $el.on('click', '[data-action=clear-all]', function () {
        var alerts = getSavedAlerts();
        _.each(alerts, function (a) { a.visible = false; });
        saveAlerts(alerts);
        $list.empty();
    });
    
    var alertTemplate = _.template('<div class="alert alert-<%=type.toLowerCase()%>" data-id="<%=id%>">' +
        '<button  type="button" class="close" data-dismiss="alert">x</button>' +
        '<%=message%></div>');

    var getSavedAlerts = function () {
        var alertData = localStorage['alerts'];
        var alerts = alertData ? JSON.parse(alertData) : [];
        return alerts;
    };
    var hideAlert = function (id) {
        var alerts = getSavedAlerts();
        var alert = _.findWhere(alerts, { id: id });
        if (alert) alert.visible = false;
        saveAlerts(alerts);
    };
    var saveAlerts = function (alerts) {
        localStorage['alerts'] = JSON.stringify(alerts);
    };
    var displayAlerts = function (alerts) {
        var html = '';
        _.chain(alerts)
            .filter(function (a) { return a.visible !== false; })
            .each(function (alert) { html += alertTemplate(alert); });
        $list.prepend(html);
    };
    var updateAlerts = function () {

        $.ajax('/api/alerts', { dataType: 'json' })
            .success(function (alerts) {
                var savedAlerts = getSavedAlerts();
                var currentIds = _.pluck(savedAlerts, 'id');
                var newAlerts = _.filter(alerts, function (alert) {
                    return !_.contains(currentIds, alert.id);
                });
                displayAlerts(newAlerts);
                var allAlerts = newAlerts.slice(0);
                allAlerts.push.apply(allAlerts, savedAlerts);
                saveAlerts(allAlerts);
            })
            .done(function () {
                _.delay(updateAlerts, 1000);
            });
    };
    displayAlerts(getSavedAlerts());
    updateAlerts();
});

