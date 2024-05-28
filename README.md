# Pokemon Review API

![UML Diagram](https://github.com/Sadeqsoli/WebApplications/blob/main/PokemonReview/UMLDiagram.PNG)

## Overview

This repository contains the Pokemon Review API, which allows users to manage and review Pokemon. The UML diagram above outlines the structure of the system, including the relationships between different entities such as Pokemon, Reviews, Reviewers, Owners, and Categories.

## API Endpoints

### Pokemon

- **Get all Pokemon**
  - `GET /api/pokemon`
  - Retrieves a list of all Pokemon.

- **Get a single Pokemon**
  - `GET /api/pokemon/{id}`
  - Retrieves details of a specific Pokemon by ID.

- **Create a new Pokemon**
  - `POST /api/pokemon`
  - Adds a new Pokemon to the database.

- **Update a Pokemon**
  - `PUT /api/pokemon/{id}`
  - Updates the details of an existing Pokemon.

- **Delete a Pokemon**
  - `DELETE /api/pokemon/{id}`
  - Deletes a specific Pokemon by ID.

### Reviews

- **Get all reviews**
  - `GET /api/reviews`
  - Retrieves a list of all reviews.

- **Get reviews for a specific Pokemon**
  - `GET /api/pokemon/{pokemonId}/reviews`
  - Retrieves all reviews for a specific Pokemon.

- **Create a new review**
  - `POST /api/reviews`
  - Adds a new review to the database.

- **Update a review**
  - `PUT /api/reviews/{id}`
  - Updates the details of an existing review.

- **Delete a review**
  - `DELETE /api/reviews/{id}`
  - Deletes a specific review by ID.

### Reviewers

- **Get all reviewers**
  - `GET /api/reviewers`
  - Retrieves a list of all reviewers.

- **Get a single reviewer**
  - `GET /api/reviewers/{id}`
  - Retrieves details of a specific reviewer by ID.

### Owners

- **Get all owners**
  - `GET /api/owners`
  - Retrieves a list of all owners.

- **Get a single owner**
  - `GET /api/owners/{id}`
  - Retrieves details of a specific owner by ID.

### Categories

- **Get all categories**
  - `GET /api/categories`
  - Retrieves a list of all categories.

- **Get a single category**
  - `GET /api/categories/{id}`
  - Retrieves details of a specific category by ID.

## UML Diagram Explanation

### Entities

- **Pokemon**: Represents a Pokemon entity with attributes such as `Id`, `Name`, and `BirthDate`. It has relationships with `Review`, `Owner`, and `Category`.
- **Review**: Represents a review entity with attributes such as `id`, `Title`, and `Text`. It is linked to `Reviewer` and `Pokemon`.
- **Reviewer**: Represents a reviewer entity with attributes such as `Id`, `FirstName`, and `LastName`. It has a relationship with `Review`.
- **Owner**: Represents an owner entity with attributes such as `Id`, `Name`, and `Gym`. It is linked to `Pokemon` and `Country`.
- **Country**: Represents a country entity with attributes such as `Id` and `Name`. It has a relationship with `Owner`.
- **Category**: Represents a category entity with attributes such as `Id` and `Name`. It is linked to `Pokemon`.

### Relationships

- **Pokemon-Review**: A Pokemon can have multiple reviews.
- **Pokemon-Owner**: A Pokemon can have multiple owners and an owner can own multiple Pokemon.
- **Pokemon-Category**: A Pokemon can belong to multiple categories and a category can include multiple Pokemon.
- **Owner-Country**: An owner belongs to a country and a country can have multiple owners.
- **Review-Reviewer**: A review is written by a reviewer.

## Working with the Repository

### Prerequisites

- Ensure you have [.NET Core SDK](https://dotnet.microsoft.com/download) installed.
- A database (e.g., PostgreSQL, MySQL) to store the data.

### Installation

1. **Clone the repository**:
    ```bash
    git clone https://github.com/Sadeqsoli/WebApplications.git
    ```

2. **Navigate to the project directory**:
    ```bash
    cd WebApplications/PokemonReview
    ```

3. **Restore dependencies**:
    ```bash
    dotnet restore
    ```

4. **Configure the database connection**:
    - Create an `appsettings.Development.json` file in the root directory.
    - Add your database configuration to the `appsettings.Development.json` file:
      ```json
      {
        "ConnectionStrings": {
          "DefaultConnection": "Server=your-database-server;Database=your-database-name;User Id=your-username;Password=your-password;"
        }
      }
      ```

5. **Run database migrations**:
    ```bash
    dotnet ef database update
    ```

6. **Run the application**:
    ```bash
    dotnet run
    ```

7. Open your browser and go to `http://localhost:5000` to access the API.

### Contribution

To contribute to this project:

1. Fork the repository.
2. Create a new branch for your feature:
    ```bash
    git checkout -b feature/your-feature-name
    ```
3. Make your changes and commit them:
    ```bash
    git commit -m 'Add some feature'
    ```
4. Push your changes to your branch:
    ```bash
    git push origin feature/your-feature-name
    ```
5. Open a pull request and describe your changes.

### License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for more details.

### Contact

For any questions or suggestions, please open an issue in the repository or contact the project maintainers.
