-- Insert test user directly into database
-- Run this against your database: db43476.public.databaseasp.net

-- Check if user already exists
IF NOT EXISTS (SELECT * FROM users WHERE username = 'testuser')
BEGIN
    INSERT INTO users (name, phone, username, password, email, active, emailconfirmed, role)
    VALUES ('Test User', '+1234567890', 'testuser', 'Test123!', 'test@shoftv.com', 1, 1, 'User');
    PRINT 'Test user created successfully';
END
ELSE
BEGIN
    PRINT 'Test user already exists';
END
GO

-- Check if admin already exists
IF NOT EXISTS (SELECT * FROM users WHERE username = 'admin')
BEGIN
    INSERT INTO users (name, phone, username, password, email, active, emailconfirmed, role)
    VALUES ('Admin User', '+1234567891', 'admin', 'Admin123!', 'admin@shoftv.com', 1, 1, 'Admin');
    PRINT 'Admin user created successfully';
END
ELSE
BEGIN
    PRINT 'Admin user already exists';
END
GO

-- Display all users
SELECT id, name, username, email, active, emailconfirmed, role FROM users;
GO
