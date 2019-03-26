app.controller('InformativoInternaController', function($scope, Informativos, $routeParams){
    
    $scope.palavra = '';
    $scope.categoriaid = 0
    $scope.data = 0;

    var slug = $routeParams.slug;

    Informativos._getDetalhes(slug)
    .then(function successCallback(response) {
        $scope.conteudo = response.data;
    });

    Informativos._getDados(1, 4, $scope.palavra, $scope.data, $scope.categoriaid)
    .then(function successCallback(response) {
        
        $scope.informativos = response.data.Informativo; 
          
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