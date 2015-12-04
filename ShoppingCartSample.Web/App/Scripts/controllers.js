'use strict';

app.controller('HomeCtrl', ['$scope', 'Products', function ($scope, Products) {
    Products.getAll(function (data) {
        $scope.products = data;
    });
}]);

app.controller('MainCtrl', ['$scope', 'Currencies', 'Cart', function ($scope, Currencies, Cart) {
    Currencies.getAvailable(function (data) {
        $scope.currencies = data;

        var defaultCurrency = _.find(data, function (currency) { return currency.isDefault; });
        $scope.defaultCurrency = defaultCurrency.code;
        $scope.selectedCurrency = defaultCurrency;
        $scope.defaultCurrencySymbol = defaultCurrency.symbol;
    });

    Cart.get(function (data) {
        $scope.cart = data;
    });

    $scope.addToCart = function (productId, quantity) {
        quantity = (quantity || 1);
        Cart.addOrder({
            productId: productId,
            quantity: quantity
        }, function (data) {
            var newOrderId = data.id;
            var subTotal = data.subTotal;
            
            var orderToUpdate = _.find($scope.cart.orders, function (order) { return order.id === newOrderId });

            if (!orderToUpdate) {
                $scope.cart.orders.push({
                    id: newOrderId,
                    productID: productId,
                    quantity: quantity,
                    subTotal: subTotal
                });
            } else {
                orderToUpdate.quantity += quantity;
                orderToUpdate.subTotal = data.subTotal;
            }            

            $scope.cart.total = _.sum($scope.cart.orders, function(order) { return order.subTotal; });
        });
    };

    $scope.removeFromCart = function (orderId) {
        Cart.removeOrder({
            orderId: orderId
        }, function () {
            _.remove($scope.cart.orders, function (order) { return order.id === orderId; });

            $scope.cart.total = _.sum($scope.cart.orders, function (order) { return order.subTotal; });
        });
    };
}]);

app.controller('SideCtrl', ['$scope', 'Cart', function ($scope) {
}]);

app.controller('ProductDetailCtrl', ['$scope', '$routeParams', 'Products', function ($scope, $routeParams, Products) {

    //reset the value every time it's instantiated
    $scope.customerQuantity = 0;

    Products.getById({ id: $routeParams.id }, function (data) {
        $scope.product = data;
    });

    $scope.addToCart = function (productId) {        
        $scope.$parent.addToCart(productId, $scope.customerQuantity);
    }
}]);

app.controller('UserActionsCtrl', ['$scope', '$routeParams', 'UserActions', function ($scope, $routeParams, UserActions) {
    UserActions.getAll(function (data) {
        $scope.userActions = data;
    });
}]);

app.controller('ConfirmCtrl', ['$scope', '$location', 'Cart', function ($scope, $location, Cart) {    
    $scope.goToCheckout = function() {
        Cart.processCheckout({},
            function() {

            });
    };
}]);