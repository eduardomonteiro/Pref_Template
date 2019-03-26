app.controller('documentosController', function($scope, Documentos){
    
    var page = 1;
    $scope.limitforPage = 6;
    $scope.documentos = [];
    $scope.palavra = '';
    $scope.categoriaid = 0
    $scope.data = 0;


    Documentos._getCategorias()
    .then(function successCallback(response) {
        
        $scope.categorias = response.data;
        
    });

    $scope.datas = [
        {
            Id: 1, Descricao: 'Última Semana'
        },
        {
            Id: 2, Descricao: 'Último Mês'
        },
        {
            Id: 3, Descricao: 'Último Ano'
        }
    ];


    var request = function() {

        Documentos._getDados(page, $scope.limitforPage, $scope.palavra, $scope.data, $scope.categoriaid.Id)
        .then(function successCallback(response) {
            if (response.data.length != 0) {
                $scope.documentos = $scope.documentos.concat(response.data);
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
        
        Documentos._getDados(page, $scope.limitforPage, $scope.palavra, $scope.data, $scope.categoriaid.Id)
        .then(function successCallback(response) {

            $scope.documentos = response.data;
            
        });
    };
    
    request();

});