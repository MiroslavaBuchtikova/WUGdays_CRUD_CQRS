import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';
import {FormsModule} from '@angular/forms';
import {HttpClientModule} from '@angular/common/http';
import {RouterModule} from '@angular/router';

import {AppComponent} from './app.component';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import {StudentDialogComponent} from "./student-dialog/student-dialog.component";
import {StudentTableComponent} from "./student-table/student-table.component";
import {EnrollDialogComponent} from "./enroll-dialog/enroll-dialog.component";
import {TransferDialogComponent} from "./transfer-dialog/transfer-dialog.component";
import {DisenrollDialogComponent} from "./disenroll-dialog/disenroll-dialog.component";
import {PersonalInfoDialogComponent} from "./personal-info-dialog/personal-info-dialog.component";

@NgModule({
    declarations: [
        AppComponent,
        StudentTableComponent,
        StudentDialogComponent,
        EnrollDialogComponent,
        TransferDialogComponent,
        PersonalInfoDialogComponent,
        DisenrollDialogComponent,
    ],
    imports: [
        BrowserModule.withServerTransition({appId: 'ng-cli-universal'}),
        HttpClientModule,
        FormsModule,
        RouterModule.forRoot([
            {path: '', component: StudentTableComponent, pathMatch: 'full'}
        ]),
        NgbModule
    ],
    providers: [],
    bootstrap: [AppComponent]
})
export class AppModule {
}
