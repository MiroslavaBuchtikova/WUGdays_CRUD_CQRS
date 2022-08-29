import {Component, Inject} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { NgbModal} from "@ng-bootstrap/ng-bootstrap";
import {StudentDialogComponent} from "../student-dialog/student-dialog.component";
import {StudentDto} from "../models/student";
import {CourseDto} from "../models/course";

@Component({
  selector: 'app-student-table',
  templateUrl: './student-table.component.html',
  styles:[
    '.table tr.active td {  \n' +
    '    background-color:#404040 !important;  \n' +
    '    color: white;  \n' +
    '  } '
  ]
})
export class StudentTableComponent {
  public http: HttpClient;
  public baseUrl: string;
  public students: StudentDto[] = [];
  public courses: CourseDto[] = [{
    name: '',
    credits: null
  }];
  public selectedStudent: StudentDto | undefined;

  public HighlightRow : number;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private modalService: NgbModal) {
    this.http = http;
    this.baseUrl = baseUrl;

    http.get<any[]>(baseUrl + 'student').subscribe(result => {
     this.students = result;
    }, error => console.error(error));
    http.get<any[]>(baseUrl + 'course').subscribe(result => {
      result.forEach(x=>{
        let course: CourseDto={
          name: x.name,
          credits: x.credits
        };
        this.courses.push(course);
      })
    }, error => console.error(error));
  }

  openAddStudentModal(){
    const modalRef = this.modalService.open(StudentDialogComponent);
    modalRef.componentInstance.courses = this.courses;
    modalRef.componentInstance.update = false;
    modalRef.componentInstance.passEntry.subscribe((studentDto:StudentDto) => {
      this.http.post(this.baseUrl + 'student', studentDto).subscribe((result:StudentDto) => {
        this.students.push(result);
      }, error => console.error(error));
    })
  }

  openUpdateStudentModal(){
    const modalRef = this.modalService.open(StudentDialogComponent);
    modalRef.componentInstance.courses = this.courses;
    modalRef.componentInstance.update = true;
    if(this.selectedStudent != undefined){
      modalRef.componentInstance.student = this.selectedStudent;
    }
    modalRef.componentInstance.passEntry.subscribe((studentDto:StudentDto) => {
      this.http.put(this.baseUrl + 'student/' + studentDto.ssn, studentDto).subscribe(result => {
      }, error => console.error(error));
    })
  }
  deleteStudent() {
    this.http.delete(this.baseUrl + 'student/' + this.selectedStudent.ssn).subscribe(result => {
    }, error => console.error(error));
    this.students.splice(this.HighlightRow,1);
  }

  rowSelected(student: StudentDto, index: any) {
    this.selectedStudent = student;
    this.HighlightRow = index;
  }
}
