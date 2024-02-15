# Books API
### .NET API with MongoDB as Database




## Requirements
* .NET SDK 8.0+
* MongoDb



## Extra Resources
UML class diagram available in /ExtraResources/UmlDiagram.png



## Installation
1. Clone from github, using this command:
```git clone https://github.com/cris-jac/booksApi_netMongo.git```

2. Navigate to the repository folder:
```cd booksApi_netMongo```

3. Build the app:
```dotnet build```

4. Run the app:
```dotnet run```



## Port
By default, the port used in this app is:
http://localhost:5238/swagger/index.html



## API endpoints
### Auth
* GET /api/Auth/users: Retrieves all users.
* POST /api/Auth/register: Register a new user
* POST /api/Auth/login: Log in an existing user

### Authors
* GET /api/Authors: Retrieves all authors from the database.
* POST /api/Authors: Creates a new author.
* DELETE /api/Authors: Deletes an existing author.
* PUT /api/Authors: Updates an existing author's information.
* GET /api/Authors/{id}: Retrieves an author by their unique ID.
* GET /api/Authors/GetByCountry: Retrieves all authors based on their country or nationality.
* PUT /api/Authors/AddNationalityId: Updates an author's nationality.

### Books
* GET /api/Books: Retrieves all books from the database.
* POST /api/Books: Creates a new book.
* DELETE /api/Books: Deletes an existing book.
* GET /api/Books/{id}: Retrieves a book by its unique ID.
* GET /api/Books/GetByAuthor: Retrieves all books written by a specific author.
* GET /api/Books/GetByCategory: Retrieves all books based on their category.
* GET /api/Books/GetByPublisher: Retrieves all books published by a specific publisher.
* PUT /api/Books/AddAuthorToBook: Adds an author to the list of authors of a book.
* PUT /api/Books/RemoveAuthorFromBook: Removes an author from the list of authors of a book.
* PUT /api/Books/AddCategoryToBook: Adds a category to the list of categories of a book.
* PUT /api/Books/RemoveCategoryFromBook: Removes a category from the list of categories of a book.
* PUT /api/Books/AddPublisherToBook: Adds a publisher to the list of publishers of a book.
* PUT /api/Books/RemovePublisherFromBook: Removes a publisher from the list of publishers of a book.

### Categories
* GET /api/Categories: Retrieves all categories from the database.
* POST /api/Categories: Creates a new category.
* DELETE /api/Categories: Deletes an existing category.
* GET /api/Categories/{id}: Retrieves a category by its unique ID.

### Nationalities
* GET /api/Nationalities: Retrieves all nationalities from the database.
* POST /api/Nationalities: Creates a new nationality.
* DELETE /api/Nationalities: Deletes an existing nationality.
* GET /api/Nationalities/{id}: Retrieves a nationality by its unique ID.
* GET /api/Nationalities/GetByName: Retrieves a nationality by its name.

#### Publishers
* GET /api/Publishers: Retrieves all publishers from the database.
* POST /api/Publishers: Creates a new publisher.
* DELETE /api/Publishers: Deletes an existing publisher.

### Readers
* GET /api/Readers: Retrieves all readers from the database.
* POST /api/Readers: Creates a new reader.
* GET /api/Readers/{id}: Retrieves a reader by their unique ID.
* GET /api/Readers/GetReaderBooks: Retrieves all books associated with a specific reader.
* PUT /api/Readers/AddBookToReader: Adds a book to the list of books read by a reader.
* PUT /api/Readers/RemoveBookFromReader: Removes a book from the list of books read by a reader.
* PUT /api/Readers/UpdateNationality: Updates a reader's nationality.

### Reviews
* GET /api/Reviews: Retrieves all reviews from the database.
* POST /api/Reviews: Creates a new review.
* DELETE /api/Reviews: Deletes an existing review.
* PUT /api/Reviews: Updates an existing review.
* GET /api/Reviews/{id}: Retrieves a review by its unique ID.
* GET /api/Reviews/GetReviewsByBook: Retrieves all reviews for a specific book.
* GET /api/Reviews/GetReviewsByReader: Retrieves all reviews written by a specific reader.

