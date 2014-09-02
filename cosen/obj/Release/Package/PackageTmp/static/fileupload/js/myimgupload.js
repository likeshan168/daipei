
(function (i, s, o, g, r, a, m) {
    i['GoogleAnalyticsObject'] = r; i[r] = i[r] || function () {
        (i[r].q = i[r].q || []).push(arguments)
    }, i[r].l = 1 * new Date(); a = s.createElement(o),
    m = s.getElementsByTagName(o)[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m)
})(window, document, 'script', '//www.google-analytics.com/analytics.js', 'ga');
ga('create', 'UA-41071247-1', 'blueimp.github.io');
ga('send', 'pageview');



//清空上传图片的信息不是删除
function clearlog() {
    var tr = $("#tbfiles").children('tr.template-download');
    console.log(tr);
    // $("#tbfiles").children('tr.template-download').remove();
    tr.remove();

}
//编辑图片名称
function editimg(obj) {

    var btn = $(obj),
        tdname = btn.parent().prev().prev(),
        oldname = btn.attr('title');
    tdname.html('<input type="text" onkeydown="savekdown();" class="form-control" value="' + oldname.split('.')[0] + '" />');
    btn.parent().prepend('<button title="' + oldname + '" onclick="saveimg(this);"  type="button" class="btn btn-primary" ><i class="glyphicon glyphicon-save"></i><span>保存</span></button> <button title="' + oldname + '" onclick="canceledit(this);"  type="button" class="btn btn-warning" ><i class="glyphicon glyphicon glyphicon-ban-circle"></i><span>取消</span></button>');
    btn.remove()

}
//取消编辑图片名称的功能
function canceledit(obj) {
    var btn_cancel = $(obj), oldname = btn_cancel.attr('title');
    btn_cancel.prev().remove();
    btn_cancel.parent().prepend('<button onclick="editimg(this);" title="' + oldname + '" type="button" class="btn btn-primary" ><i class="glyphicon glyphicon-edit"></i><span>编辑</span></button>');

    btn_cancel.parent().prev().prev().html(oldname);
    btn_cancel.remove();

}
//保存编辑之后的图片名称
function saveimg(obj) {
    var btn_save = $(obj),
        oldname = btn_save.attr('title'),
        input = btn_save.parent().prev().prev().children('input')[0],
        newname = $(input).val().split('.')[0];
    $.post('/api/DpApi/SaveImgName?oldName=' + oldname + '&newName=' + newname + '&imgType=' + $("input[name='imgtype']:checked").val() + "&dpid=" + $("#sltdp").val(), function (data) {
        console.log(data);
        if (data === 'ok') {
            reverge(btn_save, newname);

        } else {
            reverge(btn_save, oldname);
        }

    });

}
//保存成功之后恢复至可编辑状态
function reverge(btn_save, value) {
    btn_save.parent().prepend('<button onclick="editimg(this);" title="' + value + ".jpg" + '" type="button" class="btn btn-primary" ><i class="glyphicon glyphicon-edit"></i><span>编辑</span></button>');
    btn_save.next().remove();
    btn_save.parent().prev().prev().html(value + '.jpg');
    btn_save.remove();
}

var cosen = {};
cosen["imgpage"] = 2;
cosen["tbfiles"] = $("#tbfiles");
//键盘enter事件
function onkdown(e) {

    var key = window.event ? window.event.keyCode : e.which;
    if (key === 13) {
        btn_search();
    }


}
//enter键保存
function savekdown(e) {

    var key = window.event ? window.event.keyCode : e.which;
    var target = window.event ? window.event.target : e.target;
    if (key === 13) {
        $($(target).parent().next().next().children()[0]).click();
    }
}
//单击查询按钮
function btn_search() {
    cosen["tbfiles"].html("");
    getimgnameandytype(1);
    cosen["imgpage"] = 2;
    $(window).bind('scroll', function (event) {
        var totalheight = parseFloat($(window).height()) + parseFloat($(window).scrollTop());
        if ($(document).height() <= totalheight) {
            getimgnameandytype(cosen["imgpage"]);
            cosen["imgpage"] = cosen["imgpage"] + 1;

        }
    });
}

//根据类型和名称查找图片问价
function getimgnameandytype(imgpage) {
    var imgtype = $("input[name='imgtype']:checked").val(),
        imgname = $('#imgname').val(), dpid = $("#sltdp").val();
    searchimg(imgname, imgtype, imgpage, dpid);

}
//查询图片
function searchimg(imgname, imgtype, imgpage, dpid) {
    $.post('/api/DpApi/SearchImg/?imgName=' + imgname + "&imgType=" + imgtype + "&imgPage=" + imgpage + "&dpid=" + dpid, function (data, textStatus, xhr) {
        // console.log(data);
        if (data !== "None") {

            var html = tmpl("template-download", data);
            if ($.trim(html).length === 0) {
                if (cosen["imgpage"] === 2) {
                    cosen["tbfiles"].html("<tr class='template-download'><td><div class='alert alert-danger'>没有查到图片信息</div></td></tr>");
                } else {
                    $(window).unbind('scroll');
                }

            } else {

                cosen["tbfiles"].append(html);

            }

        } else {
            console.log("nodata")
        }
    });
}

//选中上传场地图
$(document).ready(function () {
    $("input[name='imgtype']").click(function () {
        console.log($(this).val());
        if ($(this).val() === "area") {
            $("#sltdp").fadeIn(1000);
        }
        else {
            $("#sltdp").fadeOut(200);
        }
    });
});