app.controller('patrimonioController', function($scope, Patrimonio){

    $scope.limitforPage = 6;


    Patrimonio._getCategorias()
    .then(function successCallback(response) {
        
        $scope.categorias = response.data;
        
    });
    
    Patrimonio._getDados()
    .then(function successCallback(response) {

        $scope.conteudo = response.data;
        
    });

    $scope.loadMore = function() {
        $scope.limitforPage = $scope.limitforPage + 6;
    };
    
});