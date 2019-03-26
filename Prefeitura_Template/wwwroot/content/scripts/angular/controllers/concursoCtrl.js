app.controller('concursosController', function($scope, Concursos){
    
    var page = 1;
    $scope.limitforPage = 6;
    $scope.concursos = [];
    $scope.palavra = '';
    $scope.statusid = 0
    $scope.datainicio = '';
    $scope.datafim = '';


    Concursos._getStatus()
    .then(function successCallback(response) {
        
        $scope.status = response.data;
        
    });


    var request = function() {

        Concursos._getDados(page, $scope.limitforPage, $scope.palavra, $scope.datainicio, $scope.datafim, $scope.statusid)
        .then(function successCallback(response) {

            if (response.data.length != 0) {
                $scope.concursos = $scope.concursos.concat(response.data);
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
        
        Concursos._getDados(page, $scope.limitforPage, $scope.palavra, inicio, fim, $scope.statusid)
        .then(function successCallback(response) {

            $scope.concursos = response.data;
            
        });
    };

    request();

});