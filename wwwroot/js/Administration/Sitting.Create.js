
$(() => {
    var calendar = new FullCalendar.Calendar($('#calendar')[0], {
        initialView: 'dayGridMonth',
        showNonCurrentDates: false,
        events: { url: '/api/sittings/events' },
        eventClick: (info) => { alert(info.event.id + " " + info.event.title + info.event.start + "!!!" + info.event.end) },

        headerToolbar: {
            left: 'prev,next today',
            center: 'title',
            right: 'dayGridMonth,timeGridWeek,timeGridDay'
        },
        editable: true,
        selectable: true,
        droppable: true,

        eventDrop: function (event, delta, revertFunc, jsEvent, ui, view) {
            alert("eventDrop: " + event.start);
        },


    });
    calendar.render();




}) 

