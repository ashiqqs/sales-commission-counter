import { Injectable, ErrorHandler, Injector, Inject } from '@angular/core';
import { HttpResponse, HttpErrorResponse } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';


@Injectable()

export class AppErrorHandler implements ErrorHandler {

    constructor(private inject: Injector) {
    }

    handleError(error: Error | HttpErrorResponse) {
        const toaster = this.inject.get(ToastrService);
        const router = this.inject.get(Router);

        if (!navigator.onLine) {
            return toaster.error('Check your internet connection');
        }
        if (error instanceof HttpErrorResponse) {
            toaster.toastrConfig.preventDuplicates = true;
            toaster.toastrConfig.onActivateTick = true;

            if (error.status == 0) {
                return toaster.error('No response from server');
            }
            else if (error.status == 404) {
                return router.navigate(['/not-found'])
            }
            else if (error.status == 401) {
                return toaster.error('Unauthorized access!');
            }
            else if (error.status == 500) {
                return toaster.error('Internal Server Error');
            }
            else {
                return toaster.error(error.message, error.statusText);
            }
        }
        else {
            sessionStorage.setItem('error', error.message);
            toaster.error(error.message);
            console.error(error)
        }
    }
}