using System.Diagnostics;
using System.Globalization;
using KASHOP.DAL;
using Mapster;

namespace KASHOP.BLL;

public static class MapsterConfig
{
    public static void MapsterConfigRegister()
    {
        TypeAdapterConfig<Category, CategoryResponse>
            .NewConfig()
            .Map(dest => dest.UserId, src => src.CreatedById)
            .Map(dest => dest.User, src => src.CreatedBy.UserName)
            .Map(
                dest => dest.Name, 
                src => src.Translations
                    .Where(t => t.Language == CultureInfo.CurrentUICulture.Name)
                    .Select(t => t.Name)
                    .FirstOrDefault()
            );
    }
}
