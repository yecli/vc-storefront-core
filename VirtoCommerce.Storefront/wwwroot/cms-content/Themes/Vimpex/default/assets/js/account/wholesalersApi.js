angular.module('storefront.account')
.factory('storefront.wholesalersApi', ['$resource', function ($resource) {
    return $resource('storefrontapi/wholesalers', {}, {
        getWholesalersList: { url: 'storefrontapi/wholesalers', isArray: true },
        sentDeliveryAgreementRequest: { url: 'storefrontapi/wholesalers/agreement/send', method: 'POST' },
        selectWholesaler: { url: 'storefrontapi/wholesalers/:id/select' },
        confirmDeliveryAgreementRequest: { url: 'wholesalers/agreements/:id/confirm'}
    });
}]);
