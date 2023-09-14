use BeanBookings;

select * from ResturantAreas;
select * from Restaurants;
select * from Reservations;
select * from Sittings;

select * from People;



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





delete from Reservations
where Id >= 1;



select * from ReservationStatuses;
select * from ResevationOrigins;

select * from AspNetUsers;
select * from AspNetRoles;
select * from AspNetUserRoles;


select * from People
where UserId = 'dcf5c1fb-0bc3-4dec-a023-5cf26f949446';

delete from People
where id >= 1;

select * from AspNetUsers;



select * from ReservationRestaurantTable;


select * from Sittings;

select * from Restaurants;
