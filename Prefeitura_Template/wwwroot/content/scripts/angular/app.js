var app = angular.module('app', ['ngRoute', 'slickCarousel', 'ngMap', 'ngSanitize', 'ngMask', 'cfp.hotkeys', 'ngAnimate', 'angulartics', 'angulartics.google.tagmanager']);

app.config(function($routeProvider, $locationProvider){
	// Configuração de Rota do Projeto
	$routeProvider
	.when("/", {
		templateUrl: "/views/home.html",
		title: 'Home',
		controller: 'homeController'
	})
	.when("/a-cidade/perfil-socioeconomico", {
		templateUrl: "/views/perfil-socioeconomico.html",
		title: 'Perfil Socioeconômico',
		controller: 'perfilSocioController'
	})
	.when("/a-cidade/invista-cidade", {
		templateUrl: "/views/invista-cidade.html",
		title: 'Invista na Cidade',
		controller: 'sobreController'
	})
	.when("/a-cidade/sobre", {
		templateUrl: "/views/sobre.html",
		title: 'Sobre a Cidade',
		controller: 'sobreController'
	})
	.when("/a-cidade/patromonio", {
		templateUrl: "/views/patrimonio.html",
		title: 'Patrimônio Histórico-Cultural',
		controller: 'patrimonioController'
	})
	.when("/a-cidade/atracoes-turisticas", {
		templateUrl: "/views/atracoes-turisticas.html",
		title: 'Atrações Turísticas',
		controller: 'atracoesTuristicasControlller'
	})
	.when("/o-governo/secretarias", {
		templateUrl: "/views/secretarias.html",
		title: 'Secretarias',
		controller: 'secretariaController'
	})
	.when("/o-governo/secretarias/:slug", {
		templateUrl: "/views/secretarias-interna.html",
		title: 'Secretarias',
		controller: 'secretariaInternaController'
	})
	.when("/o-governo/gabinete-prefeito", {
		templateUrl: "/views/gabinete-do-prefeito.html",
		title: 'Gabinete do Prefeito',
		controller: 'secretariaInternaController'
	})
	.when("/o-governo/galeria-exprefeitos", {
		templateUrl: "/views/galeria-exprefeitos.html",
		title: 'Galeria de Ex-prefeitos',
		controller: 'exPrefeitosController'
	})
	.when("/o-governo/projetos", {
		templateUrl: "/views/projetos.html",
		title: 'Projetos',
		controller: 'projetosController'
	})
	.when("/o-governo/projetos/:slug", {
		templateUrl: "/views/projetos-interna.html",
		title: 'Projetos',
		controller: 'projetosInternaController'
	})
	.when("/informativo/noticias", {
		templateUrl: "/views/noticias.html",
		title: 'Notícias',
		controller: 'NoticiasController'
	})
	.when("/informativo/noticias/:slug", {
		templateUrl: "/views/noticias-interna.html",
		title: 'Notícias',
		controller: 'NoticiasInternaController'
	})
	.when("/multimidia/documentos", {
		templateUrl: "/views/documentos.html",
		title: 'Documentos',
		controller: 'documentosController'
	})
	.when("/multimidia/galeria-fotos", {
		templateUrl: "/views/galeria-fotos.html",
		title: 'Galeria de Fotos',
		controller: 'galeriaFotosController'
	})
	.when("/multimidia/galeria-fotos/:slug", {
		templateUrl: "/views/galeria-fotos-interna.html",
		title: 'Galeria de Fotos',
		controller: 'galeriaFotosInternaController'
	})
	.when("/multimidia/galeria-videos", {
		templateUrl: "/views/galeria-videos.html",
		title: 'Galeria de Vídeos',
		controller: 'galeriaVideosController'
	})
	.when("/multimidia/galeria-videos/:slug", {
		templateUrl: "/views/galeria-video-interna.html",
		title: 'Galeria de Vídeos',
		controller: 'galeriaVideosInternaController'
	})
	.when("/multimidia/galeria-audios", {
		templateUrl: "/views/galeria-audios.html",
		title: 'Galeria de Áudios',
		controller: 'galeriaAudiosController'
	})
	.when("/informativo/eventos", {
		templateUrl: "/views/eventos.html",
		title: 'Eventos',
		controller: 'EventosController'
	})
	.when("/informativo/eventos/:slug", {
		templateUrl: "/views/evento-interna.html",
		title: 'Evento',
		controller: 'EventoInternaController'
	})
	.when("/informativo/informativos", {
		templateUrl: "/views/informativos.html",
		title: 'Informativos',
		controller: 'informativosController'
	})
	.when("/informativo/informativos/:slug", {
		templateUrl: "/views/informativos-interna.html",
		title: 'Informativos',
		controller: 'InformativoInternaController'
	})
	.when("/servicos", {
		templateUrl: "/views/servicos.html",
		title: 'Serviços',
		controller: 'ServicosController'
	})
	.when("/servicos/:slug", {
		templateUrl: "/views/servicos-interna.html",
		title: 'Serviços',
		controller: 'ServicosInternaController'
	})
	.when("/participe/enquetes", {
		templateUrl: "/views/enquetes.html",
		title: 'Enquetes',
		controller: 'EnquetesController'
	})
	.when("/participe/faca-uma-sugestao", {
		templateUrl: "/views/faca-sugestao.html",
		title: 'Faça uma Sugestão',
		controller: 'sugestaoController'
	})
	.when("/busca/:palavra", {
		templateUrl: "/views/busca.html",
		title: 'Busca',
		controller: 'buscaController'
	})
	.when("/transparencia", {
		templateUrl: "/views/transparencia.html",
		title: 'Transparência'
	})
	.when("/publicacoes-oficiais/concursos", {
		templateUrl: "/views/concursos.html",
		title: 'Concursos',
		controller: 'concursosController'
	})
	.when("/prefeitura-az", {
		templateUrl: "/views/prefeitura-az.html",
		title: 'Prefeitura de A a Z',
		controller: 'prefeituraAZController'
	})
	.when("/publicacoes-oficiais/concursos/:slug", {
		templateUrl: "/views/concursos-interna.html",
		title: 'Concursos',
		controller: 'concursosInternaController'
	})
	.when("/publicacoes-oficiais/legislacao", {
		templateUrl: "/views/legislacao.html",
		title: 'Legislação',
		controller: 'LegislacaoController'
	})
	.when("/publicacoes-oficiais/licitacoes", {
		templateUrl: "/views/licitacoes.html",
		title: 'Licitações',
		controller: 'LicitacoesController'
	})
	.when("/publicacoes-oficiais/licitacoes/:slug", {
		templateUrl: "/views/licitacoes-interna.html",
		title: 'Licitações',
		controller: 'licitacaoInternaController'
	})
	.when("/contato/fale-conosco", {
		templateUrl: "/views/fale-conosco.html",
		title: 'Fale Conosco',
		controller: 'FaleConoscoController'
	})
	.when("/contato/perguntas-frequentes", {
		templateUrl: "/views/perguntas-frequentes.html",
		title: 'Perguntas Frequentes',
		controller: 'perguntasController'
	})
	.when("/acessibilidade", {
		templateUrl: "/views/acessibilidade.html",
		title: 'Acessibilidade'
	})
	.otherwise({
		redirectTo: '/error',
		templateUrl: "views/error.html",
		controller: 'appController',
		title: 'Error 404'
	});
})
.run(['$rootScope', '$route', function($rootScope, $route) {
    $rootScope.$on('$routeChangeSuccess', function() {
		document.title = $rootScope.nameProject + ' - ' + $route.current.title;
    });
}]); 


app.filter('createDate', function(){
	return function(input) {
		
		var returnText = '';
		var splitDate = input.split('/');
	
		switch(splitDate[1]) {
            case '01':
                returnText = 'JAN'
                break;
            case '02':
                returnText = 'FEV'
                break;
            case '03':
                returnText = 'MAR'
                break;
            case '04':
                returnText = 'ABR'
                break;
            case '05':
                returnText = 'MAI'
                break;
            case '06':
                returnText = 'JUN'
				break;
			case '07':
                returnText = 'JUL'
				break;
			case '08':
                returnText = 'AGO'
				break;
			case '09':
                returnText = 'SET'
				break;
			case '10':
                returnText = 'OUT'
				break;
			case '11':
                returnText = 'NOV'
				break;
			case '12':
                returnText = 'NOV'
                break;						                  
            default:
                returnText
		}
		
		input = splitDate[0]  + ' ' + returnText;

		return input;

	};
});

app.config(['$analyticsProvider', function ($analyticsProvider) {
	
	$analyticsProvider.settings.ga = {
		userId: 'UA-24788654-2'
	};
}]);