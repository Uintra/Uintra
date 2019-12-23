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
    {
        id: 'socialDetailsPanel',
        path: '__dynamic__',
        loadChildren: './ui/panels/socials/social-details/module/social-details-panel.module#SocialDetailsPanelModule'
    }
];
