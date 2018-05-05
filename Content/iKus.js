/// <reference path="jQuery/jquery-2.1.4.min.js" />

function T_Ajax(url, data, callback) {
    $.ajax({
        url: url,// 跳转到 action
        data: data,
        type: 'post',
        cache: false,
        dataType: 'json',
        success: function (data) {
            callback(data);
        },
        error: function () {
            //alert("异常！");
        }
    });
}
function T_Ajax(url, data, callback, $R) {
    $.ajax({
        url: url,// 跳转到 action
        data: data,
        type: 'get',
        cache: false,
        dataType: 'json',
        success: function (data) {
            callback(data, $R);
        },
        error: function () {
            //alert("异常！");
        }
    });
}

function LayUI_Laypage_Init(Laypage, Selector, PageCount, PageIndex, callback) {
    return Laypage.render({
        cont: $(Selector), //容器。值支持id名、原生dom对象，jquery对象,
        pages: PageCount, //总页数
        curr: PageIndex,
        skip: true, //是否开启跳页
        skin: 'molv',
        groups: 5, //连续显示分页数
        jump: function (e) { //触发分页后的回调
            callback(e);
            //e.pages = e.last = 20;
            //e.skin="molv"
            //$.getJSON('test/demo1.json', { curr: e.curr }, function (res) {
            //    e.pages = e.last = res.pages; //重新获取总页数，一般不用写
            //    //渲染
            //    var view = document.getElementById('view1'); //你也可以直接使用jquery
            //    var demoContent = (new Date().getTime() / Math.random() / 1000) | 0; //此处仅仅是为了演示
            //    view.innerHTML = res.content + demoContent;
            //});
        }
    });
}
function LayUI_Layer_OpeniFrame(Layer, Url, Title) {
    return Layer.open({
        type: 2,
        title: Title,
        area: ['90%', '90%'],
        shade: 0.3,
        maxmin: true,
        content: Url //这里content是一个URL，如果你不想让iframe出现滚动条，你还可以content: ['http://sentsin.com', 'no']
    });
}
