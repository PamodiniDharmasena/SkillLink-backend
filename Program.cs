using SkillLink.ApplicationServices.JobActivityModule;
using SkillLink.ApplicationServices.ProfileModule;
using SkillLink.DataServices.JobActivityModule;
using SkillLink.DataServices.ProfileModule;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SkillLink.ApplicationServices.AuthModule;
using SkillLink.DataServices.AuthModule;
using SkillLink.ApplicationServices.NotificationModule;
using SkillLink.DataServices.NotificationModule;
using SkillLink.RealTimeUpdates;
using Microsoft.AspNetCore.SignalR;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add SignalR
builder.Services.AddSignalR();

builder.Services.AddControllers();

// Configure JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddScoped<ProfileApplicationService>();
builder.Services.AddScoped<ProfileDataService>();  // Add ProfileDataService
builder.Services.AddScoped<JobCardApplicationService>();
builder.Services.AddScoped<JobCardDataService>();
builder.Services.AddScoped<AuthApplicationService>(); // Register AuthApplicationService
builder.Services.AddScoped<AuthDataService>();
//builder.Services.AddScoped<NotificationApplicationService>(); // Register AuthApplicationService
//builder.Services.AddScoped<NotificationDataService>();
//builder.Services.AddHostedService<NotificationService>();


//builder.Services.AddScoped<DatabaseService>();
builder.Services.AddSingleton<DatabaseService>();

// Add the NotificationService and provide the connection string from configuration





builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000") // Your frontend URL
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});





var app = builder.Build();

app.UseCors("AllowSpecificOrigin");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseAuthentication(); // Add authentication middleware
app.UseAuthorization();

app.MapControllers();

// Map the SignalR hub
app.MapHub<NotificationHub>("/notificationHub");

app.Run();
