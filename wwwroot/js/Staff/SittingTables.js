$(() => {

    
})


async function GetTablesByAreaId(areaId) {
    if (areaId === undefined) {
        return null;
    } else {
        try {
            const listOfTables = new URL("/Staff/Tables/GetListofTables?areaId=" + areaId, baseURL())
            const response = await fetch(listOfTables)

            if (!response.ok) {
                console.log(response.status)
            }

            console.log(await response.json());
            /*return await response.json();*/

        } catch (error) {
            console.log(error)
        }

    }


}