'use strict';

app.controller('HomeCtrl', ['$scope', 'Products', function ($scope, Products) {
    Products.getAll(
        function (data) {
            $scope.products = data;
        },
        function (error) {

        });
}]);

app.controller('MainCtrl', ['$scope', 'Currencies', 'Cart', 'Notifications', function ($scope, Currencies, Cart, Notifications) {
    Currencies.getAvailable(
        function (data) {
            $scope.currencies = data;

            var defaultCurrency = _.find(data, function (currency) { return currency.isDefault; });
            $scope.defaultCurrency = defaultCurrency.code;            
            $scope.selectedCurrency = defaultCurrency;
            $scope.currencySymbol = defaultCurrency.symbol;
        },
        function (error) {

        });

    Cart.get(
        function (data) {
            $scope.cart = data;
        },
        function (error) {
            switch (error.status) {
                default:
                    {
                        Notifications.addAlert('danger', 'An unspecified error has occurred.  Please try reloading the page.', 4000);
                    }
            }
        });

    $scope.changeCurrency = function(oldCurrencyCode, newCurrencyCode) {
        var currency = _.find($scope.currencies, function (currency) { return currency.code === newCurrencyCode; });

        $scope.selectedCurrency = currency;
        $scope.currencySymbol = currency.symbol;
    }

    $scope.addToCart = function (productId, quantity) {
        quantity = (quantity || 1);
        Cart.addOrder({
            productId: productId,
            quantity: quantity
        },

        function (data) {
            var newOrderId = data.id;
            var subTotal = data.subTotal;
            var name = data.productName;

            var orderToUpdate = _.find($scope.cart.orders, function (order) { return order.id === newOrderId });

            if (!orderToUpdate) {
                $scope.cart.orders.push({
                    id: newOrderId,
                    productID: productId,
                    quantity: quantity,
                    productName: name,
                    subTotal: subTotal
                });
            } else {
                orderToUpdate.quantity += quantity;
                orderToUpdate.subTotal = data.subTotal;
            }

            $scope.cart.total = _.sum($scope.cart.orders, function (order) { return order.subTotal; });
            Notifications.addAlert('success', 'Product added to cart!', 3000);
        },

        function (error) {
            switch (error.status) {
                case 400:
                    {
                        if (error.statusText === 'stock') {
                            Notifications.addAlert('danger', 'Unable to add product to cart - product is out of stock.', 4000);
                            break;
                        }
                    }
            }
        });
    };

    $scope.removeFromCart = function (orderId) {
        Cart.removeOrder({
            orderId: orderId
        },
        function (data) {
            _.remove($scope.cart.orders, function (order) { return order.id === orderId; });

            $scope.cart.total = _.sum($scope.cart.orders, function (order) { return order.subTotal; });
            Notifications.addAlert('success', 'Product removed from cart!', 3000);
        },
        function (error) {

        });
    };
}]);

app.controller('SideCtrl', ['$scope', '$location', 'Cart', 'Notifications', function ($scope, $location, Cart, Notifications) {
    $scope.clearCart = function () {
        Cart.clear(
        function (data) {
            Notifications.addAlert('success', 'Cart has been cleared!', 3000);

            Cart.get(function (data) {
                $scope.$parent.cart = data;
            });
        },
        function (error) {

        });
    };

    $scope.goToConfirmation = function () {
        $location.path('/confirm');
    };
}]);

app.controller('ProductDetailCtrl', ['$scope', '$routeParams', '$location', 'Products', 'Notifications', function ($scope, $routeParams, $location, Products, Notifications) {

    //reset the value every time it's instantiated
    $scope.customerQuantity = 0;

    Products.getById({ id: $routeParams.id },
        function (data) {
            $scope.product = data;
        },
        function (error) {
            switch (error.status) {
                case 404:
                    {
                        $scope.notFound = true;
                        break;
                    }
                default:
                    {
                        $location.path('/');
                        Notifications.addAlert('danger', 'An unspecified error has occurred... :(', 5000);
                        break;
                    }
            }
        }
    );

    $scope.addToCart = function (productId) {
        $scope.$parent.addToCart(productId, $scope.customerQuantity);
    }
}]);

app.controller('UserActionsCtrl', ['$scope', '$routeParams', 'UserActions', 'Notifications', function ($scope, $routeParams, UserActions, Notifications) {
    UserActions.getAll(
        function (data) {
            $scope.userActions = data;
        },
        function (error) {
            $scope.noUserActions = true;
            Notifications.addAlert('danger', 'User actions could not be loaded. Please try again later.', 5000);
        }
    );
}]);

app.controller('ConfirmCtrl', ['$scope', '$location', 'Cart', 'Notifications', function ($scope, $location, Cart, Notifications) {
    $scope.$parent.confirming = true;

    $scope.isTemporary = $scope.$parent.isTemporary;

    $scope.goToCheckout = function () {
        Cart.checkout({},
            function (data) {
                $location.path('/success');
                $scope.$parent.cart = {};
            },
            function (error) {
                switch (error.status) {
                    case 400:
                        {
                            if (error.statusText === 'cart') {
                                $location.path('/');
                                Notifications.addAlert('warn', 'You cannot check out an empty cart.  Returning to homepage...', 6000);
                            } else if (error.statusText === 'stock') {
                                Notifications.addAlert('warn', 'Certain items cannot be purchased because they are out of stock for the quoted amount.', 6000);
                            }
                            break;

                        }
                }
                Notifications.addAlert('danger', 'An error has occurred while trying to check out.  Please try again later.', 6000);
            }
        );
    };

    $scope.$on('$locationChangeStart', function (event) {
        $scope.$parent.confirming = false;
    });

    Cart.confirm(
           function (data) {
           },
           function (error) {
               switch (error.status) {
                   case 402:
                       {
                           //indicate that user is temporary and needs to register prior to checkout
                           $scope.isTemporary = true;
                           break;
                       }
                   default:
                       {
                           Notifications.addAlert('danger', 'An unspecified error has occurred... :(', 3000);
                           break;
                       }
               }

           });

}]);

app.controller('NotificationCtrl', ['$scope', 'Notifications', function ($scope, Notifications) {

    $scope.closeAlert = Notifications.closeAlert;
}]);

app.controller('AccountCtrl', ['$scope', '$uibModal', '$location', 'Account', 'AccountSettings', 'Notifications', 'Cart', function ($scope, $uibModal, $location, Account, AccountSettings, Notifications, Cart) {

    if (userId) {
        AccountSettings.setUserId(userId);
        $scope.userId = userId;
    }

    if (account) {
        AccountSettings.setAccount(account);
        $scope.account = account;
    }

    $scope.loginForm = {};
    $scope.registerForm = {};

    $scope.register = function () {

        AccountSettings.setUserId($scope.userId);
        var registerForm = $scope.registerForm;

        Account.register(
            {
                email: registerForm.email,
                password: registerForm.password,
                confirmPassword: registerForm.confirmPassword
            },
            function (data) {
                $location.path('/');
                location.reload();
            }, function (error) {
                switch (error.status) {
                    case 400:
                        {
                            break;
                        }
                    default:
                        {
                            Notifications.addAlert('danger', 'An error occurred while processing registration.  Please try again.', 3000);
                            break;
                        }
                }
            });
    };

    $scope.login = function () {

        AccountSettings.setUserId($scope.userId);
        var loginForm = $scope.loginForm;
        //is temporary

        Account.login(
            {
                email: loginForm.email,
                password: loginForm.password,
                rememberMe: loginForm.rememberMe
            },
            function (data) {
                $location.path('/');
                location.reload();
            },
            function (error) {
                switch (error.status) {
                    case 400:
                        {
                            //handle invalid login
                            break;
                        }
                        //user needs to accept transferring the new cart to the old user
                    case 409:
                        {
                            var modal = $uibModal.open({
                                animation: true,
                                templateUrl: 'transferCartModal.html',
                                controller: 'TransferModalCtrl',
                                size: 'sm'                                
                            });
                        }
                }
            });
    };

    $scope.logout = function () {
        Account.logout({},
            function (data) {
                $location.path('/');
                location.reload();
            },
            function (error) {
                Notifications.addAlert('danger', 'An error has occurred while attempting to log out.  Please reload the page.', 5000);
            });
    };
}]);

app.controller('TransferModalCtrl', [
    '$scope', '$uibModalInstance', 'Notifications', 'Cart', 'AccountSettings', function ($scope, $uibModalInstance, Notifications, Cart, AccountSettings) {
        var sourceUserId = AccountSettings.getUserId();

        $scope.transferCart = function () {
            Cart.transfer({ sourceUserId: sourceUserId },
                function (data) {
                    AccountSettings.setIsTemporary(false);
                },
                function (error) {
                    //what else to do in case there's an error transferring the cart?
                    Notifications.addAlert('danger', 'An error occurred while attempting to transfer your cart. Please reload the page.', 6000);
                }
            );
        }
    }
]);