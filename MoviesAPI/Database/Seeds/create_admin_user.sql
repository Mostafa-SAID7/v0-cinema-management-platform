-- Create an admin user for testing
-- Password: Admin123!

-- Check if admin user already exists
IF NOT EXISTS (SELECT 1 FROM users WHERE username = 'admin')
BEGIN
    INSERT INTO users (name, phone, username, password, email, active, emailconfirmed, role)
    VALUES ('Admin User', '+1234567890', 'admin', 'Admin123!', 'admin@shoftv.com', 1, 1, 'Admin');
    
    PRINT 'Admin user created successfully';
    PRINT 'Username: admin';
    PRINT 'Password: Admin123!';
END
ELSE
BEGIN
    -- Update existing admin user to ensure they have admin role
    UPDATE users 
    SET role = 'Admin', 
        active = 1,
        emailconfirmed = 1
    WHERE username = 'admin';
    
    PRINT 'Admin user already exists - role updated to Admin';
END
GO

-- Also update testuser to be a regular user (not admin)
IF EXISTS (SELECT 1 FROM users WHERE username = 'testuser')
BEGIN
    UPDATE users 
    SET role = 'User'
    WHERE username = 'testuser';
    
    PRINT 'testuser role set to User';
END
GO

-- Display all users with their roles
SELECT username, name, email, role, active AS is_active, emailconfirmed AS email_confirmed
FROM users
ORDER BY role DESC, username;
