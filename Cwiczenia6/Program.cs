using System.Data.SqlClient;
using Cwiczenia6.DTOs;
using Cwiczenia6.Validators;
using FluentValidation;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddValidatorsFromAssemblyContaining<CreateAnimalRequestValidator>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

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
app.MapPost("animals", (CreateAnimalRequest request) =>
{
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
app.Run();
