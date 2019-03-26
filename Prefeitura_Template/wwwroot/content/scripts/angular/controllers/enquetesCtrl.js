app.controller('EnquetesController', function($scope, Enquetes){
    
    var page = 1;
    $scope.limitforPage = 6;
    $scope.enquetes = [];
    $scope.palavra = '';
    $scope.statusid = 0
    $scope.datainicio = '';
    $scope.datafim = '';
    $scope.QuestaoId = undefined;

    $scope.status = [
        {
            Id: 1,
            Descricao: 'Ativos'
        },
        {
            Id: 2,
            Descricao: 'Encerrados'
        }
    ];


    var request = function() {

        Enquetes._getDados(page, $scope.limitforPage, $scope.palavra, $scope.datainicio, $scope.datafim, $scope.statusid)
        .then(function successCallback(response) {

            if (response.data.length != 0) {
                $scope.enquetes = $scope.enquetes.concat(response.data);
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
        
        Enquetes._getDados(page, $scope.limitforPage, $scope.palavra, inicio, fim, $scope.statusid)
        .then(function successCallback(response) {

            $scope.enquetes = response.data;
            
        });
    };

    request();


    $scope.getValue = function(id) {
            
        $scope.QuestaoId = id;
    };

    
    $scope.postEnquete = function(item) {

        if ($scope.QuestaoId == undefined) {

            swal ( "Erro!" ,  "Por favor selecione uma opção" , "error");
        }
        else {

            Enquetes._responderEnquete($scope.QuestaoId)
            .then(
                function successCallback(response) {

                    swal ("Obrigado!" ,  "Seu voto foi cadastrado com sucesso!" , "success");
                    
                }, 
                function errorCallback(response){
                    swal ( "Desculpe!" ,  "Seu voto não pode ser registrado!" , "error");
                }
            );  

        }


    };

    

});