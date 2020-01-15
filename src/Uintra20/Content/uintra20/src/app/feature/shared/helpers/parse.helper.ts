import { ActivityEnum } from '../enums/activity-type.enum';

export default class ParseHelper {
    static parseUbaselineData(data: any) {
        return JSON.parse(JSON.stringify(data));
    }

    static parseActivityType = (activityType: number): string => {
        return ActivityEnum[activityType];
    }

    static stripHtml = (html: string): string => {
        if (!html) {
            return '';
        }

        // TODO: merge replaces into one regex
        const stripped = html
            .replace(/&nbsp;/g, ' ')
            .replace(/<[^>]*>?/gm, '');

        return stripped;
    }
}
