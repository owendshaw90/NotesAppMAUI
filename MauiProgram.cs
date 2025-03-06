using Microsoft.Extensions.Logging;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Notes.Data;
using Notes.ViewModels;
using Notes.Views;
using System.Linq.Expressions;

namespace Notes;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder.Logging.AddDebug();

		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		var a = Assembly.GetExecutingAssembly();
		using var stream = a.GetManifestResourceStream("Notes.appsettings.json");

		try
		{
			if (stream != null)
			{
				var config = new ConfigurationBuilder()
					.AddJsonStream(stream)
					.Build();

				var connectionString = config.GetConnectionString("DevelopmentConnection");

				if (!connectionString.Contains("TrustServerCertificate="))
				{
					connectionString += ";TrustServerCertificate=true";
				}

				builder.Services.AddDbContext<NotesDbContext>(options => options.UseSqlServer(connectionString));

				Console.WriteLine(config.GetConnectionString("DevelopmentConnection"));
			}
			else
			{
				Console.WriteLine("Error: appsettings.json not found!");
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Exception: {ex.Message}");
			Console.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
		}



		builder.Services.AddSingleton<AllNotesViewModel>();
		builder.Services.AddTransient<NoteViewModel>();

		builder.Services.AddSingleton<AllNotesPage>();
		builder.Services.AddTransient<NotePage>();


		// builder.Configuration.AddConfiguration(config);




#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
