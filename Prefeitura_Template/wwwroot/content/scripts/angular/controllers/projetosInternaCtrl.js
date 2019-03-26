app.controller('projetosInternaController', function($scope, Projetos, $routeParams){

    var slug = $routeParams.slug;
    
    Projetos._getDetalhes(slug)
    .then(function successCallback(response) {

        $scope.conteudo = response.data;

        $scope.slickConfig2 = {
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

    


});