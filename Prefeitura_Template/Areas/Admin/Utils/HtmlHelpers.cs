using System;
using System.Text;
using System.Web;
using System.Web.Mvc;
using GridMvc.Columns;
using GridMvc.Html;
using GridMvc;
using Prefeitura_Template.Models;

namespace Prefeitura_Template.Areas.Admin.Utils
{
    public static class HtmlExtensions
    {
        public static MvcHtmlString SetMenu(this HtmlHelper helper, string controller, string action, string icon, string label, bool parentMenu = false, bool active = false)
        {
            string html = "";
            var url = new UrlHelper(helper.ViewContext.RequestContext);
            var link = url.Action(action, controller);
            if (parentMenu)
            {
                if (!icon.Contains("fa"))
                {
                    html = @"<li class=""treeview{2}"">
                                <a href=""#"">
                                    <i class=""{0}""></i><span>{1}</span> <i class=""fa fa-angle-left pull-right""></i>
                                </a><ul class=""treeview-menu"">";
                }
                else
                {
                    html = @"<li class=""treeview{2}"">
                                <a href=""#"">
                                    <i class=""fa {0}""></i><span>{1}</span> <i class=""fa fa-angle-left pull-right""></i>
                                </a><ul class=""treeview-menu"">";
                }
                return MvcHtmlString.Create(string.Format(html, icon, label, active ? " active" : ""));
            }
            else
            {
                if (!icon.Contains("fa-"))
                {
                    html = @"<li>
                        <a href=""{0}"">
                            <i class=""{1}""></i><span>{2}</span>
                        </a>
                    </li>";
                }
                else
                {
                    html = @"<li>
                        <a href=""{0}"">
                            <i class=""fa {1}""></i><span>{2}</span>
                        </a>
                    </li>";
                }
                return MvcHtmlString.Create(string.Format(html, link, icon, label));
            }  
        }

        public static MvcHtmlString SetMenuChild(this HtmlHelper helper, string controller, string action, string subicon, string sublabel)
        {
            string html = "";
            var url = new UrlHelper(helper.ViewContext.RequestContext);
            var link = url.Action(action, controller);
            if (!subicon.Contains("fa-"))
            {
                html = @"<li>
                            <a href=""{0}"">
                                <i class=""{1}""></i><span>{2}</span>
                            </a>
                        </li>";
            }
            else
            {
                html = @"<li>
                            <a href=""{0}"">
                                <i class=""fa {1}""></i><span>{2}</span>
                            </a>
                        </li>";
            }

            return MvcHtmlString.Create(string.Format(html, link, subicon, sublabel));
        }

        public static IHtmlString StatusFaleConosco(string text)
        {
            string cssClass = "label-primary";

            if (text == "Em Análise")
            {
                cssClass = "label-warning";
            }
            else if (text == "Respondido")
            {
                cssClass = "label-success";
            }

            return new HtmlString("<span class='label " + cssClass + "'>" + text + "</span>");
        }

        public static IHtmlString Label(string text, string expected)
        {
            var cssClass = "label-success";
            if (text != expected)
            {
                cssClass = "label-danger";
            }
            return new HtmlString("<span class='label " + cssClass + "'>" + text + "</span>");
        }

        public static IHtmlString Label(bool condition, string textTrue, string textFalse)
        {
            var cssClass = "label-success";
            var text = textTrue;
            if (!condition)
            {
                if (string.IsNullOrEmpty(textFalse))
                {
                    return new HtmlString("");
                }
                cssClass = "label-danger";
                text = textFalse;
            }
            else
            {
                if (string.IsNullOrEmpty(textTrue))
                {
                    return new HtmlString("");
                }
            }
            return new HtmlString("<span class='label " + cssClass + "'>" + text + "</span>");
        }

        public static IHtmlString LabelNota(int condition, string textTrue, string textFalse)
        {
            var cssClass = "label-success";
            var text = textTrue;
            /*
             *  [Display(Name = "Não Conferida")]
            NaoConferido = 0,
            [Display(Name = "Conferida Automaticamente")]
            ConferidoAutomaticamente = 1,
            [Display(Name = "Conferida Manualmente")]
            ConferidoManualmente = 2,
            [Display(Name = "Nota Inválida")]
            Invalido = 3,
            [Display(Name = "Estornada")]
            Estornado = 4
             */
            if (condition == 3)
            {
                if (string.IsNullOrEmpty(textFalse))
                {
                    return new HtmlString("");
                }
                cssClass = "label-invalido";
                text = textFalse;
            }
            else if (condition == 0 || condition == 4)
            {
                if (string.IsNullOrEmpty(textFalse))
                {
                    return new HtmlString("");
                }
                cssClass = "label-danger";
                text = textFalse;
            }
            else
            {
                if (string.IsNullOrEmpty(textTrue))
                {
                    return new HtmlString("");
                }
            }
            return new HtmlString("<span class='label " + cssClass + "'>" + text + "</span>");
        }

        public static IHtmlString If(this HtmlHelper helper, bool condition, string textTrue, string textFalse = "")
        {
            if (condition)
            {
                return new HtmlString(textTrue);
            }
            return new HtmlString(textFalse);
        }

        public static IHtmlString IIF(this HtmlHelper helper, bool condition, string text, string expected)
        {
            return Label(text, expected);
        }

        public static IHtmlString Label(this HtmlHelper helper, object value, Enum expected)
        {
            var cssClass = "label-success";
            if (Convert.ToInt32(value) != Convert.ToInt32(expected))
            {
                cssClass = "label-danger";
            }
            return new HtmlString("<span class='label " + cssClass + "'>" + EnumExtensions.GetEnumDisplayName(expected.GetType(), value) + "</span>");
        }

        public static IHtmlString Label(this HtmlHelper helper, bool condition, string textTrue, string textFalse)
        {
            return Label(condition, textTrue, textFalse);
        }

        public static IHtmlString Label(this HtmlHelper helper, object value, Enum expected, Enum expectedYellow)
        {
            var cssClass = "label-danger";
            if (Convert.ToInt32(value) == Convert.ToInt32(expectedYellow))
            {
                cssClass = "label-warning";
            }
            else if (Convert.ToInt32(value) == Convert.ToInt32(expected))
            {
                cssClass = "label-success";
            }
            return new HtmlString("<span class='label " + cssClass + "'>" + EnumExtensions.GetEnumDisplayName(expected.GetType(), value) + "</span>");
        }

        public static IHtmlString BotoesPadroes(this HtmlHelper helper, UrlHelper url, long modelId, string modelTitle, bool modelEnabled = false, bool details = true, bool edit = true, bool enableDisable = true, bool delete = true, bool padrao = false, bool padraoEnabled = true, string descPadrao = "", string controller = "")
        {
            var html = new StringBuilder();
            html.Append("<text>");
            if (padrao)
            {
                if (!padraoEnabled)
                {
                    html.Append("<a href=\"#\" onclick=\"javascript:Confirmar('tornar vigente', '" + descPadrao + "', '" + modelTitle + "', '" + url.Action("PadraoItemOn", new { id = modelId }) + "');return false;\" class='btn btn-github btn-sm' alt='Padrao' title='Não vigente'><i class='fa fa-star-o'></i></a> ");
                }
                else
                {
                    html.Append("<a href=\"#\" class='btn btn-github btn-sm' alt='Padrao' title='Vigente'><i class='fa fa-star' readonly></i></a> ");
                }
            }
            if (details)
            {
                if (string.IsNullOrEmpty(controller))
                    html.Append("<a href='" + url.Action("Details", new { id = modelId }) + @"' class='btn btn-primary btn-sm' alt='Detalhes' title='Detalhes'><i class='fa fa-search-plus'></i></a> ");
                else
                    html.Append("<a href='" + url.Action("Details", controller, new { id = modelId }) + @"' class='btn btn-primary btn-sm' alt='Detalhes' title='Detalhes'><i class='fa fa-search-plus'></i></a> ");
            }
            if (edit)
            {
                if (string.IsNullOrEmpty(controller))
                    html.Append("<a href='" + url.Action("Edit", new { id = modelId }) + @"' class='btn btn-info btn-sm' alt='Editar' title='Editar'><i class='fa fa-pencil'></i></a> ");
                else
                    html.Append("<a href='" + url.Action("Edit", controller, new { id = modelId }) + @"' class='btn btn-info btn-sm' alt='Editar' title='Editar'><i class='fa fa-pencil'></i></a> ");
            }
            if (enableDisable)
            {
                if (!modelEnabled)
                {
                    if (string.IsNullOrEmpty(controller))
                        html.Append("<a href=\"#\" onclick=\"javascript:unlock('" + url.Action("UnBlockItem", new { id = modelId }) + "');return false;\" class='btn btn-success btn-sm' alt='Ativar' title='Ativar'><i class='fa fa-check'></i></a> ");
                    else
                        html.Append("<a href=\"#\" onclick=\"javascript:unlock('" + url.Action("UnBlockItem", controller, new { id = modelId }) + "');return false;\" class='btn btn-success btn-sm' alt='Ativar' title='Ativar'><i class='fa fa-check'></i></a> ");
                }
                else
                {
                    if (string.IsNullOrEmpty(controller))
                        html.Append("<a href=\"#\" onclick=\"javascript:block('" + url.Action("BlockItem", new { id = modelId }) + "');return false;\" class='btn btn-warning btn-sm' alt='Desativar' title='Desativar'><i class='fa fa-ban'></i></a> ");
                    else
                        html.Append("<a href=\"#\" onclick=\"javascript:block('" + url.Action("BlockItem", controller, new { id = modelId }) + "');return false;\" class='btn btn-warning btn-sm' alt='Desativar' title='Desativar'><i class='fa fa-ban'></i></a> ");
                }
            }
            if (delete)
            {
                if (string.IsNullOrEmpty(controller))
                    html.Append("<a href=\"#\" onclick=\"javascript:mensagem('" + url.Action("Delete", new { id = modelId }) + "');return false;\" class='btn btn-danger btn-sm' alt='Excluir' title='Excluir'><i class='fa fa-trash-o'></i></a> ");
                else
                    html.Append("<a href=\"#\" onclick=\"javascript:mensagem('" + url.Action("Delete", controller, new { id = modelId }) + "');return false;\" class='btn btn-danger btn-sm' alt='Excluir' title='Excluir'><i class='fa fa-trash-o'></i></a> ");
            }

            html.Append("<text>");
            return new HtmlString(html.ToString());
        }

        public static IHtmlString BotoesPadroesAnexos(this HtmlHelper helper, UrlHelper url, bool modelEnabled = false, long modelId = 0, string controller = "", string actionBlock = "", string actionUnBlock = "", string actionDelete = "", string actionEdit = "", bool pin = false)
        {
            var html = new StringBuilder();
            html.Append("<text>");

            if (pin)
            {
                html.Append("<a href=\"#\" onclick=\"javascript:editarpin('" + url.Action(actionEdit, controller) + "' , '" + modelId + "');return false;\" class='btn btn-info btn-sm btneditar' alt='Editar' title='Editar'><i class='fa fa-pencil'></i></a> ");
            }
            else
            {
                html.Append("<a href=\"#\" onclick=\"javascript:editarquivo('" + url.Action(actionEdit, controller) + "' , '" + modelId + "');return false;\" class='btn btn-info btn-sm btneditar' alt='Editar' title='Editar'><i class='fa fa-pencil'></i></a> ");
            }

            if (!modelEnabled)
            {
                html.Append("<a href=\"#\" onclick=\"javascript:unlock('" + url.Action(actionUnBlock, controller, new { id = modelId }) + "');return false;\" class='btn btn-success btn-sm' alt='Ativar' title='Ativar'><i class='fa fa-check'></i></a> ");
            }
            else
            {
                html.Append("<a href=\"#\" onclick=\"javascript:block('" + url.Action(actionBlock, controller, new { id = modelId }) + "');return false;\" class='btn btn-warning btn-sm' alt='Desativar' title='Desativar'><i class='fa fa-ban'></i></a> ");
            }

            html.Append("<a href=\"#\" onclick=\"javascript:mensagem('" + url.Action(actionDelete, controller, new { id = modelId }) + "');return false;\" class='btn btn-danger btn-sm' alt='Excluir' title='Excluir'><i class='fa fa-trash-o'></i></a> ");

            html.Append("<text>");
            return new HtmlString(html.ToString());
        }

        public static IHtmlString GridFilterLink(this HtmlHelper helper, string controller, string fieldName, string fieldValue, string displayValue, bool condition = true)
        {
            if (condition)
            {
                return new HtmlString("<a href='" + GridFilterUrl(helper, controller, fieldName, fieldValue, 1) + "' target='_blank'>" + displayValue + "</a>");
            }
            return new HtmlString("<text>" + displayValue + "</text>");
        }

        public static string GridFilterUrl(this HtmlHelper helper, string controller, string fieldName, string fieldValue, int filterType)
        {
            return "/Admin/" + controller + "?grid-filter=" + fieldName + "__" + filterType + "__" + fieldValue;
        }


        public static IHtmlString DetailsLink(this HtmlHelper helper, string controller, long? id, string displayValue)
        {
            if (id != null)
            {
                return new HtmlString("<a href='/Admin/" + controller + "/Details/" + id + "' target='_blank'>" + displayValue + "</a>");
            }
            return new HtmlString("<text>" + displayValue + "</text>");
        }

        public static IHtmlString DownloadLink(this HtmlHelper helper, string caminho, string displayValue)
        {
            return new HtmlString("<a href='" + caminho + "' target='_blank' download='" + caminho + "'>" + displayValue + "</a>");
        }

        public static IHtmlString Image(this HtmlHelper helper, string caminho, string displayValue)
        {
            return new HtmlString("<a href='" + caminho + "' download='" + displayValue + "'><img src='" + caminho + "' style='max-width:250px'></a>");
        }

        public static IGridHtmlOptions<T> DefaultOptions<T>(this IGridHtmlOptions<T> grid) where T : class
        {
            return grid
                        .Named("mainGrid")
                            .SetLanguage("pt")
                            .Sortable(true)
                            .Filterable(true)
                            .WithMultipleFilters()
                            .Selectable(false)
                            .WithPaging(20)
                            .EmptyText("Nenhum resultado encontrado.")
                            ;
        }



        public static IGridColumn<T> FormatDate<T>(this IGridColumn<T> column, int width = 120, string format = "{0:dd/MM/yyyy HH:mm}") where T : class
        {
            return column.SetWidth(width).Format(format).Css("center");
        }

        public static IGridColumn<T> FormatNumber<T>(this IGridColumn<T> column, int width = 100) where T : class
        {
            return column.SetWidth(width).Css("center");
        }

        public static IGridColumn<T> NoHeader<T>(this IGridColumn<T> column) where T : class
        {
            return column.Css("header-hide").Sortable(false).Filterable(false);
        }

        public static IGridColumn<T> Label<T>(this IGridColumn<T> column, int width = 100) where T : class
        {
            return column.SetWidth(width).Css("center").Html();
        }

        public static IGridColumn<T> Html<T>(this IGridColumn<T> column) where T : class
        {
            return column.Encoded(false).Sanitized(false);
        }


    }




}