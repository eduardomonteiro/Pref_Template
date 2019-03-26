app.controller('informativosController', function($scope, Informativos){
    
    var page = 1;
    $scope.limitforPage = 6;
    $scope.informativos = [];
    $scope.palavra = '';
    $scope.categoriaid = 0
    $scope.data = 0;

    $scope.datas = [
        {
            Id: 1, Descricao: 'Última Semana'
        },
        {
            Id: 2, Descricao: 'Última Mês'
        },
        {
            Id: 3, Descricao: 'Último Ano'
        }
    ];


    Informativos._getCategorias()
    .then(function successCallback(response) {
        
        $scope.categorias = response.data;
        
    });


    Informativos._getDestaques('', 0, 0)
    .then(function successCallback(response) {

        $scope.destaques = response.data;  

        $scope.slickConfig = {
            dots: true,
            autoplay: true,
            infinite: true,
            arrows: false,
            autoplaySpeed: 4000,
        };  
        
    });


    var request = function() {

        Informativos._getDados(page, $scope.limitforPage, $scope.palavra, $scope.data, $scope.categoriaid)
        .then(function successCallback(response) {

            if (response.data.Informativo.length != 0) {
                $scope.informativos = $scope.informativos.concat(response.data.Informativo);
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
        
        Informativos._getDados(page, $scope.limitforPage, $scope.palavra, $scope.data, $scope.categoriaid)
        .then(function successCallback(response) {

            $scope.informativos = response.data.Informativo;
            
        });
    };

    request();

});