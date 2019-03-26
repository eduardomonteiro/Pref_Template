app.controller('galeriaVideosInternaController', function($scope, Videos, $routeParams){
    
    var slug = $routeParams.slug;

    Videos._getDetalhes(slug)
    .then(function successCallback(response) {
        $scope.conteudo = response.data;
    });

});