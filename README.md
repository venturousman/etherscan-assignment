# etherscan-assignment

#HOW TO RUN

The following steps to run the application:
1.	Read this file (readme.docx)
2.	Install MySQL in your machine
3.	Find the file “scripts.sql” in the project folder and run it.
4.	Modify the “DefaultConnectionString” in all three projects.
5.	Press F5 Or Build and Run one of three project.
Thank you!

#OVERVIEW

I have completed the assignment with full of required features.
I created a MySQL database with the given script. Then I use the Entity Framework to connect to it.
The hierarchy level is (a solution with 3 projects inside):
1.	CrawlDataConsoleApp: A Console Application to crawl data from the give API.
2.	EtherscanAssignment: ASP.NET WebForms Application, contains Controllers, Pages, View Models…
3.	EtherscanAssignment.Infrastructure: it’s a library project, contains the entity models or data access layer to connect to database. I separate it to utilize it in two above projects. To avoid the duplication in code. 

Basically, I have completed the requirements. However, I found that there are some points have to do in the future such as:
1.	Add the logging system (i.e. log4net, …)
2.	Add the caching layer (i.e. redis, …)
3.	Add the beautiful notification / toast to message to user.
4.	Move the logic to the Service layer instead of Controller.

#PART 1

User see a SPA with the input form, pie chart and table of data.
The input form validates all input data exist or not.
User can create a new token here, then the chart and the table will reflect accordingly.
From the table, user can have the pagination with the page size is 10. To test it, we can change the page size in the request to server. 
User can edit a token by clicking Edit button, the selected token will appear in the form, then user can change info and save it.
User can export the data table to a csv file. 

#PART 2

User can go to the detail page when clicking on the Symbol value.
  
#PART 3

I created a Console Application to pull the token price from the given API
It runs every 5 mins. The simplest way is to use an infinite loop and sleep 5 mins after processing.
The idea is:
1.	Get all existing tokens’ symbols
2.	Introduce a function GetPrice for a symbol (e.g. make the request, parse data from json…)
3.	Run the tasks in parallel to get the price for all symbols
4.	Update to database
