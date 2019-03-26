app.controller('perfilSocioController', function($scope, PerfilSocioEconomico){
    
    PerfilSocioEconomico._getDados()
    .then(function successCallback(response) {

        $scope.conteudo = response.data;

    });
    
});