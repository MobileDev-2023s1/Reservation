﻿$(() => {   

    $("#RestaurantAreaList").on('change', function () {
        ClearSeletedTablesSection();
        GetTablesByAreaId()
    })

    $('#NewReservationStatusId').on('change', function () {
        $('#assignTable')[0].checked = false;
        $('#assignTableSection').css('display', 'none')
        ClearSeletedTablesSection();
    })

    $('#closeButton').on('click', function () {
        ClearSeletedTablesSection();
    })

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
                RestaurantAreaId: $('#RestaurantAreaList').val(),
                Comments: $('#comments').val(),
                ReservationId: $('#BookingId').val(),
                ReservationStatusId: $('NewReservationStatusId').val(),
            }

            console.log(c.RestaurantTables)

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
            ListOfAreaTables(tables);

        } catch (error) {
            console.log(error)
        }

    }


}

function ListOfAreaTables(tables) {

    while ($('#tablesInArea').children().length > 0) {
        $('#tablesInArea').children().remove()
    }

    tables.forEach((item) => {
        let option = $('<button>')
            .val(item.id)
            .prop({ id: `selected${(item.name)}` })
            .addClass('image-button')
            .css({
                'width': '30px',
                'height': '30px',
                'background-image': `url("/images/icons/main-table.png")`,
                'background-size': 'cover',
                'border': 'none',
                'color': 'red',
                'font-height': '15'
            })
            .html(item.name);

        $('#tablesInArea').append(option);
        
        if (!item.status) {
            option.prop('disabled', true)
                .addClass('image-button')
                .css({
                    'background-image': `url("/images/icons/table-reserved.png")`,
                })

            if (item.reservationId == $('#BookingId').val()) {
                BlockTablesForBooking(item)
            }
        } 

        option.click(function (){
            let exist = $('#SelectedTables').contents().filter(function () { return this.nodeType === 1 && $(this).val() === option.val()}).length > 0;
            if (!exist) {
                    $('#UserAlert').css({ display: 'none' })
                    BlockTablesForBooking(item)
            } else
            {
                $('#UserAlert').html('Each table can only be selected once. Select another').css({ display: 'flex' }).addClass('alert alert-danger')
            }
        })
    })
} 


function ClearSeletedTablesSection() {
    while ($('#SelectedTables').children().length > 0) {
        $('#SelectedTables').children().remove()
    }

}

function BlockTablesForBooking(item) {
    let selected = $('<button>');
    selected
        .val(item.id)
        .prop({ id: `selected${(item.name)}` })
        .addClass('image-button')
        .css({
            'width': '30px',
            'height': '30px',
            'background-image': `url("/images/icons/main-table.png")`,
            'background-size': 'cover', 
            'border': 'none', 
            'color': 'red',
            'font-height': '15'
        })
        .html(item.name);
      
      
    $('#SelectedTables').append(selected)

    selected.click(function () {$(`#${selected.prop('id')}`).remove();})
}

function FinalTablesList() {

    let listTables = [];

    const selectedTables = document.getElementById('SelectedTables')

    selectedTables.childNodes.forEach(item => {
        listTables.push({ id: item.value })
    })

    listTables.splice(0, 1)
    return listTables;
}