using FluentValidation;
using WebApplication1.Models.Dtos;
using WebApplication1.Models.Entities;
using WebApplication1.QueryParameters;
using WebApplication1.Repositories.Roll;
using WebApplication1.Repositories.Stats;

namespace WebApplication1.Endpoints
{
    public static class RollsEndpoints
    {

        public static void MapRollsEndpoints(this WebApplication app)
        {

            var group = app.MapGroup("/rolls");

            group.MapGet("/", async ([AsParameters] GetRollQuery query, IRollRepo repo, IValidator<GetRollQuery> validator) =>
            {
                var results = validator.Validate(query);
                if (!results.IsValid) { return Results.ValidationProblem(results.ToDictionary()); }

                var rolls = await repo.GetPagedAsync(query);
                return Results.Ok(rolls);
            });


            group.MapGet("/stats", async ([AsParameters] GetStatsQuery dto, IStatsRepo repo, IValidator<GetStatsQuery> validator) => {
                var results = validator.Validate(dto);
                if (!results.IsValid) { return Results.ValidationProblem(results.ToDictionary()); }

                var stats = await repo.GetStatsAsync(dto);
                
                if (stats is null) { throw new Exception("В этот период на складе не было ни одного рулона"); } 

                return Results.Ok(stats);
            });


            group.MapPost("/", async (IRollRepo repo, CreateDtoRoll dto, IValidator<CreateDtoRoll> validator) =>
            {
                var results = validator.Validate(dto);
                if (!results.IsValid) { return Results.ValidationProblem(results.ToDictionary()); }

                var roll = await repo.AddAsync(dto);

                return Results.Created($"/rolls/{roll.Id}", roll);
            });


            group.MapDelete("/{id}", async (IRollRepo repo, long id) =>
            {
                var roll = await repo.FindByIdAsync(id);

                if (roll is null) { throw new Exception("Рулон не найден"); }

                if (roll.RemovedDate.HasValue) { throw new Exception("Рулон уже удален"); }
                await repo.DeleteAsync(roll);

                return Results.Ok(roll);
            });
        }
    }
}
