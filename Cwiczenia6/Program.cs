using System.Data.SqlClient;
using Cwiczenia6.DTOs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
    using (var sqlConnection = new SqlConnection("Server:localhost,1433;Database=Animals;User Id=SA;Password=yourStrong(!)Password"))
    {
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

app.Run();
