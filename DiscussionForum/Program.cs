using DiscussionForum.Data;
using DiscussionForum.Services;
using DiscussionForum.UnitOfWork;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DiscussionForum_API")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDev",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});
// Add services to the container.

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IDesignationService, DesignationService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ICommunityCategoryService, CommunityCategoryService>();
builder.Services.AddScoped<ICommunityStatusService, CommunityStatusService>();
builder.Services.AddScoped<IThreadStatusService, ThreadStatusService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddControllers();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDev",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngularDev");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
