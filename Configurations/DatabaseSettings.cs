namespace API.Configurations;

public class DatabaseSettings
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
    public string UserCollectionName { get; set; }
    public string ReaderCollectionName { get; set; }
    public string BookCollectionName { get; set; }
    public string AuthorCollectionName { get; set; }
    public string NationalityCollectionName { get; set; }
    public string CategoryCollectionName { get; set; }
    public string PublisherCollectionName { get; set; }
    public string ReviewCollectionName { get; set; }
}