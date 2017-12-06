<ng-outlet ng-init=""></ng-outlet>
<script id="account-wholesalers.tpl" type="text/ng-template">
    <div>
        <h4>List of wholesalers</h4>
        <table class="full" ng-if="$ctrl.entries.length">
            <thead>
                <tr>
                    <th>Name</th>
                    <th>Email</th>
                    <th>Phone</th>
                    <th>Status</th>
                    <th>Actions</th>
                </tr>
            </thead>
            <tbody>
                <tr ng-repeat="wholesaler in $ctrl.entries">
                    <td ng-bind="wholesaler.name"></td>
                    <td ng-bind="wholesaler.email"></td>
                    <td ng-bind="wholesaler.phone"></td>
                    <td ng-bind="wholesaler.agreementRequest.status"></td>
                    <td>
                        <span>
                            <a ng-if="wholesaler.agreementRequest.status == 'NotSent'" ng-click="$ctrl.sendAgreement(wholesaler)" href="">Send agreement</a>
                            <a ng-if="!wholesaler.isActive && wholesaler.agreementRequest.status == 'Confirmed'" ng-click="$ctrl.selectWholesaler(wholesaler)" href="">Set current</a>
                            <a ng-if="wholesaler.agreementRequest.status == 'Sent'" ng-click="$ctrl.confirmAgreement(wholesaler.agreementRequest)" href="">Confirm (only for test)</a>
                            <a ng-if="wholesaler.isActive" ng-href="{{ wholesaler.url }}">Active (go to store)</a>
                        </span>
                    </td>
                </tr>
            </tbody>
        </table>
        <div ng-if='$ctrl.pageSettings.totalItems > $ctrl.pageSettings.itemsPerPageCount' ng-include="'pagerTemplate.html'"></div>
        <p ng-if="!$ctrl.entries.length && !$ctrl.loader.isLoading">No wholesalers available</p>
    </div>
</script>
