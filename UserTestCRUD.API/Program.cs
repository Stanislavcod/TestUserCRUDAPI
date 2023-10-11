using UserTestCRUD.API.Configuration;

var builder = WebApplication.CreateBuilder(args);

ConfigurationService.ConfigureService(builder.Services, builder.Configuration);

var app = builder.Build();

ConfigurationService.Configure(app);

app.Run();
