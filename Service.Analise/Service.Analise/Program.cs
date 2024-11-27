using Microsoft.Extensions.Configuration;
using Service.Analise;
using Service.Analise.Repository;
using Service.Analise.Repository.Contracts;
using Service.Analise.Service;
using Service.Analise.Service.Contracts;
using System.Data;
using System.Data.SqlClient;

var builder = Host.CreateApplicationBuilder(args);
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

builder.Services.AddHostedService<Worker>();

//var stringConexao = configuration.GetSection("ConnectionStrings");
var valor = configuration.GetValue<string>("ConnectionStrings:DefaultConnection");

builder.Services.AddTransient<IDbConnection>((conexao) => new SqlConnection(valor));

builder.Services.AddSingleton<IAnalyzeSentimentService, AnalyzeSentimentService>();
builder.Services.AddSingleton<IMessageBusService, MessageBusService>();
builder.Services.AddSingleton<IAnalyzeRepository, AnalyzeRepository>();



var host = builder.Build();
host.Run();
