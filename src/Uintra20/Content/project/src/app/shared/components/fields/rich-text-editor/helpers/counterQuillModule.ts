import "quill";

export interface Config {
  container: string;
  maxLength: number;
}

export interface QuillInstance {
  on: any;
  getText: any;
}

export default class Counter {
  constructor(public quill: QuillInstance, public options: Config) {
    const container = document.querySelector(this.options.container);
    this.setHTML(container);

    this.quill.on("text-change", () => {
      this.setHTML(container);
    });
  }

  setHTML(container) {
    const length = this.calculate();
    container.innerHTML = `<span>${length}</span> / ${this.options.maxLength}`;

    if (length > this.options.maxLength) {
      container.classList.add("invalid");
    }
  }

  calculate() {
    const text = this.quill.getText().trim();
    return text.length;
  }
}
