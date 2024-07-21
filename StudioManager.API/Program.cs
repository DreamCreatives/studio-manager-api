using System.Diagnostics.CodeAnalysis;
using Asp.Versioning.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using StudioManager.API;
using StudioManager.API.Common;
using StudioManager.Application;
using StudioManager.Infrastructure;
using StudioManager.Infrastructure.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(option =>
{
    option.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyHeader()
            .AllowAnyMethod()
            .SetIsOriginAllowed(_ => true)
            .AllowCredentials();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger(builder.Configuration);

builder.Services.AddSwaggerGen();

builder.Services.AddHttpContextAccessor();

builder.Services.RegisterInfrastructure(builder.Configuration);
builder.Services.RegisterApplication(builder.Configuration);
builder.Services.RegisterApi(builder.Configuration);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// builder.Services.AddHostedService<FinishedReservationsBackgroundService>(); TODO: Implement redis cache for read lock + change logic to find reservations already returned

var app = builder.Build();
app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("CorsPolicy");

app.UseAuthorization();
app.UseAuthentication();

app.UseStaticFiles();
app.UseExceptionHandler();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    foreach (var description in provider.ApiVersionDescriptions)
        options.SwaggerEndpoint(
            $"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());

    var authOptions = app.Configuration.GetRequiredSection(KeyCloakConfiguration.SectionName).Get<KeyCloakConfiguration>();
    
    ArgumentNullException.ThrowIfNull(authOptions, nameof(authOptions));
    
    options.OAuthClientSecret(authOptions.Secret);
    options.OAuthClientId(authOptions.ClientId);
    options.OAuthUseBasicAuthenticationWithAccessCodeGrant();
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
