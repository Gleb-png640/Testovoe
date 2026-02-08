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

            group.MapGet("/", ([AsParameters] GetRollQuery query, IRollRepo repo, IValidator<GetRollQuery> validator) =>
            {
                var results = validator.Validate(query);
                if (!results.IsValid) { return Results.ValidationProblem(results.ToDictionary()); }

                return Results.Ok(repo.GetPaged(query));
            });


            group.MapGet("/stats", ([AsParameters] GetStatsQuery dto, IStatsRepo repo, IValidator<GetStatsQuery> validator) => {
                var results = validator.Validate(dto);
                if (!results.IsValid) { return Results.ValidationProblem(results.ToDictionary()); }

                RollStatsDto? stats = repo.GetStats(dto);
                
                if (stats is null) { throw new Exception("В этот период на складе не было ни одного рулона"); } 

                return Results.Ok(stats);
            });


            group.MapPost("/", (IRollRepo repo, CreateDtoRoll dto, IValidator<CreateDtoRoll> validator) =>
            {
                var results = validator.Validate(dto);
                if (!results.IsValid) { return Results.ValidationProblem(results.ToDictionary()); }

                EntityRoll roll = repo.Add(dto);

                return Results.Created($"/rolls/{roll.Id}", roll);
            });


            group.MapDelete("/{id}", (IRollRepo repo, long id) =>
            {
                var roll = repo.FindById(id);

                if (roll is null) { throw new Exception("Рулон не найден"); }

                if (roll.RemovedDate.HasValue) { throw new Exception("Рулон уже удален"); }
                repo.Delete(roll);

                return Results.Ok(roll);
            });
        }
    }
}
