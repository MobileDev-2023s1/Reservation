use BeanBookings;

select * from ResturantAreas;
select * from Restaurants;

select * from Sittings;


delete from People
where id >= 1;

select * from Reservations;

select st.Name as Sitting_type, CONCAT(pr.FirtName, ' ', pr.LastName) as Customer_name,
Reservations.Start as Start, rsSt.Name as Status, resOr.Name as Origin,
Reservations.Guests as Guests, place.Name as Place
from Reservations
join Sittings as st on Reservations.SittingID = st.Id
join People as pr on Reservations.PersonId = pr.Id
join ReservationStatuses as rsSt on Reservations.ReservationStatusID = rsSt.Id
join ResevationOrigins as resOr on Reservations.ResevationOriginId = resOr.Id
join Restaurants as place on st.RestaurantId = place.Id
where PersonId = 13;



delete from People
where Id >= 1;

delete from Reservations
where Id >= 1;



select * from ReservationStatuses;
select * from ResevationOrigins;

select * from People;

select * from AspNetUsers;

delete from AspNetUsers
where id >= '04540b37-783b-4fdb-b272-59e756bef703';

delete from AspNetUsers
where id = '706fbd1e-cda8-4f82-9d60-c473bbdeb52c';

select * from AspNetRoles;
select * from AspNetUserRoles;



delete from People
where id >= 1;

select * from AspNetUsers;



select * from ReservationRestaurantTable;


select * from Sittings;

select * from Restaurants;
