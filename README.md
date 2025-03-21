# Steal All The Cats API

This is a RESTful API that fetches cat images from [TheCatAPI](https://thecatapi.com/) and stores them in a Microsoft SQL Server database using Entity Framework Core.

##Features
- Fetch and store cat images with their tags.
- Retrieve cats by ID.
- Paginated retrieval of cats.
- Filter cats by temperament (tag).
- Swagger API documentation at `/swagger`.

---

## Prerequisites
Before running the project, ensure you have:
- .NET 8 SDK
- SQL Server 2019+
- Git
- Visual Studio 2022+

---

## Installation Steps
### 1. Clone the Repository
```sh
git clone https://github.com/philipnouris/StealAllTheCats.git
cd ./StealAllTheCats
```

### 2. Configure Database Connection
###1. Open appsettings.json and modify the ConnectionStrings:
```
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=StealAllTheCatsDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
```
#### 2. Replace YOUR_SERVER_NAME with your SQL Server Instance (e.g. STEALCATSDB\\SQLEXPRESS)

### 3. Install Dependencies

Run the follwoing command:
```dotnet restore```

### 4. Set up Database
Run these commands to apply database migrations:
```
dotnet ef migrations add InitialCreate
dotnet ef database update
```
This will create the required tables in SQL Server

### 5. Configure API URL and API Key
```To change the API URL or API Key, modify the appsettings.json file
"CatApi": {
  "BaseUrl": "https://api.thecatapi.com/v1/images/search?limit=25&has_breeds=1",
  "ApiKey": "YOUR_API_KEY_HERE"
}
```
### 6. Run the APplication
Start the application either with:
```
dotnet run
```
Or simply run https button from the Visual studio 2022

### 7. API Endpoints
#### 1. Fetch 25 Cats from API and Store in Database
```https
POST /api/cats/fetch
https://localhost:7118/api/cats/fetch
```
```Response body:
{
  "message": "Cats fetched and stored successfully!"
}
```
#### 2. Get Cat By Id

```https: i.e. input value of id: 26
GET /api/cats/{id}
https://localhost:7118/api/cats/26
```
```Response body:
{
  "$id": "1",
  "id": 26,
  "catId": "p6x60nX6U",
  "width": 1032,
  "height": 774,
  "imageUrl": "https://cdn2.thecatapi.com/images/p6x60nX6U.jpg",
  "created": "2025-03-19T23:30:51.513",
  "tags": {"$id": "2","$values": [{"$id": "3","id": 12,"name": "Active","created": "2025-03-19T23:30:51.423","cats": {"$id": "4","$values": [{"$ref": "1"}]}},......]
  }
}
```
#### 3. Get Cats per Page (Pagination)
```https: i.e. input values for page: 1 and pagesize: 10 
GET /api/cats/getCatsPerPage
https://localhost:7118/api/cats/getCatsPerPage?page=1&pageSize=10
```
```Response body:
{
  "$id": "1",
  "$values": [
    {
      "$id": "2",
      "id": 26,
      "catId": "p6x60nX6U",
      "width": 1032,
      "height": 774,
      "imageUrl": "https://cdn2.thecatapi.com/images/p6x60nX6U.jpg",
      "tags": {
        "$id": "3",
        "$values": [
          "Active",
          "Energetic",
          "Independent",
          "Intelligent",
          "Gentle"
        ]
      }
    },
   ........
```
#### 4. Get Cats By Tag (Pagination)
```https: i.e. input values for tag: 'active', page: 1, pagesize: 10
GET /api/cats/getCatsByTag
https://localhost:7118/api/cats/getCatsByTag?tag=active&page=1&pageSize=10
```
```Response body:

{
  "$id": "1",
  "$values": [
    {
      "$id": "2",
      "id": 26,
      "catId": "p6x60nX6U",
      "width": 1032,
      "height": 774,
      "imageUrl": "https://cdn2.thecatapi.com/images/p6x60nX6U.jpg",
      "tags": {
        "$id": "3",
        "$values": [
          "Active",
          "Energetic",
          "Independent",
          "Intelligent",
          "Gentle"
        ]
      }
    },
    {
      "$id": "4",
      "id": 29,
      "catId": "8CuEPFNuD",
      "width": 1024,
      "height": 768,
      "imageUrl": "https://cdn2.thecatapi.com/images/8CuEPFNuD.jpg",
      "tags": {
        "$id": "5",
        "$values": [
          "Active",
          "Intelligent",
          "Affectionate",
          "Curious",
          "Playful"
        ]
      }
    },
```



