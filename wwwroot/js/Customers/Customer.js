const baseURL = "https://localhost:7113";

document.addEventListener("DOMContentLoaded", () => {
    
    const selectedDate = document.getElementById("Date");
    const id = document.getElementById("RestaurantId");
    const menu = document.getElementById("MenuType");

    selectedDate.addEventListener("focusout", async () => {
        try {
            console.log(id.value)
            console.log(selectedDate.value)

            const menuData = await GetListOfAreas(id.value, selectedDate.value);

            menuData.forEach(menuItem => {
                const option = document.createElement("option");
                option.value = menuItem.id;
                option.textContent = menuItem.name;
                console.log(option);
                menu.appendChild(option);
            })

            
            
        } catch (error) {
            console.log(error);
            alert(error);
        }

    });
    


    async function GetListOfAreas(restaurantId, selectedDate) {

        const url = new URL("/Customers/Bookings/GetRestuarantData?Id="+restaurantId+"&Date="+selectedDate, baseURL);

        try {
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

