import { Component, Input, Output, EventEmitter, SimpleChange } from '@angular/core';
import { SubscribeService } from '../../service/subscribe.service';
import { SafeHtml } from '@angular/platform-browser';

export interface ISubscribeData {
  description: string;
  title: string;
  agreementText: ISubscribeAgreementData;
  lists: ISubscribeListItem[];
}

export interface ISubscribeListItem {
  listId: string;
  listName: string;
}

export interface ISubscribeAgreementData {
  title: string;
  description: SafeHtml;
}

@Component({
  selector: 'ubl-subscribe',
  templateUrl: './subscribe.component.html',
  styleUrls: ['./subscribe.component.less']
})
export class SubscribeComponent {
  @Input() data: ISubscribeData;
  @Output() showPrivacy = new EventEmitter();

  isBusy: boolean;
  isSuccess: boolean;

  agreed: boolean;
  email: string;
  emailPattern = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

  selectAll = true;
  partlySelected = false;
  model: { name: string; id: string; checked: boolean; }[];

  constructor(
    private subscribeService: SubscribeService,
  ) {}

  ngOnInit()
  {
    this.model = this.data && this.data.lists && this.data.lists.map(i => {
      return {
        name: i.listName,
        id: i.listId,
        checked: this.selectAll
      }
    });
  }

  ngDoCheck()
  {
    // this.selectAll = this.model.filter(i => i.checked).length === this.model.length;
    this.partlySelected = this.isPartlyChecked();
    if (this.partlySelected && this.selectAll) this.selectAll = false;
  }

  async onSubscribe(email: string)
  {
    this.isBusy = true;
    const result = await this.subscribeService.subscribe({
      email,
      listIds: this.model.filter(i => i.checked).map(i => i.id),
      agreementText: {
        title: this.data.agreementText.title,
        description: this.data.agreementText.description as string,
      }
    });

    this.isBusy = false;

    if (result === 200)
    {
      this.isSuccess = true;
    } else {
      this.isSuccess = false;
    }
  }

  toggleSelectAll()
  {
    this.model.forEach(i => i.checked = !this.selectAll);
  }

  isPartlyChecked()
  {
    const checked = this.model.filter(i => i.checked).length;

    if (checked)
    {
      return checked  !== this.model.length && !!this.model.find(i => i.checked);
    }

    return false;
  }
}
