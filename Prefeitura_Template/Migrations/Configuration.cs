namespace Prefeitura_Template.Migrations
{
    using Areas.Admin.Enums;
    using Microsoft.AspNet.Identity;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Text;

    internal sealed class Configuration : DbMigrationsConfiguration<Prefeitura_Template.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Prefeitura_Template.Models.ApplicationDbContext context)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(context.Database.Connection.ConnectionString))
                {
                    connection.Open();
					//using (SqlCommand command = new SqlCommand(string.Format("ALTER DATABASE prefeituraDataBase COLLATE Latin1_General_CI_AI", context.Database.Connection.Database), connection))
					//{
					//    command.ExecuteNonQuery();
					//}
					using (SqlCommand command = new SqlCommand(string.Format("ALTER TABLE AaZ ALTER COLUMN Titulo NVARCHAR(100) COLLATE Latin1_General_CI_AI NOT NULL", context.Database.Connection.Database), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand(string.Format("ALTER TABLE AaZ ALTER COLUMN Url NVARCHAR(1000) COLLATE Latin1_General_CI_AI NOT NULL", context.Database.Connection.Database), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand(string.Format("ALTER TABLE Bairro ALTER COLUMN Descricao NVARCHAR(200) COLLATE Latin1_General_CI_AI NOT NULL", context.Database.Connection.Database), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand(string.Format("ALTER TABLE Concurso ALTER COLUMN Titulo NVARCHAR(60) COLLATE Latin1_General_CI_AI NOT NULL", context.Database.Connection.Database), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand(string.Format("ALTER TABLE Concurso ALTER COLUMN Descricao NVARCHAR(MAX) COLLATE Latin1_General_CI_AI NOT NULL", context.Database.Connection.Database), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand(string.Format("ALTER TABLE Concurso ALTER COLUMN Slug NVARCHAR(100) COLLATE Latin1_General_CI_AI NOT NULL", context.Database.Connection.Database), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand(string.Format("ALTER TABLE Contato ALTER COLUMN Nome NVARCHAR(300) COLLATE Latin1_General_CI_AI NOT NULL", context.Database.Connection.Database), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand(string.Format("ALTER TABLE Contato ALTER COLUMN Email NVARCHAR(300) COLLATE Latin1_General_CI_AI NOT NULL", context.Database.Connection.Database), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand(string.Format("ALTER TABLE Documento ALTER COLUMN Titulo NVARCHAR(60) COLLATE Latin1_General_CI_AI NOT NULL", context.Database.Connection.Database), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand(string.Format("ALTER TABLE Documento ALTER COLUMN Texto NVARCHAR(MAX) COLLATE Latin1_General_CI_AI NOT NULL", context.Database.Connection.Database), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand(string.Format("ALTER TABLE Enquete ALTER COLUMN Pergunta NVARCHAR(1500) COLLATE Latin1_General_CI_AI NOT NULL", context.Database.Connection.Database), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand(string.Format("ALTER TABLE Evento ALTER COLUMN Titulo NVARCHAR(140) COLLATE Latin1_General_CI_AI NOT NULL", context.Database.Connection.Database), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand(string.Format("ALTER TABLE Evento ALTER COLUMN SubTitulo NVARCHAR(140) COLLATE Latin1_General_CI_AI NOT NULL", context.Database.Connection.Database), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand(string.Format("ALTER TABLE Evento ALTER COLUMN Texto NVARCHAR(MAX) COLLATE Latin1_General_CI_AI NOT NULL", context.Database.Connection.Database), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand(string.Format("ALTER TABLE Evento ALTER COLUMN Slug NVARCHAR(100) COLLATE Latin1_General_CI_AI NOT NULL", context.Database.Connection.Database), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand(string.Format("ALTER TABLE ExPrefeito ALTER COLUMN Nome NVARCHAR(200) COLLATE Latin1_General_CI_AI NOT NULL", context.Database.Connection.Database), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand(string.Format("ALTER TABLE GaleriaAudio ALTER COLUMN Titulo NVARCHAR(60) COLLATE Latin1_General_CI_AI NOT NULL", context.Database.Connection.Database), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand(string.Format("ALTER TABLE GaleriaAudio ALTER COLUMN Descricao NVARCHAR(MAX) COLLATE Latin1_General_CI_AI NOT NULL", context.Database.Connection.Database), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand(string.Format("ALTER TABLE GaleriaFoto ALTER COLUMN Titulo NVARCHAR(60) COLLATE Latin1_General_CI_AI NOT NULL", context.Database.Connection.Database), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand(string.Format("ALTER TABLE GaleriaFoto ALTER COLUMN Descricao NVARCHAR(MAX) COLLATE Latin1_General_CI_AI NOT NULL", context.Database.Connection.Database), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand(string.Format("ALTER TABLE GaleriaFoto ALTER COLUMN Slug NVARCHAR(100) COLLATE Latin1_General_CI_AI NOT NULL", context.Database.Connection.Database), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand(string.Format("ALTER TABLE GaleriaVideo ALTER COLUMN Titulo NVARCHAR(60) COLLATE Latin1_General_CI_AI NOT NULL", context.Database.Connection.Database), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand(string.Format("ALTER TABLE GaleriaVideo ALTER COLUMN Descricao NVARCHAR(MAX) COLLATE Latin1_General_CI_AI NOT NULL", context.Database.Connection.Database), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand(string.Format("ALTER TABLE Informativo ALTER COLUMN Titulo NVARCHAR(60) COLLATE Latin1_General_CI_AI NOT NULL", context.Database.Connection.Database), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand(string.Format("ALTER TABLE Informativo ALTER COLUMN SubTitulo NVARCHAR(140) COLLATE Latin1_General_CI_AI NOT NULL", context.Database.Connection.Database), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand(string.Format("ALTER TABLE Informativo ALTER COLUMN Slug NVARCHAR(100) COLLATE Latin1_General_CI_AI NOT NULL", context.Database.Connection.Database), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand(string.Format("ALTER TABLE Informativo ALTER COLUMN Texto NVARCHAR(MAX) COLLATE Latin1_General_CI_AI NOT NULL", context.Database.Connection.Database), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand(string.Format("ALTER TABLE Legislacao ALTER COLUMN Titulo NVARCHAR(60) COLLATE Latin1_General_CI_AI NOT NULL", context.Database.Connection.Database), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand(string.Format("ALTER TABLE Legislacao ALTER COLUMN Descricao NVARCHAR(MAX) COLLATE Latin1_General_CI_AI NOT NULL", context.Database.Connection.Database), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand(string.Format("ALTER TABLE Licitacao ALTER COLUMN Titulo NVARCHAR(140) COLLATE Latin1_General_CI_AI NOT NULL", context.Database.Connection.Database), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand(string.Format("ALTER TABLE Licitacao ALTER COLUMN Descricao NVARCHAR(MAX) COLLATE Latin1_General_CI_AI NOT NULL", context.Database.Connection.Database), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand(string.Format("ALTER TABLE Licitacao ALTER COLUMN Slug NVARCHAR(100) COLLATE Latin1_General_CI_AI NOT NULL", context.Database.Connection.Database), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand(string.Format("ALTER TABLE NewsLetter ALTER COLUMN Nome NVARCHAR(300) COLLATE Latin1_General_CI_AI NOT NULL", context.Database.Connection.Database), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand(string.Format("ALTER TABLE NewsLetter ALTER COLUMN Email NVARCHAR(100) COLLATE Latin1_General_CI_AI NOT NULL", context.Database.Connection.Database), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand(string.Format("ALTER TABLE Noticia ALTER COLUMN Titulo NVARCHAR(140) COLLATE Latin1_General_CI_AI NOT NULL", context.Database.Connection.Database), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand(string.Format("ALTER TABLE Noticia ALTER COLUMN SubTitulo NVARCHAR(140) COLLATE Latin1_General_CI_AI NOT NULL", context.Database.Connection.Database), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand(string.Format("ALTER TABLE Noticia ALTER COLUMN Slug NVARCHAR(100) COLLATE Latin1_General_CI_AI NOT NULL", context.Database.Connection.Database), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand(string.Format("ALTER TABLE Noticia ALTER COLUMN Texto NVARCHAR(MAX) COLLATE Latin1_General_CI_AI NOT NULL", context.Database.Connection.Database), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand(string.Format("ALTER TABLE PatrimonioHistoricoCultural ALTER COLUMN Nome NVARCHAR(200) COLLATE Latin1_General_CI_AI NOT NULL", context.Database.Connection.Database), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand(string.Format("ALTER TABLE PatrimonioHistoricoCultural ALTER COLUMN Descricao NVARCHAR(MAX) COLLATE Latin1_General_CI_AI NOT NULL", context.Database.Connection.Database), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand(string.Format("ALTER TABLE PerfilSocioEconomico ALTER COLUMN Titulo NVARCHAR(60) COLLATE Latin1_General_CI_AI NOT NULL", context.Database.Connection.Database), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand(string.Format("ALTER TABLE PerfilSocioEconomico ALTER COLUMN Descricao NVARCHAR(MAX) COLLATE Latin1_General_CI_AI NOT NULL", context.Database.Connection.Database), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand(string.Format("ALTER TABLE PerguntasFrequentes ALTER COLUMN Titulo NVARCHAR(500) COLLATE Latin1_General_CI_AI NOT NULL", context.Database.Connection.Database), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand(string.Format("ALTER TABLE PerguntasFrequentes ALTER COLUMN Descricao NVARCHAR(MAX) COLLATE Latin1_General_CI_AI NOT NULL", context.Database.Connection.Database), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand(string.Format("ALTER TABLE Projeto ALTER COLUMN Titulo NVARCHAR(60) COLLATE Latin1_General_CI_AI NOT NULL", context.Database.Connection.Database), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand(string.Format("ALTER TABLE Projeto ALTER COLUMN SubTitulo NVARCHAR(140) COLLATE Latin1_General_CI_AI NOT NULL", context.Database.Connection.Database), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand(string.Format("ALTER TABLE Projeto ALTER COLUMN Descricao NVARCHAR(MAX) COLLATE Latin1_General_CI_AI NOT NULL", context.Database.Connection.Database), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand(string.Format("ALTER TABLE Projeto ALTER COLUMN Slug NVARCHAR(100) COLLATE Latin1_General_CI_AI NOT NULL", context.Database.Connection.Database), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand(string.Format("ALTER TABLE Secretaria ALTER COLUMN Nome NVARCHAR(300) COLLATE Latin1_General_CI_AI NOT NULL", context.Database.Connection.Database), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand(string.Format("ALTER TABLE Secretaria ALTER COLUMN Slug NVARCHAR(300) COLLATE Latin1_General_CI_AI NOT NULL", context.Database.Connection.Database), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand(string.Format("ALTER TABLE Servico ALTER COLUMN Nome NVARCHAR(200) COLLATE Latin1_General_CI_AI NOT NULL", context.Database.Connection.Database), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand(string.Format("ALTER TABLE Servico ALTER COLUMN Slug NVARCHAR(200) COLLATE Latin1_General_CI_AI NOT NULL", context.Database.Connection.Database), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand(string.Format("ALTER TABLE Turismo ALTER COLUMN Nome NVARCHAR(200) COLLATE Latin1_General_CI_AI NOT NULL", context.Database.Connection.Database), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand(string.Format("ALTER TABLE Turismo ALTER COLUMN Descricao NVARCHAR(MAX) COLLATE Latin1_General_CI_AI NOT NULL", context.Database.Connection.Database), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand(string.Format("ALTER TABLE Usuario ALTER COLUMN Nome NVARCHAR(300) COLLATE Latin1_General_CI_AI NOT NULL", context.Database.Connection.Database), connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    SqlConnection.ClearAllPools();
                    connection.Close();
                }

                //var Cidade = context.Cidade.FirstOrDefault();
                //if (Cidade == null || Cidade.Id == 0)
                //{
                //    context.Cidade.AddOrUpdate(u => u.Id,
                //        new Cidade
                //        {
                //            Id = 1,
                //            Descricao = "descricao de 'a cidade'",
                //            DescricaoBandeira = "descricao bandeira",
                //            DescricaoBrasao = "descricao do brasao",
                //            DescricaoInvista = "decricao invista",
                //            ExPrefeito = "descricao exprefeitos",
                //            Turismo = "descrico turismo",
                //            PatrimonioHistoricoCultural = "descricao patrimonio",
                //            ImagemBandeira = "bandeira.jpg",
                //            ImagemBrasao = "brasao.jpg",
                //            ImagemDescricao = "descricao.jpg",
                //            ImagemInvista = "invista.jpg",
                //            DescricaoHino = "descricao do hino",
                //            AudioHino = "hino.mp3",
                //            Altitude = "18.000",
                //            Area = "15.000 M2",
                //            AtualPrefeito = "Jo�o Ningu�m",
                //            Clima = "Seco",
                //            Densidade = "10.000",
                //            Populacao = "30 mil",
                //            DataFundacao = DateTime.Parse("01/01/1950")
                //        });
                //    context.SaveChanges();
                //}

                //context.Perfil.AddOrUpdate(u => u.Id,
                //    new Perfil
                //    {
                //        Id = 1,
                //        Descricao = "Administrador",
                //        DataCadastro = DateTime.Now,
                //        Status = (int)StatusPadrao.Ativo,
                //    });
                //context.SaveChanges();

                context.Area.AddOrUpdate(p => p.Id,
                      new Area
                      {
                          Id = 1,
                          Descricao = "Usu�rios",
                          Nome = "Usuarios",
                          Action = "Index",
                          LinkClass = "fa fa-user",
                          AreaPai = 0,
                          Ordem = 20,
                          Status = (int)StatusPadrao.Ativo,
                          DataCadastro = DateTime.Now
                      },
                      new Area
                      {
                          Id = 2,
                          Descricao = "Sobre a Cidade",
                          Nome = "Cidade",
                          Action = "Index",
                          LinkClass = "fa fa-building",
                          AreaPai = 29,
                          Ordem = 1,
                          Status = (int)StatusPadrao.Ativo,
                          DataCadastro = DateTime.Now
                      },
                      new Area
                      {
                          Id = 3,
                          Descricao = "Ex-Prefeitos",
                          Nome = "ExPrefeitos",
                          Action = "Index",
                          LinkClass = "fa fa-users",
                          AreaPai = 30,
                          Ordem = 1,
                          Status = (int)StatusPadrao.Ativo,
                          DataCadastro = DateTime.Now
                      },
                      new Area
                      {
                          Id = 4,
                          Descricao = "Perfis",
                          Nome = "Perfis",
                          Action = "Index",
                          LinkClass = "fa fa-tags",
                          AreaPai = 0,
                          Ordem = 19,
                          Status = (int)StatusPadrao.Ativo,
                          DataCadastro = DateTime.Now
                      },
                      new Area
                      {
                          Id = 5,
                          Descricao = "Atra��es Tur�sticas",
                          Nome = "Turismo",
                          Action = "Index",
                          LinkClass = "fa fa-picture-o",
                          AreaPai = 29,
                          Ordem = 4,
                          Status = (int)StatusPadrao.Ativo,
                          DataCadastro = DateTime.Now
                      },
                      new Area
                      {
                          Id = 6,
                          Descricao = "Prefeitura de A a Z",
                          Nome = "AaZ",
                          Action = "Index",
                          LinkClass = "fa fa-sort-alpha-asc",
                          AreaPai = 0,
                          Ordem = 18,
                          Status = (int)StatusPadrao.Ativo,
                          DataCadastro = DateTime.Now
                      },
                      new Area
                      {
                          Id = 7,
                          Descricao = "Eventos",
                          Nome = "Eventos",
                          Action = "Index",
                          LinkClass = "fa fa-calendar",
                          AreaPai = 32,
                          Ordem = 2,
                          Status = (int)StatusPadrao.Ativo,
                          DataCadastro = DateTime.Now
                      },
                      new Area
                      {
                          Id = 8,
                          Descricao = "Not�cias",
                          Nome = "Noticias",
                          Action = "Index",
                          LinkClass = "fa fa-newspaper-o",
                          AreaPai = 32,
                          Ordem = 1,
                          Status = (int)StatusPadrao.Ativo,
                          DataCadastro = DateTime.Now
                      },
                      new Area
                      {
                          Id = 9,
                          Descricao = "Projetos",
                          Nome = "Projetos",
                          Action = "Index",
                          LinkClass = "fa fa-thumb-tack",
                          AreaPai = 30,
                          Ordem = 3,
                          Status = (int)StatusPadrao.Ativo,
                          DataCadastro = DateTime.Now
                      },
                      new Area
                      {
                          Id = 10,
                          Descricao = "Documentos",
                          Nome = "Documentos",
                          Action = "Index",
                          LinkClass = "fa fa-files-o",
                          AreaPai = 34,
                          Ordem = 4,
                          Status = (int)StatusPadrao.Ativo,
                          DataCadastro = DateTime.Now
                      },
                      new Area
                      {
                          Id = 11,
                          Descricao = "Secretarias",
                          Nome = "Secretarias",
                          Action = "Index",
                          LinkClass = "fa fa-info-circle",
                          AreaPai = 30,
                          Ordem = 2,
                          Status = (int)StatusPadrao.Ativo,
                          DataCadastro = DateTime.Now
                      },
                      new Area
                      {
                          Id = 12,
                          Descricao = "Informativos",
                          Nome = "Informativos",
                          Action = "Index",
                          LinkClass = "fa fa-file-o",
                          AreaPai = 32,
                          Ordem = 3,
                          Status = (int)StatusPadrao.Ativo,
                          DataCadastro = DateTime.Now
                      },
                      new Area
                      {
                          Id = 13,
                          Descricao = "Galeria de Fotos",
                          Nome = "GaleriaFotos",
                          Action = "Index",
                          LinkClass = "fa fa-file-image-o",
                          AreaPai = 31,
                          Ordem = 1,
                          Status = (int)StatusPadrao.Ativo,
                          DataCadastro = DateTime.Now
                      },
                      new Area
                      {
                          Id = 14,
                          Descricao = "Galeria de Audios",
                          Nome = "GaleriaAudios",
                          Action = "Index",
                          LinkClass = "fa fa-file-audio-o",
                          AreaPai = 31,
                          Ordem = 3,
                          Status = (int)StatusPadrao.Ativo,
                          DataCadastro = DateTime.Now
                      },
                      new Area
                      {
                          Id = 15,
                          Descricao = "Fale Conosco",
                          Nome = "FaleConosco",
                          Action = "Index",
                          LinkClass = "fa fa-book",
                          AreaPai = 35,
                          Ordem = 1,
                          Status = (int)StatusPadrao.Ativo,
                          DataCadastro = DateTime.Now
                      },
                      new Area
                      {
                          Id = 16,
                          Descricao = "Fa�a uma Sugest�o",
                          Nome = "Sugestoes",
                          Action = "Index",
                          LinkClass = "fa fa-comments",
                          AreaPai = 33,
                          Ordem = 2,
                          Status = (int)StatusPadrao.Ativo,
                          DataCadastro = DateTime.Now
                      },
                      new Area
                      {
                          Id = 17,
                          Descricao = "Licita��es",
                          Nome = "Licitacoes",
                          Action = "Index",
                          LinkClass = "fa fa-folder-o",
                          AreaPai = 34,
                          Ordem = 1,
                          Status = (int)StatusPadrao.Ativo,
                          DataCadastro = DateTime.Now
                      },
                      new Area
                      {
                          Id = 18,
                          Descricao = "Concursos",
                          Nome = "Concursos",
                          Action = "Index",
                          LinkClass = "fa fa-newspaper-o",
                          AreaPai = 34,
                          Ordem = 3,
                          Status = (int)StatusPadrao.Ativo,
                          DataCadastro = DateTime.Now
                      },
                      new Area
                      {
                          Id = 19,
                          Descricao = "Bairros",
                          Nome = "Bairros",
                          Action = "Index",
                          LinkClass = "fa fa-building-o",
                          AreaPai = 0,
                          Ordem = 17,
                          Status = (int)StatusPadrao.Ativo,
                          DataCadastro = DateTime.Now
                      },
                      new Area
                      {
                          Id = 20,
                          Descricao = "Enquetes",
                          Nome = "Enquetes",
                          Action = "Index",
                          LinkClass = "fa fa-plus-square",
                          AreaPai = 33,
                          Ordem = 1,
                          Status = (int)StatusPadrao.Ativo,
                          DataCadastro = DateTime.Now
                      },
                      new Area
                      {
                          Id = 21,
                          Descricao = "Galeria de Video",
                          Nome = "GaleriaVideos",
                          Action = "Index",
                          LinkClass = "fa fa-file-video-o",
                          AreaPai = 31,
                          Ordem = 2,
                          Status = (int)StatusPadrao.Ativo,
                          DataCadastro = DateTime.Now
                      },
                      new Area
                      {
                          Id = 22,
                          Descricao = "Legisla��o",
                          Nome = "Legislacoes",
                          Action = "Index",
                          LinkClass = "fa fa-file",
                          AreaPai = 34,
                          Ordem = 2,
                          Status = (int)StatusPadrao.Ativo,
                          DataCadastro = DateTime.Now
                      },
                      new Area
                      {
                          Id = 23,
                          Descricao = "Newsletter",
                          Nome = "Newsletter",
                          Action = "Index",
                          LinkClass = "fa fa-at",
                          AreaPai = 33,
                          Ordem = 0,
                          Status = (int)StatusPadrao.Ativo,
                          DataCadastro = DateTime.Now
                      },
                      new Area
                      {
                          Id = 24,
                          Descricao = "Patrimonio Hist�rico",
                          Nome = "PatrimonioHistoricoCultural",
                          Action = "Index",
                          LinkClass = "fa fa-university",
                          AreaPai = 29,
                          Ordem = 2,
                          Status = (int)StatusPadrao.Ativo,
                          DataCadastro = DateTime.Now
                      },
                      new Area
                      {
                          Id = 25,
                          Descricao = "Perfil S�cio-Econ�mico",
                          Nome = "PerfilSocioEconomico",
                          Action = "Index",
                          LinkClass = "fa fa-mouse-pointer",
                          AreaPai = 29,
                          Ordem = 3,
                          Status = (int)StatusPadrao.Ativo,
                          DataCadastro = DateTime.Now
                      },
                      new Area
                      {
                          Id = 26,
                          Descricao = "Perguntas Frequentes",
                          Nome = "Perguntas Frequentes",
                          Action = "Index",
                          LinkClass = "fa fa-question-circle",
                          AreaPai = 35,
                          Ordem = 2,
                          Status = (int)StatusPadrao.Ativo,
                          DataCadastro = DateTime.Now
                      },
                      new Area
                      {
                          Id = 27,
                          Descricao = "Servi�os",
                          Nome = "Servicos",
                          Action = "Index",
                          LinkClass = "fa fa-arrows",
                          AreaPai = 0,
                          Ordem = 5,
                          Status = (int)StatusPadrao.Ativo,
                          DataCadastro = DateTime.Now
                      },
                      new Area
                      {
                          Id = 28,
                          Descricao = "Timeline",
                          Nome = "Timeline",
                          Action = "Index",
                          LinkClass = "fa fa-bars",
                          AreaPai = 0,
                          Ordem = 15,
                          Status = (int)StatusPadrao.Ativo,
                          DataCadastro = DateTime.Now
                      },
                      new Area
                      {
                          Id = 29,
                          Descricao = "� Cidade",
                          Nome = "",
                          Action = "",
                          LinkClass = "fa fa-building-o",
                          AreaPai = 0,
                          Ordem = 1,
                          Status = (int)StatusPadrao.Ativo,
                          DataCadastro = DateTime.Now
                      },
                      new Area
                      {
                          Id = 30,
                          Descricao = "Governo",
                          Nome = "",
                          Action = "",
                          LinkClass = "fa fa-info",
                          AreaPai = 0,
                          Ordem = 2,
                          Status = (int)StatusPadrao.Ativo,
                          DataCadastro = DateTime.Now
                      },
                      new Area
                      {
                          Id = 31,
                          Descricao = "Multim�dia",
                          Nome = "",
                          Action = "",
                          LinkClass = "fa fa-file-text",
                          AreaPai = 0,
                          Ordem = 3,
                          Status = (int)StatusPadrao.Ativo,
                          DataCadastro = DateTime.Now
                      },
                      new Area
                      {
                          Id = 32,
                          Descricao = "Informativos",
                          Nome = "",
                          Action = "",
                          LinkClass = "fa fa-archive",
                          AreaPai = 0,
                          Ordem = 4,
                          Status = (int)StatusPadrao.Ativo,
                          DataCadastro = DateTime.Now
                      },
                      new Area
                      {
                          Id = 33,
                          Descricao = "Participe",
                          Nome = "",
                          Action = "",
                          LinkClass = "fa fa-commenting-o",
                          AreaPai = 0,
                          Ordem = 6,
                          Status = (int)StatusPadrao.Ativo,
                          DataCadastro = DateTime.Now
                      }
                      ,
                      new Area
                      {
                          Id = 34,
                          Descricao = "Publica��es Oficias",
                          Nome = "",
                          Action = "",
                          LinkClass = "fa fa-newspaper-o",
                          AreaPai = 0,
                          Ordem = 7,
                          Status = (int)StatusPadrao.Ativo,
                          DataCadastro = DateTime.Now
                      }
                      ,
                      new Area
                      {
                          Id = 35,
                          Descricao = "Contato",
                          Nome = "",
                          Action = "",
                          LinkClass = "fa fa-envelope-o",
                          AreaPai = 0,
                          Ordem = 8,
                          Status = (int)StatusPadrao.Ativo,
                          DataCadastro = DateTime.Now
                      }
                    );
                context.SaveChanges();

                //for (int I = 1; I <= 35; I++)
                //{
                //    context.Perfil_Area.AddOrUpdate(p => p.Id,
                //          new Perfil_Area
                //          {
                //              Id = I,
                //              AreaId = I,
                //              PerfilId = 1,
                //              Leitura = true,
                //              Criacao = true,
                //              Alteracao = true,
                //              Exclusao = true,
                //          }
                //        );
                //}
                //context.SaveChanges();

                //context.DocumentoCategoria.AddOrUpdate(u => u.Id,
                //    new DocumentoCategoria
                //    {
                //        Id = 1,
                //        Descricao = "Teste Categoria Documento",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.Documento.AddOrUpdate(u => u.Id,
                //    new Documento
                //    {
                //        Id = 1,
                //        Titulo = "Teste Documento",
                //        DocumentoCategoriaId = 1,
                //        Texto = "texto do documento",
                //        DataPublicacao = DateTime.Now,
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.DocumentoArquivo.AddOrUpdate(u => u.Id,
                //    new DocumentoArquivo
                //    {
                //        Id = 1,
                //        Arquivo = "Teste.pdf",
                //        ArquivoNome = "Nome de exibi��o",
                //        Tamanho = "15MB",
                //        DocumentoId = 1,
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.Tag.AddOrUpdate(u => u.Id,
                //    new Tag
                //    {
                //        Id = 1,
                //        RegistroId = 1,
                //        AreaId = 10,
                //        Descricao = "Teste",
                //        Slug = "teste",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.EventoCategoria.AddOrUpdate(u => u.Id,
                //    new EventoCategoria
                //    {
                //        Id = 1,
                //        Descricao = "Teste Categoria Evento",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.Evento.AddOrUpdate(u => u.Id,
                //    new Evento
                //    {
                //        Id = 1,
                //        Titulo = "Teste Evento",
                //        EventoCategoriaId = 1,
                //        Texto = "texto do Evento",
                //        SubTitulo = "Teste subtitulo do evento",
                //        Slug = "teste-evento",
                //        Imagem = "teste.jpg",
                //        LinkVideo = "_uY5SThmZzw",
                //        DataHorarioEvento = DateTime.Now,
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.EventoGaleria.AddOrUpdate(u => u.Id,
                //    new EventoGaleria
                //    {
                //        Id = 1,
                //        Midia = "Teste.jpg",
                //        Credito = "Cr�dito Imagem Teste",
                //        Legenda = "Legenda Imagem Teste",
                //        EventoId = 1,
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.Tag.AddOrUpdate(u => u.Id,
                //    new Tag
                //    {
                //        Id = 2,
                //        RegistroId = 1,
                //        AreaId = 7,
                //        Descricao = "Teste",
                //        Slug = "teste",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.ExPrefeito.AddOrUpdate(u => u.Id,
                //    new ExPrefeito
                //    {
                //        Id = 1,
                //        Nome = "Joao Ninguem",
                //        DataInicioLegislatura = DateTime.Now.AddYears(-4),
                //        DataFimLegislatura = DateTime.Now,
                //        Imagem = "Teste.jpg",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.NoticiaCategoria.AddOrUpdate(u => u.Id,
                //    new NoticiaCategoria
                //    {
                //        Id = 1,
                //        Descricao = "Teste Noticia Categoria",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.Noticia.AddOrUpdate(u => u.Id,
                //    new Noticia
                //    {
                //        Id = 1,
                //        Titulo = "Teste Noticia",
                //        NoticiaCategoriaId = 1,
                //        Texto = "texto da noticia",
                //        SubTitulo = "Teste subtitulo da noticia",
                //        Slug = "teste-noticia",
                //        Imagem = "Teste.jpg",
                //        LinkVideo = "_uY5SThmZzw",
                //        Destaque = true,
                //        DataPublicacao = DateTime.Now,
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.NoticiaGaleria.AddOrUpdate(u => u.Id,
                //    new NoticiaGaleria
                //    {
                //        Id = 1,
                //        Midia = "Teste.jpg",
                //        Credito = "Cr�dito Imagem Teste",
                //        Legenda = "Legenda Imagem Teste",
                //        NoticiaId = 1,
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.Tag.AddOrUpdate(u => u.Id,
                //    new Tag
                //    {
                //        Id = 3,
                //        RegistroId = 1,
                //        AreaId = 8,
                //        Descricao = "Teste",
                //        Slug = "teste",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.PatrimonioHistoricoCulturalCategoria.AddOrUpdate(u => u.Id,
                //    new PatrimonioHistoricoCulturalCategoria
                //    {
                //        Id = 1,
                //        Descricao = "Teste Patrimonio Historico Cultural Categoria",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.PatrimonioHistoricoCultural.AddOrUpdate(u => u.Id,
                //    new PatrimonioHistoricoCultural
                //    {
                //        Id = 1,
                //        Nome = "Teste patrimonio",
                //        Imagem = "Teste.jpg",
                //        Descricao = "descricao patrimonio teste",
                //        LinkMaps = "https://www.google.com.br/maps",
                //        PatrimonioHistoricoCulturalCategoriaId = 1,
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.PerfilSocioEconomicoCategoria.AddOrUpdate(u => u.Id,
                //    new PerfilSocioEconomicoCategoria
                //    {
                //        Id = 1,
                //        Descricao = "Teste Perfil Socio Economico Categoria",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.PerfilSocioEconomico.AddOrUpdate(u => u.Id,
                //    new PerfilSocioEconomico
                //    {
                //        Id = 1,
                //        Imagem = "Teste.jpg",
                //        Titulo = "Teste de Perfil Socio Economico",
                //        Descricao = "Descricao Perfil Socio Economico teste",
                //        PerfilSocioEconomicoCategoriaId = 1,
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.SecretariaCategoria.AddOrUpdate(u => u.Id,
                //    new SecretariaCategoria
                //    {
                //        Id = 1,
                //        Descricao = "Gabinete do Prefeito",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    },
                //    new SecretariaCategoria
                //    {
                //        Id = 2,
                //        Descricao = "Secretaria",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    },
                //    new SecretariaCategoria
                //    {
                //        Id = 3,
                //        Descricao = "Subsecretaria",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    },
                //    new SecretariaCategoria
                //    {
                //        Id = 4,
                //        Descricao = "Setor ou Conselho",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.SecretariaNomePrefixo.AddOrUpdate(u => u.Id,
                //    new SecretariaNomePrefixo
                //    {
                //        Id = 1,
                //        Descricao = "Secretaria Municipal",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    },
                //    new SecretariaNomePrefixo
                //    {
                //        Id = 2,
                //        Descricao = "Controladoria",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    },
                //    new SecretariaNomePrefixo
                //    {
                //        Id = 3,
                //        Descricao = "Procuradoria",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.Tag.AddOrUpdate(u => u.Id,
                //    new Tag
                //    {
                //        Id = 4,
                //        RegistroId = 1,
                //        AreaId = 11,
                //        Descricao = "Teste",
                //        Slug = "teste",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.Tag.AddOrUpdate(u => u.Id,
                //    new Tag
                //    {
                //        Id = 5,
                //        RegistroId = 2,
                //        AreaId = 11,
                //        Descricao = "Teste",
                //        Slug = "teste",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.ProjetoCategoria.AddOrUpdate(u => u.Id,
                //    new ProjetoCategoria
                //    {
                //        Id = 1,
                //        Descricao = "Teste Projeto Categoria",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.Projeto.AddOrUpdate(u => u.Id,
                //    new Projeto
                //    {
                //        Id = 1,
                //        Imagem = "Teste.jpg",
                //        Descricao = "Descricao Projeto Teste",
                //        Titulo = "Teste Projeto",
                //        SubTitulo = "subtitulo projeto teste",
                //        LinkExterno = "www.google.com.br",
                //        ProjetoCategoriaId = 1,
                //        SecretariaId = 1,
                //        Slug = "teste-projeto",
                //        LinkVideo = "_uY5SThmZzw",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.ProjetoArquivo.AddOrUpdate(u => u.Id,
                //    new ProjetoArquivo
                //    {
                //        Id = 1,
                //        Arquivo = "Teste.pdf",
                //        ArquivoNome = "Nome de exibi��o",
                //        Tamanho = "15MB",
                //        ProjetoId = 1,
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.Tag.AddOrUpdate(u => u.Id,
                //    new Tag
                //    {
                //        Id = 6,
                //        RegistroId = 1,
                //        AreaId = 9,
                //        Descricao = "Teste",
                //        Slug = "teste",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.Turismo.AddOrUpdate(u => u.Id,
                //    new Turismo
                //    {
                //        Id = 1,
                //        Imagem = "Teste.jpg",
                //        Descricao = "Descricao Turismo Teste",
                //        LinkMaps = "https://www.google.com.br/maps/@-22.5328945,-44.195564,16.25z",
                //        Nome = "Ponto turistico teste",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.InformativoCategoria.AddOrUpdate(u => u.Id,
                //    new InformativoCategoria
                //    {
                //        Id = 1,
                //        Descricao = "Oficial",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    },
                //     new InformativoCategoria
                //     {
                //         Id = 2,
                //         Descricao = "Informativo ao Servidor e Informativos de Gest�o",
                //         Status = (int)StatusPadrao.Ativo,
                //         DataCadastro = DateTime.Now
                //     });
                //context.SaveChanges();

                //context.Informativo.AddOrUpdate(u => u.Id,
                //    new Informativo
                //    {
                //        Id = 1,
                //        Titulo = "Teste Informativo",
                //        InformativoCategoriaId = 1,
                //        Texto = "texto do informativo",
                //        SubTitulo = "Teste subtitulo da informativo",
                //        Slug = "teste-informativo",
                //        Arquivo = "Teste.pdf",
                //        Imagem = "teste.jpg",
                //        LinkVideo = "_uY5SThmZzw",
                //        DataPublicacao = DateTime.Now,
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.InformativoGaleria.AddOrUpdate(u => u.Id,
                //    new InformativoGaleria
                //    {
                //        Id = 1,
                //        Midia = "Teste.jpg",
                //        Credito = "Cr�dito Imagem Teste",
                //        Legenda = "Legenda Imagem Teste",
                //        InformativoId = 1,
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.Tag.AddOrUpdate(u => u.Id,
                //   new Tag
                //   {
                //       Id = 7,
                //       RegistroId = 1,
                //       AreaId = 12,
                //       Descricao = "Teste",
                //       Slug = "teste",
                //       Status = (int)StatusPadrao.Ativo,
                //       DataCadastro = DateTime.Now
                //   });
                //context.SaveChanges();

                //context.GaleriaFotoCategoria.AddOrUpdate(u => u.Id,
                //    new GaleriaFotoCategoria
                //    {
                //        Id = 1,
                //        Descricao = "Teste Categoria Galeria de Foto",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.GaleriaFoto.AddOrUpdate(u => u.Id,
                //    new GaleriaFoto
                //    {
                //        Id = 1,
                //        Titulo = "Teste Galeria foto",
                //        GaleriaFotoCategoriaId = 1,
                //        Descricao = "texto da galeria",
                //        Slug = "teste-galeria-foto",
                //        Fotografo = "Paulo Ricardo",
                //        Destaque = true,
                //        DataPublicacao = DateTime.Now,
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.GaleriaFotoGaleria.AddOrUpdate(u => u.Id,
                //    new GaleriaFotoGaleria
                //    {
                //        Id = 1,
                //        Imagem = "Teste.jpg",
                //        Credito = "Cr�dito Imagem Teste",
                //        Legenda = "Legenda Imagem Teste",
                //        GaleriaFotoId = 1,
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.Tag.AddOrUpdate(u => u.Id,
                //    new Tag
                //    {
                //        Id = 8,
                //        RegistroId = 1,
                //        AreaId = 13,
                //        Descricao = "Teste",
                //        Slug = "teste",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.GaleriaVideoCategoria.AddOrUpdate(u => u.Id,
                //    new GaleriaVideoCategoria
                //    {
                //        Id = 1,
                //        Descricao = "Teste Galeira de Video Categoria",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.GaleriaVideo.AddOrUpdate(u => u.Id,
                //    new GaleriaVideo
                //    {
                //        Id = 1,
                //        Titulo = "Teste Galeria Video",
                //        Descricao = "descricao galeira video",
                //        Destaque = true,
                //        DataPublicacao = DateTime.Now,
                //        LinkVideo = "_uY5SThmZzw",
                //        GaleriaVideoCategoriaId = 1,
                //        Slug = "teste-video",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.Tag.AddOrUpdate(u => u.Id,
                //    new Tag
                //    {
                //        Id = 9,
                //        RegistroId = 1,
                //        AreaId = 14,
                //        Descricao = "Teste",
                //        Slug = "teste",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.GaleriaAudioCategoria.AddOrUpdate(u => u.Id,
                //    new GaleriaAudioCategoria
                //    {
                //        Id = 1,
                //        Descricao = "Teste Galeira de Audio Categoria",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.GaleriaAudio.AddOrUpdate(u => u.Id,
                //    new GaleriaAudio
                //    {
                //        Id = 1,
                //        Titulo = "Teste Galeria Video",
                //        Descricao = "descricao galeira video",
                //        Destaque = true,
                //        DataPublicacao = DateTime.Now,
                //        Audio = "Teste.mp3",
                //        GaleriaAudioCategoriaId = 1,
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.Tag.AddOrUpdate(u => u.Id,
                //    new Tag
                //    {
                //        Id = 10,
                //        RegistroId = 1,
                //        AreaId = 15,
                //        Descricao = "Teste",
                //        Slug = "teste",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.ServicoCategoria.AddOrUpdate(u => u.Id,
                //    new ServicoCategoria
                //    {
                //        Id = 1,
                //        Descricao = "Cidad�o",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    },
                //    new ServicoCategoria
                //    {
                //        Id = 2,
                //        Descricao = "Empreendedor",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    },
                //    new ServicoCategoria
                //    {
                //        Id = 3,
                //        Descricao = "Estudante",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    },
                //    new ServicoCategoria
                //    {
                //        Id = 4,
                //        Descricao = "Servidor P�blico",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.Servico.AddOrUpdate(u => u.Id,
                //    new Servico
                //    {
                //        Id = 1,
                //        Icone = "icone.jpg",
                //        Nome = "Servico Teste",
                //        Slug = "servico-teste",
                //        LinkExterno = "www.google.com",
                //        ServicoCategoriaId = 1,
                //        Imagem = "Teste.jpg",
                //        Descricao = "descricao galeira video",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.ServicoArquivo.AddOrUpdate(u => u.Id,
                //    new ServicoArquivo
                //    {
                //        Id = 1,
                //        Arquivo = "Teste.pdf",
                //        ArquivoNome = "Documento Teste",
                //        ServicoId = 1,
                //        Tamanho = "15MB",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.ServicoPin.AddOrUpdate(u => u.Id,
                //    new ServicoPin
                //    {
                //        Id = 1,
                //        ServicoId = 1,
                //        Latitude = "-22.550057",
                //        Longitude = "-44.176994",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.SecretariaServico.AddOrUpdate(u => u.Id,
                //    new SecretariaServico
                //    {
                //        Id = 1,
                //        ServicoId = 1,
                //        SecretariaId = 1,
                //        Ordem = 1,
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.SecretariaServico.AddOrUpdate(u => u.Id,
                //    new SecretariaServico
                //    {
                //        Id = 2,
                //        ServicoId = 1,
                //        SecretariaId = 2,
                //        Ordem = 1,
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.Enquete.AddOrUpdate(u => u.Id,
                //    new Enquete
                //    {
                //        Id = 1,
                //        Pergunta = "Teste e pergunta",
                //        DataPublicacao = DateTime.Now,
                //        DataEncerramento = DateTime.Now.AddDays(-1),
                //        DataInicial = DateTime.Now.AddDays(-2),
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.EnqueteOpcao.AddOrUpdate(u => u.Id,
                //    new EnqueteOpcao
                //    {
                //        Id = 1,
                //        Opcao = "Op��o 1",
                //        EnqueteId = 1,
                //        Ordem = 0,
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.EnqueteOpcao.AddOrUpdate(u => u.Id,
                //    new EnqueteOpcao
                //    {
                //        Id = 2,
                //        Opcao = "Op��o 2",
                //        EnqueteId = 1,
                //        Ordem = 1,
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.EnqueteOpcao.AddOrUpdate(u => u.Id,
                //    new EnqueteOpcao
                //    {
                //        Id = 3,
                //        Opcao = "Op��o 3",
                //        EnqueteId = 1,
                //        Ordem = 2,
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.EnqueteOpcao.AddOrUpdate(u => u.Id,
                //    new EnqueteOpcao
                //    {
                //        Id = 4,
                //        Opcao = "Op��o 4",
                //        EnqueteId = 1,
                //        Ordem = 3,
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.EnqueteOpcao.AddOrUpdate(u => u.Id,
                //    new EnqueteOpcao
                //    {
                //        Id = 5,
                //        Opcao = "Op��o 5",
                //        EnqueteId = 1,
                //        Ordem = 4,
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.EnqueteResposta.AddOrUpdate(u => u.Id,
                //    new EnqueteResposta
                //    {
                //        Id = 1,
                //        DataResposta = DateTime.Now,
                //        EnqueteOpcaoId = 2
                //    });
                //context.SaveChanges();

                //context.Enquete.AddOrUpdate(u => u.Id,
                //    new Enquete
                //    {
                //        Id = 2,
                //        Pergunta = "Teste e pergunta 2",
                //        DataPublicacao = DateTime.Now,
                //        DataEncerramento = DateTime.Now.AddDays(+1),
                //        DataInicial = DateTime.Now.AddDays(-1),
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.EnqueteOpcao.AddOrUpdate(u => u.Id,
                //    new EnqueteOpcao
                //    {
                //        Id = 6,
                //        Opcao = "Op��o 1",
                //        EnqueteId = 2,
                //        Ordem = 0,
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.EnqueteOpcao.AddOrUpdate(u => u.Id,
                //    new EnqueteOpcao
                //    {
                //        Id = 7,
                //        Opcao = "Op��o 2",
                //        EnqueteId = 2,
                //        Ordem = 1,
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.EnqueteOpcao.AddOrUpdate(u => u.Id,
                //    new EnqueteOpcao
                //    {
                //        Id = 8,
                //        Opcao = "Op��o 3",
                //        EnqueteId = 2,
                //        Ordem = 2,
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.EnqueteOpcao.AddOrUpdate(u => u.Id,
                //    new EnqueteOpcao
                //    {
                //        Id = 9,
                //        Opcao = "Op��o 4",
                //        EnqueteId = 2,
                //        Ordem = 3,
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.EnqueteOpcao.AddOrUpdate(u => u.Id,
                //    new EnqueteOpcao
                //    {
                //        Id = 10,
                //        Opcao = "Op��o 5",
                //        EnqueteId = 2,
                //        Ordem = 4,
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.EnqueteResposta.AddOrUpdate(u => u.Id,
                //    new EnqueteResposta
                //    {
                //        Id = 2,
                //        DataResposta = DateTime.Now,
                //        EnqueteOpcaoId = 8
                //    });
                //context.SaveChanges();

                //context.Bairro.AddOrUpdate(u => u.Id,
                //    new Bairro
                //    {
                //        Id = 1,
                //        Descricao = "Bairro Teste",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.ContatoCategoria.AddOrUpdate(u => u.Id,
                //    new ContatoCategoria
                //    {
                //        Id = 1,
                //        Descricao = "Fale Conosco",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    },
                //    new ContatoCategoria
                //    {
                //        Id = 2,
                //        Descricao = "Sugest�es",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    }
                //    );
                //context.SaveChanges();

                //context.ContatoTipo.AddOrUpdate(u => u.Id,
                //    new ContatoTipo
                //    {
                //        Id = 1,
                //        Descricao = "Tipo Fale Conosco teste",
                //        ContatoCategoriaId = 1,
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.ContatoTipo.AddOrUpdate(u => u.Id,
                //    new ContatoTipo
                //    {
                //        Id = 2,
                //        Descricao = "Tipo Sugest�o teste",
                //        ContatoCategoriaId = 2,
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();


                //context.StatusPublicacao.AddOrUpdate(u => u.Id,
                //    new StatusPublicacao
                //    {
                //        Id = 1,
                //        Descricao = "Publicado",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    },
                //    new StatusPublicacao
                //    {
                //        Id = 2,
                //        Descricao = "Impugnado",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    },
                //    new StatusPublicacao
                //    {
                //        Id = 3,
                //        Descricao = "Suspenso",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    },
                //    new StatusPublicacao
                //    {
                //        Id = 4,
                //        Descricao = "Republicado",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    },
                //    new StatusPublicacao
                //    {
                //        Id = 5,
                //        Descricao = "Homologado",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    },
                //    new StatusPublicacao
                //    {
                //        Id = 6,
                //        Descricao = "Anulado",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.LicitacaoModalidade.AddOrUpdate(u => u.Id,
                //    new LicitacaoModalidade
                //    {
                //        Id = 1,
                //        Descricao = "Tomada de Pre�o",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    },
                //    new LicitacaoModalidade
                //    {
                //        Id = 2,
                //        Descricao = "Convite",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    },
                //    new LicitacaoModalidade
                //    {
                //        Id = 3,
                //        Descricao = "Chamada P�blica",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    },
                //    new LicitacaoModalidade
                //    {
                //        Id = 4,
                //        Descricao = "Concorr�ncia P�blica",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    },
                //    new LicitacaoModalidade
                //    {
                //        Id = 5,
                //        Descricao = "Dispensa de Licita��o",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    },
                //    new LicitacaoModalidade
                //    {
                //        Id = 6,
                //        Descricao = "Inexigibilidade",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    },
                //    new LicitacaoModalidade
                //    {
                //        Id = 7,
                //        Descricao = "Leil�o",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    },
                //    new LicitacaoModalidade
                //    {
                //        Id = 8,
                //        Descricao = "Preg�o Eletr�nico",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    },
                //    new LicitacaoModalidade
                //    {
                //        Id = 9,
                //        Descricao = "Preg�o Presencial",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.Licitacao.AddOrUpdate(u => u.Id,
                //    new Licitacao
                //    {
                //        Id = 1,
                //        Titulo = "Licita��o Teste",
                //        NumeroNome = "123",
                //        Descricao = "Descri��o da Licita��o Teste",
                //        LicitacaoModalidadeId = 1,
                //        Slug = "licitacao-teste",
                //        DataAbertura = DateTime.Now.AddDays(-10),
                //        DataPublicacao = DateTime.Now,
                //        Numero = "4286",
                //        StatusPublicacaoId = 1,
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.LicitacaoArquivo.AddOrUpdate(u => u.Id,
                //    new LicitacaoArquivo
                //    {
                //        Id = 1,
                //        Arquivo = "Teste.pdf",
                //        ArquivoNome = "Documento Teste",
                //        LicitacaoId = 1,
                //        Tamanho = "15MB",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.ConcursoModalidade.AddOrUpdate(u => u.Id,
                //    new ConcursoModalidade
                //    {
                //        Id = 1,
                //        Descricao = "Concurso",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    },
                //    new ConcursoModalidade
                //    {
                //        Id = 2,
                //        Descricao = "Convoca��o e Processo Seletivo",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.Concurso.AddOrUpdate(u => u.Id,
                //    new Concurso
                //    {
                //        Id = 1,
                //        Titulo = "Concurso Teste",
                //        Descricao = "Descri��o da Concurso Teste",
                //        ConcursoModalidadeId = 1,
                //        Slug = "concurso-teste",
                //        DataInicio = DateTime.Now.AddDays(-10),
                //        DataFim = DateTime.Now.AddDays(10),
                //        Numero = "4286",
                //        StatusPublicacaoId = 1,
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.ConcursoArquivo.AddOrUpdate(u => u.Id,
                //    new ConcursoArquivo
                //    {
                //        Id = 1,
                //        Arquivo = "Teste.pdf",
                //        ArquivoNome = "Documento Teste",
                //        ConcursoId = 1,
                //        Tamanho = "15MB",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.LegislacaoCategoria.AddOrUpdate(u => u.Id,
                //    new LegislacaoCategoria
                //    {
                //        Id = 1,
                //        Descricao = "C�digo de Obras",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    },
                //    new LegislacaoCategoria
                //    {
                //        Id = 2,
                //        Descricao = "C�digo Tribut�rio",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    },
                //    new LegislacaoCategoria
                //    {
                //        Id = 3,
                //        Descricao = "Decretos",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    },
                //    new LegislacaoCategoria
                //    {
                //        Id = 4,
                //        Descricao = "Estatutos",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    },
                //    new LegislacaoCategoria
                //    {
                //        Id = 5,
                //        Descricao = "Lei Org�nica",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    },
                //    new LegislacaoCategoria
                //    {
                //        Id = 6,
                //        Descricao = "Lei Ordin�ria",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    },
                //    new LegislacaoCategoria
                //    {
                //        Id = 7,
                //        Descricao = "Leis",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    },
                //    new LegislacaoCategoria
                //    {
                //        Id = 8,
                //        Descricao = "Plano Diretor",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.Legislacao.AddOrUpdate(u => u.Id,
                //    new Legislacao
                //    {
                //        Id = 1,
                //        Titulo = "Legisla��o Teste",
                //        Descricao = "Descri��o da Legisla��o Teste",
                //        LegislacaoCategoriaId = 1,
                //        DataPublicacao = DateTime.Now,
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.LegislacaoArquivo.AddOrUpdate(u => u.Id,
                //    new LegislacaoArquivo
                //    {
                //        Id = 1,
                //        Arquivo = "Teste.pdf",
                //        ArquivoNome = "Documento Teste",
                //        LegislacaoId = 1,
                //        Tamanho = "15MB",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.PerguntasFrequentesCategoria.AddOrUpdate(u => u.Id,
                //    new PerguntasFrequentesCategoria
                //    {
                //        Id = 1,
                //        Descricao = "Teste Categoria Perguntas Frequentes",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.PerguntasFrequentes.AddOrUpdate(u => u.Id,
                //    new PerguntasFrequentes
                //    {
                //        Id = 1,
                //        Titulo = "Teste de pergunta frequente",
                //        Descricao = "resposta do teste de pergunta frequente",
                //        PerguntasFrequentesCategoriaId = 1,
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.AaZCategoria.AddOrUpdate(u => u.Id,
                //    new AaZCategoria
                //    {
                //        Id = 1,
                //        Descricao = "Teste Categoria AaZ",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.AaZ.AddOrUpdate(u => u.Id,
                //    new AaZ
                //    {
                //        Id = 1,
                //        Titulo = "Teste de AaZ",
                //        Url = "www.google.com",
                //        AaZCategoriaId = 1,
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();

                //context.Timeline.AddOrUpdate(u => u.Id,
                //    new Timeline
                //    {
                //        Id = 1,
                //        Descricao = "Aconteceu algo",
                //        Ano = "2016",
                //        Imagem = "teste.jpg",
                //        Status = (int)StatusPadrao.Ativo,
                //        DataCadastro = DateTime.Now
                //    });
                //context.SaveChanges();


            }
            //catch (Exception e)
            //{
            //    string Erro = "";

            //    if (e.Message != null)
            //        Erro += "MENSAGEM ----> " + e.Message.ToString();
            //    if (e.InnerException != null)
            //        Erro += "INNER EXCEPTION ----> " + e.InnerException.ToString();

            //    throw new DbEntityValidationException(
            //        "Entity Validation Failed - errors follow:\n" +
            //        Erro, e
            //    ); // Add the original exception as the innerException
            //}
            catch (DbEntityValidationException ex)
            {
                StringBuilder sb = new StringBuilder();

                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }

                throw new DbEntityValidationException(
                    "Entity Validation Failed - errors follow:\n" +
                    sb.ToString(), ex
                ); // Add the original exception as the innerException
            }
        }
    }
}
