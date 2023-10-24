
const listofTables = document.getElementById('tablesInArea');
const selectedTables = document.getElementById('SelectedTables')

$(() => {
    listofTables.class = "btn-group";
    listofTables.role = "group"
    listofTables.ariaLabel = "Basic Example"
})

async function GetTablesByAreaId(areaId)
{
    if (areaId === undefined) {
        return null;
    } else {
        try {

            var c = {
                StartTime: new Date(date.value),
                Duration: bookingLength.value,
                Guests: bookingGuests.value,
                SittingId: sittingId.value,
                PersonId: currentPersonId.value,
                RestaurantAreaId: RestaurantAreaList.value,
                Comments: bookingCommnets.value,
                ReservationId: currentBookingId.value,
                ReservationStatusId: newReservationStatusId.value,
                /*AssignedTables: listOfTakenTables*/

            }

            const url = new URL("/Staff/Tables/TablesAvailableInSitting?c=" + c, baseURL())
            const response = await fetch(url, {
                method: 'POST',
                headers: {
                    "Accept": "application/json",
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(c)
            });

            if (!response.ok) {
                console.log(response.status)
            }

            const tables = await response.json();
            
            AddTablesToView(tables);

            console.log(tables)

        } catch (error) {
            console.log(error)
        }

    }


}

function AddTablesToView(tables) {

    while (listofTables.childElementCount > 0) {
        listofTables.removeChild(listofTables.firstChild)
    }

    tables.forEach((item) => {
        const option = document.createElement('button')
        option.innerHTML = item.name
        option.id = `button${item.name}`
        option.className = "btn btn-light"
        
        if (!item.status) {
            option.disabled = true
        } 
        listofTables.appendChild(option)
        option.addEventListener('click', ()=> {
            BlockTablesForBooking(item)
        })
    })

}

function BlockTablesForBooking(item) {
    const selected = document.createElement('button')
    selected.innerHTML = item.name
    selected.value = item.id
    selected.id = `selected${item.name}`
    selectedTables.appendChild(selected)
    selected.addEventListener('click', () => {
        ReleaseTableForBooking(selected)
    })
}

function ReleaseTableForBooking(element) {
    selectedTables.removeChild(element)
}