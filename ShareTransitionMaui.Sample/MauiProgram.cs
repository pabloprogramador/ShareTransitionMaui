using Microsoft.Extensions.Logging;

namespace ShareTransitionMaui.Sample;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if IOS
		Microsoft.Maui.Handlers.BorderHandler.Mapper.PrependToMapping("DON'T ANIMATE YOU FILTHY ANIMAL", (handler, view) =>
        {
            foreach (var layer in handler.PlatformView.Layer.Sublayers)
            {
                layer.RemoveAllAnimations();
            }
            handler.PlatformView.Layer.RemoveAllAnimations();

        });
#endif

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
