import { UrlSerializer, UrlTree, DefaultUrlSerializer } from "@angular/router";

export class CustomUrlSerializer implements UrlSerializer {
  parse(url: any): UrlTree {
    let dus = new DefaultUrlSerializer();
    return dus.parse(url);
  }

  serialize(tree: UrlTree): any {
    let dus = new DefaultUrlSerializer(),
      path = dus.serialize(tree);
    return path
      .replace(/%2/g, ",")
      .replace(/%3F/g, "?")
      .replace(/%3D/g, "=");
  }
}
