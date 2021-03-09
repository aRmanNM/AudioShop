import {Component, OnInit, ViewChild} from '@angular/core';
import {SpinnerService} from '../../services/spinner.service';
import {MatDialog} from '@angular/material/dialog';
import {AdminService} from '../../services/admin.service';
import {Salesperson} from '../../models/salesperson';
import {MatPaginator} from '@angular/material/paginator';
import {SalespersonEditComponent} from './salesperson-edit/salesperson-edit.component';

@Component({
  selector: 'app-salespersons',
  templateUrl: './salespersons.component.html',
  styleUrls: ['./salespersons.component.scss']
})
export class SalespersonsComponent implements OnInit {
  searchString = '';
  dialogActive = false;
  salespersons: Salesperson[];
  columnsToDisplay = ['userName', 'name', 'percentage', 'currentSale', 'totalSale', 'accepted', 'actions'];
  totalItems: number;
  pageSize = 10;
  pageIndex = 0;
  onlyShowUsersWithUnacceptedCred = false;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(public spinnerService: SpinnerService,
              private dialog: MatDialog,
              private adminService: AdminService) {
  }

  ngOnInit(): void {
    this.adminService.salespersonsUpdateEmmiter.subscribe(() => {
      this.getSalespersons();
    });

    this.adminService.onSalespersonsUpdate();
  }

  toggleAccepted(): void {
    this.onlyShowUsersWithUnacceptedCred = !this.onlyShowUsersWithUnacceptedCred;
    this.adminService.onSalespersonsUpdate();
  }

  openAddOrEditDialog(salesperson: Salesperson): void {
    this.dialogActive = true;
    const dialogRef = this.dialog.open(SalespersonEditComponent, {
      width: '550px',
      data: {
        salespersonId: salesperson.id
      }
    });

    dialogRef.afterClosed().subscribe(res => {
      this.dialogActive = false;
    });
  }

  search(): void {
    this.adminService.onSalespersonsUpdate();
  }

  clearSearch(): void {
    this.searchString = '';
    this.adminService.onSalespersonsUpdate();
  }

  getSalespersons(): void {
    this.adminService.getAllSalespersons(this.searchString,
      this.onlyShowUsersWithUnacceptedCred, this.pageIndex, this.pageSize).subscribe((res) => {

      this.salespersons = res.items;
      this.totalItems = res.totalItems;
    });
  }

  changePage(): void {
    this.pageIndex = this.paginator.pageIndex;
    this.pageSize = this.paginator.pageSize;
    this.adminService.onSalespersonsUpdate();
  }

  refresh(): void {
    this.adminService.onSalespersonsUpdate();
  }
}
