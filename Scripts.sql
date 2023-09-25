use BeanBookings;

select * from ResturantAreas;
select * from Restaurants;
select * from SittingTypes;

select count(*) from Sittings;

delete from Sittings
where Id >= 1;

select * from Reservations;
select * from sittings
where RestaurantId = 1 and Start >= '2023-09-25 7:00:00 AM' and [End] <= '2023-09-25 11:00:00 PM';

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
where PersonId = 1;



delete from People
where Id = 14;

delete from Reservations
where Id = 11;



select * from ReservationStatuses;
select * from ResevationOrigins;


select * from People;

select * from AspNetUsers;

delete from AspNetUsers
where id >= '3ac25ef3-267e-47d5-a4ef-4781b7db5cb6';

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
