import { Component, ViewContainerRef, ViewChild, Input } from '@angular/core';
import { DynamicComponentLoaderService } from '../../dynamic-component-loader/dynamic-component-loader.service';
import { isDefaultModel, defaultModel2UProperty as defaultModel2ViewModel } from 'src/app/dynamic-module/lib/mapper/defaultModel2UProperty';

export interface IPanel {
  contentTypeAlias: string;
}
@Component({
  selector: 'app-dynamic-component',
  templateUrl: './dynamic.component.html',
  styleUrls: ['./dynamic.component.less']
})
export class DynamicComponent {
  @Input() data: IPanel;

  constructor(private dynamicComponentLoader: DynamicComponentLoaderService) { }

  @ViewChild('component', {read: ViewContainerRef, static: false}) 
  set componentOutlet(containerRef: ViewContainerRef)
  {
    if (!this.data.contentTypeAlias) return;
    this.loadComponentTo(containerRef);
  }

  private loadComponentTo(containerRef: ViewContainerRef)
  {
    this.dynamicComponentLoader.getComponentFactory(this.data.contentTypeAlias).subscribe(
      factory => {
        if (!factory) return;
        
        const component = containerRef.createComponent<any>(factory);
        const dataMapper = this.dynamicComponentLoader.getDataMapperFor(this.data.contentTypeAlias);
        
        if (isDefaultModel(this.data))
        {
          this.data = dataMapper ? dataMapper.map(this.data) : defaultModel2ViewModel(this.data);
        }
        
        component.instance.data = this.data;
      },
      error => { debugger; }
    );
  }
}
