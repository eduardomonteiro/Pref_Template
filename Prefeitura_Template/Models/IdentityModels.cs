using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Prefeitura_Template.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser<int, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, int> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class CustomUserRole : IdentityUserRole<int> { }
    public class CustomUserClaim : IdentityUserClaim<int> { }
    public class CustomUserLogin : IdentityUserLogin<int> { }

    public class CustomRole : IdentityRole<int, CustomUserRole>
    {
        public CustomRole() { }
        public CustomRole(string name) { Name = name; }
    }

    public class CustomUserStore : UserStore<ApplicationUser, CustomRole, int,
        CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public CustomUserStore(ApplicationDbContext context)
            : base(context)
        {
        }
    }

    public class CustomRoleStore : RoleStore<CustomRole, int, CustomUserRole>
    {
        public CustomRoleStore(ApplicationDbContext context)
            : base(context)
        {
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, CustomRole, int, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public DbSet<AaZ> AaZ { get; set; }
        public DbSet<AaZCategoria> AaZCategoria { get; set; }
        public DbSet<Area> Area { get; set; }
        public DbSet<Bairro> Bairro { get; set; }
        public DbSet<Cidade> Cidade { get; set; }
        public DbSet<Concurso> Concurso { get; set; }
        public DbSet<ConcursoArquivo> ConcursoArquivo { get; set; }
        public DbSet<ConcursoModalidade> ConcursoModalidade { get; set; }
        public DbSet<Contato> Contato { get; set; }
        public DbSet<ContatoCategoria> ContatoCategoria { get; set; }
        public DbSet<ContatoTipo> ContatoTipo { get; set; }
        public DbSet<Documento> Documento { get; set; }
        public DbSet<DocumentoArquivo> DocumentoArquivo { get; set; }
        public DbSet<DocumentoCategoria> DocumentoCategoria { get; set; }
        public DbSet<Enquete> Enquete { get; set; }
        public DbSet<EnqueteOpcao> EnqueteOpcao { get; set; }
        public DbSet<EnqueteResposta> EnqueteResposta { get; set; }
        public DbSet<Evento> Evento { get; set; }
        public DbSet<EventoCategoria> EventoCategoria { get; set; }
        public DbSet<EventoGaleria> EventoGaleria { get; set; }
        public DbSet<ExPrefeito> ExPrefeito { get; set; }
        public DbSet<GaleriaAudio> GaleriaAudio { get; set; }
        public DbSet<GaleriaAudioCategoria> GaleriaAudioCategoria { get; set; }
        public DbSet<GaleriaFoto> GaleriaFoto { get; set; }
        public DbSet<GaleriaFotoCategoria> GaleriaFotoCategoria { get; set; }
        public DbSet<GaleriaFotoGaleria> GaleriaFotoGaleria { get; set; }
        public DbSet<GaleriaVideo> GaleriaVideo { get; set; }
        public DbSet<GaleriaVideoCategoria> GaleriaVideoCategoria { get; set; }
        public DbSet<Informativo> Informativo { get; set; }
        public DbSet<InformativoCategoria> InformativoCategoria { get; set; }
        public DbSet<InformativoGaleria> InformativoGaleria { get; set; }
        public DbSet<Legislacao> Legislacao { get; set; }
        public DbSet<LegislacaoArquivo> LegislacaoArquivo { get; set; }
        public DbSet<LegislacaoCategoria> LegislacaoCategoria { get; set; }
        public DbSet<Licitacao> Licitacao { get; set; }
        public DbSet<LicitacaoArquivo> LicitacaoArquivo { get; set; }
        public DbSet<LicitacaoModalidade> LicitacaoModalidade { get; set; }
        public DbSet<Log> Log { get; set; }
        public DbSet<NewsLetter> NewsLetter { get; set; }
        public DbSet<Noticia> Noticia { get; set; }
        public DbSet<NoticiaCategoria> NoticiaCategoria { get; set; }
        public DbSet<NoticiaGaleria> NoticiaGaleria { get; set; }
        public DbSet<PatrimonioHistoricoCultural> PatrimonioHistoricoCultural { get; set; }
        public DbSet<PatrimonioHistoricoCulturalCategoria> PatrimonioHistoricoCulturalCategoria { get; set; }
        public DbSet<Perfil> Perfil { get; set; }
        public DbSet<Perfil_Area> Perfil_Area { get; set; }
        public DbSet<PerfilSocioEconomico> PerfilSocioEconomico { get; set; }
        public DbSet<PerfilSocioEconomicoCategoria> PerfilSocioEconomicoCategoria { get; set; }
        public DbSet<PerguntasFrequentes> PerguntasFrequentes { get; set; }
        public DbSet<PerguntasFrequentesCategoria> PerguntasFrequentesCategoria { get; set; }
        public DbSet<Projeto> Projeto { get; set; }
        public DbSet<ProjetoCategoria> ProjetoCategoria { get; set; }
        public DbSet<ProjetoArquivo> ProjetoArquivo { get; set; }
        public DbSet<Secretaria> Secretaria { get; set; }
        public DbSet<SecretariaCategoria> SecretariaCategoria { get; set; }
        public DbSet<SecretariaServico> SecretariaServico { get; set; }
        public DbSet<SecretariaNomePrefixo> SecretariaNomePrefixo { get; set; }
        public DbSet<Servico> Servico { get; set; }
        public DbSet<ServicoCategoria> ServicoCategoria { get; set; }
        public DbSet<ServicoArquivo> ServicoArquivo { get; set; }
        public DbSet<ServicoPin> ServicoPin { get; set; }
        public DbSet<StatusPublicacao> StatusPublicacao { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<Timeline> Timeline { get; set; }
        public DbSet<Turismo> Turismo { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<UsuarioSecretaria> UsuarioSecretaria { get; set; }


        public ApplicationDbContext()
            : base("Name=prefeituraDataBase")
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
            Database.SetInitializer<ApplicationDbContext>(null);
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

    }
}