CableCo
=======

A simple prototype implementation of an event-driven architecture using Rebus with Castle Windsor and NHibernate.


Setting up the Project
----------------------

Visual Studio 2012 solution with ASP.Net MVC4. 

Nuget package restore has been enabled for the solution, Visual Studio should prompt you to restore missing packages when you first open the solution. The solution should then build after packages have been downloaded.


Running the Application
-----------------------

The solution contains 3 applications, which are designed to collaborate via messaging and make things happen:

- A front-end web application CableCo.Accounts.WebApp
- A service CableCo.AccountsService that makes changes to customer accounts
- A service called CableCo.ProvisioningService that pretends to provision TV services that a customer has subscribed to

Both of the service applications run as console apps. You can set up the solution to run (and debug) these applications together as follows:

- Right-click on the solution in Solution Explorer and choose properties (or select solution and ALT-ENTER)
- Go to Common Properties > Startup Project
- Select Multiple startup projects and set the Action option to "Start" for CableCo.Accounts.WebApp, CableCo.AccountsService and CableCo.ProvisioningService

Hit F5!

Database set up is automated. The solution uses your SQL Server Express LocalDb instance (localDB)\v11.0, which should be available on any machine running Visual Studio 2012. Note that the main database is reset every time the web application restarts. See http://www.danmalcolm.com/2013/04/testing-your-database-with-tinynh.html for more about automated database setup.

MSMQ queue creation is also automated by Rebus and queues should be created the first time that you run the application. You _might_ notice errors the very first time that start the application. CableCo.AccountsService and CableCo.ProvisioningService subscribe to each other's event messages and one might attempt to send a subscription message to the other's queue before it has been set up. In practice, I haven't seen this happen regularly.

While automated queue and database creation is very useful during development, a different approach should be used for test and production environments that takes into account permissions, security etc etc.


WTFs
----

A common source of errors / confusion during development is left-over messages and subscription data. If you have made a functional change, renamed a queue or similar, check that all .input queues are empty before restarting the solution. You can also reset subscription data used by Rebus by dropping the(service name)Endpoint.Dev databases - these will be recreated upon startup.


