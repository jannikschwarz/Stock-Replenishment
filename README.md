# Stock-Repleneishment
A simple Stock Replenishment Reuqest system used by in factory production line workers to request the refilling of materials at their station when running low. The request follows this lifecycle
Draft → Submitted → Approved → Fulfilled
                  ↘ Rejected

Tech stack: 
- BackendAPI: ASP.Net Core (.Net 10) using controller RequestController.cs
- Data Access: Entity Framework Core
- Database: In memory database context
- Frontend: Blazor + MudBlazor
- Testing: NUnit + NSubstitute

# Comments
I managed to setup a fully working WebAPI with the mentioned RequestController.cs. I used Visual Studio Code as my coding enviroment and setup the needed projects I think would make a good structure for such an application. 
I have seeded the application with some data but currently the only real functional way of interacting with the API is utilizing swagger "[localhost:5273/swagger](http://localhost:5273/swagger/index.html)". This is because I sadly have to admit I have not yet coded with Mudblazor and only known Razor in of itself. 
This made it as such that I simply ran out of time when implementing the MudBlazor table the user would interact with, and as can be seen in my code (Replenishment.Blazor/Pages/Requests.razor) I got stuck on the onclick interaction. If more time is given I would love to finish the application, as sadly I first saw the mail on the 22th of May and could first work on it the 24th of May. 

# To run
Currently two terminals are needed to run 
dotnet run --project Replenishment.Api
dotnet run --project Replenishment.Blazor
