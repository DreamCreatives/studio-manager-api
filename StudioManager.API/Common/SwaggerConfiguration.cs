using System.Diagnostics.CodeAnalysis;
using Asp.Versioning;

namespace StudioManager.API.Common;

[ExcludeFromCodeCoverage]
public static class SwaggerConfiguration
{
    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader(),
                    new HeaderApiVersionReader("x-api-version"));
            })
            .AddMvc()
            .AddApiExplorer(x =>
            {
                x.GroupNameFormat = "'v'VVV";
                x.SubstituteApiVersionInUrl = true;
            });

        services.AddSwaggerGen();

        services.ConfigureOptions<NamedSwaggerGenOptions>();
    }
}
