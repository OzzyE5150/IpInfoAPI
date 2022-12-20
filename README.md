# IpInfoAPI
 Sample ASP.net API using the [IP2C](https://about.ip2c.org/#about) service to fetch Country info given an IP address.

## 
## ⚗️ Technologies:

* Backend ASP.net API developed in C# .net6 ![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=c-sharp&logoColor=white) ![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
* Local data on MS SQL server ![MicrosoftSQLServer](https://img.shields.io/badge/Microsoft%20SQL%20Sever-CC2927?style=for-the-badge&logo=microsoft%20sql%20server&logoColor=white)
* Documentation and test environment using Swagger ![Swagger](https://img.shields.io/badge/-Swagger-%23Clojure?style=for-the-badge&logo=swagger&logoColor=white)
##

🔮 The API exposes an endpoint which returns the details of a given IP address `[CountryName, TwoLetterCode, ThreeLetterCode]`, first looking up in cache memory and if no cached data is available calls to the IP2C service to fetch.

⌚ IP information is periodically fetched and checked for changes from the IP2C service (in an hourly timespan).

🤖 The API exposes a second endpoint that returns a report on how many addresses per given country (by TwoLetterCode) exist in the database, along with the last time an IP of said country was updated, as below:
```json
{ CountryName:string, AddressesCount:int, LastAddressUpdated: DateTime }
```
--------------------------
## How to run:
* Set up the local SQL server
* Run `dotnet run` in the project folder
