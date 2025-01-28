use beanbookings;

select *  from aspnetusers;
select *  from aspnetroles;
select *  from aspnetuserroles;
select *  from aspnetusertokens;

select * from reservationstatuses;
select * from resevationorigins;
select *  from restaurants;
select * from resturantareas;
select *  from sittingtypes;
select *  from sittings;
select * from restauranttables;

delete from resevationorigins
where id >= 4;

delete from aspnetroles
where id >= "2993c8c6-9ee7-4c2f-9a8c-2848129445b2";
