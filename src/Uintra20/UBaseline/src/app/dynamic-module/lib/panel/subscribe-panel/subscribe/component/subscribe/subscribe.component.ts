import { Component, Input, Output, EventEmitter, ViewChild, ElementRef } from '@angular/core';
import { SubscribeService, SubscribeServiceResponseStatus, ISubscribeModel } from '../../service/subscribe.service';
import { SafeHtml } from '@angular/platform-browser';
import { NgForm } from '@angular/forms';
import { group } from '@angular/animations';

export interface ISubscribeData {
  description: string;
  title: string;
  agreementText: ISubscribeAgreementData;
  lists: ISubscribeListItem[];
}

export interface ISubscribeListItem {
  listId: string;
  listName: string;
  groups: {groupId: string}[];
}

export interface ISubscribeAgreementData {
  title: string;
  description: SafeHtml;
}

interface IViewModel {
  name: string; id:
  string; checked:
  boolean;
  groups: {groupId: string}[]
}
@Component({
  selector: 'ubl-subscribe',
  templateUrl: './subscribe.component.html',
  styleUrls: ['./subscribe.component.less']
})
export class SubscribeComponent {
  @Input() data: ISubscribeData;
  @Output() showPrivacy = new EventEmitter();

  @ViewChild('subscribeForm', {static: false}) form: NgForm;
  @ViewChild('button', {static: false}) showAllTopics: ElementRef;

  isBusy: boolean;
  isSuccess: boolean;
  temp = false;
  agreed: boolean;
  email: string;
  emailPattern = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

  selectAll = true;
  partlySelected = false;
  model: IViewModel[];

  constructor(
    private subscribeService: SubscribeService,
  ) {}

  ngOnInit()
  {
    this.model = this.data && this.data.lists && this.data.lists.map(i => {
      if (!i.listId || !i.listName) return null;
      return {
        name: i.listName,
        id: i.listId,
        groups: i.groups,
        checked: this.selectAll
      }
    }).filter(i => i !== null);
  }

  ngDoCheck()
  {
    this.partlySelected = this.isPartlyChecked();
    if (this.partlySelected && this.selectAll) return this.selectAll = false;

    if (this.model.filter(i => i.checked).length === this.model.length) this.selectAll = true;
  }

  async onSubscribe(email: string)
  {
    this.isBusy = true;

    const lists = this
      .model.filter(entry => entry.checked)
      .map(entry => {
        return {
          id: entry.id,
          groups: entry.groups.map(g => g.groupId)
        }
      })

    const postModel: ISubscribeModel = {
      email,
      lists,
      agreementText: {
        title: this.data.agreementText.title,
        description: this.data.agreementText.description as string,
      }
    }
    const result = await this.subscribeService.subscribe(postModel);

    if (result === SubscribeServiceResponseStatus.statussubscribed)
    {
      const { form } = this.form;

      this.isSuccess = true;
      this.agreed = false;
      this.toggleSelectAll(true);
      this.email = '';
      this.showAllTopics && ((this.showAllTopics as any).toggled = false);

      form.markAsUntouched();
      form.markAsPristine();
      form.updateValueAndValidity();
    } else {
      this.isSuccess = false;
    }

    this.isBusy = false;
  }

  someListIdSelected()
  {
    return this.model.some(i => i.checked);
  }

  toggleSelectAll(flag: boolean)
  {
    this.model.forEach(i => i.checked = flag);
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
