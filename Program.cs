
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Endpoints;
using WebApplication1.Middlewares;
using WebApplication1.Models.Dtos;
using WebApplication1.QueryParameters;
using WebApplication1.Repositories.Roll;
using WebApplication1.Repositories.Stats;
using WebApplication1.Validation.Roll;

namespace WebApplication1
{
    public class Program {
        public static void Main(string[] args) 
        {

            var builder = WebApplication.CreateBuilder(args);


            var configuration = builder.Configuration;

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IRollRepo, RollRepo>();
            builder.Services.AddScoped<IStatsRepo, StatsRepo>();

            builder.Services.AddScoped<IValidator<GetRollQuery>, GetQueryValidator>();
            builder.Services.AddScoped<IValidator<CreateDtoRoll>, CreateValidator>();
            builder.Services.AddScoped<IValidator<GetStatsQuery>, GetStatsValidator>();

            builder.Services.AddDbContext<SeverstalDbContext>(options => 
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            });

            var app = builder.Build();

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment()) {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.MapRollsEndpoints();

            app.Run();
        }
    }
}
