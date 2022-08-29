import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';
import {FormsModule} from '@angular/forms';
import {HttpClientModule} from '@angular/common/http';
import {RouterModule} from '@angular/router';

import {AppComponent} from './app.component';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import {StudentTableComponent} from "./dialogs/student-table/student-table.component";
import {TransferDialogComponent} from "./dialogs/transfer-dialog/transfer-dialog.component";
import {EnrollDialogComponent} from "./dialogs/enroll-dialog/enroll-dialog.component";
import {PersonalInfoDialogComponent} from "./dialogs/personal-info-dialog/personal-info-dialog.component";
import {DisenrollDialogComponent} from "./dialogs/disenroll-dialog/disenroll-dialog.component";
import {StudentDialogComponent} from "./dialogs/student-dialog/student-dialog.component";

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
