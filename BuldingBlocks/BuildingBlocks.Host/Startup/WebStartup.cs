using BuildingBlocks.Web.Filters;
using BuildingBlocks.Web.Startup.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace BuildingBlocks.Web.Startup
{
    public abstract class WebStartup
    {
        protected IConfiguration Configuration { get; }

        protected WebStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            ConfigureRoutingService(services);
            ConfigureCorsService(services);
            ConfigureSwaggerService(services);
        }

        public virtual void Configure(IApplicationBuilder app)
        {
        }

        #region Routing config

        protected void ConfigureRoutingService(IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddApiVersioning();
            services.AddVersionedApiExplorer();

            services.AddControllers(options =>
                {
                    options.Filters.Add<GlobalExceptionFilter>();
                    options.Filters.Add<ValidationFilter>();
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                });
        }

        #endregion

        #region Cors config

        protected void ConfigureCorsService(IServiceCollection services) => services.AddCors();

        protected virtual void ConfigureCors(IApplicationBuilder app) => app.UseCors(builder => builder
            .AllowAnyHeader()
            .SetIsOriginAllowed((host) => true)
            .AllowAnyMethod()
            .AllowAnyOrigin());

        #endregion

        #region Swagger config

        protected void ConfigureSwaggerService(IServiceCollection services)
        {
            var swaggerOpenApiOptions =
                Configuration.GetSection(SwaggerOpenApiOptions.Section).Get<SwaggerOpenApiOptions>();

            if (swaggerOpenApiOptions.UseSwagger)
            {
                var swaggerConfig = new OpenApiInfo
                {
                    Title = swaggerOpenApiOptions.Title,
                    Description = swaggerOpenApiOptions.Description
                };

                services.AddSwaggerGen(c =>
                {
                    if (swaggerOpenApiOptions.UseAuth)
                    {
                        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                        {
                            Description =
                                "JWT Authorization header using Bearer scheme. Example \"Authorization: Bearer {token}\"",
                            Name = "Authorization",
                            In = ParameterLocation.Header,
                            Type = SecuritySchemeType.ApiKey
                        });

                        c.AddSecurityRequirement(new OpenApiSecurityRequirement
                        {
                            {
                                new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = "Bearer"
                                    },
                                    Scheme = "oauth2",
                                    Name = "Bearer",
                                    In = ParameterLocation.Header
                                },

                                ArraySegment<string>.Empty
                            }
                        });
                    }

                    using var serviceProvider = services.BuildServiceProvider();
                    var provider = serviceProvider.GetRequiredService<IApiVersionDescriptionProvider>();

                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        swaggerConfig.Version = description.ApiVersion.ToString();
                        c.SwaggerDoc(description.GroupName, swaggerConfig);
                    }

                    c.DescribeAllParametersInCamelCase();
                    c.CustomSchemaIds(x => x.FullName);
                });
            }
        }

        protected virtual void ConfigureSwagger(IApplicationBuilder app)
        {
            var swaggerOpenApiOptions =
                Configuration.GetSection(SwaggerOpenApiOptions.Section).Get<SwaggerOpenApiOptions>();

            if (swaggerOpenApiOptions.UseSwagger)
            {
                var provider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", $"REST API v{description.ApiVersion}");
                    }
                });
            }
        }

        #endregion
    }
}