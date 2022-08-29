import {Component, Inject} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {NgbModal} from "@ng-bootstrap/ng-bootstrap";
import {StudentDialogComponent} from "../student-dialog/student-dialog.component";
import {StudentDto} from "../../models/student";
import {CourseDto} from "../../models/course";
import {EnrollDialogComponent} from "../enroll-dialog/enroll-dialog.component";
import {Grades} from "../../models/grade";
import {EnrollmentDto} from "../../models/enrollment";
import {TransferDialogComponent} from "../transfer-dialog/transfer-dialog.component";
import {TransferDto} from "../../models/transfer";
import {DisenrollDialogComponent} from "../disenroll-dialog/disenroll-dialog.component";
import {DisenrollmentDto} from "../../models/disenrollment";
import {PersonalInfoDialogComponent} from "../personal-info-dialog/personal-info-dialog.component";
import {PersonalInfoDto} from "../../models/personal-info";

@Component({
    selector: 'app-student-table',
    templateUrl: './student-table.component.html',
    styles: [
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

    public HighlightRow: number;

    constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private modalService: NgbModal) {
        this.http = http;
        this.baseUrl = baseUrl;
        
        this.getStudents();
        this.getCourses();

    }
    
    getStudents(){
        this.http.get<any[]>(this.baseUrl + 'student').subscribe(result => {
            this.students = result;
        }, error => console.error(error));
    }
    
    getCourses(){
        this.http.get<any[]>(this.baseUrl + 'course').subscribe(result => {
            result.forEach(x => {
                let course: CourseDto = {
                    name: x.name,
                    credits: x.credits
                };
                this.courses.push(course);
            })
        }, error => console.error(error));
    }

    openAddStudentModal() {
        const modalRef = this.modalService.open(StudentDialogComponent);

        modalRef.componentInstance.courses = this.courses;
        modalRef.componentInstance.update = false;

        modalRef.componentInstance.passEntry.subscribe((studentDto: StudentDto) => {
            this.http.post(this.baseUrl + 'student', studentDto).subscribe((result: boolean) => {
                if(result){
                    this.getStudents();
                }
            }, error => console.error(error));
        })
    }

    updatePersonalInfo() {
        const modalRef = this.modalService.open(PersonalInfoDialogComponent);

        if (this.selectedStudent != undefined) {
            modalRef.componentInstance.ssn = this.selectedStudent.ssn;
            modalRef.componentInstance.personalInfo = {
                name: this.selectedStudent.name,
                email: this.selectedStudent.email
            }
        }
        modalRef.componentInstance.passEntry.subscribe((personalInfoDto: PersonalInfoDto) => {
            this.http.put(this.baseUrl + 'student/' + this.selectedStudent.ssn, personalInfoDto).subscribe(result => {
                if(result){
                    this.getStudents();
                }
            }, error => console.error(error));
        })
    }

    unregisterStudent() {
        this.http.delete(this.baseUrl + 'student/' + this.selectedStudent.ssn).subscribe(result => {
            if(result){
                this.getStudents();
            }
        }, error => console.error(error));
        this.students.splice(this.HighlightRow, 1);
    }

    rowSelected(student: StudentDto, index: any) {
        this.selectedStudent = student;
        this.HighlightRow = index;
    }

    transferModal(course: string, grade: string, enrollmentIndex: number) {
        const modalRef = this.modalService.open(TransferDialogComponent);

        modalRef.componentInstance.courses = this.courses;
        modalRef.componentInstance.selectedCourse = course;
        modalRef.componentInstance.selectedGrade = grade;

        modalRef.componentInstance.passEntry.subscribe((transferDto:TransferDto) => {
            this.http.put(this.baseUrl + 'student/' +this.selectedStudent.ssn + '/enrollments/' + enrollmentIndex, transferDto).subscribe((result:boolean) => {
                if(result)
                {
                    this.getStudents();
                }
            }, error => console.error(error));
        })
    }

    disenrollModal(enrollmentIndex: number) {
        const modalRef = this.modalService.open(DisenrollDialogComponent);

        modalRef.componentInstance.courses = this.courses;

        modalRef.componentInstance.passEntry.subscribe((disenrollmentDto:DisenrollmentDto) => {
            this.http.post(this.baseUrl + 'student/' +this.selectedStudent.ssn + '/enrollments/' + enrollmentIndex + '/disenroll', disenrollmentDto).subscribe((result:boolean) => {
                if(result)
                {
                    this.getStudents();
                }
            }, error => console.error(error));
        })
    }

    enrollModal(course: string, grade: string) {
        const modalRef = this.modalService.open(EnrollDialogComponent);

        modalRef.componentInstance.courses = this.courses;
        modalRef.componentInstance.actualCourse = course;
        modalRef.componentInstance.actualGrade = grade;

        modalRef.componentInstance.passEntry.subscribe((enrollmentDto:EnrollmentDto) => {
            this.http.post(this.baseUrl + 'student/' + this.selectedStudent.ssn + '/enrollments', enrollmentDto).subscribe((result:boolean) => {
                if(result)
                {
                    this.getStudents();
                }
            }, error => console.error(error));
        })
    }
}
