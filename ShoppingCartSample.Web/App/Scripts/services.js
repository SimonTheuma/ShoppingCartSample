angular.module('cartService', ['ngResource'])

    .factory('Products', function ($resource) {
        return $resource('/products/:operation/:id', { id: '@id' },
        {
            getAll: {
                method: 'GET',
                isArray: true,
                success: function (data) { return data; },
                error: function (data) { return data; }
            },

            getById: {
                method: 'GET',
                isArray: false,
                success: function (data) { return data; },
                error: function (data) { return data; }
            }
        });
    })

    .factory('Currencies', function ($resource) {
        return $resource('/currencies/:operation', {},
        {
            getAvailable: {
                method: 'GET',
                params: { operation: 'available' },
                isArray: true,
                success: function (data) { return data; },
                error: function (data) { return data; }
            }
        });
    })

    .factory('Cart', function ($resource) {
        return $resource('/cart/:operation', {},
        {
            get: {
                method: 'GET',
                isArray: false,
                success: function (data) { return data; },
                error: function (data) { return data; }
            },

            addOrder: {
                method: 'PUT',
                success: function (data) { return data; },
                error: function (data) { return data; }
            },

            removeOrder: {
                method: 'DELETE',
                success: function (data) { return data; },
                error: function (data) { return data; }
            },

            updateOrder: {
                method: 'POST',
                success: function (data) { return data; },
                error: function (data) { return data; }
            },

            confirm: {
                method: 'POST',
                params: { operation: 'confirm' },
                success: function (data) { return data; },
                error: function (data) { return data; }
            },

            checkout: {
                method: 'POST',
                params: { operation: 'checkout' },
                success: function (data) { return data; },
                error: function (data) { return data; }
            },

            transfer: {
                method: 'POST',
                params: { operation: 'transfer' },
                success: function (data) { return data; },
                error: function (data) { return data; }
            },

            clear: {
                method: 'DELETE',
                params: { operation: 'clear' },
                success: function (data) { return data; },
                error: function (data) { return data; }
            }
        });
    })

    .factory('UserActions', function ($resource) {
        return $resource('/useractions', {},
        {
            getAll: {
                method: 'GET',
                isArray: true,
                success: function (data) { return data; },
                error: function (data) { return data; }
            }
        });
    })

    .factory('Notifications', function ($rootScope) {
        return {
            addAlert: function (type, message, timeout) {

                if (!$rootScope.alerts) {
                    $rootScope.alerts = [];
                }

                $rootScope.alerts.push(
                {
                    type: type,
                    msg: message,
                    timeout: (timeout || 5000)
                });
            },

            closeAlert: function (index) {
                $rootScope.alerts.splice(index, 1);
            }
        }
    })

    .factory('Account', function ($resource) {
        return $resource('/account/:operation', {},
        {
            login: {
                method: 'POST',              
                params: { operation: 'login' },
                success: function (data) { return data; },
                error: function (data) { return data; }
            },
            register: {
                method: 'POST',              
                params: { operation: 'register' },
                success: function (data) { return data; },
                error: function (data) { return data; }
            },
            logout: {
                method: 'POST',
                params: {operation: 'logout' },
                success: function (data) { return data; },
                error: function (data) { return data; }
            }
        });
    });