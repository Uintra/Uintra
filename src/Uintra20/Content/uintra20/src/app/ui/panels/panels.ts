export const panels = [
    {
        id: 'activityCreatePanel',
        path: '__dynamic__',
        loadChildren: './ui/panels/activity-create/activity-create-panel.module#ActivityCreatePanelModule'
    },

    {
        id: 'centralFeedPanel',
        path: '__dynamic__',
        loadChildren: './ui/panels/central-feed/central-feed-panel.module#CentralFeedPanelModule'
    },

    {
        id: 'latestActivitiesPanel',
        path: '__dynamic__',
        loadChildren: './ui/panels/latest-activities/module/latest-activities-panel.module#LatestActivitiesPanelModule'
    },
];
