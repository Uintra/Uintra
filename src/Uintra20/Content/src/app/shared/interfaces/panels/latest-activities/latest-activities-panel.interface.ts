import { IPublication } from 'src/app/ui/panels/central-feed/central-feed-panel.interface';

export interface ILatestActivitiesPanel {
    activityType?: any;
    addToSitemap?: boolean;
    contentTypeAlias?: string;
    countToDisplay?: number;
    feed?: Array<IPublication>;
    id?: number;
    name?: string;
    nodeType?: number;
    showSeeAllButton?: boolean;
    teaser?: string;
    title?: string;
    url?: string;
}
