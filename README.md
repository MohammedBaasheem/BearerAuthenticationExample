# JWT Bearer Authentication in ASP.NET Core

## Overview
This project demonstrates how to implement **JWT Bearer Authentication** in an **ASP.NET Core Web API** application. It includes user registration, login, and securing API endpoints using **JWT tokens**.

## Technologies Used
- **ASP.NET Core**
- **Entity Framework Core**
- **Microsoft Identity Model Tokens**
- **SQL Server**
- **Swagger**

## Features
- User **registration** with JWT token generation.
- User **login** and JWT issuance.
- Secured API endpoints using the `[Authorize]` attribute.

## Configuration
### 1. JWT Configuration
Define JWT settings in `appsettings.json`:
```json
"Jwt": {
  "Issuer": "your-issuer",
  "Audiense": "your-audience",
  "SigningKey": "your-secret-key",
  "Lifetime": 60
}
```

### 2. Setting Up Authentication in `Program.cs`
```csharp
var jwt = builder.Configuration.GetSection("Jwt").Get<Jwt>();

builder.Services.AddAuthentication()
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwt.Issuer,
                        ValidateAudience = true,
                        ValidAudience = jwt.Audiense,
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.SigningKey)),
                        ValidateIssuerSigningKey = true
                    };
                });

builder.Services.AddSingleton(jwt);
```

### 3. AuthenticationController
#### **Register User and Generate JWT**
```csharp
[HttpPost("register")]
public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto dto)
{
    var newUser = new User { Username = dto.Username, Email = dto.Email, Password = dto.Password, Name = dto.Name };
    await _dbcontext.Users.AddAsync(newUser);
    await _dbcontext.SaveChangesAsync();

    var token = GenerateJwtToken(newUser);
    return Ok(token);
}
```
#### **User Login and JWT Token Generation**
```csharp
[HttpPost("login")]
public async Task<IActionResult> LoginAsync([FromBody] LoginDto dto)
{
    var user = await _dbcontext.Users.FirstOrDefaultAsync(u => u.Username == dto.Username && u.Password == dto.Password);
    if (user == null) return BadRequest("Invalid credentials.");
    
    var token = GenerateJwtToken(user);
    return Ok(token);
}
```

### 4. Protecting API Endpoints
```csharp
[Authorize]
[HttpGet("weather")]
public IEnumerable<WeatherForecast> Get()
{
    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
    {
        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
        TemperatureC = Random.Shared.Next(-20, 55),
        Summary = "Sunny"
    }).ToArray();
}
```

### 5. Running the Application
1. Configure the **database connection** in `appsettings.json`.
2. Run **migrations** if using **Entity Framework Core**.
3. Start the API using `dotnet run`.
4. Use **Swagger UI** or **Postman** to test the authentication endpoints.

## Conclusion
This example demonstrates a **basic JWT authentication mechanism** in **ASP.NET Core**, allowing users to register, log in, and access protected resources securely.

