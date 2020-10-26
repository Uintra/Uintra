import { ActivityEnum } from '../enums/activity-type.enum';

export default class ParseHelper {
    static parseActivityType = (activityType: number): string => {
        return ActivityEnum[activityType];
    }

    static stripHtml = (html: string): string => {
        if (!html) {
            return '';
        }

        const stripped = html
            .replace(/&nbsp;/g, ' ')
            .replace(/<[^>]*>?/gm, '');

        return stripped;
    }
}
