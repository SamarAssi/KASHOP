namespace KASHOP.DAL;

public class CategoryResponse
{
    public int Id { get; set; }
    public List<CategoryTranslationResponse> Translations { get; set; }
}
