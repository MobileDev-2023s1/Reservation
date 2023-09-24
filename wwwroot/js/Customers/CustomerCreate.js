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

            //add function to call time conversion for bookings ...
            

            /**
             * add a function that calls the function reservation within selected times and 
             * if there is capacity then check which sitting is available at that time. 
             * 
             * else ignore rest and include alert on screen about capacity
             */

            //change this function to get the list of sittings and update name
            const menuData = await GetListOfAreas(id.value, selectedDate.value);

            if (menuData != undefined) {
                while (menu.childElementCount>0) {
                    menu.removeChild(menu.firstChild);
                }
                menuData.forEach(menuItem => {
                    const option = document.createElement("option");
                    option.value = menuItem.id;
                    option.textContent = menuItem.name;
                    menu.appendChild(option);
                })
            }


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
            const result = TimeValidation(selectedDate);
            /*const capacityValidation */
            if (result) {
                const url = new URL("/Customers/Bookings/GetRestuarantData?Id=" + restaurantId + "&Date=" + selectedDate, baseURL);
                const response = await fetch(url);

                if (!response.ok) {
                    throw new Error("HTTP error " + response.status);
                }

                const data = await response.json();
                console.log(data);
                return await data;
            } 

        } catch (error) {
            console.error(error);
        }
          
    };

    /**
     * validate that date time is between 7 AM and 11 PM.
     * Kitchen closes at 10 so if someone books between 10 and 11 display alert  
     * */
    function TimeValidation(selectedDate) {
        try {
            const hourOfDay = new Date(Date.parse(selectedDate)).getHours();
            const bookingDetails = document.getElementById("BookingDetails");
            const result = document.getElementById("HourAlert");
            
            if (hourOfDay >= 7 && hourOfDay < 22) {
                result.style = "display: none"
                bookingDetails.style = "display: contents"
                return true;
                
            } else if (hourOfDay >= 22 && hourOfDay < 23) {
                result.style = "display: flex"
                result.className = "alert alert-warning"
                result.innerHTML = "We are open!! However, our kitchen is closed. You may order beverages only."
                bookingDetails.style == null ? true : "display: contents"
                return true;
            } else if (hourOfDay < 7 || hourOfDay >= 23) {
                result.style = "display: flex"
                bookingDetails.style = "display: none"
                result.className = "alert alert-danger"
                result.innerHTML = "Not trading hours. Please chose an alternative time."
                return false;
            } 

        } catch (error) {

        }
    }


    function capacityValidation() {

    }
})

