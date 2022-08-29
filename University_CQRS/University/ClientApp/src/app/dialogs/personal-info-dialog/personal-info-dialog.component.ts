import {Component, EventEmitter, Inject, Input, OnInit, Output} from '@angular/core';
import {NgbActiveModal} from "@ng-bootstrap/ng-bootstrap";
import {PersonalInfoDto} from "../../models/personal-info";
import {CourseDto} from "../../models/course";
import {getGrades, Grades} from "../../models/grade";

@Component({
  selector: 'app-personal-info-dialog',
  templateUrl: './personal-info-dialog.component.html'
})
export class PersonalInfoDialogComponent  {
  @Input() personalInfo: PersonalInfoDto;
  @Input() ssn: string;
  @Input() courses: CourseDto[] = [];
  @Output() passEntry: EventEmitter<PersonalInfoDto> = new EventEmitter();

  public grades = getGrades();
  public selectedCourse1?: CourseDto;
  public selectedGrade1?: Grades;

  public selectedCourse2?: CourseDto;
  public selectedGrade2?: Grades;

  constructor(public activeModal: NgbActiveModal) {
  }

  passBack() {
    this.passEntry.emit(this.personalInfo);
    this.activeModal.close();
  }
}
