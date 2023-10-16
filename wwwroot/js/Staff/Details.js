
/**https://fullcalendar.io/docs/event-source-object#options
 * https://fullcalendar.io/docs/event-display
 * https://fullcalendar.io/docs/eventDataTransform
 * https://fullcalendar.io/docs/event-object
 * https://fullcalendar.io/docs/event-source-object
 * https://www.w3schools.com/colors/colors_picker.asp
 * https://fullcalendar.io/docs/v4/events-json-feed
 */

/**Variables that are loaded inclided in the HTML */
var restaurantLocation = document.getElementById('restaurantLocation');
var bookingEmail = document.getElementById('userEmail')
var bookingStatus = document.getElementById('bookingStatus')

/**Function load when Dom is loaded */
$(() => {

    LoadSearchVariables();

    LoadCalendar(restaurantLocation, bookingEmail, bookingStatus)

    document.getElementById('searchCustomer').addEventListener('click', () => {
            LoadCalendar(restaurantLocation, bookingEmail, bookingStatus)
    })

    document.getElementById('clearButton').addEventListener('click', () => {
        if (restaurantLocation.selectedIndex != 0 || bookingEmail.value != null || bookingStatus.selectedIndex != 0) {
            restaurantLocation.selectedIndex = 0
            bookingStatus.selectedIndex = 0
            bookingEmail.value = null;

            LoadCalendar(restaurantLocation, bookingEmail, bookingStatus)
        }
    })

    document.querySelector(".btn-primary");

});

/**
 * Loads the calendar
 * @param {any} restaurantLocation includes the information related to the Restaurant. It is the Id. On page load is 0
 * @param {any} bookingEmail email used when booking was made. If member, must be email of member. On page load is null
 * @param {any} bookingStatus status Id of the booking. On page load is 0
 */
function LoadCalendar(restaurantLocation, bookingEmail, bookingStatus)
{
    var calendarEl = document.getElementById('calendar');
    var calendar = new FullCalendar.Calendar(calendarEl, {
        initialView: 'dayGridMonth',
        showNonCurrentDates: false,
        events: {
            url: '/Staff/Bookings/GetReservations',
            extraParams: {
                location: restaurantLocation.options[restaurantLocation.selectedIndex].value,
                email: bookingEmail.value,
                status: bookingStatus.options[bookingStatus.selectedIndex].value
            }
        },  
        eventClick: async (info) => {
            $('#exampleModalCenter').modal('show');
            var data = await LoadSearchVariables(info.event.id)
            document.getElementById('RestaurantId').value = data.restaurantID;
            document.getElementById('bookingDetails').innerHTML = "Booking: "+ data.bookingId;
            document.getElementById('Date').value = data.startTime; 
            document.getElementById('duration').value = data.duration
            document.getElementById('guests').value = data.guest
            document.getElementById('comments').value = data.comments
            
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

}

async function LoadSearchVariables(bookingID) {
    if (bookingID == undefined) {
    } else {
        try {
            const baseUrl = baseURL()
            const url = new URL("/Staff/Bookings/GetReservationById?bookingID=" + bookingID, baseUrl);
            const response = await fetch(url);

            if (!response.ok) {
                throw alert(new Error("HTTP error " + response.status));
            }
            const data = await response.json();
            console.log(data);
            return await data;

        } catch (error) {
            alert(error)
        }
    }
}






