export function capitalize(str: string, firstOnly = false): string {
    if (typeof str !== 'string') return '';

    if (firstOnly)
    {
        return str.charAt(0).toUpperCase() + str.slice(1);
    }

    return str.split(' ').map(part => part.charAt(0).toUpperCase() + part.slice(1)).join(' ');
}
