const baseURL = "https://localhost:7113";


document.addEventListener('DOMContentLoaded', function () {

    var currentDate = new Date(Date.now());

    var calendarEl = document.getElementById('calendar');
    var calendar = new FullCalendar.Calendar(calendarEl, {
        initialView: 'dayGridMonth',
        initialDate: GetDate(currentDate),
        headerToolbar: {
            left: 'prev,next today',
            center: 'title',
            right: 'dayGridMonth,timeGridWeek,timeGridDay'
        }        
    });
    addEvent(calendar); //https://fullcalendar.io/docs/Calendar-addEvent
    calendar.render();
});

//document.addEventListener('DOMContentLoaded', function () {
//    const previousMonth = document.querySelector(".fc-prev-button");

//    previousMonth.addEventListener("click", function () {

        

//        var calendarEl = document.getElementById('calendar');
//        var calendar = new FullCalendar.Calendar(calendarEl, {
//            initialView: 'dayGridMonth',
//            initialDate: GetDate(currentDate),
//            headerToolbar: {
//                left: 'prev,next today',
//                center: 'title',
//                right: 'dayGridMonth,timeGridWeek,timeGridDay'
//            }
//        });
//        addEvent(calendar);
//        calendar.render();
//    })
//});




async function GetReservation() {
    try {
        const url = new URL("/Staff/Bookings/GetReservations", baseURL)
        const response = await fetch(url);

        if (!response.ok) {
            throw new Error("HTTP error " + response.status);
        }
        const data = await response.json();
        console.log(data)
        return await data;

    } catch (error) {
        console.error(error);
    }
}

function addEvent(calendar) {
    GetReservation().then((data) => {
        data.forEach((item) =>
        (
            calendar.addEvent({
                title: item.name,
                start: item.start,
                end: item.end
            })
        ))

    })
}


//Other functions
/**
 * Gets the initial date to display for the calendar
 * @returns yyyy-mm-dd date format as text
 */
function GetDate(currentDate) {
    return currentDate.getFullYear().toString() + "-" + GetMonth(currentDate) + "-" + GetDay(currentDate)
}

/**
 * Returns the current month as a string.
 * @param {any} currentDate is today's date
 * @returns Month in today's date
 */
function GetMonth(currentDate) {
    var currentMonth = currentDate.getMonth() + 1

    if (currentMonth < 10) {
        return '0'.concat(currentMonth.toString());
    } else {
        return currentMonth.toString();
    }
}

/**
 * 
 * @param {any} currentDate
 * @returns Returns current date in 2 digits format
 */
function GetDay(currentDate) {
    var currentDay = currentDate.getDate().toString();

    if (currentDay < 10) {
        return '0'.concat(currentDay);
    } else {
        return currentDay;
    }
    return 
}

