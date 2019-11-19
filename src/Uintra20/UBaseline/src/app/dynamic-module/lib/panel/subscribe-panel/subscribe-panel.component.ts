import { Component, Input } from '@angular/core';
import { ISubscribeData, ISubscribeAgreementData, ISubscribeListItem } from './subscribe/component/subscribe/subscribe.component';
import { IUProperty, UProperty } from '../../core/interface/umbraco-property';
import { Html } from '../../core/interface/types';

export interface ISubscribePanelData {
  description: IUProperty<string>;
  title: IUProperty<string>;
  agreementText: IAgreementTextItem;
  lists: IUProperty<IListItemPanelData>[];
}

export interface IListItemPanelData {
  listId: IUProperty<string>;
  listName: IUProperty<string>;
  groups: IUProperty<IUProperty<{groupId: string}>[]>;
}

export interface IAgreementTextItem {
  description: IUProperty<Html>;
  name: string;
  title: IUProperty<string>;
  url: string;
}

@Component({
  selector: 'ubl-subscribe-panel',
  templateUrl: './subscribe-panel.component.html',
  styleUrls: ['./subscribe-panel.component.less']
})
export class SubscribePanelComponent {
  @Input() data: ISubscribePanelData;

  subscribeData: ISubscribeData;
  showModal: boolean;
  ngOnInit()
  {
    let mapped = UProperty.extract<ISubscribePanelData, ISubscribeData>(this.data,
      ['title', 'description', 'agreementText', 'lists']);
    mapped.lists = mapped.lists.map(list => {
      const mappedList = UProperty.extract<IListItemPanelData, ISubscribeListItem>(list as any, ['listId', 'listName', 'groups'])
      mappedList.groups = mappedList.groups.map(g => {
        return {groupId: (g.groupId as any as IUProperty<string>).value}
      }) as any;

      return mappedList;
    });

    const {description, title} = UProperty.extract<IAgreementTextItem, ISubscribeAgreementData>(mapped.agreementText as any,
      ['description', 'title',]);

    mapped.agreementText.description = description;
    mapped.agreementText.title = title;
    this.subscribeData = mapped;
  }

  showAgreements()
  {
    this.showModal = true;
  }
}
