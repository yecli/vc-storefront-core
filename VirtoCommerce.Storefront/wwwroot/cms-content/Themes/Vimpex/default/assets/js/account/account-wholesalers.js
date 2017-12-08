angular.module('storefront.account')
.component('vcAccountWholesalers', {
    templateUrl: "themes/assets/account-wholesalers.tpl.liquid",
    $routeConfig: [
        { path: '/', name: 'WholesalersList', component: 'vcAccountWholesalersList', useAsDefault: true }
    ],
    controller: ['storefront.wholesalersApi', function (accountApi) {
        var $ctrl = this;
    }]
})

.component('vcAccountWholesalersList', {
    templateUrl: "account-wholesalers.tpl",
    bindings: { $router: '<' },
    controller: ['storefrontApp.mainContext', '$scope', 'storefront.wholesalersApi', 'loadingIndicatorService', 'dialogService', 'confirmService', '$location', '$translate', function (mainContext, $scope, wholesalersApi, loader, dialogService, confirmService, $location, $translate) {
        var $ctrl = this;        
        $ctrl.loader = loader;
        $ctrl.pageSettings = { currentPage: 1, itemsPerPageCount: 5, numPages: 10 };
        $ctrl.pageSettings.pageChanged = function () {
            loader.wrapLoading(function () {
                return wholesalersApi.getWholesalersList(function (data) {
                    $ctrl.entries = data;
                    $ctrl.pageSettings.totalItems = data.length;                      
                }).$promise;
            });
        };

        this.$routerOnActivate = function (next) {
            $ctrl.pageSettings.currentPage = next.params.pageNumber || $ctrl.pageSettings.currentPage;
        };

        $scope.$watch(
            function () { return mainContext.customer; },
            function (customer) {                 
                    $ctrl.pageSettings.pageChanged();                    
            }
        );
                    

        $ctrl.sendAgreement = function (wholesaler) {
            loader.wrapLoading(function () {
                return wholesalersApi.sentDeliveryAgreementRequest(wholesaler.agreementRequest, function (data) {
                    $ctrl.pageSettings.pageChanged();
                }).$promise;
            });
            dialogService.showDialog({ wholesaler: wholesaler }, 'recentlySentWholesalerAgreementDialog', 'storefront.recently-sent-wholesaler-agreement-dialog.tpl');
        };

        $ctrl.confirmAgreement = function (agreement) {
            loader.wrapLoading(function () {
                return wholesalersApi.confirmDeliveryAgreementRequest({ id: agreement.id }, {}, function (data) {
                    $ctrl.pageSettings.pageChanged();
                }).$promise;
            });
        };

        $ctrl.selectWholesaler = function (wholesaler) {
            loader.wrapLoading(function () {
                return wholesalersApi.selectWholesaler({ id: wholesaler.id }, {}, function (data) {
                    $ctrl.pageSettings.pageChanged();
                }).$promise;
            });
        };
            
        $ctrl.validate = function () {
            $ctrl.inviteForm.$setSubmitted();
            return $ctrl.inviteForm.valid;
        };

    }]
});

storefrontApp.controller('recentlySentWholesalerAgreementDialog', ['$scope', '$uibModalInstance', 'dialogData', function ($scope, $uibModalInstance, dialogData) {
    $scope.dialogData = dialogData;

    $scope.close = function () {
        $uibModalInstance.close();
    }
}]);
