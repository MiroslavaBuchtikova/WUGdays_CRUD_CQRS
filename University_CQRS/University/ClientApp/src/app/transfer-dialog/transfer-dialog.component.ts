import {Component, EventEmitter, Inject, Input, OnInit, Output} from '@angular/core';
import {NgbActiveModal, NgbModal} from "@ng-bootstrap/ng-bootstrap";
import {CourseDto} from "../models/course";
import {getGrades, Grades} from "../models/grade";
import {TransferDto} from "../models/transfer";

@Component({
  selector: 'app-transfer-dialog',
  templateUrl: './transfer-dialog.component.html'
})
export class TransferDialogComponent {
  @Input() courses: CourseDto[] = [];
  @Input() selectedGrade: string = '';
  @Input() selectedCourse: string = '';
  
  @Output() passEntry: EventEmitter<TransferDto> = new EventEmitter();

  public grades = getGrades();
  constructor(public activeModal: NgbActiveModal) {
  }

  passBack() {
    this.passEntry.emit({
      grade: this.selectedGrade,
      course: this.selectedCourse
    });
    this.activeModal.close();
  }
}
