function myfuncti() {

    console.log("q damiers")

}

const baseURL = "https://localhost:7113";

async function GetListOfAreas(restaurantId) {
    try {
        const url = new URL("/Customers/Bookings/GetRestuarantData/" + restaurantId, baseURL);
        const response = await fetch(url, {
            method: "GET",
            headers: {
                Accept: "application/json",
                "Content-Type": "application/json",
            },
        });

        if (!response.ok) {
            throw new Error("Request failed with status " + response.status);
        }

        const list = await response.json();

        
        return list;

    } catch (error) {
        console.error("Error signing in to API:", error);
        throw error;
    }

};