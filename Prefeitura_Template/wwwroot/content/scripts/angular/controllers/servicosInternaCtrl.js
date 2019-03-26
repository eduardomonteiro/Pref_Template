app.controller('ServicosInternaController', function($scope, Servicos, $routeParams){
    
    var slug = $routeParams.slug;
    
    Servicos._getDetalhes(slug)
    .then(function successCallback(response) {
        $scope.conteudo = response.data;
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
    });

   
        
});