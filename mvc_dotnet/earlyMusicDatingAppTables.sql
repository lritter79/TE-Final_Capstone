-- Switch to the system (aka master) database
USE master;
GO

-- Delete the NPGeek Database (IF EXISTS)
DROP DATABASE IF EXISTS EarlyMusicDating;
GO

-- Create a new NPGeek Database
CREATE DATABASE EarlyMusicDating;
GO

-- Switch to the NPGeek Database
USE EarlyMusicDating;
GO

BEGIN TRANSACTION;

CREATE TABLE User (
ID int identity (1, 1) NOT NULL,
email varchar (100) NOT NULL,
username varchar(50) NOT NULL,
birtdate datetime NOT NULL,
home_city varchar(50) NOT NULL,
home_state varchar(50) NOT NULL,
self_description varchar(Max) NOT NULL,
password_hash varchar(50) NOT NULL,
salt varchar(50) NOT NUll,
CONSTRAINT pk_users PRIMARY KEY (ID)

);

CREATE TABLE Places (
ID int identity (1, 1) NOT NULL,
city varchar(50) NOT NULL,
state_name varchar(50) NOT NULL,
from_date datetime NOT NULL,
to_date datetime NOT NULL,
CONSTRAINT fk_places_users FOREIGN KEY (ID) REFERENCES Users (ID)
);

CREATE TABLE Instruments_Played (
ID int identity (1, 1) NOT NULL,
instrument_name varchar(50) NOT NULL,

CONSTRAINT fk_instuments_played_users FOREIGN KEY (ID) REFERENCES Users (ID)
);

COMMIT

