$(document).ready(function () {
    $(".sidebar .treeview").tree();

    $('.datepicker').datetimepicker({
        format: "DD/MM/YYYY",
        locale: 'pt-br',
        minDate: new Date(2000, 01, 01),
        maxDate: new Date(3000, 01, 01)
    });

    $('.datetimepicker').datetimepicker({
        format: "DD/MM/YYYY HH:mm",
        locale: 'pt-br',
        minDate: new Date(2000, 01, 01),
        maxDate: new Date(3000, 01, 01)
    });
});

$(".sidebar .treeview").tree();

(function ($) {
    "use strict";

    $.fn.tree = function () {

        return this.each(function () {
            var btn = $(this).children("a").first();
            var menu = $(this).children(".treeview-menu").first();
            var isActive = $(this).hasClass('active');

            //initialize already active menus
            if (isActive) {
                menu.show();
                btn.children(".fa-angle-left").first().removeClass("fa-angle-left").addClass("fa-angle-down");
            }
            //Slide open or close the menu on link click
            btn.click(function (e) {
                e.preventDefault();
                if (isActive) {
                    //Slide up to close menu
                    menu.slideUp();
                    isActive = false;
                    btn.children(".fa-angle-down").first().removeClass("fa-angle-down").addClass("fa-angle-left");
                    btn.parent("li").removeClass("active");
                } else {
                    //Slide down to open menu
                    menu.slideDown();
                    isActive = true;
                    btn.children(".fa-angle-left").first().removeClass("fa-angle-left").addClass("fa-angle-down");
                    btn.parent("li").addClass("active");
                }
            });

            /* Add margins to submenu elements to give it a tree look */
            menu.find("li > a").each(function () {
                var pad = parseInt($(this).css("margin-left")) + 10;

                $(this).css({ "margin-left": pad + "px" });
            });

        });

    };


}(jQuery));

function mensagemexcluirarquivo(url, id) {

    swal({
        title: "Excluir conteúdo?",
        text: "Este conteúdo não poderá ser recuperado!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Excluir",
        cancelButtonText: "Cancelar",
        closeOnConfirm: true,

    }, function (isConfirm) {
        if (isConfirm) {
            $.ajax({
                url: url,
                type: "POST",
                success: function (msg) {
                    if (msg.Sucesso == true) {
                        $("#" + id).val('');
                        $("#Botoes" + id).fadeOut();
                        swal("Sucesso", "O arquivo excluído com exito", "success");
                    }
                    else {
                        swal("Erro", "O arquivo não foi excluído devido à um erro inesperado", "error");
                    }
                },
                error: function (jqXHR, textStatus) {
                    swal("Erro", "O arquivo não foi excluído devido à um erro inesperado", "error");
                }
            });
        }
    });
}

function mensagem(url) {

    swal({
        title: "Excluir conteúdo?",
        text: "Este conteúdo não poderá ser recuperado!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Excluir",
        cancelButtonText: "Cancelar",
        closeOnConfirm: true,

    }, function (isConfirm) {
        if (isConfirm) {
            window.location = url;
        }
    });
}

function unlock(url) {
    swal({
        title: "Ativar conteúdo?",
        text: "Deseja ativar este conteúdo?",
        type: "success",
        showCancelButton: true,
        confirmButtonColor: "#33cc33",
        confirmButtonText: "Ativar",
        cancelButtonText: "Cancelar",
        closeOnConfirm: true,

    }, function (isConfirm) {
        if (isConfirm) {
            window.location = url;
        }
    });
}

function block(url) {
    swal({
        title: "Bloquear conteúdo?",
        text: "Deseja bloquear este conteúdo?",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Bloquear",
        cancelButtonText: "Cancelar",
        closeOnConfirm: true,

    }, function (isConfirm) {
        if (isConfirm) {
            window.location = url;
        }
    });
}

function ApplyCkEditor(field, height) {
    if (!height) {
        height = "200px";
    }
    $(document).ready(function () {
        var editor = CKEDITOR.replace(field,
            {
                allowedContent: true,
                height: height
            });
    });
}