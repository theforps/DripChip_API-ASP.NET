using DripChip_API.DAL;
using DripChip_API.DAL.Interfaces;
using DripChip_API.DAL.Repositories;
using DripChip_API.Domain.Models;
using DripChip_API.Service.Implementations;
using DripChip_API.Service.Interfaces;
using DripChip_API.Service.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Drip_chip_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlite("Name=DefaultConnection"));
            
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAutoMapper(typeof(UserMapping), typeof(AnimalMapping));
            
            builder.Services.AddScoped<IAccountRepository, AccountRepository>();
            builder.Services.AddScoped<IAnimalRepository, AnimalRepository>();
            builder.Services.AddScoped<IRegisterRepository, RegisterRepository>();
            
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IAnimalService, AnimalService>();
            builder.Services.AddScoped<IRegisterService, RegisterService>();
            
            
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}