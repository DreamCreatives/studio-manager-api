using System.Diagnostics.CodeAnalysis;
using Asp.Versioning.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using StudioManager.API;
using StudioManager.Infrastructure;
using StudioManager.API.Common;
using StudioManager.Application;

var builder = WebApplication.CreateBuilder(args);

const string policyName = "DefaultPolicy";
builder.Services.AddCors(opt =>
{
    opt.AddPolicy(name: policyName, pol =>
    {
        pol.WithOrigins("http://127.0.0.1:4200")
            .SetIsOriginAllowedToAllowWildcardSubdomains();
        pol.WithOrigins("http://127.0.0.1:3000")
            .SetIsOriginAllowedToAllowWildcardSubdomains();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();

builder.Services.AddSwaggerGen();

builder.Services.RegisterInfrastructure(builder.Configuration);
builder.Services.RegisterApplication();
builder.Services.RegisterApi();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCors(policyName);

app.UseExceptionHandler();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    foreach (var description in provider.ApiVersionDescriptions)
    {
        options.SwaggerEndpoint(
            $"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant()); 
    } 
});

app.MapControllers();

await using (var scope = app.Services.CreateAsyncScope())
{
    var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<StudioManagerDbContext>>();
    await using var dbContext = await dbContextFactory.CreateDbContextAsync();
    await dbContext.Database.MigrateAsync();
}

await app.RunAsync();

[ExcludeFromCodeCoverage]
public partial class Program;