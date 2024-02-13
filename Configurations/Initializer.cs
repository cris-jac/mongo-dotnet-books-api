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
                new Book { Title = "Metamorphosis and Other Stories", Description = "Kafka's masterpiece of unease and black humour, Metamorphosis, the story of an ordinary man transformed into an insect, is brought together in this collection with the rest of his works that he thought worthy of publication. It includes Contemplation, a collection of his earlier short studies; The Judgement, written in a single night of frenzied creativity; The Stoker, the first chapter of a novel set in America; and an eyewitness account of an air display. Together, these stories, fragments and miniature gems reveal the breadth of his vision, his sense of the absurd, and above all his acute, uncanny wit.", PublicationDate = new DateTime(2020) },
                new Book { Title = "El Juguete Rabioso", Description = "Silvio Astier, el protagonista de esta novela, es inteligente, de opiniones agudas y culto, pero vive en un entorno social pobre y limitado. En su lucha por escapar de esa realidad, forma con sus amigos el Club de los Caballeros de la Media Noche, con los que se dedica a llevar a cabo pequefios hurtos, como su héroe de la ficción, Rocambole.", PublicationDate = new DateTime(2019) },
                new Book { Title = "Animal Farm", Description = "Mr Jones of Manor Farm is so lazy and drunken that one day he forgets to feed his livestock. The ensuing rebellion under the leadership of the pigs Napoleon and Snowball leads to the animals taking over the farm. Vowing to eliminate the terrible inequities of the farmyard, the renamed Animal Farm is organised to benefit all who walk on four legs. But as time passes, the ideals of the rebellion are corrupted, then forgotten. And something new and unexpected emerges...", PublicationDate = new DateTime(2008) }
            };

            booksCollection.InsertMany(initializeBooks);
        }

        // Authors
        var authorsCollection = _database.GetCollection<Author>(_settings.AuthorCollectionName);
        if (authorsCollection.CountDocuments(FilterDefinition<Author>.Empty) == 0)
        {
            var initializeAuthors = new List<Author>
            {
                new Author { Name = "Roberto Arlt", Bio = "Argentine writer known for his innovative and experimental works. His writing often explored themes of urban life, social inequality, and the human condition. Arlt's works are characterized by their raw, gritty realism and their portrayal of marginalized characters struggling to survive in the harsh realities of modern society.", NationalityId = ObjectId.Empty },
                new Author { Name = "Franz Kafka", Bio = "German-speaking Bohemian, today Czech Republic, writer known for his surreal and existential works. Kafka's works are characterized by their dream-like quality, labyrinthine narratives, and exploration of the human psyche. Some of his most famous works include 'The Metamorphosis','The Trial', and 'The Castle', which have had a profound influence on modern literature and philosophy.", NationalityId = ObjectId.Empty },
                new Author { Name = "George Orwell", Bio = "English writer known for his politically charged and dystopian works. His writing often critiqued totalitarianism, imperialism, and social injustice, drawing from his own experiences and observations. Some of his most famous works include 'Animal Farm' and 'Nineteen Eighty-Four', which have become seminal texts in the genre of dystopian literature and have had a lasting impact on popular culture.", NationalityId = ObjectId.Empty }
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
                new Nationality { Name = "Great Britain" },
                new Nationality { Name = "Czech Republic" }
            };

            nationalitiesCollection.InsertMany(initializeNationalities);
        }
    }
}