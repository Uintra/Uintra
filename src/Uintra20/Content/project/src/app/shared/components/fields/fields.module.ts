import {NgModule} from '@angular/core';
import {FormsModule} from '@angular/forms';

import {InputFieldComponent} from './input-field/input-field.component';
import { CommonModule } from '@angular/common';

@NgModule({
    imports: [
        CommonModule,
        FormsModule
    ],
    declarations: [
        InputFieldComponent,
    ],
    exports: [
        InputFieldComponent,
    ]
})
export class FieldsModule {}
