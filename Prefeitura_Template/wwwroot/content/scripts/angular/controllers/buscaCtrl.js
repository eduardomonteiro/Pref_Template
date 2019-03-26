app.controller('buscaController', function($scope, $rootScope, $routeParams, $route, GetRootPage, $window, $location, Busca){

    $scope.palavra = $routeParams.palavra;

    Busca._getDados($scope.palavra)
    .then(
        function successCallback(response) {

            $scope.resultado = response.data;

            angular.forEach($scope.resultado, function(value, key){
                

                var idArea = $scope.resultado[key].AreaId;
                var urlArea = '';
                $scope.resultado[key].url = '';

                switch(idArea) {
                    case 1:
                        urlArea = 'usuarios'
                        break;
                    case 2:
                        urlArea = 'a-cidade/sobre'
                        break;
                    case 3:
                        urlArea = 'o-governo/galeria-exprefeitos'
                        break;
                    case 4:
                        urlArea = 'a-cidade/perfil-socioeconomico'
                        break;
                    case 5:
                        urlArea = 'a-cidade/atracoes-turisticas'
                        break;
                    case 6:
                        urlArea = 'prefeitura-az'
                        break;
                    case 7:
                        urlArea = 'informativo/eventos'
                        break;
                    case 8:
                        urlArea = 'informativo/noticias'
                        break;
                    case 9:
                        urlArea = 'informativo/projetos'
                        break;
                    case 10:
                        urlArea = 'multimidia/documentos'
                        break;
                    case 11:
                        urlArea = 'secretarias'
                        break;
                    case 12:
                        urlArea = 'informativo/informativos'
                        break;
                    case 13:
                        urlArea = 'multimidia/galeria-fotos'
                        break;
                    case 14:
                        urlArea = 'multimidia/galeria-audios'
                        break;
                    case 15:
                        urlArea = 'contato/fale-conosco'
                        break;
                    case 16:
                        urlArea = 'participe/faca-uma-sugestao'
                        break;
                    case 17:
                        urlArea = 'publicacoes-oficiais/licitacoes'
                        break;
                    case 18:
                        urlArea = 'publicacoes-oficiais/concursos'
                        break;
                    case 19:
                        urlArea = 'bairros'
                        break;
                    case 20:
                    urlArea = 'participe/enquetes'
                        break;
                    case 21:
                        urlArea = 'multimidia/galeria-videos'
                        break;
                    case 22:
                        urlArea = 'publicacoes-oficiais/legislacao'
                        break;
                    case 23:
                        urlArea = 'newsletter'
                        break;
                    case 24:
                        urlArea = 'a-cidade/patromonio'
                        break;
                    case 25:
                        urlArea = 'a-cidade/perfil-socioeconomico'
                        break;
                    case 26:
                        urlArea = 'contato/perguntas-frequentes'
                        break;
                    case 27:
                        urlArea = 'a-cidade/sobre'
                        break;                                                                                                        
                }

                $scope.resultado[key].url = urlArea;
                
            });
           
        }, 
        function errorCallback(response){
           
        }
    );

});