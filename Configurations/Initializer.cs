using API.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace API.Configurations;

public class Initializer
{
    private readonly IMongoDatabase _database;
    private readonly DatabaseSettings _settings;

    public Initializer(
        IMongoDatabase database,
        IOptions<DatabaseSettings> settings
    )
    {
        _database = database;
        _settings = settings.Value;
    }

    public void InitializeCollections()
    {
        if (string.IsNullOrEmpty(_settings.BookCollectionName))
        {
            throw new InvalidOperationException("Book collection name is not specified.");
        }

        CreateCollectionIfNotExists(_settings.BookCollectionName);
        CreateCollectionIfNotExists(_settings.UserCollectionName);
        CreateCollectionIfNotExists(_settings.ReaderCollectionName);
        CreateCollectionIfNotExists(_settings.AuthorCollectionName);
        CreateCollectionIfNotExists(_settings.NationalityCollectionName);
        CreateCollectionIfNotExists(_settings.CategoryCollectionName);
        CreateCollectionIfNotExists(_settings.PublisherCollectionName);
        CreateCollectionIfNotExists(_settings.ReviewCollectionName);

        SeedData();
    }

    private void CreateCollectionIfNotExists(string collectionName)
    {
        if (string.IsNullOrEmpty(collectionName))
        {
            throw new ArgumentNullException(nameof(collectionName), "Collection name cannot be null or empty.");
        }

        var collections = _database.ListCollectionNames().ToList();
        if (!collections.Contains(collectionName))
        {
            _database.CreateCollection(collectionName);
        }
    }

    private void SeedData()
    {
        // Books
        var booksCollection = _database.GetCollection<Book>(_settings.BookCollectionName);
        if (booksCollection.CountDocuments(FilterDefinition<Book>.Empty) == 0)
        {
            var initializeBooks = new List<Book>
            {
                new Book { Title = "Metamorphosis and Other Stories", Description = "Kafka's masterpiece of unease and black humour, Metamorphosis, the story of an ordinary man transformed into an insect, is brought together in this collection with the rest of his works that he thought worthy of publication. It includes Contemplation, a collection of his earlier short studies; The Judgement, written in a single night of frenzied creativity; The Stoker, the first chapter of a novel set in America; and an eyewitness account of an air display. Together, these stories, fragments and miniature gems reveal the breadth of his vision, his sense of the absurd, and above all his acute, uncanny wit.", PublicationDate = new DateTime(2020), AuthorIds = new List<ObjectId>(), CategoryIds = new List<ObjectId>(), PublisherIds = new List<ObjectId>() },
                new Book { Title = "El Juguete Rabioso", Description = "Silvio Astier, el protagonista de esta novela, es inteligente, de opiniones agudas y culto, pero vive en un entorno social pobre y limitado. En su lucha por escapar de esa realidad, forma con sus amigos el Club de los Caballeros de la Media Noche, con los que se dedica a llevar a cabo pequefios hurtos, como su héroe de la ficción, Rocambole.", PublicationDate = new DateTime(2019), AuthorIds = new List<ObjectId>(), CategoryIds = new List<ObjectId>(), PublisherIds = new List<ObjectId>() },
                new Book { Title = "Animal Farm", Description = "Mr Jones of Manor Farm is so lazy and drunken that one day he forgets to feed his livestock. The ensuing rebellion under the leadership of the pigs Napoleon and Snowball leads to the animals taking over the farm. Vowing to eliminate the terrible inequities of the farmyard, the renamed Animal Farm is organised to benefit all who walk on four legs. But as time passes, the ideals of the rebellion are corrupted, then forgotten. And something new and unexpected emerges...", PublicationDate = new DateTime(2008), AuthorIds = new List<ObjectId>(), CategoryIds = new List<ObjectId>(), PublisherIds = new List<ObjectId>() }
            };

            booksCollection.InsertMany(initializeBooks);
        }

        // Authors
        var authorsCollection = _database.GetCollection<Author>(_settings.AuthorCollectionName);
        if (authorsCollection.CountDocuments(FilterDefinition<Author>.Empty) == 0)
        {
            var initializeAuthors = new List<Author>
            {
                new Author { Name = "Roberto Arlt", Bio = "Escritor argentino conocido por sus obras innovadoras y experimentales. Sus escritos a menudo exploraban temas de la vida urbana, la desigualdad social y la condición humana. Las obras de Arlt se caracterizan por su realismo crudo y descarnado y su retrato de personajes marginados que luchan por sobrevivir en las duras realidades de la sociedad moderna.", NationalityId = ObjectId.Empty },
                new Author { Name = "Franz Kafka", Bio = "Escritor bohemio conocido por sus obras surrealistas y existenciales. Las obras de Kafka se caracterizan por su calidad onírica, narrativas laberínticas y exploración de la psique humana. Algunas de sus obras más famosas incluyen 'La Metamorfosis', 'El Proceso' y 'El Castillo', que han tenido una profunda influencia en la literatura y la filosofía modernas.", NationalityId = ObjectId.Empty },
                new Author { Name = "George Orwell", Bio = "Escritor inglés conocido por sus obras distópicas y con carga política. Sus escritos a menudo criticaban el totalitarismo, el imperialismo y la injusticia social, basándose en sus propias experiencias y observaciones. Algunas de sus obras más famosas incluyen 'Animal Farm' y '1984', que se han convertido en textos fundamentales en el género de la literatura distópica y han tenido un impacto duradero en la cultura popular.", NationalityId = ObjectId.Empty }
            };

            authorsCollection.InsertMany(initializeAuthors);
        }

        // Nationalities
        var nationalitiesCollection = _database.GetCollection<Nationality>(_settings.NationalityCollectionName);
        if (nationalitiesCollection.CountDocuments(FilterDefinition<Nationality>.Empty) == 0)
        {
            var initializeNationalities = new List<Nationality>
            {
                new Nationality { Name = "Argentina" },
                new Nationality { Name = "Gran Bretaña" },
                new Nationality { Name = "República Checa" }
            };

            nationalitiesCollection.InsertMany(initializeNationalities);
        }

        // Publishers
        var publishersCollection = _database.GetCollection<Publisher>(_settings.PublisherCollectionName);
        if (publishersCollection.CountDocuments(FilterDefinition<Publisher>.Empty) == 0)
        {
            var initializePublishers = new List<Publisher>
            {
                new Publisher { Name = "Penguin" },
                new Publisher { Name = "Planeta" },
                new Publisher { Name = "Alfaguara" },
            };

            publishersCollection.InsertMany(initializePublishers);
        }

        // Categories
        var categoriesCollection = _database.GetCollection<Category>(_settings.CategoryCollectionName);
        if (categoriesCollection.CountDocuments(FilterDefinition<Category>.Empty) == 0)
        {
            var initializeCategories = new List<Category>
            {
                new Category { Name = "Alegoría" },
                new Category { Name = "Sátira" },
                new Category { Name = "Política" },
                new Category { Name = "Psicológica" },
            };

            categoriesCollection.InsertMany(initializeCategories);
        }

        // Readers
        var readersCollection = _database.GetCollection<Reader>(_settings.ReaderCollectionName);
        if (readersCollection.CountDocuments(FilterDefinition<Reader>.Empty) == 0)
        {
            var initializeReaders = new List<Reader>
            {
                new Reader { Name = "Pepe Argento", Username = "pepe123", BookIds = new List<ObjectId>(), NationalityId = ObjectId.Empty },
                new Reader { Name = "Mario Santos", Username = "santos123", BookIds = new List<ObjectId>(), NationalityId = ObjectId.Empty },
                new Reader { Name = "Fatiga Argento", Username = "fatiga666", BookIds = new List<ObjectId>(), NationalityId = ObjectId.Empty }
            };

            readersCollection.InsertMany(initializeReaders);
        }
    }
}