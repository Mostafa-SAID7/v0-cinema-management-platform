-- SQL Server Database Schema for Movies Project
-- Modified for existing database db43476

-- Users table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'users')
BEGIN
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
END
GO

-- Genres table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'genres')
BEGIN
    CREATE TABLE genres (
        id INT IDENTITY(1,1) PRIMARY KEY,
        name NVARCHAR(100) NOT NULL UNIQUE
    );
END
GO

-- Movie table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'movie')
BEGIN
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
END
GO

-- MovieGenres junction table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'moviegenres')
BEGIN
    CREATE TABLE moviegenres (
        movieid INT NOT NULL,
        genreid INT NOT NULL,
        PRIMARY KEY (movieid, genreid),
        FOREIGN KEY (movieid) REFERENCES movie(id) ON DELETE CASCADE,
        FOREIGN KEY (genreid) REFERENCES genres(id) ON DELETE CASCADE
    );
END
GO

-- Hall table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'hall')
BEGIN
    CREATE TABLE hall (
        id INT IDENTITY(1,1) PRIMARY KEY,
        name NVARCHAR(100) NOT NULL,
        rows INT NOT NULL,
        seats_per_row INT NOT NULL
    );
END
GO

-- Hall_seat table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'hall_seat')
BEGIN
    CREATE TABLE hall_seat (
        id INT IDENTITY(1,1) PRIMARY KEY,
        hall_id INT NOT NULL,
        row_number INT NOT NULL,
        seat_number INT NOT NULL,
        FOREIGN KEY (hall_id) REFERENCES hall(id) ON DELETE CASCADE,
        UNIQUE (hall_id, row_number, seat_number)
    );
END
GO

-- Screening table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'screening')
BEGIN
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
END
GO

-- Ticket table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ticket')
BEGIN
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
END
GO

-- Seat_for_screening table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'seat_for_screening')
BEGIN
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
END
GO

-- MovieRatings table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'movieratings')
BEGIN
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
END
GO

-- FutureMovies table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'futuremovies')
BEGIN
    CREATE TABLE futuremovies (
        id INT IDENTITY(1,1) PRIMARY KEY,
        name NVARCHAR(255) NOT NULL,
        genres NVARCHAR(500),
        poster_path NVARCHAR(500)
    );
END
GO

-- FAQs table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'faqs')
BEGIN
    CREATE TABLE faqs (
        id INT IDENTITY(1,1) PRIMARY KEY,
        question NVARCHAR(MAX) NOT NULL,
        answer NVARCHAR(MAX) NOT NULL,
        category NVARCHAR(100),
        createdat DATETIME DEFAULT GETDATE(),
        updatedat DATETIME DEFAULT GETDATE()
    );
END
GO

-- Insert sample genres (only if table is empty)
IF NOT EXISTS (SELECT * FROM genres)
BEGIN
    INSERT INTO genres (name) VALUES 
    ('Action'), ('Comedy'), ('Drama'), ('Horror'), ('Sci-Fi'), 
    ('Romance'), ('Thriller'), ('Documentary'), ('Animation'), ('Fantasy');
END
GO

-- Insert sample halls (only if table is empty)
IF NOT EXISTS (SELECT * FROM hall)
BEGIN
    INSERT INTO hall (name, rows, seats_per_row) VALUES 
    ('Hall 1', 10, 12),
    ('Hall 2', 8, 10),
    ('Hall 3', 12, 15);
    
    -- Create hall seats for Hall 1
    DECLARE @hallId INT = 1;
    DECLARE @row INT = 1;
    DECLARE @seat INT;
    
    WHILE @row <= 10
    BEGIN
        SET @seat = 1;
        WHILE @seat <= 12
        BEGIN
            INSERT INTO hall_seat (hall_id, row_number, seat_number) VALUES (@hallId, @row, @seat);
            SET @seat = @seat + 1;
        END
        SET @row = @row + 1;
    END
    
    -- Create hall seats for Hall 2
    SET @hallId = 2;
    SET @row = 1;
    WHILE @row <= 8
    BEGIN
        SET @seat = 1;
        WHILE @seat <= 10
        BEGIN
            INSERT INTO hall_seat (hall_id, row_number, seat_number) VALUES (@hallId, @row, @seat);
            SET @seat = @seat + 1;
        END
        SET @row = @row + 1;
    END
    
    -- Create hall seats for Hall 3
    SET @hallId = 3;
    SET @row = 1;
    WHILE @row <= 12
    BEGIN
        SET @seat = 1;
        WHILE @seat <= 15
        BEGIN
            INSERT INTO hall_seat (hall_id, row_number, seat_number) VALUES (@hallId, @row, @seat);
            SET @seat = @seat + 1;
        END
        SET @row = @row + 1;
    END
END
GO

PRINT 'Database schema created successfully!';
