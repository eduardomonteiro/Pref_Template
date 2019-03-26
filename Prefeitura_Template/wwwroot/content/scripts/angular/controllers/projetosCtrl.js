app.controller('projetosController', function($scope, Projetos){

    var page = 1;
    $scope.limitforPage = 8;
    $scope.lista = [];
    $scope.loading = false;


    var request = function() {

        $scope.loading = true;

        Projetos._getDados(page, $scope.limitforPage)
        .then(function successCallback(response) {

            if (response.data.length != 0) {
                $scope.lista = $scope.lista.concat(response.data);
            }
            else {
                $scope.disabled = true;
            }  
               
            $scope.loading = false;       
        });

    };

    $scope.loadMore = function() {
        page++;
        request();
    };

    request();
    
});