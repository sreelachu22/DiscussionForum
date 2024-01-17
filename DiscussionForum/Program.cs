using DiscussionForum.Services;
//using DiscussionForum.UnitOfWork.ThreadStatusService.Infrastructure.UnitOfWork;
using DiscussionForum.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using DiscussionForum.Data;
using DiscussionForum.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DiscussionForum_API")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IThreadStatusRepository, ThreadStatusRepository>();
builder.Services.AddScoped<IThreadStatusService, ThreadStatusService>();


// Add services to the container.

builder.Services.AddControllers();
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
