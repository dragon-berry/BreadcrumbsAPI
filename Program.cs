// ------------------------------------------------------------
// Program.cs – entry point & configuration for the Breadcrumbs API
// ------------------------------------------------------------
// This file uses the "top‑level statements" feature introduced in C# 9.
// Think of it as the Main method: code here executes at start‑up to
// configure services, middleware, and ultimately run the web server.
// ------------------------------------------------------------

// NOTE: Using‑directives are usually at the top of the file. They are
// omitted here for brevity – assume the standard ASP.NET Core / EF Core
// / Identity / MediatR / JWT namespaces are imported.
// You can find all the imports inside the GlobalUsings.cs file.

// 1️. Build the WebApplication builder – provides DI, config, logging …
var builder = WebApplication.CreateBuilder(args);

// 2️. Make HttpContext available via DI (e.g., in services / repos)
builder.Services.AddHttpContextAccessor();

// 3️. Resolve the database connection string.
//     • Prefer the AZURE_SQL_CONNECTIONSTRING environment variable when
//       running in the cloud (keeps secrets out of appsettings.json).
//     • Fallback to the value in configuration files for local dev.
string connectionString = Environment.GetEnvironmentVariable("AZURE_SQL_CONNECTIONSTRING")
    ?? builder.Configuration.GetConnectionString("DefaultConnection")!;

//Update the configuration so everything downstream sees the resolved value.
builder.Configuration["ConnectionStrings:DefaultConnection"] = connectionString;

// 4️. Register the EF Core DbContext – the heart of data access.
builder.Services.AddDbContext<BreadcrumbsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 5️. Add MVC controllers (attribute‑routed APIs).
builder.Services.AddControllers();

// 6️. Add minimal‑API exploration + Swagger/OpenAPI for interactive docs.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 7. MediatR – CQRS / in‑process messaging library (Command/Query handlers).
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly));

// ------------------------------------------------------------
// 🔐  JWT Authentication
// ------------------------------------------------------------
// Read JWT settings from configuration, guard against missing values.
var jwtSettings = Guard.Against.Null(builder.Configuration.GetSection("JwtOptions"));
var secret = Guard.Against.NullOrEmpty(jwtSettings.GetValue<string>("Secret"));
var issuer = Guard.Against.NullOrEmpty(jwtSettings.GetValue<string>("Issuer"));
var audience = Guard.Against.NullOrEmpty(jwtSettings.GetValue<string>("Audience"));
var key = Encoding.ASCII.GetBytes(secret);

// Configure the authentication handler to validate JWTs on incoming requests.
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = issuer,
        ValidateAudience = true,
        ValidAudience = audience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero // no extra grace period
    };
});

// ------------------------------------------------------------
// 🔑  Authorization – policy requiring authenticated users by default.
// ------------------------------------------------------------
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Default", new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser()
        .Build());
});

// ------------------------------------------------------------
// 👤  ASP.NET Core Identity – user management & password hashing.
// ------------------------------------------------------------
builder.Services.AddIdentity<User, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<BreadcrumbsDbContext>()
    .AddDefaultTokenProviders();

// Fine‑tune Identity options (password rules, lockout, etc.).
builder.Services.Configure<IdentityOptions>(options =>
{
    // 🔒  Lockout settings – protect against brute‑force login attempts.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // 🔑  Password requirements – relaxed a bit for testing purposes.
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false; // special char not required
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // 📧  Sign‑in settings.
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;

    options.User.RequireUniqueEmail = true;
});

// ------------------------------------------------------------
// 🏗️  Application‑specific services / repositories.
// ------------------------------------------------------------
builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<IJwtUtils, JwtUtils>();
builder.Services.AddScoped<ICrumbRepository, CrumbRepository>();
builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<ICodeValueRepository, CodeValueRepository>();

// ------------------------------------------------------------
// 🌐  CORS – allow requests from any origin (relaxed for now; tighten later).
// ------------------------------------------------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "Breadcrumbs", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// ------------------------------------------------------------
// 🚀  Build the app & configure the HTTP request pipeline.
// ------------------------------------------------------------
var app = builder.Build();

// Enable CORS policy defined above.
app.UseCors("Breadcrumbs");

// Swagger UI – currently exposed in all environments for testing purposes.
// if (app.Environment.IsDevelopment())
// {
app.UseSwagger();
app.UseSwaggerUI();
// }

// Enforce HTTPS redirection (will send 308 responses when HTTP is used).
app.UseHttpsRedirection();

// Add authentication & authorization middleware to the pipeline.
app.UseAuthentication();
app.UseAuthorization();

// Map attribute‑routed controllers (e.g., /api/crumbs).
app.MapControllers();

// Finally, start listening for incoming HTTP requests.
app.Run();