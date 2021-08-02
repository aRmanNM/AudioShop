import {Component, ElementRef, Inject, Input, OnInit, ViewChild} from '@angular/core';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {Course} from '../../../models/course';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog';
import {CoursesAndEpisodesService} from '../../../services/courses-and-episodes.service';
import {MatSnackBar} from '@angular/material/snack-bar';
import {environment} from '../../../../environments/environment';
import {SpinnerService} from '../../../services/spinner.service';
import {CategoryService} from '../../../services/category.service';
import {Category} from '../../../models/category';
import {COMMA, ENTER} from '@angular/cdk/keycodes';
import {Observable} from 'rxjs';
import {MatAutocomplete, MatAutocompleteSelectedEvent} from '@angular/material/autocomplete';
import {MatChipInputEvent} from '@angular/material/chips';
import {map, startWith} from 'rxjs/operators';

interface DialogData {
  course: Course;
}

@Component({
  selector: 'app-create-or-edit',
  templateUrl: './create-or-edit.component.html',
  styleUrls: ['./create-or-edit.component.scss']
})
export class CreateOrEditComponent implements OnInit {
  baseUrl = environment.apiUrl + 'Files/';
  imgUrl;
  allCategories: Category[] = [];
  categories: Category[] = [];
  separatorKeysCodes: number[] = [ENTER, COMMA];
  categoryCtrl = new FormControl();
  filteredCategories: Observable<Category[]>;

  @ViewChild('fileInput') fileInput: ElementRef;
  @ViewChild('categoryInput') categoryInput: ElementRef<HTMLInputElement>;
  @ViewChild('auto') matAutocomplete: MatAutocomplete;

  constructor(public dialogRef: MatDialogRef<CreateOrEditComponent>,
              @Inject(MAT_DIALOG_DATA) public data: DialogData,
              private coursesAndEpisodesService: CoursesAndEpisodesService,
              private snackBar: MatSnackBar,
              public spinnerService: SpinnerService,
              private categoryService: CategoryService) {
  }

  courseForm = new FormGroup(
    {
      id: new FormControl(''),
      name: new FormControl('', [Validators.required]),
      instructor: new FormControl('', [Validators.required]),
      description: new FormControl(''),
      price: new FormControl('', [Validators.required]),
      isActive: new FormControl('', [Validators.required]),
      waitingTimeBetweenEpisodes: new FormControl('', [Validators.required]),
      categories: new FormControl([]),
      isFeatured: new FormControl(''),
      courseType: new FormControl('')
    }
  );

  ngOnInit(): void {

    this.getCategories();

    if (this.data.course) {
      this.courseForm.setValue({
        id: this.data.course.id,
        name: this.data.course.name,
        instructor: this.data.course.instructor,
        description: this.data.course.description,
        price: this.data.course.price,
        isActive: this.data.course.isActive,
        waitingTimeBetweenEpisodes: this.data.course.waitingTimeBetweenEpisodes.toString(),
        categories: this.data.course.categories,
        isFeatured: this.data.course.isFeatured,
        courseType: this.data.course.courseType.toString()
      });

      this.categories = JSON.parse(JSON.stringify(this.data.course.categories));
      this.getImage();
    }
  }

  add(event: MatChipInputEvent): void {
    this.categories.push(this.allCategories.find(c => c.title === event.value));

    // // Clear the input value
    // event.categoryInput!.clear();

    this.categoryCtrl.setValue(null);
  }

  remove(category: Category): void {
    const index = this.categories.indexOf(category);

    if (index >= 0) {
      this.categories.splice(index, 1);
    }
  }

  selected(event: MatAutocompleteSelectedEvent): void {
    this.categories.push(event.option.value);
    this.categoryInput.nativeElement.value = '';
    this.categoryCtrl.setValue(null);
  }

  private _filter(value: Category): Category[] {

    return this.allCategories.filter(c => this.allCategories.indexOf(c) === 0);
  }

  getCategories(): void {
    this.categoryService.getCategories().subscribe((res) => {
      this.allCategories = res;

      this.filteredCategories = this.categoryCtrl.valueChanges.pipe(
        startWith(null),
        map((category: Category | null) => category ? this._filter(category) : this.allCategories.slice()));
    });
  }

  createOrEditCourse(): void {
    if (this.data.course) {
      const course: Course = this.courseForm.value;
      course.categories = this.categories;
      this.coursesAndEpisodesService.updateCourse(course).subscribe((res) => {
        this.snackBar.open('ویرایش دوره موفقیت آمیز بود', null, {
          duration: 2000,
        });
        this.coursesAndEpisodesService.onUpdate();
        this.closeDialog();
      });
    } else {
      const course: Course = this.courseForm.value;
      course.categories = this.categories;
      this.coursesAndEpisodesService.createCourse(course).subscribe((res) => {
        this.snackBar.open('دوره جدید با موفقیت ایجاد شد', null, {
          duration: 2000,
        });
        this.coursesAndEpisodesService.onUpdate();
        this.closeDialog();
      });
    }
  }

  closeDialog(): void {
    this.dialogRef.close();
    // this.coursesAndEpisodesService.onReviewsUpdate();
  }

  uploadPhoto(): any {
    console.log('upload function called');
    const nativeElement = this.fileInput.nativeElement;
    this.coursesAndEpisodesService.uploadPhoto(this.data.course.id, nativeElement.files[0]).subscribe((res) => {
      console.log('upload subscribe answered!');
      this.data.course.photoFileName = res.fileName;
      this.getImage();
      this.coursesAndEpisodesService.onUpdate();
    }, ((e) => {
      this.snackBar.open(e.error, null, {
        duration: 2000,
      });
    }));
  }

  getImage(): void {
    this.imgUrl = this.baseUrl + this.data.course.id + '/' + this.data.course.photoFileName;
  }


}
