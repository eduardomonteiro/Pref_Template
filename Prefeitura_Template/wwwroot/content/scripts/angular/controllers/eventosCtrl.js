app.controller('EventosController', function($scope, Eventos){
    
    var page = 1;
    $scope.limitforPage = 6;
    $scope.eventos = [];
    $scope.palavra = '';
    $scope.categoriaid = 0
    $scope.data = 0;

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

    $scope.slickConfig = {
        dots: true,
        autoplay: true,
        infinite: true,
        arrows: false,
        autoplaySpeed: 4000,
    };

    Eventos._getCategorias()
    .then(function successCallback(response) {
        
        $scope.categorias = response.data;
        
    });

    var request = function() {

        Eventos._getDados(page, $scope.limitforPage, $scope.palavra, $scope.data, $scope.categoriaid)
        .then(function successCallback(response) {

            if (response.data.length != 0) {
                $scope.eventos = $scope.eventos.concat(response.data);
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
        
        Eventos._getDados(page, $scope.limitforPage, $scope.palavra, $scope.data, $scope.categoriaid)
        .then(function successCallback(response) {

            $scope.eventos = response.data;
            
        });
    };

    request();

});