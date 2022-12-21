# 🌐IpInfoAPI
 IpInfoAPI is a simple API that allows you to get information about countries based on IP addresses or country codes, using the [IP2C](https://about.ip2c.org/#about) service.

## 🏃Getting Started

To get started with IpInfoAPI, you'll need to make sure that you have the following dependencies installed:

* .NET 6
* Microsoft SQL Server
You'll also need to make sure that you have a database set up and connected to the API. The database should include a Countries table with the following fields:

Id (int, primary key)
TwoLetterCode (string(2))
ThreeLetterCode (string(3))
Name (string(50))
Once you have the above dependencies installed and your database set up, you can start the API by running the following command from the root directory of the project:
```
dotnet run
```

## 🔌Endpoints 

IpInfoAPI has two main endpoints:

### GET /{ipAddress} 
This endpoint allows you to get information about a specific country based on an IP address. The response will be in the following format:
```json
{
  "name": "United States",
  "twoLetterCode": "US",
  "threeLetterCode": "USA"
}
```

⌚ IP information is periodically fetched and checked for changes from the IP2C service (in an hourly timespan).

### POST /reports
This endpoint allows you to get information about multiple countries based on their two-letter codes. The request body should be a JSON array of strings, where each string is a two-letter country code. The response will be in the following format:
```json
[  {    "countryName": "Greece",    "addressessCount": 3,    "lastAddressUpdated": "2022-10-12T08:41:37"  }]
```
## ⚗️Testing 
To run the tests for IpInfoAPI, navigate to the TestProject directory and run the following command:
```
dotnet test
```

## 🔨Built With
* .NET6 ![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=c-sharp&logoColor=white) ![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
* Microsoft SQL Server  ![MicrosoftSQLServer](https://img.shields.io/badge/Microsoft%20SQL%20Sever-CC2927?style=for-the-badge&logo=microsoft%20sql%20server&logoColor=white)
* Documentation and test environment using Swagger ![Swagger](https://img.shields.io/badge/-Swagger-%23Clojure?style=for-the-badge&logo=swagger&logoColor=white)


