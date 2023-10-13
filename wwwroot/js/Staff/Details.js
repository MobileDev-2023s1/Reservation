
$(() => {
    
    var calendarEl = document.getElementById('calendar');
    var calendar = new FullCalendar.Calendar(calendarEl, {
        initialView: 'dayGridMonth',
        showNonCurrentDates: false,
        events: {
            url: '/Staff/Bookings/GetReservations'
        },  
        eventClick: (info) => {
            alert(info.event.id + " " + info.event.title)
        },
        headerToolbar: {
            left: 'prev,next today',
            center: 'title',
            right: 'dayGridMonth,timeGridWeek,timeGridDay'
        },
        editable: true,
        selectable: true
    });
    calendar.render();
});

function GetEvent(calendar, id) {
    calendar.GetEvent(id)
}






