    /**
     * @ngdoc model
     * @name NotificationMonitorDisplay
     * @function
     *
     * @description
     * Represents a JS version of Merchello's NotificationMonitorDisplay object
     */
    var NotificationMonitorDisplay = function() {
        var self = this;
        self.monitorKey = '';
        self.name = '';
        self.alias = '';
    };

    angular.module('merchello.models').constant('NotificationMonitorDisplay', NotificationMonitorDisplay);
