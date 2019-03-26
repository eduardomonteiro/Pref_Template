app.controller('galeriaFotosController', function($scope, Fotos){
    
    var page = 1;
    $scope.limitforPage = 6;
    $scope.fotos = [];
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
    
    Fotos._getCategorias()
    .then(function successCallback(response) {
        
        $scope.categorias = response.data;
        
    });


    
    Fotos._getDestaques('', 0, 0)
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
        $scope.loading = true;        
        Fotos._getDados(page, $scope.limitforPage, $scope.palavra, $scope.data, $scope.categoriaid)
        .then(function successCallback(response) {
            if (response.data.GaleriaFoto.length != 0) {
                $scope.fotos = $scope.fotos.concat(response.data.GaleriaFoto);
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
    
    request();

    $scope.searchFilter = function() {
        
        page = 1;
        $scope.disabled = false;
        
        Fotos._getDados(page, $scope.limitforPage, $scope.palavra, $scope.data, $scope.categoriaid)
        .then(function successCallback(response) {

            $scope.fotos = response.data.GaleriaFoto;
            
        });
    };

});