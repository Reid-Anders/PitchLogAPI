using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using PitchLogAPI.Services;
using PitchLogData;
using Marvin.Cache.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(configure =>
{
    configure.ReturnHttpNotAcceptable = true;
})
.AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
})
.ConfigureApiBehaviorOptions(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var problemDetailsFactory = context.HttpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();

        var validationProblemDetails = problemDetailsFactory.CreateValidationProblemDetails(context.HttpContext, context.ModelState);

        validationProblemDetails.Detail = "See the errors field for details.";
        validationProblemDetails.Instance = context.HttpContext.Request.Path;
        validationProblemDetails.Status = StatusCodes.Status422UnprocessableEntity;
        validationProblemDetails.Title = "One or more validation errors occurred.";

        return new UnprocessableEntityObjectResult(validationProblemDetails)
        {
            ContentTypes = { "application/problem+json" }
        };
    };
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IGradesRepository, GradesRepository>();
builder.Services.AddScoped<IAreasRepository, AreasRepository>();
builder.Services.AddScoped<ISectorsRepository, SectorsRepository>();

builder.Services.AddDbContext<PitchLogContext>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddResponseCaching();

builder.Services.AddHttpCacheHeaders(expirationOptions =>
{
    expirationOptions.MaxAge = 30;
    expirationOptions.CacheLocation = CacheLocation.Private;
}, validationOptions =>
{
    validationOptions.MustRevalidate = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseResponseCaching();

app.UseHttpCacheHeaders();

app.MapControllers();

app.Run();
