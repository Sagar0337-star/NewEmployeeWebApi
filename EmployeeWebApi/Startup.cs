using Employee.Domain.Employee;
using Employee.EF;
using Employee.EF.Repositories;
using EmployeeWebAPI.AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        // Add DbContext service with connection string
        services.AddDbContext<EmployeeDBContext>(options =>
        {
            options.UseSqlServer(_configuration.GetConnectionString("EmployeeDB"));
        });

        //Add scoped services for repositories

services.AddScoped<IEmployeeService, EmployeeService>();
       services.AddScoped<IEmployeeRepository, EmployeeRepository>();

        // Add AutoMapper
        services.AddAutoMapper(typeof(MapperProfile));

        // Add authentication with JWT Bearer
        services.AddAuthentication(options =>
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
                ValidIssuer = _configuration["JwtSettings:Issuer"],
                ValidAudience = _configuration["JwtSettings:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]))
            };
        });

        //Add Swagger and configure JWT Bearer for Swagger

       services.AddSwaggerGen(options =>
       {
           options.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

           // Configure Swagger to use the JWT Bearer token
           options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
           {
               Name = "Authorization",
               Type = SecuritySchemeType.ApiKey,
               Scheme = "Bearer",
               BearerFormat = "JWT",
               In = ParameterLocation.Header,
               Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
           });

           options.AddSecurityRequirement(new OpenApiSecurityRequirement
           {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
           });
       });

       // Add controllers
       services.AddControllers();
        services.AddEndpointsApiExplorer();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API v1"));
        }

        app.UseHttpsRedirection();

        // Order is important: UseRouting before authentication/authorization
        app.UseRouting();

        // Authentication & Authorization middleware must be before UseEndpoints
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }

}
