app.controller('galeriaFotosInternaController', function($scope, Fotos, $routeParams){

    var slug = $routeParams.slug;

    Fotos._getDetalhes(slug)
    .then(function successCallback(response) {
        $scope.conteudo = response.data;
        
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