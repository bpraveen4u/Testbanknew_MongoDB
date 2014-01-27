

$(document).ready(function () {

    //$('.dummyTimerUpdate').datepick({ minDate: new Date(2010, 12 - 1, 25) });

    var austDay = new Date();
    austDay = new Date(austDay.getFullYear(), 1, 26);
    
    var d = new Date();
    var v = new Date();
    v.setMinutes(d.getMinutes() + 30);

    $('.dummyTimerUpdate').countdown({ 
        until: v,
        onExpiry: liftOff,
        format: 'MS',
        layout: 'Time left: {mn} {ml}, {sn} {sl}',
        //labels: ['Yr', 'Mo', 'Wk', 'Dy', 'Hours', 'Min', 'Sec'],
        description : 'Time left'
    });
    //$('#year').text(austDay.getFullYear());
});

function liftOff() {
    alert("done");
}