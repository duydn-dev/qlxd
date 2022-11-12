using Api.Installer;
using BusinessLogic.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

var builder = WebApplication.CreateBuilder(args);
Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();
builder.Logging.AddSerilog();
try
{
    string corsKey = "cors";
    builder.Services.AddMemoryCache();
    builder.Services.AddDistributedMemoryCache();
    builder.Services.AddResponseCaching();
    string[] origins = builder.Configuration["origins:domains"].Split(",");
    builder.Services.Installer(builder.Configuration);
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: corsKey,
                          builder =>
                          {
                              builder
                              .WithOrigins(origins)
                              .AllowAnyHeader()
                              .AllowAnyMethod()
                              .AllowCredentials();
                          });
    });
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api", Version = "v1" });
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "Nhập Token vào input. ví dụ: Bearer ey5dffg7...",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement()
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
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
        }
                });

    });
    builder.Services.AddMvc(options =>
    {
        // giữ cái chữ Async ở tên Action
        options.SuppressAsyncSuffixInActionNames = false;
    });
    builder.Services.AddSignalR();
    builder.Services.AddControllers().AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
    );



    var app = builder.Build();
    app.CreateConfig();
    if (string.IsNullOrWhiteSpace(app.Environment.WebRootPath))
    {
        app.Environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
    }

    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api v1"));
    }
    if (app.Environment.IsProduction())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.RoutePrefix = "";
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api v1");
        });
        app.UseRewriter(new RewriteOptions().AddRedirectToHttps(StatusCodes.Status301MovedPermanently, 443));
    }
    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();
    app.UseCors(corsKey);
    app.UseAuthentication();
    app.UseResponseCaching();
    app.UseAuthorization();

    app.MapControllers();
    app.MapHub<AppHub>("/apphub");

    app.Run();

}
catch (System.Exception ex)
{
    Log.Error(ex.ToString());
}
