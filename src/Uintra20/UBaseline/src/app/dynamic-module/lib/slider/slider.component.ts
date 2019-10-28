import { Component, OnInit, Input, ChangeDetectionStrategy } from '@angular/core';
import { trigger, state, style, transition, animate, AnimationEvent } from '@angular/animations';
import { Subject } from 'rxjs';

export interface ISliderConfig {
  show: {dots: boolean, arrows: boolean};
  slidesCount: number;
}
@Component({
  selector: 'app-slider',
  templateUrl: './slider.component.html',
  styleUrls: ['./slider.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  animations: [
    trigger('slide', [
      state('animate', style({transform: 'translateX(-{{slideOffset}}%)'}), {params: {slideOffset: 100}}),
      transition('done => animate', animate(300)),
      transition('animate => done', animate(0))
    ])
  ]
})
export class SliderComponent implements OnInit {
  @Input() config: ISliderConfig;

  private _config: ISliderConfig;
  private defaultConfig: Partial<ISliderConfig>;
  private $slideIndex = new Subject<number>();

  animation: any;
  currentSlideIndex: number = 0;
  slidesCount: number = 0;
  toolbarVm = {show: {prev: false, next: false}};

  constructor() 
  {
    this.defaultConfig = {
      show: {dots: true, arrows: true},
      slidesCount: 0
    }
  }

  ngOnInit() 
  {
    this._config = Object.assign({}, this.defaultConfig, this.config);
    this.slidesCount = this._config.slidesCount;
    
    if (this.slidesCount) this.toolbarVm.show.next = true;
    
    this.$slideIndex.subscribe(index => {
      this.currentSlideIndex = index;
      this.currentSlideIndex ? this.toolbarVm.show.prev = true : this.toolbarVm.show.prev = false;
      this.currentSlideIndex < this.slidesCount - 1 ? this.toolbarVm.show.next = true : this.toolbarVm.show.next = false;

      this.animation = this.animate();
    });
  }

  handleIndexChanged(index: number) 
  {    
    this.$slideIndex.next(index);
  }

  handlePrevNextEvent(step: number)
  {
    const index = this.currentSlideIndex + step;

    if (index >= 0 && index <= this._config.slidesCount) this.$slideIndex.next(index)
  }

  private animate()
  {
    return {value: 'animate', params: {slideOffset: this.currentSlideIndex * 100}}
  }

}
