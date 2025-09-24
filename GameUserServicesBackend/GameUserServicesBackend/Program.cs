using BLL.Services;
using DAL.Context;
using DAL.DAO;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GameUserServicesBackend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<UserRepository>();
            builder.Services.AddScoped<UserServices>();
            builder.Services.AddScoped<UserDAO>();

            builder.Services.AddScoped<CategoryDetailsRepository>();
            builder.Services.AddScoped<CategoryDetailServices>();
            builder.Services.AddScoped<CateDAO>();

            builder.Services.AddDbContext<db_userservicesContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(9, 0)) // Thay bằng version MySQL bạn dùng
    )
);


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowUnity", policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyOrigin().AllowAnyHeader();
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowUnity");
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
