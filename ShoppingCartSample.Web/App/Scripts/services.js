angular.module('cartService', ['ngResource'])

    .factory('Products', function ($resource) {
        return $resource('/products/:operation/:id', { id: '@id' },
        {
            getAll: {
                method: 'GET',
                isArray: true,
                success: function (data) { return data; },
                error: function (data) { }
            },

            getById: {
                method: 'GET',
                isArray: false,
                success: function (data) { return data; },
                error: function (data) { }
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
                error: function (data) { }
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
                error: function (data) { }
            },

            addOrder: {
                method: 'PUT',
                success: function (data) { return data; }
            },

            removeOrder: {
                method: 'DELETE',
                success: function (data) { return data; }
            },

            updateOrder: {
                method: 'POST',
                success: function (data) { return data; }
            },

            confirmation: {
                method: 'POST',
                params: { operation: 'confirm' },
                success: function (data) { return data; }
                //TODO: add error here if user is temporary
            },

            checkout: {
                method: 'POST',
                params: { operation: 'checkout' },
                success: function (data) { return data; }
            },

            transfer: {
                method: 'POST',
                params: { operation: 'transfer' },
                success: function (data) { return data; }
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
                error: function (data) { }
            }
        });
    });