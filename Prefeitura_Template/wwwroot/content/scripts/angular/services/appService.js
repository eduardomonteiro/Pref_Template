var baseUrl =  'http://pinheiral.sili.com.br/API/Api/';

app.factory('ExPrefeitos', ['$http', function ($http) {
    
    var obj =  {};

    obj._getDados = function() {

        return  $http({
            method: 'GET',
            url: baseUrl + 'ExPrefeitos',
            headers: { 'content-type': 'application/json, text/json' },
        });

    };

    return obj;

}])
.factory('Servicos', ['$http', function ($http) {
    
    var obj =  {};

    obj._getDados = function(pagenumber, pagesize, palavra, categoriaid) {

        return  $http({
            method: 'GET',
            url: baseUrl + 'Servicos?PageNumber=' + pagenumber + '&PageSize=' + pagesize + '&Palavra=' + palavra + '&CategoriaId=' + categoriaid,
            headers: { 'content-type': 'application/json, text/json' },
        });

    };

    obj._getDestaques = function(palavra, categoriaid) {
        
        return  $http({
            method: 'GET',
            url: baseUrl + 'ServicoDestaques?Palavra=' + palavra + '&CategoriaId=' + categoriaid,
            headers: { 'content-type': 'application/json, text/json'}
        });

    };

    obj._getDetalhes = function(slug) {

        return  $http({
            method: 'GET',
            url: baseUrl + 'Servico?Slug=' + slug,
            headers: { 'content-type': 'application/json, text/json' },
        });
    };

    obj._getPins = function(categoriaid) {

        return  $http({
            method: 'GET',
            url: baseUrl + 'ServicosPins?CategoriaId=' + categoriaid,
            headers: { 'content-type': 'application/json, text/json' },
        });
    };

    obj._getCategorias = function(pagenumber, pagesize) {
        
        return  $http({
            method: 'GET',
            url: baseUrl + 'CategoriasServicos',
            headers: { 'content-type': 'application/json, text/json' },
        });

    };

    return obj;

}])
.factory('Enquetes', ['$http', function ($http) {
    
    var obj =  {};

    obj._getDados = function(pagenumber, pagesize, palavra, datainicio, datafim, status) {

        return  $http({
            method: 'GET',
            url: baseUrl + 'Enquetes?PageNumber=' + pagenumber + '&PageSize=' + pagesize + '&Palavra=' + palavra + '&DataInicio=' + datainicio + '&DataFim=' + datafim + '&Status=' + status,
            headers: { 'content-type': 'application/json, text/json' },
        });

    };

    obj._responderEnquete = function(id) {

        return  $http({
            method: 'POST',
            url: baseUrl + 'RespoderEnquete?EnqueteOpcaoId=' + id,
            headers: { 'content-type': 'application/json, text/json' }
        });
    };

    return obj;

}])
.factory('PerguntasFrequentes', ['$http', function ($http) {
    
    var obj =  {};

    obj._getDados = function(pagenumber, pagesize, palavra, categoriaid) {

        return  $http({
            method: 'GET',
            url: baseUrl + 'PerguntasFrequentes',
            headers: { 'content-type': 'application/json, text/json' },
        });

    };

    obj._getCategorias = function(pagenumber, pagesize) {
        
        return  $http({
            method: 'GET',
            url: baseUrl + 'CategoriasPerguntasFrequentes',
            headers: { 'content-type': 'application/json, text/json' },
        });

    };

    return obj;

}])
.factory('Noticias', ['$http', function ($http) {
    
    var obj =  {};

    obj._getDados = function(pagenumber, pagesize, palavra, data, categoriaid) {

        return  $http({
            method: 'GET',
            url: baseUrl + 'Noticias?PageNumber=' + pagenumber + '&PageSize=' + pagesize + '&Palavra=' + palavra + '&Data=' + data + '&CategoriaId=' + categoriaid,
            headers: { 'content-type': 'application/json, text/json' },
        });

    };

    obj._getDestaques = function(palavra, data, categoriaid) {
        
        return  $http({
            method: 'GET',
            url: baseUrl + 'NoticiaDestaques?Palavra=' + palavra + '&Data=' + data + '&CategoriaId=' + categoriaid,
            headers: { 'content-type': 'application/json, text/json' },
        });

    };

    obj._getDetalhes = function(slug) {

        return  $http({
            method: 'GET',
            url: baseUrl + 'Noticia?Slug=' + slug,
            headers: { 'content-type': 'application/json, text/json' },
        });
    };


    obj._getCategorias = function() {
        
        return  $http({
            method: 'GET',
            url: baseUrl + 'CategoriasNoticias',
            headers: { 'content-type': 'application/json, text/json' },
        });

    };

    return obj;

}])
.factory('Informativos', ['$http', function ($http) {
    
    var obj =  {};

    obj._getDados = function(pagenumber, pagesize, palavra, data, categoriaid) {

        return  $http({
            method: 'GET',
            url: baseUrl + 'Informativos?PageNumber=' + pagenumber + '&PageSize=' + pagesize + '&Palavra=' + palavra + '&Data=' + data + '&CategoriaId=' + categoriaid,
            headers: { 'content-type': 'application/json, text/json' },
        });

    };

    obj._getDestaques = function(palavra, data, categoriaid) {
        
        return  $http({
            method: 'GET',
            url: baseUrl + 'InformativoDestaques?Palavra=' + palavra + '&Data=' + data + '&CategoriaId=' + categoriaid,
            headers: { 'content-type': 'application/json, text/json' },
        });

    };

    obj._getDetalhes = function(slug) {

        return  $http({
            method: 'GET',
            url: baseUrl + 'Informativo?Slug=' + slug,
            headers: { 'content-type': 'application/json, text/json' },
        });
    };


    obj._getCategorias = function() {
        
        return  $http({
            method: 'GET',
            url: baseUrl + 'CategoriasInformativos',
            headers: { 'content-type': 'application/json, text/json' },
        });

    };

    return obj;

}])
.factory('Videos', ['$http', function ($http) {
    
    var obj =  {};

    obj._getDados = function(pagenumber, pagesize, palavra, data, categoriaid) {

        return  $http({
            method: 'GET',
            url: baseUrl + 'GaleriaVideos?PageNumber=' + pagenumber + '&PageSize=' + pagesize + '&Palavra=' + palavra + '&Data=' + data + '&CategoriaId=' + categoriaid,
            headers: { 'content-type': 'application/json, text/json' },
        });

    };

    obj._getDestaques = function(palavra, data, categoriaid) {
        
        return  $http({
            method: 'GET',
            url: baseUrl + 'GaleriaVideosDestaques?Palavra=' + palavra + '&Data=' + data + '&CategoriaId=' + categoriaid,
            headers: { 'content-type': 'application/json, text/json' },
        });

    };

    obj._getDetalhes = function(slug) {

        return  $http({
            method: 'GET',
            url: baseUrl + 'GaleriaVideo?Slug=' + slug,
            headers: { 'content-type': 'application/json, text/json' },
        });
    };

    obj._getCategorias = function(pagenumber, pagesize) {
        
        return  $http({
            method: 'GET',
            url: baseUrl + 'CategoriasGaleriaVideos',
            headers: { 'content-type': 'application/json, text/json' },
        });

    };

    return obj;

}])
.factory('Fotos', ['$http', function ($http) {
    
    var obj =  {};

    obj._getDados = function(pagenumber, pagesize) {

        return  $http({
            method: 'GET',
            url: baseUrl + 'GaleriaFotos?PageNumber=' + pagenumber + '&PageSize=' + pagesize,
            headers: { 'content-type': 'application/json, text/json' },
        });

    };

    obj._getDestaques = function(palavra, data, categoriaid) {
        
        return  $http({
            method: 'GET',
            url: baseUrl + 'GaleriaFotosDestaques?Palavra=' + palavra + '&Data=' + data + '&CategoriaId=' + categoriaid,
            headers: { 'content-type': 'application/json, text/json' },
        });

    };

    obj._getDetalhes = function(slug) {

        return  $http({
            method: 'GET',
            url: baseUrl + 'GaleriaFoto?Slug=' + slug,
            headers: { 'content-type': 'application/json, text/json' },
        });
    };

    obj._getCategorias = function(pagenumber, pagesize) {
        
        return  $http({
            method: 'GET',
            url: baseUrl + 'CategoriasGaleriaFotos',
            headers: { 'content-type': 'application/json, text/json' },
        });

    };

    return obj;

}])
.factory('Documentos', ['$http', function ($http) {
    
    var obj =  {};

    obj._getDados = function(pagenumber, pagesize, palavra, data, categoriaid) {

        return  $http({
            method: 'GET',
            url: baseUrl + 'Documentos?PageNumber=' + pagenumber + '&PageSize=' + pagesize + '&Palavra=' + palavra + '&Data=' + data + '&CategoriaId=' + categoriaid,
            headers: { 'content-type': 'application/json, text/json' },
        });

    };

    obj._getCategorias = function(pagenumber, pagesize) {
        
        return  $http({
            method: 'GET',
            url: baseUrl + 'CategoriasDocumentos',
            headers: { 'content-type': 'application/json, text/json' },
        });

    };

    return obj;

}])
.factory('Audios', ['$http', function ($http) {
    
    var obj =  {};

    obj._getDados = function(pagenumber, pagesize, palavra, data, categoriaid) {

        return  $http({
            method: 'GET',
            url: baseUrl + 'GaleriaAudios?PageNumber=' + pagenumber + '&PageSize=' + pagesize + '&Palavra=' + palavra + '&Data=' + data + '&CategoriaId=' + categoriaid,
            headers: { 'content-type': 'application/json, text/json' },
        });

    };

    obj._getCategorias = function(pagenumber, pagesize) {
        
        return  $http({
            method: 'GET',
            url: baseUrl + 'CategoriasGaleriaAudios',
            headers: { 'content-type': 'application/json, text/json' },
        });

    };

    return obj;

}])
.factory('Eventos', ['$http', function ($http) {
    
    var obj =  {};

    obj._getDados = function(pagenumber, pagesize, palavra, data, categoriaid) {

        return  $http({
            method: 'GET',
            url: baseUrl + 'Eventos?PageNumber=' + pagenumber + '&PageSize=' + pagesize + '&Palavra=' + palavra + '&Data=' + data + '&CategoriaId=' + categoriaid,
            headers: { 'content-type': 'application/json, text/json' },
        });

    };

    obj._getDetalhes = function(slug) {

        return  $http({
            method: 'GET',
            url: baseUrl + 'Evento?Slug=' + slug,
            headers: { 'content-type': 'application/json, text/json' },
        });
    };

    obj._getCategorias = function(slug) {
        
        return  $http({
            method: 'GET',
            url: baseUrl + 'CategoriasEventos',
            headers: { 'content-type': 'application/json, text/json' },
        });
    };

    return obj;

}])
.factory('Concursos', ['$http', function ($http) {
    
    var obj =  {};

    obj._getDados = function(pagenumber, pagesize, palavra, datainicio, datafim, statusid) {

        return  $http({
            method: 'GET',
            url: baseUrl + 'Concursos?PageNumber=' + pagenumber + '&PageSize=' + pagesize + '&Palavra=' + palavra + '&DataInicio=' + datainicio + '&DataFim=' + datafim+ '&StatusId=' + statusid,
            headers: { 'content-type': 'application/json, text/json' },
        });

    };

    obj._getDetalhes = function(slug) {

        return  $http({
            method: 'GET',
            url: baseUrl + 'Concurso?Slug=' + slug,
            headers: { 'content-type': 'application/json, text/json' },
        });
    };

    obj._getStatus = function() {
        
        return  $http({
            method: 'GET',
            url: baseUrl + 'StatusConcursos',
            headers: { 'content-type': 'application/json, text/json' },
        });
    };

    obj._getModalidades = function() {
        
        return  $http({
            method: 'GET',
            url: baseUrl + 'ModalidadesConcursos',
            headers: { 'content-type': 'application/json, text/json' },
        });
    };

    return obj;

}])
.factory('Licitacoes', ['$http', function ($http) {
    
    var obj =  {};

    obj._getDados = function(pagenumber, pagesize, palavra, datainicio, datafim, statusid) {

        return  $http({
            method: 'GET',
            url: baseUrl + 'Licitacoes?PageNumber=' + pagenumber + '&PageSize=' + pagesize + '&Palavra=' + palavra + '&DataInicio=' + datainicio + '&DataFim=' + datafim+ '&StatusId=' + statusid,
            headers: { 'content-type': 'application/json, text/json' },
        });

    };

    obj._getDetalhes = function(slug) {

        return  $http({
            method: 'GET',
            url: baseUrl + 'Licitacao?Slug=' + slug,
            headers: { 'content-type': 'application/json, text/json' },
        });
    };

    obj._getStatus = function() {
        
        return  $http({
            method: 'GET',
            url: baseUrl + 'StatusLicitacoes',
            headers: { 'content-type': 'application/json, text/json' },
        });
    };

    obj._getModalidades = function() {
        
        return  $http({
            method: 'GET',
            url: baseUrl + 'ModalidadesLicitacoes',
            headers: { 'content-type': 'application/json, text/json' },
        });
    };

    return obj;

}])
.factory('Legislacao', ['$http', function ($http) {
    
    var obj =  {};

    obj._getDados = function(pagenumber, pagesize, palavra, datainicio, datafim, categoriaid) {

        return  $http({
            method: 'GET',
            url: baseUrl + 'Legislacoes?PageNumber=' + pagenumber + '&PageSize=' + pagesize + '&Palavra=' + palavra + '&DataInicio=' + datainicio + '&DataFim=' + datafim+ '&StatusId=' + categoriaid,
            headers: { 'content-type': 'application/json, text/json' },
        });

    };

    obj._getCategorias = function() {
        
        return  $http({
            method: 'GET',
            url: baseUrl + 'CategoriasLegislacao',
            headers: { 'content-type': 'application/json, text/json' },
        });
    };

    return obj;

}])
.factory('FaleConosco', ['$http', function ($http) {
    
    var obj =  {};

    obj._postUser = function(user) {

        return  $http({
            method: 'POST',
            data: user,
            url: baseUrl + 'CadastrarFaleConosco',
            headers: { 'content-type': 'application/json' }
        });

    };

    obj._returnAssunto = function() {
        
        return  $http({
            method: 'GET',
            url: baseUrl + 'FaleConoscoAssunto',

            headers: { 'content-type': 'application/json, text/json' },
        });

    };
 
    obj._returnBairros = function() {

        return  $http({
            method: 'GET',
            url: baseUrl + 'Bairros',

            headers: { 'content-type': 'application/json, text/json' },
        });
    };

    obj._returnInfo = function() {
        
        return  $http({
            method: 'GET',
            url: baseUrl + 'FaleConoscoContato',
            headers: { 'content-type': 'application/json, text/json' },
        });
    };

    return obj;

}])
.factory('Projetos', ['$http', function ($http) {
    
    var obj =  {};

    obj._getDados = function(pagenumber, pagesize) {

        return  $http({
            method: 'GET',
            url: baseUrl + 'Projetos?PageNumber=' + pagenumber + '&PageSize=' + pagesize,
            headers: { 'content-type': 'application/json, text/json' },
        });

    };

    obj._getDetalhes = function(slug) {

        return  $http({
            method: 'GET',
            url: baseUrl + 'Projeto?Slug=' + slug,
            headers: { 'content-type': 'application/json, text/json' },
        });
    };

    return obj;

}])
.factory('ClimaTempo', ['$http', function ($http) {
    
    var obj =  {};

    obj._getDados = function() {

        return  $http({
            method: 'GET',
            url: baseUrl + 'PrevisaoTempo',
            headers: { 'content-type': 'application/json, text/json' },
        });

    };

    return obj;

}])
.factory('Secretaria', ['$http', function ($http) {
    
    var obj =  {};

    obj._getDados = function(pagenumber, pagesize) {

        return  $http({
            method: 'GET',
            url: baseUrl + 'Secretarias',
            headers: { 'content-type': 'application/json, text/json' },
        });

    };

    obj._getDetalhes = function(slug, gabinete) {

        return  $http({
            method: 'GET',
            url: baseUrl + 'Secretaria?Slug=' + slug + '&Gabinete=' + gabinete,
            headers: { 'content-type': 'application/json, text/json' },
        });
    };

    return obj;

}])
.factory('NewsLetter', ['$http', function ($http) {
    
    var obj =  {};

    obj._post = function(user) {

        return  $http({
            method: 'POST',
            data: user,
            url: baseUrl + 'CadastrarNewsLetter',
            headers: { 'content-type': 'application/json' },
        });

    };

    return obj;

}])
.factory('Cidade', ['$http', function ($http) {
    
    var obj =  {};

    obj._getDados = function() {

        return  $http({
            method: 'GET',
            url: baseUrl + 'Cidade',
            headers: { 'content-type': 'application/json, text/json' },
        });

    };

    return obj;

}])
.factory('PerfilSocioEconomico', ['$http', function ($http) {
    
    var obj =  {};

    obj._getDados = function() {

        return  $http({
            method: 'GET',
            url: baseUrl + 'PerfilSocioEconomico',
            headers: { 'content-type': 'application/json, text/json' },
        });

    };

    obj._getCategorias = function() {
        
        return  $http({
            method: 'GET',
            url: baseUrl + 'CategoriasPerfilSocioEconomico',
            headers: { 'content-type': 'application/json, text/json' },
        });

    };
    

    return obj;

}])
.factory('Turismo', ['$http', function ($http) {
    
    var obj =  {};

    obj._getDados = function() {

        return  $http({
            method: 'GET',
            url: baseUrl + 'Turismo',
            headers: { 'content-type': 'application/json, text/json' },
        });

    };

    return obj;

}])
.factory('Patrimonio', ['$http', function ($http) {
    
    var obj =  {};

    obj._getDados = function() {

        return  $http({
            method: 'GET',
            url: baseUrl + 'PatrimonioHistoricoCultural',
            headers: { 'content-type': 'application/json, text/json' },
        });

    };

    obj._getCategorias = function() {
        
        return  $http({
            method: 'GET',
            url: baseUrl + 'CategoriasPatrimonio',
            headers: { 'content-type': 'application/json, text/json' },
        });

    };
    

    return obj;

}])
.factory('PrefeituraAz', ['$http', function ($http) {
    
    var obj =  {};

    obj._getDados = function(pagenumber, pagesize, palavra) {

        return  $http({
            method: 'GET',
            url: baseUrl + 'AaZ?PageNumber='+ pagenumber+ '&PageSize='+ pagesize + '&Palavra=' + palavra,
            headers: { 'content-type': 'application/json, text/json' },
        });

    };

    return obj;

}])
.factory('Busca', ['$http', function ($http) {
    
    var obj =  {};

    obj._getDados = function(palavra) {

        return  $http({
            method: 'GET',
            url: baseUrl + 'Busca?Palavra=' + palavra,
            headers: { 'content-type': 'application/json, text/json' },
        });

    };

    return obj;

}])
.factory('Sugestao', ['$http', function ($http) {
    
    var obj =  {};
    
    obj._postUser = function(user) {

        return  $http({
            method: 'POST',
            data: user,
            url: baseUrl + 'CadastrarSugestao',
            headers: { 'content-type': 'application/json' }
        });

    };

    obj._returnTipos = function() {

        return  $http({
            method: 'GET',
            url: baseUrl + 'SugestaoTipo',

            headers: { 'content-type': 'application/json, text/json' },
        });
    };
    

    return obj;

}])
.factory('GetRootPage', ['$http', function ($http) {
    
    var obj =  {};

    obj._transform = function(url) {


        var returnText = '';
        var textreplace = url.split('/');

        switch(textreplace[1]) {
            case 'a-cidade':
                returnText = 'A Cidade'
                break;
            case 'o-governo':
                returnText = 'O Governo'
                break;
            case 'informativo':
                returnText = 'Informativo'
                break;
            case 'participe':
                returnText = 'Participe'
                break;
            case 'publicacoes-oficiais':
                returnText = 'Publicações Oficiais'
                break;
            case 'contato':
                returnText = 'Contato'
                break;
            case 'multimidia':
                returnText = 'Multimídia'
                break;                      
            default:
                returnText
        }
        
        return returnText;
    };

    return obj;

}]);