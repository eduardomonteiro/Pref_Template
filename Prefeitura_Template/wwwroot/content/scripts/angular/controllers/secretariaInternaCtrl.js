app.controller('secretariaInternaController', function($scope, $routeParams, Secretaria, $location){

    
    var slug = $routeParams.slug;
    var gabinete = false;

    if ($location.$$path == '/o-governo/gabinete-prefeito') {
        gabinete = true;
    }


    Secretaria._getDetalhes(slug, gabinete)
    .then(function successCallback(response) {
        
        $scope.conteudo = response.data;
        
        $scope.slickConfig3 = {
            dots: true,
            infinite: true,
            arrows: false,
            slidesToShow: 4,
            slidesToScroll: 4,
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
    
    
        $scope.slickConfig2 = {
            dots: true,
            infinite: true,
            arrows: false,
            slidesToShow: 4,
            slidesToScroll: 4,
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

});