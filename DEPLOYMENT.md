# MoviesAPI Deployment Guide

## Production Configuration

### Backend API
- **URL**: https://movie-api73.runasp.net
- **Database**: db43476.public.databaseasp.net
- **Frontend**: https://shof-tv.vercel.app/

## Database Connection

The production database connection string is configured in `appsettings.json`:

```json
"DBSettings": {
  "SqlServerDB": "Server=db43476.public.databaseasp.net;Database=db43476;User Id=db43476;Password=Dq4+3?gKfP@9;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True"
}
```

## Deployment Steps

### 1. Database Setup

Run the SQL schema file to create the database structure:
```bash
sqlserver_schema.sql
```

### 2. Seed Initial Data

After deployment, call the seed endpoint to populate the database:
```bash
POST https://movie-api73.runasp.net/api/seed/all
```

This will create:
- 6 sample movies
- Multiple screenings for each movie
- Sample halls

### 3. Deploy to RunASP.NET

1. Push changes to GitHub:
   ```bash
   git add .
   git commit -m "your commit message"
   git push origin main
   ```

2. Deploy to movie-api73.runasp.net through your hosting provider

### 4. Verify Deployment

Test the following endpoints:

- `GET /api/movies` - Should return all movies
- `GET /api/movies/genres` - Should return all genres
- `POST /api/account/register` - Should create a new user
- `POST /api/account/login` - Should authenticate and return token

## CORS Configuration

The API is configured to allow all origins for development:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
```

**For production**, consider restricting to specific origins:

```csharp
policy.WithOrigins("https://shof-tv.vercel.app")
      .AllowAnyMethod()
      .AllowAnyHeader();
```

## Environment-Specific Configuration

### Local Development
```json
"DBSettings": {
  "SqlServerDB": "Server=localhost;Database=movieprojectdb;Integrated Security=True;TrustServerCertificate=True"
}
```

### Production
```json
"DBSettings": {
  "SqlServerDB": "Server=db43476.public.databaseasp.net;Database=db43476;User Id=db43476;Password=Dq4+3?gKfP@9;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True"
}
```

## API Endpoints

### Movies
- `GET /api/movies` - Get all movies
- `GET /api/movies/{id}` - Get movie by ID
- `GET /api/movies/genre/{genre}` - Get movies by genre
- `GET /api/movies/genres` - Get all genres
- `GET /api/movies/toprated/{n}` - Get top N rated movies
- `POST /api/movies` - Create movie
- `PUT /api/movies/{id}` - Update movie
- `DELETE /api/movies/{id}` - Delete movie
- `PUT /api/movies/{movieId}/rating` - Rate movie

### Authentication
- `POST /api/account/register` - Register new user
- `POST /api/account/login` - Login user
- `POST /api/account/logout` - Logout user
- `POST /api/account/forgotpassword` - Request password reset
- `POST /api/account/resetpassword` - Reset password

### Screenings
- `GET /api/screenings` - Get all screenings
- `GET /api/screenings/{id}` - Get screening by ID
- `GET /api/screenings/{id}/reservedseats` - Get reserved seats
- `POST /api/screenings/{id}/book` - Book seats
- `POST /api/screenings` - Create screening
- `PUT /api/screenings/{id}` - Update screening
- `DELETE /api/screenings/{id}` - Delete screening

### Tickets
- `GET /api/tickets` - Get all tickets
- `GET /api/tickets/user/{userId}` - Get user tickets

### Seed Data
- `POST /api/seed/movies` - Seed movies
- `POST /api/seed/screenings` - Seed screenings
- `POST /api/seed/all` - Seed all data

## Troubleshooting

### Database Connection Issues
- Verify connection string is correct
- Check firewall rules allow connection from hosting server
- Verify database user has proper permissions

### CORS Errors
- Update CORS policy to include frontend domain
- Verify CORS middleware is properly configured

### Authentication Issues
- Check JWT configuration in Program.cs
- Verify email service configuration for password reset
- Check that AccountController is properly registered
