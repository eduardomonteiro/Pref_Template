app.controller('concursosInternaController', function($scope, Concursos, $routeParams){


    var slug = $routeParams.slug;

    Concursos._getDetalhes(slug)
    .then(function successCallback(response) {
        $scope.conteudo = response.data;
    });

});