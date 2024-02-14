using DiscussionForum.Data;
using DiscussionForum.Services;
using DiscussionForum.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Cryptography;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DiscussionForum_API")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDev",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

var jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>();
var jwtAudience = builder.Configuration.GetSection("Jwt:Audience").Get<string>();
var jwtKey = builder.Configuration.GetSection("Jwt:Key").Get<string>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SuperAdmin", policy => policy.RequireRole("SuperAdmin"));
});



// Add services to the container.
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDesignationService, DesignationService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ICommunityCategoryService, CommunityCategoryService>();
builder.Services.AddScoped<ICommunityStatusService, CommunityStatusService>();
builder.Services.AddScoped<IThreadStatusService, ThreadStatusService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICommunityService, CommunityService>();
builder.Services.AddScoped<ICommunityCategoryMappingService, CommunityCategoryMappingService>();
builder.Services.AddScoped<INoticeService, NoticeService>();
builder.Services.AddScoped<IThreadService, ThreadService>();
builder.Services.AddScoped<IReplyService, ReplyService>();
builder.Services.AddScoped<IPointService, PointService>();
builder.Services.AddScoped<IThreadVoteService, ThreadVoteService>();
builder.Services.AddScoped<IReplyVoteService, ReplyVoteService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IMailjetService, MailjetService>();



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();



app.UseMiddleware<RequestLoggingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngularDev");

app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
