create database UserDB

use UserDB

create table UserInformation(
UserId int primary key identity(1,1),
FirstName varchar(50),
LastName varchar(50),
Age int,
Email varchar(100),
UserPassword varchar(200),
IsActive bit default 1,
RegistrationDate datetime default getdate()
)

create table RefreshTokenHistory(
TokenHistoryId int primary key identity,
UserId int references UserInformation(UserId),
Token varchar(500),
RefreshToken varchar(200),
CreationDate datetime,
ExpirationDate datetime,
IsActive AS (iif(ExpirationDate < getdate(), convert(bit,0), convert(bit,1)))
)

