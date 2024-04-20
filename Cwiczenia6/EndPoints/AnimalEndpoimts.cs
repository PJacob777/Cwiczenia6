using System.Data.SqlClient;
using Cwiczenia6.DTOs;
using FluentValidation;

namespace Cwiczenia6.EndPoints;

public static class AnimalEndpoimts
{
    public static void RegisterEndPointsAnimal(this WebApplication app)
    {
        app.MapGet("animals", (IConfiguration configuration) =>
            {
                var animals = new List<GetAnimalResponse>();
                using (var sqlConnection = new SqlConnection("Server=localhost,1433;Database=Animals;User Id=SA;Password=yourStrong(!)Password;")){
                    var sqlCommand = new SqlCommand("SELECT * FROM Animal", sqlConnection);
                    sqlCommand.Connection.Open();
                    var reader = sqlCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        animals.Add(new GetAnimalResponse(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            reader.GetString(3),
                            reader.GetString(4)
                            
                        ));
                    }
                }

                return Results.Ok(animals);
            });
        app.MapGet("animals/{id:int}", (IConfiguration configuration,int id) =>
            {
                using (var sqlConnection = new SqlConnection("Server=localhost,1433;Database=Animals;User Id=SA;Password=yourStrong(!)Password;")){
                    var sqlCommand = new SqlCommand("SELECT * FROM Animal WHERE ID=@id", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@id", id);
                    sqlCommand.Connection.Open();
                    var reader = sqlCommand.ExecuteReader();
                    if (!reader.Read()) return Results.NotFound();
                    
                        return Results.Ok(new GetAnimalResponse(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            reader.GetString(3),
                            reader.GetString(4)
                            
                        ));
                    
                }
            });
        app.MapPost("animals", (CreateAnimalRequest request, IValidator<CreateAnimalRequest> validator) =>
            {
                var validation = validator.Validate(request);
                if (!validation.IsValid) return Results.ValidationProblem(validation.ToDictionary());
                using (var sqlConnection = new SqlConnection("Server=localhost,1433;Database=Animals;User Id=SA;Password=yourStrong(!)Password;")){
                    var sqlCommand = new SqlCommand("INSERT INTO Animal (Name, Description,Category,Area) VALUES (@name,@desc,@cat,@area)", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@name", request.Name);
                    sqlCommand.Parameters.AddWithValue("@desc", request.Desc);
                    sqlCommand.Parameters.AddWithValue("@cat", request.Category);
                    sqlCommand.Parameters.AddWithValue("@area", request.Area);
                    sqlCommand.Connection.Open();
                    sqlCommand.ExecuteNonQuery();
                    return Results.Created("", null);
                }
            });
        app.MapDelete("animals/id{int}", (IConfiguration configuration,int id) =>
            {
                using (var sqlConnection =
                       new SqlConnection("Server=localhost,1433;Database=Animals;User Id=SA;Password=yourStrong(!)Password;"))
                {
                    var sqlCommand = new SqlCommand("DELETE FROM Animal WHERE ID=@id", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@id", id);
                    sqlCommand.Connection.Open();
                    sqlCommand.ExecuteNonQuery();
                    return Results.Ok();
                }
            });
        app.MapPut("animals/id{int}", (IConfiguration conf, int id,CreateAnimalRequest request, IValidator<CreateAnimalRequest> validator) =>
            {
                var validation = validator.Validate(request);
                if (!validation.IsValid) return Results.ValidationProblem(validation.ToDictionary());
                using (var sqlConnection = new SqlConnection("Server=localhost,1433;Database=Animals;User Id=SA;Password=yourStrong(!)Password;")){
                    var sqlCommand = new SqlCommand("UPDATE Animal SET Name=@name, Description = @desc, Category = @cat, Area = @area WHERE ID = @id", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@name", request.Name);
                    sqlCommand.Parameters.AddWithValue("@desc", request.Desc);
                    sqlCommand.Parameters.AddWithValue("@cat", request.Category);
                    sqlCommand.Parameters.AddWithValue("@area", request.Area);
                    sqlCommand.Parameters.AddWithValue("@id", id);
                    sqlCommand.Connection.Open();
                    sqlCommand.ExecuteNonQuery();
                    return Results.NoContent();
                }
                
            });
        app.MapGet("animals/orderBy{string}", (IConfiguration configuration,string orderBy) =>
            {
                var animals = new List<GetAnimalResponse>();
                
                using (var sqlConnection = new SqlConnection("Server=localhost,1433;Database=Animals;User Id=SA;Password=yourStrong(!)Password;")){
                    var sqlCommand = new SqlCommand("SELECT * FROM Animal ORDER BY Name", sqlConnection);
                    if(orderBy.Equals("Description"))
                         sqlCommand = new SqlCommand("SELECT * FROM Animal ORDER BY Description", sqlConnection);
                    if(orderBy.Equals("Area"))
                        sqlCommand = new SqlCommand("SELECT * FROM Animal ORDER BY Area", sqlConnection);
                    if(orderBy.Equals("Category"))
                        sqlCommand = new SqlCommand("SELECT * FROM Animal ORDER BY Category", sqlConnection);
                    sqlCommand.Connection.Open();
                    var reader = sqlCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        animals.Add(new GetAnimalResponse(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            reader.GetString(3),
                            reader.GetString(4)
                            
                        ));
                    }
                }

                return Results.Ok(animals);
            });
    }
}