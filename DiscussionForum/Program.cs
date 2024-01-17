using DiscussionForum.Data;
using DiscussionForum.Services;
using DiscussionForum.UnitOfWork;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DiscussionForum_API")));

// Add services to the container.

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IForumCategoryService, ForumCategoryService>();
builder.Services.AddScoped<IForumStatusService, ForumStatusService>();
builder.Services.AddScoped<IThreadStatusService, ThreadStatusService>();

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
