
$(() => {
   
    var checkbox = document.getElementById('drop-remove');
    var containerEl = document.getElementById('external-events');
    var draggableEl = document.getElementById('mydraggable');
    var Draggable = FullCalendar.Draggable;
    var Calendar = FullCalendar.Calendar;


    new Draggable(containerEl, {
        itemSelector: '.fc-event',
        eventData: function (eventEl) {
            return {
                title: eventEl.innerText
            };
        }
    });

    var calendarEl = document.getElementById('calendar');
    var calendar = new FullCalendar.Calendar(calendarEl, {
        initialView: 'dayGridMonth',
        showNonCurrentDates: false,
        events: {
            url: '/Administration/Sitting/Index'
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
        selectable: true,
        droppable: true, 
        // drop: function (info) {
        //    // is the "remove after drop" checkbox checked?
        //    if (checkbox.checked) {
        //        // if so, remove the element from the "Draggable Events" list
        //        info.draggedEl.parentNode.removeChild(info.draggedEl);
        //    }
        //}
    });
    calendar.render();


});



function GetEvent(calendar, id) {
    calendar.GetEvent(id)
}






