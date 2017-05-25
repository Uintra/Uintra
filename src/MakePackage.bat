choice /c yn /m "Delete all package files in folder?"
if %errorlevel% equ 1 del *.nupkg
 
 
.\.nuget\nuget pack uIntra.Users\uIntra.Users.csproj -Build -Prop Configuration=Release
.\.nuget\nuget pack uIntra.BaseControls\uIntra.BaseControls.csproj -Build -Prop Configuration=Release
.\.nuget\nuget pack uIntra.CentralFeed\uIntra.CentralFeed.csproj -Build -Prop Configuration=Release
.\.nuget\nuget pack uIntra.Comments\uIntra.Comments.csproj -Build -Prop Configuration=Release
.\.nuget\nuget pack uIntra.Core\uIntra.Core.csproj -Build -Prop Configuration=Release
.\.nuget\nuget pack uIntra.Likes\uIntra.Likes.csproj -Build -Prop Configuration=Release
.\.nuget\nuget pack uIntra.Navigation\uIntra.Navigation.csproj -Build -Prop Configuration=Release
.\.nuget\nuget pack uIntra.News\uIntra.News.csproj -Build -Prop Configuration=Release
.\.nuget\nuget pack uIntra.Notification\uIntra.Notification.csproj -Build -Prop Configuration=Release
.\.nuget\nuget pack uIntra.Subscribe\uIntra.Subscribe.csproj -Build -Prop Configuration=Release
.\.nuget\nuget pack uIntra.Events\uIntra.Events.csproj -Build -Prop Configuration=Release

pause