using DiscussionForum.Data;
using DiscussionForum.Services;
using DiscussionForum.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

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

app.UseAuthorization();

app.MapControllers();

app.Run();
