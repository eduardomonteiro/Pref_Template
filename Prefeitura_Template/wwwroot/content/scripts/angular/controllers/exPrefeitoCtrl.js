app.controller('exPrefeitosController', function($scope, ExPrefeitos){
    
    $scope.limitForPage = 6;
    
    ExPrefeitos._getDados()
    .then(function successCallback(response) {
        
        $scope.conteudo = response.data;  
    });

});