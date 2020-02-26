# Custom Tournaments
**Custom Tournaments** is an app that allows you to easily create and keep track of any kind of Tournament you participate in with your friends, co-workers and such. It supports custom Team creation and both League and Cup Tournament type, with visual representation of League table and Cup Tournament brackets.

**Some other features include:**
* Cup Tournaments with up to 128 teams.
  * App automatically adds as many Dummy Teams as needed to fill out the required Cup quota.
* League tournaments with odd number of Teams.
  * App automatically adds a Dummy Team to fill out the even number of League quota.
* Home and Away matchups for League Tournaments.
* Custom League sorting rules including custom amount of points for a win and a draw.
* Custom "official score" against Dummy Teams.
* Custom Tournament Prizes.

## Getting Started
To be able to use this app you'll need:
* [Visual Studio 2019 Community Edition.](https://visualstudio.microsoft.com/downloads/) The app hasn't been tested on older VS versions.
* [Sql Server Data Tools for Visual Studio 2019](https://docs.microsoft.com/en-us/sql/ssdt/download-sql-server-data-tools-ssdt?view=sql-server-2017)

### Quick Start
1. Clone the repository to your computer and open Custom Tournaments solution in Visual Studio.
2. In Solution Explorer, locate "PublishedProfiles" folder, open it and double click on "CustomTournamentsDB.publish.xml".
3. Click on "Publish" button and wait for Visual Studio to publish the database.
4. Press F5 to start the app.
