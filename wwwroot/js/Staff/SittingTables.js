
const listofTables = document.getElementById('tablesInArea');
const selectedTables = document.getElementById('SelectedTables')

$(() => {
    /*$('#tablesInArea').addClass("btn-group")*/
    /*listofTables.class = "btn-group";*/


    /*listofTables.role = "group"*/
    /*listofTables.ariaLabel = "Basic Example"*/
})

async function GetTablesByAreaId()
{
    
    if ($('#CurrentRestaurantArea').val() === undefined) {
        
        return null;
    } else {
        try {

            var c = {
                StartTime: new Date($('#Date').val()),
                Duration: $('#duration').val(),
                Guests: $('#guests').val(),
                SittingId: $('#MenuType').val(),
                PersonId: $('PersonId').val(),
                RestaurantAreaId: RestaurantAreaList.value,
                Comments: $('#comments').val(),
                ReservationId: $('#BookingId').val(),
                ReservationStatusId: $('NewReservationStatusId').val(),
                selectedTables: $('#ListOfTables')
            }

            console.log($('#SelectedTables'))

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
        option.value = item.id
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
    selected.id = `selected${(item.name)}`

    /*console.log($('#SelectedTables').eq(`#${item.id}`).children())*/
    console.log($('#SelectedTables').children().prop('id', `${item.id}`))
    $('#SelectedTables').append(selected)

    
    selected.addEventListener('click', () => {
        
        console.log($('#SelectedTables').children())
        $(`#${selected.id}`).remove();
    })
}

function FilterTablesDuplicates(element) {

   

    
    
    //$('#SelectedTables').remove(element, e => {
        
    //})
}


function FinalTablesList() {

    let listTables = [];

    selectedTables.childNodes.forEach(item => {
        listTables.push({ id: item.value })
    })
    
    listTables.splice(0, 1)
    console.log(listTables)
    return listTables;
}