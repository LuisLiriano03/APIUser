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