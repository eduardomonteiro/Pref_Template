app.controller('perguntasController', function($scope, PerguntasFrequentes, $rootScope){
    
    PerguntasFrequentes._getDados()
    .then(function successCallback(response) {


        $scope.lista = response.data;
                  
    });   
});