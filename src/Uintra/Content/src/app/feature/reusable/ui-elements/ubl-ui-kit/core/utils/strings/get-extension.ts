export function getExtension(str: string) {
  if (!!str && str.match(/\.\w+$/)) {
    return `picture--${str.match(/\.\w+$/)[0].slice(1)}`;
  }

  return '';
}