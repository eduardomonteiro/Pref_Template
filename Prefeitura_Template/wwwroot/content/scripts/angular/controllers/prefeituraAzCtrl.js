app.controller('prefeituraAZController', function($scope, $rootScope, $routeParams, $route, GetRootPage, $window, $location, PrefeituraAz){

    var page = 1;
    $scope.limitforPage = 6;
    $scope.lista = [];
    $scope.palavra = '';


    var request = function() {

        PrefeituraAz._getDados(page, $scope.limitforPage, $scope.palavra)
        .then(function successCallback(response) {

            if (response.data.length != 0) {
                $scope.lista = $scope.lista.concat(response.data);
            }
            else {
                $scope.disabled = true;
            }            
        });

    };

    $scope.loadMore = function() {
        page++;
        request();
    }
    
    request();

});