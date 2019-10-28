import { ErrorHandler } from '@angular/core';

export class AppErrorHandler implements ErrorHandler {
    handleError(error: any): void {
        try {
            console.error(error.message)
            console.error(error.stack);
        } catch {
            console.error(error);
        }
    }
}