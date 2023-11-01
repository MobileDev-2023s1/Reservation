use BeanBookings;

select * from ResturantAreas;
select * from Restaurants;
select * from SittingTypes;

select count(*) from Sittings;

delete from RestaurantTables
where Id >= 1;

select * from People;

select * from Reservations
where Start >= '2023-11-01' and Start <= '2023-11-30'

delete from Reservations
where id = 6;

update Reservations
set Start = '2023-10-24 8:28:00 PM'
where Id >= 6;

select * from sittings
where RestaurantId = 1 and Start <= '2023-10-24 8:28:00 AM' and [End] >= '2023-10-24 8:28:00 AM';

select * from Sittings
where Id = 4536;

select * from ReservationRestaurantTable

select * from Reservations as res
join RestaurantTables as rt on res.RestaurantAreaId = rt.RestaurantAreaId


select * from RestaurantTables
where RestaurantAreaId = 4;

select * from Reservations
where SittingID = 122

delete from RestaurantTables
where id >= 61


update RestaurantTables
set Name = 'M1', RestaurantAreaId=4;

select * from Reservations
where Start >= '2023-10-24 07:00:00' and Start <= '2023-10-31 11:00:00 PM';

update Reservations 
set ReservationStatusID = 2
where Id = 1035;

select COUNT(*) from Reservations
where Start >= '2023-10-01 7:00:00 AM' and Start <= '2023-10-31 11:00:00 PM'

Select * from Sittings
where name = 'Continental Lunch' and RestaurantId = 1;



delete from People
where id >= 1;



select st.Name as Sitting_type, CONCAT(pr.FirtName, ' ', pr.LastName) as Customer_name,
Reservations.Start as Start, rsSt.Name as Status, resOr.Name as Origin,
Reservations.Guests as Guests, place.Name as Place
from Reservations
join Sittings as st on Reservations.SittingID = st.Id
join People as pr on Reservations.PersonId = pr.Id
join ReservationStatuses as rsSt on Reservations.ReservationStatusID = rsSt.Id
join ResevationOrigins as resOr on Reservations.ResevationOriginId = resOr.Id
join Restaurants as place on st.RestaurantId = place.Id
where place.Name <> 'Opera Bar' and Reservations.Start >=  '2023-10-01 7:00:00 AM' and Reservations.Start <= '2023-10-31 11:00:00 PM'
and rsSt.Name = 'Pending';



delete from People
where Id = 4;

delete from Reservations
where Id = 11;



select * from ReservationStatuses;

select * from ResevationOrigins;




select * from AspNetUsers;

delete from AspNetUsers
where id >= 'baaefd53-1772-42b0-99c7-1d62505f4c03';

delete from AspNetUsers
where id = '7c18ea7d-be1f-4635-98f8-e474ac1731fb';

select * from AspNetRoles;
select * from AspNetUserRoles;

select count (*) from sittings
where RestaurantId = 2;


delete from People
where id >= 1;

select * from AspNetUsers;



select * from ReservationRestaurantTable;


select * from Restaurants;
