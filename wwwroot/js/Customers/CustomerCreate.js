﻿const baseURL = "https://localhost:7113";


document.addEventListener("DOMContentLoaded", () => {

    /**Gets the date chosen by the user for the reservation*/
    const selectedDate = document.getElementById("Date");
    /**Gets the number of people reservation is intended for*/
    const guests = document.getElementById("guests");
    /**Gets the amount of time reservation is intender for*/
    const duration = document.getElementById("duration")

    const id = document.getElementById("RestaurantId");
    const menu = document.getElementById("MenuType");

    const result = document.getElementById("UserAlert");
    const bookingDetails = document.getElementById("BookingDetails");

    let current = new Date();
    current.setMinutes(current.getMinutes() - 1).toLocaleString();

    BookingVariables.addEventListener("focusin", async () => {
        bookingDetails.style = "display: none"
        result.style = "display: none"
    })

    /**
     * event listener for changes in the date and time of the bookings
     */
    BookingVariables.addEventListener("change", async () => {
        try {
            if (DateNotInPast() && BookingBeforeClosing()) {
                //Check at the timeframe chosen what the capacity capacity is
                let timeFrame = await ConvertDateTime(selectedDate.value);

                //retrieves sittings and bookings at the time selected
                const menuData = await GetAvailableSittings(id.value, timeFrame);

                if (menuData != undefined) {
                    //define and validate the total capacity for the restaurant at that the chosen time
                    let totalCapacity = await TotalCapacityValidation(menuData);
                    //checks the current capacity given current level of reservations
                    let currentOccupancy = await CurrentCapacityValidation(menuData)

                    let remainder = totalCapacity - currentOccupancy;

                    //if there is capacity in the restaurant, allow the booking
                    if (remainder >= guests.value) {
                        while (menu.childElementCount > 0) {
                            menu.removeChild(menu.firstChild);
                        }

                        menuData.forEach(menuItem => {
                            const option = document.createElement("option");
                            option.value = menuItem.id;
                            option.textContent = menuItem.name;
                            menu.appendChild(option);
                        })
                    } else //if there isn't capacity, then give message to choose other time. 
                    {
                        DisplayDangerAlert("We cannot accomodate your party at the time. Please chose an alternative time.");
                    }
                }
            } else if (!BookingBeforeClosing()) {
                DisplayDangerAlert("Please reduce desired time. Booking cannot go over closing time");
            } else {
            DisplayDangerAlert("Booking date cannot be in the past. Please chose an alternative time.");
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
    async function GetAvailableSittings(restaurantId, timeFrame) {
        try {
            //validate time amd date: time has to be after 7am open time
            //and before 
            const result = TimeValidation(selectedDate.value);
            /*const capacityValidation */
            if (result) {
                /*const url = new URL("/Customers/Bookings/GetAvailableSittings?Id=" + restaurantId + "&Date=" + selectedDate, baseURL);*/
                const url = new URL("/Customers/Bookings/GetAvailableSittings?Id=" + restaurantId + "&begin=" + timeFrame[0] + "&final=" + timeFrame[1], baseURL);
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

    function DateNotInPast() {
        return new Date(Date.parse(selectedDate.value)) >= current ? true : false;
    }

    function BookingBeforeClosing() {
        var currentDate = new Date(Date.parse(selectedDate.value)).getHours() + (duration.value / 60);
        

        return currentDate < 23 ? true : false;
    }

    /**
     * Converts the date selected into database search time
     * @param {any} selectedDate
     * @returns
     */
    async function ConvertDateTime(date) {
        try {
            /*const capacityValidation */
            const url = new URL("/Customers/Bookings/ConvertDateTime?date=" + date, baseURL);
            const response = await fetch(url);

            if (!response.ok) {
                throw new Error("HTTP error " + response.status);
            }
            const data = await response.json();
            return await data;
        } catch (error) {
            console.error(error);
        }
    };
    function DisplayDangerAlert(message) {
        result.style = "display: flex"
        bookingDetails.style = "display: none"
        result.className = "alert alert-danger"
        result.innerHTML = message;
    }

    /**
     * validate that date time is between 7 AM and 11 PM.
     * Kitchen closes at 10 so if someone books between 10 and 11 display alert  
     * */
    function TimeValidation(selectedDate) {
        try {
            const hourOfDay = new Date(Date.parse(selectedDate)).getHours();

            if (hourOfDay >= 7 && hourOfDay < 22) {
                result.style = "display: none"
                bookingDetails.style = "display: contents"
                return true;
                
            } else if (hourOfDay >= 22 && hourOfDay < 23) {
                result.style = "display: flex"
                result.className = "alert alert-warning"
                result.innerHTML = "We are open!! However, our kitchen is closed. You may order beverages only."
                bookingDetails.style = "display: contents"
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

    /**
     * Validates the total number of people a sitting can accomodate
     * @param {any} menuData includes an object that contains all sittings active at the time salecte
     * it includes all reservations made between time frame
     * @returns count with the amount of people 
     */
    async function TotalCapacityValidation(menuData) {
        try {
            let count = 0 
            menuData.forEach(item => {
                count = count + item.capacity;
            })

            return await count;
        } catch (error) {
            console.error(error);
        }
    }

    /**
     * Returns the number of people we can accomodate in a sitting given the reservations 
     * allocated to the sitting
     * @param {any} menuData
     * @returns the number of people reservations for a given booking time
     */
    async function CurrentCapacityValidation(menuData) {
        try {
            let count = 0
            menuData.forEach(item => {
                item.reservations.forEach(res => {
                    count = count + res.guests;
                })
            })
            return await count;
        } catch (error) {
            console.error(error);
        }
    }

    

})

