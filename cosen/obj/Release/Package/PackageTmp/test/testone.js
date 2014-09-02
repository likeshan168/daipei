function MyViewModel() {
    var self = this;
    self.tjdates = ko.observableArray();


    self.getdp = function getdp() {
        $.getJSON('/api/DpApi/GetDaiPei?sltDp=D011&pageNum=1&tjDate=2014-07-22', function (data, textStatus, xhr) {
            var html = [];
            var rows = data.rows, tjrows = data.tjrows;


            if (tjrows.length === 0) {
                self.tjdates([]);
            }
            else {
                self.tjdates(tjrows);
                console.log(tjrows);

            }


        });

    }
}

ko.applyBindings(new MyViewModel());