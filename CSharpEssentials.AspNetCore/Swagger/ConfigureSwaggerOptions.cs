using System;
using Asp.Versioning.ApiExplorer;
using CSharpEssentials.AspNetCore.Swagger.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CSharpEssentials.AspNetCore;
public sealed class DefaultConfigureSwaggerOptions(
           IServiceProvider serviceProvider,
           IHostEnvironment environment,
           IConfiguration configuration)
           : ConfigureSwaggerOptions(serviceProvider, environment, configuration);

public abstract class ConfigureSwaggerOptions(
            IServiceProvider serviceProvider,
           IHostEnvironment environment,
           IConfiguration configuration)
           : IConfigureNamedOptions<SwaggerGenOptions>
{
    public virtual void Configure(SwaggerGenOptions options)
    {
        var title = configuration["Swagger:Title"] ?? "API";
        var description = configuration["Swagger:Description"] ?? "API Description";
        var license = configuration["Swagger:License"] ?? "API License";
        var licenseUrl = configuration["Swagger:LicenseUrl"] ?? "https://opensource.org/license/mit";
        var swaggerLicense = new OpenApiLicense
        {
            Name = license,
            Url = new Uri(licenseUrl)
        };
        options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
        var provider = serviceProvider.GetService<IApiVersionDescriptionProvider>();
        if (provider == null)
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = title,
                Version = "v1",
                Description = description,
                License = swaggerLicense
            });
        }
        else
        {
            foreach (var apiVersionDescription in provider.ApiVersionDescriptions)
                options.SwaggerDoc(apiVersionDescription.GroupName, new OpenApiInfo
                {
                    Title = title,
                    Version = apiVersionDescription.GroupName,
                    Description = CreateDescription(description, apiVersionDescription, environment),
                    License = swaggerLicense
                });
        }
      

        options.OperationFilter<ReApplyOptionalRouteParameterOperationFilter>();

        var timeSchema = new OpenApiSchema
        {
            Type = "string",
            Example = new OpenApiString("00:00:00")
        };

        options.MapType<TimeSpan>(() => timeSchema);
        options.MapType<TimeOnly>(() => timeSchema);
    }

    public virtual void Configure(string? name, SwaggerGenOptions options)
    {
        Configure(options);
    }

    private static string CreateDescription(string? description, ApiVersionDescription apiVersionDescription,
        IHostEnvironment environment)
    {
        var env = environment.EnvironmentName;
        var version = apiVersionDescription.ApiVersion.ToString();
        description ??= string.Empty;
        description = $"[{env}] {version} {description}";
        return apiVersionDescription.IsDeprecated
            ? $"{description} This API version has been deprecated"
            : description;
    }
}