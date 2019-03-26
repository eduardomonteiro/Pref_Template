app.controller('EventoInternaController', function($scope, Eventos, $routeParams){
    
    var slug = $routeParams.slug;

    Eventos._getDetalhes(slug)
    .then(function successCallback(response) {
        $scope.conteudo = response.data;
    });

    Eventos._getDados(1, 4, '', 0, 0)
    .then(function successCallback(response) {
        
        $scope.eventos = response.data; 
          
    });


    $scope.slickConfig = {
        dots: false,
        infinite: true,
        arrows: true,
        nextArrow: '<button class="fa arrow fa-angle-right"></button>',
        prevArrow: '<button class="fa arrow fa-angle-left"></button>',
        autoplaySpeed: 4000,
    };

});