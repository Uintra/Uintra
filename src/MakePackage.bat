choice /c yn /m "Delete all package files in folder?"
if %errorlevel% equ 1 del *.nupkg
 
 
.\.nuget\nuget pack uCommunity.Users\uCommunity.Users.csproj -Build -Prop Configuration=Release
.\.nuget\nuget pack uCommunity.BaseControls\uCommunity.BaseControls.csproj -Build -Prop Configuration=Release
.\.nuget\nuget pack uCommunity.CentralFeed\uCommunity.CentralFeed.csproj -Build -Prop Configuration=Release
.\.nuget\nuget pack uCommunity.Comments\uCommunity.Comments.csproj -Build -Prop Configuration=Release
.\.nuget\nuget pack uCommunity.Core\uCommunity.Core.csproj -Build -Prop Configuration=Release
.\.nuget\nuget pack uCommunity.Likes\uCommunity.Likes.csproj -Build -Prop Configuration=Release
.\.nuget\nuget pack uCommunity.Navigation\uCommunity.Navigation.csproj -Build -Prop Configuration=Release
.\.nuget\nuget pack uCommunity.News\uCommunity.News.csproj -Build -Prop Configuration=Release
.\.nuget\nuget pack uCommunity.Notification\uCommunity.Notification.csproj -Build -Prop Configuration=Release
.\.nuget\nuget pack uCommunity.Subscribe\uCommunity.Subscribe.csproj -Build -Prop Configuration=Release

pause