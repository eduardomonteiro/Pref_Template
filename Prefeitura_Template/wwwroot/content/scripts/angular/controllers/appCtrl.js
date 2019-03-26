app.controller('appCtrl', function($scope, $rootScope, $route, GetRootPage, $window, $location, Servicos, ClimaTempo, NewsLetter, Enquetes, hotkeys, FaleConosco){
    
    // Título a ser mudado conforme o projeto
    $rootScope.nameProject = 'Prefeitura de Pinheiral';
    $scope.endereco = 'R. Justino Ribeiro, 228 - Centro, Pinheiral - RJ, 27197-00';

    FaleConosco._returnInfo()
    .then(function successCallback(response) {
        $scope.telefone = response.data.GabineteTelefone;
    });

    $scope.menu = false;

    $scope.clickMenu = function() {

        $scope.menu = !$scope.menu;
    };

    $rootScope.$on('$routeChangeStart', function() {
        $scope.menu = false;
    });

    $scope.scrollPage = function(div) {
        
        var position = $(div).offset().top;

        $('html').animate({
            scrollTop: position
        });
        
    }
        
    hotkeys.add({
        combo: 'alt+1',
        callback: function() {

            scrollPage('#conteudo');
        }
    });

    hotkeys.add({
        combo: 'alt+shift+1',
        callback: function() {

            $scope.scrollPage('#conteudo');
        }
    });

    hotkeys.add({
        combo: 'alt+2',
        callback: function() {

            $scope.scrollPage('#menu');
        }
    });

    hotkeys.add({
        combo: 'alt+shift+2',
        callback: function() {

            $scope.scrollPage('#menu');
        }
    });

    hotkeys.add({
        combo: 'alt+3',
        callback: function() {

            $scope.scrollPage('#topo');
            $('#busca').focus();
        }
    });

    hotkeys.add({
        combo: 'alt+shift+3',
        callback: function() {

            $scope.scrollPage('#topo');
            $('#busca').focus();
        }
    });

    hotkeys.add({
        combo: 'alt+4',
        callback: function() {

            $scope.scrollPage('#rodape');
        }
    });

    hotkeys.add({
        combo: 'alt+shift+4',
        callback: function() {

            $scope.scrollPage('#rodape');
        }
    });



    $scope.contrast = false;     
    $scope.addContrast = function() {
        $scope.contrast = !$scope.contrast;
    };

    $scope.printPage = function() {
        $window.print();  
    };

    $rootScope.$on('$routeChangeSuccess', function() {

        $scope.linkPage = $location.$$absUrl;
        var routeVerify = $route.current.$$route.originalPath;
        $scope.rootArea = GetRootPage._transform(routeVerify);
        $scope.titlePage = $route.current.$$route.title;

    });

    


    $scope.postNewsletter = function(user) {

        NewsLetter._post(user)
        .then(
            function successCallback(response) {
    
                swal ( "Obrigado!" ,  "Seu cadastro foi efetuado com sucesso!" , "success");
            }, 
            function errorCallback(response){
                swal ( "Desculpe!" ,  "Seu cadastro não pode ser efetuado!" , "error");
            }
        );

    };


    ClimaTempo._getDados()
    .then(function successCallback(response) {
        
        $scope.clima = response.data.previsao[0];


        $scope.callClass = function(clima) {

            var ico = '';

            switch(clima) {
                
                case 'Encoberto com Chuvas Isoladas':
                    ico = 'wi-rain'
                break;
                case 'Chuvas Isoladas':
                    ico = 'wi-rain'
                break;
                case 'Chuva':
                    ico = 'wi-rain'
                break;
                case 'Instável':
                    ico = 'wi-day-rain'
                break;
                case 'Instável':
                    ico = 'wi-day-rain-mix'
                break;
                case 'Chuva pela Manhã':
                    ico = 'wi-day-hail'
                break;
                case 'Chuva a Noite':
                    ico = 'wi-night-hail'
                break;
                case 'Pancadas de Chuva a Tarde':
                    ico = 'wi-day-rain-mix'
                break;
                case 'Pancadas de Chuva pela Manhã':
                    ico = 'wi-day-rain-mix'
                break;
                case 'Nublado e Pancadas de Chuva':
                    ico = 'wi-fog'
                break;
                case 'Pancadas de Chuva':
                    ico = 'wi-rain'
                break;
                case 'Parcialmente Nublado':
                    ico = 'wi-cloudy'
                break;
                case 'Chuvisco':
                    ico = 'wi-rain'
                break;
                case 'Chuvoso':
                    ico = 'wi-rain'
                break;
                case 'Tempestade':
                    ico = 'wi-storm-showers'
                break;
                case 'Predomínio de Sol':
                    ico = 'wi-day-sunny'
                break;
                case 'Encoberto':
                    ico = 'wi-fog'
                break;
                case 'Nublado':
                    ico = 'wi-fog'
                break;
                case 'Céu Claro':
                    ico = 'wi-cloud'
                break;
                case 'Nevoeiro':
                    ico = 'wi-fog'
                break;
                case 'Geada':
                    ico = 'wi-snow'
                break;
                case 'Neve':
                    ico = 'wi-snow'
                break;
                case 'Não Definido':
                    ico = 'wi-cloud'
                break;
                case 'Possibilidade de Chuva':
                    ico = 'wi-night-alt-rain-mix'
                break;
                case 'Possibilidade de Chuva pela Manhã':
                    ico = 'wi-day-sleet'
                break;
                case 'Possibilidade de Chuva a Tarde':
                    ico = 'wi-day-sleet'
                break;
                case 'Possibilidade de Chuva a Noite':
                    ico = 'wi-night-sleet'
                break;
                case 'Nublado com Pancadas a Tarde':
                    ico = 'wi-night-alt-rain-mix'
                break;
                case 'Nublado com Pancadas a Noite':
                    ico = 'wi-night-alt-rain-mix'
                break;
                case 'Nublado com Possibilidade de Chuva a Noite':
                    ico = 'wi-night-alt-rain-mix'
                break;
                case 'Nublado com Possibilidade de Chuva a Tarde':
                    ico = 'wi-day-sleet'
                break;
                case 'Nublado com Possibilidade de Chuva pela Manhã':
                    ico = 'wi-day-sleet'
                break;
                case 'Nublado com Pancadas pela Manhã':
                    ico = 'wi-day-rain'
                break;
                case 'Nublado com Possibilidade de Chuva':
                    ico = 'wi-day-sleet'
                break;
                case 'Variação de Nebulosidade':
                    ico = 'wi-day-fog'
                break;
                case 'Chuva a Tarde':
                    ico = 'wi-day-rain-mix'
                break;
                case 'Possibilidade de Chuva a Noite':
                    ico = 'wi-night-alt-rain-mix'
                break;
                case 'Possibilidade de Pancadas de Chuva a Noite':
                    ico = 'wi-night-alt-rain-mix'
                break;
                case 'Possibilidade de Pancadas de Chuva a Tarde':
                    ico = 'wi-day-sleet'
                break;
                default:
                    ico = 'wi-sun'
            }
    
            return ico;
        };



    });

    $scope.searchForm = function(palavra) {
        window.location = '#!/busca/' + palavra
    };


    Enquetes._getDados(1, 1, '', 0, 0, 1)
    .then(function successCallback(response) {

        $scope.enquete = response.data;
    });

    $scope.getValue = function(id) {
        
        $scope.QuestaoId = id;
    };

    $scope.postEnquete = function(item) {
        
        if ($scope.QuestaoId == undefined) {

            swal ( "Erro!" ,  "Por favor selecione uma opção" , "error");
        }
        else {

            Enquetes._responderEnquete($scope.QuestaoId)
            .then(
                function successCallback(response) {

                    swal ("Obrigado!" ,  "Seu voto foi cadastrado com sucesso!" , "success");
                    
                }, 
                function errorCallback(response){
                    swal ( "Desculpe!" ,  "Seu voto não pode ser registrado!" , "error");
                }
            );  

        }


    };

    

});
