app.controller('sugestaoController', function($scope, Sugestao, $routeParams, FaleConosco){

    $scope.master =  {};

    Sugestao._returnTipos()
    .then(function successCallback(response) {
        
        $scope.tipos = response.data;
    });

    FaleConosco._returnBairros()
    .then(function successCallback(response) {
        
        $scope.bairros = response.data;

    });


    $scope.postContact = function(user) {

        user.BairroId = Number(user.BairroId);


        user.ContatoTipoId = $scope.tipo.Id.Id;
        user.BairroId = $scope.bairro.Id;

        Sugestao._postUser(user)
        .then(
            function successCallback(response) {

                swal ( "Obrigado!" ,  "Sua mensagem foi enviada com sucesso." , "success");
                $scope.user = angular.copy($scope.master);

            }, 
            function errorCallback(response){
                swal ( "Desculpe!" ,  "Sua mensagem não pode ser enviada!" , "error");
            }
        );
                    
        
    };

});