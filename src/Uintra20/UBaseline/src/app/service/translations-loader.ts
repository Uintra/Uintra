import { Injectable } from "@angular/core";
import { TranslateLoader } from '@ngx-translate/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { config } from '../app.config';

@Injectable({
    providedIn: 'root'
})
export class TranslationsLoader implements TranslateLoader {
    constructor(private http: HttpClient) {}
    
    getTranslation(lang: string): Observable<any> 
    {
        return this.http.get(config.translateApi);
    }
    
}