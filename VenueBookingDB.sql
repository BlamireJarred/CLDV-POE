--DATABASE CREATION
USE master
IF EXISTS (SELECT*FROM sys.databases WHERE name = 'VenueBookingDB')
DROP DATABASE VenueBookingDB
CREATE DATABASE VenueBookingDB

USE VenueBookingDB

--Table Creation
CREATE TABLE Venue (
VenueId INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
VenueName VARCHAR(50) NOT NULL,
Location VARCHAR(50) NOT NULL,
Capacity INT NOT NULL,
ImageURL VARCHAR(50) NOT NULL
)

CREATE TABLE Event (
EventId INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
EventName VARCHAR(50) NOT NULL,
EventDate DATE NOT NULL,
Description VARCHAR(250) NOT NULL,
VenueId INT,
CONSTRAINT FK_Event_Venue FOREIGN KEY (VenueId) REFERENCES Venue(VenueId) ON DELETE CASCADE
)

CREATE TABLE Booking(
BookingId INT IDENTITY(1,1) PRIMARY KEY,
EventId INT,
CONSTRAINT FK_Booking_Venue FOREIGN KEY (VenueId) REFERENCES Venue(VenueId) ON DELETE CASCADE,
VenueId INT,
CONSTRAINT FK_Booking_Event FOREIGN KEY (EventId) REFERENCES Event(EventId),
BookingDate Date
)

INSERT INTO Venue ( VenueName, Location, Capacity, ImageURL)
Values ( 'Venuename', 'krugersdorp', 50, 'URL.com')

SELECT*FROM Venue