var storefrontApp = angular.module('storefrontApp');
storefrontApp.component('vcMemberDetail', {
    templateUrl: "themes/assets/memberDetail.tpl.html",
    bindings: {
        member: '=',
        memberComponent: '=',
        fieldsConfig: '<'
    },
    controller: ['$scope', 'storefront.accountApi', function ($scope, accountApi) {
        var $ctrl = this;

        $ctrl.config = [
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
                visible: true
            },
            {
                field: 'Password',
                disabled: false,
                visible: true
            },
            {
                field: 'Roles',
                disabled: false,
                visible: false
            }
        ];

        if ($ctrl.fieldsConfig)
            angular.extend($ctrl.config, $ctrl.fieldsConfig);

        $ctrl.rolesComponent = null;

        this.$onInit = function () {
            $ctrl.memberComponent = this;
        };

        this.$onDestroy = function () {
            $ctrl.memberComponent = null;
        };

        $ctrl.setForm = function (frm) {
            $ctrl.form = frm;
        };

        $ctrl.validate = function () {
            if ($ctrl.form) {
                $ctrl.form.$setSubmitted();
                _.each(components, function (c) { return c.validate() }); // validate all
                return _.all(components, function (c) { return c.validate() }) && $ctrl.form.$valid;
            }

            return true;
        };

        var components = [];
        $ctrl.addComponent = function (component) {
            components.push(component);
        };
        $ctrl.removeComponent = function (component) {
            components = _.without(components, component);
        };

        $ctrl.showField = function (field) {
            return getFieldConfig(field).visible == true;
        }

        $ctrl.disableField = function (field) {
            return getFieldConfig(field).disabled == true;
        }

        $ctrl.requiredField = function (field) {
            return getFieldConfig(field).required == true;
        }

        function getFieldConfig(field) {
            var configItem = _.first(_.filter($ctrl.config, function (configItem) { return configItem.field === field; }));
            return configItem;
        }

        $ctrl.availCountries = accountApi.getCountries();

        $ctrl.getCountryRegions = function (country) {
            return accountApi.getCountryRegions(country).$promise;
        };
    }]
});

storefrontApp.directive('confirmPasswordValidation', function () {
    return {
        require: 'ngModel',
        link: function (scope, elem, attr, ngModel) {
            ngModel.$parsers.unshift(function (value, scope) {
                var isValid = true;
                var password = ngModel.$$parentForm.Password.$viewValue;

                if (password) {
                    isValid = password === value;
                }

                ngModel.$setValidity('confirmPasswordValidation', isValid);
                return value;
            });
        }
    };
});
