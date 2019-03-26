app.controller('galeriaVideosController', function($scope, Videos){
    
    var page = 1;
    $scope.limitforPage = 6;
    $scope.videos = [];
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

    Videos._getCategorias()
    .then(function successCallback(response) {
        
        $scope.categorias = response.data;
        
    });


    Videos._getDestaques('', 0, 0)
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

        Videos._getDados(page, $scope.limitforPage, $scope.palavra, $scope.data, $scope.categoriaid)
        .then(function successCallback(response) {
            $scope.loading = true; 
            if (response.data.GaleriaVideo.length != 0) {
                $scope.videos = $scope.videos.concat(response.data.GaleriaVideo);
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

            $scope.videos = response.data.GaleriaVideo;
            
        });
    };

    request();
});