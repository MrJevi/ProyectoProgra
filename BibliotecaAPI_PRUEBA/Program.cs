/* PARTE POR DEFECTO

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Conexion del Gestor para que la API 
builder.Services.AddSingleton<BibliotecaAPI.DataStructures.Services.GestorBiblioteca>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

*/



//PARTE CON SWAGGER

var builder = WebApplication.CreateBuilder(args);

// Servicios
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();  //ESTO NO VIEN POR DEFECTO, ES PARA SWAGGER
builder.Services.AddSwaggerGen(); //ESTO NO VIENE POR DEFECTO, ES PARA SWAGGER


//  CORS agregado FUNCIONA PARA LA SUBIDA DE ARCHIVOS DESDE EL FRONTEND, PERO TAMBIÉN PARA CUALQUIER PETICIÓN DESDE OTRO ORIGEN (FRONTEND, POSTMAN, ETC.)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});


var app = builder.Build();

// Swagger SIEMPRE activo
app.UseSwagger();  //ESTO NO VIENE POR DEFECTO, ES PARA SWAGGER
app.UseSwaggerUI(); //ESTO NO VIENE POR DEFECTO, ES PARA SWAGGER

app.UseHttpsRedirection();

// IMPORTANTE: Para la subida de archivos desde el frontend, es necesario permitir CORS (Cross-Origin Resource Sharing) para que el navegador permita las solicitudes desde un origen diferente al del backend. En este caso, se ha configurado una política de CORS que permite cualquier origen, método y encabezado. Esto es útil durante el desarrollo, pero en producción se recomienda restringir los orígenes permitidos por razones de seguridad.
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();

