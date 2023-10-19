
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
            url: '/Administration/Sitting/Create'
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


document.getElementById("sittingName").addEventListener("blur", update);
document.getElementById("capacity").addEventListener("blur", update);
document.getElementById("typeid").addEventListener("onclick", update);

function update() {
    let sittingname = document.getElementById("sittingName").value;
    let capacity = document.getElementById("capacity").value;
    let typeid = document.getElementById("typeid").value;
    let sittingtype = ["BreakFast", "Lunch", "Dinner"][typeid - 1];
    

    let newSitting = document.getElementById("newSitting");
    newSitting.innerHTML=sittingname +" CAP-"+ capacity + "  "+ sittingtype ;
}


function GetEvent(calendar, id) {
    calendar.GetEvent(id)
}






