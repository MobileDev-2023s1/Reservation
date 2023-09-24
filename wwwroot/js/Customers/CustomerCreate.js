const baseURL = "https://localhost:7113";

document.addEventListener("DOMContentLoaded", () => {
    
    const selectedDate = document.getElementById("Date");
    const id = document.getElementById("RestaurantId");
    const menu = document.getElementById("MenuType");

    /**
     * event listener for changes in the date and time of the bookings
     */
    selectedDate.addEventListener("focusout", async () => {
        try {
            const menuData = await GetListOfAreas(id.value, selectedDate.value);

            while (menu.childElementCount>0) {
                menu.removeChild(menu.firstChild);
            }

            menuData.forEach(menuItem => {
                const option = document.createElement("option");
                option.value = menuItem.id;
                option.textContent = menuItem.name;
                menu.appendChild(option);
            })
        } catch (error) {
            console.log(error);
            alert(error);
        }

    });
    

    /**Get the type of food that is going to be offered by the restaurant, based on the time of the booking and the date
     * @param {any} restaurantId
     * @param {any} selectedDate
     * @returns
     */
    async function GetListOfAreas(restaurantId, selectedDate) {
        try {
            //validate time amd date: time has to be after 7am open time
            //and before 

            const url = new URL("/Customers/Bookings/GetRestuarantData?Id="+restaurantId+"&Date="+selectedDate, baseURL);
            const response = await fetch(url);

            if (!response.ok) {
                throw new Error("HTTP error " + response.status);
            }

            const data = await response.json();
            console.log(data);
            return await data;

        } catch (error) {
            console.error(error);
        }
          
    };

})

