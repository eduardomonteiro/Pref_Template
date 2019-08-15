using Prefeitura_Template.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Globalization;
using GridMvc;
using System.Web.Helpers;
using ClosedXML.Excel;
using System.ComponentModel;

namespace Prefeitura_Template.Areas.Admin.Utils
{
    public static class Utils
    {
        public static string TamanhoEmMB(double Size = 0)
        {
            if(Size == 0)
            {
                return "";
            }

            double KByte = Math.Round((Size / 1024), 2);

            if(KByte > 1000)
            {
                double MByte = Math.Round((KByte / 1024), 2);

                return MByte.ToString() + " MB";
            }

            return KByte.ToString() + " KB";
        }

        public static Dictionary<string, string> clima = new Dictionary<string, string>()
        {
            {"ec", "Encoberto com Chuvas Isoladas"},
            {"ci", "Chuvas Isoladas"},
            {"c", "Chuva"},
            {"in", "Instável"},
            {"pp", "Possibilidade de Pancadas de Chuva"},
            {"cm", "Chuva pela Manhã"},
            {"cn", "Chuva a Noite"},
            {"pt", "Pancadas de Chuva a Tarde"},
            {"pm", "Pancadas de Chuva pela Manhã"},  
            {"np", "Nublado e Pancadas de Chuva"},  
            {"pc", "Pancadas de Chuva"},  
            {"pn", "Parcialmente Nublado"},  
            {"cv", "Chuvisco"},  
            {"ch", "Chuvoso"},  
            {"t", "Tempestade"}, 
            {"ps", "Predomínio de Sol"},  
            {"e", "Encoberto"},  
            {"n", "Nublado"},  
            {"cl", "Céu Claro"},
            {"nv", "Nevoeiro"},  
            {"g", "Geada"},   
            {"ne", "Neve"}, 
            {"nd", "Não Definido"}, 
            {"pnt", "Pancadas de Chuva a Noite"}, 
            {"psc", "Possibilidade de Chuva"}, 
            {"pcm", "Possibilidade de Chuva pela Manhã"},
            {"pct", "Possibilidade de Chuva a Tarde"},
            {"pcn", "Possibilidade de Chuva a Noite"},
            {"npt", "Nublado com Pancadas a Tarde"}, 
            {"npn", "Nublado com Pancadas a Noite"}, 
            {"ncn", "Nublado com Possibilidade de Chuva a Noite"}, 
            {"nct", "Nublado com Possibilidade de Chuva a Tarde"}, 
            {"ncm", "Nublado com Possibilidade de Chuva pela Manhã"},
            {"npm", "Nublado com Pancadas pela Manhã"}, 
            {"npp", "Nublado com Possibilidade de Chuva"}, 
            {"vn", "Variação de Nebulosidade"}, 
            {"ct", "Chuva a Tarde"},
            {"ppn", "Possibilidade de Pancadas de Chuva a Noite"},
            {"ppt", "Possibilidade de Pancadas de Chuva a Tarde"},
            {"ppm", "Possibilidade de Pancadas de Chuva pela Manhã" },
            {"nppc", "Nublado com Possibilidade de Pancadas de Chuva" },
            {"nppcm", "Nublado com Possibilidade de Pancadas de Chuva de Manhã" },
            {"nppct", "Nublado com Possibilidade de Pancadas de Chuva a Tarde" },
            {"nppcn", "Nublado com Possibilidade de Pancadas de Chuva a Noite" }
        };

        public static string UploadImagem(HttpPostedFileBase file, string dir, string[] extensions)
        {
            try
            {
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                if (file == null)
                {
                    return "";
                }
                var fullFileName = file.FileName;

                if (file.ContentLength > 15000000)
                {
                    return "Erro: O arquivo enviado excedeu o tamanho máximo de 15MB!";
                }

                var filename = Path.GetFileNameWithoutExtension(fullFileName);

                var extension = Path.GetExtension(file.FileName.ToLower());
                if (!extensions.Contains(extension))
                {
                    return string.Format("Erro: O arquivo enviado não possui o formato correto. Somente arquivos {0}!" , string.Join(", ", extensions));
                }

                string finalName = DateTime.UtcNow.ToString("ddMMyyyyHHmmssfff", CultureInfo.InvariantCulture) + extension;

                var finalFullPath = Path.Combine(dir, finalName);

                int j = 0;
                while (File.Exists(finalFullPath))
                {
                    j++;
                    finalName = finalName + "_" + j + extension;
                    finalFullPath = Path.Combine(dir, finalName);
                }

                file.SaveAs(finalFullPath);

                return finalName;
            }
            catch(Exception e)
            {
                string Message = "";
                if(e.Message != null)
                {
                    Message += "Mensagem -> " + e.Message.ToString();
                }
                if (e.InnerException != null)
                {
                    Message += " Excpection -> " + e.InnerException.ToString();
                }

                return "Erro: Ocorreu um erro desconhecido ao salvar a imagem -> " + Message;
            }
        }

        public static void VerificaPermissoesUsuario(int AreaId,
            string UsuarioId,
            bool filtrarCriacao = false,
            bool filtrarLeitura = false,
            bool filtrarAlteracao = false,
            bool filtrarExclusao = false)
        {

            try
            {
                var db = new ApplicationDbContext();

                int CodUser = int.Parse(UsuarioId);
                using (db)
                {
                    Usuario Usuario = db.Usuario.Where(x => x.Id == CodUser).FirstOrDefault();
                    if (Usuario.Email != "admin@mysite.com.br")
                    {
                        var pu = db.Perfil_Area.Where(x => x.AreaId == AreaId && x.PerfilId == Usuario.PerfilId).AsQueryable().ToList();
                        if (filtrarCriacao)
                            pu = pu.Where(y => y.Criacao == true).ToList();

                        if (filtrarLeitura)
                            pu = pu.Where(y => y.Leitura == true).ToList();

                        if (filtrarAlteracao)
                            pu = pu.Where(y => y.Alteracao == true).ToList();

                        if (filtrarExclusao)
                            pu = pu.Where(y => y.Exclusao == true).ToList();

                        if (!pu.Any())
                            HttpContext.Current.Response.Redirect("~/Admin/Home/AcessoRestrito");
                    }
                }

            }
            catch
            {
                HttpContext.Current.Response.Redirect("~/Admin/Home/AcessoRestrito");
            }
        }

        public static bool VerificaPermissoesUsuarioBool(int AreaId,
            string UsuarioId,
            bool filtrarCriacao = false,
            bool filtrarLeitura = false,
            bool filtrarAlteracao = false,
            bool filtrarExclusao = false)
        {

            try
            {
                var db = new ApplicationDbContext();
                int CodUser = int.Parse(UsuarioId);
                using (db)
                {
                    Usuario Usuario = db.Usuario.Where(x => x.Id == CodUser).FirstOrDefault();
                    if (Usuario.Email != "admin@mysite.com.br")
                    {
                        var pu = db.Perfil_Area.Where(x => x.AreaId == AreaId && x.PerfilId == Usuario.PerfilId).AsQueryable().ToList();
                        if (filtrarCriacao)
                            pu = pu.Where(y => y.Criacao == true).ToList();

                        if (filtrarLeitura)
                            pu = pu.Where(y => y.Leitura == true).ToList();

                        if (filtrarAlteracao)
                            pu = pu.Where(y => y.Alteracao == true).ToList();

                        if (filtrarExclusao)
                            pu = pu.Where(y => y.Exclusao == true).ToList();

                        if (!pu.Any())
                            return false;
                        else
                            return true;
                    }
                    else
                    {
                        return true;
                    }
                }

            }
            catch
            {
                return false;
            }
        }

        public static List<Perfil_Area> VerificaPermissoesUsuario(int UsuarioId)
        {
            var db = new ApplicationDbContext();

            using (db)
            {
                Usuario Usuario = db.Usuario.Where(x => x.Id == UsuarioId).FirstOrDefault();
                var pu = db.Perfil_Area.Where(x => x.PerfilId == Usuario.PerfilId).AsQueryable().ToList();
                if (pu != null)
                {
                    return pu;
                }
            }

            return null;
        }

        public static void ExportToExcel(IEnumerable<object> data, string sheetName)
        {
            XLWorkbook wb = new XLWorkbook();

            var ws = wb.Worksheets.Add(sheetName);

            var count = data.FirstOrDefault().GetType().GetProperties().ToList().Count();

            var columns = data.FirstOrDefault().GetType().GetProperties().ToList();

            //foreach (var t in columns)
            //{
            //    int index = columns.IndexOf(t) + 1;

            //    ws.Cell(1, index).Value = GetDisplayName(t);
            //}

            var lista = data.ToList();


            for (var i = 0; i < lista.Count(); i++)
            {
                foreach (var ts in lista[i].GetType().GetProperties())
                {
                    if (ts.GetValue(lista[i]) != null)
                    {

                        if (ts.GetType() == typeof(string))
                        {
                            ts.SetValue(lista[i], Regex.Replace(GetDisplayName(ts), "<.*?>", string.Empty));
                        }
                    }

                }
            }

            ws.Cell(1, 1).InsertTable(lista);

            ws.Columns().AdjustToContents();
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            HttpContext.Current.Response.AddHeader("content-disposition", String.Format(@"attachment;filename={0}.xlsx", sheetName.Replace(" ", "_")));

            using (MemoryStream memoryStream = new MemoryStream())
            {
                wb.SaveAs(memoryStream, false);
                memoryStream.WriteTo(HttpContext.Current.Response.OutputStream);
                memoryStream.Close();
            }

            HttpContext.Current.Response.End();
        }

        public static string EnviaEmail(string body, string subject, string emailTo, List<FileStream> fileStream)
        {
            try
            {
                var mail = new MailMessage();

                //mail.To.Add("comunicacaopinheiral@gmail.com");
                mail.To.Add(emailTo);
                mail.Subject = subject;
                mail.Body = body;
                mail.From = new MailAddress("Contato <contato@pinheiral.rj.gov.br>");
                mail.IsBodyHtml = true;

                if (fileStream != null)
                {
                    foreach (var f in fileStream)
                    {
                        f.Position = 0;
                        if (f.Name.Contains("pdf"))
                        {
                            mail.Attachments.Add(new Attachment(f, Path.GetFileName(f.Name), "application/pdf"));
                        }
                        if (f.Name.Contains("xls"))
                        {
                            mail.Attachments.Add(new Attachment(f, Path.GetFileName(f.Name), "application/ms-excel"));
                        }
                        f.Flush();
                    }

                    fileStream.Clear();
                }

                var smtp = new SmtpClient
                {
                    Host = "smtp.pinheiral.rj.gov.br",
                    Port = 587,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("contato@pinheiral.rj.gov.br", "taquye6_A-e7*"),
                    EnableSsl = false
                };


                    try
                    {
                        smtp.Send(mail);
                    }
                    catch (Exception e)
                    {

                         return e.ToString();
                    }

                    return "OK";
                
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        public static List<long> MontarLista(IGrid Model)
        {
            var lista = new List<long>();

            //pega os membros da model
            // vide bindingflasg em https://msdn.microsoft.com/pt-br/library/system.reflection.bindingflags(v=vs.110).aspx
            // no caso preciso pegar o _source que é um parâmentro  que é declarado como non-public dentro da instancia IGrid
            var properties = Model
                    .GetType().GetMembers(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);

            Dictionary<string, object> propertiesDic = new Dictionary<string, object>();

            //carregando todos os membros, especifico o que eu quero, _source, e salvo no dicionário isolando ele
            foreach (MemberInfo prop in properties.Where(x => x.Name == "_source"))
            {
                propertiesDic.Add(prop.Name, MemberInfoGetValue(prop, Model));
            }

            //separo em uma variável
            var grid = propertiesDic["_source"];

            //dentro de _scource existe o GridItems, que são TODOS os registros não paginados
            foreach (var props in grid.GetType().GetProperties().Where(x => x.Name == "GridItems"))
            {
                //pego essa lista de itens em formato IEnumerable e tranformo em uma List 
                var enumeraveis = ((IEnumerable<object>)props.GetValue(grid, null)).ToList();

                //ordenação feita com base no filtro vindo da URL
                enumeraveis = Ordernar(enumeraveis);

                foreach (var p in enumeraveis)
                {
                    //preencho a lista com a coluna "Id" para criar os inputs com os Ids ordenados
                    lista.Add((int)p.GetType().GetProperties().ToList().Where(x => x.Name == "Id").FirstOrDefault().GetValue(p));
                }
            }

            //retorno a lista
            return lista;
        }

        public static List<object> Ordernar(this List<object> obj)
        {
            //pego as variaveis da url
            var coluna = HttpContext.Current.Request.QueryString["grid-column"];
            var order = HttpContext.Current.Request.QueryString["grid-dir"];

            //verifico se há ordenação
            if (order != null && obj.Count() > 0)
            {
                //ordeno se acordo com o parametro setado 0 ou 1
                if (order == "0")
                {
                    SortList(obj, coluna, SortDirection.Ascending);
                }
                else
                {
                    SortList(obj, coluna, SortDirection.Descending);
                }
            }

            //retorno o obj
            return obj;
        }

        public static void SortList<T>(this List<T> list, string columnName, SortDirection direction)
        {
            var property = list.FirstOrDefault().GetType().GetProperty(columnName);
            var multiplier = direction == SortDirection.Descending ? -1 : 1;
            list.Sort((t1, t2) =>
            {
                var col1 = property.GetValue(t1);
                var col2 = property.GetValue(t2);
                return multiplier * Comparer<object>.Default.Compare(col1, col2);
            });
        }

        public static object MemberInfoGetValue(this MemberInfo memberInfo, object forObject)
        {
            switch (memberInfo.MemberType)
            {
                case MemberTypes.Field:
                    return ((FieldInfo)memberInfo).GetValue(forObject);
                case MemberTypes.Property:
                    return ((PropertyInfo)memberInfo).GetValue(forObject);
                default:
                    return null;
            }
        }

        public static string GerarSlug(this string phrase)
        {
            string str = RemoverAcento(phrase).ToLower();
            // invalid chars           
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            str = str.Replace("-", "");
            str = str.Replace(":", "");
            // convert multiple spaces into one space   
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // cut and trim 
            str = str.Substring(0, str.Length).Trim();
            str = Regex.Replace(str, @"\s", "-"); // hyphens   
            return str;
        }

        public static string RemoverAcento(this string txt)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }

        public static string RemoveDiacritics(string input)
        {
            if (input == null)
                input = "";

            string stFormD = input.Normalize(NormalizationForm.FormD);
            int len = stFormD.Length;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < len; i++)
            {
                System.Globalization.UnicodeCategory uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(stFormD[i]);
                if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[i]);
                }
            }
            var retorno = sb.ToString().Normalize(NormalizationForm.FormC);
            return retorno;
        }

        public static string RemoverHtml(this string texto)
        {
            if (texto == null)
            {
                return "";
            }

            return Regex.Replace(texto, @"<[^>]+>|&nbsp;", string.Empty).Trim();
        }

        public static string GetDisplayName(this PropertyInfo prop)
        {
            if (prop.CustomAttributes == null || prop.CustomAttributes.Count() == 0)
                return prop.Name;

            var displayNameAttribute = prop.CustomAttributes.Where(x => x.AttributeType == typeof(DisplayNameAttribute)).FirstOrDefault();

            if (displayNameAttribute == null || displayNameAttribute.ConstructorArguments == null || displayNameAttribute.ConstructorArguments.Count == 0)
                return prop.Name;

            return displayNameAttribute.ConstructorArguments[0].Value.ToString() ?? prop.Name;
        }

        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()
                            .GetName();
        }

        public static string RetornaDiretorioExPrefeitos()
        {
            return "/API/Areas/Admin/Conteudo/ExPrefeito/";
        }

        public static string RetornaDiretorioCidade()
        {
            return "/API/Areas/Admin/Conteudo/Cidade/";
        }
        public static string RetornaDiretorioTimeLine()
        {
            return "/API/Areas/Admin/Conteudo/TimeLine/";
        }

        public static string RetornaDiretorioPatrimonio()
        {
            return "/API/Areas/Admin/Conteudo/Patrimonio/";
        }

        public static string RetornaDiretorioPerfilSocioEconomico()
        {
            return "/API/Areas/Admin/Conteudo/PerfilSocioEconomico/";
        }

        public static string RetornaDiretorioTurismo()
        {
            return "/API/Areas/Admin/Conteudo/Turismo/";
        }

        public static string RetornaDiretorioNoticia()
        {
            return "/API/Areas/Admin/Conteudo/Noticia/";
        }
        public static string RetornaDiretorioProjeto()
        {
            return "/API/Areas/Admin/Conteudo/Projeto/";
        }

        public static string RetornaDiretorioEvento()
        {
            return "/API/Areas/Admin/Conteudo/Evento/";
        }
        public static string RetornaDiretorioDocumento()
        {
            return "/API/Areas/Admin/Conteudo/Documento/";
        }

        public static string RetornaDiretorioLegislacao()
        {
            return "/API/Areas/Admin/Conteudo/Legislacao/";
        }

        public static string RetornaDiretorioServico()
        {
            return "/API/Areas/Admin/Conteudo/Servico/";
        }
        
        public static string RetornaDiretorioLicitacao()
        {
            return "/API/Areas/Admin/Conteudo/Licitacao/";
        }

        public static string RetornaDiretorioConcurso()
        {
            return "/API/Areas/Admin/Conteudo/Concurso/";
        }

        public static string RetornaDiretorioSecretaria()
        {
            return "/API/Areas/Admin/Conteudo/Secretaria/";
        }

        public static string RetornaDiretorioInformativo()
        {
            return "/API/Areas/Admin/Conteudo/Informativo/";
        }

        public static string RetornaDiretorioGaleriaImagem()
        {
            return "/API/Areas/Admin/Conteudo/GaleriaFoto/";
        }

        public static string RetornaDiretorioAudio()
        {
            return "/API/Areas/Admin/Conteudo/Audio/";
        }
    }
}