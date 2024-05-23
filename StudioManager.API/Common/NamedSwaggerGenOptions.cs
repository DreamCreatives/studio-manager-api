using System.Diagnostics.CodeAnalysis;
using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace StudioManager.API.Common;

[ExcludeFromCodeCoverage]
public sealed class NamedSwaggerGenOptions(
    IApiVersionDescriptionProvider provider)
    : IConfigureNamedOptions<SwaggerGenOptions>
{
    public void Configure(SwaggerGenOptions options)
    {
        // add swagger document for every API version discovered
        foreach (var description in provider.ApiVersionDescriptions)
            options.SwaggerDoc(
                description.GroupName,
                CreateVersionInfo(description));
    }

    public void Configure(string? name, SwaggerGenOptions options)
    {
        Configure(options);
    }

    private static OpenApiInfo CreateVersionInfo(
        ApiVersionDescription description)
    {
        var info = new OpenApiInfo
        {
            Title = "Studio Manager API " + description.GroupName,
            Version = description.ApiVersion.ToString()
        };
        if (description.IsDeprecated) info.Description += " This API version has been deprecated.";
        return info;
    }
}
