app.controller('atracoesTuristicasControlller', function($scope, Turismo){
    
    Turismo._getDados()
    .then(function successCallback(response) {

        $scope.conteudo = response.data;
    
    });

});