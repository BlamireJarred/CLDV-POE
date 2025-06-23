--DATABASE CREATION
USE master
IF EXISTS (SELECT*FROM sys.databases WHERE name = 'VenueBookingDB')
DROP DATABASE VenueBookingDB
CREATE DATABASE VenueBookingDB

USE VenueBookingDB

-- Create Venue table
CREATE TABLE Venue (
    VenueId INT PRIMARY KEY IDENTITY(1,1),
    VenueName NVARCHAR(100) NOT NULL,
    Location NVARCHAR(200) NOT NULL,
    Capacity INT NOT NULL,
    ImageUrl NVARCHAR(500) NULL,
    CONSTRAINT CHK_Capacity CHECK (Capacity > 0)
);

-- Create Event table
CREATE TABLE Event (
    EventId INT PRIMARY KEY IDENTITY(1,1),
    EventName NVARCHAR(100) NOT NULL,
    EventDate DATETIME2 NOT NULL,
    Description NVARCHAR(MAX) NULL,
    VenueId INT NOT NULL,
    CONSTRAINT FK_Event_Venue FOREIGN KEY (VenueId) REFERENCES Venue(VenueId),
    CONSTRAINT CHK_EventDate CHECK (EventDate > GETDATE())
);

-- Create Booking table
CREATE TABLE Booking (
    BookingId INT PRIMARY KEY IDENTITY(1,1),
    EventId INT NOT NULL,
    VenueId INT NOT NULL,
    BookingDate DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Booking_Event FOREIGN KEY (EventId) REFERENCES Event(EventId),
    CONSTRAINT FK_Booking_Venue FOREIGN KEY (VenueId) REFERENCES Venue(VenueId),
    CONSTRAINT UQ_EventVenueDate UNIQUE (EventId, VenueId, BookingDate)
);


INSERT INTO Venue ( VenueName, Location, Capacity, ImageURL)
Values ( 'Venuename', 'krugersdorp', 50, 'URL.com')

SELECT*FROM Venue