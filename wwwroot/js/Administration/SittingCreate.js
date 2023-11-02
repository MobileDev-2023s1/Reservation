const sittingname = document.getElementById("SittingName");
const capacity = document.getElementById("capacity");
const startTime = document.getElementById("StartTime");
var start = new Date();



$(() => {
   
    var checkbox = document.getElementById('drop-remove');
    var containerEl = document.getElementById('external-events');
    const startTime = document.getElementById("StartTime");

    //var draggableEl = document.getElementById('mydraggable');
    var Draggable = FullCalendar.Draggable;
   // var Calendar = FullCalendar.Calendar;
  
   

    new Draggable(containerEl, {
        itemSelector: '.fc-event',
        eventData: function (eventEl) {

             return {
                    title:sittingname.value,    
                    capacity:capacity.value,
                    start: start.value,
                    end: '2023-10-24T14:00:00'
             };
        }
    });


    var calendarEl = document.getElementById('calendar');

    var calendar = new FullCalendar.Calendar(calendarEl, {
        initialView: 'dayGridMonth',
        
        showNonCurrentDates: false,
       events: { url: '/Administration/Sitting/index' },



        eventClick: (info) => { alert(info.event.id + " " + info.event.title + info.event.start +"!!!"+ info.event.end) },

        headerToolbar: {
                        left: 'prev,next today',
                        center: 'title',
                        right: 'dayGridMonth,timeGridWeek,timeGridDay'
                        },        
        editable: true,
        selectable: true,
        droppable: true, 
        
        drop: create,
        
       
        eventDrop: function (event, delta, revertFunc, jsEvent, ui, view) {
            alert("eventDrop: " + event.start);
        },
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



const startTimes = ["06:00", "12:00", "17:00"];

const typeid = document.getElementById("typeid");



var end = new Date();



const durationH = document.getElementById("DurationH");
const durationM = document.getElementById("DurationM");
var duration = new Date();
    
const newsitting = document.getElementById("newSitting");

const sittingtypes = ["BreakFast", "Lunch", "Dinner"];



document.getElementById("SittingName").addEventListener("blur", update);
document.getElementById("capacity").addEventListener("blur", update);
document.getElementById("typeid").addEventListener("mouseleave", update);

document.getElementById("DurationH").addEventListener("mouseleave", update);
document.getElementById("DurationM").addEventListener("mouseleave", update);


document.getElementById("StartTime").addEventListener("blur", update);
//document.getElementById("Duration").addEventListener("blur", update);
//function getDuration() {
//    duration.setHours(durationH, durationM);
//}


function update() {
    let sittingtype = sittingtypes[typeid.value - 1];
    startTime.value = startTimes[typeid.value - 1];
    //end = start.setHours(startTime + duration);
    //newsitting.start=start,
    newsitting.name = sittingname.value,
      
    newsitting.capacity = capacity.value,
    newsitting.sittingtype=sittingtype,

    //newsitting.start=start.value,
    //newsitting.end=end.value,
    newsitting.innerHTML = newsitting.name+"-"+ newsitting.capacity+ "ppl-"+ newsitting.sittingtype;
}




document.getElementById("test").addEventListener("click", test);
function test(info) {
   


    alert(newsitting.title + newsitting.capacity + newsitting.sittingtype + info.event.start)
}
function create(date, jsEvent, ui, resourceId) {
    
        //start.setFullYear(date),
        //start.setHours(startTime),
    alert("time stamped" + date.setTime(startTime).date)
}


function GetEvent(calendar, id) {
    calendar.GetEvent(id)
}






