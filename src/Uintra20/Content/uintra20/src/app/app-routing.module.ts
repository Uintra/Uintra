import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { ResolveComponent, ResolveService } from '@ubaseline/next';
import { pages } from './ui/pages/pages';
import { panels } from './ui/panels/panels';

const routes = [
  {
    path: "**", component: ResolveComponent, resolve: { data: ResolveService },
  },
  ...pages, ...panels
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes)
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
