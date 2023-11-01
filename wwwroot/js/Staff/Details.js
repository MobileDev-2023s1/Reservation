
/**https://fullcalendar.io/docs/event-source-object#options
 * https://fullcalendar.io/docs/event-display
 * https://fullcalendar.io/docs/eventDataTransform
 * https://fullcalendar.io/docs/event-object
 * https://fullcalendar.io/docs/event-source-object
 * https://www.w3schools.com/colors/colors_picker.asp
 * https://fullcalendar.io/docs/v4/events-json-feed
 */
//look into Jquery to not have getElementbyId

/**Function load when Dom is loaded */
$(() => {
    LoadSearchVariables();
    LoadCalendar()
});

/**Event Listeners*/

$('#searchCustomer').click(LoadCalendar)

$('#clearButton').click(e => {
    if ($('#restaurantLocation').prop('selectedIndex') != 0
        || $('#userEmail').val() != null || $('#bookingStatus').prop('selectedIndex') != 0) {
        ClearSearchParameters();
        LoadCalendar()
    }
})
function ClearSearchParameters() {
    $('#restaurantLocation').prop('selectedIndex', 0)
    $('#bookingStatus').prop('selectedIndex', 0)
    $('#userEmail').val(null)
}


$('#submit').click(async e => {
    await UpdateDetails()
    LoadCalendar()
    $('#exampleModalCenter').modal('hide');
})

$('#closeButton').click(e => {
    $('#exampleModalCenter').modal('hide');
})


$('#assignTable').click(async e => {
    $('tableSection')
})

$('#assignTable').click(async e => {
    //booking needs to be confirmed before assigning a table
    if ($('#assignTableSection').css('display') == 'block') {
        $('#assignTableSection').css('display', 'none')
        ClearSeletedTablesSection()
    } else {
        $('#assignTableSection').css('display', 'block')
        ClearSeletedTablesSection()
        await GetTablesByAreaId()
    }
})

$('#NewReservationStatusId').on('change', async e => {
    if ($('#NewReservationStatusId').prop('selectedIndex') != 1 && $('#NewReservationStatusId').prop('selectedIndex') != 3) {
        $('#assingTableOption').css('display', 'none')
        $('#assignTable')[0].checked = false;
        $('#assignTableSection').css('display', 'none')
    } else {
        $('#assingTableOption').css('display', 'block')
    }
})

/**
 * Loads the calendar
 * @param {any} restaurantLocation includes the information related to the Restaurant. It is the Id. On page load is 0
 * @param {any} bookingEmail email used when booking was made. If member, must be email of member. On page load is null
 * @param {any} bookingStatus status Id of the booking. On page load is 0
 */
function LoadCalendar()
{
    var calendar = new FullCalendar.Calendar($('#calendar')[0], {
        initialView: 'dayGridMonth',
        showNonCurrentDates: false,
        events: {
            url: '/Staff/Bookings/GetReservations',
            display: {
            },
            extraParams: {
                location: $('#restaurantLocation').val(),
                email: $('#userEmail').val(), 
                status: $('#bookingStatus').val()
            }
        },  
        eventClick: async (info) => {
            $('#exampleModalCenter').modal('show');
            var data = await LoadSearchVariables(info.event.id)
            PopulateList(await GetAvailableAreas(data.restaurantID), data);

            $('#RestaurantId').val(data.restaurantID);
            $('#bookingTitle').html('booking ' + data.bookingId)
            $('#Date').val(data.startTime)
            $('#duration').val(data.duration)
            $('#guests').val(data.guest)
            $('#comments').val(data.comments)
            $('#NewReservationStatusId').prop('selectedIndex', data.reservationStatusId - 1)
            $('#currentSittingSelection').html(data.sittingName)
            $('#currentSittingSelection').val(data.sittingId)
            $('#PersonId').val(data.personId)
            $('#BookingId').val(data.bookingId)
            $('#CurrentRestaurantArea').val(data.restaurantAreaId)
            $('#ListOfTables').val(data.assignedTable)

            if ($('#NewReservationStatusId').prop('selectedIndex') != 1 && $('#NewReservationStatusId').prop('selectedIndex') != 3) {
                $('#assingTableOption').css('display', 'none')
            } else {
                $('#assingTableOption').css('display', 'block');
                ClearSeletedTablesSection();
                GetTablesByAreaId();

            }
            
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

async function UpdateDetails() {

    console.log($('.SelectedTables'))


    var c = {
        StartTime: new Date($('#Date').val()),
        Duration: $('#duration').val(),
        Guests: $('#guests').val(),
        SittingId: $('#MenuType').val(),
        PersonId: $('#PersonId').val(),
        RestaurantAreaId: $('#RestaurantAreaList').val(),
        Comments: $('#comments').val(),
        ReservationId: $('#BookingId').val(),
        ReservationStatusId: $('#NewReservationStatusId').val(),
        RestaurantTables: FinalTablesList()
    }

    try {
        const baseUrl = baseURL()
        const url = new URL("/Staff/Bookings/NewBookingDetails?c="+ c, baseUrl);
        const response = await fetch(url, {
            method: 'POST',
            headers: {
                "Accept": "application/json",
                "Content-Type": "application/json"
            },
            body: JSON.stringify(c)
        });

        if (!response.ok) {
            console.log(response.error)
            
        } else {
            const result = await response.text();
            console.log(result)
        }
    } catch (error) {

    }
}
        