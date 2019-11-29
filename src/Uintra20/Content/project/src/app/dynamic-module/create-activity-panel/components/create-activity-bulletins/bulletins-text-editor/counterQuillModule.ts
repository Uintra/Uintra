import 'quill';

export interface Config {
  container: string;
  maxLenght: number;
}

export interface QuillInstance {
  on: any;
  getText: any;
}

export default class Counter {
  quill: QuillInstance;
  options: Config;

  constructor(quill, options) {
    this.quill = quill;
    this.options = options;

    const container = document.querySelector(this.options.container);
    this.setHTML(container);

    this.quill.on('text-change', () => {
      this.setHTML(container);
    });
  }

  setHTML(container) {
    const length = this.calculate();
    container.innerHTML = `<span>${length}</span> / ${this.options.maxLenght}`;

    if (length > this.options.maxLenght) {
      container.classList.add('invalid');
    }
  }

  calculate() {
    const text = this.quill.getText().trim();
    return text.length;
  }
}
