app.controller('galeriaAudiosController', function($scope, Audios){
    
    var page = 1;
    $scope.limitforPage = 6;
    $scope.audios = [];
    $scope.palavra = '';
    $scope.categoriaid = 0
    $scope.data = 0;
    $scope.loading = false;

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

    Audios._getCategorias()
    .then(function successCallback(response) {
        
        $scope.categorias = response.data;
        
    });

    var request = function() {

        $scope.loading = true;
        
        Audios._getDados(page, $scope.limitforPage, $scope.palavra, $scope.data, $scope.categoriaid)
        .then(function successCallback(response) {

            if (response.data.length != 0 || response.data.length != null) {
                $scope.audios = $scope.audios.concat(response.data);
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
    }
    
    $scope.searchFilter = function() {

        page = 1;
        $scope.disabled = false;
        
        Videos._getDados(page, $scope.limitforPage, $scope.palavra, $scope.data, $scope.categoriaid)
        .then(function successCallback(response) {

            $scope.audios = response.data;
            
        });
    };

    request();

});