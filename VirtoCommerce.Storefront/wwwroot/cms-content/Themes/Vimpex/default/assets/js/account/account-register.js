var storefrontApp = angular.module('storefrontApp');
storefrontApp.controller('accountRegisterController', ['$q', '$scope', 'storefrontApp.mainContext', 'storefront.accountApi', 'loadingIndicatorService',
    function ($q, $scope, mainContext, accountApi, loader) {

        $scope.loader = loader;
        $scope.memberComponent = null;
        $scope.newMember = null;

        $scope.registerMemberFieldsConfig = [
            {
                field: 'CompanyName',
                disabled: false,
                visible: true,
                required: true
            },
            {
                field: 'Email',
                disabled: false,
                visible: true,
                required: true
            },
            {
                field: 'UserName',
                disabled: false,
                visible: true,
                required: true
            },
            {
                field: 'Password',
                disabled: false,
                visible: true,
                required: true
            }
        ];

        function getParams() {
            var params = window.location.search.substring(1).split("&"), result = {}, param, i;
            for (i in params) {
                if (params.hasOwnProperty(i)) {
                    if (params[i] === "") continue;

                    param = params[i].split("=");
                    result[decodeURIComponent(param[0])] = decodeURIComponent(param[1]);
                }
            }
            return result;
        }

        $scope.register = function () {
            $scope.error = null;
            $scope.loader.wrapLoading(function () {
                $scope.errors = null;

                return accountApi.register($scope.newMember, function (result) {
                    if (result.redirectUrl) {
                        $scope.outerRedirect(result.redirectUrl)
                    } else if (_.any(result.errors)) {
                        $scope.errors = result.errors;
                        scrollToTop();
                    } else {
                        $scope.complete = true;
                    }
                }, function (rejection) {
                }).$promise;
            });
        };

        // smooth scroll to window top
        function scrollToTop() {
            var currentScroll = document.documentElement.scrollTop || document.body.scrollTop;
            if (currentScroll > 0) {
                window.requestAnimationFrame(scrollToTop);
                window.scrollTo(0, currentScroll - (currentScroll / 5) - 20);
            }
        }
    }]);
