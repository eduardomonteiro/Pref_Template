app.controller('secretariaController', function($scope, Secretaria){
   
    Secretaria._getDados()
    .then(function successCallback(response) {
        
        $scope.secretarias = response.data;
          
    });


});