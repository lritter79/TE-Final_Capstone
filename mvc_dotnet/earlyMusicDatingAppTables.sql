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

CREATE TABLE Users (
ID int identity (1, 1) NOT NULL,
username varchar(50) NOT NULL,
birtdate datetime NOT NULL,
home_city varchar(50) NOT NULL,
home_state varchar(50) NOT NULL,
self_description varchar(Max) NOT NULL,
password_hash varchar(50) NOT NULL,
salt varchar(50) NOT NUll,
CONSTRAINT Pk_users PRIMARY KEY (ID)

);

COMMIT

