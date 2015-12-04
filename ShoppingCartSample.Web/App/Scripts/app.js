'use strict';

var app = angular.module('cart', ['ngRoute', 'cartService', 'ui.bootstrap']);

app.config(['$routeProvider', 
    function($routeProvider) {
        $routeProvider
            .when('/', { templateUrl: '/App/Partials/Home.html' })
            .when('/products', { templateUrl: '/App/Partials/Products/List.html' })
            .when('/products/:id', { templateUrl: '/App/Partials/Products/Detail.html' })
            .when('/actions', { templateUrl: '/App/Partials/Actions.html' })
            .when('/confirm', { templateUrl: '/App/Partials/Confirmation.html' })
            .otherwise({ redirectTo: '/'});
    }]);