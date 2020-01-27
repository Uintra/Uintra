import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, ActivatedRoute } from '@angular/router';

import { UbaselineCoreModule } from '@ubaseline/next';
import { UintraewsetailsagePage } from './uintraewsetailsage-page.component';

@NgModule({
  declarations: [UintraewsetailsagePage],
  imports: [
    CommonModule,
    RouterModule.forChild([{path: "", component: UintraewsetailsagePage}]),
    UbaselineCoreModule,
  ],
  entryComponents: [UintraewsetailsagePage]
})
export class UintraewsetailsagePageModule {
  data: any;

  constructor(
    private activatedRoute: ActivatedRoute
  ) {
    this.activatedRoute.data.subscribe(data => this.data = data);
  }

  ngOnInit(): void {
  }
}
