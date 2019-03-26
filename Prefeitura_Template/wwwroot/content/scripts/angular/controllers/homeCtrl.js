app.controller('homeController', function($scope, Noticias, Secretaria, Eventos, Servicos, Videos, NgMap){
    

    Noticias._getDados(1, 3, '', 0, 0)
    .then(function successCallback(response) {
        
        $scope.noticias = response.data.Noticia;
        $scope.destaques = response.data.NoticiaDestaque;  
        $scope.slickConfig = {
            dots: true,
            autoplay: true,
            infinite: true,
            arrows: true,
            slidesToShow: 1,
            slidesToScroll: 1,
            nextArrow: '<button class="fa arrow fa-angle-right"></button>',
            prevArrow: '<button class="fa arrow fa-angle-left"></button>',
            autoplaySpeed: 4000,
        };  

    });

    Secretaria._getDados()
    .then(function successCallback(response) {
        
        $scope.secretarias = response.data;
        $scope.slickConfig3 = {
            dots: true,
            infinite: true,
            arrows: false,
            slidesToShow: 3,
            slidesToScroll: 3,
            arrows: true,
            nextArrow: '<button class="fa arrow fa-angle-right"></button>',
            prevArrow: '<button class="fa arrow fa-angle-left"></button>',
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
          
    });

    Eventos._getDados(1, 4, '', 0, 0)
    .then(function successCallback(response) {
        $scope.eventos = response.data;
    });


    Servicos._getDestaques('', 0)
    .then(function successCallback(response) {
        
        $scope.servicos = response.data; 
        $scope.slickConfig2 = {
            dots: true,
            infinite: true,
            slidesToShow: 5,
            slidesToScroll: 5,
            arrows: true,
            nextArrow: '<button class="fa arrow fa-angle-right"></button>',
            prevArrow: '<button class="fa arrow fa-angle-left"></button>',
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

    });

    Servicos._getCategorias()
    .then(function successCallback(response) {

        $scope.catSelect = 1;
        $scope.categoriasServico = response.data;

        var loadPins = function() {

            $scope.markers = [];
            
            Servicos._getPins($scope.catSelect)
            .then(function successCallback(response) {

                $scope.markers = response.data;
                $scope.centerMap = $scope.markers[0].ServicoPin[0];

            });

        };

        $scope.changePin = function(id) {

            $scope.markers = [];
            $scope.catSelect = id;
            loadPins();
        }

        loadPins();

    });

    Videos._getDados(1, 2, '', 0, 0)
    .then(function successCallback(response) {
        
        $scope.videos = response.data.GaleriaVideo; 

    });



});