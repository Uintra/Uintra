import {
  Component,
  OnInit,
  ViewEncapsulation,
  HostBinding,
  Input,
  Output,
  EventEmitter,
  ViewChild
} from "@angular/core";
import { DEFAULT_DROPZONE_CONFIG } from "src/app/constants/dropzone/drop-zone.const";
import { DropzoneComponent } from "ngx-dropzone-wrapper";

export interface IDropzoneConfig {
  maxFiles?: number;
  acceptedFiles?: string;
}

@Component({
  selector: "app-dropzone-wrapper",
  templateUrl: "./dropzone-wrapper.component.html",
  styleUrls: ["./dropzone-wrapper.component.less"],
  encapsulation: ViewEncapsulation.None
})
export class DropzoneWrapperComponent implements OnInit {
  @ViewChild(DropzoneComponent, { static: false })
  dropdownRef?: DropzoneComponent;
  @Input() customMessage: string;
  @Input() filesLength: number;
  @Input() maxFiles: number;
  @Input() disabled: boolean = false;
  @Input() withImage: boolean = true;
  @Input() allowedExtensions: string;
  @Output() success = new EventEmitter();
  @Output() removedFile = new EventEmitter();
  @HostBinding("class") className: string;

  message: string;
  config: IDropzoneConfig = {};

  constructor() {}

  ngOnInit() {
    this.config.maxFiles = this.maxFiles || DEFAULT_DROPZONE_CONFIG.maxFiles;
    this.config.acceptedFiles = this.allowedExtensions ? this.allowedExtensions : null;
    this.className = "dropzone-wrapper";
    this.message = `${this.withImage ? "<span class='custom-message'><span class='icon-upload'>" : ''}</span>${
      this.customMessage ? this.customMessage : "Insert image"
    }</span>`;
  }

  onUploadSuccess(event) {
    this.success.emit(event);
  }

  onFileRemoved(event) {
    this.removedFile.emit(event);
  }

  handleReset() {
    this.dropdownRef.directiveRef.reset();
  }
}
