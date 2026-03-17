using Microsoft.EntityFrameworkCore;
using backend.Data;
using ExpertsEncryption.Sdk.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Base de Datos y Repositorio
builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("TaskManagerDb"));
builder.Services.AddScoped<backend.Repositories.ITaskTodo, backend.Repositories.TaskTodo>();

// 2. Configuración Estándar de API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// --- 3. CONFIGURACIÓN DEL SDK DE SEGURIDAD (CORREGIDO) ---

// Registrar el proveedor de llaves local
builder.Services.AddScoped<IKmsKeyProvider, LocalKmsKeyProvider>();

// Registrar el servicio de máscaras (¡IMPORTANTE! No lo tenías registrado)
builder.Services.AddScoped<PiiMaskingService>();

// Registrar el servicio principal (UNA SOLA LÍNEA)
// .NET automáticamente inyectará el KeyProvider y el MaskingService aquí.
builder.Services.AddScoped<IPiiEncryptionService>(provider => 
{
    var keyProvider = provider.GetRequiredService<IKmsKeyProvider>();
    var maskingService = provider.GetRequiredService<PiiMaskingService>();
    
    // Aquí le pasamos los 3 argumentos que pide tu constructor:
    // 1. El Provider, 2. El Masking, 3. Un String (tu ID de llave)
    return new PiiEncryptionService(keyProvider, maskingService, "dev-local-key-id");
});

// --- FIN CONFIGURACIÓN SDK ---

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors("AllowAngularApp");
app.UseAuthorization();
app.MapControllers();

app.Run();