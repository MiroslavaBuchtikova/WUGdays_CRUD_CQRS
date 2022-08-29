import {Component, EventEmitter, Inject, Input, OnInit, Output} from '@angular/core';
import {NgbActiveModal, NgbModal} from "@ng-bootstrap/ng-bootstrap";
import {CourseDto} from "../models/course";
import {getGrades, Grades} from "../models/grade";
import {TransferDto} from "../models/transfer";
import {DisenrollmentDto} from "../models/disenrollment";

@Component({
  selector: 'app-disenroll-dialog',
  templateUrl: './disenroll-dialog.component.html'
})
export class DisenrollDialogComponent {
  @Output() passEntry: EventEmitter<DisenrollmentDto> = new EventEmitter();

 public comment: string;
  constructor(public activeModal: NgbActiveModal) {
  }

  passBack() {
    this.passEntry.emit({
     comment: this.comment
    });
    this.activeModal.close();
  }
}
