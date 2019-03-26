app.controller('FaleConoscoController', function($scope, FaleConosco, $routeParams){

    $scope.master =  {};

    FaleConosco._returnAssunto()
    .then(function successCallback(response) {
        
        $scope.assuntos = response.data;
    });

    FaleConosco._returnBairros()
    .then(function successCallback(response) {
        
        $scope.bairros = response.data;
    });

    FaleConosco._returnInfo()
    .then(function successCallback(response) {
        
        $scope.info = response.data;
    });


    $scope.postContact = function(user) {

        user.BairroId = Number(user.BairroId);


        user.ContatoTipoId = $scope.assunto.Id.Id;
        user.BairroId = $scope.bairro.Id;

        FaleConosco._postUser(user)
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