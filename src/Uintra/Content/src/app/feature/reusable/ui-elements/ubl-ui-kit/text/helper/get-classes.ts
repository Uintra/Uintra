import { ElementRef } from '@angular/core';

export function getContentClasses(content: ElementRef) {
  if (!content || content.nativeElement.childNodes.length === 0) return '';

  const tag = content.nativeElement.childNodes[0].localName;
  const isOnlyOneTag = content.nativeElement.childNodes.length === 1;

  if ((tag === 'q' || tag === 'blockquote') && isOnlyOneTag) {
    return '--quote';
  }
  if (tag === 'code' && isOnlyOneTag) {
    return '--code';
  }
  if (tag) {
    return '--html';
  }

  return '--text';
}