choice /c yn /m "Delete all package files in folder?"
if %errorlevel% equ 1 del *.nupkg
 
 
.\.nuget\nuget pack uCommunity.Users\uCommunity.Users.csproj
.\.nuget\nuget pack uCommunity.BaseControls\uCommunity.BaseControls.csproj
.\.nuget\nuget pack uCommunity.CentralFeed\uCommunity.CentralFeed.csproj
.\.nuget\nuget pack uCommunity.Comments\uCommunity.Comments.csproj
.\.nuget\nuget pack uCommunity.Core\uCommunity.Core.csproj
.\.nuget\nuget pack uCommunity.Likes\uCommunity.Likes.csproj
.\.nuget\nuget pack uCommunity.Navigation\uCommunity.Navigation.csproj
.\.nuget\nuget pack uCommunity.News\uCommunity.News.csproj
.\.nuget\nuget pack uCommunity.Notification\uCommunity.Notification.csproj
.\.nuget\nuget pack uCommunity.Subscribe\uCommunity.Subscribe.csproj

pause