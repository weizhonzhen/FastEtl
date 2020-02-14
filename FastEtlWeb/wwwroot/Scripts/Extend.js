$.extend({
    Checkbox: {
        CheckAll: function (obj, checkboxName) {
            var checkBox = document.getElementsByName(checkboxName);
            for (var i = 0; i < checkBox.length; i++) { checkBox[i].checked = obj.checked; }
        },
        GetCheckedText: function (name) {
            var s = [];
            $("input[name=" + name + "]:checked").each(function () {
                s.push($(this).attr("text"));
            });
            return s.join(",");
        },
        GetCheckedValue: function (name) {
            var s = [];
            $("input[name=" + name + "]:checked").each(function () {
                s.push($(this).val());
            });
            return s.join(",");
        },
        IsCheked: function (name) {
            var isSelect = false;
            $("input[name=" + name + "]:checked").each(function () {
                isSelect = true;
            });
            return isSelect;
        }
    },
    RadioBox: {
        GetValue: function (name) {
            var result = $('input:radio[name="' + name + '"]:checked').val();
            if (result == undefined)
                return "";
            else
                return result;
        },
        SetValue: function (name, value) {
            $($("input:radio[name=" + name + "][value=" + value + "]")).prop("checked", true);
        },
        RemoveValue: function (name) {
            $("input:radio[name=" + name + "]").removeAttr('checked');
        }
    },
    TableOrderBy: function (id) {
        var name = $(id).data("orderby");
        if (name != undefined) {
            if (window.OrderByName != undefined) {
                var idDesc = $.inArray(name + "desc", window.OrderByName);
                var idAsc = $.inArray(name + "asc", window.OrderByName);
                if (idDesc >= 0) {
                    window.OrderByName.splice(idDesc, 1, name + "asc");
                    return name + " asc";
                }
                else if (idAsc >= 0) {
                    window.OrderByName.splice(idAsc, 1, name + "desc");
                    return name + " desc";
                }
                else {
                    window.OrderByName = new Array();
                    window.OrderByName.push(name + "desc");
                    return name + " desc";
                }
            }
            else {
                window.OrderByName = new Array();
                window.OrderByName.push(name + "desc");
                return name + " desc";
            }
        }
        else
            return "";
    },
    TableOrderByColor: function (id) {
        if (window.OrderByName != undefined) {
            $("#" + id + " thead tr td").each(function () {
                var idDesc = $.inArray($(this).data("orderby") + "desc", window.OrderByName);
                var idAsc = $.inArray($(this).data("orderby") + "asc", window.OrderByName);
                if (idDesc >= 0 || idAsc >= 0)
                    $(this).css("color", "red");
                else
                    $(this).css("color", "black");
            });
        }
    },
    QueryList: function (Url, FormId, ContentId, TableId) {
        layer.load(2);
        $.ajax({
            type: "POST",
            url: Url,
            data: $("#" + FormId).serialize(),
            success: function (result) {
                layer.closeAll('loading');
                if (result.success == false)
                    layer.msg(result.msg);
                else {
                    $("#" + ContentId).html(result);                    
                    if (TableId != "") {
                        $("#" + TableId + " tbody tr").children().first().click();
                        $.TableOrderByColor(TableId);
                    }
                }
            }
        });
    },
    QueryListParam: function (Url, Param, ContentId, TableId) {
        layer.load(2);
        $.ajax({
            type: "POST",
            url: Url,
            data: Param,
            success: function (result) {
                layer.closeAll('loading');
                if (result.success == false)
                    layer.msg(result.msg);
                else {
                    $("#" + ContentId).html(result);
                    if (TableId != "") {
                        $("#" + TableId + " tbody tr").children().first().click();
                        $.TableOrderByColor(TableId);
                    }
                }
            }
        });
    },
    TableClickColor: function (TableId) {
        $('#' + TableId + ' tbody tr').click(function () {
            $(this).css('background-color', '#6CC2CC');
            $('#' + TableId + ' tbody tr:odd').not(this).css('background-color', '#ffffff');
            $('#' + TableId + ' tbody tr:even').not(this).css('background-color', '#f3f4f5');
        });
    },
    CheckForm: function (name) {
        var msg = "";
        $(name + " select").each(function () {
            if ($(this).data("val")) {
                if ($(this).data("val-required") != undefined) {
                    if ($(this).val() == "") {
                        msg = $(this).data("val-required");
                        return false;
                    }
                }
            }
        });

        if (msg != "")
            return msg;

        var reg = new RegExp(",", "g");
        $(name + " input").each(function () {
            if ($(this).data("val")) {
                var type = $(this).attr("type");
                var name = $(this).attr("name");

                if ($(this).data("isdate") != undefined) {
                    if (!$.IsValidDate($(this).val())) {
                        msg = $(this).data("isdate");
                        return false;
                    }
                }

                if ($(this).data("val-required") != undefined) {
                    if ($(this).data("val-replace") != undefined) {
                        if ($(this).val().replace($(this).data("val-replace"), "") == "" && (type == "text" || type == "hidden")) {
                            msg = $(this).data("val-required");
                            return false;
                        }
                    }
                    else {
                        if ($(this).val() == "" && (type == "text" || type == "hidden")) {
                            msg = $(this).data("val-required");
                            return false;
                        }
                    }
                    if ($.RadioBox.GetValue(name) == "" && type == "radio") {
                        msg = $(this).data("val-required");
                        return false;
                    }
                    if ($.Checkbox.GetCheckedValue(name).replace(reg, "，") == "" && type == "checkbox") {
                        msg = $(this).data("val-required");
                        return false;
                    }
                }

                if ($(this).data("val-length") != undefined) {
                    if ($(this).val().length > $(this).data("val-length-max")) {
                        msg = $(this).data("val-length");
                        return false;
                    }
                }
            }
        });

        if (msg != "")
            return msg;

        $(name + " textarea").each(function () {
            if ($(this).data("val")) {
                if ($(this).data("val-required") != undefined) {
                    if ($(this).val() == "") {
                        msg = $(this).data("val-required");
                        return false;
                    }
                }

                if ($(this).data("val-length") != undefined) {
                    if ($(this).val().length > $(this).data("val-length-max")) {
                        msg = $(this).data("val-length");
                        return false;
                    }
                }
            }
        });

        return msg;
    },
    IsValidDate: function (date) {
        return true;
    }
});