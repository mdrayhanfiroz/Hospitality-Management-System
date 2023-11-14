create database Travel_Db
go

use Travel_Db
go

create table Client(
	ClientId int identity primary key,
	ClientName nvarchar(50) not null,
	DateOfBirth date not null,
	Age int not null,
	Picture nvarchar(max) null,
	MaritalStatus bit null
)
go

create table Spot(
	SpotId int identity primary key,
	SpotName nvarchar(50) not null
)
go

select * from Spot
go


create table BookingEntry(
	BookingEntryId int identity primary key,
	ClientId int not null references Client(ClientId),
	SpotId int not null references Spot(SpotId)
)
go

insert into Spot values('Cox Bazar')
insert into Spot values('Saint Martin')
insert into Spot values('Sajek')
insert into Spot values('Ladhak')
insert into Spot values('Kuakata')
insert into Spot values('Jaflong')