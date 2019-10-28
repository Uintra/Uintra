import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SliderComponent } from './slider.component';
import { SlideComponent } from './components/slide/slide.component';
import { ToolbarComponent } from './components/toolbar/toolbar.component';
import { DotsComponent } from './components/dots/dots.component';

@NgModule({
  declarations: [SliderComponent, SlideComponent, ToolbarComponent, DotsComponent],
  imports: [
    CommonModule,
    
  ],
  exports: [SliderComponent, SlideComponent]
})
export class SliderModule { }
