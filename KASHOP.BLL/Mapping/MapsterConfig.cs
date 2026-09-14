using System.Diagnostics;
using System.Globalization;
using System.Transactions;
using KASHOP.DAL;
using Mapster;

namespace KASHOP.BLL;

public static class MapsterConfig
{
    public static void MapsterConfigRegister()
    {
        TypeAdapterConfig<Category, CategoryResponse>
            .NewConfig()
            .Map(destination => destination.UserId, source => source.CreatedById)
            .Map(destination => destination.User, source => source.CreatedBy.UserName)
            .Map(
                destination => destination.Name, 
                source => source.Translations
                    .Where(translation => translation.Language == CultureInfo.CurrentUICulture.Name)
                    .Select(translation => translation.Name)
                    .FirstOrDefault()
            );

        TypeAdapterConfig<Product, ProductResponse>
            .NewConfig()
            .Map(
                destination => destination.Name,
                source => source.Translations
                    .Where(translation => translation.Language == CultureInfo.CurrentUICulture.Name)
                    .Select(translation => translation.Name)
                    .FirstOrDefault()
            )
            .Map(
                destination => destination.Description,
                source => source.Translations
                    .Where(translation => translation.Language == CultureInfo.CurrentUICulture.Name)
                    .Select(translation => translation.Description)
                    .FirstOrDefault()
            )
            .Map(
                destination => destination.MainImage,
                source => $"/Uploads/{source.MainImage}"
            );
    }
}
