import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

// force compiler to generate lazyLoaded modules
// at start application the RouteConfigService replace this config
const routes: Routes = [
    {path: "", loadChildren: "./ui/pages/home/home.module#HomeModule"},
    {path: "", loadChildren: "./ui/pages/content/content.module#ContentModule"},
    {path: "**", loadChildren: "./ui/pages/not-found/not-found.module#NotFoundModule"},
    {path: "", loadChildren: "./ui/pages/search-result/search-result.module#SearchResultModule"},
    {path: "", loadChildren: "./ui/pages/news-overview/news-overview.module#NewsOverviewModule"},
    {path: "", loadChildren: "./ui/pages/news-details/news-details.module#NewsDetailsModule"},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
