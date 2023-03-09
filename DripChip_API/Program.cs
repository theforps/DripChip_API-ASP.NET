using DripChip_API.DAL;
using DripChip_API.DAL.Interfaces;
using DripChip_API.DAL.Repositories;
using DripChip_API.Service.Handlers;
using DripChip_API.Service.Implementations;
using DripChip_API.Service.Interfaces;
using DripChip_API.Service.Mapping;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace Drip_chip_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
            builder.Services.AddAuthorization();
            
            builder.Services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlite("Name=DefaultConnection"));
            
            builder.Services.AddEndpointsApiExplorer();
            
            builder.Services.AddSwaggerGen(c =>  
            {
                c.AddSecurityDefinition("basic", new OpenApiSecurityScheme  
                {  
                    Name = "Authorization",  
                    Type = SecuritySchemeType.Http,  
                    Scheme = "basic",  
                    In = ParameterLocation.Header,  
                    Description = "Enter the login and password for authorization."  
                });  
                c.AddSecurityRequirement(new OpenApiSecurityRequirement  
                {  
                    {  
                        new OpenApiSecurityScheme  
                        {  
                            Reference = new OpenApiReference  
                            {  
                                Type = ReferenceType.SecurityScheme,  
                                Id = "basic"  
                            }  
                        },  
                        new string[] {}  
                    }  
                });  
            });  

            builder.Services.AddAutoMapper(
                typeof(UserMapping), 
                typeof(AnimalMapping), 
                typeof(LocationMapping),
                typeof(TypeMapping)
                );
            
            builder.Services.AddScoped<IAccountRepository, AccountRepository>();
            builder.Services.AddScoped<IAnimalRepository, AnimalRepository>();
            builder.Services.AddScoped<IRegisterRepository, RegisterRepository>();
            builder.Services.AddScoped<ILocationRepository, LocationRepository>();
            builder.Services.AddScoped<ITypeRepository, TypeRepository>();
            builder.Services.AddScoped<ILocationInfoRepository, LocationInfoRepository>();
            
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IAnimalService, AnimalService>();
            builder.Services.AddScoped<IRegisterService, RegisterService>();
            builder.Services.AddScoped<ILocationService, LocationService>();
            builder.Services.AddScoped<ITypeService, TypeService>();
            builder.Services.AddScoped<ILocationInfoService, LocationInfoService>();
            
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}