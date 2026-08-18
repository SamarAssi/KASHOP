namespace KASHOP.DAL;

public class Category : AuditLog
{
    public int Id { get; set; }
    public List<CategoryTranslation> Translations { get; set; }
}
