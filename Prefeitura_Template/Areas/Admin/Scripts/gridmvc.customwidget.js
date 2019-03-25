function EnumFilter(columnName, enumName, displayName) {
    this.columnName = columnName;
    this.enumName = enumName;
    this.displayName = displayName != null ? displayName : columnName;
    this.getAssociatedTypes = function () {
        return ["EnumFilter-"+this.columnName];
    };
    this.onShow = function () {
        
    };
    this.showClearFilterButton = function () {
        return true;
    };
    this.onRender = function (container, lang, typeName, values, cb, data) {
        //store parameters:
        this.cb = cb;
        this.container = container;
        this.lang = lang;

        //this filterwidget demo supports only 1 filter value for column column
        this.value = values.length > 0 ? values[0] : { filterType: 1, filterValue: "" };

        this.renderWidget(); 
        this.loadItems(); 
        this.registerEvents(); 
    };
    this.renderWidget = function () {
        var html = '<p><b>' + this.displayName + ':</b></p>\
                    <select style="width:250px;" class="grid-filter-type itemlist form-control">\
                    </select>';
        this.container.append(html);
    };

    this.loadItems = function () {
        var $this = this;
        $this.fillItems();
    };
    
    this.fillItems = function () {
        var itemList = this.container.find(".itemlist");
            itemList.append('<option value="">Selecione...</option>');
            itemList.append('<option ' + ("Ativo" == this.value.filterValue ? 'selected="selected"' : '') + ' value="Ativo">Ativo</option>');
            itemList.append('<option ' + ("Inativo" == this.value.filterValue ? 'selected="selected"' : '') + ' value="Inativo">Inativo</option>');
    };
    
    this.registerEvents = function () {
        var itemList = this.container.find(".itemlist");
        var $context = this;
        itemList.change(function () {
            var values = [{ filterValue: $(this).val(), filterType: 1  }];
            $context.cb(values);
        });
    };

}

function FaleConoscoFilter(columnName, enumName, displayName) {
    this.columnName = columnName;
    this.enumName = enumName;
    this.displayName = displayName != null ? displayName : columnName;
    this.getAssociatedTypes = function () {
        return ["EnumFilter-" + this.columnName];
    };
    this.onShow = function () {

    };
    this.showClearFilterButton = function () {
        return true;
    };
    this.onRender = function (container, lang, typeName, values, cb, data) {
        //store parameters:
        this.cb = cb;
        this.container = container;
        this.lang = lang;

        //this filterwidget demo supports only 1 filter value for column column
        this.value = values.length > 0 ? values[0] : { filterType: 1, filterValue: "" };

        this.renderWidget();
        this.loadItems();
        this.registerEvents();
    };
    this.renderWidget = function () {
        var html = '<p><b>' + this.displayName + ':</b></p>\
                    <select style="width:250px;" class="grid-filter-type itemlist form-control">\
                    </select>';
        this.container.append(html);
    };

    this.loadItems = function () {
        var $this = this;
        $this.fillItems();
    };

    this.fillItems = function () {
        var itemList = this.container.find(".itemlist");
        itemList.append('<option value="">Selecione...</option>');
        itemList.append('<option ' + ("Novo" == this.value.filterValue ? 'selected="selected"' : '') + ' value="Novo">Novo</option>');
        itemList.append('<option ' + ("Em Análise" == this.value.filterValue ? 'selected="selected"' : '') + ' value="Em Análise">Em Análise</option>');
        itemList.append('<option ' + ("Respondido" == this.value.filterValue ? 'selected="selected"' : '') + ' value="Respondido">Respondido</option>');
    };

    this.registerEvents = function () {
        var itemList = this.container.find(".itemlist");
        var $context = this;
        itemList.change(function () {
            var values = [{ filterValue: $(this).val(), filterType: 1 }];
            $context.cb(values);
        });
    };

}