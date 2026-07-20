using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace KASHOP.PL;

public static class LocalizationExtension
{
    public static WebApplicationBuilder AddLocalization(
        this WebApplicationBuilder builder
    )
    {
        builder.Services.AddLocalization(options => options.ResourcesPath = "");

        const string defaultCulture = "en";
        var supportedCultures = new[]
        {
            new CultureInfo(defaultCulture),
            new CultureInfo("ar")
        };

        builder.Services.Configure<RequestLocalizationOptions>(options =>
        {
            options.DefaultRequestCulture = new RequestCulture(defaultCulture);
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
            options.RequestCultureProviders.Clear();
            options.RequestCultureProviders.Add(new AcceptLanguageHeaderRequestCultureProvider());
        });

        return builder;
    }
}
