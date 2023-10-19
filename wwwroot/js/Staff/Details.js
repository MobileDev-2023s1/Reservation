
/**https://fullcalendar.io/docs/event-source-object#options
 * https://fullcalendar.io/docs/event-display
 * https://fullcalendar.io/docs/eventDataTransform
 * https://fullcalendar.io/docs/event-object
 * https://fullcalendar.io/docs/event-source-object
 * https://www.w3schools.com/colors/colors_picker.asp
 * https://fullcalendar.io/docs/v4/events-json-feed
 */

/**Variables that are loaded in the HTML */
var restaurantLocation = document.getElementById('restaurantLocation');
var bookingEmail = document.getElementById('userEmail')
var bookingStatus = document.getElementById('bookingStatus')

/**Variables that are related to the pop up window for updating details of the booking*/
var restaurantId = document.getElementById('RestaurantId')
var bookingTitle = document.getElementById('bookingTitle')
var date = document.getElementById('Date')
var bookingLength = document.getElementById('duration')
var bookingGuests = document.getElementById('guests')
var bookingCommnets = document.getElementById('comments')
var newReservationStatusId = document.getElementById('NewReservationStatusId')
var currentSittingSelection = document.getElementById('currentSittingSelection')
var sittingId = document.getElementById('MenuType')
var currentPersonId = document.getElementById("PersonId")


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

    document.getElementById('submit').addEventListener('click', (data) => {
        UpdateDetails(data)
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
            PopulateList(await GetAvailableAreas(data.restaurantID), data);

            restaurantId.value = data.restaurantID;
            bookingTitle.innerHTML = "Booking: "+ data.bookingId;
            date.value = data.startTime; 
            bookingLength.value = data.duration
            bookingGuests.value = data.guest
            bookingCommnets.value = data.comments
            newReservationStatusId.selectedIndex = data.reservationStatusId - 1
            currentSittingSelection.innerHTML = data.sittingName
            currentSittingSelection.value = data.sittingId
            currentPersonId.value = data.personId 
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

/**
 * Cleans values in the list and creates new options where required. 
 * @param {any} areas
 */
async function PopulateList(areas, data) {
    while (RestaurantAreaList.childElementCount > 0) {
        RestaurantAreaList.removeChild(RestaurantAreaList.firstChild);
    }

    areas.forEach(areaItem => {
        const option = document.createElement("option");
        option.value = areaItem.id;
        option.textContent = areaItem.name;
        if (areaItem.id == data.restaurantAreaId) {
            option.selected = true
        } else {
            option.selected = false
        }
        restaurantAreas.appendChild(option);
    })
}

/**
 * Gets Areas by restaurant Id
 * @param {any} restaurantId
 * @returns
 */
async function GetAvailableAreas(restaurantId) {
    try {
        const listAreas = new URL("/Customers/Bookings/GetListAreas?restuarantId=" + restaurantId, baseURL());
        areasResponse = await fetch(listAreas)

        if (!areasResponse.ok) {
            throw new Error("HTTP error " + areasResponse.status);
        }
        const areasFinal = await areasResponse.json();
        return await areasFinal;

    } catch (error) {
        console.error(error);
    }

}

/**
 * 
 * @param {any} bookingID
 * @returns
 */
async function LoadSearchVariables(bookingID) {
    if (bookingID === undefined) {
        return null;
    } else {
        try {
            const baseUrl = baseURL()
            const url = new URL("/Staff/Bookings/GetReservationById?bookingID=" + bookingID, baseUrl);
            const response = await fetch(url);

            if (!response.ok) {
                throw alert(new Error("HTTP error " + response.status));
            }
            const data = await response.json();
            return await data;

        } catch (error) {
            alert(error)
        }
    }
}

async function UpdateDetails(data) {

    var c = {
        Start: date.value,
        Duration: bookingLength.value,
        Guests: bookingGuests.value,
        SittingId: sittingId.value,
        PersonId: currentPersonId.value,
        RestaurantAreaId: RestaurantAreaList.value,
        Comments: bookingCommnets.value

    }

    try {
        const baseUrl = baseURL()
        const url = new URL("/Customer/Bookings/Index"


            , baseUrl);
        const response = await fetch(url, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(c), // Send the 'c' object as a JSON payload
        });

        if (response.ok) {
            const data = await response.json();
            console.log(data);
            return data;
        } else {
            console.error("Failed to update booking:", response.status, response.statusText);
        }     

        return await data;
        
    } catch (error) {

    }/*console.log(c)*/
}