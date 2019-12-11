import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { ResolveComponent, ResolveService } from '@ubaseline/next';
import { pages } from './ui/pages/pages';
import { panels } from './ui/panels/panels';
import { DeveloperUIKitPage } from './ui/pages/developer-ui-kit/developer-ui-kit.component';
import { DeveloperUIKitModule } from './ui/pages/developer-ui-kit/developer-ui-ki.module';

const routes = [
  // remove it
  {
    path: "developer-ui-kit", component: DeveloperUIKitPage
  },
  {
    path: "**", component: ResolveComponent, resolve: { data: ResolveService },
  },
  ...pages, ...panels
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes),
    DeveloperUIKitModule
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
