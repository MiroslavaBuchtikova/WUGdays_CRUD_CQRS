import {Component, EventEmitter, Inject, Input, OnInit, Output} from '@angular/core';
import {NgbActiveModal, NgbModal} from "@ng-bootstrap/ng-bootstrap";
import {CourseDto} from "../../models/course";
import {EnrollmentDto} from "../../models/enrollment";
import {getGrades} from "../../models/grade";

@Component({
  selector: 'app-enroll-dialog',
  templateUrl: './enroll-dialog.component.html'
})
export class EnrollDialogComponent {
  @Input() courses: CourseDto[] = [];
  @Output() passEntry: EventEmitter<EnrollmentDto> = new EventEmitter();

  public grades = getGrades();
  public selectedGrade: string;
  public selectedCourse: string;

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
