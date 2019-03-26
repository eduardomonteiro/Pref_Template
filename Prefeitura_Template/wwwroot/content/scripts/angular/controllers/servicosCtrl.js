app.controller('ServicosController', function($scope, Servicos){
    
    var page = 1;
    $scope.limitforPage = 8;
    $scope.servicos = [];
    $scope.palavra = '';
    $scope.categoriaid = {};
    $scope.categoriaid.Id = 0;


    $scope.config = {
        dots: true,
        infinite: true,
        slidesToShow: 3,
        slidesToScroll: 3,
        arrows: false,
        responsive: [
            {
                breakpoint: 768,
                settings: {
                    slidesToShow: 1,
                    slidesToScroll: 1
                }
            }
        ]   
    };

    Servicos._getCategorias()
    .then(function successCallback(response) {

        $scope.catSelect = 1;
        $scope.categoriasServico = response.data;

        var loadPins = function() {

            Servicos._getPins($scope.catSelect)
            .then(function successCallback(response) {

                $scope.markers = response.data;
                $scope.centerMap = $scope.markers[0].ServicoPin[0];

            });

        };

        $scope.changePin = function(id) {
            $scope.catSelect = id;
            loadPins();
        }

        loadPins();

    });


    var request = function() {

        Servicos._getDados(page, $scope.limitforPage, $scope.palavra, $scope.categoriaid.Id)
        .then(function successCallback(response) {

            if (response.data.length != 0) {
                $scope.servicos = $scope.servicos.concat(response.data);
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
        
        Servicos._getDados(page, $scope.limitforPage, $scope.palavra, $scope.categoriaid.Id)
        .then(function successCallback(response) {

            $scope.servicos = response.data;
            
        });
    };

    request();


});