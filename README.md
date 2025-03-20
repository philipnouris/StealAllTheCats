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
### **Clone the Repository**
```sh
git clone https://github.com/philipnouris/StealAllTheCats.git
cd ./StealAllTheCats
