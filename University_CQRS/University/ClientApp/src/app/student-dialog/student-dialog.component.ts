import {Component, EventEmitter, Inject, Input, OnInit, Output} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {NgbActiveModal, NgbModal} from "@ng-bootstrap/ng-bootstrap";
import {StudentDto} from '../models/student';
import {CourseDto} from "../models/course";
import {getGrades, Grades} from "../models/grade";

@Component({
  selector: 'app-student-dialog',
  templateUrl: './student-dialog.component.html'
})
export class StudentDialogComponent implements OnInit {

  @Input() student: StudentDto = {
    name: '',
    ssn: '',
    email: '',
    course1: '',
    course1Grade: '',
    course1DisenrollmentComment: '',
    course1Credits: null,

    course2: '',
    course2Grade: '',
    course2DisenrollmentComment: '',
    course2Credits: null
  };
  @Input() courses: CourseDto[] = [];
  @Input() update: boolean;
  @Output() passEntry: EventEmitter<any> = new EventEmitter();

  public grades = getGrades();
  public selectedCourse1?: CourseDto;
  public selectedGrade1?: Grades;

  public selectedCourse2?: CourseDto;
  public selectedGrade2?: Grades;

  constructor(public activeModal: NgbActiveModal) {
  }

  ngOnInit() {
    this.selectedCourse1 = this.courses.find(x => x.name == this.student?.course1);
    this.selectedCourse2 = this.courses.find(x => x.name == this.student?.course2);

    this.selectedGrade1 = this.grades.find(x => x == this.student?.course1Grade);
    this.selectedGrade2 = this.grades.find(x => x == this.student?.course2Grade);
  }

  passBack() {
    this.student.course1 = this.selectedCourse1?.name;
    this.student.course1Credits = this.selectedCourse1?.credits;
    this.student.course1Grade = this.selectedGrade1;

    this.student.course2 = this.selectedCourse2?.name;
    this.student.course2Credits = this.selectedCourse2?.credits;
    this.student.course2Grade = this.selectedGrade2;

    this.passEntry.emit(this.student);
    this.activeModal.close(this.student);
  }

  setupCourse1(course: CourseDto) {
    this.selectedCourse1 = course;
    if(this.selectedCourse1.name == '')
    {
      this.selectedGrade1 = null;
    }
  }

  setupCourse2(course: CourseDto) {
    this.selectedCourse2 = course;
    if(this.selectedCourse2.name == '')
    {
      this.selectedGrade1 = null;
    }
  }
}
