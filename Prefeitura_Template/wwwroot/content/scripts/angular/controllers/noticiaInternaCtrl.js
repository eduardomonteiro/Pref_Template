app.controller('NoticiasInternaController', function($scope, Noticias, $routeParams){
    
    $scope.palavra = '';
    $scope.categoriaid = 0
    $scope.data = 0;

    var slug = $routeParams.slug;

    Noticias._getDetalhes(slug)
    .then(function successCallback(response) {

        $scope.conteudo = response.data;

    });

    Noticias._getDados(1, 4, $scope.palavra, $scope.data, $scope.categoriaid)
    .then(function successCallback(response) {
        
        $scope.noticias = response.data.Noticia; 
          
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