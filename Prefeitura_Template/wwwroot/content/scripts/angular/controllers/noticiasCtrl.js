app.controller('NoticiasController', function($scope, Noticias){
    
    var page = 1;
    $scope.limitforPage = 6;
    $scope.noticias = [];
    $scope.palavra = '';
    $scope.categoriaid = 0;
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


    Noticias._getCategorias()
    .then(function successCallback(response) {
        
        $scope.categorias = response.data;
        
    });


    Noticias._getDestaques('', 0, 0)
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

        Noticias._getDados(page, $scope.limitforPage, $scope.palavra, $scope.data, $scope.categoriaid)
        .then(function successCallback(response) {

            if (response.data.Noticia.length != 0) {
                $scope.noticias = $scope.noticias.concat(response.data.Noticia);
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
        
        Noticias._getDados(page, $scope.limitforPage, $scope.palavra, $scope.data, $scope.categoriaid)
        .then(function successCallback(response) {

            $scope.noticias = response.data.Noticia;
            
        });
    };

    request();

});