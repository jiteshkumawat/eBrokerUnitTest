# eBroker

## Overview and Assumptions
eBroker is a project for validating learnings of **Jitesh Kumawat** Unit Testing for NAGP Program. This project is to imitate an online trading application. The User here is identified as a Trader, who can perform operations like purchase and sell of equities. A Trader can perform the following operations:
1. **Add Funds:** The trader is able to add funds to his virtual pocket. Funds will be used by traders to purchase equities. A trader can add funds any time of the day and any amount can be added. On adding funds above 1L, a brokerage of 0.05% would be charged, i.e. Rs. 50 for 1L.
1. **Buy Equity:** The trader is allowed to purchase equities from available funds in his virtual pocket. He should be able to purchase equity only between 9:00 AM and 3:00 PM of the day. _For the time constraint, not localization is considered._ Trader should have sufficient funds to purchase any equity, if he does not have sufficient funds, purchase operation should be unsuccessful.
1. **Sell Equity:** The trader can sell equities that he holds. Just like the Purchase of equities, a trader can sell his equity only between 9:00 AM and 3:00 PM, and localization is not considered here as well. Equity quantity can not be more than what the trader holds. On selling of equities, a brokerage of 0.05% or Rs. 20 (whichever is more) is deducted from the equity amount.

## Application Structure

Use solution file eBroker.sln for accessing all the projects in Visual Studio. The central entities / models for working with application are Trader, Equity, and TraderEquity. Trader represents a user who can perform all the above operations. Equity represents an equity entity that can be purchased and sold by a trader. Equity entities are referenced into trader and represent reference, quantity to equity which a trader holds. So we have the following entities/model in the system:

| Column | Data Type | Description |
| - | - | - |
| **BaseModel** | | |
| ID  | int  | Entity Identifier |
| Name  | string  | Entity Name |
| **Equity <- BaseModel** | | |
| Amount  | double | Amount of one Equity |
| **Trader <- BaseModel** | | |
| Funds  | double | Funds / Amount in Virtual pocket of Trader |
| TraderEquities | ICollection<TraderEquity> | Equities purchased by trader |
| **TraderEquity** | | |
| Id  | int | Trader Equity Identifier |
| TraderId | int | Trader Id (FK -> Trader(Id)) |
| Trader | Trader | Trader Instance |
| EquityId | int | Equity Id (FK -> Equity(Id)) |
| Equity | int | Equity Instance |
| Quantity | int | Equity Quantity |

All the projects are placed in different folders with the same name. So we have following projects in application:

### eBroker.Core
This project is utility / services project for complete application. Just for simplicity, all the models are also placed in this project. OperationsUtility is static utility to be provided with a wrapper OperationUtilityProxy around it.

### eBroker.Presentation
This project is the ***startup project*** for the complete solution. It contains RESTful API controllers for Equity and Trader. Apart from CRUD opertions, user can also perform BuyEquity, SellEquity and AddFunds in Trader controller. So following API are available:

| API | HTTP Method | Description |
| - | - | - |
| **EquityControler** | | |
| {API}/Trader | HTTP GET | Get all the Traders |
| {API}/Trader/{id} | HTTP GET | Get a Trader with id |
| {API}/Trader | HTTP POST | Insert a Trader |
| {API}/Trader | HTTP PUT | Update a Trader |
| {API}/Trader/{id} | HTTP DELETE | Remove a Trader |
| {API}/Trader/buy/{traderId}/{equityId}/{quantity}/{time} | HTTP POST | Buy Equities |
| {API}/Trader/sell/{traderId}/{equityId}/{quantity}/{time} | HTTP POST | Sell Equities |
| {API}/Trader/addfunds/{traderId}/{amount} | HTTP POST | Add Funds to Virtual Pocket |
| **TraderControler** | | |
| {API}/Equity | HTTP GET | Get all the Equities |
| {API}/Equity/{id} | HTTP GET | Get a Equity with id |
| {API}/Equity | HTTP POST | Insert a Equity |
| {API}/Equity | HTTP PUT | Update a Equity |
| {API}/Equity/{id} | HTTP DELETE | Remove a Equity |

### eBroker.Business
This project contains all the Business logic for application. It fetch data from Data Access Layer, and provide it to presentation layer.

### eBroker.DAL
This project contains repository, context and all the logic to access and update data in database. It is accessible by Business layer.

### eBorker.Test
This project contains all the Unit Test cases. Following are the important files in this project.

| File | Description |
| - | - |
| TestBed.cs | It contains logic to execute before any test case. DBOptions provider for InMemory Db is done in this file. |
| DataSource \ EquityDataSource.cs | Provide data source for writing Unit test cases for Equity Manager. |
| DataSource \ TraderDataSource.cs | Provide data source for writing Unit test cases for Trader Manager. |
| Repository \ EquityRepository.Test.cs | Test cases for Equity Repository. |
| Repository \ TradeRepository.Test.cs | Test cases for Trade Repository. |
| Repository \ TradeEquityRepository.test.cs | Test cases for Trade-Equity Repository |
| Business \ EquityManager.Test.cs | Test cases for Equity Manager. |
| Business \ TraderManager.Test.cs | Test cases for Trader Manager. |
| Presentation \ EquityController.Test.cs | Test cases for Equity Controller. |
| Presentation \ TraderController.Test.cs | Test cases for Trader Controller. |
| Presentation \ Startup.Test.cs | Test cases for Startup file |
| Services \ OperationUtility.Test.cs | Test cases for Operation Utility |

Appart from this, in this project coverate report is saved in **coveragereport** folder and Test Results are saved in **TestResults** folder.

## Running Project

To run this project, from **eBroker.Presentation** folder execute following command:
```console
dotnet run
```
This will run application on port 5000. All the services are accessible with "localhost:5000/{API}" syntax. Also for documentation and use of API, I have used Swagger to enable user interation with controller. For using swager navigate to "[localhost:5000/swagger](http://localhost:5000/swagger)".

When using Visual Studio, use eBroker.sln, set eBroker.Presentation as startup project and run the application, it will run application on some random port, and above mentioned interface would be available on that port.

To exeucte all the test cases and generating coverage report. Install report generator global tool using below command
```console
dotnet tool install -g dotnet-reportgenerator-globaltool
```

Now execute following commands from **eBroker.Test** project folder:
```console
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=$(BuildSourcesDirectory)\TestResults\Coverage
```
Now using the path for coverage file, execute following to generate html report:
```console
reportgenerator -reports:"{coverage file path}" targetdir:"coveragereport" - reporttypes:Html
```

I have also executed test cases and saved coverage report for initial start.
Follwing is the test result:
>+----------------------+--------+--------+--------+
| Module               | Line   | Branch | Method |
+----------------------+--------+--------+--------+
| eBroker.Business     | 100%   | 100%   | 100%   |
+----------------------+--------+--------+--------+
| eBroker.Core         | 100%   | 94.44% | 100%   |
+----------------------+--------+--------+--------+
| eBroker.DAL          | 100%   | 100%   | 100%   |
+----------------------+--------+--------+--------+
| eBroker.Presentation | 86.59% | 90%    | 85.71% |
+----------------------+--------+--------+--------+

>+---------+--------+--------+--------+
|         | Line   | Branch | Method |
+---------+--------+--------+--------+
| Total   | 95.17% | 96.34% | 96.55% |
+---------+--------+--------+--------+
| Average | 96.64% | 96.11% | 96.42% |
+---------+--------+--------+--------+

>Passed!  - Failed:     0, Passed:   162, Skipped:     0, Total:   162, Duration: 714 ms 
