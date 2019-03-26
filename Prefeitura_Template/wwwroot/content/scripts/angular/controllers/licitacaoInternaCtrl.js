app.controller('licitacaoInternaController', function($scope, Licitacoes, $routeParams){


    var slug = $routeParams.slug;

    Licitacoes._getDetalhes(slug)
    .then(function successCallback(response) {
        $scope.conteudo = response.data;
    });

});