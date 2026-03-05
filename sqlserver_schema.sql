-- SQL Server Database Schema for Movies Project
-- Run this script to create the database structure

USE master;
GO

-- Create database if it doesn't exist
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'movieprojectdb')
BEGIN
    CREATE DATABASE movieprojectdb;
END
GO

USE movieprojectdb;
GO

-- Users table
CREATE TABLE users (
    id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(255) NOT NULL,
    phone NVARCHAR(50),
    username NVARCHAR(100) NOT NULL UNIQUE,
    password NVARCHAR(255) NOT NULL,
    email NVARCHAR(255) NOT NULL UNIQUE,
    active BIT DEFAULT 0,
    emailconfirmed BIT DEFAULT 0,
    role NVARCHAR(50) DEFAULT 'User'
);
GO

-- Genres table
CREATE TABLE genres (
    id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(100) NOT NULL UNIQUE
);
GO

-- Movie table
CREATE TABLE movie (
    id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(255) NOT NULL,
    duration INT NOT NULL,
    release_date DATE NOT NULL,
    amount DECIMAL(10,2) NOT NULL,
    poster_path NVARCHAR(500),
    plot NVARCHAR(MAX),
    actors NVARCHAR(MAX),
    directors NVARCHAR(500)
);
GO

-- MovieGenres junction table
CREATE TABLE moviegenres (
    movieid INT NOT NULL,
    genreid INT NOT NULL,
    PRIMARY KEY (movieid, genreid),
    FOREIGN KEY (movieid) REFERENCES movie(id) ON DELETE CASCADE,
    FOREIGN KEY (genreid) REFERENCES genres(id) ON DELETE CASCADE
);
GO

-- Hall table
CREATE TABLE hall (
    id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(100) NOT NULL,
    rows INT NOT NULL,
    seats_per_row INT NOT NULL
);
GO

-- Hall_seat table
CREATE TABLE hall_seat (
    id INT IDENTITY(1,1) PRIMARY KEY,
    hall_id INT NOT NULL,
    row_number INT NOT NULL,
    seat_number INT NOT NULL,
    FOREIGN KEY (hall_id) REFERENCES hall(id) ON DELETE CASCADE,
    UNIQUE (hall_id, row_number, seat_number)
);
GO

-- Screening table
CREATE TABLE screening (
    id INT IDENTITY(1,1) PRIMARY KEY,
    movie_id INT NOT NULL,
    screening_date_time DATETIME NOT NULL,
    total_tickets INT NOT NULL,
    available_tickets INT NOT NULL,
    hall_id INT NOT NULL,
    ticket_price DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (movie_id) REFERENCES movie(id) ON DELETE CASCADE,
    FOREIGN KEY (hall_id) REFERENCES hall(id)
);
GO

-- Ticket table
CREATE TABLE ticket (
    id INT IDENTITY(1,1) PRIMARY KEY,
    movie_id INT NOT NULL,
    user_id INT NOT NULL,
    watch_movie DATETIME NOT NULL,
    price DECIMAL(10,2) NOT NULL,
    hall_seat_id INT,
    FOREIGN KEY (movie_id) REFERENCES movie(id) ON DELETE CASCADE,
    FOREIGN KEY (user_id) REFERENCES users(id),
    FOREIGN KEY (hall_seat_id) REFERENCES hall_seat(id)
);
GO

-- Seat_for_screening table
CREATE TABLE seat_for_screening (
    id INT IDENTITY(1,1) PRIMARY KEY,
    screening_id INT NOT NULL,
    hall_seat_id INT NOT NULL,
    user_id INT NOT NULL,
    ticket_id INT NOT NULL,
    FOREIGN KEY (screening_id) REFERENCES screening(id) ON DELETE CASCADE,
    FOREIGN KEY (hall_seat_id) REFERENCES hall_seat(id),
    FOREIGN KEY (user_id) REFERENCES users(id),
    FOREIGN KEY (ticket_id) REFERENCES ticket(id),
    UNIQUE (screening_id, hall_seat_id)
);
GO

-- MovieRatings table
CREATE TABLE movieratings (
    id INT IDENTITY(1,1) PRIMARY KEY,
    movie_id INT NOT NULL,
    user_id INT NOT NULL,
    rating DECIMAL(3,2) NOT NULL CHECK (rating >= 0 AND rating <= 10),
    comment NVARCHAR(MAX),
    FOREIGN KEY (movie_id) REFERENCES movie(id) ON DELETE CASCADE,
    FOREIGN KEY (user_id) REFERENCES users(id),
    UNIQUE (movie_id, user_id)
);
GO

-- FutureMovies table
CREATE TABLE futuremovies (
    id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(255) NOT NULL,
    genres NVARCHAR(500),
    poster_path NVARCHAR(500)
);
GO

-- FAQs table
CREATE TABLE faqs (
    id INT IDENTITY(1,1) PRIMARY KEY,
    question NVARCHAR(MAX) NOT NULL,
    answer NVARCHAR(MAX) NOT NULL,
    category NVARCHAR(100),
    createdat DATETIME DEFAULT GETDATE(),
    updatedat DATETIME DEFAULT GETDATE()
);
GO

-- Insert sample genres
INSERT INTO genres (name) VALUES 
('Action'), ('Comedy'), ('Drama'), ('Horror'), ('Sci-Fi'), 
('Romance'), ('Thriller'), ('Documentary'), ('Animation'), ('Fantasy');
GO

PRINT 'Database schema created successfully!';
