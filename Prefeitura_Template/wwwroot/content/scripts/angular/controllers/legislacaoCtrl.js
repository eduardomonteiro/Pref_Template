app.controller('LegislacaoController', function($scope, Legislacao){
    
    var page = 1;
    $scope.limitforPage = 6;
    $scope.lista = [];
    $scope.palavra = '';
    $scope.categoriaid = 0
    $scope.datainicio = '';
    $scope.datafim = '';


    Legislacao._getCategorias()
    .then(function successCallback(response) {
        
        $scope.categorias = response.data;
        
    });


    var request = function() {

        Legislacao._getDados(page, $scope.limitforPage, $scope.palavra, $scope.datainicio, $scope.datafim, $scope.categoriaid)
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

    $scope.searchFilter = function() {

        page = 1;
        $scope.disabled = false;

        var inicio = $filter('date')($scope.datainicio, "dd/MM/yyyy");
        var fim  = $filter('date')($scope.datafim, "dd/MM/yyyy");
        
        Legislacao._getDados(page, $scope.limitforPage, $scope.palavra, inicio, fim, $scope.categoriaid)
        .then(function successCallback(response) {

            $scope.lista = response.data;
            
        });
    };

    request();

});