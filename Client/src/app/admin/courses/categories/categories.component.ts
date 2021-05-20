import {Component, OnInit} from '@angular/core';
import {Category} from '../../../models/category';
import {CategoryService} from '../../../services/category.service';
import {SpinnerService} from '../../../services/spinner.service';
import {ActivatedRoute} from '@angular/router';
import {MatSnackBar} from '@angular/material/snack-bar';

@Component({
  selector: 'app-categories',
  templateUrl: './categories.component.html',
  styleUrls: ['./categories.component.scss']
})
export class CategoriesComponent implements OnInit {
  mode = 'add';
  categoryToEdit: Category;
  categories: Category[];
  newCategoryTitle: string;
  displayedColumns = ['id', 'title', 'actions'];

  constructor(private categoryService: CategoryService,
              public spinnerService: SpinnerService,
              private snackBar: MatSnackBar) {
  }

  ngOnInit(): void {
    this.getCategories();
  }

  getCategories(): void {
    this.categoryService.getCategories().subscribe((res) => {
      this.categories = res;
    });
  }

  createOrEditCategory(): void {
    if (this.mode === 'add') {
      const newCategory: Category = {
        id: 0,
        title: this.newCategoryTitle
      };

      this.categoryService.createCategory(newCategory).subscribe((res) => {
        this.newCategoryTitle = '';
        this.getCategories();
        this.snackBar.open('دسته بندی اضافه شد', null, {
          duration: 1000,
        });
      });
    } else if (this.mode === 'edit') {
      this.categoryToEdit.title = this.newCategoryTitle;
      this.categoryService.updateCategory(this.categoryToEdit).subscribe((res) => {
        this.categoryToEdit = null;
        this.newCategoryTitle = '';
        this.mode = 'add';
        this.getCategories();
        this.snackBar.open('دسته بندی با موفقیت بروزرسانی شد', null, {
          duration: 1000,
        });
      });
    }
  }

  changeMode(category: Category): void {
    this.mode = 'edit';
    this.newCategoryTitle = category.title;
    this.categoryToEdit = category;
  }

  deleteCategory(categoryId: number): void {
    const result = confirm('آیا از انجام این عملیات اطمینان دارید؟');
    if (result) {
      this.categoryService.deleteCategory(categoryId).subscribe((res) => {
        this.getCategories();
        this.snackBar.open('دسته بندی با موفقیت حذف شد', null, {
          duration: 1000,
        });
      }, e => {
        this.snackBar.open('حذف دسته بندی شکست خورد', null, {
          duration: 2000,
        });
      });
    }
  }
}
